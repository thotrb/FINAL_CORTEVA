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
    public class EventsController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public EventsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpDelete("deleteCIP/{id}")]
        public JsonResult deleteCIP(int id)
        {

            string QueryDeleteCIP = @"delete from dbo.ole_unplanned_event_cips where id = @id";

            DataTable DeleteCIP = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QueryDeleteCIP, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    reader = command.ExecuteReader();
                    DeleteCIP.Load(reader);
                    reader.Close();
                }


                connection.Close();
            }
            return new JsonResult(DeleteCIP);
        }

        [HttpGet("getOverlappedCIP/{productionLine}/{date}/{OLE}")]
        public JsonResult getOverlappedCIP(string productionLine, string date, string OLE)
        {

            string[] _date = date.Split('-');
            int beginningYear = Int16.Parse(_date[0]);
            int beginningMonth = Int16.Parse(_date[1]);
            int beginningDay = Int16.Parse(_date[2]);
            DateTime cipDateMax = new DateTime(beginningYear, beginningMonth, beginningDay);
            cipDateMax = cipDateMax.AddDays(1);
            string dateString = cipDateMax.ToString("yyyy-MM-dd");

            string QueryOverlappingCIP = @"select * 
                                        from dbo.ole_unplanned_event_cips
                                        where productionline = @productionLine
                                        and created_at >= @minDate
                                        and created_at <= @maxDate
                                        and OLE = @OLE
                                        and finished = 1";


            DataTable OverlappingCIP = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QueryOverlappingCIP, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@maxDate", dateString);
                    command.Parameters.AddWithValue("@minDate", date);
                    command.Parameters.AddWithValue("@OLE", OLE);
                    reader = command.ExecuteReader();
                    OverlappingCIP.Load(reader);
                    reader.Close();
                }


                connection.Close();
            }

            return new JsonResult(OverlappingCIP);
        }

        [HttpGet("events/{po}/{productionLine}/{shift}")]
        public JsonResult GetEventsShift(string po, string productionLine, string shift)
        {
            string queryPlannedEvents = @"select pe.id, pe.duration as total_duration, pe.reason as type,
                                        pe.comment, pe.created_at as updated_at, pe.OLE, pe.productionline, pe.kind
                                        from dbo.ole_planned_events pe
                                        where pe.productionline = @productionLine
                                        and pe.OLE = @po and pe.shift = @shift";

            string queryChanginClients = @"select id, total_duration, type, comment, created_at as updated_at, OLE, productionline, kind
                                        from dbo.ole_unplanned_event_changing_clients
                                        where productionline = @productionLine
                                        and OLE = @po and shift = @shift";

            string queryCIP = @"select id, total_duration, type, comment, created_at as updated_at, OLE, productionline, kind
                                from dbo.ole_unplanned_event_cips
                                where productionline = @productionLine
                                and OLE = @po and shift = @shift";

            string queryUnplanned = @"select id, total_duration, type, comment, created_at as updated_at, OLE, productionline, kind
                                    from dbo.ole_unplanned_event_unplanned_downtimes
                                    where productionline = @productionLine
                                    and OLE = @po and shift = @shift";

            string queryChangingFormats = @"select id, total_duration, type, comment, created_at as updated_at, OLE, productionline, kind
                                        from dbo.ole_unplanned_event_changing_formats
                                        where productionline = @productionLine
                                        and OLE = @po and  shift = @shift";

            DataTable plannedEvents = new DataTable();
            DataTable changingClients = new DataTable();
            DataTable CIP = new DataTable();
            DataTable unplanned = new DataTable();
            DataTable changingFormats = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryPlannedEvents, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@po", po);
                    command.Parameters.AddWithValue("@shift", shift);

                    reader = command.ExecuteReader();
                    plannedEvents.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(queryChanginClients, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@po", po);
                    command.Parameters.AddWithValue("@shift", shift);

                    reader = command.ExecuteReader();
                    changingClients.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(queryCIP, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@po", po);
                    command.Parameters.AddWithValue("@shift", shift);

                    reader = command.ExecuteReader();
                    CIP.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(queryUnplanned, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@po", po);
                    command.Parameters.AddWithValue("@shift", shift);

                    reader = command.ExecuteReader();
                    unplanned.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(queryChangingFormats, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@po", po);
                    command.Parameters.AddWithValue("@shift", shift);

                    reader = command.ExecuteReader();
                    changingFormats.Load(reader);
                    reader.Close();
                }


                connection.Close();
            }

            DataTable[] data = { plannedEvents, changingClients, CIP, unplanned, changingFormats };

            return new JsonResult(data);
        }



        [HttpGet("events/{po}/{productionLine}")]
        public JsonResult GetEvents(string po, string productionLine)
        {
            string queryPlannedEvents = @"select pe.duration as total_duration, pe.reason as type,
                                        pe.comment, pe.updated_at, pe.OLE, pe.productionline, pe.kind
                                        from dbo.ole_planned_events pe
                                        where pe.productionline = @productionLine
                                        and pe.OLE = @po";

            string queryChanginClients = @"select total_duration, type, comment, updated_at, OLE, productionline, kind
                                        from dbo.ole_unplanned_event_changing_clients
                                        where productionline = @productionLine
                                        and OLE = @po";

            string queryCIP = @"select total_duration, type, comment, updated_at, OLE, productionline, kind
                                from dbo.ole_unplanned_event_cips
                                where productionline = @productionLine
                                and OLE = @po";

            string queryUnplanned = @"select total_duration, type, comment, updated_at, OLE, productionline, kind
                                    from dbo.ole_unplanned_event_unplanned_downtimes
                                    where productionline = @productionLine
                                    and OLE = @po";

            string queryChangingFormats = @"select total_duration, type, comment, updated_at, OLE, productionline, kind
                                        from dbo.ole_unplanned_event_changing_formats
                                        where productionline = @productionLine
                                        and OLE = @po";

            DataTable plannedEvents = new DataTable();
            DataTable changingClients = new DataTable();
            DataTable CIP = new DataTable();
            DataTable unplanned = new DataTable();
            DataTable changingFormats = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryPlannedEvents, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@po", po);
                    reader = command.ExecuteReader();
                    plannedEvents.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(queryChanginClients, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@po", po);
                    reader = command.ExecuteReader();
                    changingClients.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(queryCIP, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@po", po);
                    reader = command.ExecuteReader();
                    CIP.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(queryUnplanned, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@po", po);
                    reader = command.ExecuteReader();
                    unplanned.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(queryChangingFormats, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@po", po);
                    reader = command.ExecuteReader();
                    changingFormats.Load(reader);
                    reader.Close();
                }


                connection.Close();
            }

            DataTable[] data = { plannedEvents, changingClients, CIP, unplanned, changingFormats };

            return new JsonResult(data);
        }

        [HttpGet("UnplannedDowntimeEvents/{productionLine}/{startYear}/{endYear}/{shift}")]
        public JsonResult GetUnplannedDowntimeEvents(string productionLine, int startYear, int endYear, string shift)
        {
            string queryCIP = @"select *
                                from dbo.ole_unplanned_event_cips cip
                                where cip.productionline = @productionLine
                                and year(cip.created_at) >= @startYear
                                and year(cip.created_at) <= @endYear";

            string queryBNC = @"select *
                                from dbo.ole_unplanned_event_changing_clients bnc
                                where bnc.productionline = @productionLine
                                and year(bnc.created_at) >= @startYear
                                and year(bnc.created_at) <= @endYear";

            string queryCOV = @"select *
                                from dbo.ole_unplanned_event_changing_formats cov
                                where cov.productionline = @productionLine
                                and year(cov.created_at) >= @startYear
                                and year(cov.created_at) <= @endYear";

            string queryMachineShutdowns = @"select *
                                            from dbo.ole_unplanned_event_unplanned_downtimes
                                            where productionline = @productionLine
                                            and implicated_machine != 'other'
                                            and year(created_at) >= @startYear
                                            and year(created_at) <= @endYear";

            string queryExternalShutdowns = @"select *
                                            from dbo.ole_unplanned_event_unplanned_downtimes
                                            where productionline = @productionLine
                                            and implicated_machine = 'other'
                                            and year(created_at) >= @startYear
                                            and year(created_at) <= @endYear";

            string querySeqCips = @"select *
                                    from dbo.ole_unplanned_event_cips cip, dbo.ole_pos pos, dbo.ole_products prod
                                    where cip.productionline = @productionLine
                                    and pos.productionline_name = @productionLine
                                    and year(cip.created_at) >= @startYear
                                    and year(cip.created_at) <= @endYear
                                    and pos.number = cip.OLE
                                    and prod.GMID = pos.GMIDCode";


            string querySeqCovs = @"select *
                                    from dbo.ole_unplanned_event_changing_formats cov, dbo.ole_pos pos, dbo.ole_products prod
                                    where cov.productionline = @productionLine
                                    and pos.productionline_name = @productionLine
                                    and year(cov.created_at) >= @startYear
                                    and year(cov.created_at) <= @endYear
                                    and pos.number = cov.OLE
                                    and prod.GMID = pos.GMIDCode";

            if (shift != "allTeams")
            {
                queryCIP += " and cip.shift = @shift";
                queryCOV += " and bnc.shift = @shift";
                queryBNC += " and cov.shift = @shift";
                queryMachineShutdowns += " and shift = @shift";
                queryExternalShutdowns += " and shift = @shift";
                querySeqCips += " and cip.shift = @shift";
                querySeqCovs += " and cov.shift = @shift";
            }

            DataTable CIP = new DataTable();
            DataTable COV = new DataTable();
            DataTable BNC = new DataTable();
            DataTable machineShutdowns = new DataTable();
            DataTable externalShutdowns = new DataTable();
            DataTable seqCips = new DataTable();
            DataTable seqCovs = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryCIP, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@startYear", startYear);
                    command.Parameters.AddWithValue("@endYear", endYear);
                    if (shift != "allTeams")
                    {
                        command.Parameters.AddWithValue("@shift", shift);
                    }
                    reader = command.ExecuteReader();
                    CIP.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(queryCOV, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@startYear", startYear);
                    command.Parameters.AddWithValue("@endYear", endYear);
                    if (shift != "allTeams")
                    {
                        command.Parameters.AddWithValue("@shift", shift);
                    }
                    reader = command.ExecuteReader();
                    COV.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(queryBNC, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@startYear", startYear);
                    command.Parameters.AddWithValue("@endYear", endYear);
                    if (shift != "allTeams")
                    {
                        command.Parameters.AddWithValue("@shift", shift);
                    }
                    reader = command.ExecuteReader();
                    BNC.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(queryMachineShutdowns, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@startYear", startYear);
                    command.Parameters.AddWithValue("@endYear", endYear);
                    if (shift != "allTeams")
                    {
                        command.Parameters.AddWithValue("@shift", shift);
                    }
                    reader = command.ExecuteReader();
                    machineShutdowns.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(queryExternalShutdowns, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@startYear", startYear);
                    command.Parameters.AddWithValue("@endYear", endYear);
                    if (shift != "allTeams")
                    {
                        command.Parameters.AddWithValue("@shift", shift);
                    }
                    reader = command.ExecuteReader();
                    externalShutdowns.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(querySeqCips, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@startYear", startYear);
                    command.Parameters.AddWithValue("@endYear", endYear);
                    if (shift != "allTeams")
                    {
                        command.Parameters.AddWithValue("@shift", shift);
                    }
                    reader = command.ExecuteReader();
                    seqCips.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(querySeqCovs, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@startYear", startYear);
                    command.Parameters.AddWithValue("@endYear", endYear);
                    if (shift != "allTeams")
                    {
                        command.Parameters.AddWithValue("@shift", shift);
                    }
                    reader = command.ExecuteReader();
                    seqCovs.Load(reader);
                    reader.Close();
                }



                connection.Close();
            }

            IDictionary<string, DataTable> data = new Dictionary<string, DataTable>()
            {
                { "CIP", CIP },
                { "COV", COV },
                { "BNC", BNC },
                { "machines", machineShutdowns },
                { "external", externalShutdowns },
                { "seqCIP", seqCips },
                { "seqCOV", seqCovs } ,
            };

            return new JsonResult(data);
        }

        [HttpGet("UnplannedDowntimeEventsDate/{productionLine}/{startDate}/{endDate}/{shift}")]
        public JsonResult GetUnplannedDowntimeEventsDate(string productionLine, string startDate, string endDate, string shift)
        {
            startDate += " 00:00:00.000";
            endDate += " 23:59:59.000";

            string queryCIP = @"select *
                                from dbo.ole_unplanned_event_cips cip
                                where cip.productionline = @productionLine
                                and cip.created_at >= @startDate
                                and cip.created_at <= @endDate";

            string queryBNC = @"select *
                                from dbo.ole_unplanned_event_changing_clients bnc
                                where bnc.productionline = @productionLine
                                and bnc.created_at >= @startDate
                                and bnc.created_at <= @endDate";

            string queryCOV = @"select *
                                from dbo.ole_unplanned_event_changing_formats cov
                                where cov.productionline = @productionLine
                                and cov.created_at >= @startDate
                                and cov.created_at <= @endDate";

            string queryMachineShutdowns = @"select *
                                            from dbo.ole_unplanned_event_unplanned_downtimes
                                            where productionline = @productionLine
                                            and implicated_machine != 'other'
                                            and created_at >= @startDate
                                            and created_at <= @endDate";

            string queryExternalShutdowns = @"select *
                                            from dbo.ole_unplanned_event_unplanned_downtimes
                                            where productionline = @productionLine
                                            and implicated_machine = 'other'
                                            and created_at >= @startDate
                                            and created_at <= @endDate";

            string querySeqCips = @"select *
                                    from dbo.ole_unplanned_event_cips cip, dbo.ole_pos pos, dbo.ole_products prod
                                    where cip.productionline = @productionLine
                                    and pos.productionline_name = @productionLine
                                    and cip.created_at >= @startDate
                                    and cip.created_at <= @endDate
                                    and pos.number = cip.OLE
                                    and prod.GMID = pos.GMIDCode";


            string querySeqCovs = @"select *
                                    from dbo.ole_unplanned_event_changing_formats cov, dbo.ole_pos pos, dbo.ole_products prod
                                    where cov.productionline = @productionLine
                                    and pos.productionline_name = @productionLine
                                    and cov.created_at >= @startDate
                                    and cov.created_at <= @endDate
                                    and pos.number = cov.OLE
                                    and prod.GMID = pos.GMIDCode";

            if (shift != "allTeams")
            {
                queryCIP += " and cip.shift = @shift";
                queryCOV += " and cov.shift = @shift";
                queryBNC += " and bnc.shift = @shift";
                queryMachineShutdowns += " and shift = @shift";
                queryExternalShutdowns += " and shift = @shift";
                querySeqCips += " and cip.shift = @shift";
                querySeqCovs += " and cov.shift = @shift";
            }

            DataTable CIP = new DataTable();
            DataTable COV = new DataTable();
            DataTable BNC = new DataTable();
            DataTable machineShutdowns = new DataTable();
            DataTable externalShutdowns = new DataTable();
            DataTable seqCips = new DataTable();
            DataTable seqCovs = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryCIP, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);
                    if (shift != "allTeams")
                    {
                        command.Parameters.AddWithValue("@shift", shift);
                    }
                    reader = command.ExecuteReader();
                    CIP.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(queryCOV, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);
                    if (shift != "allTeams")
                    {
                        command.Parameters.AddWithValue("@shift", shift);
                    }
                    reader = command.ExecuteReader();
                    COV.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(queryBNC, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);
                    if (shift != "allTeams")
                    {
                        command.Parameters.AddWithValue("@shift", shift);
                    }
                    reader = command.ExecuteReader();
                    BNC.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(queryMachineShutdowns, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);
                    if (shift != "allTeams")
                    {
                        command.Parameters.AddWithValue("@shift", shift);
                    }
                    reader = command.ExecuteReader();
                    machineShutdowns.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(queryExternalShutdowns, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);
                    if (shift != "allTeams")
                    {
                        command.Parameters.AddWithValue("@shift", shift);
                    }
                    reader = command.ExecuteReader();
                    externalShutdowns.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(querySeqCips, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);
                    if (shift != "allTeams")
                    {
                        command.Parameters.AddWithValue("@shift", shift);
                    }
                    reader = command.ExecuteReader();
                    seqCips.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(querySeqCovs, connection))
                {
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);
                    if (shift != "allTeams")
                    {
                        command.Parameters.AddWithValue("@shift", shift);
                    }
                    reader = command.ExecuteReader();
                    seqCovs.Load(reader);
                    reader.Close();
                }



                connection.Close();
            }

            IDictionary<string, DataTable> data = new Dictionary<string, DataTable>()
            {
                { "CIP", CIP },
                { "COV", COV },
                { "BNC", BNC },
                { "machines", machineShutdowns },
                { "external", externalShutdowns },
                { "seqCIP", seqCips },
                { "seqCOV", seqCovs } ,
            };

            return new JsonResult(data);
        }

        [HttpGet("allevents/{site}/{productionLine}/{beginningDate}/{endingDate}/{team}")]
        public JsonResult GetAllEventsPeriod(string site, string productionLine, string beginningDate, string endingDate, string team)
        {
            string[] _begDate = beginningDate.Split('-');
            int beginningYear = Int16.Parse(_begDate[0]);
            int beginningMonth = Int16.Parse(_begDate[1]);
            int beginningDay = Int16.Parse(_begDate[2]);

            string[] _endDate = endingDate.Split('-');
            int endingYear = Int16.Parse(_endDate[0]);
            int endingMonth = Int16.Parse(_endDate[1]);
            int endingDay = Int16.Parse(_endDate[2]);

            string startDate = beginningYear + "-" + beginningMonth + "-" + beginningDay + " 00:00:00.000";
            string endDate = endingYear + "-" + endingMonth + "-" + endingDay + " 23:59:59.000";

            string QuerySites = @"Select *
                                from dbo.ole_productionline pl, dbo.worksite w, dbo.ole_pos pos,
                                dbo.ole_products prod, dbo.ole_rejection_counters rc
                                where w.name = pl.worksite_name
                                and pos.productionline_name = pl.productionline_name
                                and pos.GMIDCode = prod.GMID
                                and rc.po = pos.number
                                and w.name = @site
                                and pl.productionline_name = @productionLine
                                and pos.created_at >= @beginningDate
                                and pos.created_at <= @endingDate
                                and pos.shift = rc.shift";


            if (team != "allTeams")
            {
                QuerySites += " and pos.shift = @team";
            }


            DataTable Sites = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QuerySites, connection))
                {
                    command.Parameters.AddWithValue("@site", site);
                    command.Parameters.AddWithValue("@productionLine", productionLine);
                    command.Parameters.AddWithValue("@beginningDate", startDate);
                    command.Parameters.AddWithValue("@endingDate", endDate);
                    if (team != "allTeams")
                    {
                        command.Parameters.AddWithValue("@team", team);
                    }
                    reader = command.ExecuteReader();
                    Sites.Load(reader);
                    reader.Close();
                }

                if (Sites.Rows.Count > 0)
                {
                    string QuerySpeedLossesEvents = @"select sl.duration, sl.reason, sl.comment, pos.id, pos.qtyProduced,
                                                      pos.workingDuration, prod.size, prod.idealRate
                                                      from dbo.ole_speed_losses sl, dbo.ole_pos pos, dbo.ole_products prod
                                                      where pos.number = sl.OLE
                                                      and prod.GMID = pos.GMIDCode
                                                      and pos.shift = sl.shift
                                                      and sl.productionline = @productionlineName
                                                      and sl.created_at >= @startDate
                                                      and sl.created_at <= @endDate";

                    string QueryBM = @"select isnull(sum(pe.duration),0) as Duration, count(*) as nbEvents
                                       from dbo.ole_planned_events pe
                                       where pe.productionline = @productionlineName
                                       and pe.created_at >= @startDate
                                       and pe.created_at <= @endDate
                                       and (pe.reason = 'break' or pe.reason = 'meeting'
                                       or pe.reason = 'noProductionPlanned')";

                    string QueryCP = @"select isnull(sum(pe.duration),0) as Duration, count(*) as nbEvents
                                       from dbo.ole_planned_events pe
                                       where pe.productionline = @productionlineName
                                       and pe.created_at >= @startDate
                                       and pe.created_at <= @endDate
                                       and pe.reason = 'projectImplementation'";

                    string QueryPM = @"select isnull(sum(pe.duration),0) as Duration, count(*) as nbEvents
                                       from dbo.ole_planned_events pe
                                       where pe.productionline = @productionlineName
                                       and pe.created_at >= @startDate
                                       and pe.created_at <= @endDate
                                       and pe.reason = 'maintenance'";

                    string QueryPP = @"select isnull(sum(pe.duration),0) as Duration, count(*) as nbEvents
                                       from dbo.ole_planned_events pe
                                       where pe.productionline = @productionlineName
                                       and pe.created_at >= @startDate
                                       and pe.created_at <= @endDate
                                       and pe.reason = 'noProductionPlanned'";

                    string QueryCIP = @"select isnull(sum(cip.total_duration),0) as Duration, count(*) as nbEvents
                                       from dbo.ole_unplanned_event_cips cip
                                       where cip.productionline = @productionlineName
                                       and cip.created_at >= @startDate
                                       and cip.created_at <= @endDate";

                    string QueryCOV = @"select isnull(sum(cov.total_duration),0) as Duration, count(*) as nbEvents
                                       from dbo.ole_unplanned_event_changing_clients cov
                                       where cov.productionline = @productionlineName
                                       and cov.created_at >= @startDate
                                       and cov.created_at <= @endDate";

                    string QueryBNC = @"select isnull(sum(bnc.total_duration),0) as Duration, count(*) as nbEvents
                                       from dbo.ole_unplanned_event_changing_formats bnc
                                       where bnc.productionline = @productionlineName
                                       and bnc.created_at >= @startDate
                                       and bnc.created_at <= @endDate";

                    string QueryUEE = @"select isnull(sum(ud.total_duration),0) as Duration, count(*) as nbEvents
                                       from ole_unplanned_event_unplanned_downtimes ud
                                        where ud.implicated_machine = 'other'
                                        and ud.productionline = @productionlineName 
                                        and ud.created_at >= @startDate
                                        and ud.created_at <= @endDate
                                       ";

                    string QueryUSM = @"select isnull(sum(ud.total_duration),0) as Duration, count(*) as nbEvents
                                        from ole_unplanned_event_unplanned_downtimes ud
                                        where ud.implicated_machine != 'other'
                                        and ud.implicated_machine != 'filler' 
                                        and ud.productionline = @productionlineName 
                                        and ud.created_at >= @startDate
                                        and ud.created_at <= @endDate";

                    string QueryFUS = @"select isnull(sum(ud.total_duration),0) as Duration, count(*) as nbEvents
                                       from ole_unplanned_event_unplanned_downtimes ud
                                        where ud.implicated_machine = 'filler'
                                        and ud.productionline = @productionlineName 
                                        and ud.created_at >= @startDate
                                        and ud.created_at <= @endDate";

                    string QueryRRF = @"select isnull(sum(sl.duration),0) as Duration, count(*) as nbEvents
                                       from dbo.ole_speed_losses sl
                                       where sl.productionline = @productionlineName
                                       and sl.created_at >= @startDate
                                       and sl.created_at <= @endDate
                                       and sl.reason = 'reducedRateAtFiller'";

                    string QueryRRFMonth = @"select *
                                            from dbo.ole_speed_losses sl
                                            where sl.productionline = @productionlineName
                                            and sl.created_at >= @startDate
                                            and sl.created_at <= @endDate
                                            and sl.reason = 'reducedRateAtFiller'";

                    string QueryRRMMonth = @"select *
                                            from dbo.ole_speed_losses sl
                                            where sl.productionline = @productionlineName
                                            and sl.created_at >= @startDate
                                            and sl.created_at <= @endDate
                                            and sl.reason = 'reducedRateAtAnOtherMachine'";

                    string QueryFOSMonth = @"select *
                                            from dbo.ole_speed_losses sl
                                            where sl.productionline = @productionlineName
                                            and sl.created_at >= @startDate
                                            and sl.created_at <= @endDate
                                            and sl.reason = 'fillerOwnStoppage'";

                    string QueryFSMMonth = @"select *
                                            from dbo.ole_speed_losses sl
                                            where sl.productionline = @productionlineName
                                            and sl.created_at >= @startDate
                                            and sl.created_at <= @endDate
                                            and sl.reason = 'fillerOwnStoppageByAnOtherMachine'";

                    string QueryRRM = @"select isnull(sum(sl.duration),0) as Duration, count(*) as nbEvents
                                        from dbo.ole_speed_losses sl
                                        where sl.productionline = @productionlineName
                                        and sl.created_at >= @startDate
                                        and sl.created_at <= @endDate
                                        and sl.reason = 'reducedRateAtAnOtherMachine'";

                    string QueryFOS = @"select isnull(sum(sl.duration),0) as Duration, count(*) as nbEvents
                                        from dbo.ole_speed_losses sl
                                        where sl.productionline = @productionlineName
                                        and sl.created_at >= @startDate
                                        and sl.created_at <= @endDate
                                        and sl.reason = 'fillerOwnStoppage'";

                    string QueryFSM = @"select isnull(sum(sl.duration),0) as Duration, count(*) as nbEvents
                                        from dbo.ole_speed_losses sl
                                        where sl.productionline = @productionlineName
                                        and sl.created_at >= @startDate
                                        and sl.created_at <= @endDate
                                        and sl.reason = 'fillerOwnStoppageByAnOtherMachine'";

                    string QueryPlannedEvents = @"select *
                                                  from dbo.ole_planned_events pe
                                                  where pe.productionline = @productionlineName
                                                  and pe.created_at >= @startDate
                                                  and pe.created_at <= @endDate";

                    string QueryChangingClients = @"select *
                                                  from dbo.ole_unplanned_event_changing_clients cov
                                                  where cov.productionline = @productionlineName
                                                  and cov.created_at >= @startDate
                                                  and cov.created_at <= @endDate";

                    string QueryCIPBis = @"select *
                                           from dbo.ole_unplanned_event_cips cip
                                           where cip.productionline = @productionlineName
                                           and cip.created_at >= @startDate
                                           and cip.created_at <= @endDate";

                    string QueryUnplanned = @"select *
                                           from dbo.ole_unplanned_event_unplanned_downtimes ud
                                           where ud.productionline = @productionlineName
                                           and ud.created_at >= @startDate
                                           and ud.created_at <= @endDate";

                    string QueryChangingFormats = @"select *
                                                   from dbo.ole_unplanned_event_changing_formats bnc
                                                   where bnc.productionline = @productionlineName
                                                   and bnc.created_at >= @startDate
                                                   and bnc.created_at <= @endDate";

                    if (team != "allTeams")
                    {
                        QuerySpeedLossesEvents += " and sl.shift = @team";
                        QueryBM += " and pe.shift = @team";
                        QueryCP += " and pe.shift = @team";
                        QueryPM += " and pe.shift = @team";
                        QueryPP += " and pe.shift = @team";
                        QueryCIP += " and cip.shift = @team";
                        QueryCOV += " and cov.shift = @team";
                        QueryBNC += " and bnc.shift = @team";
                        QueryUEE += " and ud.shift = @team";
                        QueryUSM += " and ud.shift = @team";
                        QueryFUS += " and ud.shift = @team";
                        QueryRRF += " and sl.shift = @team";
                        QueryRRFMonth += " and sl.shift = @team";
                        QueryRRMMonth += " and sl.shift = @team";
                        QueryFOSMonth += " and sl.shift = @team";
                        QueryFSMMonth += " and sl.shift = @team";
                        QueryRRM += " and sl.shift = @team";
                        QueryFOS += " and sl.shift = @team";
                        QueryFSM += " and sl.shift = @team";
                        QueryPlannedEvents += " and pe.shift = @team";
                        QueryChangingClients += " and cov.shift = @team";
                        QueryCIPBis += " and cip.shift = @team";
                        QueryUnplanned += " and ud.shift = @team";
                        QueryChangingFormats += " and bnc.shift = @team";
                    }

                    IDictionary<string, DataTable> Queries = new Dictionary<string, DataTable>()
                    {
                        { QuerySpeedLossesEvents, new DataTable() },
                        { QueryBM, new DataTable() },
                        { QueryCP, new DataTable() },
                        { QueryPM, new DataTable() },
                        { QueryPP, new DataTable() },
                        { QueryCIP, new DataTable() },
                        { QueryCOV, new DataTable() },
                        { QueryBNC, new DataTable() },
                        { QueryUEE, new DataTable() },
                        { QueryUSM, new DataTable() },
                        { QueryFUS, new DataTable() },
                        { QueryRRF, new DataTable() },
                        { QueryRRFMonth, new DataTable() },
                        { QueryRRMMonth, new DataTable() },
                        { QueryFOSMonth, new DataTable() },
                        { QueryFSMMonth, new DataTable() },
                        { QueryRRM, new DataTable() },
                        { QueryFOS, new DataTable() },
                        { QueryFSM, new DataTable() },
                        { QueryPlannedEvents, new DataTable() },
                        { QueryChangingClients, new DataTable() },
                        { QueryCIPBis, new DataTable() },
                        { QueryUnplanned, new DataTable() },
                        { QueryChangingFormats, new DataTable() }
                    };

                    foreach (KeyValuePair<string, DataTable> Entry in Queries)
                    {
                        using (SqlCommand command = new SqlCommand(Entry.Key, connection))
                        {
                            command.Parameters.AddWithValue("@productionlineName", productionLine);
                            command.Parameters.AddWithValue("@startDate", startDate);
                            command.Parameters.AddWithValue("@endDate", endDate);
                            if (team != "allTeams")
                            {
                                command.Parameters.AddWithValue("@team", team);
                            }
                            reader = command.ExecuteReader();
                            Entry.Value.Load(reader);
                            reader.Close();
                        }
                    }

                    connection.Close();
                    return new JsonResult(new Dictionary<string, DataTable>()
                    {
                        { "BM", Queries[QueryBM] },
                        { "CP", Queries[QueryCP] },
                        { "PM", Queries[QueryPM] },
                        { "PP", Queries[QueryPP] },
                        { "CIP", Queries[QueryCIP] },
                        { "COV", Queries[QueryCOV] },
                        { "BNC", Queries[QueryBNC] },
                        { "UEE", Queries[QueryUEE] },
                        { "USM", Queries[QueryUSM] },
                        { "FUS", Queries[QueryFUS] },
                        { "RRF", Queries[QueryRRF] },
                        { "RRM", Queries[QueryRRM] },
                        { "FOS", Queries[QueryFOS] },
                        { "FSM", Queries[QueryFSM] },
                        { "SITE", Sites },
                        { "EVENTS", Queries[QueryChangingFormats] },
                        { "SLEVENTS", Queries[QuerySpeedLossesEvents] },
                        { "PLANNEDEVENTS", Queries[QueryPlannedEvents] },
                        { "UNPLANNEDEVENTS", Queries[QueryUnplanned] },
                        { "CIPEVENTS", Queries[QueryCIPBis] },
                        { "LOTCHANGING", Queries[QueryChangingClients] },
                        { "RRFMonth", Queries[QueryRRFMonth] },
                        { "RRMMonth", Queries[QueryRRMMonth] },
                        { "FOSMonth", Queries[QueryFOSMonth] },
                        { "FSMMonth", Queries[QueryFSMMonth] }
                    });
                }
                else
                {
                    connection.Close();
                    return new JsonResult(new Dictionary<string, int>()
                    {
                        { "BM", 0 },
                        { "CP", 0 },
                        { "PM", 0 },
                        { "PP", 0 },
                        { "CIP", 0 },
                        { "COV", 0 },
                        { "BNC", 0 },
                        { "UEE", 0 },
                        { "USM", 0 },
                        { "FUS", 0 },
                        { "RRF", 0 },
                        { "RRM", 0 },
                        { "FOS", 0 },
                        { "FSM", 0 },
                        { "SITE", 0 },
                        { "EVENTS", 0 }
                    });
                }
            }
        }

        [HttpGet("assignation/{username}/{po}/{productionline}")]
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


        [HttpPost("assignation")]
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

        [HttpPost("PO")]
        public JsonResult CreatePO(PO po)
        {
            string QueryNewPO = @"insert into dbo.ole_pos (number, GMIDCode, productionline_name)
                                  values (@Number, @GMIDCode, @ProductionLine)";


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
                    reader = command.ExecuteReader();
                    NewPO.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(NewPO);
        }

        [HttpPost("unplannedEvent/changingFormat")]
        public JsonResult SaveUnplannedEventChangingFormat(UnplannedEventChangingFormat ue)
        {
            string QuerySaveUECF = @"insert into dbo.ole_unplanned_event_changing_formats
                                   (created_at, OLE, productionline, predicted_duration, total_duration, comment, shift)
                                   values (@created_at, @OLE, @PL, @PD, @TD, @C, @shift)";


            DataTable SaveUECF = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QuerySaveUECF, connection))
                {
                    command.Parameters.AddWithValue("@created_at", ue.created_at);
                    command.Parameters.AddWithValue("@OLE", ue.OLE);
                    command.Parameters.AddWithValue("@PL", ue.productionline);
                    command.Parameters.AddWithValue("@PD", ue.predicted_duration);
                    command.Parameters.AddWithValue("@TD", ue.total_duration);
                    command.Parameters.AddWithValue("@C", ue.comment);
                    command.Parameters.AddWithValue("@shift", ue.shift);

                    reader = command.ExecuteReader();
                    SaveUECF.Load(reader);
                    reader.Close();
                }


                connection.Close();
            }

            return new JsonResult(SaveUECF);
        }

        [HttpPost("unplannedEvent/clientChanging")]
        public JsonResult SaveUnplannedEventChangingClient(UnplannedEventChangingClient ue)
        {
            string QuerySaveUECC = @"insert into dbo.ole_unplanned_event_changing_clients
                                   (created_at, OLE, productionline, predicted_duration, total_duration, comment, lot_number, shift)
                                   values (@created_at, @OLE, @PL, @PD, @TD, @C, @LN, @shift)";


            DataTable SaveUECC = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QuerySaveUECC, connection))
                {
                    command.Parameters.AddWithValue("@created_at", ue.created_at);
                    command.Parameters.AddWithValue("@OLE", ue.OLE);
                    command.Parameters.AddWithValue("@PL", ue.productionline);
                    command.Parameters.AddWithValue("@PD", ue.predicted_duration);
                    command.Parameters.AddWithValue("@TD", ue.total_duration);
                    command.Parameters.AddWithValue("@C", ue.comment);
                    command.Parameters.AddWithValue("@LN", ue.lot_number);
                    command.Parameters.AddWithValue("@shift", ue.shift);
                    reader = command.ExecuteReader();
                    SaveUECC.Load(reader);
                    reader.Close();
                }


                connection.Close();
            }

            return new JsonResult(SaveUECC);
        }

        [HttpPost("unplannedEvent/CIP")]
        public JsonResult SaveUnplannedEventCIP(UnplannedEventCIP ue)
        {
            string QuerySaveUECIP = @"insert into dbo.ole_unplanned_event_cips
                                   (created_at, OLE, productionline, predicted_duration, total_duration, comment, previous_bulk, shift, finished)
                                   values (@created_at, @OLE, @PL, @PD, @TD, @C, @PB, @shift, @finished)";


            DataTable SaveUECIP = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QuerySaveUECIP, connection))
                {
                    command.Parameters.AddWithValue("@created_at", ue.created_at);
                    command.Parameters.AddWithValue("@OLE", ue.OLE);
                    command.Parameters.AddWithValue("@PL", ue.productionline);
                    command.Parameters.AddWithValue("@PD", ue.predicted_duration);
                    command.Parameters.AddWithValue("@TD", ue.total_duration);
                    command.Parameters.AddWithValue("@C", ue.comment);
                    command.Parameters.AddWithValue("@PB", ue.previous_bulk);
                    command.Parameters.AddWithValue("@shift", ue.shift);
                    command.Parameters.AddWithValue("@finished", ue.finished);
                    reader = command.ExecuteReader();
                    SaveUECIP.Load(reader);
                    reader.Close();
                }


                connection.Close();
            }

            return new JsonResult(SaveUECIP);
        }

        [HttpPost("unplannedEvent/unplannedDowntime")]
        public JsonResult SaveUnplannedEventUnplannedDowntime(UnplannedEventUnplannedDowntime ue)
        {

            string QuerySaveUEUD = @"insert into dbo.ole_unplanned_event_unplanned_downtimes
                                   (created_at, updated_at, OLE, productionline, implicated_machine, total_duration, comment, component, shift)
                                   values (@created_at, @created_at, @OLE, @PL, @IM, @TD, @COMM, @COMP, @shift)";


            DataTable SaveUEUD = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QuerySaveUEUD, connection))
                {
                    command.Parameters.AddWithValue("@created_at", ue.created_at);
                    command.Parameters.AddWithValue("@OLE", ue.OLE);
                    command.Parameters.AddWithValue("@PL", ue.productionline);
                    command.Parameters.AddWithValue("@COMP", ue.component);
                    command.Parameters.AddWithValue("@TD", ue.total_duration);
                    command.Parameters.AddWithValue("@COMM", ue.comment);
                    command.Parameters.AddWithValue("@IM", ue.implicated_machine);
                    command.Parameters.AddWithValue("@shift", ue.shift);
                    reader = command.ExecuteReader();
                    SaveUEUD.Load(reader);
                    reader.Close();
                }


                connection.Close();
            }

            return new JsonResult(SaveUEUD);
        }

        [HttpPost("plannedEvent")]
        public JsonResult SavePlannedEvent(PlannedEvent pe)
        {
            string QuerySavePE = @"insert into dbo.ole_planned_events
                                   (created_at, OLE, productionline, reason, duration, comment, shift)
                                   values (@created_at, @OLE, @PL, @R, @D, @COMM, @shift)";


            DataTable SavePE = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QuerySavePE, connection))
                {
                    command.Parameters.AddWithValue("@created_at", pe.created_at);
                    command.Parameters.AddWithValue("@OLE", pe.OLE);
                    command.Parameters.AddWithValue("@PL", pe.productionline);
                    command.Parameters.AddWithValue("@R", pe.reason);
                    command.Parameters.AddWithValue("@D", pe.duration);
                    command.Parameters.AddWithValue("@COMM", pe.comment);
                    command.Parameters.AddWithValue("@shift", pe.shift);
                    reader = command.ExecuteReader();
                    SavePE.Load(reader);
                    reader.Close();
                }


                connection.Close();
            }

            return new JsonResult(SavePE);
        }

        [HttpGet("generalUnplannedData/{pl}/{from}/{to}")]
        public JsonResult generalUnplannedData(string pl, string from, string to)
        {
            from += " 00:00:00.000";
            to += " 23:59:59.000";

            string TotalDurationQuery = @"select sum(cip.total_duration) as duration
                                       from dbo.ole_unplanned_event_cips cip
                                       where cip.productionline = @pl
                                       and cip.created_at >= @from
                                       and cip.created_at <= @to";
            
            string CountQuery = @"select count(*) as eventCount
                            from dbo.ole_unplanned_event_cips cip
                            where cip.productionline = @pl
                            and cip.created_at >= @from
                            and cip.created_at <= @to
                            and cip.finished = 1";


            DataTable TotalDuration = new DataTable();
            DataTable Count = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(TotalDurationQuery, connection))
                {
                    command.Parameters.AddWithValue("@pl", pl);
                    command.Parameters.AddWithValue("@from", from);
                    command.Parameters.AddWithValue("@to", to);
                    reader = command.ExecuteReader();
                    TotalDuration.Load(reader);
                    reader.Close();
                }

                using (SqlCommand command = new SqlCommand(CountQuery, connection))
                {
                    command.Parameters.AddWithValue("@pl", pl);
                    command.Parameters.AddWithValue("@from", from);
                    command.Parameters.AddWithValue("@to", to);
                    reader = command.ExecuteReader();
                    Count.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(new Dictionary<string, DataTable>()
                {
                    { "totalDuration", TotalDuration },
                    { "count", Count }
                });
        }

        

        [HttpDelete("deleteUnplannedDowntime/{id}")]
        public JsonResult DeleteUnplannedDowntime(int id)
        {
            string queryDelete = @"delete from dbo.ole_unplanned_event_unplanned_downtimes 
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

        [HttpDelete("deletePackNumberChanging/{id}")]
        public JsonResult DeletePackNumberChanging(int id)
        {
            string queryDelete = @"delete from dbo.ole_unplanned_event_changing_clients 
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

        [HttpDelete("deleteFormatChanging/{id}")]
        public JsonResult DeleteFormatChanging(int id)
        {
            string queryDelete = @"delete from dbo.ole_unplanned_event_changing_formats 
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

        [HttpDelete("deletePlannedEvent/{id}")]
        public JsonResult DeletePlannedEvent(int id)
        {
            string queryDelete = @"delete from dbo.ole_planned_events 
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
    }
}