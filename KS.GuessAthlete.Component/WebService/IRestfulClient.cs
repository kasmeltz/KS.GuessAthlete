using System.Collections.Generic;
using System.Threading.Tasks;

namespace KS.GuessAthlete.Component.WebService
{
    public interface IRestfulClient
    {
        Task<T> Get<T>(string serviceAddress, bool isAuthenticated = true);
        Task<IEnumerable<T>> GetBatch<T>(int batchSize, string serviceAddress, bool isAuthenticated = true);
        Task PostBatch<T>(int batchSize, string serviceAddress, IEnumerable<T> items, bool isAuthenticated = true);
        Task<T> Post<T>(string serviceAddress, T item, bool isAuthenticated = true);
        Task<K> Post<T, K>(string serviceAddress, T item, bool isAuthenticated = true);
        Task PutBatch<T>(int batchSize, string serviceAddress, IEnumerable<T> items, bool isAuthenticated = true);
        Task<T> Put<T>(string serviceAddress, T item, bool isAuthenticated = true);
        Task<K> Put<T, K>(string serviceAddress, T item, bool isAuthenticated = true);
        Task Delete(string serviceAddress, bool isAuthenticated = true);
        Task Authenticate(Account account);
    }
}
