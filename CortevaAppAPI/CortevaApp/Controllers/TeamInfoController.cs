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
    public class TeamInfoController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public TeamInfoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

  

        [HttpGet("administratorTeamInfo/{worksite}")]
        public JsonResult GetTeamInfosAdministrator(string worksite)
        {
            string queryDowntimeReason = @"select *
                                          from dbo.teamInfo 
                                          order by worksite_name, type, workingDebut ASC";

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

        [HttpPut("insertTeamInfo")]
        public JsonResult CreateNewMachine(TeamInfo teamInfo)
        {
            string QueryNewPO = @"insert into dbo.teamInfo (workingDebut, workingEnd, type, worksite_name, state)
                                  values (@workingDebut, @workingEnd, @type, @worksite_name, @state)";


            DataTable NewMachine = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QueryNewPO, connection))
                {
                    command.Parameters.AddWithValue("@workingDebut", teamInfo.workingDebut);
                    command.Parameters.AddWithValue("@workingEnd", teamInfo.workingEnd);
                    command.Parameters.AddWithValue("@type", teamInfo.type);
                    command.Parameters.AddWithValue("@worksite_name", teamInfo.worksite_name);
                    command.Parameters.AddWithValue("@state", teamInfo.state);

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