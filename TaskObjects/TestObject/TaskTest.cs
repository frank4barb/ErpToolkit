using System.Diagnostics;

namespace TestObject
{

    public class TaskTest
    {
        public void init(string parameters = "0")
        {
            Trace.TraceInformation("MccDaq.MccBoard instrument open");
        }
        public int exec(string startTime = "", string endTime = "")
        {
            Trace.TraceInformation("MccDaq.MccBoard instrument self test not available");
            return 0;
        }
        public void term(string parameters = "0")
        {
            Trace.TraceInformation("MccDaq.MccBoard instrument close");
        }
    }

}
