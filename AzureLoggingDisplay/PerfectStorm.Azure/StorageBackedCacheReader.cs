using Microsoft.ApplicationServer.Caching;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PerfectStorm.Azure
{
    /// <summary>
    /// The purpose of this class is to provide access to a cache that is backed by table storage.
    /// 
    /// Currently I use the key as the primary and the environment.
    /// I plan to move this to a key strategy object so it can be replaced.
    /// 
    /// The next step will be to replace these objects with CloudFx's Reliable version
    /// </summary>
    public class StorageBackedCacheReader
    {
        private DataCache _cache;
        CloudStorageAccount _storageAccount;
        CloudTable _cloudTable;
        string _environmentName;
        double _cacheDuration;

        public StorageBackedCacheReader(CloudStorageAccount storageAccount, 
                                          string cacheTablename, 
                                          string enviromentName,
                                          double cacheDuration)
        {
           _cache = new DataCache();
           _storageAccount = storageAccount;
           CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();
           _cloudTable = tableClient.GetTableReference(cacheTablename);
           _cloudTable.CreateIfNotExists();
           _environmentName = enviromentName;
           _cacheDuration = cacheDuration;
        }

        public object ReadCacheValue(string key)
        {
            object result = _cache.Get(key);

            if (result == null)
            { 
                // Get from table storage and add to cache.
                TableQuery query = new TableQuery();
                query.FilterString = string.Format("PartitionKey eq '{0}' and RowKey eq '{1}'", key, _environmentName);
                var items = _cloudTable.ExecuteQuery(query);

                result = items.FirstOrDefault();

                if (result != null)
                {
                    try
                    {
                        _cache.Add(key, result, TimeSpan.FromMinutes(_cacheDuration));
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceWarning("Cache of key '{0}' in environment '{1}' failed with exception {2}", key, _environmentName, ex);
                    }
                }
            }

            return result;
        }

    }
}
