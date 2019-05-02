using System;

namespace AlanZucconi
{
    public class Cached<T>
    {
        public Func<T> Get; // Get a new value

        private bool Valid = false;
        private T CachedValue = default(T);

        public T Value
        {
            get
            {
                if (!Valid)
                {
                    CachedValue = Get();
                    Valid = true;
                }
                return CachedValue;
            }
        }

        // Invalidates the cache
        public void Invalidate()
        {
            Valid = false;
            CachedValue = default(T);
        }

        public Cached(Func<T> get)
        {
            Get = get;
        }

        public static implicit operator T(Cached<T> x)
        {
            return x.Value;
        }
    }
}