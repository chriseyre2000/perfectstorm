using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PerfectStorm.Azure
{
    public class AzureLogQuery
    {
        private CloudStorageAccount _storageAccount;

        public AzureLogQuery(CloudStorageAccount storageAccount)
        {
            _storageAccount = storageAccount;
        }

        public IEnumerable<DynamicTableEntity> Query(string filter)
        {
            IEnumerable<DynamicTableEntity> result = null;

            CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();
            CloudTable cloudTable = tableClient.GetTableReference("WADLogsTable");
            TableQuery query = new TableQuery();
            query.FilterString = filter;
            result = cloudTable.ExecuteQuery(query).ToList();
            return result;
        }

        /// <summary>
        /// This is the most efficient that I can make the query since it is using ranges on the PartitionKey
        /// </summary>
        /// <returns></returns>
        public string LastHourFilter()
        {
            DateTime keepThreshold = DateTime.UtcNow.AddHours(-1);
            string filter = string.Format("PartitionKey gt '0{0}'", keepThreshold.Ticks);
            return filter;
        }

        /// <summary>
        /// This is the most efficient that I can make the query since it is using ranges on the PartitionKey
        /// </summary>
        /// <returns></returns>
        public string BetweenTime(DateTime utcStartOfRange, DateTime utcEndOfRange)
        {
            string filter = string.Format("PartitionKey ge '0{0}' and PartitionKey le '0{1}'", utcStartOfRange.Ticks, utcEndOfRange.Ticks);
            return filter;
        }

    }
}
