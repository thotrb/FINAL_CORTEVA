using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using CortevaApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace CortevaApp.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class MachineComponentController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public MachineComponentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("machine_component/{worksite}")]
        public JsonResult GetWorksite(string worksite)
        {
            string query = @"select * from dbo.machine_component order by  worksite,productionLine, machineName, name ASC";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    //command.Parameters.AddWithValue("@worksite", worksite);
                    reader = command.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    connection.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpDelete("deleteMachineComponent/{id}")]
        public JsonResult DeleteMachine(int id)
        {
            string queryDelete = @"delete from dbo.machine_component 
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

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select * from dbo.machine_component";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    reader = command.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    connection.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost("insertMachineComponent")]
        public JsonResult Post(MachineComponent mc)
        {
            string query = @"insert into dbo.machine_component (name, machineName, other_machine, worksite, productionLine)
                            values (@name, @machineName, @otherMachine, @worksite, @productionLine)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", mc.name);
                    command.Parameters.AddWithValue("@machineName", mc.machineName);
                    command.Parameters.AddWithValue("@otherMachine", mc.other_machine);
                    command.Parameters.AddWithValue("@worksite", mc.worksite);
                    command.Parameters.AddWithValue("@productionLine", mc.productionLine);

                    reader = command.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    connection.Close();
                }
            }

            return new JsonResult("Added successfully");
        }

        [HttpPut]
        public JsonResult Put(MachineComponent mc)
        {
            string query = @"update dbo.machine_component
                            set name=@name, machineName=@machineName, other_machine=@otherMachine
                            where id=@id";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", mc.id);
                    command.Parameters.AddWithValue("@name", mc.name);
                    command.Parameters.AddWithValue("@machineName", mc.machineName);
                    command.Parameters.AddWithValue("@otherMachine", mc.other_machine);
                    reader = command.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    connection.Close();
                }
            }

            return new JsonResult("Updated successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"delete from dbo.machine_component where id=@id";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    reader = command.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    connection.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }
    }
}