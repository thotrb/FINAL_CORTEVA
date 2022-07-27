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
    public class POController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public POController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("po/{po}/{shift}")]
        public JsonResult IsPOPossible(string po, string shift)
        {
            string QueryPon = @"select count(*) as result
                                from dbo.ole_pos po
                                where po.number = @po and po.shift=@shift";


            DataTable Pon = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QueryPon, connection))
                {
                    command.Parameters.AddWithValue("@po", po);
                    command.Parameters.AddWithValue("@shift", shift);

                    reader = command.ExecuteReader();
                    Pon.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(Pon.Select()[0][0]);
        }

        [HttpGet("pos/{shift}/{site}")]
        public JsonResult getMachines(string shift, string site)
        {
            string queryPos = @"select *
                                from dbo.ole_assignement_team_pos atp, dbo.ole_pos pos, dbo.worksite w
                                where atp.worksite = w.id
                                and pos.number = atp.po
                                and w.name = @site
                                and atp.shift = @shift";


            DataTable pos = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryPos, connection))
                {
                    command.Parameters.AddWithValue("@site", site);
                    command.Parameters.AddWithValue("@shift", shift);
                    reader = command.ExecuteReader();
                    pos.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(pos);
        }

        [HttpGet("previousBulk/{productionline}/{ponumber}/{currentGMID}")]
        public JsonResult getPreviousBulk(string site, string productionLine, string ponumber, string currentGMID)
        {
            string queryPreviousBulk = @"select distinct prod.""bulk""
                                        from dbo.ole_pos pos, dbo.ole_products prod
                                        where pos.GMIDCode = prod.GMID
                                        and pos.productionline_name = @productionLine
                                        order by prod.""bulk""";

            DataTable pos = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryPreviousBulk, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    reader = command.ExecuteReader();
                    pos.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(pos);
        }

        [HttpGet("countassignation/{username}/{po}/{productionline}")]
        public JsonResult isAssignantionPossible(string username, string po, int productionLine)
        {
            string QueryAssignation = @"select count(*) as result
                                       from dbo.ole_assignement_team_pos atp
                                       where atp.username = @username
                                       and atp.po = @po
                                       and atp.productionline = @productionLine";


            DataTable Assignation = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QueryAssignation, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@po", po);
                    command.Parameters.AddWithValue("@productionline", productionLine);
                    reader = command.ExecuteReader();
                    Assignation.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(Assignation.Select()[0][0]);
        }

        [HttpGet("getHistoricPO/{productionName}")]
        public JsonResult getHistoricPO(string productionName)
        {
            string Query = @"select ole_pos.created_at, ole_pos.number, ole_pos.shift, ole_pos.startTime as workingDebut, ole_pos.endTime as workingEnd, ole_pos.qtyProduced
	                        from ole_pos
			                where ole_pos.productionline_name = @productionName
		                    order by ole_pos.created_at desc";


            DataTable Assignation = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.AddWithValue("@productionName", productionName);
                    reader = command.ExecuteReader();
                    Assignation.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(Assignation);
        }


        [HttpPost("addassignation")]
        public JsonResult CreateAssignement(AssignementTeamPO Assignement)
        {
            string QueryNewAssignement = @"insert into dbo.ole_assignement_team_pos (username, productionline, po, shift, worksite)
                                       values (@Username, @ProductionLine, @PO, @Shift, @Worksite)";


            DataTable NewAssignement = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QueryNewAssignement, connection))
                {
                    command.Parameters.AddWithValue("@Username", Assignement.username);
                    command.Parameters.AddWithValue("@ProductionLine", Assignement.productionline);
                    command.Parameters.AddWithValue("@PO", Assignement.po);
                    command.Parameters.AddWithValue("@Shift", Assignement.shift);
                    command.Parameters.AddWithValue("@Worksite", Assignement.worksite);
                    reader = command.ExecuteReader();
                    NewAssignement.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(NewAssignement);
        }

        [HttpPut("PO/insertPO/PO")]
        public JsonResult CreatePO(PO po)
        {
            string QueryNewPO = @"insert into dbo.ole_pos (number, GMIDCode, productionline_name, shift, created_at)
                                  values (@Number, @GMIDCode, @ProductionLine, @Shift, @datePO)";



            DataTable NewPO = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QueryNewPO, connection))
                {
                    command.Parameters.AddWithValue("@Number", po.number);
                    command.Parameters.AddWithValue("@GMIDCode", po.GMIDCode);
                    command.Parameters.AddWithValue("@ProductionLine", po.productionline_name);
                    command.Parameters.AddWithValue("@Shift", po.shift);
                    command.Parameters.AddWithValue("@datePO", po.created_at);

                    reader = command.ExecuteReader();
                    NewPO.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(NewPO);
        }


        [HttpPost("stopPO/{PO}/{availability}/{performance}/{quality}/{OLE}/{quantityProduced}/{totalDuration}/{totalOperatingTime}/{totalNetOperatingTime}/{shift}/{startPO}/{endPO}")]
        public JsonResult StopPO(string po, double availability, double performance, double quality, double OLE, int quantityProduced, int totalDuration, int totalOperatingTime, int totalNetOperatingTime, string shift, string startPO, string endPO)
        {
            string QueryStopPO = @"update dbo.ole_pos
                                   set state = 0,
                                   performance = @Performance,
                                   availability = @Availability,
                                   quality = @Quality,
                                   OLE = @OLE,
                                   qtyProduced = @QuantityProduced,
                                   workingDuration = @TotalDuration,
                                   totalOperatingTime = @totalOT,
                                   totalNetOperatingTime = @totalNetOT,
                                   startTime = @startPO, 
                                   endTime = @endPO
                                   where number = @PO and shift=@shift";


            DataTable StopPO = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QueryStopPO, connection))
                {
                    command.Parameters.AddWithValue("@Performance", performance);
                    command.Parameters.AddWithValue("@Availability", availability);
                    command.Parameters.AddWithValue("@Quality", quality);
                    command.Parameters.AddWithValue("@OLE", OLE);
                    command.Parameters.AddWithValue("@QuantityProduced", quantityProduced);
                    command.Parameters.AddWithValue("@TotalDuration", totalDuration);
                    command.Parameters.AddWithValue("@totalOT", totalOperatingTime);
                    command.Parameters.AddWithValue("@totalNetOT", totalNetOperatingTime);
                    command.Parameters.AddWithValue("@PO", po);
                    command.Parameters.AddWithValue("@shift", shift);
                    command.Parameters.AddWithValue("@startPO", startPO);
                    command.Parameters.AddWithValue("@endPO", endPO);

                    reader = command.ExecuteReader();
                    StopPO.Load(reader);
                    reader.Close();
                }


                connection.Close();
            }

            return new JsonResult(StopPO);
        }
    }
}