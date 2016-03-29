using KS.GuessAthlete.Component.Caching.Interface;
using System;
using System.Runtime.Caching;

namespace KS.GuessAthlete.Component.Caching.Implementation
{
    /// <summary>
    /// Represents an item that provides a data cache using
    /// System.Runtime.Caching.MemoryCache
    /// </summary>
    public class MemoryCacheProvider : ICacheProvider
    {
        private static volatile MemoryCacheProvider instance;
        private static object syncRoot = new Object();

        public static MemoryCacheProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new MemoryCacheProvider();
                    }
                }

                return instance;
            }
        }

        private MemoryCacheProvider()
        {
        }

        public object Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            return MemoryCache.Default.Get(key);
        }

        public void Insert(string key, object item, int seconds)
        {
            if (seconds == 0)
            {
                return;
            }

            DateTimeOffset offset = new DateTimeOffset(DateTime.UtcNow.AddSeconds(seconds));

            if (item == null || string.IsNullOrEmpty(key))
            {
                return;
            }

            MemoryCache.Default.Add(key, item, offset);
        }
      
        public void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            MemoryCache.Default.Remove(key);
        }
    }
}
