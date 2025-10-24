using Sandbox.Definitions;
using System;
using BigGustave;
using System.Collections.Generic;

namespace OreDetectorReforged
{
    class PermaCache<T, TResult>
    {
        readonly Dictionary<T, TResult> cache = new Dictionary<T, TResult>();
        readonly Func<T, TResult> loader;
        public PermaCache(Func<T, TResult> loader)
        {
            this.loader = loader;
        }
        public TResult Get(T t)
        {
            TResult result;
            if (!cache.TryGetValue(t, out result))
                result = cache[t] = loader(t);
            return result;
        }
    }
    class PermaCache<T1, T2, TResult>
    {
        readonly Dictionary<KeyValuePair<T1,T2>, TResult> cache = new Dictionary<KeyValuePair<T1, T2>, TResult>();
        readonly Func<T1,T2,TResult> loader;
        public PermaCache(Func<T1, T2, TResult> loader)
        {
            this.loader = loader;
        }
        public TResult Get(T1 t1, T2 t2)
        {
            TResult result;
            var t = new KeyValuePair<T1,T2>(t1, t2);
            if (!cache.TryGetValue(t, out result))
                result = cache[t] = loader(t1, t2);
            return result;
        }
    }
}
