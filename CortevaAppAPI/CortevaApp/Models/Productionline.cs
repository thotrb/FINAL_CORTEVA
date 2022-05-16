using Microsoft.AspNetCore.Http;
using System;
namespace CortevaApp.Models
{
    public class Productionline
    {        

        public int id { get; set; }
        public string productionline_name { get; set; }

        public string worksite_name { get; set; }

        public byte[] ImgByte { get; set; }

        public IFormFile Files { get; set; } = null;


    }
}
