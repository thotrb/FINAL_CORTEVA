<template>

  <div>
    <template v-if="displayNumber === 0">
      <div align="center">
        <div align="center" class="col productionName rcorners2">
          {{$t("performance")}}
        </div>


        <template v-if="historicPO.length > 0">

          <div align="center" class="col productionName rcorners1">
            {{$t("previousPOYourTeamWorkedOn")}}
          </div>

          <div class="container table-info-data" align="center">
            <table class="table">
              <thead class="thead-dark">
              <tr>
                <th scope="col">{{$t("productionOrder")}}</th>
                <th scope="col">{{$t("date")}}</th>
                <th scope="col">{{$t("startTime")}}</th>
                <th scope="col">{{$t("endTime")}}</th>
                <th scope="col">{{$t("finalQuantityProduced(Cases)")}}</th>

              </tr>
              </thead>
              <tbody>
                <tr v-for="shift in historicPO" :key="shift.created_at">
                  <th scope="row">{{shift.number}}</th>
                  <td>{{shift.created_at.split('T')[0]}}</td>
                  <td>{{shift.workingDebut}}</td>
                  <td>{{shift.workingEnd}}</td>
                  <td>{{shift.qtyProduced}}</td>
                </tr>
              </tbody>
            </table>


          </div>
          <br/>
          <br/>
          <br/>
        </template>

        <form>
          <div class="form-group row blockInput">
            <label class="col-sm-2 col-form-label rcorners1" for="startingPO">{{$t("POStartTime")}}</label>
            <div class="col-sm-10">
              <!--<input type="number" id="startingPO" class="form-control-plaintext rcorners2"
              v-bind:value="startPO" disabled>-->

              <input type="time" id="startingPO" class="form-control-plaintext rcorners2"
                     min="00:00" max="24:00" required v-model="startPO">
            </div>

          </div>

          <div class="form-group row blockInput">
            <label for="endingPO" class="col-sm-2 rcorners1">{{$t("POEndTime")}}</label>
            <div class="col-sm-10">

              <input type="time" id="endingPO" class="form-control-plaintext rcorners2"
                     min="00:00" max="24:00" required v-model="endPO">
            </div>
          </div>

          <div class="form-group row blockInput">
            <label for="finalQuantityProduced" class="col-sm-2 rcorners1">{{$t("finalQuantityProduced(Cases)")}}<br>
            </label>
            <div class="col-sm-10">
              <input type="number" id="finalQuantityProduced" class="form-control-plaintext rcorners2"
                     v-model="finalQuantityProduced">
            </div>
          </div>

          <button class="btn btn-primary d-flex align-items-center btn-info" type="button"
                  @click.prevent="validateCalculation()">
            {{$t("validate")}}
          </button>


        </form>
        <br/>
        <br/>

        <template v-if="valider === 1">
          <div align="center" class="rcorners2">

            <div align="left">

              {{$t("totalPOProductionTime")}} (min): {{totalProductionTime}}
              <br/>
              {{$t("totalPOOperatingTime")}} (min): {{totalOperatingTime}}
              <br/>
              {{$t("difference")}}(min): {{totalProductionTime - totalOperatingTime}}
              <br/>
              <span style="color:red">
                {{$t("speedLossesToJustify")}} (min): {{differenceToJustify.toFixed(0)}}
              </span>
              <br/>

              {{$t("totalPOPerformance")}} (%) : {{(performance * 100).toFixed(2)}}
            </div>

            <br/>

            <template v-if="speedLoss.length <= 0">
              <h4>{{$t("noPerformanceLossRegistered")}}</h4>
            </template>
            <template v-else>
              <table class="table">
                <thead>
                <tr>
                  <th scope="col">{{$t("reason")}}</th>
                  <th scope="col">{{$t("duration")}}</th>
                  <th scope="col">{{$t("comments")}}</th>
                  <th scope="col">{{$t("action")}}</th>

                </tr>
                </thead>
                <tbody>
                  <tr  v-for="event in speedLoss" :key="event.id">
                    <th scope="row">{{$t(event.reason)}}</th>
                    <td>{{event.duration}}</td>
                    <td>{{event.comment}}</td>
                    <td><button type="button" id="deleteSpeedLossButton" class="btn btn-danger" @click="deleteSpeedloss(event.id)">{{$t("delete")}}</button> </td>

                  </tr>
                </tbody>

              </table>
            </template>
          </div>

        </template>


        <div align="right">
          <button class="btn btn-primary border-danger align-items-right btn-danger" type="button"
                  @click.prevent="backPage()">
            {{$t("cancel")}}
          </button>
        </div>

        <template v-if="valider===1">

          <div class="d-flex flex-row justify-content-between align-items-center bg-white">
            <button class="btn btn-primary d-flex align-items-center btn-danger" type="button"
                    @click.prevent="backPage()">
              {{$t("back")}}
            </button>

            <button class="btn btn-primary d-flex align-items-center btn-warning" id="addReasonButton"
                    type="button"
                    @click.prevent="addSpeedLoss()">
              {{$t("speedLossJustification")}}
            </button>


            <button class="btn btn-primary border-success align-items-center btn-success" type="button"
                    @click.prevent="validateInformations()">
              OK
            </button>
          </div>
        </template>


      </div>
    </template>

    <template v-if="displayNumber ===1">
      <div>
        <div align="center" class="productionName rcorners2">
          {{$t("speedLoss")}} {{nbSpeedLosses + 1}}
        </div>

        <div id="formSpeedLoss"></div>

        <br/>

        <div class="row" align="center" style="margin-bottom: 30px;">

          <div class="col-sm-4">
            <label for="answer1" class="checkbox labelsAnswer">
              <input type="radio"
                     id="answer1" name="reponseQuestion"
                     value="reducedRateAtFiller" class="response">
              <span>{{$t("reducedRateAtFiller")}}</span>
            </label>

          </div>

          <div class="col-sm-4">
            <label for="answer2" class="checkbox labelsAnswer">
              <input type="radio"
                     id="answer2" name="reponseQuestion"
                     value="reducedRateAtAnOtherMachine" class="response">
              <span>{{$t("reducedRateAtAnOtherMachine")}}</span>
            </label>


          </div>

          <div class="col-sm-4">

            <label for="answer3" class="checkbox labelsAnswer">
              <input type="radio"
                     id="answer3" name="reponseQuestion"
                     value="fillerOwnStoppage" class="response">
              <span>{{$t("fillerOwnStoppage")}}</span>
            </label>


          </div>

          <div class="col-sm-4">
            <label for="answer4" class="checkbox labelsAnswer">
              <input type="radio"
                     id="answer4" name="reponseQuestion"
                     value="fillerOwnStoppageByAnOtherMachine" class="response">
              <span>{{$t("fillerOwnStoppageByAnOtherMachine")}}</span>
            </label>

          </div>

        </div>


        <br>

        <form id="needs-validation" novalidate>
          <div class="form-group row">
            <label for="comments" class="col-sm-2 rcorners1">{{$t("comments")}}</label>
            <div class="col-sm-10">
              <textarea id="comments" class="form-control"></textarea>
            </div>
          </div>
          <div class="form-group row">
            <label for="comments" class="col-sm-2 rcorners1">{{$t("duration")}} (min)</label>
            <div class="col-sm-10">
              <input id="sl-duration" class="form-control" style="min-width: 100px;" type="number" min="1" step="1" required/>
            </div>
          </div>
          <div class="d-flex flex-row justify-content-between align-items-center bg-white">
            <button class="btn btn-primary d-flex align-items-center btn-danger" type="button"
                    @click.prevent="resetPage()">
              {{$t("back")}}
            </button>


            <button class="btn align-items-center btn-success" type="button"
                    @click.prevent="confirmSpeedloss()">
              OK
            </button>
          </div>
        </form>



      </div>


    </template>

    <template v-if="displayNumber===2">

      <div align="center">
        <div align="center" class="col productionName rcorners2">
          {{$t("quality")}}
        </div>

        <div align="left" style="margin-left: 50px">
          {{$t("numberOfBottlesProduced")}} : {{nbBottlesFilled}}<br/>
          {{$t("numberOfCasesProduced")}} : {{finalQuantityProduced}}

        </div>


        <table class="table table-hover" align="center">
          <thead>
          <tr>
            <th scope="col"></th>
            <th scope="col">{{$t("filler")}}</th>
            <th scope="col">{{$t("caper")}}</th>
            <th scope="col">{{$t("labeler")}}</th>
            <th scope="col">{{$t("boxWeigher")}}</th>
            <th scope="col">{{$t("qualityControl")}}</th>


          </tr>
          </thead>
          <tbody>
          <tr>
            <th scope="row">{{$t("counter")}}</th>
            <td>
              <input type="number" id="FillerCounter" class="rcorners2" v-model="FillerCounter">
            </td>
            <td>
              <input type="number" id="CaperCounter" class="rcorners2" v-model="CaperCounter">
            </td>
            <td>
              <input type="number" id="EtiqueteuseCounter" class="rcorners2"
                     v-model="EtiqueteuseCounter">
            </td>
            <td>
              <input type="number" id="WieghtBoxCounter" class="rcorners2" v-model="WieghtBoxCounter">
            </td>

            <td>
              <input type="number" id="qualityControlCounter" class="rcorners2"
                     v-model="QualityControlCounter">
            </td>

          </tr>
          <tr>
            <th scope="row">{{$t("rejection")}}</th>
            <td>
              <input type="number" id="FillerRejection" class="rcorners2" v-model="FillerRejection">
            </td>
            <td>
              <input type="number" id="CaperRejection" class="rcorners2" v-model="CaperRejection">
            </td>
            <td>
              <input type="number" id="EtiqueteuseRejection" class="rcorners2"
                     v-model="EtiqueteuseRejection">
            </td>
            <td>
              <input type="number" id="WieghtBoxRejection" class="rcorners2"
                     v-model="WieghtBoxRejection">
            </td>
            <td>
              <input type="number" id="qualityControlRejection" class="rcorners2"
                     v-model="QualityControlRejection">
            </td>


          </tr>

          </tbody>
        </table>
      </div>

      <div class="d-flex flex-row justify-content-between align-items-center bg-white">
        <button class="btn btn-primary d-flex align-items-center btn-danger" type="button"
                @click.prevent="resetPage()">
          {{$t("back")}}
        </button>


        <button class="btn btn-primary border-success align-items-center btn-success" type="button"
                @click.prevent="displayQualityIndicators()">
          OK
        </button>
      </div>
      <!--<quality-declaration></quality-declaration>-->

    </template>

    <template v-if="displayNumber===3">

      <h2 align="center">{{$t("summary")}}</h2>

      <br>

      <span>

                {{$t("totalPOProductionTime")}} (min): {{totalProductionTime}}
                <br/>
                {{$t("totalPOOperatingTime")}} (min): {{totalOperatingTime}}
                <br/>
                {{$t("difference")}}(min): {{totalProductionTime - totalOperatingTime}}
                <br/>
                {{$t("speedLossesToJustify")}} (min): {{differenceToJustify.toFixed(0)}}
            </span>

      <br/>


      <h5>{{$t("speedlosses")}}</h5>

      <template v-if="speedLoss.length <= 0">
        <h5>{{$t("noPerformanceRegistered")}}</h5>
      </template>


      <template v-else>
        <table class="table">
          <thead>
          <tr>
            <th scope="col">{{$t("reason")}}</th>
            <th scope="col">{{$t("comments")}}</th>
          </tr>
          </thead>
          <tbody>
            <tr  v-for="event in speedLoss" :key="event.id">
              <th scope="row">{{event.reason}}</th>
              <td>{{event.comment}}</td>
            </tr>
          </tbody>

        </table>
      </template>


      <h5>{{$t("quality")}}</h5>

      <br/>



      <table class="table table-hover" align="center">
        <thead>
        <tr>
          <th scope="col"></th>
          <th scope="col"> {{$t("filler")}}</th>
          <th scope="col">{{$t("caper")}}</th>
          <th scope="col">{{$t("labeler")}}</th>
          <th scope="col">{{$t("boxWeigher")}}</th>
          <th scope="col">{{$t("qualityControl")}}</th>


        </tr>
        </thead>
        <tbody>
        <tr>
          <th scope="row">{{$t("counter")}}</th>
          <td>
            {{FillerCounter}}
          </td>
          <td>
            {{CaperCounter}}
          </td>
          <td>
            {{EtiqueteuseCounter}}
          </td>
          <td>
            {{WieghtBoxCounter}}
          </td>
          <td>
            {{QualityControlCounter}}
          </td>

        </tr>
        <tr>
          <th scope="row">{{$t("rejection")}}</th>
          <td>
            {{FillerRejection}}
          </td>
          <td>
            {{CaperRejection}}
          </td>
          <td>
            {{EtiqueteuseRejection}}
          </td>
          <td>
            {{WieghtBoxRejection}}
          </td>
          <td>
            {{QualityControlRejection}}
          </td>

        </tr>

        </tbody>
      </table>
      <br/>

      <h5>{{$t("indicators")}}</h5>

      <span>

                 {{$t("availability")}} (%) :  {{(availability * 100).toFixed(2)}}
                <br/>
                 {{$t("performance")}} (%) :  {{(performance * 100).toFixed(2)}}
                <br/>
                 {{$t("quality")}} (%) :  {{(quality * 100).toFixed(2)}}
                <br/>
                 OLE (%) :  {{(OLE * 100).toFixed(2)}}

            </span>

      <br/>

      <div class="d-flex flex-row justify-content-between align-items-center bg-white">
        <button class="btn btn-primary d-flex align-items-center btn-danger" type="button"
                @click.prevent="backPage2()">
          {{$t("back")}}
        </button>


        <button class="btn btn-primary border-success align-items-center btn-success" type="button"
                @click.prevent="saveEndPO()">
          {{$t("validate")}}
        </button>
      </div>
    </template>

  </div>
</template>

<script>

import axios from "axios";
import router from "@/router";
import { urlAPI} from "@/variables";

export default {
  name: "EndPO_Declaration",


  data() {
    return {

      totalUnplannedDowtimes: sessionStorage.getItem("sommeUnplannedEvents"),
      totalPlannedDowtimes: sessionStorage.getItem("sommePlannedEvents"),


      indice: sessionStorage.getItem("indice"),
      PO: sessionStorage.getItem("pos").split(',')[this.indice],

      startPO:  sessionStorage.getItem("time1"),


      FillerCounter: 0,
      CaperCounter: 0,
      EtiqueteuseCounter: 0,
      WieghtBoxCounter: 0,
      QualityControlCounter: 0,
      FillerRejection: 0,
      CaperRejection: 0,
      EtiqueteuseRejection: 0,
      WieghtBoxRejection: 0,
      QualityControlRejection: 0,
      availability: 0,
      performance: 0,

      finalQuantityProduced: sessionStorage.getItem("finalQuantityProduced"),


      nbSpeedLosses: 0,

      url: sessionStorage.getItem("url"),

      displayNumber: 0,

      productionName: sessionStorage.getItem("productionName"),


      prodlines: sessionStorage.getItem("prodlines").split(','),

      parameters: [],

      username: localStorage.getItem("username"),


      speedLossEvent: {
        OLE: '',

        productionline: sessionStorage.getItem("productionName"),

        reason: '',
        //commentaire
        comment: '',

        shift : '',

        created_at: '',
      },


      totalOperatingTime: 0,

      totalNetOperatingTime: 0,

      totalProductionTime: 0,

      totalSpeedLosses : 0,

      totalPOQuality: 0,

      endPO: sessionStorage.getItem("time2"),

      GMID: sessionStorage.getItem("GMIDCODE"),

      valider: 0,

      nbBottlesFilled: 0,

      totalDuration: 0,

      quality: 1,

      speedLoss:null,
      netOP : null,
      performanceIndexes : null,

      typeTeam : sessionStorage.getItem("typeTeam"),

      differenceToJustify : 0,

      historicPO: [],



    };
  },

  methods: {

    deleteSpeedloss : async function (id) {

      await axios.delete(urlAPI + "deleteSpeedloss/" + id);
      location.reload();

    },

    backPage2: function () {
      this.FillerCounter= 0; this.CaperCounter= 0; this.EtiqueteuseCounter= 0;
      this.WieghtBoxCounter= 0; this.QualityControlCounter = 0; this.FillerRejection= 0;
      this.CaperRejection= 0; this.EtiqueteuseRejection= 0; this.WieghtBoxRejection= 0;
      this.QualityControlRejection= 0;
      this.displayNumber = 2;

    },
    errorMessage: function () {
      var h1 = document.getElementsByClassName("error");
      if (h1.length <= 0) {
        let error = document.createElement('h1');
        error.setAttribute("class", "error");
        error.innerHTML = this.$t("errorInput");
        error.setAttribute("style", "color:red;")
        error.setAttribute("align", "center");
        let br = document.createElement('br');

        let form = document.getElementById("form");
        form.insertBefore(br, form.firstChild);
        form.insertBefore(error, form.firstChild);
      }

    },

    errorMessage2: function () {
      var h1 = document.getElementsByClassName("errorS");
      if (h1.length <= 0) {
        let error = document.createElement('h1');
        error.setAttribute("class", "errorS");
        error.innerHTML = this.$t("errorInput");
        error.setAttribute("style", "color:red;")
        error.setAttribute("align", "center");
        //let br = document.createElement('br');

        let form = document.getElementById("formSpeedLoss");
        //form.insertBefore(br, form.firstChild);
        form.appendChild(error);
        //form.insertBefore(error, form.firstChild);
      }

    },

    validateCalculation: async function () {
      console.log("CLICK")
      console.log(document.getElementById('endingPO').value);


      if(sessionStorage.getItem("time1") === null){
        sessionStorage.time1 = this.startPO;
      }else{
        sessionStorage.setItem("time1",this.startPO);
      }


      if(sessionStorage.getItem("time2") === null){
        sessionStorage.time2 = this.endPO;
      }else{
        sessionStorage.setItem("time2",this.endPO);
      }

      if(sessionStorage.getItem("finalQuantityProduced") === null){
        sessionStorage.finalQuantityProduced = this.finalQuantityProduced;
      }else{
        sessionStorage.setItem("finalQuantityProduced",this.finalQuantityProduced);
      }



      var splitted1 = this.startPO.toString().split(':');
      var splitted2 = this.endPO.toString().split(':');

      var time1 = 0;
      var time2 = 0;


      if (splitted1[0] <= splitted2[0]) {

        time1 = this.startPO.toString().split(':')[0] * 60 + this.startPO.toString().split(':')[1] * 1;

        time2 = this.endPO.toString().split(':')[0] * 60 + this.endPO.toString().split(':')[1] * 1;

        this.totalProductionTime = time2 - time1;

        this.totalDuration = time2 - time1;


      } else {

        time1 = 24 * 60 - (this.startPO.toString().split(':')[0] * 60 + this.startPO.toString().split(':')[1] * 1);

        time2 = this.endPO.toString().split(':')[0] * 60 + this.endPO.toString().split(':')[1] * 1;


        this.totalProductionTime = time1 + time2;

        this.totalDuration = time2 + time1;


      }








      if (isNaN(time1) || isNaN(time2)) {
        this.errorMessage();
      } else {
        this.totalProductionTime -= (this.totalPlannedDowtimes);
        this.totalOperatingTime = this.totalProductionTime - this.totalUnplannedDowtimes;

        if (this.totalOperatingTime === 0 || this.totalProductionTime === 0) {
          this.availability = 0;
        } else {
          this.availability = (this.totalOperatingTime / this.totalProductionTime).toFixed(3);

        }
        if(this.availability > 1){
          this.availability = 1;
        }

        //this.$store.dispatch('getNetOPTime', this.parameters);

        this.nbBottlesFilled = this.finalQuantityProduced * this.netOP[0].bottlesPerCase;
        this.normalRate = this.netOP[0].bottlesPerCase * this.netOP[0].idealRate;

        this.totalNetOperatingTime = this.totalOperatingTime - this.totalSpeedLosses;

        if (this.totalOperatingTime === 0) {
          this.differenceToJustify = 0;
        } else {
          this.differenceToJustify = (this.totalOperatingTime * this.netOP[0].idealRate - this.finalQuantityProduced * this.netOP[0].bottlesPerCase) / this.netOP[0].idealRate
        }

        console.log('OH DIFF : ' + this.differenceToJustify);
        console.log('OP time' + this.totalOperatingTime);
        console.log('NET OP : ' + this.totalNetOperatingTime);
        console.log(this.nbBottlesFilled);
        console.log(this.netOP);


        console.log(this.speedLoss);

        var numerateur = this.nbBottlesFilled / this.netOP[0].idealRate;

        console.log(this.nbBottlesFilled);
        console.log(this.netOP);
        console.log(numerateur);
        //this.performance = (this.nbBottlesFilled / this.normalRate).toFixed(2);
        if(this.finalQuantityProduced === '0' ||this.finalQuantityProduced === 0 ){
          this.performance = 0;
        }else{
          this.performance = (numerateur / this.totalOperatingTime);
          if(this.performance > 1){
            this.performance = 1;
          }
        }

        console.log('Perf : ' + this.performance);

        this.valider = 1;

      }


    },

    backPage: function () {
      router.replace('/homePage');
    },

    resetPage: function () {
      this.FillerCounter= 0; this.CaperCounter= 0; this.EtiqueteuseCounter= 0;
      this.WieghtBoxCounter= 0; this.QualityControlCounter = 0; this.FillerRejection= 0;
      this.CaperRejection= 0; this.EtiqueteuseRejection= 0; this.WieghtBoxRejection= 0;
      this.QualityControlRejection= 0;
      this.displayNumber = 0;
    },

    addSpeedLoss: function () {
      this.displayNumber = 1;
    },

    displayQualityIndicators: function () {

      if ((this.isCompteurNull() && this.isRejectionNull()) || (this.isCompteurNull2() && this.isRejectionNull2())) {
        this.quality = 1;
      } else {
        console.log(this.EtiqueteuseCounter);
        console.log(this.WieghtBoxRejection);
        console.log(this.QualityControlCounter);
        console.log(this.FillerRejection);
        console.log(this.CaperRejection);
        console.log(this.QualityControlRejection);
        console.log(this.EtiqueteuseRejection);
        console.log(this.FillerCounter);
        console.log(this.CaperCounter);
        console.log(this.WieghtBoxCounter);





        var N = this.nbBottlesFilled;
        var summRejection = this.FillerRejection*1 + this.CaperRejection*1 +
            this.EtiqueteuseRejection*1 + this.WieghtBoxRejection*this.netOP[0].bottlesPerCase + this.QualityControlRejection*1*this.netOP[0].bottlesPerCase;

        var summCompteur = 0;

        console.log('N : ' + N);
        console.log('SUMM REJ : ' + summRejection);

        if (this.FillerCounter !== 0) {
          summCompteur += (this.FillerCounter - N);
          console.log('ICI1 : ' + summCompteur);
        }


        if (this.CaperCounter !== 0) {
          summCompteur += (this.CaperCounter - N);
          console.log('ICI2 : ' + summCompteur);

        }

        if (this.EtiqueteuseCounter !== 0 && this.EtiqueteuseCounter !== '0') {
          summCompteur += (this.EtiqueteuseCounter - N);
          console.log('ICI3 : ' + summCompteur);
          console.log('ICI3 : ' + this.EtiqueteuseCounter);

        }

        if (this.QualityControlCounter !== 0) {
          summCompteur += (this.QualityControlCounter*this.netOP[0].bottlesPerCase - N);
          console.log('ICI4 : ' + summCompteur);
        }


        if (this.WieghtBoxCounter !== 0) {
          summCompteur += (this.WieghtBoxCounter*this.netOP[0].bottlesPerCase - N);
          console.log('ICI5 : ' + summCompteur);

        }


        console.log('NB BOUTEILLES : ' + N);
        console.log('NB BOUTEILLES REJ : ' + summRejection);
        console.log('NB BOUTEILLES COUNT : ' + summCompteur);

        if(N + summRejection + summCompteur === 0 ){
          this.quality = 1;
        }else{
          this.quality = (N / (N + summRejection + summCompteur));
        }

        if(this.quality > 1){
          this.quality = 1;
        }
      }
      this.OLE = (this.quality * this.availability * this.performance);

      if (sessionStorage.getItem("quality") === null) {
        sessionStorage.quality = this.quality;
      } else {
        sessionStorage.setItem("quality", this.quality);
      }

      if (sessionStorage.getItem("performance") === null) {
        sessionStorage.performance = this.performance;
      } else {
        sessionStorage.setItem("performance", this.performance);
      }

      if (sessionStorage.getItem("availability") === null) {
        sessionStorage.availability = this.availability;
      } else {
        sessionStorage.setItem("availability", this.availability);
      }

      if (sessionStorage.getItem("OLE") === null) {
        sessionStorage.OLE = this.OLE;
      } else {
        sessionStorage.setItem("OLE", this.OLE);
      }



      this.displayNumber = 3;

    },

    saveEndPO: async function () {


      var end = this.endPO;


      this.endPO = sessionStorage.getItem("pos").split(',')[this.indice];


      console.log("Total duration : " + this.totalDuration);
      console.log("TIME : " + this.totalOperatingTime);
      console.log("TIME : " + this.totalNetOperatingTime);

      console.log("START : " + this.startPO);

      console.log("END : " + end);

      let selectedDate = sessionStorage.getItem("dateInput");


      await axios.post(urlAPI+'stopPO/'+this.endPO+'/'+this.availability+'/'+this.performance+'/'+this.quality+'/'+
          this.OLE+'/'+this.finalQuantityProduced+'/'+this.totalDuration+'/'+this.totalOperatingTime + '/'+this.totalNetOperatingTime+'/'+
          sessionStorage.getItem("typeTeam")+'/'+this.startPO+'/'+end+"/"+selectedDate );

      await this.resolveAfter1Second();

      if (this.FillerCounter === 0  || this.FillerCounter === '' || this.FillerCounter === undefined || this.FillerCounter === null) {
        this.FillerCounter = this.nbBottlesFilled;

      }
      if (this.CaperCounter === 0 || this.CaperCounter === '' || this.CaperCounter === undefined || this.CaperCounter === null) {
        this.CaperCounter = this.nbBottlesFilled;
      }

      if (this.EtiqueteuseCounter === 0  || this.EtiqueteuseCounter === '' || this.EtiqueteuseCounter === undefined || this.EtiqueteuseCounter === null) {
        this.EtiqueteuseCounter = this.nbBottlesFilled;
      }

      if (this.WieghtBoxCounter === 0 || this.WieghtBoxCounter === '' || this.WieghtBoxCounter === undefined || this.WieghtBoxCounter === null) {
        this.WieghtBoxCounter = this.nbBottlesFilled;
      }else {
        this.WieghtBoxCounter = this.WieghtBoxCounter*this.netOP[0].bottlesPerCase
      }

      if (this.QualityControlCounter === 0 || this.QualityControlCounter === '' || this.QualityControlCounter === undefined || this.QualityControlCounter === null) {
        this.QualityControlCounter = this.nbBottlesFilled;
      }else {
        this.QualityControlCounter = this.QualityControlCounter*this.netOP[0].bottlesPerCase
      }


      await axios.post(urlAPI+'storeRejection', {
        po: this.endPO,
        fillerCounter: this.FillerCounter,
        caperCounter: this.CaperCounter,
        labelerCounter: this.EtiqueteuseCounter,
        weightBoxCounter: this.WieghtBoxCounter,
        qualityControlCounter : this.QualityControlCounter,
        fillerRejection : this.FillerRejection,
        caperRejection : this.CaperRejection,
        labelerRejection : this.EtiqueteuseRejection,
        weightBoxRejection : this.WieghtBoxRejection*this.netOP[0].bottlesPerCase,
        qualityControlRejection : this.QualityControlRejection*this.netOP[0].bottlesPerCase,
        shift : this.typeTeam,
        created_at: selectedDate
      });

      await this.resolveAfter1Second();

      router.replace("/teamInfo");



    },

    loadTable: function () {

      var productionlinesTab = [];

      for (let i = 0; i < this.user[3].length; i++) {
        if (this.user[3][i].worksiteID === this.user[0][0].worksiteID) {
          productionlinesTab.push(this.user[3][i].productionline_name);
        }
      }

      if (sessionStorage.getItem("prodlines") === null) {
        sessionStorage.prodlines = productionlinesTab;
      } else {
        sessionStorage.setItem("prodlines", productionlinesTab);
      }

    },

    resolveAfter1Second: function () {
      return new Promise(resolve => {
        setTimeout(() => {
          resolve('resolved');
        }, 1000);
      });


    },

    isRejectionNull: function () {
      return this.FillerRejection === 0 && this.CaperRejection === 0
          && this.EtiqueteuseRejection === 0 && this.WieghtBoxRejection === 0 && this.QualityControlRejection === 0;
    },

    isCompteurNull: function () {
      return this.FillerCounter === 0 && this.CaperCounter === 0 && this.EtiqueteuseCounter === 0
          && this.WieghtBoxCounter === 0 && this.QualityControlCounter === 0;
    },

    isRejectionNull2: function () {
      return this.FillerRejection === '0' && this.CaperRejection === '0'
          && this.EtiqueteuseRejection === '0' && this.WieghtBoxRejection === '0' && this.QualityControlRejection === '0';
    },

    isCompteurNull2: function () {
      return this.FillerCounter === '0' && this.CaperCounter === '0' && this.EtiqueteuseCounter === '0'
          && this.WieghtBoxCounter === '0' && this.QualityControlCounter === '0';
    },


    validateInformations: async function () {

      this.valider = 0;
      this.displayNumber = 2;

    },

    confirmSpeedloss: async function () {
      var form = document.getElementById('needs-validation');
      if (form.checkValidity() === false) {
        event.preventDefault();
        event.stopPropagation();
        console.log("PAS OK");

      }else{
        var responses = document.getElementsByClassName('response');
        var reason = '';
        for (let i = 0; i < responses.length; i++) {
          if (responses[i].checked) {
            reason = responses[i].value;
          }
          responses[i].setAttribute('disabled', 'disabled');
        }

        this.speedLossEvent.comment = document.getElementById('comments').value;
        this.speedLossEvent.OLE = sessionStorage.getItem("pos").split(',')[this.indice];
        this.speedLossEvent.reason = reason;
        this.speedLossEvent.shift = sessionStorage.getItem("typeTeam");
        this.speedLossEvent.duration = document.getElementById('sl-duration').value;
        this.speedLossEvent.created_at = sessionStorage.getItem('dateInput');

        console.log(this.speedLossEvent);

        if (this.speedLossEvent.reason === "") {
          this.errorMessage2();
        } else {

          await axios.post(urlAPI+'speedLoss',  this.speedLossEvent)
        

          window.location.reload();
        }

      }

    }


  }
  ,

  async mounted() {

    if (sessionStorage.getItem("language") !== null) {
      this.$i18n.locale = sessionStorage.getItem("language");
    }

    console.log("TEAM " + this.typeTeam)

    var tab = [];
    tab.push(this.productionName);
    for (let i = 0; i < this.prodlines.length; i++) {
      if (this.productionName === this.prodlines[i]) {
        this.indice = i;
      }
    }

    console.log('t1 = ' + this.startPO);
    console.log('t2 = ' + this.endPO);


    console.log('INDICE = ' + this.indice);

    var poNumber = sessionStorage.getItem("pos").split(',')[this.indice];

    this.PO = sessionStorage.getItem("pos");


    await axios.get(urlAPI + 'speedLosses/' + poNumber + '/' + this.productionName + '/' + sessionStorage.getItem("typeTeam"))
        .then(response => (this.speedLoss = response.data))

    console.log(this.speedLoss);
    for(let i=0; i<this.speedLoss.length; i++){
      this.totalSpeedLosses += this.speedLoss[i].duration;
    }

    await axios.get(urlAPI + 'netOP/' + this.GMID)
        .then(response => (this.netOP = response.data))

    await axios.get(urlAPI + 'performance/' + this.PO)
        .then(response => (this.performanceIndexes = response.data))

    //console.log("getHistoricPO/" + this.typeTeam +"/"+this.productionName)

    await axios.get(urlAPI + "getHistoricPO/"+this.productionName)
      .then(response => (this.historicPO = response.data))

    console.log("Historic " +  sessionStorage.getItem("pos"));

    console.log(this.historicPO);




  }
  ,


}
</script>

<style scoped>
.blockInput {
  margin-top: -20px;
}

.productionName {
  left: 0;
  top: 0;
  min-width: 150px;
  max-width: 250px;

  margin-bottom: 50px;
}

.rcorners1 {
  border-radius: 25px;
  background: lightblue;
  padding: 20px;
  margin-bottom: 30px;
  width: 180px;

}


.rcorners2 {
  border-radius: 25px;
  border: 2px solid lightblue;
  padding: 20px;
}

.wrapper {
  display: grid;
  grid-template-columns: 1fr 1fr 1fr;
}


button {
  color: white;
  margin-top: 20px;
}

#deleteSpeedLossButton {
  margin-top: 0;

}

#addReasonButton {
  color: black;
}

input {
  width: 40%;

}

#comments {
  height: 150px;
  width: 70%;
  border-radius: 25px;
  border: 2px solid lightblue;
  padding: 20px;

}

form {
  display: block;
  margin-left: auto;
  margin-right: auto;
}

label.checkbox {
  cursor: pointer;
}

label.checkbox input {
  position: absolute;
  top: 0;
  left: 0;
  visibility: hidden;
  pointer-events: none;

}

label.labelError {
  cursor: pointer;
}

label.labelError input {
  position: absolute;
  top: 0;
  left: 0;
  visibility: hidden;
  pointer-events: none;

}


label.checkbox span {
  padding: 4px 0px;
  border: 1px solid #56baed;
  display: inline-block;
  color: #56baed;
  width: 200px;
  text-align: center;
  border-radius: 3px;
  margin-top: 7px;
  text-transform: uppercase
}

label.checkbox input:checked + span {
  border-color: #56baed;
  background-color: #56baed;
  color: #fff
}

thead {
  color: white;
  background: #56baed;
}

/* Chrome, Safari, Edge, Opera */
input::-webkit-outer-spin-button,
input::-webkit-inner-spin-button {
  -webkit-appearance: none;
  margin: 0;
}

/* Firefox */
input[type=number] {
  -moz-appearance: textfield;
}

.table-info-data {
  overflow:scroll; max-height: 300px;
}

</style>