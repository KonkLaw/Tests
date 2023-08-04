using System.Collections;

namespace TestDotNet.Utils;

public class Collection<T> : IEnumerable<T>
{
    private readonly Enumerator enumerator;

    public Collection(IEnumerable<T> enumerable)
    {
        enumerator = new Enumerator(enumerable.GetEnumerator());
    }

    public T? GetLastEnumerated() => enumerator!.LastUsed;

    public IEnumerator<T> GetEnumerator() => enumerator;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


    public class Enumerator : IEnumerator<T>
    {
        private readonly IEnumerator<T> enumerator;

        public T? LastUsed { get; private set; }

        public Enumerator(IEnumerator<T> enumerator) => this.enumerator = enumerator;

        public bool MoveNext() => enumerator.MoveNext();

        public void Reset() => enumerator.Reset();

        public T Current
        {
            get
            {
                LastUsed = enumerator.Current;
                return enumerator.Current;
            }
        }

        object? IEnumerator.Current => Current;

        public void Dispose() => enumerator.Dispose();
    }
}