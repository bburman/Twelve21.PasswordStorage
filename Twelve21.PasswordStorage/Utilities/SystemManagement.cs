using System.Linq;
using System.Management;

namespace Twelve21.PasswordStorage.Utilities
{
    public static class SystemManagement
    {
        public static int GetTotalCpuCores()
        {
            return new ManagementObjectSearcher("SELECT * FROM Win32_Processor")
                .Get()
                .Cast<ManagementBaseObject>()
                .Select(mbo => int.Parse(mbo["NumberOfCores"].ToString()))
                .Sum();
        }
    }
}
