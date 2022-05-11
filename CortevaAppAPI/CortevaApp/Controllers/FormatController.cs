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
    public class FormatController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public FormatController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("administratorFormat/{worksite}")]
        public JsonResult GetFormatAdministrator(string worksite)
        {
            string queryDowntimeReason = @"select * 
                                           from ole_formats, ole_productionline 
                                            where ole_formats.productionlineName = ole_productionline.productionline_name 
                                            order by worksite_name, productionlineName ASC, [format] DESC 
";

            DataTable downtimeReason = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryDowntimeReason, connection))
                {
                    //command.Parameters.AddWithValue("@worksite", worksite);
                    reader = command.ExecuteReader();
                    downtimeReason.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(downtimeReason);
        }

        [HttpDelete("deleteFormat/{id}")]
        public JsonResult DeleteFormat(int id)
        {
            string queryDelete = @"delete from dbo.ole_formats 
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

        [HttpPut("insertFormat")]
        public JsonResult InsertFormatAdministrator(Format format)
        {
            string QueryNewPO = @"insert into dbo.ole_formats (format, shape, mat1, mat2, mat3, design_rate, productionlineName)
                                  values (@format, @shape, @mat1, @mat2, @mat3, @design_rate, @productionlineName)";


            DataTable NewFormat = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QueryNewPO, connection))
                {
                    command.Parameters.AddWithValue("@format", format.format);
                    command.Parameters.AddWithValue("@shape", format.shape);
                    command.Parameters.AddWithValue("@mat1", format.mat1);
                    command.Parameters.AddWithValue("@mat2", format.mat2);
                    command.Parameters.AddWithValue("@mat3", format.mat3);
                    command.Parameters.AddWithValue("@design_rate", format.design_rate);
                    command.Parameters.AddWithValue("@productionlineName", format.productionlineName);

                    reader = command.ExecuteReader();
                    NewFormat.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(NewFormat);
        }

    }
}