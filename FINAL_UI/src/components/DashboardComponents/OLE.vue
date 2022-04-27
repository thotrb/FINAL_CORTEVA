<template>
  <div>
  <div class="row dataInput">
    <div class="col-sm">
      <div class="data">
        <div class="">

          <form>
            <label class="" for="site">{{$t("site")}} : </label>
            <select name="site" id="site" class="form-select" v-model="site" @change="team=''">
              <template v-for="site in dataWorksite">
                <option v-bind:value="site.name" v-bind:key="site.id">
                  {{site.name}}
                </option>
              </template>
            </select>
          </form>
        </div>
        <div>
          <form>
            <label class="" for="productionline">{{$t("productionLine")}} : </label>
            <select name="productionline" id="productionline" class="form-select"
                    v-model="productionline" @change="team='allTeams'">
              <template v-for="productionline in dataProductionlines">
                <template v-if="productionline.name === site">
                  <option v-bind:value="productionline.productionline_name" v-bind:key="productionline.id">
                    {{productionline.productionline_name}}
                  </option>
                </template>

              </template>

            </select>
          </form>
        </div>
        <div>
            <form>
              <label for="team">{{$t("team")}}: </label>
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
    </div>

    <div class="col-sm">
      <div class=data>
        <div>
          <h1>
            {{$t("productionShift")}}
          </h1>
        </div>
        <br/>

        <div>
          <label class="" for="startYear">{{$t("year")}}</label>

          <select id="startYear" v-model="year">
            <template v-for="year of years">
              <option v-bind:key="year" v-bind:value="year">{{year}}</option>
            </template>
          </select>


        </div>
      </div>
    </div>
  </div>


  <div align="center">
    <template v-if="show===1">

      <h1>
        {{$t('Overall Line Effectivness')}}
      </h1>

    </template>
    <div class="d-flex">
      <div class="col-8">

        <canvas class="diagram"  id="myChart4"></canvas>

      </div>


      <div class="col">
        <template v-if="show===1">

          <table class="table">
            <thead>
            <tr>
              <th scope="col"></th>
              <th scope="col">{{$t("peakSeason")}}</th>
              <th scope="col">{{$t("allYear")}}</th>

            </tr>
            </thead>
            <tbody>
            <tr>
              <th scope="row">{{$t("availability")}}</th>
              <td>{{(AvailabilityPerMonth[peakSeason]*100).toFixed(2)}} %</td>
              <td>{{(availability * 100).toFixed(2)}} %</td>
            </tr>
            <tr>
              <th scope="row">{{$t("performance")}}</th>
              <td>{{(PerformancePerMonth[peakSeason]*100).toFixed(2)}} %</td>
              <td>{{(performance * 100).toFixed(2)}} %</td>
            </tr>
            <tr>
              <th scope="row">{{$t("quality")}}</th>
              <td>{{(QualityPerMonth[peakSeason]*100).toFixed(2)}} %</td>
              <td>{{(quality * 100).toFixed(2)}} %</td>
            </tr>
            <tr>
              <th scope="row">OLE</th>
              <td>{{(OLEPerMonth[peakSeason]*100).toFixed(2)}} %</td>
              <td>{{(OLE * 100).toFixed(2)}} %</td>
            </tr>
            </tbody>
          </table>

          <br/>

          <h3>
            {{$t("trendVersusPreviousYear")}}
          </h3>

          <br/>

          <table class="table">
            <thead>
            <tr>
              <th scope="col"></th>
              <th scope="col"></th>

            </tr>
            </thead>
            <tbody>
            <tr>
              <th scope="row">{{$t("availability")}}</th>
              <td v-if="((availability - availability2)*100).toFixed(2) > 0">
                + {{((availability - availability2)*100).toFixed(2) }} %
              </td>
              <td v-else>
                {{((availability - availability2)*100).toFixed(2) }} %
              </td>

            </tr>
            <tr>
              <th scope="row">{{$t("performance")}}</th>

              <td v-if="((performance - performance2)*100).toFixed(2)  > 0">
                + {{((performance - performance2)*100).toFixed(2)}} %
              </td>
              <td v-else>
                {{((performance - performance2)*100).toFixed(2) }} %
              </td>


            </tr>
            <tr>
              <th scope="row">{{$t("quality")}}</th>


              <td v-if="((quality - quality2)*100).toFixed(2)  > 0">
                + {{((quality - quality2)*100).toFixed(2)}} %
              </td>
              <td v-else>
                {{((quality - quality2)*100).toFixed(2)}} %
              </td>


            </tr>
            <tr>

              <th scope="row">OLE</th>


              <td v-if="((OLE - OLE2)*100).toFixed(2)  > 0">
                + {{((OLE - OLE2)*100).toFixed(2) }} %
              </td>
              <td v-else>
                {{((OLE - OLE2)*100).toFixed(2) }} %
              </td>

            </tr>
            </tbody>
          </table>


        </template>
      </div>


    </div>

  </div>

</div>

</template>

<script>
import axios from "axios";
import {urlAPI} from "@/variables";
import Chart from 'chart.js/auto'

export default {
  name: "OLE",
  data() {
    return {
      months: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
      years: [],
      year: 0,
      yearsAfterFrom: [],
      currentYear: (new Date()).getFullYear(),
      startYear: 2015,
      site: '',
      productionline: '',
      team: '',
      show: 0,

      allEvents2: null,
      dataTeams : null,

      OLEPerMonth: [],
      AvailabilityPerMonth: [],
      PerformancePerMonth: [],
      QualityPerMonth: [],
      netOperatingTimePerMonth: [],
      plannedDowntimesPerMonth: [],
      unplannedDowntimesPerMonth: [],
      plannedProductionTimePerMonth: [],
      operatingTimePerMonth: [],
      speedLossesPerMonth: [],

      OLEPerMonth2: [],
      AvailabilityPerMonth2: [],
      PerformancePerMonth2: [],
      QualityPerMonth2: [],
      netOperatingTimePerMonth2: [],
      plannedDowntimesPerMonth2: [],
      unplannedDowntimesPerMonth2: [],
      plannedProductionTimePerMonth2: [],
      operatingTimePerMonth2: [],
      speedLossesPerMonth2: [],

      peakSeason: 0,

      plannedDowntimes: 0,
      unplannedDowntimes: 0,
      plannedProductionTime: 0,
      operatingTime: 0,
      netOperatingTime: 0,
      performance: 0,
      availability: 0,
      quality: 0,
      OLE: 0,
      chartFinal : null,

      plannedDowntimes2: 0,
      unplannedDowntimes2: 0,
      plannedProductionTime2: 0,
      operatingTime2: 0,
      netOperatingTime2: 0,
      performance2: 0,
      availability2: 0,
      quality2: 0,
      OLE2: 0,
      username: localStorage.getItem("username"),


      lo: this.$t("load"),

      dataWorksite : null,
      dataProductionlines : null,
      teamData : null,
      allEvents : null,
    }
  },

  methods: {

    load: async function () {
      if (this.productionline !== '' && this.year !== '' && this.team !== '') {
        var firstDayYear = this.year + '-01-01';
        var lastDayYear = this.year + '-12-31';

        var firstDayYear2 = this.year-1 + '-01-01';
        var lastDayYear2 = this.year-1 + '-12-31';

        var tab = [];
        tab.push(this.site);
        tab.push(this.productionline);
        tab.push(firstDayYear);
        tab.push(lastDayYear);


        await axios.get(urlAPI + 'allevents/' + this.site + '/' + this.productionline + '/' + firstDayYear + '/' + lastDayYear + '/' + this.team)
            .then(response => (this.allEvents = response.data))

        await axios.get(urlAPI + 'allevents/' + this.site + '/' + this.productionline + '/' + firstDayYear2 + '/' + lastDayYear2 + '/' + this.team)
            .then(response => (this.allEvents2 = response.data))


        console.log(this.allEvents);
        this.loadProductionTime();
        this.loadProductionTimeThisYear();

        if(this.allEvents2["SITE"].length > 0){
          this.loadProductionTimePreviousYear();
        }


        firstDayYear = this.year - 1 + '-01-01';
        lastDayYear = this.year - 1 + '-12-31';
        tab = [];
        tab.push(this.site);
        tab.push(this.productionline);
        tab.push(firstDayYear);
        tab.push(lastDayYear);


        this.show = 1;
        this.graph2();
      }


    },

    loadProductionTimePreviousYear: function () {
      var sommePlannedEvents = 0;
      var sommeUnplannedEvents = 0;

      var sommeRejection = 0;

      var fillerCounter = 0;
      var caperCounter = 0;
      var labelerCounter = 0;
      var wieghtBoxCounter = 0;
      let qualityCheckCounter = 0;


      this.netOperatingTime2 = 0;
      var sommeWorkingTime = 0;

      var n=0;
      var quantityProducedForQuality = 0;


      for (let i = 0; i < this.allEvents2['SITE'].length; i++) {

        sommeWorkingTime += this.allEvents2['SITE'][i].workingDuration * 1;


        let PO = this.allEvents2['SITE'][i];
        sommeRejection += PO.fillerRejection * 1 + PO.caperRejection * 1 + PO.labelerRejection * 1 + PO.weightBoxRejection * 1 + PO.qualityControlRejection * 1;
        fillerCounter += PO.fillerCounter * 1;
        caperCounter += PO.caperCounter * 1;
        labelerCounter += PO.labelerCounter * 1;
        wieghtBoxCounter += PO.weightBoxCounter * 1;
        qualityCheckCounter += PO.qualityControlCounter*1;

        if(!(this.allEvents2['SITE'][i].labelerCounter === 0 && this.allEvents2['SITE'][i].weightBoxCounter === 0
            && this.allEvents2['SITE'][i].qualityControlCounter === 0 && this.allEvents2['SITE'][i].caperCounter === 0
            && this.allEvents2['SITE'][i].fillerCounter === 0)){

          quantityProducedForQuality += this.allEvents2['SITE'][i].qtyProduced * this.allEvents2['SITE'][i].bottlesPerCase;

        }
        n += (this.allEvents2['SITE'][i].qtyProduced * this.allEvents2['SITE'][i].bottlesPerCase / this.allEvents2['SITE'][i].idealRate);


        this.netOperatingTime2 += (this.allEvents2['SITE'][i].qtyProduced * this.allEvents2['SITE'][i].bottlesPerCase * 1) / this.allEvents2['SITE'][i].idealRate * 1;
        for (let j = 0; j < this.allEvents2['EVENTS'].length; j++) {

          if (this.allEvents2['EVENTS'][j].OLE === PO.number) {
            sommeUnplannedEvents += this.allEvents2['EVENTS'][j].total_duration * 1;


          }

        }
      }

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

      if (qualityCheckCounter !== 0) {
        summCompteur += (qualityCheckCounter - quantityProducedForQuality);
      }
      if (wieghtBoxCounter !== 0) {
        summCompteur += (wieghtBoxCounter - quantityProducedForQuality);
      }

      for (let j = 0; j < this.allEvents2['UNPLANNEDEVENTS'].length; j++) {

        sommeUnplannedEvents += this.allEvents2['UNPLANNEDEVENTS'][j].total_duration * 1;
      }

      for (let j = 0; j < this.allEvents2['CIPEVENTS'].length; j++) {

        sommeUnplannedEvents += this.allEvents2['CIPEVENTS'][j].total_duration * 1;

      }

      for (let j = 0; j < this.allEvents2['EVENTS'].length; j++) {

        sommeUnplannedEvents += this.allEvents2['EVENTS'][j].total_duration * 1;

      }

      for (let j = 0; j < this.allEvents2['LOTCHANGING'].length; j++) {
        sommeUnplannedEvents += this.allEvents2['LOTCHANGING'][j].total_duration * 1;
      }

      for (let k = 0; k < this.allEvents2['PLANNEDEVENTS'].length; k++) {
        sommePlannedEvents += this.allEvents2['PLANNEDEVENTS'][k].duration * 1;
      }


      for (let k = 0; k < this.allEvents2['PLANNEDEVENTS'].length; k++) {
          sommePlannedEvents += this.allEvents2['PLANNEDEVENTS'][k].duration * 1;
      }


      this.plannedDowntimes2 = sommePlannedEvents;
      this.unplannedDowntimes2 = sommeUnplannedEvents;


      this.plannedProductionTime2 = sommeWorkingTime - sommePlannedEvents;
      this.operatingTime2 = sommeWorkingTime - sommePlannedEvents - sommeUnplannedEvents;

      this.availability2 = (this.operatingTime2 / this.plannedProductionTime2);
      this.performance2 = (this.netOperatingTime2 / this.operatingTime2);

      console.log('net OP TIME : ');
      console.log(this.netOperatingTime2);
      console.log(' OP TIME : ');
      console.log(this.operatingTime2);

      this.availability2 = this.operatingTime2 / this.plannedProductionTime2;
      this.performance2 = n / this.operatingTime2;

      if(quantityProducedForQuality + sommeRejection + summCompteur === 0 ){
        this.quality2 = 1;
      }else{
        this.quality2 = (quantityProducedForQuality) / (quantityProducedForQuality + sommeRejection + summCompteur);
      }



      this.OLE2 = (this.availability2 * this.performance2 * this.quality2)

      console.log("AVAILABILITY2 : " + this.availability2);
      console.log("PERFORMANCE2 : " + this.performance2);
      console.log("QUALITY2 : " + this.quality2);



    },

    loadProductionTimeThisYear: function () {


      var sommePlannedEvents = 0;
      var sommeUnplannedEvents = 0;

      var sommeRejection = 0;

      var fillerCounter = 0;
      var caperCounter = 0;
      var labelerCounter = 0;
      var wieghtBoxCounter = 0;
      var qualityCheckCounter = 0;

      this.netOperatingTime = 0;
      var sommeWorkingTime = 0;

      this.speedLosses = 0;

      if (this.allEvents['RRF'][0].nbEvents > 0) {
        this.speedLosses += this.allEvents['RRF'][0].Duration * 1;
      }

      if (this.allEvents['RRM'][0].nbEvents > 0) {
        this.speedLosses += this.allEvents['RRM'][0].Duration * 1;
      }

      if (this.allEvents['FOS'][0].nbEvents > 0) {
        this.speedLosses += this.allEvents['FOS'][0].Duration * 1;


      }

      if (this.allEvents['FSM'][0].nbEvents > 0) {
        this.speedLosses += this.allEvents['FSM'][0].Duration * 1;

      }


      var n = 0;
      var quantityProducedForQuality = 0;
      for (let i = 0; i < this.allEvents['SITE'].length; i++) {

        sommeWorkingTime += this.allEvents['SITE'][i].workingDuration;


        let PO = this.allEvents['SITE'][i];
        sommeRejection += PO.fillerRejection * 1 + PO.caperRejection * 1 + PO.labelerRejection * 1 + PO.weightBoxRejection * 1 + PO.qualityControlRejection * 1;
        fillerCounter += PO.fillerCounter * 1;
        caperCounter += PO.caperCounter * 1;
        labelerCounter += PO.labelerCounter * 1;
        wieghtBoxCounter += PO.weightBoxCounter * 1;
        qualityCheckCounter += PO.qualityControlCounter * 1;

        if(!(this.allEvents['SITE'][i].labelerCounter === 0 && this.allEvents['SITE'][i].weightBoxCounter === 0
            && this.allEvents['SITE'][i].qualityControlCounter === 0 && this.allEvents['SITE'][i].caperCounter === 0
            && this.allEvents['SITE'][i].fillerCounter === 0)){

          quantityProducedForQuality += this.allEvents['SITE'][i].qtyProduced * this.allEvents['SITE'][i].bottlesPerCase;

        }
        n += (this.allEvents['SITE'][i].qtyProduced * this.allEvents['SITE'][i].bottlesPerCase / this.allEvents['SITE'][i].idealRate);
      }

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

      if (qualityCheckCounter !== 0) {
        summCompteur += (qualityCheckCounter - quantityProducedForQuality);
      }
      if (wieghtBoxCounter !== 0) {
        summCompteur += (wieghtBoxCounter - quantityProducedForQuality);
      }


      for (let j = 0; j < this.allEvents['UNPLANNEDEVENTS'].length; j++) {

        sommeUnplannedEvents += this.allEvents['UNPLANNEDEVENTS'][j].total_duration * 1;
      }

      for (let j = 0; j < this.allEvents['CIPEVENTS'].length; j++) {

        sommeUnplannedEvents += this.allEvents['CIPEVENTS'][j].total_duration * 1;

      }

      for (let j = 0; j < this.allEvents['EVENTS'].length; j++) {

        sommeUnplannedEvents += this.allEvents['EVENTS'][j].total_duration * 1;

      }

      for (let j = 0; j < this.allEvents['LOTCHANGING'].length; j++) {
        sommeUnplannedEvents += this.allEvents['LOTCHANGING'][j].total_duration * 1;
      }

      for (let k = 0; k < this.allEvents['PLANNEDEVENTS'].length; k++) {
          sommePlannedEvents += this.allEvents['PLANNEDEVENTS'][k].duration * 1;
      }


      this.plannedDowntimes = sommePlannedEvents;
      this.unplannedDowntimes = sommeUnplannedEvents;


      this.plannedProductionTime = sommeWorkingTime - sommePlannedEvents;


      this.operatingTime = sommeWorkingTime - sommePlannedEvents - sommeUnplannedEvents;
      this.netOperatingTime = this.operatingTime - this.speedLosses;
      console.log('working TIME : ');
      console.log(sommeWorkingTime);
      console.log('planned TIME : ');
      console.log(sommePlannedEvents);
      console.log('unplanned TIME : ');
      console.log(sommeUnplannedEvents);
      console.log('speedLosses TIME : ');
      console.log(this.speedLosses);


      this.availability = this.operatingTime / this.plannedProductionTime;
      this.performance = n / this.operatingTime;

      console.log('net OP TIME : ');
      console.log(this.netOperatingTime);
      console.log(' OP TIME : ');
      console.log(this.operatingTime);

      if(quantityProducedForQuality + sommeRejection + summCompteur === 0 ){
        this.quality = 1;
      }else{
        this.quality = (quantityProducedForQuality) / (quantityProducedForQuality + sommeRejection + summCompteur);
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
      console.log('Operating Time : ' + this.operatingTime);

    },



    loadProductionTime: function () {


      var sommeWorkingTimePerMonth = [];
      var sommeQtyProducedPerMonth = [];
      var sommeRejectionPerMonth = [];
      var fillerCounterPerMonth = [];
      var caperCounterPerMonth = [];
      var labelerCounterPerMonth = [];
      var weightBoxCounterPerMonth = [];
      var qualityControlCounterPerMonth = [];
      var sumPlannedEventsPerMonth = [];
      var sumUnplannedEventsPerMonth = [];

      var sommeQtyProducedPerMonthForQuality = [];

      var nPerMonthForPerformance = [];

      for (let i = 0; i < 12; i++) {
        sommeWorkingTimePerMonth[i] = 0;
        sommeQtyProducedPerMonth[i] = 0;
        sommeQtyProducedPerMonthForQuality[i] = 0;

        sommeRejectionPerMonth[i] = 0;
        fillerCounterPerMonth[i] = 0;
        caperCounterPerMonth[i] = 0;
        labelerCounterPerMonth[i] = 0;
        weightBoxCounterPerMonth[i] = 0;
        qualityControlCounterPerMonth[i] = 0;
        sumPlannedEventsPerMonth[i] = 0;
        sumUnplannedEventsPerMonth[i] = 0;
        nPerMonthForPerformance[i] = 0;
        this.OLEPerMonth[i] = 0;
        this.AvailabilityPerMonth[i] = 0;
        this.PerformancePerMonth[i] = 0;
        this.QualityPerMonth[i] = 0;
        this.netOperatingTimePerMonth[i] = 0;
        this.plannedDowntimesPerMonth [i] = 0;
        this.unplannedDowntimesPerMonth[i] = 0;
        this.plannedProductionTimePerMonth[i] = 0;
        this.operatingTimePerMonth[i] = 0;
        this.speedLossesPerMonth[i] = 0;

        //var idealRatePerMonth = [];


      }

      var month = 0;


      for (let i = 0; i < this.allEvents['SITE'].length; i++) {

        month = this.allEvents['SITE'][i].created_at.split('-')[1] - 1;

        let PO = this.allEvents['SITE'][i];

        console.log("MOIS : " + month);


        sommeWorkingTimePerMonth[month] += PO.workingDuration * 1;
        sommeQtyProducedPerMonth[month] += PO.qtyProduced * PO.bottlesPerCase * 1;


        if (!(this.allEvents['SITE'][i].labelerCounter === 0 && this.allEvents['SITE'][i].weightBoxCounter === 0
            && this.allEvents['SITE'][i].qualityControlCounter === 0 && this.allEvents['SITE'][i].caperCounter === 0
            && this.allEvents['SITE'][i].fillerCounter === 0)) {

          sommeQtyProducedPerMonthForQuality[month]+= PO.qtyProduced * PO.bottlesPerCase;
          sommeRejectionPerMonth[month] += PO.fillerRejection * 1 + PO.caperRejection * 1 + PO.labelerRejection * 1 + PO.weightBoxRejection * 1 + PO.qualityControlRejection * 1;
          fillerCounterPerMonth[month] += PO.fillerCounter * 1;
          caperCounterPerMonth[month] += PO.caperCounter * 1;
          labelerCounterPerMonth[month] += PO.labelerCounter * 1;
          weightBoxCounterPerMonth[month] += PO.weightBoxCounter * 1;
          qualityControlCounterPerMonth[month] += PO.qualityControlCounter * 1;
        }
      }

      var max = -1;
      for (let l = 0; l < sommeWorkingTimePerMonth.length; l++) {

        if (sommeWorkingTimePerMonth[l] > max) {
          max = sommeWorkingTimePerMonth[l];
          this.peakSeason = l;
        }
      }

      this.plannedDowntimesPerMonth = sumPlannedEventsPerMonth;
      this.unplannedDowntimesPerMonth = sumUnplannedEventsPerMonth;
      var currentMonth = 1;
      for (let j = 0; j < this.allEvents['UNPLANNEDEVENTS'].length; j++) {

        currentMonth = this.allEvents['UNPLANNEDEVENTS'][j].created_at.split('-')[1] - 1;
        sumUnplannedEventsPerMonth[currentMonth] += this.allEvents['UNPLANNEDEVENTS'][j].total_duration * 1;
      }

      for (let j = 0; j < this.allEvents['CIPEVENTS'].length; j++) {

        currentMonth = this.allEvents['CIPEVENTS'][j].created_at.split('-')[1] - 1;
        sumUnplannedEventsPerMonth[currentMonth] += this.allEvents['CIPEVENTS'][j].total_duration * 1;

      }


      for (let j = 0; j < this.allEvents['EVENTS'].length; j++) {

        currentMonth = this.allEvents['EVENTS'][j].created_at.split('-')[1] - 1;
        sumUnplannedEventsPerMonth[currentMonth] += this.allEvents['EVENTS'][j].total_duration * 1;

      }

      for (let j = 0; j < this.allEvents['LOTCHANGING'].length; j++) {

        currentMonth = this.allEvents['LOTCHANGING'][j].created_at.split('-')[1] - 1;
        sumUnplannedEventsPerMonth[currentMonth] += this.allEvents['LOTCHANGING'][j].total_duration * 1;

      }


      for (let k = 0; k < this.allEvents['PLANNEDEVENTS'].length; k++) {
        currentMonth = this.allEvents['PLANNEDEVENTS'][k].created_at.split('-')[1] - 1;
        sumPlannedEventsPerMonth[currentMonth] += this.allEvents['PLANNEDEVENTS'][k].duration * 1;
      }


      for (let k = 0; k < this.allEvents['RRFMonth'].length; k++) {
        currentMonth = this.allEvents['RRFMonth'][k].created_at.split('-')[1] - 1;
        this.speedLossesPerMonth[currentMonth] += this.allEvents['RRFMonth'][k].duration;
      }

      for (let k = 0; k < this.allEvents['RRMMonth'].length; k++) {
        currentMonth = this.allEvents['RRMMonth'][k].created_at.split('-')[1] - 1;
        this.speedLossesPerMonth[currentMonth] += this.allEvents['RRMMonth'][k].duration;
      }

      for (let k = 0; k < this.allEvents['FOSMonth'].length; k++) {
        currentMonth = this.allEvents['FOSMonth'][k].created_at.split('-')[1] - 1;
        this.speedLossesPerMonth[currentMonth] += this.allEvents['FOSMonth'][k].duration;
      }

      for (let k = 0; k < this.allEvents['FSMMonth'].length; k++) {
        currentMonth = this.allEvents['FSMMonth'][k].created_at.split('-')[1] - 1;
        this.speedLossesPerMonth[currentMonth] += this.allEvents['FSMMonth'][k].duration;
      }


      for (let m = 0; m < this.plannedProductionTimePerMonth.length; m++) {
        this.plannedProductionTimePerMonth[m] = sommeWorkingTimePerMonth[m] - sumPlannedEventsPerMonth[m];
      }

      for (let n = 0; n < this.operatingTimePerMonth.length; n++) {
        this.operatingTimePerMonth[n] = sommeWorkingTimePerMonth[n] - sumPlannedEventsPerMonth[n] - sumUnplannedEventsPerMonth[n];
      }

      for (let n = 0; n < this.netOperatingTimePerMonth.length; n++) {
        this.netOperatingTimePerMonth[n] = this.operatingTimePerMonth[n] - this.speedLossesPerMonth[n];
      }


      for (let j = 0; j < this.AvailabilityPerMonth.length; j++) {
        if (this.operatingTimePerMonth[j] === 0 || this.plannedProductionTimePerMonth[j] === 0) {
          this.AvailabilityPerMonth[j] = 0;

        } else {
          this.AvailabilityPerMonth[j] = this.operatingTimePerMonth[j] / this.plannedProductionTimePerMonth[j];
        }
      }

      var n = 0;
      for(var i = 0; i<this.allEvents['SITE'].length; i++)
      {
        month = this.allEvents['SITE'][i].created_at.split('-')[1] - 1;
        n = (this.allEvents['SITE'][i].qtyProduced * this.allEvents['SITE'][i].bottlesPerCase / this.allEvents['SITE'][i].idealRate);
        nPerMonthForPerformance[month] += n;
      }

      for (let j = 0; j < this.PerformancePerMonth.length; j++) {
        if( this.operatingTimePerMonth[j] === 0 ){
          this.PerformancePerMonth[j] = 0;
        }else{
          this.PerformancePerMonth[j] = nPerMonthForPerformance[j] / this.operatingTimePerMonth[j];

        }
      }







      for (let j = 0; j < this.QualityPerMonth.length; j++) {
        if (sommeRejectionPerMonth[j] === 0 && fillerCounterPerMonth[j] === 0 && caperCounterPerMonth[j] === 0
            && labelerCounterPerMonth[j] === 0 && weightBoxCounterPerMonth[j] === 0 && qualityControlCounterPerMonth[j] === 0) {
          this.QualityPerMonth[j] = 1;
        } else {
          var s = 0

          if (fillerCounterPerMonth[j]  !== 0) {
            s += (fillerCounterPerMonth[j]  - sommeQtyProducedPerMonthForQuality[j]);
          }


          if (caperCounterPerMonth[j]  !== 0) {
            s += (caperCounterPerMonth[j] - sommeQtyProducedPerMonthForQuality[j]);
          }

          if (labelerCounterPerMonth[j] !== 0) {
            s += (labelerCounterPerMonth[j] - sommeQtyProducedPerMonthForQuality[j]);
          }

          if (weightBoxCounterPerMonth[j] !== 0) {
            s += (weightBoxCounterPerMonth[j] - sommeQtyProducedPerMonthForQuality[j]);
          }

          if (qualityControlCounterPerMonth[j] !== 0) {
            s += (qualityControlCounterPerMonth[j] - sommeQtyProducedPerMonthForQuality[j]);
          }

          if(sommeQtyProducedPerMonthForQuality[j] + sommeRejectionPerMonth[j] + s === 0 ){
            this.QualityPerMonth[j] = 1;
          }else{
            this.QualityPerMonth[j] = (sommeQtyProducedPerMonthForQuality[j]) / (sommeQtyProducedPerMonthForQuality[j] + sommeRejectionPerMonth[j] + s);
          }
          console.log("QUALITYDATA SET : ");
          console.log(sommeQtyProducedPerMonthForQuality[j]);
          console.log(sommeRejectionPerMonth[j]);
          console.log(s);
          console.log(fillerCounterPerMonth[j]);
          console.log(caperCounterPerMonth[j]);



        }
      }

      for (let j = 0; j < this.OLEPerMonth.length; j++) {
        this.OLEPerMonth[j] = this.AvailabilityPerMonth[j] * this.PerformancePerMonth[j] * this.QualityPerMonth[j];
      }

      console.log("QTY PER MONTH");
      console.log(sommeQtyProducedPerMonth);
      console.log("PERF PER MONTH");
      console.log(this.PerformancePerMonth);
      console.log("QUALITY PER MONTH");
      console.log(this.QualityPerMonth);
      console.log("AVAILABILITY PER MONTH");
      console.log(this.AvailabilityPerMonth);

      console.log("WORKING PER MONTH");
      console.log(sommeWorkingTimePerMonth);

      console.log("NET OPETATING PER MONTH");
      console.log(this.netOperatingTimePerMonth);

      console.log("PLANNED DOWNTIME  PER MONTH");
      console.log(this.plannedDowntimesPerMonth);
      console.log("UNPLANNED DOWNTIME PER MONTH");
      console.log(this.unplannedDowntimesPerMonth);
      console.log("PLannedProduction PER MONTH");
      console.log(this.plannedProductionTimePerMonth);

      console.log("OPERATING TIME PER MONTH");
      console.log(this.operatingTimePerMonth);

      console.log("SPEEDLOSS PER MONTH");
      console.log(this.speedLossesPerMonth);


    },

    loadProductionTime2: function () {

      var sommeWorkingTimePerMonth = [];
      var sommeQtyProducedPerMonth = [];
      var sommeRejectionPerMonth = [];
      var fillerCounterPerMonth = [];
      var caperCounterPerMonth = [];
      var labelerCounterPerMonth = [];
      var weightBoxCounterPerMonth = [];
      var sumPlannedEventsPerMonth = [];
      var sumUnplannedEventsPerMonth = [];


      for (let i = 0; i < 12; i++) {
        sommeWorkingTimePerMonth[i] = 0;
        sommeQtyProducedPerMonth[i] = 0;
        sommeRejectionPerMonth[i] = 0;
        fillerCounterPerMonth[i] = 0;
        caperCounterPerMonth[i] = 0;
        labelerCounterPerMonth[i] = 0;
        weightBoxCounterPerMonth[i] = 0;
        sumPlannedEventsPerMonth[i] = 0;
        sumUnplannedEventsPerMonth[i] = 0;
        this.OLEPerMonth2[i] = 0;
        this.AvailabilityPerMonth2[i] = 0;
        this.PerformancePerMonth2[i] = 0;
        this.QualityPerMonth2[i] = 0;
        this.netOperatingTimePerMonth2[i] = 0;
        this.plannedDowntimesPerMonth2[i] = 0;
        this.unplannedDowntimesPerMonth2[i] = 0;
        this.plannedProductionTimePerMonth2[i] = 0;
        this.operatingTimePerMonth2[i] = 0;
        this.speedLossesPerMonth2[i] = 0;


      }

      var month = 0;


      for (let i = 0; i < this.allEvents['SITE'].length; i++) {

        month = this.allEvents['SITE'][i].created_at.split('-')[1] - 1;

        let PO = this.allEvents['SITE'][i];

        console.log("MOIS : " + month);


        sommeWorkingTimePerMonth[month] += PO.workingDuration * 1;

        sommeQtyProducedPerMonth[month] += PO.qtyProduced * PO.bottlesPerCase * 1;

        sommeRejectionPerMonth[month] += PO.fillerRejection * 1 + PO.caperRejection * 1 + PO.labelerRejection * 1 + PO.weightBoxRejection * 1;


        fillerCounterPerMonth[month] += PO.fillerCounter * 1;
        caperCounterPerMonth[month] += PO.caperCounter * 1;
        labelerCounterPerMonth[month] += PO.labelerCounter * 1;
        weightBoxCounterPerMonth[month] += PO.weightBoxCounter * 1;


        var max = -1;
        for (let l = 0; l < sommeWorkingTimePerMonth.length; l++) {

          if (sommeWorkingTimePerMonth[l] > max) {
            max = sommeWorkingTimePerMonth[l];
            this.peakSeason = l;
          }
        }


      }

      this.plannedDowntimesPerMonth2 = sumPlannedEventsPerMonth;
      this.unplannedDowntimesPerMonth2 = sumUnplannedEventsPerMonth;
      var currentMonth = 1;
      for (let j = 0; j < this.allEvents['EVENTS'].length; j++) {

        currentMonth = this.allEvents['EVENTS'][j].created_at.split('-')[1] - 1;
        sumUnplannedEventsPerMonth[currentMonth] += this.allEvents['EVENTS'][j].total_duration * 1;


      }

      for (let k = 0; k < this.allEvents['PLANNEDEVENTS'].length; k++) {
        currentMonth = this.allEvents['PLANNEDEVENTS'][k].created_at.split('-')[1] - 1;
        sumPlannedEventsPerMonth[currentMonth] += this.allEvents['PLANNEDEVENTS'][k].duration * 1;
      }


      for (let k = 0; k < this.allEvents['RRFMonth'].length; k++) {
        currentMonth = this.allEvents['RRFMonth'][k].created_at.split('-')[1] - 1;
        this.speedLossesPerMonth2[currentMonth] += this.allEvents['RRFMonth'][k].duration;
      }

      for (let k = 0; k < this.allEvents['RRMMonth'].length; k++) {
        currentMonth = this.allEvents['RRMMonth'][k].created_at.split('-')[1] - 1;
        this.speedLossesPerMonth2[currentMonth] += this.allEvents['RRMMonth'][k].duration;
      }

      for (let k = 0; k < this.allEvents['FOSMonth'].length; k++) {
        currentMonth = this.allEvents['FOSMonth'][k].created_at.split('-')[1] - 1;
        this.speedLossesPerMonth2[currentMonth] += this.allEvents['FOSMonth'][k].duration;
      }

      for (let k = 0; k < this.allEvents['FSMMonth'].length; k++) {
        currentMonth = this.allEvents['FSMMonth'][k].created_at.split('-')[1] - 1;
        this.speedLossesPerMonth2[currentMonth] += this.allEvents['FSMMonth'][k].duration;
      }


      for (let m = 0; m < this.plannedProductionTimePerMonth2.length; m++) {
        this.plannedProductionTimePerMonth2[m] = sommeWorkingTimePerMonth[m] - sumPlannedEventsPerMonth[m];
      }

      for (let n = 0; n < this.operatingTimePerMonth2.length; n++) {
        this.operatingTimePerMonth2[n] = sommeWorkingTimePerMonth[n] - sumPlannedEventsPerMonth[n] - sumUnplannedEventsPerMonth[n];
      }

      for (let n = 0; n < this.netOperatingTimePerMonth2.length; n++) {
        this.netOperatingTimePerMonth2[n] = this.operatingTimePerMonth2[n] - this.speedLossesPerMonth2[n];
      }


      for (let j = 0; j < this.AvailabilityPerMonth2.length; j++) {
        if (this.operatingTimePerMonth2[j] === 0 || this.plannedProductionTimePerMonth2[j] === 0) {
          this.AvailabilityPerMonth2[j] = 0;

        } else {
          this.AvailabilityPerMonth2[j] = this.operatingTimePerMonth2[j] / this.plannedProductionTimePerMonth2[j];
        }
      }

      for (let j = 0; j < this.PerformancePerMonth2.length; j++) {
        if (this.netOperatingTimePerMonth2[j] === 0 || this.operatingTimePerMonth2[j] === 0) {
          this.PerformancePerMonth2[j] = 0;
        } else {
          this.PerformancePerMonth2[j] = this.netOperatingTimePerMonth2[j] / this.operatingTimePerMonth2[j];
        }
      }


      for (let j = 0; j < this.QualityPerMonth2.length; j++) {
        if (sommeRejectionPerMonth[j] === 0 && fillerCounterPerMonth[j] === 0 && caperCounterPerMonth[j] === 0
            && labelerCounterPerMonth[j] === 0 && weightBoxCounterPerMonth[j] === 0) {
          this.QualityPerMonth2[j] = 1;
        } else {
          var s = (fillerCounterPerMonth[j] - sommeQtyProducedPerMonth[j]) + (caperCounterPerMonth[j] - sommeQtyProducedPerMonth[j])
              + (labelerCounterPerMonth[j] - sommeQtyProducedPerMonth[j]) + (weightBoxCounterPerMonth[j] - sommeQtyProducedPerMonth[j]);
          this.QualityPerMonth2[j] = (sommeQtyProducedPerMonth[j]) / (sommeQtyProducedPerMonth[j] + sommeRejectionPerMonth[j] + s);

        }
      }

      for (let j = 0; j < this.OLEPerMonth2.length; j++) {
        this.OLEPerMonth2[j] = this.AvailabilityPerMonth2[j] * this.PerformancePerMonth2[j] * this.QualityPerMonth2[j];
      }


    },

    graph2: function () {
      var chart = document.getElementById("myChart4").getContext('2d');
      var chartMaison = {
        type: "bar",
        data: {
          labels: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
          datasets: [
            {
              label: "Overall Line Effectiveness",
              type: "bar",
              showInLegend: true,
              data: [this.OLEPerMonth[0] * 100,
                      this.OLEPerMonth[1] * 100,
                      this.OLEPerMonth[2] * 100,
                      this.OLEPerMonth[3] * 100,
                      this.OLEPerMonth[4] * 100,
                      this.OLEPerMonth[5] * 100,
                      this.OLEPerMonth[6] * 100,
                      this.OLEPerMonth[7] * 100,
                      this.OLEPerMonth[8] * 100,
                      this.OLEPerMonth[9] * 100,
                      this.OLEPerMonth[10] * 100,
                      this.OLEPerMonth[11] * 100
              ],
              backgroundColor: "rgba(71, 183,132,.5)",
              borderColor: "#47b784",
              borderWidth: 3
            },
            {
              type: "line",
              label: "Performance",
              showInLegend: true,
              backgroundColor: "rgba(214, 2, 2,.5)",
              borderColor: "#FF0000",
              data: [this.PerformancePerMonth[0] * 100,
                this.PerformancePerMonth[1] * 100,
                this.PerformancePerMonth[2] * 100,
                this.PerformancePerMonth[3] * 100,
                this.PerformancePerMonth[4] * 100,
                this.PerformancePerMonth[5] * 100,
                this.PerformancePerMonth[6] * 100,
                this.PerformancePerMonth[7] * 100,
                this.PerformancePerMonth[8] * 100,
                this.PerformancePerMonth[9] * 100,
                this.PerformancePerMonth[10] * 100,
                this.PerformancePerMonth[11] * 100
              ]
            },
            {
              type: "line",
              label: "Availability",
              showInLegend: true,
              backgroundColor: "rgba(125, 60, 152 ,.5)",
              borderColor: "#7D3C98",
              data: [this.AvailabilityPerMonth[0] * 100,
                this.AvailabilityPerMonth[1] * 100,
                this.AvailabilityPerMonth[2] * 100,
                this.AvailabilityPerMonth[3] * 100,
                this.AvailabilityPerMonth[4] * 100,
                this.AvailabilityPerMonth[5] * 100,
                this.AvailabilityPerMonth[6] * 100,
                this.AvailabilityPerMonth[7] * 100,
                this.AvailabilityPerMonth[8] * 100,
                this.AvailabilityPerMonth[9] * 100,
                this.AvailabilityPerMonth[10] * 100,
                this.AvailabilityPerMonth[11] * 100
              ]
            },
            {
              type: "line",
              label: "Quality",
              showInLegend: true,
              backgroundColor: "rgba(241, 196, 15,.5)",
              borderColor: "#F1C40F",
              data: [this.QualityPerMonth[0] * 100,
                this.QualityPerMonth[1] * 100,
                this.QualityPerMonth[2] * 100,
                this.QualityPerMonth[3] * 100,
                this.QualityPerMonth[4] * 100,
                this.QualityPerMonth[5] * 100,
                this.QualityPerMonth[6] * 100,
                this.QualityPerMonth[7] * 100,
                this.QualityPerMonth[8] * 100,
                this.QualityPerMonth[9] * 100,
                this.QualityPerMonth[10] * 100,
                this.QualityPerMonth[11] * 100
              ]
            }
          ]
        },
        options: {
          responsive: true,
          scales: {

            /**
            yAxes: [
              {
                ticks: {
                  beginAtZero: true,
                  padding: 25
                }
              }
            ]
             **/
          }
        }
      };
      if(this.chartFinal instanceof Chart){
        this.chartFinal.destroy();
      }
      this.chartFinal = new Chart(chart, chartMaison);

/**
      chart = chart.Chart("chartContainer", {
        animationEnabled: true,
        theme: "light2",
        title: {
          text: "Overall Line Effectiveness"
        },
        axisX: {
          valueFormatString: "MMM"
        },
        axisY: {},
        toolTip: {
          shared: true
        },
        legend: {
          cursor: "pointer",
          //itemclick: this.toggleDataSeries
        },

        data: [
          {
            type: "column",
            name: "OLE",
            showInLegend: true,
            xValueFormatString: "MMMM YYYY",
            yValueFormatString: "#,##0",
            dataPoints: [
              {x: new Date(this.year, 0), y: this.OLEPerMonth[0] * 100},
              {x: new Date(this.year, 1), y: this.OLEPerMonth[1] * 100},
              {x: new Date(this.year, 2), y: this.OLEPerMonth[2] * 100},
              {x: new Date(this.year, 3), y: this.OLEPerMonth[3] * 100},
              {x: new Date(this.year, 4), y: this.OLEPerMonth[4] * 100},
              {x: new Date(this.year, 5), y: this.OLEPerMonth[5] * 100},
              {x: new Date(this.year, 6), y: this.OLEPerMonth[6] * 100},
              {x: new Date(this.year, 7), y: this.OLEPerMonth[7] * 100},
              {x: new Date(this.year, 8), y: this.OLEPerMonth[8] * 100},
              {x: new Date(this.year, 9), y: this.OLEPerMonth[9] * 100},
              {x: new Date(this.year, 10), y: this.OLEPerMonth[10] * 100},
              {x: new Date(this.year, 11), y: this.OLEPerMonth[11] * 100}
            ]
          },
          {
            type: "line",
            name: "Performance",
            showInLegend: true,
            yValueFormatString: "#,##0",
            dataPoints: [
              {x: new Date(this.year, 0), y: this.PerformancePerMonth[0] * 100},
              {x: new Date(this.year, 1), y: this.PerformancePerMonth[1] * 100},
              {x: new Date(this.year, 2), y: this.PerformancePerMonth[2] * 100},
              {x: new Date(this.year, 3), y: this.PerformancePerMonth[3] * 100},
              {x: new Date(this.year, 4), y: this.PerformancePerMonth[4] * 100},
              {x: new Date(this.year, 5), y: this.PerformancePerMonth[5] * 100},
              {x: new Date(this.year, 6), y: this.PerformancePerMonth[6] * 100},
              {x: new Date(this.year, 7), y: this.PerformancePerMonth[7] * 100},
              {x: new Date(this.year, 8), y: this.PerformancePerMonth[8] * 100},
              {x: new Date(this.year, 9), y: this.PerformancePerMonth[9] * 100},
              {x: new Date(this.year, 10), y: this.PerformancePerMonth[10] * 100},
              {x: new Date(this.year, 11), y: this.PerformancePerMonth[11] * 100}
            ]
          },
          {
            type: "line",
            name: "Availability",
            showInLegend: true,
            yValueFormatString: "#,##0",
            dataPoints: [
              {x: new Date(this.year, 0), y: this.AvailabilityPerMonth[0] * 100},
              {x: new Date(this.year, 1), y: this.AvailabilityPerMonth[1] * 100},
              {x: new Date(this.year, 2), y: this.AvailabilityPerMonth[2] * 100},
              {x: new Date(this.year, 3), y: this.AvailabilityPerMonth[3] * 100},
              {x: new Date(this.year, 4), y: this.AvailabilityPerMonth[4] * 100},
              {x: new Date(this.year, 5), y: this.AvailabilityPerMonth[5] * 100},
              {x: new Date(this.year, 6), y: this.AvailabilityPerMonth[6] * 100},
              {x: new Date(this.year, 7), y: this.AvailabilityPerMonth[7] * 100},
              {x: new Date(this.year, 8), y: this.AvailabilityPerMonth[8] * 100},
              {x: new Date(this.year, 9), y: this.AvailabilityPerMonth[9] * 100},
              {x: new Date(this.year, 10), y: this.AvailabilityPerMonth[10] * 100},
              {x: new Date(this.year, 11), y: this.AvailabilityPerMonth[11] * 100}
            ]
          },
          {
            type: "line",
            name: "Quality",
            showInLegend: true,
            yValueFormatString: "#,##0",
            dataPoints: [
              {x: new Date(this.year, 0), y: this.QualityPerMonth[0] * 100},
              {x: new Date(this.year, 1), y: this.QualityPerMonth[1] * 100},
              {x: new Date(this.year, 2), y: this.QualityPerMonth[2] * 100},
              {x: new Date(this.year, 3), y: this.QualityPerMonth[3] * 100},
              {x: new Date(this.year, 4), y: this.QualityPerMonth[4] * 100},
              {x: new Date(this.year, 5), y: this.QualityPerMonth[5] * 100},
              {x: new Date(this.year, 6), y: this.QualityPerMonth[6] * 100},
              {x: new Date(this.year, 7), y: this.QualityPerMonth[7] * 100},
              {x: new Date(this.year, 8), y: this.QualityPerMonth[8] * 100},
              {x: new Date(this.year, 9), y: this.QualityPerMonth[9] * 100},
              {x: new Date(this.year, 10), y: this.QualityPerMonth[10] * 100},
              {x: new Date(this.year, 11), y: this.QualityPerMonth[11] * 100}
            ]
          }]
      }, null );
      chart.render();
**/
    },


  },


  async mounted() {
    if (sessionStorage.getItem("language") !== null) {
      this.$i18n.locale = sessionStorage.getItem("language");
    }

    if(sessionStorage.getItem("user-status") == 1){
      await axios.get(urlAPI + 'sites/'+this.username)
          .then(response => (this.dataSite = response.data))
    }else{
      await axios.get(urlAPI + 'sites')
          .then(response => (this.dataSite = response.data))
    }


    this.dataWorksite = this.dataSite[0];
    this.dataProductionlines = this.dataSite[1];
    this.dataTeams = this.dataSite[2];

    let externalScript = document.createElement('script');
    externalScript.setAttribute('src', 'https://canvasjs.com/assets/script/canvasjs.min.js');
    document.head.appendChild(externalScript);

    for (let i = this.startYear; i <= this.currentYear; i++) this.years.push(i);

  },



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


.wrapper {
  width: 60%;
  display: block;
  overflow: hidden;
  margin: 0 auto;
  padding: 60px 50px;
  background: #fff;
  border-radius: 4px;
}

canvas {
  background: #fff;
  height: 400px;
}

div.data {
  flex-direction: column;
  border: solid 1px;
  border-radius: 5px;
  padding: 10px 5px;
}

div.dataInput {
  border-bottom: solid 1px;

}


div {
  padding: 15px;
  background-color: #fff;

}


</style>
