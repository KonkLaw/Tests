using BenchmarkDotNet.Attributes;
using System.Data.SQLite;
using Microsoft.EntityFrameworkCore;

namespace TestDotNet.Tests.Database;

public class ArraySave
{
    private readonly string connectionString;

    public ArraySave()
    {
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.db");
        connectionString = $"Data Source={path}";

        if (File.Exists(path))
        {
            File.Delete(path);
        }
        SQLiteConnection.CreateFile(path);
    }

    [GlobalSetup]
    public unsafe void Setup()
    {
        using (var context = new TestDbContext(connectionString))
        {
            context.Database.EnsureCreated();

            context.Records.ExecuteDelete();
            context.ByteRecords.ExecuteDelete();
            context.FloatRecords.ExecuteDelete();

            const int count = 100000;
            float[] array = Enumerable.Range(0, count).Select(i => (float)i).ToArray();

            FloatRecord[] recordsArray = Enumerable.Range(0, 1000).Select(i => new FloatRecord
            {
                Value = array[i]
            }).ToArray();
            context.Records.AddRange(recordsArray);
            context.SaveChanges();
            
            var floatArrayRecord = new FloatArrayRecord
            {
                Array = array
            };
            context.FloatRecords.Add(floatArrayRecord);
            context.SaveChanges();

            byte[] byteArray;
            fixed (float* floatPtr = array)
            {
                byteArray = new byte[array.Length * sizeof(float)];

                Span<byte> sourceArray = new Span<byte>(floatPtr, byteArray.Length);
                Span<byte> destArray = new Span<byte>(byteArray);
                sourceArray.CopyTo(destArray);
            }
            var byteArrayRecord = new ByteArrayRecord
            {
                Array = byteArray
            };
            context.ByteRecords.Add(byteArrayRecord);
            context.SaveChanges();
        }
    }

    [Benchmark]
    public unsafe float[] ReadArrayByBytes()
    {
        using (var context = new TestDbContext(connectionString))
        {
            ByteArrayRecord arrayRecord = context.ByteRecords.FirstOrDefault()!;
            byte[] bytes = arrayRecord.Array!;
            float[] floats = new float[bytes.Length / 4];

            fixed (float* floatPtr = floats)
            {
                var destSpan = new Span<byte>(floatPtr, bytes.Length);
                new Span<byte>(bytes).CopyTo(destSpan);
            }
            return floats;
        }
    }

    [Benchmark]
    public float[] ReadValuesByOne()
    {
        using (var context = new TestDbContext(connectionString))
        {
            float[] values = context.Records.Select(u => u.Value).ToArray();
            return values;
        }
    }

    [Benchmark]
    public float[] ReadArrayByFloats()
    {
        using (var context = new TestDbContext(connectionString))
        {
            FloatArrayRecord arrayRecord = context.FloatRecords.FirstOrDefault()!;
            return arrayRecord.Array!;
        }
    }
}




public class TestDbContext : DbContext
{
    public DbSet<FloatRecord> Records { get; set; } = null!;

    public DbSet<FloatArrayRecord> FloatRecords { get; set; } = null!;

    public DbSet<ByteArrayRecord> ByteRecords { get; set; } = null!;

    public TestDbContext(string connectionString) : base(GetOptions(connectionString)) { }

    private static DbContextOptions<TestDbContext> GetOptions(string connectionString)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
        optionsBuilder.UseSqlite(connectionString);
        return optionsBuilder.Options;
    }
}

public class FloatRecord
{
    public int Id { get; set; }
    public float Value { get; set; }
}

public class FloatArrayRecord
{
    public int Id { get; set; }
    public float[]? Array { get; set; }
}


public class ByteArrayRecord
{
    public int Id { get; set; }
    public byte[]? Array { get; set; }
}