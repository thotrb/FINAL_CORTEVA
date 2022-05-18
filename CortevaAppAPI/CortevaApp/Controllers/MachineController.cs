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
    public class MachineController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public MachineController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("machines/{productionlineId}/{worksite}")]
        public JsonResult getMachines(int productionlineId, string worksite)
        {
            string queryMachines = @"select *
                                    from dbo.ole_machines m, dbo.ole_productionline pl
                                    where m.productionline_name = pl.productionline_name
                                    and pl.id = @productionlineId and m.worksite = @worksite
                                    order by m.ordre asc";

            string queryFormats = @"select *
                                    from dbo.ole_formats f, dbo.ole_productionline pl
                                    where pl.id = @productionlineId and pl.worksite_name = @worksite";

            DataTable machines = new DataTable();
            DataTable formats = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryMachines, connection))
                {
                    command.Parameters.AddWithValue("@productionlineId", productionlineId);
                    command.Parameters.AddWithValue("@worksite", worksite);

                    reader = command.ExecuteReader();
                    machines.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(queryFormats, connection))
                {
                    command.Parameters.AddWithValue("@productionlineId", productionlineId);
                    command.Parameters.AddWithValue("@worksite", worksite);

                    reader = command.ExecuteReader();
                    formats.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            DataTable[] data = { machines, formats };

            return new JsonResult(data);
        }

        [HttpDelete("deleteMachine/{id}")]
        public JsonResult DeleteMachine(int id)
        {
            string queryDelete = @"delete from dbo.ole_machines 
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

        [HttpGet("machines/{productionlineId}")]
        public JsonResult getMachines(int productionlineId)
        {
            string queryMachines = @"select *
                                    from dbo.ole_machines m, dbo.ole_productionline pl
                                    where m.productionline_name = pl.productionline_name
                                    and pl.id = @productionlineId
                                    order by m.ordre asc";

            string queryFormats = @"select *
                                    from dbo.ole_formats f, dbo.ole_productionline pl
                                    where pl.id = @productionlineId";

            DataTable machines = new DataTable();
            DataTable formats = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryMachines, connection))
                {
                    command.Parameters.AddWithValue("@productionlineId", productionlineId);
                    reader = command.ExecuteReader();
                    machines.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(queryFormats, connection))
                {
                    command.Parameters.AddWithValue("@productionlineId", productionlineId);
                    reader = command.ExecuteReader();
                    formats.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            DataTable[] data = { machines, formats };

            return new JsonResult(data);
        }

        [HttpGet("unplannedDowntime/unplannedDowntime/{machineName}/{worksite}/0/{productionline}")]
        public JsonResult GetUnplannedDowntimeMachineIssueFiller(string machineName, string worksite, string productionline)
        {
            string queryIssues = @"select mc.name as component, m.name, other_machine, mc.worksite
                                   from dbo.ole_machines m, dbo.machine_component mc
                                   where m.name = mc.machineName
                                   and m.name = @machineName and mc.worksite = @worksite and mc.other_machine=0 and mc.productionLine = @productionline";

            DataTable Issues = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryIssues, connection))
                {
                    command.Parameters.AddWithValue("@machineName", machineName);
                    command.Parameters.AddWithValue("@worksite", worksite);
                    command.Parameters.AddWithValue("@productionline", productionline);

                    reader = command.ExecuteReader();
                    Issues.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(Issues);
        }

        [HttpGet("unplannedDowntime/unplannedDowntime/{machineName}/{worksite}/{productionline}")]
        public JsonResult GetUnplannedDowntimeMachineIssue(string machineName, string worksite, string productionline)
        {
            string queryIssues = @"select mc.name as component, m.name, other_machine, mc.worksite, mc.machineName
                                   from dbo.ole_machines m, dbo.machine_component mc
                                   where m.name = mc.machineName
                                   and m.name = @machineName and mc.worksite = @worksite and mc.productionLine = @productionline
                                   union
                                   select mc.name as component, mc.machineName, other_machine, mc.worksite, mc.machineName
                                   from dbo.machine_component mc
                                   where mc.machineName = 'other' and mc.worksite = @worksite and mc.productionLine = @productionline";

            DataTable Issues = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryIssues, connection))
                {
                    command.Parameters.AddWithValue("@machineName", machineName);
                    command.Parameters.AddWithValue("@worksite", worksite);
                    command.Parameters.AddWithValue("@productionline", productionline);

                    reader = command.ExecuteReader();
                    Issues.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(Issues);
        }

        [HttpGet("administratorMachine/{worksite}")]
        public JsonResult GetAdministratorMachine(string worksite)
        {
            string queryIssues = @"select *
                                   from dbo.ole_machines m
                                   order by worksite, productionline_name, ordre ASC";

            DataTable Issues = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryIssues, connection))
                {
                    //command.Parameters.AddWithValue("@worksite", worksite);

                    reader = command.ExecuteReader();
                    Issues.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(Issues);
        }

        [HttpPut("insertMachine")]
        public JsonResult CreateNewMachine(Machine machine)
        {
            string QueryNewPO = @"insert into dbo.ole_machines (name, operation, fabricant, modele, productionline_name, denomination_ordre, ordre, rejection, worksite)
                                  values (@name, @operation, @fabricant, @modele, @productionline_name, @denomination_ordre, @ordre, @rejection, @worksite)";


            DataTable NewMachine = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QueryNewPO, connection))
                {
                    command.Parameters.AddWithValue("@name", machine.name);
                    command.Parameters.AddWithValue("@operation", machine.operation);
                    command.Parameters.AddWithValue("@fabricant", machine.fabricant);
                    command.Parameters.AddWithValue("@modele", machine.modele);
                    command.Parameters.AddWithValue("@productionline_name", machine.productionline_name);
                    command.Parameters.AddWithValue("@denomination_ordre", machine.denomination_ordre);
                    command.Parameters.AddWithValue("@ordre", machine.ordre);
                    command.Parameters.AddWithValue("@rejection", machine.rejection);
                    command.Parameters.AddWithValue("@worksite", machine.worksite);

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