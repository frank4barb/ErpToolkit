using System;

namespace ErpToolkit.Helpers
{
    [Serializable]
    public class ErpConfigurationException: Exception
    {
        public ErpConfigurationException() { }

        public ErpConfigurationException(string exception): base(exception) { }
    }
}
