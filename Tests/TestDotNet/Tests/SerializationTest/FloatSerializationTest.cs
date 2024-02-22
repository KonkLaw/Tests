using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using TestDotNet.Utils;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace TestDotNet.Tests.SerializationTest;

public class FloatSerializationTest : BaseTest<int>
{
    class FloatSerialize : ComputeInfo<int>
    {
        private readonly Func<float[], MemoryStream> func;

        public FloatSerialize(Func<float[], MemoryStream> func) : base(GetName(func))
        {
            this.func = func;
        }

        public override Action GetAction(int count)
        {
            var array = RandomHelper.GetFloatNumbers(count);
            return () => func(array);
        }
    }

    class FloatDeserialize : ComputeInfo<int>
    {
        private readonly Func<float[], MemoryStream> getStream;
        private readonly Func<MemoryStream, float[]> readStream;

        public FloatDeserialize(Func<float[], MemoryStream> getStream, Func<MemoryStream, float[]> readStream)
            : base(GetName(readStream))
        {
            this.getStream = getStream;
            this.readStream = readStream;
        }

        public override Action GetAction(int count)
        {
            float[] array = RandomHelper.GetFloatNumbers(count);
            MemoryStream memoryStream = getStream(array);

            return () =>
            {
                memoryStream.Position = 0;
                float[] res = readStream(memoryStream);
            };
        }
    }

    [Params(100, 1_000, 10_000)]
    public int DataSize { get; set; }

    protected override int GetParam() => DataSize;

    protected override IEnumerable<ComputeInfo<int>> GetComputeInfo()
    {
        var list = new List<ComputeInfo<int>>
        {
            new FloatSerialize(SaveJsonNewtonSoft),
            new FloatSerialize(SaveBinary),
            new FloatSerialize(SaveNativeConvert),
            new FloatDeserialize(SaveJsonNewtonSoft, ReadJsonNewtonSoft),
            new FloatDeserialize(SaveBinary, ReadBinary),
            new FloatDeserialize(SaveNativeConvert, ReadNativeConvert)
        };
        return list;
    }

    private MemoryStream SaveJsonNewtonSoft(float[] array)
    {
        var memoryStream = new MemoryStream();
        JsonSerializer serializer = new JsonSerializer();
        StreamWriter writer = new StreamWriter(memoryStream);
        serializer.Serialize(writer, array);
        writer.Flush();
        return memoryStream;
    }

    private MemoryStream SaveBinary(float[] array)
    {
        //var stream = new MemoryStream();
        //var formatter = new BinaryFormatter();
        //formatter.Serialize(stream, array);
        //return stream;
        throw new NotImplementedException();
    }

    private unsafe MemoryStream SaveNativeConvert(float[] array)
    {
        var stream = new MemoryStream();

        fixed (float* ptr = array)
        {
            var span = new ReadOnlySpan<byte>(ptr, array.Length * sizeof(float));
            stream.Write(span);
        }
        return stream;
    }

    private float[] ReadJsonNewtonSoft(MemoryStream memoryStream)
    {
        JsonSerializer serializer = new JsonSerializer();
        var sr = new StreamReader(memoryStream);
        var jsonTextReader = new JsonTextReader(sr);
        {
            return serializer.Deserialize<float[]>(jsonTextReader);
        }
    }

    private float[] ReadBinary(MemoryStream memoryStream)
    {
        //var formatter = new BinaryFormatter();
        //return (float[])formatter.Deserialize(memoryStream);
        throw new NotImplementedException();
    }

    private unsafe float[] ReadNativeConvert(MemoryStream memoryStream)
    {
        float[] array = new float[memoryStream.Length / 4];

        fixed (float* ptr = array)
        {
            var span = new Span<byte>(ptr, array.Length * sizeof(float));
            memoryStream.Read(span);
        }
        return array;
    }
}