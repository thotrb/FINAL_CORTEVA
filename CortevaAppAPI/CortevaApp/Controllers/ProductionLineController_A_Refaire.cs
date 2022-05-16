/**using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web;
using CortevaApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net;

namespace CortevaApp.Controllers
{
    [System.Web.Http.Route("api")]
    [ApiController]
    [System.Web.Http.Authorize]
    public class ProductionLineController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public static IWebHostEnvironment _oWebHostEnvironment;
        public ProductionLineController(IConfiguration configuration, IWebHostEnvironment oWebHostEnvironment)

        {
            _configuration = configuration;
            _oWebHostEnvironment = oWebHostEnvironment;
        }


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("productionlineID/{productionLine}")]
        public JsonResult GetProductionLineID(string productionLine)
        {
            string QueryProductionLineID= @"select id
                                           from dbo.ole_productionline
                                           where productionline_name = @productionLine";

            DataTable ProductionLineID = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QueryProductionLineID, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    reader = command.ExecuteReader();
                    ProductionLineID.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(ProductionLineID);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("getProductionLines/{worksite}")]

        public JsonResult GetProductionLineAdministrator(string worksite)
        {
            string QueryProductionLineID = @"select *
                                           from dbo.ole_productionline
                                           where worksite_name = @worksite";

            DataTable ProductionLineID = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QueryProductionLineID, connection))
                {
                    command.Parameters.AddWithValue("@worksite", worksite);
                    reader = command.ExecuteReader();
                    ProductionLineID.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(ProductionLineID);
        }

     

        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("deleteProductionLine/{id}")]

        public JsonResult DeleteMachine(int id)
        {
            string queryDelete = @"delete from dbo.ole_productionline 
                                          where id = @id";

            DataTable result = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryDelete, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    reader = command.ExecuteReader();
                    result.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(result);

        }


        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("insertProductionline")]

        public JsonResult CreateNewMachine(Productionline productionline)
        {
            string QueryNewPO = @"insert into dbo.ole_productionline (productionline_name, worksite_name)
                                  values (@productionline_name, @worksite_name)";


            DataTable NewMachine = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QueryNewPO, connection))
                {
                    command.Parameters.AddWithValue("@productionline_name", productionline.productionline_name);
                    command.Parameters.AddWithValue("@worksite_name", productionline.worksite_name);
          

                    reader = command.ExecuteReader();
                    NewMachine.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(NewMachine);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("getPicProductionLine/{productionLineId}")]

        public Byte[] getAllProductionlinePic(int productionLineId)
        {
            string fileName = "StudentPic_" + productionLineId + ".png";
            var path = Path.Combine(_oWebHostEnvironment.WebRootPath, "ProductionlinePics", fileName);
            return System.IO.File.ReadAllBytes(path);
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("insertProductionlineImage")]

        public async Task<AcceptedAtActionResult> CreateNewProductionLine([FromForm] Productionline productionline)
        {
            string QueryNewPO = @"insert into dbo.ole_productionline (productionline_name, worksite_name)
                                  values (@productionline_name, @worksite_name)";


            try
            {

                string message = "";
                var files = productionline.Files;
                productionline.Files = null;
                if (productionline.id > 0 && files != null && files.Length > 0)
                {
                    string path = _oWebHostEnvironment.WebRootPath + "\\ProductionlinePucs\\";
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    string fileName = "Pic_" + productionline.id + ".png";
                    if (System.IO.File.Exists(path + fileName))
                    {
                        System.IO.File.Delete(path + fileName);

                    }
                    using (FileStream fileStream = System.IO.File.Create(path + fileName))
                    {
                        files.CopyTo(fileStream);
                        fileStream.Flush();
                        message = "Success";
                    }
                }
                else if (productionline.id == 0)
                {
                    message = "Failed";
                }
                else
                {
                    message = "Success";
                }

                if (message == "Success") return (AcceptedAtActionResult)StatusCode((int)HttpStatusCode.InternalServerError, message);
                else return (AcceptedAtActionResult)StatusCode((int)HttpStatusCode.InternalServerError, message);

            }
            catch(Exception ex)
            {
                return (AcceptedAtActionResult)StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
**/