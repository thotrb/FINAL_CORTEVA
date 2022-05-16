<template>
  <div class="" id="component">
    <div class="row">
      <div class="col-sm">
        <div class="data">
            <div>

            <form>
              <label class="" for="site">{{$t("site")}} : </label>
              <select name="site" id="site" class="form-select" v-model="site" @change="team = ''">
                  <option  v-for="site in dataWorksite" :key="site.name" v-bind:value="site.name">
                    {{site.name}}
                  </option>
              </select>
            </form>
            </div>
            <div>

            <form>
              <label class="" for="productionline">{{$t("productionLine")}} : </label>
              <select name="productionline" id="productionline" class="form-select"
                      v-model="productionline" @change="team = 'allTeams'">
                <template v-for="productionline in dataProductionlines">
                  <template v-if="productionline.name === site">
                    <option v-bind:key="productionline.productionline_name" v-bind:value="productionline.productionline_name">
                        {{productionline.productionline_name}}
                      </option>
                  </template>
                </template>
              </select>
            </form>
            </div>
            <div>
            <form>
              <label for="team-select">{{$t("team")}}: </label>
              <select name="team" id="team-select" class="form-select" v-model="team">
                <option value="allTeams" id="all-teams-option">{{$t("allTeams")}}</option>
                <template v-for="team in dataTeams">
                  <template v-if="team.worksite_name === site">
                    <option v-bind:key="team.type" v-bind:value="team.type">{{team.type}}</option>
                  </template>
                </template> 
              </select>
            </form>
  
            </div>
          <div>
            <input v-on:click="load()" type="button" class="btn btn-outline-info" v-bind:value="lo">
          </div>
        </div>

        <br/>

        <template v-if="show===1">


          <div class="row">

            <div class="col-sm">
              <p>
                {{$t("plantOperatingTime")}} : {{sommeWorkingTime}}<!-- {{ allEvents['POInfo'][0].plantOperatingTime}}--> mn
                <br/>
                {{$t("plannedProductionTime")}} :
                {{plannedProductionTime}}
                mn <br/>
              </p>
            </div>

            <div class="col-sm">

              <p>
                {{$t("volumePacked")}} : {{littersProduced}} L / Kg<br/>
                {{$t("numberOfProductionOrder")}} : {{ allEvents['SITE'].length}} <br/>
                {{$t("numberOfItemsProduced")}} : {{ qtyProduced }} {{$t("bottles")}} <br/>

              </p>

            </div>
          </div>


          <div class="">
            <table class="table">
              <thead>
              <tr>
                <th scope="col"> {{$t("plannedDowntime")}} ({{$t("prioritizeList")}})</th>
                <th scope="col">{{$t("duration(Minutes)")}}</th>
              </tr>
              </thead>
              <tbody>

              <tr>
                <th scope="row">1. {{$t("noProductionPlanned")}} (PP)</th>
                <template v-if="allEvents['PP'] === 0 || allEvents['PP'][0].Duration === null">
                  <td>0 mn</td>
                </template>
                <template v-else>
                  <td>{{allEvents['PP'][0].Duration}} mn</td>
                </template>

              </tr>
              <tr>
                <th scope="row">2. {{$t("plannedMaintenanceActivites")}} (PM)</th>
                <template v-if="allEvents['PM'] === 0 || allEvents['PM'][0].Duration === null">
                  <td>0 mn</td>
                </template>
                <template v-else>
                  <td>{{allEvents['PM'][0].Duration}} mn</td>
                </template>
              </tr>
              <tr>
                <th scope="row">3. {{$t("capitalProjectImplementation")}} (CP)</th>
                <template v-if="allEvents['CP'] === 0 || allEvents['CP'][0].Duration === null">
                  <td>0 mn</td>
                </template>
                <template v-else>
                  <td>{{allEvents['CP'][0].Duration}} mn</td>
                </template>
              </tr>
              <tr>
                <th scope="row">4. {{$t("breaksMeetingShiftChange")}} (BM)</th>
                  <td>{{plannedDowntimes - allEvents['PP'][0].Duration - allEvents['PM'][0].Duration - allEvents['CP'][0].Duration}} mn</td>
              </tr>

              </tbody>
            </table>
          </div>


          <div class="">
            <table class="table">
              <thead>
              <tr>
                <th scope="col">{{$t("unplannedDowntime")}}</th>
                <th scope="col">{{$t("duration(Minutes)")}}</th>
                <th scope="col">{{$t("numberOfEvents")}}</th>
              </tr>
              </thead>
              <tbody>
              <tr>
                <th scope="row">1. {{$t("cleaningInPlace")}} (CIP)</th>
                <template v-if="allEvents['CIP'] === 0 || allEvents['CIP'][0].Duration === null">
                  <td>0 mn</td>
                  <td>0</td>

                </template>
                <template v-else>
                  <td>{{allEvents['CIP'][0].Duration}} mn</td>
                  <td>{{allEvents['CIP'][0].nbEvents}}</td>
                </template>


              </tr>
              <tr>
                <th scope="row">2. {{$t("changeOver")}} (COV)</th>
                <template v-if="allEvents['COV'] === 0 || allEvents['COV'][0].Duration === null">
                  <td>0 mn</td>
                  <td>0</td>

                </template>
                <template v-else>
                  <td>{{allEvents['COV'][0].Duration}} mn</td>
                  <td>{{allEvents['COV'][0].nbEvents}}</td>
                </template>


              </tr>
              <tr>
                <th scope="row">3. {{$t("batchNumberChange")}} (BNC)</th>
                <template v-if="allEvents['BNC'] === 0 || allEvents['BNC'][0].Duration === null">
                  <td>0 mn</td>
                  <td>0</td>

                </template>
                <template v-else>
                  <td>{{allEvents['BNC'][0].Duration}} mn</td>
                  <td>{{allEvents['BNC'][0].nbEvents}}</td>
                </template>


              </tr>
              <tr>
                <th scope="row">4. {{$t("unplannedExternalEvents")}} (UEE)</th>
                <template v-if="allEvents['UEE'] === 0 || allEvents['UEE'][0].Duration === null">
                  <td>0 mn</td>
                  <td>0</td>

                </template>
                <template v-else>
                  <td>{{allEvents['UEE'][0].Duration}} mn</td>
                  <td>{{allEvents['UEE'][0].nbEvents}}</td>
                </template>


              </tr>
              <tr>
                <th scope="row">5. {{$t("unplannedShutdownOfMachine")}} (USM)</th>
                <template v-if="allEvents['USM'] === 0 || allEvents['USM'][0].Duration === null">
                  <td>0 mn</td>
                  <td>0</td>
                </template>
                <template v-else>
                  <td>{{allEvents['USM'][0].Duration}} mn</td>
                  <td>{{allEvents['USM'][0].nbEvents}}</td>
                </template>


              </tr>
              <tr>
                <th scope="row">6. {{$t("fillerUnplannedShutdown")}} (UEE)</th>
                <template v-if="allEvents['FUS'] === 0 || allEvents['FUS'][0].Duration === null">
                  <td>0 mn</td>
                  <td>0</td>

                </template>
                <template v-else>
                  <td>{{allEvents['FUS'][0].Duration}} mn</td>
                  <td>{{allEvents['FUS'][0].nbEvents}}</td>
                </template>

              </tr>

              </tbody>
            </table>
          </div>

          <br/>

          <div class="">
            <table class="table">
              <thead>
              <tr>
                <th scope="col">{{$t("speedlosses")}}</th>
                <th scope="col">{{$t("duration(Minutes)")}}</th>
                <th scope="col">{{$t("numberOfEvents")}}</th>
              </tr>
              </thead>
              <tbody>
              <tr>
                <th scope="row">1. {{$t("reducedRateAtFiller")}} (RRF)</th>
                <template v-if="allEvents['RRF'] === 0 || allEvents['RRF'][0].Duration === null">
                  <td>0 mn</td>
                  <td>0</td>

                </template>
                <template v-else>
                  <td>{{allEvents['RRF'][0].Duration}} mn</td>
                  <td>{{allEvents['RRF'][0].nbEvents}}</td>
                </template>


              </tr>
              <tr>
                <th scope="row">2. {{$t("reducedRateAtAnOtherMachine")}} (RRM)</th>
                <template v-if="allEvents['RRM'] === 0 || allEvents['RRM'][0].Duration === null">
                  <td>0 mn</td>
                  <td>0</td>

                </template>
                <template v-else>
                  <td>{{allEvents['RRM'][0].Duration}} mn</td>
                  <td>{{allEvents['RRM'][0].nbEvents}}</td>
                </template>


              </tr>
              <tr>
                <th scope="row">3. {{$t("fillerOwnStoppage")}} (FOS)</th>
                <template v-if="allEvents['FOS'] === 0 || allEvents['FOS'][0].Duration === null">
                  <td>0 mn</td>
                  <td>0</td>
                </template>
                <template v-else>
                  <td>{{allEvents['FOS'][0].Duration}} mn</td>
                  <td>{{allEvents['FOS'][0].nbEvents}}</td>
                </template>

              </tr>
              <tr>
                <th scope="row">4. {{$t("fillerOwnStoppageByAnOtherMachine")}}(FSM)</th>
                <template v-if="allEvents['FSM'] === 0 || allEvents['FSM'][0].Duration === null">
                  <td>0 mn</td>
                  <td>0</td>

                </template>
                <template v-else>
                  <td>{{allEvents['FSM'][0].Duration}} mn</td>
                  <td>{{allEvents['FSM'][0].nbEvents}}</td>
                </template>

              </tr>
              </tbody>
            </table>
          </div>
        </template>
      </div>

      <div class="col-sm">
        <div class="data">

          <div>
            <h1>
              {{$t("downtimesReport")}}
            </h1>
          </div>

          <div class="">
            <label class="" for="startingPO">{{$t("from")}}</label>
            <input type="date" id="startingPO" class=" " required v-model="beginningDate">


            <label class="" for="endingPO">{{$t("to")}}</label>
            <input type="date" id="endingPO" class=""
                   required v-model="endingDate">
          </div>

        </div>


        <template v-if="show===1">

          <div id="squareZone">

            <div class="row">
              <div class="col-sm">
                <canvas id="can" width="100" height="100"/>
                <h5>{{$t("packSizeSplit")}}</h5>


              </div>

              <div class="col-sm">
                <canvas id="can2" width="100" height="100"/>
                <h5>{{$t("formVolumeSplit")}}</h5>

              </div>

            </div>

            <br/>

            <h5>{{$t("plantOperatingTimeOverview")}} </h5>
            <div class="row rect" id="rect1">
              <p class="blueBack">
                {{$t("plannedProductionTime")}} (PPT)
              </p>
              <p class="greenBack">
                {{$t("plannedDowntime")}} (PD)
              </p>
              <p v-if="plannedProductionTime === 0">
                0.00%

              </p>
              <p v-else>

                {{(plannedDowntimes / sommeWorkingTime * 100).toFixed(2)}}%
              </p>
            </div>


            <div class="row rect" id="rect2">
              <p class="blueBack">
                {{$t("operatingTime")}} (OT)
              </p>
              <p class="redBack">
                {{$t("unplannedDowntime")}} (UD)
              </p>
              <p v-if="operatingTime ===0 ">
                0.00%
              </p>
              <p v-else>
                {{(unplannedDowntimes / sommeWorkingTime * 100).toFixed(2)}}%
              </p>
            </div>

            <div class="row rect" id="rect3">
              <p class="blueBack">
                {{$t("netOperatingTime")}} (NOT)
              </p>
              <p class="redBack">
                {{$t("speedLosses")}} (SL)
              </p>

              <p v-if="speedLosses ===0 ">
                0.00%
              </p>
              <p v-else>
                {{((speedLosses / sommeWorkingTime) * 100).toFixed(2)}}%
              </p>
            </div>

            <div class="row rect" id="rect4">
              <p class="blueBack">
                {{$t("valuableOperatingTime")}} (VOT)
              </p>
              <p class="redBack">
                {{$t("qualityLosses")}} (QL)
              </p>
              <p>
                {{(this.QL / this.sommeWorkingTime* 100).toFixed(2)}}%
              </p>
            </div>

          </div>
        </template>

      </div>
    </div>

    <div class="row zoneDessin">
      <div class="col-sm">
        <canvas id="Availability">
        </canvas>
        <template v-if="show===1">
          <h5>{{$t("availability")}}</h5>
        </template>
      </div>

      <div class="col-sm">
        <canvas id="Performance">
        </canvas>
        <template v-if="show===1">
          <h5>{{$t("performance")}}</h5>
        </template>
      </div>


      <div class="col-sm">
        <canvas id="Quality">
        </canvas>
        <template v-if="show===1">
          <h5>{{$t("quality")}}</h5>
        </template>
      </div>

      <div class="col-sm">
        <canvas id="OLE">
        </canvas>
        <template v-if="show===1">
          <h5>OLE</h5>
        </template>
      </div>

    </div>


  </div>
</template>

<script>
import axios from "axios";
import {urlAPI} from "@/variables";

export default {


  name: "DowntimesReport",

  data() {
    return {
      QL : 0,
      lo: this.$t("load"),
      site: '',
      productionline: '',
      product: '',
      formulationType: '',
      reporting: '',
      tool: '',
      beginningDate: '',
      endingDate: '',
      team: '',
      username: localStorage.getItem("username"),
      index: -1,
      show: 0,
      qtyProduced: 0,


      productsName: [],
      quantityArray: [],
      formatArray: [],
      quantityPerArray: [],
      formulationArray: [],

      littersProduced: 0,

      plannedDowntimes: 0,
      unplannedDowntimes: 0,
      plannedProductionTime: 0,
      operatingTime: 0,
      netOperatingTime: 0,
      performance: 0,
      availability: 0,
      quality: 0,
      OLE: 0,
      sommeRejection : 0,
      speedLosses: 0,

      dataSite: null,
      dataWorksite : null,
      dataProductionlines : null,
      dataTeams: null,
      allEvents : null,

      sommeWorkingTime : 0,

      summIdealRate : 0,
      quantityPerGMID : [],

      userWorksite  : null,
      crewLeaders :  null,
      shiftCrew : null,
      productionLParam :null,


    }
  },

  methods: {
    load: async function () {


      if (this.productionline !== '' && this.beginningDate !== '' && this.endingDate !== '' && this.team !== '') {
        var tab = [];
        tab.push(this.site);
        tab.push(this.productionline);
        tab.push(this.beginningDate);
        tab.push(this.endingDate);


        await axios.get(urlAPI + 'allevents/'+this.site+'/'+this.productionline+'/'+this.beginningDate+'/'+this.endingDate+'/'+this.team)
            .then(response => (this.allEvents = response.data))


        this.show = 1;
        await this.resolveAfter15Second();

        this.loadArray();

        this.loadProductionTime();


        this.circle();
        this.pieCharts();


      }

      console.log(this.productionline);
      console.log(this.beginningDate);
      console.log(this.endingDate);

    },

    loadArray: function () {
      this.formulationArray = [];
      this.formatArray = [];
      this.quantityArray = [];
      this.quantityPerArray = [];
      this.qtyProduced = 0;
      this.littersProduced = 0;
      this.productsName = [];

      this.quantityPerGMID = [];
      this.summIdealRate = 0;

      for (let i = 0; i < this.allEvents['SITE'].length; i++) {
        this.quantityArray[i] = 0;
        this.quantityPerArray[i] = 0;
      }

      for (let i = 0; i < this.allEvents['SITE'].length; i++) {

        this.summIdealRate +=  this.allEvents['SITE'][i].idealRate;

        let nbBottles = this.allEvents['SITE'][i].qtyProduced * this.allEvents['SITE'][i].bottlesPerCase;
        this.qtyProduced += nbBottles * 1;

        if (!this.formulationArray.includes(this.allEvents['SITE'][i].bulk)) {
          this.formulationArray.push(this.allEvents['SITE'][i].bulk);
        }

        let indexFormulation = this.formulationArray.indexOf(this.allEvents['SITE'][i].bulk);

        this.quantityPerArray[indexFormulation] += this.allEvents['SITE'][i].qtyProduced * this.allEvents['SITE'][i].bottlesPerCase* this.allEvents['SITE'][i].size;



        if (!this.formatArray.includes(this.allEvents['SITE'][i].size)) {
          this.formatArray.push(this.allEvents['SITE'][i].size);
        }

        let indexSize = this.formatArray.indexOf(this.allEvents['SITE'][i].size);
        this.quantityArray[indexSize] += this.allEvents['SITE'][i].qtyProduced * this.allEvents['SITE'][i].bottlesPerCase*this.allEvents['SITE'][i].size;


        if (!this.productsName.includes(this.allEvents['SITE'][i].product)) {
          this.productsName.push(this.allEvents['SITE'][i].product);
        }

        let index = this.productsName.indexOf(this.allEvents['SITE'][i].product);

        this.littersProduced += this.allEvents['SITE'][i].qtyProduced * this.allEvents['SITE'][i].bottlesPerCase
            * this.allEvents['SITE'][i].size;
        this.formatArray[index] = this.allEvents['SITE'][i].size;


      }

      console.log(this.productsName);
      console.log(this.quantityArray);
      console.log(this.formatArray);
      console.log(this.quantityPerArray);

      console.log("SPEEDLOSSES");
      console.log(this.speedLosses);
      console.log("formulations : ");

      console.log(this.formulationArray);


    },

    resolveAfter15Second: function () {
      return new Promise(resolve => {
        setTimeout(() => {
          resolve('resolved');
        }, 1500);
      });
    },

    loadProductionTime: function () {

      var sommePlannedEvents = 0;
      var sommeUnplannedEvents = 0;

      this.sommeRejection = 0;


      var fillerCounter = 0;
      var caperCounter = 0;
      var labelerCounter = 0;
      var wieghtBoxCounter = 0;
      var qualityControlCounter = 0;

      this.netOperatingTime = 0;
      this.sommeWorkingTime = 0;

      this.speedLosses = 0;

      for (let i = 0; i < this.allEvents['SLEVENTS'].length; i++){
        this.speedLosses += this.allEvents['SLEVENTS'][i].duration;

      }






      console.log(this.allEvents);

      var POArray = [];
      for (let i = 0; i < this.allEvents['SITE'].length; i++) {

        this.sommeWorkingTime += this.allEvents['SITE'][i].workingDuration;

        let PO = this.allEvents['SITE'][i];
        this.sommeRejection += (PO.fillerRejection * 1) + (PO.caperRejection * 1) + (PO.labelerRejection * 1) + (PO.weightBoxRejection * 1) + (PO.qualityControlRejection * 1);

        this.QL += ((PO.fillerRejection * 1) + (PO.caperRejection * 1) + (PO.labelerRejection * 1) + (PO.weightBoxRejection * 1) + (PO.qualityControlRejection * 1))/ PO.idealRate;
        fillerCounter += PO.fillerCounter * 1;
        caperCounter += PO.caperCounter * 1;
        labelerCounter += PO.labelerCounter * 1;
        wieghtBoxCounter += PO.weightBoxCounter * 1;
        qualityControlCounter += PO.qualityControlCounter * 1;

        if(!POArray.includes(PO.number)){
          POArray.push(PO.number);
        }

        this.netOperatingTime += (this.allEvents['SITE'][i].qtyProduced * this.allEvents['SITE'][i].bottlesPerCase * 1) / this.allEvents['SITE'][i].idealRate * 1;

      }
      for (let j = 0; j < this.allEvents['EVENTS'].length; j++) {
        if(POArray.includes(this.allEvents['EVENTS'][j].OLE )){
          sommeUnplannedEvents += this.allEvents['EVENTS'][j].total_duration * 1;
        }
      }

      for (let j = 0; j < this.allEvents['UNPLANNEDEVENTS'].length; j++) {
        if(POArray.includes(this.allEvents['UNPLANNEDEVENTS'][j].OLE )){
          sommeUnplannedEvents += this.allEvents['UNPLANNEDEVENTS'][j].total_duration * 1;
        }
      }

      for (let j = 0; j < this.allEvents['CIPEVENTS'].length; j++) {
        if(POArray.includes(this.allEvents['CIPEVENTS'][j].OLE )){
          sommeUnplannedEvents += this.allEvents['CIPEVENTS'][j].total_duration * 1;
        }
      }

      for (let j = 0; j < this.allEvents['LOTCHANGING'].length; j++) {
        if(POArray.includes(this.allEvents['LOTCHANGING'][j].OLE )){
          sommeUnplannedEvents += this.allEvents['LOTCHANGING'][j].total_duration * 1;
        }
      }


      for (let k = 0; k < this.allEvents['PLANNEDEVENTS'].length; k++) {
        if(POArray.includes(this.allEvents['PLANNEDEVENTS'][k].OLE )){
          sommePlannedEvents += this.allEvents['PLANNEDEVENTS'][k].duration * 1;
        }
      }

      /**
      var SpeedLossesPerPO = [];

      for(let m = 0; m<POArray.length; m++){
        let summPlannedDuration = 0;
        let summUnplannedDuration = 0;
        for (let k = 0; k < this.allEvents['PLANNEDEVENTS'].length; k++) {
          if(POArray[m] ===this.allEvents['PLANNEDEVENTS'][k].OLE){
            summPlannedDuration +=  this.allEvents['PLANNEDEVENTS'][k].duration;
          }


          for (let j = 0; j < this.allEvents['EVENTS'].length; j++) {
            if(POArray[m] === this.allEvents['EVENTS'][j].OLE ){
              summUnplannedDuration += this.allEvents['EVENTS'][j].total_duration * 1;
            }
          }

          for (let j = 0; j < this.allEvents['UNPLANNEDEVENTS'].length; j++) {
            if(POArray[m] === this.allEvents['UNPLANNEDEVENTS'][j].OLE ){
              summUnplannedDuration += this.allEvents['UNPLANNEDEVENTS'][j].total_duration * 1;
            }
          }

          for (let j = 0; j < this.allEvents['CIPEVENTS'].length; j++) {
            if(POArray[m] === this.allEvents['CIPEVENTS'][j].OLE ){
              summUnplannedDuration += this.allEvents['CIPEVENTS'][j].total_duration * 1;
            }
          }

          for (let j = 0; j < this.allEvents['LOTCHANGING'].length; j++) {
            if(POArray[m] === this.allEvents['LOTCHANGING'][j].OLE ){
              summUnplannedDuration += this.allEvents['LOTCHANGING'][j].total_duration * 1;
            }
          }

      }
      **/

      this.plannedDowntimes = sommePlannedEvents;
      this.unplannedDowntimes = sommeUnplannedEvents;







      console.log('working TIME : ');
      console.log(this.sommeWorkingTime);
      console.log('planned TIME : ');
      console.log(this.plannedDowntimes);
      console.log('unplanned TIME : ');
      console.log(sommeUnplannedEvents);
      console.log('speedLosses TIME : ');
      console.log(this.speedLosses);

      this.totalOperatingTime = this.totalProductionTime - this.totalUnplannedDowtimes;



      this.plannedProductionTime = this.sommeWorkingTime - sommePlannedEvents;
      this.operatingTime = this.plannedProductionTime  - sommeUnplannedEvents;



      this.netOperatingTime = this.operatingTime - this.speedLosses;


      this.availability = (this.operatingTime / this.plannedProductionTime);
      if(this.availability > 1){
        this.availability = 1;
      }


      //Calcul Performance
      var n = 0;
      //var idealPerf = 0;
      var quantityProducedForQuality = 0
      for(var i = 0; i<this.allEvents['SITE'].length; i++)
      {
        if(!(this.allEvents['SITE'][i].labelerCounter === 0 && this.allEvents['SITE'][i].weightBoxCounter === 0
         && this.allEvents['SITE'][i].qualityControlCounter === 0 && this.allEvents['SITE'][i].caperCounter === 0
          && this.allEvents['SITE'][i].fillerCounter === 0)){
          quantityProducedForQuality += this.allEvents['SITE'][i].qtyProduced * this.allEvents['SITE'][i].bottlesPerCase;
        }

        if(this.quantityPerGMID.includes( this.allEvents['SITE'][i].GMID)){
          this.quantityPerGMID[ this.allEvents['SITE'][i].GMID] += this.allEvents['SITE'][i].qtyProduced;
        }else{
          this.quantityPerGMID[ this.allEvents['SITE'][i].GMID] = this.allEvents['SITE'][i].qtyProduced;
        }
        n += (this.allEvents['SITE'][i].qtyProduced * this.allEvents['SITE'][i].bottlesPerCase / this.allEvents['SITE'][i].idealRate);
        //idealPerf += this.allEvents['SITE'][i].idealRate;
      }
      //var numerateur = n / this.summIdealRate;
      this.performance = n / this.operatingTime;

      //=====
      //sumQty = (2400*4 + 1600*12)
      //idealRate = (2000*4/20 + 1600*12/40 + 1500*4/20 )  = 400 + 480 + 300



      console.log('net OP TIME : ');
      console.log(this.netOperatingTime);
      console.log(' OP TIME : ');
      console.log(this.operatingTime);


      console.log('Compteur : ' + quantityProducedForQuality);

      var summCompteur = 0;
      if (fillerCounter !== 0) {
        summCompteur += (fillerCounter - quantityProducedForQuality);

      }
      if (caperCounter !== 0) {
        summCompteur += (caperCounter - quantityProducedForQuality);
      }

      if (labelerCounter !== 0) {
        summCompteur += (labelerCounter - quantityProducedForQuality);
      }

      if (qualityControlCounter !== 0) {
        summCompteur += (qualityControlCounter - quantityProducedForQuality);

      }
      if (wieghtBoxCounter !== 0) {
        summCompteur += (wieghtBoxCounter - quantityProducedForQuality);
      }

      console.log('DATA FOR QUALITY');
      console.log('QTY PRODUCED : ' + quantityProducedForQuality);
      console.log('Rejection : ' + this.sommeRejection);
      console.log('Compteur : ' + summCompteur);

      if(quantityProducedForQuality + this.sommeRejection + summCompteur === 0 ){
        this.quality = 1;
      }else{
        this.quality = (quantityProducedForQuality) / (quantityProducedForQuality + this.sommeRejection + summCompteur);
      }

      if(this.quality > 1){
        this.quality = 1;
      }



      if (this.operatingTime === 0) {
        this.availability = 0;

      }

      if (this.operatingTime === 0) {
        this.performance = 0;
        this.unplannedDowntimes = 1;
      }

      if (this.plannedProductionTime === 0) {
        this.plannedDowntimes = 1;
      }

      this.OLE = this.availability * this.performance * this.quality;

      console.log('Planned Downtime : ' + sommePlannedEvents);
      console.log('Unplanned Downtime : ' + sommeUnplannedEvents);
      console.log('Planned Production Time : ' + this.plannedProductionTime);
      console.log('Operating Time : ' + this.operatingTime);

      console.log('NOT : ' + sommePlannedEvents);
      console.log('Availability : ' + this.availability);
      console.log('Performance : ' + this.performance);
      console.log('Quality : ' + this.quality);

      console.log('Operating Time : ' + this.operatingTime);


    }
    ,

    pieCharts: function () {
      let obj;
      var data = [];
      var totalPieChart1 = 0;
      for (let j = 0; j < this.formulationArray.length; j++) {
        if(this.quantityPerArray[j] > 0 ){
          obj = {
            name: this.formulationArray[j],
            nbr: this.quantityPerArray[j]
          };
          data.push(obj);
          totalPieChart1 += this.quantityPerArray[j];
        }
      }
      console.log('DATA');

      console.log(data);

      if (data.length === 0) {
        obj = {
          name: this.$t("nothingProduced"),
          nbr: 1
        };
        data.push(obj);

        totalPieChart1 = 1;
      }


      const randomHexColorCode = () => {
        return "#" + Math.random().toString(16).slice(2, 8)
      };

      var canvas = document.getElementById("can");
      var ctx = canvas.getContext("2d");
      canvas.width = 400;
      canvas.height = 300;
      let total = data.reduce((ttl, house) => {
        return ttl + house.nbr
      }, 0);
      let startAngle = 0;
      let radius = 70;
      let cx = canvas.width / 2;
      let cy = canvas.height / 2;


      for (let j = 0; j < data.length; j++) {

        let item = data[j];
        //set the styles before beginPath
        ctx.fillStyle = randomHexColorCode();
        ctx.lineWidth = 1;
        ctx.strokeStyle = '#333';
        ctx.beginPath();
        //console.log(total, house.troops, house.troops/total);
        // draw the pie wedges
        let endAngle = ((item.nbr / total) * Math.PI * 2) + startAngle;
        ctx.moveTo(cx, cy);
        ctx.arc(cx, cy, radius, startAngle, endAngle, false);
        ctx.lineTo(cx, cy);
        ctx.fill();
        ctx.stroke();
        ctx.closePath();

        // add the labels
        ctx.beginPath();
        ctx.font = '15px Helvetica, Calibri';
        ctx.textAlign = 'center';
        ctx.fillStyle = 'rebeccapurple';
        // midpoint between the two angles
        // 1.5 * radius is the length of the Hypotenuse
        let theta = (startAngle + endAngle) / 2;
        let deltaY = Math.sin(theta) * 1.5 * radius;
        let deltaX = Math.cos(theta) * 1.5 * radius;
        /***
         SOH  - sin(angle) = opposite / hypotenuse
         = opposite / 1px
         CAH  - cos(angle) = adjacent / hypotenuse
         = adjacent / 1px
         TOA

         ***/
        var txt = item.name + '\n';
        var pct = item.nbr / totalPieChart1 * 100;
        txt = txt + ' ' + pct.toFixed(2) + '%';

        ctx.fillText(txt, deltaX + cx, deltaY + cy);
        ctx.closePath();

        startAngle = endAngle;
      }

      data = [];
      var totalPieChart2 = 0;
      for (let j = 0; j < this.formatArray.length; j++) {
        if(this.quantityArray[j]>0){
          obj = {
            name: this.formatArray[j],
            nbr: this.quantityArray[j],
          };
          data.push(obj);
          totalPieChart2 += this.quantityArray[j];
        }
      }

      if (data.length === 0) {
        obj = {
          name: this.$t("nothingProduced"),
          nbr: 1
        };
        data.push(obj);

        totalPieChart2 = 1;
      }

      console.log('DATA');

      console.log(data);


      canvas = document.getElementById("can2");
      ctx = canvas.getContext("2d");
      canvas.width = 400;
      canvas.height = 300;
      total = data.reduce((ttl, house) => {
        return ttl + house.nbr
      }, 0);
      startAngle = 0;
      radius = 70;
      cx = canvas.width / 2;
      cy = canvas.height / 2;

      for (let j = 0; j < data.length; j++) {

        let item = data[j];
        //set the styles before beginPath
        ctx.fillStyle = randomHexColorCode();
        ctx.lineWidth = 1;
        ctx.strokeStyle = '#333';
        ctx.beginPath();
        //console.log(total, house.troops, house.troops/total);
        // draw the pie wedges
        let endAngle = ((item.nbr / total) * Math.PI * 2) + startAngle;
        ctx.moveTo(cx, cy);
        ctx.arc(cx, cy, radius, startAngle, endAngle, false);
        ctx.lineTo(cx, cy);
        ctx.fill();
        ctx.stroke();
        ctx.closePath();

        // add the labels
        ctx.beginPath();
        ctx.font = '15px Helvetica, Calibri';
        ctx.textAlign = 'center';
        ctx.fillStyle = 'rebeccapurple';
        // midpoint between the two angles
        // 1.5 * radius is the length of the Hypotenuse
        let theta = (startAngle + endAngle) / 2;
        let deltaY = Math.sin(theta) * 1.5 * radius;
        let deltaX = Math.cos(theta) * 1.5 * radius;

        if (item.name !== this.$t("nothingProduced")) {
          txt = item.name + 'L/Kg\n';
          pct = item.nbr / totalPieChart2 * 100;
          txt = txt + ' ' + pct.toFixed(2) + '%';
        } else {
          txt = item.name + '\n';
        }


        startAngle = endAngle;
        ctx.fillText(txt, deltaX + cx, deltaY + cy);
        ctx.closePath();

        startAngle = endAngle;
      }


    }
    ,

    resolveAfter05Second: function () {
      return new Promise(resolve => {
        setTimeout(() => {
          resolve('resolved');
        }, 500);
      });
    }
    ,

    circle: function () {

      var canvas = document.getElementById("Availability");
      var context = canvas.getContext("2d");
      context.lineWidth = "2";
      context.fillStyle = "#FF0000";
      if (this.availability >= 0.70 && this.availability < 0.95) {
        context.fillStyle = "#FF8700";
      } else if (this.availability >= 0.95) {
        context.fillStyle = "#71FA23";
      }
      context.arc(80, 80, 40, 0, 2 * Math.PI);
      context.stroke();
      context.fill();
      context.fillStyle = "#FFF";
      context.font = '20px serif';
      let ava = this.availability * 100;
      context.fillText(ava.toFixed(2), 65, 90);


      canvas = document.getElementById("Performance");
      context = canvas.getContext("2d");
      context.lineWidth = "2";
      context.fillStyle = "#FF0000";
      if (this.performance >= 0.70 && this.performance < 0.95) {
        context.fillStyle = "#FF8700";
      } else if (this.performance >= 0.95) {
        context.fillStyle = "#71FA23";
      }
      context.arc(80, 80, 40, 0, 2 * Math.PI);
      context.stroke();
      context.fill();
      context.fillStyle = "#FFF";
      context.font = '20px serif';
      let perf = this.performance * 100;
      context.fillText(perf.toFixed(2), 65, 90);

      canvas = document.getElementById("Quality");
      context = canvas.getContext("2d");
      context.lineWidth = "2";
      context.fillStyle = "#FF0000";
      if (this.quality >= 0.7 && this.quality < 0.95) {
        context.fillStyle = "#FF8700";
      } else if (this.quality >= 0.95) {
        context.fillStyle = "#71FA23";
      }
      context.arc(80, 80, 40, 0, 2 * Math.PI);
      context.stroke();
      context.fill();
      context.fillStyle = "#FFF";
      context.font = '20px serif';
      let qua = this.quality * 100;
      context.fillText(qua.toFixed(2), 65, 90);


      canvas = document.getElementById("OLE");
      context = canvas.getContext("2d");
      context.lineWidth = "2";
      context.fillStyle = "#FF0000";
      if (this.OLE >= 0.70 && this.OLE < 0.95) {
        context.fillStyle = "#FF8700";
      } else if (this.OLE >= 0.95) {
        context.fillStyle = "#71FA23";
      }
      context.arc(80, 80, 40, 0, 2 * Math.PI);
      context.stroke();
      context.fill();
      context.fillStyle = "#FFF";
      context.font = '20px serif';
      let o = this.OLE * 100;
      context.fillText(o.toFixed(2), 65, 90);


    }
    ,

  }
  ,


  async mounted() {
    if (sessionStorage.getItem("language") !== null) {
      this.$i18n.locale = sessionStorage.getItem("language");
    }

    //récupère les sites et les lignes de production
    //sites[0] = sites
    //sites[1] = lignes de production de ce site
    //sites[2] = teams



    console.log(sessionStorage.getItem("user-status"));
    if(sessionStorage.getItem("user-status") == 1){
      await axios.get(urlAPI + 'sites/'+this.username)
          .then(response => (this.dataSite = response.data))
    } else {
      await axios.get(urlAPI + 'sites')
          .then(response => (this.dataSite = response.data))
    }

    this.dataWorksite = this.dataSite[0];
    this.dataProductionlines = this.dataSite[1];
    this.dataTeams = this.dataSite[2];

    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();

    if (dd < 10) {
      dd = '0' + dd;
    }

    if (mm < 10) {
      mm = '0' + mm;
    }

    today = yyyy + '-' + mm + '-' + dd;
    document.getElementById("startingPO").setAttribute("max", today);
    document.getElementById("endingPO").setAttribute("max", today);


    let externalScript = document.createElement('script');
    externalScript.setAttribute('src', 'https://canvasjs.com/assets/script/canvasjs.min.js');
    document.head.appendChild(externalScript)
    console.log('wouah');

    console.log(this.dataSite);
    console.log(this.dataProductionlines);
  }
  ,

}
</script>

<style scoped>
h1 {
  font-size: 1.4em;
  color: #56baed;
}

label {
  font-size: 1.4em;
  color: #56baed;
}

p {
  font-size: 1.4em;
  color: #56baed;
}

h2 {
  font-size: 1.2em;
  color: #56baed;
}

h4 {
  color: red;
}


div {
  background-color: #fff;
  padding: 15px;
}

thead {
  color: white;
  background: #56baed;
}

.container {
  margin-left: 60px;
}

h5 {
  margin-left: 60px;
}


.table-info-data {
  overflow: scroll;
  max-height: 300px;
}


.rcorners2 {
  border: 2px solid lightgray;
  padding: 20px;
}

.blueBack {
  font-size: 20px;
  color: white;
  background: #0056ff;
  padding: 10px;
  width: 60%;
}

.greenBack {
  font-size: 20px;
  color: white;
  background: #71FA23;
  padding: 10px;
  width: 30%;
}

.redBack {
  font-size: 20px;
  color: white;
  background: red;
  padding: 10px;
  width: 30%;
}

#rect1 {
  margin-right: 10px;
}

#rect2 {
  margin-right: 40px;
}

#rect3 {
  margin-right: 70px;
}

#rect4 {
  margin-right: 100px;
}

.rect {
  margin-bottom: -40px;
}

div.data {
  flex-direction: column;
  border: solid 1px;
  border-radius: 5px;
  padding: 10px 5px;
}


</style>

