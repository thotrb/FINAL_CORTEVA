<template>
  <div class="" id="component">

    <div class="row dataInput">
      <div class="col-sm">
        <div class="data">
          <div class="">

            <form>
              <label class="" for="site">{{$t("site")}} : </label>
              <select name="site" id="site" class="form-select" v-model="site">
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
                      v-model="productionline">
                <template v-for="productionline in dataProductionlines">
                  <template v-if="productionline.name === site">
                    <option v-bind:value="productionline.productionline_name"  v-bind:key="productionline.id">
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
              {{$t("productionDashboard")}}
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
        <template v-if="show===1">
          <div>
            <h1>
              {{$t("formulationSplit")}}
            </h1>
          </div>


          <div class="table-info-data dataInput" width="400">
            <table class="table">
              <thead>
              <tr>
                <th scope="col"></th>
                <th scope="col">Jan</th>
                <th scope="col">Feb</th>
                <th scope="col">Mar</th>
                <th scope="col">Apr</th>
                <th scope="col">May</th>
                <th scope="col">Jun</th>
                <th scope="col">Jul</th>
                <th scope="col">Aug</th>
                <th scope="col">Sep</th>
                <th scope="col">Oct</th>
                <th scope="col">Nov</th>
                <th scope="col">Dec</th>


              </tr>
              </thead>
              <tbody>
              <tr v-for="(formulation, index) in formulations" :key="index">
                <th scope="row">{{formulation}}</th>
                <td>{{formulationsPerMonth[index]['January']}}</td>
                <td>{{formulationsPerMonth[index]['February']}}</td>
                <td>{{formulationsPerMonth[index]['March']}}</td>
                <td>{{formulationsPerMonth[index]['April']}}</td>
                <td>{{formulationsPerMonth[index]['May']}}</td>
                <td>{{formulationsPerMonth[index]['June']}}</td>
                <td>{{formulationsPerMonth[index]['July']}}</td>
                <td>{{formulationsPerMonth[index]['August']}}</td>
                <td>{{formulationsPerMonth[index]['September']}}</td>
                <td>{{formulationsPerMonth[index]['October']}}</td>
                <td>{{formulationsPerMonth[index]['November']}}</td>
                <td>{{formulationsPerMonth[index]['December']}}</td>
              </tr>
              <tr>
                <th scope="row">{{$t("total")}}</th>
                <td>{{sumPerMonth['January']}}</td>
                <td>{{sumPerMonth['February']}}</td>
                <td>{{sumPerMonth['March']}}</td>
                <td>{{sumPerMonth['April']}}</td>
                <td>{{sumPerMonth['May']}}</td>
                <td>{{sumPerMonth['June']}}</td>
                <td>{{sumPerMonth['July']}}</td>
                <td>{{sumPerMonth['August']}}</td>
                <td>{{sumPerMonth['September']}}</td>
                <td>{{sumPerMonth['October']}}</td>
                <td>{{sumPerMonth['November']}}</td>
                <td>{{sumPerMonth['December']}}</td>
              </tr>

              </tbody>
            </table>

          </div>
        </template>

        <div class="">
          <canvas class="diagram"  id="myChart4"></canvas>
        </div>

      </div>

      <div class="col-sm">

        <template v-if="show===1">
          <div>
            <h1>
              {{$t("formulationSplit")}} (%)
            </h1>
          </div>

        </template>
        <canvas class="pieChart"  id="formulationSplit" width="100" height="100"/>


      </div>
    </div>


    <div class="row">
      <div class="col-sm">

        <template v-if="show===1">

          <div>
            <h1>
              {{$t("packSizeSplit")}}
            </h1>
          </div>


          <div class="table-info-data dataInput" width="400">
            <table class="table">
              <thead>
              <tr>
                <th scope="col"></th>
                <th scope="col">Jan</th>
                <th scope="col">Feb</th>
                <th scope="col">Mar</th>
                <th scope="col">Apr</th>
                <th scope="col">May</th>
                <th scope="col">Jun</th>
                <th scope="col">Jul</th>
                <th scope="col">Aug</th>
                <th scope="col">Sep</th>
                <th scope="col">Oct</th>
                <th scope="col">Nov</th>
                <th scope="col">Dec</th>


              </tr>
              </thead>
              <tbody>
              <tr v-for="(packsize, index) in packsizes" :key="index">
                <th scope="row">{{packsize}}L</th>
                <td>{{packsizePerMonth[index]['January']}}</td>
                <td>{{packsizePerMonth[index]['February']}}</td>
                <td>{{packsizePerMonth[index]['March']}}</td>
                <td>{{packsizePerMonth[index]['April']}}</td>
                <td>{{packsizePerMonth[index]['May']}}</td>
                <td>{{packsizePerMonth[index]['June']}}</td>
                <td>{{packsizePerMonth[index]['July']}}</td>
                <td>{{packsizePerMonth[index]['August']}}</td>
                <td>{{packsizePerMonth[index]['September']}}</td>
                <td>{{packsizePerMonth[index]['October']}}</td>
                <td>{{packsizePerMonth[index]['November']}}</td>
                <td>{{packsizePerMonth[index]['December']}}</td>
              </tr>
              <tr>
                <th scope="row"></th>
                <td>{{sumPerMonth2['January']}}</td>
                <td>{{sumPerMonth2['February']}}</td>
                <td>{{sumPerMonth2['March']}}</td>
                <td>{{sumPerMonth2['April']}}</td>
                <td>{{sumPerMonth2['May']}}</td>
                <td>{{sumPerMonth2['June']}}</td>
                <td>{{sumPerMonth2['July']}}</td>
                <td>{{sumPerMonth2['August']}}</td>
                <td>{{sumPerMonth2['September']}}</td>
                <td>{{sumPerMonth2['October']}}</td>
                <td>{{sumPerMonth2['November']}}</td>
                <td>{{sumPerMonth2['December']}}</td>
              </tr>

              </tbody>
            </table>

          </div>
        </template>

        <div class="">
          <canvas class="diagram" id="myChart5"></canvas>
        </div>

        <br/>

      </div>
      <div class="col-sm">

        <template v-if="show===1">
          <div>
            <h1>
              {{$t("formatSplit")}} (%)
            </h1>
          </div>
        </template>
        <canvas class="pieChart" id="packsizeSplit" width="100" height="100"/>

      </div>
    </div>


  </div>
</template>

<script>
import {urlAPI} from "@/variables";
import axios from "axios";


export default {
  name: "ProductionView",
  data() {
    return {
      lo: this.$t("load"),

      site: '',
      beginningDate: '',
      endingDate: '',
      productionline: '',
      show: 0,


      formulations: '',
      formulationsPerMonth: '',
      qtyPerFormulation: [],
      sumPerMonth: [],
      total: 0,

      packsizes: [],
      total2: 0,
      sumPerMonth2: [],
      packsizePerMonth: [],
      dataWorksite : null,
      dataProductionlines : null,
      dataTeams : null,
      username: localStorage.getItem("username"),

      volumes : null,
      team : ''
    }
  },

  methods: {
    load: async function () {

      this.total = 0;
      this.total2 = 0;
      if (this.productionline !== '' && this.beginningDate !== '' && this.endingDate !== '' && this.team !== '') {
        var tab = [];
        tab.push(this.site);
        tab.push(this.productionline);
        tab.push(this.beginningDate);
        tab.push(this.endingDate);

        await axios.get(urlAPI + 'allEvents/' + this.site + '/' + this.productionline + '/' + this.beginningDate + '/' + this.endingDate + '/' + this.team)
            .then(response => (this.volumes = response.data["SITE"]))

        this.makeCalculationFormulation();
        this.pieCharts();
        this.pieCharts2();

        this.graph();
        this.graph2();

        this.show = 1;


      }
    },

    resolveAfter15Second: function () {
      return new Promise(resolve => {
        setTimeout(() => {
          resolve('resolved');
        }, 1500);
      });
    },

    graph: function () {
      var tab = [];
      var colors = [];
      colors.push("#caf270");
      colors.push("#45c490");
      colors.push("#008d93");
      colors.push("#2e5468");


      for (let i = 0; i < this.formulations.length; i++) {
        let mois = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        var ar = [];
        for (let j = 0; j < mois.length; j++) {
          ar.push(this.formulationsPerMonth[i][mois[j]]);
        }
        var obj = {
          label: this.formulations[i],
          backgroundColor: colors[i],
          data: ar,
        };

        tab.push(obj);
      }

    },

    pieCharts: function () {
      var data = [];
      for (let j = 0; j < this.formulations.length; j++) {
        let formulation = this.formulations[j];
        let sum = 0;
        //console.log('PER MONTH');

        //console.log(this.formulationsPerMonth[j]);
        let mois = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];


        for (let k = 0; k < mois.length; k++) {
          let m = mois[k];
          sum += this.formulationsPerMonth[j][m];
        }

        var obj = {
          name: formulation,
          nbr: sum
        };
        data.push(obj);
      }


      const randomHexColorCode = () => {
        return "#" + Math.random().toString(16).slice(2, 8)
      };

      var canvas = document.getElementById("formulationSplit");
      var ctx = canvas.getContext("2d");
      canvas.width = 500;
      canvas.height = 500;
      let total = data.reduce((ttl, house) => {
        return ttl + house.nbr
      }, 0);
      let startAngle = 0;
      let radius = 100;
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
        var pct = item.nbr / this.total * 100;
        txt = txt + ' ' + pct.toFixed(2) + '%';
        ctx.fillText(txt, deltaX + cx, deltaY + cy);
        ctx.closePath();

        startAngle = endAngle;
      }


    },

    graph2: function () {
      var tab = [];
      var colors = [];
      colors.push("#caf270");
      colors.push("#45c490");
      colors.push("#008d93");
      colors.push("#2e5468");


      for (let i = 0; i < this.packsizes.length; i++) {
        let mois = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        var ar = [];
        for (let j = 0; j < mois.length; j++) {
          ar.push(this.packsizePerMonth[i][mois[j]]);
        }
        var obj = {
          label: this.packsizes[i] + 'L',
          backgroundColor: colors[i],
          data: ar,
        };

        tab.push(obj);
      }



    },


    pieCharts2: function () {
      var data = [];
      for (let j = 0; j < this.packsizes.length; j++) {
        let format = this.packsizes[j];
        let sum = 0;
        //console.log('PER MONTH');

        //console.log(this.formulationsPerMonth[j]);
        let mois = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];


        for (let k = 0; k < mois.length; k++) {
          let m = mois[k];
          sum += this.packsizePerMonth[j][m];
        }

        var obj = {
          name: format,
          nbr: sum
        };
        data.push(obj);
      }
      console.log('DATA PIE CHARTS 2');

      console.log(data);


      const randomHexColorCode = () => {
        return "#" + Math.random().toString(16).slice(2, 8)
      };

      var canvas = document.getElementById("packsizeSplit");
      var ctx = canvas.getContext("2d");
      canvas.width = 500;
      canvas.height = 500;
      let total = data.reduce((ttl, house) => {
        return ttl + house.nbr
      }, 0);
      let startAngle = 0;
      let radius = 100;
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

        var txt = item.name + 'L \n';
        var pct = item.nbr / this.total2 * 100;
        txt = txt + ' ' + pct.toFixed(2) + '%';
        ctx.fillText(txt, deltaX + cx, deltaY + cy);
        ctx.closePath();

        startAngle = endAngle;
      }


    },


    makeCalculationFormulation: function () {

      this.formulations = [];

      var qtyPerFormulation = [];

      console.log("VOLUMES : ");
      console.log(this.volumes);

      for (let i = 0; i < this.volumes.length; i++) {
        if (!this.formulations.includes(this.volumes[i].bulk)) {
          this.formulations.push(this.volumes[i].bulk);
          qtyPerFormulation[this.volumes[i].bulk] = this.volumes[i].qtyProduced * this.volumes[i].bottlesPerCase * 1 * this.volumes[i].size ;
        } else {
          qtyPerFormulation[this.volumes[i].bulk] += this.volumes[i].qtyProduced * this.volumes[i].bottlesPerCase  * 1 * this.volumes[i].size;
        }

      }

      var finalValue = [];
      for (let j = 0; j < this.formulations.length; j++) {
        var tableauFormulation = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        var tableauFormulationValue = [];
        for (let k = 0; k < tableauFormulation.length; k++) {
          tableauFormulationValue[tableauFormulation[k]] = 0;
        }


        for (let i = 0; i < this.volumes.length; i++) {
          if (this.volumes[i].bulk === this.formulations[j]) {
            var month = this.volumes[i].created_at.split('-')[1];
            console.log('MOIS : ' + month);

            var correspondingMonth = tableauFormulation[month - 1];
            tableauFormulationValue[correspondingMonth] += this.volumes[i].qtyProduced * this.volumes[i].bottlesPerCase  * 1 * this.volumes[i].size;
          }
        }

        finalValue.push(tableauFormulationValue);
      }

      let l = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
      let tab = [];
      for (let k = 0; k < l.length; k++) {
        tab[l[k]] = 0;
      }
      for (let t = 0; t < finalValue.length; t++) {

        for (let k = 0; k < l.length; k++) {
          let month = l[k];
          tab[month] += finalValue[t][month];
          this.total += finalValue[t][month];
        }

      }
      this.sumPerMonth = tab;
      this.formulationsPerMonth = finalValue;


      //Graph2
      this.packsizes = [];

      var qtyPerPacksize = [];

      for (let i = 0; i < this.volumes.length; i++) {
        if (!this.packsizes.includes(this.volumes[i].size)) {
          this.packsizes.push(this.volumes[i].size);
          qtyPerPacksize[this.volumes[i].size] = this.volumes[i].qtyProduced * this.volumes[i].bottlesPerCase * this.volumes[i].size* 1;
        } else {
          qtyPerPacksize[this.volumes[i].size] += this.volumes[i].qtyProduced * this.volumes[i].bottlesPerCase * this.volumes[i].size;
        }
      }

      var finalValue2 = [];
      for (let j = 0; j < this.packsizes.length; j++) {
        tableauFormulation = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        var tableauPacksizeValue = [];
        for (let k = 0; k < tableauFormulation.length; k++) {
          tableauPacksizeValue[tableauFormulation[k]] = 0;
        }


        for (let i = 0; i < this.volumes.length; i++) {
          if (this.volumes[i].size === this.packsizes[j]) {
            month = this.volumes[i].created_at.split('-')[1];

            correspondingMonth = tableauFormulation[month - 1];
            tableauPacksizeValue[correspondingMonth] += this.volumes[i].qtyProduced * this.volumes[i].bottlesPerCase * this.volumes[i].size;
          }
        }

        finalValue2.push(tableauPacksizeValue);
      }

      l = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
      tab = [];
      for (let k = 0; k < l.length; k++) {
        tab[l[k]] = 0;
      }
      for (let t = 0; t < finalValue2.length; t++) {
        for (let k = 0; k < l.length; k++) {
          let month = l[k];
          tab[month] += finalValue2[t][month];
          this.total2 += finalValue2[t][month];
        }

      }
      this.sumPerMonth2 = tab;
      this.packsizePerMonth = finalValue2;

      console.log("ARRAY")
      console.log(this.sumPerMonth2);
      console.log(this.sumPerMonth);


    }

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


    let chartJs = document.createElement('script');
    chartJs.setAttribute('src', 'https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.3.0/Chart.min.js');
    document.head.appendChild(chartJs);

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

/**
    .table-info-data {
        overflow: scroll;
        max-height: 450px;
        max-width: 400px;
    }


 */


.wrapper {
  width: 60%;
  display: block;
  overflow: hidden;
  margin: 0 auto;
  padding: 60px 50px;
  background: #fff;
  border-radius: 4px;
}

canvas.diagram {
  background: #fff;
}

canvas.piechart{

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
