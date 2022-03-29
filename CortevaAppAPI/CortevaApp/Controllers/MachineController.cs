using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpGet("unplannedDowntime/unplannedDowntime/{machineName}/{worksite}/0")]
        public JsonResult GetUnplannedDowntimeMachineIssueFiller(string machineName, string worksite)
        {
            string queryIssues = @"select mc.name as component, m.name, other_machine, mc.worksite
                                   from dbo.ole_machines m, dbo.machine_component mc
                                   where m.name = mc.machineName
                                   and m.name = @machineName and mc.worksite = @worksite and mc.other_machine=0";

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

                    reader = command.ExecuteReader();
                    Issues.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(Issues);
        }

        [HttpGet("unplannedDowntime/unplannedDowntime/{machineName}/{worksite}")]
        public JsonResult GetUnplannedDowntimeMachineIssue(string machineName, string worksite)
        {
            string queryIssues = @"select mc.name as component, m.name, other_machine, mc.worksite
                                   from dbo.ole_machines m, dbo.machine_component mc
                                   where m.name = mc.machineName
                                   and m.name = @machineName and mc.worksite = @worksite";

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

                    reader = command.ExecuteReader();
                    Issues.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(Issues);
        }
    }
}