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
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CortevaApp.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet("user/{username}")]
        public JsonResult GetUserInfo(string username)
        {
            string userInfoQuery = @"select *
                                   from dbo.users u, dbo.worksite w
                                   where u.worksite_name = w.name and u.login = @username";

            string crewLeadersQuery = @"select *
                                      from dbo.users u, dbo.worksite w
                                      where u.worksite_name = w.name and (u.status = 1 or u.status = 2) and u.worksite_name = (Select worksite_name
                                                                                                              from dbo.users u2, dbo.worksite w2
                                                                                                                where u2.worksite_name = w2.name and u2.login = @username )
                                            and u.productionline = (Select u2.productionline
                                                                      from dbo.users u2, dbo.worksite w2
                                                                      where u2.worksite_name = w2.name and u2.login = @username)";
                                            

            string shiftsQuery = @"select *
                                 from dbo.users u, dbo.teamInfo ti, dbo.worksite w
                                 where u.worksite_name = ti.worksite_name
                                 and u.worksite_name = w.name
                                 and u.login = @username";

            string productionLineQuery = @"select pl.productionline_name, w.id
                                        from dbo.users u, dbo.ole_productionline pl, dbo.worksite w
                                        where u.worksite_name = w.name
                                        and pl.worksite_name = w.name
                                        and pl.productionline_name = u.productionline
                                        and u.login = @username";



            DataTable userInfo = new DataTable();
            DataTable crewLeaders = new DataTable();
            DataTable shifts = new DataTable();
            DataTable productionLine = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(userInfoQuery, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    reader = command.ExecuteReader();
                    userInfo.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(crewLeadersQuery, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    reader = command.ExecuteReader();
                    crewLeaders.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(shiftsQuery, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    reader = command.ExecuteReader();
                    shifts.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(productionLineQuery, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    reader = command.ExecuteReader();
                    productionLine.Load(reader);
                    reader.Close();
                }

                connection.Close();
            }

            DataTable[] data = { userInfo, crewLeaders, shifts, productionLine };

            return new JsonResult(data);
        }

        [HttpDelete("deleteUser/{id}")]
        public JsonResult DeleteMachine(int id)
        {
            string queryDelete = @"delete from dbo.users 
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

        [HttpGet("getLanguage/{username}")]
        public JsonResult GetLanguage(string username)
        {
            string queryDowntimeReason = @"select language
                                          from dbo.users 
                                          where login = @username";

            DataTable downtimeReason = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryDowntimeReason, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    reader = command.ExecuteReader();
                    downtimeReason.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(downtimeReason.Select()[0][0]);
        }



        [HttpGet("administratorUsers/{worksite}")]
        public JsonResult GetUsersAdministrator(string worksite)
        {
            string queryDowntimeReason = @"select *
                                          from dbo.users 
                                          order by worksite_name, productionline, firstname, lastname, language ASC";

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

        [HttpPut("insertUser")]
        public JsonResult CreateNewUser(User user)
        {
            string QueryNewPO = @"insert into dbo.users (login, password, worksite_name, lastname, firstname, status, productionline, language)
                                  values (@login, @password, @worksite_name, @lastname, @firstname, @status, @productionline, @language)";


            DataTable NewMachine = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QueryNewPO, connection))
                {
                    command.Parameters.AddWithValue("@login", user.login);
                    command.Parameters.AddWithValue("@password", user.password);
                    command.Parameters.AddWithValue("@worksite_name", user.worksite_name);
                    command.Parameters.AddWithValue("@lastname", user.lastname);
                    command.Parameters.AddWithValue("@firstname", user.firstname);
                    command.Parameters.AddWithValue("@status", user.status);
                    command.Parameters.AddWithValue("@productionline", user.productionline);
                    command.Parameters.AddWithValue("@language", user.language);

                    reader = command.ExecuteReader();
                    NewMachine.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(NewMachine);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(User user)
        {
            string UserAuthQuery = @"select *
                                   from dbo.users u
                                   where u.login = @username and u.password = @password";


            DataTable UserAuth = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(UserAuthQuery, connection))
                {
                    command.Parameters.AddWithValue("@username", user.login);
                    command.Parameters.AddWithValue("@password", user.password);
                    reader = command.ExecuteReader();
                    UserAuth.Load(reader);
                    reader.Close();
                }

                connection.Close();
            }

            int result = UserAuth.Rows.Count;
            int _credentials = (int)UserAuth.Rows[0]["status"];
            user.status = _credentials;

            if (result > 0)
            {
                var tokenString = GenerateJWT(user);
                return Ok(new { token = tokenString });
            } else
            {
                return Unauthorized();
            }
        }

        private string GenerateJWT(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var ExpDate = DateTime.UtcNow.AddMinutes(720);
            var claims = new[]
            {
                new Claim("login", user.login),
                new Claim("status", user.status.ToString())
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Issuer"],
              claims,
              expires: ExpDate,
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        } 



    }
}