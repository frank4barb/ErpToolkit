using System;

namespace ErpToolkit.Helpers
{
    [Serializable]
    public class ErpException: Exception
    {
        public ErpException() { }

        public ErpException(string exception): base(exception) { }
    }
}
