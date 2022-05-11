<template>
  <div>
    <div class="row dataInput">
      <div class="col-sm">
        <div class="data">
          <div class="">

            <form>
              <label class="" for="site">{{$t("site")}} : </label>
              <select name="site" id="site" class="form-select" v-model="site" @change="team = ''">
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
                      v-model="productionline" @change="team = 'allTeams'">
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
              {{$t("qualityLossesDashboard")}}
            </h1>
          </div>
          <div>
            <label class="" for="startingPO">{{$t("from")}}</label>
            <input type="date" id="startingPO" class=" " required v-model="beginningDate">


            <label class="" for="endingPO">{{$t("to")}}</label>
            <input type="date" id="endingPO" class=""
                   required v-model="endingDate">
          </div>
        </div>
      </div>
    </div>

    <div class="row">
      <div class="col-sm">

                <span class="content-title">
                    {{$t("qualityLosses")}}
                </span>
      </div>
    </div>

    <div class="row">
      <div class="col-sm">
        <img src="../../assets/qualityLossesDiagram.png">
      </div>
      <div class="col-sm  border border-primary">
        <h3 style="padding-top : 55px;">
          <template v-if="show === 1">
            QL = {{quality.toFixed(4)*100}} %
          </template>
        </h3>
      </div>

    </div>
    <div class="row">
      <div class="col-sm border border-primary" style="padding-left : 50px;">
        <p>
          {{$t("qualityLossesNDescription")}}
          <br/>
          {{$t("qualityLossesMiDescription")}}
          <br/>
          {{$t("qualityLossesCiDescription")}}
        </p>
      </div>
      <div class="col-sm border border-primary">
        <p>
          M1 = {{$t("labeler")}} <br/>
          M2 = {{$t("caper")}}  <br/>
          M3 = {{$t("qualityControl")}}  <br/>
          M4 = {{$t("boxWeigher")}}  <br/>

        </p>
      </div>

    </div>

    <div class="row">
      <div class="col-sm">
        <span class="content-title">{{$t("qualityLossesByMachine")}}</span>
        <!-- <template v-if="show===1">
             <h1>
                 {{$t("qualityImpactPerMachine")}}
             </h1>
         </template>-->
        <canvas id="qualityLossesByMachines" width="100" height="100"/>
      </div>
      <div class="col-sm">
                <span class="content-title">
                    {{$t("qualityLossesByFormat")}}
                </span>
        <canvas id="qualityLossesByFormat" width="100" height="100"/>
      </div>
    </div>

    <div class="row">

      <div class="col-sm">
        <template v-if="show === 1">

          <table class="table">
            <thead>
            <tr>
              <th scope="col"></th>
              <th scope="col">{{$t("numberOfItems")}}</th>
            </tr>
            </thead>
            <tbody>


            <tr class="filler mainLine">
              <th scope="row">{{$t("filler")}}</th>
              <td>{{productionPerMachineToShow["fillerCounter"]}}
              </td>
            </tr>

            <tr class="filler">
              <th scope="row">{{$t("overProcess")}}</th>
              <td>{{productionPerMachineToShow["fillerOverProcess"]}}</td>
            </tr>

            <tr class="filler">
              <th scope="row">{{$t("rejectedItems")}}</th>
              <td>{{productionPerMachineToShow["fillerRejection"]}}</td>
            </tr>


            <tr class="caper mainLine">
              <th scope="row">{{$t("caper")}}</th>
              <td>{{productionPerMachineToShow["caperCounter"]}}
              </td>
            </tr>

            <tr class="caper">
              <th scope="row">{{$t("overProcess")}}</th>
              <td>{{productionPerMachineToShow["caperOverProcess"]}}</td>
            </tr>

            <tr class="caper">
              <th scope="row">{{$t("rejectedItems")}}</th>
              <td>{{productionPerMachineToShow["caperRejection"]}}</td>
            </tr>


            <tr class="labeler mainLine">
              <th scope="row">{{$t("labeler")}}</th>
              <td>{{productionPerMachineToShow["labelerCounter"]}}
              </td>
            </tr>

            <tr class="labeler">
              <th scope="row">{{$t("overProcess")}}</th>
              <td>{{productionPerMachineToShow["labelerOverProcess"]}}</td>
            </tr>

            <tr class="labeler">
              <th scope="row">{{$t("rejectedItems")}}</th>
              <td>{{productionPerMachineToShow["labelerRejection"]}}</td>
            </tr>

            <tr class="boxWeigher mainLine">
              <th scope="row">{{$t("boxWeigher")}}</th>
              <td>{{productionPerMachineToShow["weightBoxCounter"] }}
              </td>
            </tr>

            <tr class="boxWeigher">
              <th scope="row">{{$t("overProcess")}}</th>
              <td>{{productionPerMachineToShow["weightOverProcess"] }}</td>
            </tr>

            <tr class="boxWeigher">
              <th scope="row">{{$t("rejectedItems")}}</th>
              <td>{{productionPerMachineToShow["weightBoxRejection"] }}</td>
            </tr>

            <tr class="qualityControl mainLine">
              <th scope="row">{{$t("qualityControl")}}</th>
              <td>{{productionPerMachineToShow["qualityControlCounter"] }}
              </td>
            </tr>

            <tr class="qualityControl">
              <th scope="row">{{$t("overProcess")}}</th>
              <td>{{productionPerMachineToShow["qualityOverProcess"]}}</td>
            </tr>

            <tr class="qualityControl">
              <th scope="row">{{$t("rejectedItems")}}</th>
              <td>{{productionPerMachineToShow["qualityControlRejection"]}}</td>
            </tr>

            <tr class="totalQty">
              <th scope="row">{{$t("total")}}</th>
              <td>{{totalToShow}}</td>
            </tr>


            </tbody>
          </table>
        </template>
      </div>

      <div class="col-sm">

        <template v-if="show === 1">

          <table class="table">
            <thead>
            <tr>
              <th scope="col"></th>
              <th v-for="format in tableauFormats" scope="col"  v-bind:key="format.id">{{format}}L</th>
            </tr>
            </thead>
            <tbody>

            <tr class="filler mainLine">
              <th scope="row">{{$t("filler")}}</th>
              <td v-for="format in tableauFormats" v-bind:key="format.id">
                {{qtyPerMachine['fillerCounter'][format]}}
              </td>

            </tr>

            <tr class="filler">
              <th scope="row">{{$t("overProcess")}}</th>
              <td v-for="format in tableauFormats"  v-bind:key="format.id">
                {{qtyPerMachine['fillerOverProcess'][format]}}
              </td>
            </tr>

            <tr class="filler">
              <th scope="row">{{$t("rejectedItems")}}</th>
              <td v-for="format in tableauFormats"  v-bind:key="format.id">
                {{qtyPerMachine['fillerRejection'][format]}}
              </td>
            </tr>

            <tr class="caper mainLine">
              <th scope="row">{{$t("caper")}}</th>
              <td v-for="format in tableauFormats"  v-bind:key="format.id">
                {{qtyPerMachine['caperCounter'][format]}}
              </td>
            </tr>

            <tr class="caper">
              <th scope="row">{{$t("overProcess")}}</th>
              <td v-for="format in tableauFormats"  v-bind:key="format.id">
                {{qtyPerMachine['caperOverProcess'][format]}}
              </td>
            </tr>

            <tr class="caper">
              <th scope="row">{{$t("rejectedItems")}}</th>
              <td v-for="format in tableauFormats"  v-bind:key="format.id">
                {{qtyPerMachine['caperRejection'][format]}}
              </td>
            </tr>


            <tr class="labeler mainLine">
              <th scope="row">{{$t("labeler")}}</th>
              <td v-for="format in tableauFormats"  v-bind:key="format.id">
                {{qtyPerMachine['labelerCounter'][format]}}
              </td>
            </tr>

            <tr class="labeler">
              <th scope="row">{{$t("overProcess")}}</th>
              <td v-for="format in tableauFormats"  v-bind:key="format.id">
                {{qtyPerMachine['labelerOverProcess'][format]}}
              </td>
            </tr>

            <tr class="labeler">
              <th scope="row">{{$t("rejectedItems")}}</th>
              <td v-for="format in tableauFormats"  v-bind:key="format.id">
                {{qtyPerMachine['labelerRejection'][format]}}
              </td>
            </tr>

            <tr class="boxWeigher mainLine">
              <th scope="row">{{$t("boxWeigher")}}</th>
              <td v-for="format in tableauFormats"  v-bind:key="format.id">
                {{qtyPerMachine['weightBoxCounter'][format]}}
              </td>

            </tr>

            <tr class="boxWeigher">
              <th scope="row">{{$t("overProcess")}}</th>
              <td v-for="format in tableauFormats"  v-bind:key="format.id">
                {{qtyPerMachine['weightBoxOverProcess'][format]}}
              </td>
            </tr>

            <tr class="boxWeigher">
              <th scope="row">{{$t("rejectedItems")}}</th>
              <td v-for="format in tableauFormats"  v-bind:key="format.id">
                {{qtyPerMachine['weightBoxRejection'][format]}}
              </td>
            </tr>

            <tr class="qualityControl mainLine">
              <th scope="row">{{$t("qualityControl")}}</th>
              <td v-for="format in tableauFormats"  v-bind:key="format.id">
                {{qtyPerMachine["controlQualityCounter"][format]}}
              </td>

            </tr>

            <tr class="qualityControl">
              <th scope="row">{{$t("overProcess")}}</th>
              <td v-for="format in tableauFormats"  v-bind:key="format.id">
                {{qtyPerMachine['controlQualityOverProcess'][format]}}
              </td>
            </tr>

            <tr class="qualityControl">
              <th scope="row">{{$t("rejectedItems")}}</th>
              <td v-for="format in tableauFormats"  v-bind:key="format.id">
                {{qtyPerMachine["controlQualityRejection"][format]}}
              </td>
            </tr>


            <tr class="totalQty">
              <th scope="row">{{$t("total")}}</th>
              <td v-for="format in tableauFormats"  v-bind:key="format.id">
                {{totalRejectionPerFormat[format]}}
              </td>
            </tr>


            </tbody>
          </table>
        </template>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import {urlAPI} from "@/variables";

export default {
  name: "QualityLosses",
  data() {

    return {

      currentYear: (new Date()).getFullYear(),
      site: '',
      productionLine: '',
      lo: this.$t("load"),
      beginningDate: "",
      endingDate: "",
      productionline: "",
      show: 0,
      totalItems: 0,
      tableauFormats: [],
      overAllRejection : 0,
      qtyPerMachine: [],
      totalPerFormat: [],
      totalRejectionPerFormat : [],
      qualityLossesPerMachineArray: [],
      quality: 0,
      dataWorksite : null,
      dataProductionlines : null,
      dataTeams: null,
      allEvents : null,
      qualityLosses : null,
      username: localStorage.getItem("username"),
      team: "",

      productionPerMachineToShow : [],
      totalToShow : [],
    }
  },

  methods: {
    resolveAfter: function (milliseconds) {
      return new Promise(resolve => {
        setTimeout(() => resolve(), milliseconds);
      });
    },


    resolveAfter15Second: function () {
      return new Promise(resolve => {
        setTimeout(() => {
          resolve('resolved');
        }, 1500);
      });
    },


    load: async function () {

      var totalRejectionMachine = 0;

      if (this.productionline !== '' && this.beginningDate !== '' && this.endingDate !== '' && this.team !== '') {
        var tab = [];
        tab.push(this.site);
        tab.push(this.productionline);
        tab.push(this.beginningDate);
        tab.push(this.endingDate);

        await axios.get(urlAPI + 'allevents/' + this.site + '/' + this.productionline + '/' + this.beginningDate + '/' + this.endingDate + '/' + this.team)
            .then(response => (this.allEvents = response.data))

        console.log(this.allEvents);
        this.productionPerMachineToShow["fillerCounter"] = 0;
        this.productionPerMachineToShow["caperCounter"] = 0;
        this.productionPerMachineToShow["labelerCounter"] = 0;
        this.productionPerMachineToShow["weightBoxCounter"] = 0;
        this.productionPerMachineToShow["qualityControlCounter"] =0;
        this.productionPerMachineToShow["fillerRejection"] = 0;
        this.productionPerMachineToShow["caperRejection"] = 0;
        this.productionPerMachineToShow["labelerRejection"] = 0;
        this.productionPerMachineToShow["weightBoxRejection"] = 0;
        this.productionPerMachineToShow["qualityControlRejection"] = 0;


        this.productionPerMachineToShow["fillerOverProcess"] = 0;
        this.productionPerMachineToShow["caperOverProcess"] = 0;
        this.productionPerMachineToShow["labelerOverProcess"] = 0;
        this.productionPerMachineToShow["weightOverProcess"] = 0;
        this.productionPerMachineToShow["qualityOverProcess"] = 0;


        this.totalToShow = 0;



        for (let i = 0; i < this.allEvents['SITE'].length; i++) {




          let PO = this.allEvents['SITE'][i];

          totalRejectionMachine += (PO.fillerRejection * 1) + (PO.caperRejection * 1) + (PO.labelerRejection * 1) + (PO.weightBoxRejection * 1) + (PO.qualityControlRejection * 1);
          this.totalToShow += (PO.fillerRejection * 1) + (PO.caperRejection * 1) + (PO.labelerRejection * 1) + (PO.weightBoxRejection * 1) + (PO.qualityControlRejection * 1)
          //this.totalToShow += (PO.fillerCounter * 1) + (PO.caperCounter * 1) + (PO.labelerCounter * 1) + (PO.weightBoxCounter * 1) + (PO.qualityControlCounter * 1)

          this.productionPerMachineToShow["fillerCounter"] += PO.fillerCounter * 1 ;

          this.productionPerMachineToShow["caperCounter"] += PO.caperCounter * 1  ;
          this.productionPerMachineToShow["labelerCounter"] += PO.labelerCounter * 1  ;
          this.productionPerMachineToShow["weightBoxCounter"] += PO.weightBoxCounter * 1  ;
          this.productionPerMachineToShow["qualityControlCounter"] += PO.qualityControlCounter * 1 ;
          this.productionPerMachineToShow["fillerRejection"] += PO.fillerRejection * 1;
          this.productionPerMachineToShow["caperRejection"] += PO.caperRejection * 1;
          this.productionPerMachineToShow["labelerRejection"] += PO.labelerRejection * 1;
          this.productionPerMachineToShow["weightBoxRejection"] += PO.weightBoxRejection * 1;
          this.productionPerMachineToShow["qualityControlRejection"] += PO.qualityControlRejection * 1;

/**
          this.productionPerMachineToShow["fillerOverProcess"] +=  (this.productionPerMachineToShow["fillerCounter"] - this.productionPerMachineToShow["fillerRejection"] - (PO.qtyProduced * PO.bottlesPerCase)) ;
          this.productionPerMachineToShow["caperOverProcess"] +=  (this.productionPerMachineToShow["caperCounter"] - this.productionPerMachineToShow["caperRejection"] - (PO.qtyProduced * PO.bottlesPerCase)) ;
          this.productionPerMachineToShow["labelerOverProcess"] +=  (this.productionPerMachineToShow["labelerCounter"] - this.productionPerMachineToShow["labelerRejection"] - (PO.qtyProduced * PO.bottlesPerCase)) ;
          this.productionPerMachineToShow["weightOverProcess"] +=  (this.productionPerMachineToShow["weightBoxCounter"] - this.productionPerMachineToShow["weightBoxRejection"] - (PO.qtyProduced * PO.bottlesPerCase)) ;
          this.productionPerMachineToShow["qualityOverProcess"] +=  (this.productionPerMachineToShow["qualityControlCounter"] - this.productionPerMachineToShow["qualityControlRejection"] - (PO.qtyProduced * PO.bottlesPerCase)) ;
**/


        }





        await axios.get(urlAPI + 'qualityLosses/' + this.site + '/' + this.productionline + '/' + this.beginningDate + '/' + this.endingDate + '/' + this.team)
            .then(response => (this.qualityLosses = response.data))

        console.log(this.qualityLosses);

        this.show = 1;
        this.loadQualityLosses();
        this.qualityLossesPerMachineArray = [];
        this.qualityLossesPerMachineArray.push({
          name: "weightBox",
          value: this.productionPerMachineToShow["weightBoxRejection"] / totalRejectionMachine
        });

        this.qualityLossesPerMachineArray.push({
          name: "caper",
          value:  this.productionPerMachineToShow["caperRejection"] / totalRejectionMachine
        });

        this.qualityLossesPerMachineArray.push({
          name: "filler",
          value: this.productionPerMachineToShow["fillerRejection"] / totalRejectionMachine
        });

        this.qualityLossesPerMachineArray.push({
          name: "labeler",
          value:  this.productionPerMachineToShow["labelerRejection"] / totalRejectionMachine
        });

        this.qualityLossesPerMachineArray.push({
          name: "qualityControl",
          value: this.productionPerMachineToShow["qualityControlRejection"] / totalRejectionMachine
        });


        this.totalItems = this.totalToShow;

        this.loadQualityLossesByFormat();
        this.pieCharts();


      }
    },

    loadQualityLosses: function () {


      var sommeQtyProduced = 0;
      var sommeRejection = 0;

      var fillerCounter = 0;
      var caperCounter = 0;
      var labelerCounter = 0;
      var wieghtBoxCounter = 0;
      var qualityControlCounter = 0;


      for (let i = 0; i < this.allEvents['SITE'].length; i++) {


        let PO = this.allEvents['SITE'][i];
        sommeRejection += PO.fillerRejection * 1 + PO.caperRejection * 1 + PO.labelerRejection * 1 + PO.weightBoxRejection * 1 + PO.qualityControlRejection;
        fillerCounter += PO.fillerCounter * 1;
        caperCounter += PO.caperCounter * 1;
        labelerCounter += PO.labelerCounter * 1;
        wieghtBoxCounter += PO.weightBoxCounter * 1;
        qualityControlCounter += PO.qualityControlCounter * 1;


        if (!(this.allEvents['SITE'][i].labelerCounter === 0 && this.allEvents['SITE'][i].weightBoxCounter === 0
            && this.allEvents['SITE'][i].qualityControlCounter === 0 && this.allEvents['SITE'][i].caperCounter === 0
            && this.allEvents['SITE'][i].fillerCounter === 0)) {
          sommeQtyProduced += this.allEvents['SITE'][i].qtyProduced * this.allEvents['SITE'][i].bottlesPerCase * 1;
        }

      }

      var summCompteur = 0;
      if (fillerCounter !== 0) {
        summCompteur += (fillerCounter - sommeQtyProduced);
      }
      if (caperCounter !== 0) {
        summCompteur += (caperCounter - sommeQtyProduced);
      }

      if (labelerCounter !== 0) {
        summCompteur += (labelerCounter - sommeQtyProduced);
      }

      if (qualityControlCounter !== 0) {
        summCompteur += (qualityControlCounter - sommeQtyProduced);
      }
      if (wieghtBoxCounter !== 0) {
        summCompteur += (wieghtBoxCounter - sommeQtyProduced);
      }


      console.log('DATA FOR QUALITY');
      console.log('QTY PRODUCED : ' + sommeQtyProduced);
      console.log('Rejection : ' + sommeRejection);
      console.log('Compteur : ' + summCompteur);


      if(sommeQtyProduced + sommeRejection + summCompteur === 0 ){
        this.quality = 1;
      }else{
        this.quality = (sommeQtyProduced) / (sommeQtyProduced + sommeRejection + summCompteur);
      }

      if(this.quality > 1){
        this.quality = 1;
      }


    },

    loadQualityLossesByFormat: function () {

      this.tableauFormats = [];
      this.qtyPerMachine = [];
      this.qtyPerMachine["caperCounter"] = [];
      this.qtyPerMachine["caperRejection"] = [];
      this.qtyPerMachine["fillerCounter"] = [];
      this.qtyPerMachine["fillerRejection"] = [];
      this.qtyPerMachine["weightBoxCounter"] = [];
      this.qtyPerMachine["weightBoxRejection"] = [];
      this.qtyPerMachine["labelerCounter"] = [];
      this.qtyPerMachine["labelerRejection"] = [];
      this.qtyPerMachine["controlQualityCounter"] = [];
      this.qtyPerMachine["controlQualityRejection"] = [];

      this.qtyPerMachine["caperOverProcess"] = [];
      this.qtyPerMachine["fillerOverProcess"] = [];
      this.qtyPerMachine["weightBoxOverProcess"] = [];
      this.qtyPerMachine["labelerOverProcess"] = [];
      this.qtyPerMachine["controlQualityOverProcess"] = [];


      for (let i = 0; i < this.allEvents['SITE'].length; i++) {

        var line = this.allEvents['SITE'][i];

        var format = line.size;


        this.overAllRejection +=  line.caperRejection + line.fillerRejection +
            line.weightBoxRejection + line.labelerRejection + line.qualityControlRejection;


        if (!this.tableauFormats.includes(format)) {
          this.tableauFormats.push(format);


          this.qtyPerMachine["caperCounter"][format] = line.caperCounter;
          this.qtyPerMachine["caperRejection"][format] = line.caperRejection;
          this.qtyPerMachine["fillerCounter"][format] = line.fillerCounter;
          this.qtyPerMachine["fillerRejection"][format] = line.fillerRejection;
          this.qtyPerMachine["weightBoxCounter"][format] = line.weightBoxCounter;
          this.qtyPerMachine["weightBoxRejection"][format] = line.weightBoxRejection;
          this.qtyPerMachine["labelerCounter"][format] = line.labelerCounter;
          this.qtyPerMachine["labelerRejection"][format] = line.labelerRejection;
          this.qtyPerMachine["controlQualityCounter"][format] = line.qualityControlCounter;
          this.qtyPerMachine["controlQualityRejection"][format] = line.qualityControlRejection;

          this.qtyPerMachine["caperOverProcess"][format] = 0;
          this.qtyPerMachine["fillerOverProcess"][format] = 0;
          this.qtyPerMachine["weightBoxOverProcess"][format] = 0;
          this.qtyPerMachine["labelerOverProcess"][format] = 0;
          this.qtyPerMachine["controlQualityOverProcess"][format] = 0;


          this.totalPerFormat[format] = line.caperCounter + line.caperRejection + line.fillerCounter
              + line.fillerRejection + line.weightBoxCounter + line.weightBoxRejection + line.labelerCounter +
              line.labelerRejection + line.qualityControlCounter + line.qualityControlRejection;

          this.totalRejectionPerFormat[format] =  line.caperRejection + line.fillerRejection +
              line.weightBoxRejection + line.labelerRejection + line.qualityControlRejection;

        } else {

          this.qtyPerMachine["caperCounter"][format] += line.caperCounter;
          this.qtyPerMachine["caperRejection"][format] += line.caperRejection;
          this.qtyPerMachine["fillerCounter"][format] += line.fillerCounter;
          this.qtyPerMachine["fillerRejection"][format] += line.fillerRejection;
          this.qtyPerMachine["weightBoxCounter"][format] += line.weightBoxCounter;
          this.qtyPerMachine["weightBoxRejection"][format] += line.weightBoxRejection;
          this.qtyPerMachine["labelerCounter"][format] += line.labelerCounter;
          this.qtyPerMachine["labelerRejection"][format] += line.labelerRejection;
          this.qtyPerMachine["controlQualityCounter"][format] += line.qualityControlCounter;
          this.qtyPerMachine["controlQualityRejection"][format] += line.qualityControlRejection;
          this.totalPerFormat[format] += line.caperCounter + line.caperRejection + line.fillerCounter
              + line.fillerRejection + line.weightBoxCounter + line.weightBoxRejection + line.labelerCounter +
              line.labelerRejection + line.qualityControlCounter + line.qualityControlRejection;
          this.totalRejectionPerFormat[format] +=  line.caperRejection + line.fillerRejection +
              line.weightBoxRejection + line.labelerRejection + line.qualityControlRejection;

          this.qtyPerMachine["caperOverProcess"][format] += 0;
          this.qtyPerMachine["fillerOverProcess"][format] += 0;
          this.qtyPerMachine["weightBoxOverProcess"][format] += 0;
          this.qtyPerMachine["labelerOverProcess"][format] += 0;
          this.qtyPerMachine["controlQualityOverProcess"][format] += 0;
        }


      }



    },

    pieCharts: function () {
      let obj;
      var data = [];
      var totalPieChart1 = 0;
      for (let j = 0; j < this.qualityLossesPerMachineArray.length; j++) {
        obj = {
          name: this.qualityLossesPerMachineArray[j].name,
          nbr: this.qualityLossesPerMachineArray[j].value
        };
        data.push(obj);
        totalPieChart1 = 1;
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

      var canvas = document.getElementById("qualityLossesByMachines");
      var ctx = canvas.getContext("2d");
      canvas.width = 450;
      canvas.height = 350;
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
        var txt = this.$t(item.name) + '\n';
        var pct = item.nbr / totalPieChart1 * 100;
        txt = txt + ' ' + pct.toFixed(2) + '%';

        ctx.fillText(txt, deltaX + cx, deltaY + cy);
        ctx.closePath();

        startAngle = endAngle;
      }

      data = [];
      var totalPieChart2 = 0;
      for (let j = 0; j < this.tableauFormats.length; j++) {
        var format = this.tableauFormats[j];

        obj = {
          name: format,
          nbr: this.totalRejectionPerFormat[format]/this.overAllRejection,
        };
        data.push(obj);
        totalPieChart2 = 1;
      }

      if (data.length === 0) {
        obj = {
          name: this.$t("nothingProduced"),
          nbr: 1
        };
        data.push(obj);

        totalPieChart2 = 1;
      }

      canvas = document.getElementById("qualityLossesByFormat");
      ctx = canvas.getContext("2d");
      canvas.width = 450;
      canvas.height = 350;
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
          txt = item.name + 'L\n';
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
  },

}
</script>
<style scoped>

div {
  padding: 15px;
  background-color: #fff;

}

div.main-container {
  flex-direction: column;
  background-color: white;
  padding: 20px;
  min-width: 1000px;
  border-radius: 5px;
  margin: 20px 0px;
}

div.selection-menu {
  flex-direction: row;
  padding: 20px 0px;
  border-bottom: solid 1px;
}

div.site-pl-selection {
  flex-direction: column;
  justify-content: space-evenly;
  min-width: 200px;
}

div.site-pl-selection > div {
  align-items: center;
}

div.site-pl-selection select {
  width: 100%;
}

div.site-pl-selection label {
  margin: 0px 10px 0px 0px;
}

div.title-container {
  margin-top: 10px;
  display: flex;
  justify-content: center;
}

.content-title {
  font-size: 20px;
  font-weight: bold;
  color: black;
  width: 100%;
}

span.content-subtitle {
  font-size: 17px;
  font-weight: bold;
  color: black;
  width: 100%;
}

div.content-panel {
  display: flex;
  flex-direction: column;
}

div.upper-panel,
div.bottom-panel {
  display: flex;
  width: 100%;
}

div.ql-machine-panel,
div.ql-format-panel {
  width: 50%;
}


table.table {
}

h1 {
  font-size: 1.4em;
  color: #56baed;
}

label {
  font-size: 1.4em;
  color: #56baed;
}

p {
  font-size: 1em;
  color: black;
}

h2 {
  font-size: 1.2em;
  color: #56baed;
}

h4 {
  color: red;
}


.row {
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


.mainLine {
  border: 2px solid black;
}

.boxWeigher {
  background: palegoldenrod;
}

.caper {
  background: palegreen;
}

.labeler {
  background: paleturquoise;
}

.filler {
  background: palevioletred;
}

.qualityControl {
  background: lightpink;
}

.totalQty {
  background: papayawhip;
}

.machineDiagram {
  background: lightblue;
  color: white;
  align: center;
}

h3 {
  align: center;
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


</style>
