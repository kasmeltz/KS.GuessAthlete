using KS.GuessAthlete.Component.Caching.Interface;
using System;

namespace KS.GuessAthlete.Component.Caching.Implementation
{
    /// <summary>
    /// Represents an item that doesn't actually cache anything. 
    /// </summary>
    public class NoOpCacheProvider : ICacheProvider
    {
        public object Get(string key)
        {
            return null;
        }

        public void Insert(string key, object item, int seconds)
        {
        }

        public void Remove(string key)
        {
        }
    }
}
