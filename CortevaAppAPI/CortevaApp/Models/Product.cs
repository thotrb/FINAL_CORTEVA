using System;
namespace CortevaApp.Models
{
    public class Product
    {
        public int id { get; set; }
   
        public string product { get; set; }
        public string GMID { get; set; }
        public string bulk { get; set; }
        public string family { get; set; }
        public string GIFAP { get; set; }
        public string description { get; set; }
        public string formulationType { get; set; }
        public float size { get; set; }
        public float idealRate { get; set; }

        public int bottlesPerCase { get; set; }
    }
}
