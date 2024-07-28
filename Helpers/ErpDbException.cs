using System;

namespace ErpToolkit.Helpers
{
    [Serializable]
    public class ErpDbException: Exception
    {
        public ErpDbException() { }

        public ErpDbException(string exception): base(exception) { }
    }
}
