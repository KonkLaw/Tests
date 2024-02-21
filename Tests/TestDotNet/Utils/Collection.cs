using System.Collections;

namespace TestDotNet.Utils;

public class Collection<T> : IEnumerable<T>
{
    private readonly Enumerator enumerator;

    public Collection(IEnumerable<T> enumerable)
    {
        enumerator = new Enumerator(enumerable.GetEnumerator());
    }

    public T GetCurrentEnumerated() => enumerator.Current;

    public IEnumerator<T> GetEnumerator() => enumerator;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


    public class Enumerator : IEnumerator<T>
    {
        private readonly IEnumerator<T> enumerator;

        public Enumerator(IEnumerator<T> enumerator) => this.enumerator = enumerator;

        public bool MoveNext() => enumerator.MoveNext();

        public void Reset() => enumerator.Reset();

        public T Current => enumerator.Current;

        object? IEnumerator.Current => Current;

        public void Dispose() => enumerator.Dispose();
    }
}