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
    public class DowntimeController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public DowntimeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("getMachines/{productionName}/{downtimeType}/unplannedDowntime/{worksite}")]
        public JsonResult GetUnplannedDowntime2Worksite(string productionName, string __, string worksite)
        {
            string queryUnplannedDowntime2 = @"select *
                                              from dbo.ole_machines 
                                              where name != 'filler' and worksite = @worksite and productionline_name = @productionName";

            DataTable UnplannedDowntime2 = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryUnplannedDowntime2, connection))
                {
                    command.Parameters.AddWithValue("@worksite", worksite);
                    command.Parameters.AddWithValue("@productionName", productionName);

                    reader = command.ExecuteReader();
                    UnplannedDowntime2.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(UnplannedDowntime2);
        }

        [HttpGet("getMachines/{productionName}/{downtimeType}/unplannedDowntime")]
        public JsonResult GetUnplannedDowntime2(string _, string __)
        {
            string queryUnplannedDowntime2 = @"select *
                                              from dbo.ole_machines 
                                              where name != 'filler'";

            DataTable UnplannedDowntime2 = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryUnplannedDowntime2, connection))
                {
                    reader = command.ExecuteReader();
                    UnplannedDowntime2.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(UnplannedDowntime2);
        }

        [HttpGet("summary/{productionName}/{downtimeType}/{worksite}")]
        public JsonResult GetSummaryWorksite(string productionName, string downtimeType, string worksite)
        {
            string queryDowntimeReason = @"select *
                                          from dbo.ole_downtimeReason 
                                          where downtimeType = @downtimeType and worksite = @worksite and production_line = @productionName";

            DataTable downtimeReason = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryDowntimeReason, connection))
                {
                    command.Parameters.AddWithValue("@downtimeType", downtimeType);
                    command.Parameters.AddWithValue("@worksite", worksite);
                    command.Parameters.AddWithValue("@productionName", productionName);

                    reader = command.ExecuteReader();
                    downtimeReason.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(downtimeReason);
        }

        [HttpGet("summary/{productionName}/{downtimeType}")]
        public JsonResult GetSummary(string _, string downtimeType)
        {
            string queryDowntimeReason = @"select *
                                          from dbo.ole_downtimeReason 
                                          where downtimeType = @downtimeType";

            DataTable downtimeReason = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryDowntimeReason, connection))
                {
                    command.Parameters.AddWithValue("@downtimeType", downtimeType);
                    reader = command.ExecuteReader();
                    downtimeReason.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(downtimeReason);
        }

        [HttpDelete("deleteDowntime/{id}")]
        public JsonResult DeleteDowntimeReason(int id)
        {
            string queryDelete = @"delete from dbo.ole_downtimeReason 
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

        [HttpGet("administratorDowntimeReason/{worksite}")]
        public JsonResult GetDowntimesAdministrator(string worksite)
        {
            string queryDowntimeReason = @"select *
                                          from dbo.ole_downtimeReason 
                                          order by worksite, production_line, downtimeType, reason ASC";

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

        [HttpPut("insertDowntimeReason")]
        public JsonResult CreateNewMachine(DowntimeReason downtimeReason)
        {
            string QueryNewPO = @"insert into dbo.ole_downtimeReason (reason, downtimeType, worksite, production_line)
                                  values (@reason, @downtimeType, @worksite, @production_line)";


            DataTable NewMachine = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QueryNewPO, connection))
                {
                    command.Parameters.AddWithValue("@reason", downtimeReason.reason);
                    command.Parameters.AddWithValue("@downtimeType", downtimeReason.downtimeType);
                    command.Parameters.AddWithValue("@production_line", downtimeReason.production_line);
                    command.Parameters.AddWithValue("@worksite", downtimeReason.worksite);

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