using System.Linq;
using System.Management;

namespace Twelve21.PasswordStorage.Utilities
{
    public static class SystemManagement
    {
        public static int GetTotalCpuCores()
        {
            var cores = new ManagementObjectSearcher("SELECT * FROM Win32_Processor")
                .Get()
                .Cast<ManagementBaseObject>()
                .Select(mbo => mbo["NumberOfCores"])
                .ToList();

            return cores
                .Select(c => c.ToString())
                .Select(c => int.Parse(c))
                .Sum();
        }
    }
}
