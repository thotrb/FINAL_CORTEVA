using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CortevaApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CortevaApp.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class ProductionLineController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public ProductionLineController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet("productionlineID/{productionLine}")]
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

        [HttpGet("getProductionLines/{worksite}")]
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

        [HttpPut("insertProductionline")]
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
    }
}