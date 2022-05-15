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
    public class WorksiteController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public WorksiteController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("sites/{username}")]
        public JsonResult getSitesUser(string username)
        {
            string querySites = @"select worksite_name as name from dbo.users where login=@username";
            string queryProductionLines = @"select  pl.productionline_name, pl.worksite_name as name
                                            from dbo.ole_productionline pl, users u 
                                            where u.productionline = pl.productionline_name and u.login = @username";
            string queryTeams = @"select ti.type, ti.worksite_name
                                from dbo.teamInfo ti, users u
                                where u.login = @username 
                                and u.worksite_name = ti.worksite_name";

            DataTable sites = new DataTable();
            DataTable productionLines = new DataTable();
            DataTable teams = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(querySites, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    reader = command.ExecuteReader();
                    sites.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(queryProductionLines, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    reader = command.ExecuteReader();
                    productionLines.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(queryTeams, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    reader = command.ExecuteReader();
                    teams.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            DataTable[] data = { sites, productionLines, teams };

            return new JsonResult(data);
        }

        [HttpGet("sites")]
        public JsonResult getSites()
        {
            string querySites = @"select * from dbo.worksite";
            string queryProductionLines  = @"select pl.id, pl.productionline_name, w.id, w.name
                                            from dbo.ole_productionline pl, dbo.worksite w
                                            where w.name = pl.worksite_name order by w.name, pl.productionline_name";
            string queryTeams = @"select type, worksite_name
                                from dbo.teamInfo";
                    

            DataTable sites = new DataTable();
            DataTable productionLines = new DataTable();
            DataTable teams = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(querySites, connection))
                {
                    reader = command.ExecuteReader();
                    sites.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(queryProductionLines, connection))
                {
                    reader = command.ExecuteReader();
                    productionLines.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(queryTeams, connection))
                {
                    reader = command.ExecuteReader();
                    teams.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            DataTable[] data = { sites, productionLines, teams }; 

            return new JsonResult(data);
        }

        [HttpGet("worksiteid/{worksite}")]
        public JsonResult GetWorksiteID(string worksite)
        {
            string QueryWorksiteID = @"select id
                                       from dbo.worksite
                                       where name = @worksite";

            DataTable WorksiteID = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QueryWorksiteID, connection))
                {
                    command.Parameters.AddWithValue("@worksite", worksite);
                    reader = command.ExecuteReader();
                    WorksiteID.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(WorksiteID);
        }

        [HttpDelete("deleteWorksite/{id}")]
        public JsonResult DeleteMachine(int id)
        {
            string queryDelete = @"delete from dbo.worksite 
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

        [HttpPut("insertWorksite")]
        public JsonResult CreateNewUser(Worksite worksite)
        {
            string QueryNewPO = @"insert into dbo.worksite (name)
                                  values (@name)";


            DataTable NewMachine = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QueryNewPO, connection))
                {
                    command.Parameters.AddWithValue("@name", worksite.name);
       

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