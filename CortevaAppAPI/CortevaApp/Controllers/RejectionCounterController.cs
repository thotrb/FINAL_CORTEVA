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
    public class RejectionCounterController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public RejectionCounterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost("storeRejection")]
        public JsonResult StoreRejection(RejectionCounters rc)
        {
         

            string QueryNewStoreRejection = @"IF EXISTS(select * from ole_rejection_counters rc where rc.PO = @Po and rc.shift = @shift)
                                                   update ole_rejection_counters set fillerCounter = @FillerCounter, 
                                                          caperCounter = @CaperCounter, labelerCounter = @LabelerCounter, 
                                                          weightBoxCounter = @WeightBoxCounter, qualityControlCounter = @QualityControlCounter,
                                                          fillerRejection = @FillerRejection, caperRejection = @CaperRejection, 
                                                          labelerRejection = @LabelerRejection, weightBoxRejection = @WeightBoxRejection,
                                                          qualityControlRejection = @QualityControlRejection, created_at = @created_at
                                                          where PO = @Po and shift = @shift
                                                ELSE
                                                  insert into dbo.ole_rejection_counters (created_at, po, fillerCounter, caperCounter,
                                                      labelerCounter, weightBoxCounter, qualityControlCounter,
                                                      fillerRejection, caperRejection, labelerRejection, weightBoxRejection,
                                                      qualityControlRejection, shift)
                                                      values (@created_at, @Po, @FillerCounter, @CaperCounter, @LabelerCounter, @WeightBoxCounter,
                                                      @QualityControlCounter, @FillerRejection, @CaperRejection, @LabelerRejection,
                                                      @WeightBoxRejection, @QualityControlRejection, @shift)";


            DataTable NewStoreRejection = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("CortevaDBConnection");
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(sqlDataSource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QueryNewStoreRejection, connection))
                {
                    command.Parameters.AddWithValue("@Po", rc.po);
                    command.Parameters.AddWithValue("@FillerCounter", rc.fillerCounter);
                    command.Parameters.AddWithValue("@CaperCounter", rc.caperCounter);
                    command.Parameters.AddWithValue("@LabelerCounter", rc.labelerCounter);
                    command.Parameters.AddWithValue("@WeightBoxCounter", rc.weightBoxCounter);
                    command.Parameters.AddWithValue("@QualityControlCounter", rc.qualityControlCounter);
                    command.Parameters.AddWithValue("@FillerRejection", rc.fillerRejection);
                    command.Parameters.AddWithValue("@CaperRejection", rc.caperRejection);
                    command.Parameters.AddWithValue("@LabelerRejection", rc.labelerRejection);
                    command.Parameters.AddWithValue("@WeightBoxRejection", rc.weightBoxRejection);
                    command.Parameters.AddWithValue("@QualityControlRejection", rc.qualityControlRejection);
                    command.Parameters.AddWithValue("@shift", rc.shift);
                    command.Parameters.AddWithValue("@created_at", rc.created_at);

                    reader = command.ExecuteReader();
                    NewStoreRejection.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }

            return new JsonResult(NewStoreRejection);
        }
    }
}