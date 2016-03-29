using Dapper;
using KS.GuessAthlete.Component.Caching.Interface;
using KS.GuessAthlete.Data.DataAccess.Exceptions;
using KS.GuessAthlete.Data.DataAccess.Repository.Interface;
using KS.GuessAthlete.Data.POCO;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace KS.GuessAthlete.Data.DataAccess.Repository.Implementation
{
    /// <summary>
    /// A class that represents the common functionality required
    /// for Dapper based repositories.
    /// 
    /// Dapper documentation at https://github.com/StackExchange/dapper-dot-net
    /// </summary>
    public abstract class BaseDapperRepository<T> : IDataRepository<T>
        where T : PocoDataObject
    {
        #region Protected Members

        /// <summary>
        /// The Cache Provider for this repository.
        /// </summary>
        protected ICacheProvider CacheProvider { get; set; }

        /// <summary>
        /// The DB Connection String for this repository.
        /// </summary>
        protected string DbConnectionString { get; set; }

        /// <summary>
        /// Required for bulk import
        /// </summary>
        protected string StagingTableName { get; set; }

        /// <summary>
        /// The table name for the repository
        /// </summary>
        protected string TableName { get; set; }

        /// <summary>
        /// The name of the cache container
        /// </summary>        
        protected string CacheContainerName { get; set; }

        /// <summary>
        /// A mapping from field names to table column names for a bulk insert.
        /// </summary>
        protected Dictionary<string, string> BulkColumnMapping { get; set; }
        
        /// <summary>
        /// The number of seconds to cache items
        /// </summary>
        protected int CacheSeconds { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a base dapper repository.
        /// </summary>
        public BaseDapperRepository(ICacheProvider cacheProvider)
        {
            CacheSeconds = 15; 

            DbConnectionString = ConfigurationManager
                .ConnectionStrings["HockeyGMDB"]
                .ToString();

            CacheProvider = cacheProvider;

            CreateSql();
        }

        #endregion

        #region Sql Statements

        /// <summary>
        /// Creates the Sql statements for the repository actions
        /// </summary>
        protected abstract void CreateSql();

        protected string GetSql { get; set; }
        protected string ListSql { get; set; }
        protected string SearchSql { get; set; }
        protected string InsertSql { get; set; }
        protected string UpdateSql { get; set; }
        protected string FinalizeBulkImportSQL { get; set; }

        #endregion       

        #region IDataRepository Methods

        /// <summary>
        /// Returns all of the items in the data store.
        /// </summary>
        /// <returns>All of the items in the data store.</returns>
        protected async Task<IEnumerable<K>> List<K>(string sql, object sqlParams,
            string cacheKey)
        {
            IEnumerable<K> items = null;

            if (CacheSeconds > 0)
            {
                items = (IEnumerable<K>)CacheProvider.Get(cacheKey);
            }

            if (items != null)
            {
                return items;
            }

            using (SqlConnection connection =
                    SqlConnectionManager.GetConnection(DbConnectionString))
            {
                items = await connection.QueryAsync<K>(sql, sqlParams)
                    .ConfigureAwait(false);
            }

            if (CacheSeconds > 0)
            {
                CacheProvider.Insert(cacheKey, items, CacheSeconds);
            }

            return items;
        }

        /// <summary>
        /// Returns all of the items in the data store.
        /// </summary>
        /// <returns>All of the items in the data store.</returns>
        protected async Task<IEnumerable<T>> List(string sql, object sqlParams,
            string cacheKey)
        {
            return await List<T>(sql, sqlParams, cacheKey);
        }

        /// <summary>
        /// Returns all of the items in the data store.
        /// </summary>
        /// <returns>All of the items in the data store.</returns>
        public async Task<IEnumerable<T>> List()
        {
            if (string.IsNullOrEmpty(ListSql))
            {
                throw new ActionNotSupportedException(
                    "List is not supporrted by this repository");
            }

            return await List<T>(ListSql, null, CacheContainerName);
        }


        /// <summary>
        /// Returns search terms properly formatted for use in
        /// a Dapper based paramater
        /// </summary>
        /// <param name="searchTerms"></param>
        /// <returns></returns>
        protected string GetSearchTerms(string searchTerms)
        {
            return "%"
                + searchTerms.Replace("[", "[[]").Replace("%", "[%]")
                + "%";
        }

        public async Task<IEnumerable<T>> Search(string searchTerms)
        {
            if (string.IsNullOrEmpty(SearchSql))
            {
                throw new ActionNotSupportedException(
                    "Search is not supporrted by this repository");
            }

            string cachekey = "Search" + searchTerms;

            IEnumerable<T> items = null;

            if (CacheSeconds > 0)
            {
                items = (IEnumerable<T>)CacheProvider.Get(cachekey);
            }

            if (items != null)
            {
                return items;
            }

            using (SqlConnection connection =
                    SqlConnectionManager.GetConnection(DbConnectionString))
            {
                items = await connection.QueryAsync<T>(SearchSql,
                new
                {
                    SearchTerms = GetSearchTerms(searchTerms)
                }).ConfigureAwait(false);
            }

            if (CacheSeconds > 0)
            {
                CacheProvider.Insert(cachekey, items, CacheSeconds);
            }

            return items;
        }

        /// <summary>
        /// Gets the item from the data store with the provided id,
        /// Using the provided sql.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sql"></param>
        /// <returns>The item from the data store with the provided id.</returns>
        protected async Task<T> Get(string sql, object sqlParams, string cacheKey)
        {
            T item = default(T);

            if (CacheSeconds > 0)
            {
                item = (T)CacheProvider.Get(cacheKey);
            }

            if (item != null)
            {
                return item;
            }

            using (SqlConnection connection =
                    SqlConnectionManager.GetConnection(DbConnectionString))
            {
                IEnumerable<T> items = await connection.QueryAsync<T>(sql, sqlParams)
                    .ConfigureAwait(false);

                item = items.FirstOrDefault();
            }

            if (CacheSeconds > 0)
            {
                CacheProvider.Insert(cacheKey, item, CacheSeconds);
            }

            return item;
        }


        /// <summary>
        /// Gets the item from the data store with the provided id,
        /// Using the provided sql.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sql"></param>
        /// <returns>The item from the data store with the provided id.</returns>
        protected async Task<T> Get(string sql, int id)
        {
            string cacheKey = CacheContainerName + id;

            T item = default(T);

            if (CacheSeconds > 0)
            {
                item = (T)CacheProvider.Get(cacheKey);
            }

            if (item != null)
            {
                return item;
            }

            using (SqlConnection connection =
                    SqlConnectionManager.GetConnection(DbConnectionString))
            {
                IEnumerable<T> items = await connection.QueryAsync<T>(
                    sql, new
                    {
                        Id = id
                    }).ConfigureAwait(false);

                item = items.FirstOrDefault();
            }

            if (CacheSeconds > 0)
            {
                CacheProvider.Insert(cacheKey, item, CacheSeconds);
            }

            return item;
        }

        /// <summary>
        /// Gets the item from the data store with the provided id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The item from the data store with the provided id.</returns>
        public async Task<T> Get(int id)
        {
            if (string.IsNullOrEmpty(GetSql))
            {
                throw new ActionNotSupportedException(
                    "Get is not supporrted by this repository");
            }

            return await Get(GetSql, id);
        }

        /// <summary>
        /// Inserts an item into the data store.
        /// <exception cref="ItemAlreadyExistsException">Thrown if the specified object already exists in the data store.</exception>
        /// </summary>
        /// <param name="item"></param>
        /// <returns>The item's new Id on success, an error code of less than 0 on failure.</returns>
        public async Task Insert(T item)
        {
            if (string.IsNullOrEmpty(InsertSql))
            {
                throw new ActionNotSupportedException(
                    "Insert is not supporrted by this repository");
            }

            int result = -1;

            using (SqlConnection connection =
                 SqlConnectionManager.GetConnection(DbConnectionString))
            {
                IEnumerable<int> results =
                    await connection.QueryAsync<int>(
                        InsertSql, item)
                    .ConfigureAwait(false);

                result = results.FirstOrDefault();
            }

            if (result > 0)
            {
                item.Id = result;
            }

            if (result == -1)
            {
                throw new ItemAlreadyExistsException();
            }
        }

        /// <summary>
        /// Updates an item in the data store.
        /// <exception cref="ItemAlreadyExistsException">Thrown if the specified object already exists in the data store.</exception>
        /// </summary>
        /// <param name="item"></param>
        /// <returns>The item's Id on success, an error code of less than 0 on failure.</returns>
        public async Task Update(T item)
        {
            if (string.IsNullOrEmpty(UpdateSql))
            {
                throw new ActionNotSupportedException(
                    "Update is not supporrted by this repository");
            }

            int result = -1;

            using (SqlConnection connection =
                 SqlConnectionManager.GetConnection(DbConnectionString))
            {
                IEnumerable<int> results = await connection.QueryAsync<int>(
                    UpdateSql, item)
                .ConfigureAwait(false);

                result = results.FirstOrDefault();
            }

            if (result == -1)
            {
                throw new ItemAlreadyExistsException();
            }
        }

        /// <summary>
        /// Deletes an item from the data store.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>1 on success, an error code of less than 0 on failure.</returns>
        public async Task Delete(int id)
        {
            using (SqlConnection connection =
             SqlConnectionManager.GetConnection(DbConnectionString))
            {
                await connection.ExecuteAsync(@"
                SET NOCOUNT ON;
                DELETE FROM " +
                    TableName + @"
                WHERE
                    Id = @Id",
                new
                {
                    Id = id
                }).ConfigureAwait(false);
            }
        }

        #pragma warning disable 1998
        public async Task PurgeForTest()
        {
#if DEBUG
            using (SqlConnection connection =
             SqlConnectionManager.GetConnection(DbConnectionString))
            {
                await connection.ExecuteAsync(@"
                    SET NOCOUNT ON;
                    DELETE FROM " +
                        TableName)
                .ConfigureAwait(false);
            }
#endif
        }
        #pragma warning restore 1998

        #endregion

        /// <summary>
        /// Converts a collection of items to a data table.
        /// This could be overridden in the extending classes
        /// for superior performance, or the class can just
        /// accept the default version based on Reflection.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        protected virtual DataTable ItemToDataTable<K>(IEnumerable<K> items)
        {
            DataTable dt = new DataTable();

            PropertyInfo[] props = typeof(K)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                dt.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                dt.Rows.Add(values);
            }

            return dt;
        }

        /// <summary>
        /// Imports a set of bulk records into the data store.
        /// </summary>
        public async Task BulkImport(IEnumerable<T> items)
        {
            if (string.IsNullOrEmpty(FinalizeBulkImportSQL) ||
                string.IsNullOrEmpty(StagingTableName))
            {
                throw new ActionNotSupportedException("Bulk import cannot be performed on this repository.");
            }

            using (SqlConnection connection =
                  SqlConnectionManager.GetConnection(DbConnectionString))
            {
                using (SqlTransaction transaction = connection
                    .BeginTransaction(IsolationLevel.Serializable))
                {
                    await connection.ExecuteAsync("DELETE FROM " + StagingTableName, 
                        transaction: transaction);

                    using (SqlBulkCopy sqlBulkCopy =
                        new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, transaction))
                    {
                        sqlBulkCopy.DestinationTableName = StagingTableName;

                        int i = 0;
                        int.TryParse(ConfigurationManager.AppSettings["BatchDBImportSize"], out i);
                        if (i == 0)
                        {
                            i = 1000;
                        }

                        sqlBulkCopy.BatchSize = i;

                        foreach (KeyValuePair<string, string> kvp in BulkColumnMapping)
                        {
                            sqlBulkCopy.ColumnMappings.Add(kvp.Key, kvp.Value);
                        }

                        DataTable dt = ItemToDataTable(items);
                        await sqlBulkCopy.WriteToServerAsync(dt);

                        await connection.ExecuteAsync(FinalizeBulkImportSQL, 
                            transaction: transaction)
                            .ConfigureAwait(false);

                        transaction.Commit();

                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Executes raw SQL code
        /// </summary>
        /// <param name="sql"></param>
        public async Task ExecuteRawSql(string sql)
        {
            using (SqlConnection connection =
                    SqlConnectionManager.GetConnection(DbConnectionString))
            {
                await connection.ExecuteAsync(sql);
            }
        }
    }
}
