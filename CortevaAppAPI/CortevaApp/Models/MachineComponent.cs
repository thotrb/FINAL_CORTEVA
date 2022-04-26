using System;
namespace CortevaApp.Models
{
    public class MachineComponent
    {
        public int id { get; set; }
        public string name { get; set; }
        public string machineName { get; set; }
        public int other_machine { get; set; }
        public string worksite { get; set; }
        public string productionLine { get; set; }

    }
}
