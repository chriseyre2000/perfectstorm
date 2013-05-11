using LogViewerWebRole.Models;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LogViewerWebRole.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            Trace.TraceWarning("Index has been called...");
            ILogEntries model = LogsInTheLastHour();
            return View(model);
        }

        public ActionResult LastHour()
        {
            Trace.TraceWarning("LastHour has been called...");
            ILogEntries model = LogsInTheLastHour();
            return View("Index", model);
        }

        private static ILogEntries LogsInTheLastHour()
        {
            DateTime keepThreshold = DateTime.UtcNow.AddHours(-1);            
            string filter = string.Format("PartitionKey gt '0{0}'", keepThreshold.Ticks);
            return Logs(filter);
        }

        private static ILogEntries Logs(string filter)
        {
            string diagnostics = RoleEnvironment.GetConfigurationSettingValue("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(diagnostics);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable cloudTable = tableClient.GetTableReference("WADLogsTable");
            TableQuery query = new TableQuery();

            query.FilterString = filter;
            var items = cloudTable.ExecuteQuery(query).ToList();

            ILogEntries model = new LogEntries();

            foreach (var item in items)
            {
                model.Log.Insert(0, new LogItem
                {
                    Level = item.Properties["Level"].Int32Value ?? 0,
                    RoleInstance = item.Properties["RoleInstance"].StringValue,
                    Message = item.Properties["Message"].StringValue,
                    PartitionKey = item.PartitionKey,
                    Timestamp = item.Timestamp,
                });
            }
            return model;
        }

    }
}
