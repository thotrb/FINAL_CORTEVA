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
    public class ProductsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ProductsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("netOP/{GMID}")]
        public JsonResult GetNetOP(string GMID)
        {
            string QueryNetOP = @"select top(1) *
                                from dbo.ole_products
                                where GMID = @GMID";


            DataTable NetOP = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QueryNetOP, connection))
                {
                    command.Parameters.AddWithValue("@GMID", GMID);
                    reader = command.ExecuteReader();
                    NetOP.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(NetOP);
        }

        [HttpGet("AdministratorProducts")]
        public JsonResult GetProductsAdministrator()
        {
            string QueryNetOP = @"select *
                                from dbo.ole_products
                                order by product, formulationType, size ASC";


            DataTable NetOP = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QueryNetOP, connection))
                {
                    reader = command.ExecuteReader();
                    NetOP.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(NetOP);
        }

        [HttpDelete("deleteProduct/{id}")]
        public JsonResult DeleteMachine(int id)
        {
            string queryDelete = @"delete from dbo.ole_products 
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

        [HttpPut("insertProduct")]
        public JsonResult CreateNewMachine(Product product)
        {
            string QueryNewPO = @"insert into dbo.ole_products ( product, GMID , [bulk] ,family ,GIFAP,description ,
formulationType ,size ,idealRate, bottlesPerCase)
                                 values (@product, @GMID ,@bulk,@family ,@GIFAP,@description ,
@formulationType ,@size ,@idealRate, @bottlesPerCase)";


            DataTable NewMachine = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QueryNewPO, connection))
                {
                    command.Parameters.AddWithValue("@product", product.product);
                    command.Parameters.AddWithValue("@GMID", product.GMID);
                    command.Parameters.AddWithValue("@bulk", product.bulk);
                    command.Parameters.AddWithValue("@family", product.family);
                    command.Parameters.AddWithValue("@GIFAP", product.GIFAP);
                    command.Parameters.AddWithValue("@description", product.description); 
                    command.Parameters.AddWithValue("@formulationType", product.formulationType);
                    command.Parameters.AddWithValue("@size", product.size);
                    command.Parameters.AddWithValue("@idealRate", product.idealRate);
                    command.Parameters.AddWithValue("@bottlesPerCase", product.bottlesPerCase);

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