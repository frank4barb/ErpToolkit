//using Microsoft.Azure.Storage;
//using Microsoft.Azure.Storage.Blob;
using System;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Diagnostics.Metrics;
using System.Reflection;
using ErpToolkit.Scheduler.Interfaces;
using ErpToolkit.Helpers;

namespace ErpToolkit.Scheduler
{
    public class ExecService : IExecService
    {
        private static NLog.ILogger _logger;

        //.....
        public dynamic instrument;
        Assembly dll;
        //.....

        public ExecService()
        {
            //SetUpNLog();
            NLog.LogManager.Configuration = UtilHelper.GetNLogConfig(); // Apply config
            _logger = NLog.LogManager.GetCurrentClassLogger();
        }


        //var folderToZip = Convert.ToString(ConfigurationManager.AppSettings["FolderToZipLocation"]);
        //var folderToZip = Convert.ToString(ConfigurationManager.AppSettings["FolderFromZipLocation"]);
        //var folderToZip = Convert.ToString(ConfigurationManager.ConnectionStrings["StorageConnectionString"]);


        public async Task PerformService(string schedule)
        {
            try
            {
                _logger.Info($"{DateTime.Now}: The PerformService() is called with {schedule} schedule");

                //......
                //......
                //dynamically load .NET core library with .NET framework dependencies
                //https://learn.microsoft.com/en-us/answers/questions/1181408/how-can-i-dynamically-load-net-core-library-with-n
                //C# load DLL at runtime
                //https://stackoverflow.com/questions/72700743/c-sharp-load-dll-at-runtime
                //using System;
                //using System.Reflection;
                //class Program
                //{
                //    static void Main(string[] args)
                //    {
                //        Assembly assembly = Assembly.LoadFrom("/path/to/lib.dll");
                //        Type MyClass = example.GetType("lib.MyClass");
                //
                //        MyClass.GetMethods[0].Invoke();
                //        Console.ReadLine();
                //    }
                //}


                dll = Assembly.LoadFile(Environment.CurrentDirectory + "\\TestObject.dll");
                Type taskType = null;
                foreach (Type t in dll.GetExportedTypes())
                {
                    if (t.Name.StartsWith("Task"))
                    {
                        taskType = t;
                        break;
                    }
                }
                if (taskType == null)
                    throw new Exception($"Instrument {dll.GetName().Name} must contain type \"Task\"");

                instrument = Activator.CreateInstance(taskType);
                instrument.init("0");
                int cErr = instrument.exec("startTime", "endTime");
                instrument.term();

                //......
                //......

            }
            catch (Exception ex)
            {
                _logger.Error($"{DateTime.Now}: Exception is occured at PerformService(): {ex.Message}");
                throw new ErpConfigurationException(ex.Message);
            }
        }


    }
}
