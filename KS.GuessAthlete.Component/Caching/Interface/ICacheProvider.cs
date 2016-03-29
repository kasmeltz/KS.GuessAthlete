using System;

namespace KS.GuessAthlete.Component.Caching.Interface
{
    /// <summary>
    /// Represents an item that provides a cache for rapid data access.
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// Adds an item to the cache for an explicit length of time.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="seconds"></param>
        void Insert(string key, object item, int seconds);

        /// <summary>
        /// Returns the object from the cache inserted with the provided key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The object from the cache inserted with the provided key.</returns>
        object Get(string key);

        /// <summary>
        /// Removes the object from the cache inserted with the provided key
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
    }
}
