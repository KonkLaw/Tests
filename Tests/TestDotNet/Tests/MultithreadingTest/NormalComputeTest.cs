using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using TestDotNet.Utils;

namespace TestDotNet.Tests.MultithreadingTest;

public enum TestType
{
    UseCachedArray,
    UseCreatedArray,
    UseNativeArray,
    UseNativeWithModule,
}


unsafe public class NormalComputeTest
{
    private Vector3F[] positions;
    private int[] indeces;

    private int[] mutex;

    [Params(5_000, 8_000, 10_000, 15_000, 20_000)]
    //[Params(4_000_000)]
    public int VertexCount { get; set; }

    [Params(/*TestType.UseCachedArray, TestType.UseCreatedArray, TestType.UseNativeWithModule,*/ TestType.UseNativeArray)]
    public TestType Type { get; set; }

    [Params(false, true)]
    public bool UseOneThread { get; set; }

    private Func<Vector3F[]> func;

    [GlobalSetup]
    public void Setup()
    {
        int size = (int)Math.Sqrt(VertexCount);
        Vector3F[] points = new Vector3F[size * size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float x = (float)i / size;
                float y = (float)j / size;
                points[i] = new Vector3F(x, y, (float)(Math.Sin(x) * Math.Sin(x * x + y * y)));
            }
        }
        positions = points;
        indeces = IndexGridComputer.GetIndeces(size, size, 10_000);


        if (Type == TestType.UseCachedArray)
        {
            mutex = new int[positions.Length];
            func = RunArrayCached;
        }
        else if (Type == TestType.UseCreatedArray)
        {
            func = RunArrayCreation;
        }
        else if (Type == TestType.UseNativeArray)
        {
            func = RunWithPtr;
        }
        else if (Type == TestType.UseNativeWithModule)
        {
            func = RunWithPtrWithModule;
        }
    }

    //[Benchmark] public Vector3F[] PreRun() => func();

    [Benchmark] public Vector3F[] Run() => func();

    private Vector3F[] RunWithPtrWithModule()
    {
        if (UseOneThread)
        {
            return NormalsComputer.GetNormalsSmall(positions, indeces, null, UseOneThread, -1);
        }

        void* ptr = NativeMemory.Alloc((nuint)(sizeof(int) * positions.Length));
        var res = NormalsComputer.GetNormalsSmall(positions, indeces, (int*)ptr, UseOneThread, 9973);
        NativeMemory.Free(ptr);
        return res;
    }

    private Vector3F[] RunWithPtr()
    {
        if (UseOneThread)
            return NormalsComputer.GetNormalsSmall(positions, indeces, null, UseOneThread, positions.Length);

        void* ptr = NativeMemory.Alloc((nuint)(sizeof(int) * positions.Length));
        var res = NormalsComputer.GetNormalsSmall(positions, indeces, (int*)ptr, UseOneThread, positions.Length);
        NativeMemory.Free(ptr);
        return res;
    }

    private Vector3F[] RunArrayCreation()
    {
        var ownMutex = new int[positions.Length];
        return NormalsComputer.GetNormalsFull(positions, indeces, UseOneThread, ownMutex);
    }

    private Vector3F[] RunArrayCached()
    {
        return NormalsComputer.GetNormalsFull(positions, indeces, UseOneThread, mutex);
    }
}



unsafe class NormalsComputer
{
    public static Vector3F[] GetNormalsFull(Vector3F[] positions, int[] indices, bool useOneThread, int[] mutex)
    {
        var normals = new Vector3F[positions.Length];
        if (useOneThread)
        {
            ComputeSmoothNormalsOneThread(positions, indices, normals);
        }
        else
        {
            var computation = new NormalsComputationBigMutex(positions, indices, normals, mutex);
            ParallelComputation2.Run(computation, indices.Length / 3);
        }
        return normals;
    }

    private static void ComputeSmoothNormalsOneThread(Vector3F[] positions, int[] indices, Vector3F[] normals)
    {
        int triangleCount = indices.Length / 3;
        for (int i = 0; i < triangleCount; ++i)
        {
            int vertexIndex = i * 3;
            int i0 = indices[vertexIndex];
            int i1 = indices[vertexIndex + 1];
            int i2 = indices[vertexIndex + 2];
            Vector3F v0 = positions[i0];
            Vector3F v1 = positions[i1];
            Vector3F v2 = positions[i2];
            // normalize as normals from neighboring triangles should be equals. 
            Vector3F normal = Vector3F.Cross(v1 - v0, v2 - v0).GetNormalized();
            normals[i0] += normal;
            normals[i1] += normal;
            normals[i2] += normal;
        }
    }

    class NormalsComputationBigMutex : ParallelComputation2.IComputationInfo
    {
        private readonly Vector3F[] positions;
        private readonly int[] indices;
        private readonly Vector3F[] normals;
        private readonly int[] mutex;

        public NormalsComputationBigMutex(Vector3F[] positions, int[] indices, Vector3F[] normals, int[] mutex)
        {
            this.positions = positions;
            this.indices = indices;
            this.normals = normals;
            this.mutex = mutex;
        }

        public void ProcessItems(int startTriangle, int stopTriangle)
            => ComputeSmoothNormalsPrivateParallelFull(
                positions, indices, normals, mutex, startTriangle, stopTriangle);
    }

    private static void ComputeSmoothNormalsPrivateParallelFull(
        Vector3F[] positions, int[] indices, Vector3F[] output, int[] mutex, int startTriangle, int stopTriangle)
    {
        void SubmitNormal(int vertexIndex, Vector3F normal)
        {
            while (true)
            {
                if (Interlocked.CompareExchange(ref mutex[vertexIndex], 1, 0) == 0)
                {
                    output[vertexIndex] += normal;
                    mutex[vertexIndex] = 0;
                    break;
                }
            }
        }

        for (int i = startTriangle; i < stopTriangle; ++i)
        {
            int vertexIndex = i * 3;
            int i0 = indices[vertexIndex];
            int i1 = indices[vertexIndex + 1];
            int i2 = indices[vertexIndex + 2];
            Vector3F v0 = positions[i0];
            Vector3F v1 = positions[i1];
            Vector3F v2 = positions[i2];
            // normalize as normals from neighboring triangles should be equals. 
            Vector3F normal = Vector3F.Cross(v1 - v0, v2 - v0).GetNormalized();
            SubmitNormal(i0, normal);
            SubmitNormal(i1, normal);
            SubmitNormal(i2, normal);
        }
    }









    public static Vector3F[] GetNormalsSmall(Vector3F[] positions, int[] indices, int* mutex, bool useOneThread, int module)
    {
        var normals = new Vector3F[positions.Length];
        if (useOneThread)
        {
            ComputeSmoothNormalsOneThread(positions, indices, normals);
        }
        else
        {
            unsafe
            {
                var computation = new NormalsComputationSmall(positions, indices, normals, mutex, module);
                ParallelComputation2.Run(computation, indices.Length / 3);
            }
        }
        return normals;
    }


    //private const int Module = 23;
    //private const int Module = 223;
    //private const int Module = 1009;
    //private const int Module = 5011;
    //private const int Module = 9973;

    private static unsafe void ComputeNormalsSmall(
        Vector3F[] positions, int[] indices, Vector3F[] output, int* mutex, int startTriangle, int stopTriangle, int module)
    {
        
        void SubmitNormal(int vertexIndex1, Vector3F normal)
        {
            while (true)
            {
                int mutexIndex = vertexIndex1 % module;

                int old = mutex[mutexIndex];
                if (Interlocked.CompareExchange(ref mutex[mutexIndex], old + 1, old) == old)
                {
                    output[vertexIndex1] += normal;
                    mutex[mutexIndex] = 0;
                    break;
                }
            }
        }

        for (int i = startTriangle; i < stopTriangle; ++i)
        {
            int vertexIndex = i * 3;
            int i0 = indices[vertexIndex];
            int i1 = indices[vertexIndex + 1];
            int i2 = indices[vertexIndex + 2];
            Vector3F v0 = positions[i0];
            Vector3F v1 = positions[i1];
            Vector3F v2 = positions[i2];
            // normalize as normals from neighboring triangles should be equals. 
            Vector3F normal = Vector3F.Cross(v1 - v0, v2 - v0).GetNormalized();
            SubmitNormal(i0, normal);
            SubmitNormal(i1, normal);
            SubmitNormal(i2, normal);
        }
    }

    unsafe class NormalsComputationSmall : ParallelComputation2.IComputationInfo
    {
        private readonly Vector3F[] positions;
        private readonly int[] indices;
        private readonly Vector3F[] normals;
        private readonly int* mutex;
        private readonly int module;

        public NormalsComputationSmall(Vector3F[] positions, int[] indices, Vector3F[] normals, int* mutex, int module)
        {
            this.positions = positions;
            this.indices = indices;
            this.normals = normals;
            this.mutex = mutex;
            this.module = module;
        }

        public void ProcessItems(int startTriangle, int stopTriangle)
            => ComputeNormalsSmall(
                positions, indices, normals, mutex, startTriangle, stopTriangle, module);
    }
}

class ParallelComputation2
{
    private static readonly int DegreeOfParallelism;

    private readonly IComputationInfo computationInfo;
    private readonly int itemsCount;
    private readonly int maxBatchSize;

    static ParallelComputation2()
    {
        DegreeOfParallelism = Environment.ProcessorCount;
    }

    private protected ParallelComputation2(IComputationInfo computationInfo, int itemsCount, int maxBatchSize)
    {
        this.computationInfo = computationInfo;
        this.itemsCount = itemsCount;
        this.maxBatchSize = maxBatchSize;
    }

    internal static void Run(IComputationInfo computation, int itemsCount)
    {
        int maxBatchSize = GetMaxSizeInBatch(itemsCount);
        Parallel.For(0, DegreeOfParallelism, new ParallelComputation2(computation, itemsCount, maxBatchSize).Handle);
    }

    private void Handle(int threadId)
    {
        int start = threadId * maxBatchSize;
        int stop = Math.Min((threadId + 1) * maxBatchSize, itemsCount);
        computationInfo.ProcessItems(start, stop);
    }

    private static int GetMaxSizeInBatch(int itemsCount)
        => itemsCount / DegreeOfParallelism + (itemsCount % DegreeOfParallelism == 0 ? 0 : 1);

    internal interface IComputationInfo
    {
        void ProcessItems(int start, int stop);
    }
}