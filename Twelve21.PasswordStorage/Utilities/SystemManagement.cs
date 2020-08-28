using System;

namespace Twelve21.PasswordStorage.Utilities
{
    public static class SystemManagement
    {
        public static int GetTotalCpuCores()
        {
            return Environment.ProcessorCount;
        }
    }
}
