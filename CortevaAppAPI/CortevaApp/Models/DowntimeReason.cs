using System;
namespace CortevaApp.Models
{
    public class DowntimeReason
    {
        public int id { get; set; }
  
        public string reason { get; set; }
        public string downtimeType { get; set; }
        public string production_line { get; set; }
        public string worksite { get; set; }
    }
}
