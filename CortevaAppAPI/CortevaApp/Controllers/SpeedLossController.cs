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
    public class SpeedLossController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public SpeedLossController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("speedLosses/{po}/{productionLine}/{shift}")]
        public JsonResult GetSLShift(string po, string productionLine, string shift)
        {
            string querySpeedLosses = @"select *
                                       from dbo.ole_speed_losses sl
                                       where sl.productionline = @productionLine
                                       and sl.OLE = @po and sl.shift = @shift";


            DataTable speedLosses = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(querySpeedLosses, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@po", po);
                    command.Parameters.AddWithValue("@shift", shift);
                    reader = command.ExecuteReader();
                    speedLosses.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(speedLosses);
        }

        [HttpGet("speedLosses/{po}/{productionLine}")]
        public JsonResult GetSL(string po, string productionLine)
        {
            string querySpeedLosses = @"select *
                                       from dbo.ole_speed_losses sl
                                       where sl.productionline = @productionLine
                                       and sl.OLE = @po";


            DataTable speedLosses = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(querySpeedLosses, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@po", po);
                    reader = command.ExecuteReader();
                    speedLosses.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(speedLosses);
        }

        [HttpGet("getSpeedLosses/{site}/{productionLine}/{startingDate}/{endingDate}/{shift}")]
        public JsonResult GetSpeedLosses(string site, string productionLine, string startingDate, string endingDate, string shift)
        {
            startingDate += " 00:00:00.000";
            endingDate += " 23:59:59.999";

            string querySpeedLossesEvents = @"select sl.duration, sl.reason, sl.comment, sl.OLE, pos.qtyProduced, pos.workingDuration,
                                            prod.size, prod.idealRate, sl.id as slid
                                            from dbo.ole_speed_losses sl, dbo.ole_pos pos, dbo.ole_products prod, dbo.worksite w
                                            where sl.productionline = @productionLine
                                            and sl.OLE = pos.number
                                            and prod.GMID = pos.GMIDCode
                                            and sl.created_at >= @startingDate
                                            and sl.created_at <= @endingDate
                                            and w.name = @site";

            if (shift != "allTeams") {
                querySpeedLossesEvents += " and sl.shift = @shift";
            }

            DataTable SpeedLossesEvents = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(querySpeedLossesEvents, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@startingDate", startingDate);
                    command.Parameters.AddWithValue("@endingDate", endingDate);
                    command.Parameters.AddWithValue("@site", site);
                    if (shift != "allTeams") {
                        command.Parameters.AddWithValue("@shift", shift);
                    }
                    reader = command.ExecuteReader();
                    SpeedLossesEvents.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            IDictionary<string, DataTable> Result = new Dictionary<string, DataTable>()
            {
              { "SLEVENTS", SpeedLossesEvents }
            };

            return new JsonResult(Result);
        }

        [HttpPost("speedLoss")]
        public JsonResult SaveSpeedLoss(SpeedLoss sl)
        {
            string QuerySaveSL = @"insert into dbo.ole_speed_losses
                                   (OLE, productionline, duration, reason, comment, shift)
                                   values (@OLE, @PL, @D, @R, @COMM, @shift)";


            DataTable SaveSL = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QuerySaveSL, connection))
                {
                    command.Parameters.AddWithValue("@OLE", sl.OLE);
                    command.Parameters.AddWithValue("@PL", sl.productionline);
                    command.Parameters.AddWithValue("@R", sl.reason);
                    command.Parameters.AddWithValue("@D", sl.duration);
                    command.Parameters.AddWithValue("@COMM", sl.comment);
                    command.Parameters.AddWithValue("@shift", sl.shift);
                    reader = command.ExecuteReader();
                    SaveSL.Load(reader);
                    reader.Close();
                }


                connection.Close();
            }

            return new JsonResult(SaveSL);
        }
    }
}