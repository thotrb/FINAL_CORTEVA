<template>
  <div>

    <div id="imageArrow">
    </div>


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
                    <option v-bind:value="productionline.productionline_name" v-bind:key="productionline.id">
                      {{productionline.productionline_name}}
                    </option>
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
              {{$t("packagingLineID")}}
            </h1>
          </div>
        </div>
      </div>
    </div>

    <div class="row">
      <div class="col-sm">
        <template v-if="show === 1">
          <div align="center">
            <h2 align="center">
              {{$t("flowDiagram")}}
            </h2>
          </div>
      <!--
        <div id="flowDiagram">
        </div>
        -->
        <img :src="imgUrl" width="100%">
        </template>

      </div>

      <div class="col-sm">
        <template v-if="show === 1">
          <div>
            <h2>
              {{$t("machineList")}}
            </h2>
          </div>

          <div class="table-info-data">
            <table class="table">
              <thead>
              <tr>
                <th scope="col"></th>
                <th scope="col">{{$t("machine")}}</th>
                <th scope="col">{{$t("operation")}}</th>
                <th scope="col">{{$t("provider")}}</th>
                <th scope="col">{{$t("model")}}</th>
              </tr>
              </thead>
              <tbody>
              <template v-for="machine in machines[0]">
                <tr v-bind:key="machine.id">
                  <th scope="row">{{machine.denomination_ordre}}</th>
                  <td>{{machine.name}}</td>
                  <td>{{machine.operation}}</td>
                  <td>{{machine.fabricant}}</td>
                  <td>{{machine.modele}}</td>

                </tr>
              </template>
              </tbody>
            </table>
          </div>


          <br/>
          <div>
            <h2>
              {{$t("formatList")}}
            </h2>
          </div>


          <div class="table-info-data">
            <table class="table">
              <thead>
              <tr>
                <th scope="col">{{$t("format")}}</th>
                <th scope="col">{{$t("form")}}</th>
                <th scope="col">{{$t("mat1")}}</th>
                <th scope="col">{{$t("mat2")}}</th>
                <th scope="col">{{$t("mat3")}}</th>
                <th scope="col">{{$t("designRate")}}</th>

              </tr>
              </thead>
              <tbody>
              <template v-for="format in machines[1]">
                <tr v-bind:key="format.id">
                  <th scope="row">{{format.format}}</th>
                  <td>{{$t(format.shape)}}</td>
                  <td>{{format.mat1}}</td>
                  <td>{{format.mat2}}</td>
                  <td>{{format.mat3}}</td>
                  <td>{{format.design_rate}}</td>

                </tr>
              </template>
              </tbody>
            </table>

          </div>

          <br/>

        </template>
      </div>


    </div>



  </div>

</template>

<script>


import axios from "axios";
import {urlAPI} from "@/variables";
//import ArrowHorizontale from "@/components/DashboardComponents/ArrowHorizontale";


export default {
  name: "PackaginglineID",
  data() {
    return {
      site: '',
      productionline: '',
      product: '',
      formulationType: '',
      reporting: '',
      tool: '',
      username: localStorage.getItem("username"),
      index: -1,
      show: 0,
      imgUrl : '',
      lo: this.$t("load"),

      dataWorksite:null,
      dataProductionlines : null,
      machines : null,
    }
  },

  methods: {
    resolveAfter15Second: function () {
      return new Promise(resolve => {
        setTimeout(() => {
          resolve('resolved');
        }, 1500);
      });
    },


    load: async function () {
      var index = 0;
      //var tableauMachine = [];

      for (let i = 0; i < this.dataProductionlines.length; i++) {

        if (this.dataProductionlines[i].productionline_name === this.productionline) {
          index = this.dataProductionlines[i].id;
          console.log(this.dataProductionlines[i])

        }
      }


      if (this.productionline !== '') {

        await axios.get(urlAPI + 'machines/' + index + '/'+this.site)
            .then(response => (this.machines = response.data))

        this.index = index;

      }

      var image = require.context('../../assets/', false, /\.png$/);

      this.imgUrl = image('./' + this.productionline + '.png');



      await this.resolveAfter15Second();

      /**
      var bigDiv2 = null;
      var machine2 = null;
      var row2 = null;
      var title2 = null;
      var rej2 = null;
      var insert = document.getElementById("flowDiagram");
      for (let i = 0; i < this.machines[0].length; i++) {
        machine2 = this.machines[0][i];
        row2 = document.createElement("div");
        row2.setAttribute("class", "row");
        row2.setAttribute("style", "margin: 30px;height: 50px;");
        row2.setAttribute("class", "machine col-sm");
        row2.setAttribute("style", "color:white; background-color: lightblue; height: 50px; margin-bottom: 30px;");
        row2.setAttribute("align", "center");
        title2 = document.createElement("h1");
        title2.innerHTML = machine2.name;
        row2.appendChild(title2);
        if(machine2.rejection === 1){
          console.log('je passe');
          rej2 = document.createElement("h1");
          rej2.innerHTML = "R";
          var img2 = document.createElement("img");
          img2.src = 'http://www.google.com/intl/en_com/images/logo_plain.png';
          row2.appendChild(img2);
          row2.appendChild(rej2)
        }
        insert.append(row2);
      }
**/




/**
      var rejection = null;
      var R = null;


      var row = document.createElement("div");
      row.setAttribute("class", "row");
      row.setAttribute("style", "margin: 30px;height: 50px; height: 50px;");


      var insert = document.getElementById("flowDiagram");
      var previousMachine = this.machines[0][0];
      var div = document.createElement("div");
      div.setAttribute("id", previousMachine.name);
      var h5 = document.createElement("h5");
      h5.innerHTML = previousMachine.denomination_ordre;
      h5.setAttribute("style", "padding-top:15px;");
      //h5.setAttribute("id", previousMachine.name);
      div.appendChild(h5);
      div.setAttribute("class", "machine col-sm");
      div.setAttribute("style", "color:white; background-color: lightblue; height: 50px; margin-bottom: 30px;");
      div.setAttribute("align", "center");


      if (previousMachine.rejection === 1) {
        rejection = document.createElement("div");
        R = document.createElement("h5");
        R.innerHTML = "R";
        R.setAttribute("style", "padding-top:15px;");
        R.setAttribute("id", "rejection_" + previousMachine.name);

        rejection.append(R);
        rejection.setAttribute("id", "rejection_" + previousMachine.name);
        rejection.setAttribute("style", "color:red;");
        rejection.setAttribute("align", "center");
        rejection.setAttribute("class", "col-sm");
        //var img2 = document.createElement('img');
        //img2.src = require('../../assets/arrowHorizontale.png');
        //var img2  = require('../../assets/arrowHorizontale.png');
        //row.appendChild(img2);
        row.appendChild(rejection);

      }


      row.appendChild(div);
      insert.appendChild(row);
      //var arrowVerticale = document.createElement("img");
      //arrowVerticale.src = '../../assets/arrowVerticale.png';

      //insert.appendChild(arrowVerticale);
      var machine = null;

      for (let i = 1; i < this.machines[0].length; i++) {

        machine = this.machines[0][i];

        row = document.createElement("div");
        row.setAttribute("class", "row");
        row.setAttribute("style", "margin: 30px;height: 50px; height: 50px;");

        div = document.createElement("div");
        h5 = document.createElement("h5");
        h5.innerHTML = machine.denomination_ordre;
        h5.setAttribute("style", "padding-top:15px;");

        div.appendChild(h5);
        //h5.setAttribute("id", machine.name);
        div.setAttribute("class", "machine col-sm");
        div.setAttribute("style", "color:white; background-color: lightblue; ");
        div.setAttribute("align", "center");
        div.setAttribute("id", machine.name);


        row.appendChild(div);



        if (machine.rejection === 1) {
          rejection = document.createElement("div");

          R = document.createElement("h5");
          R.innerHTML = "R";
          R.setAttribute("style", "padding-top:15px;");

          rejection.append(R);
          rejection.setAttribute("id", "rejection_" + machine.name);
          rejection.setAttribute("style", "color:red;");
          rejection.setAttribute("align", "center");
          rejection.setAttribute("class", "col-sm");
          //var img = document.createElement("img");
          //img.src = '../../assets/arrowHorizontale.png';


          //row.appendChild(img);
          row.appendChild(rejection);

        }

        insert.appendChild(row);
        //var arrowVerticale2 = document.createElement("img");
        //arrowVerticale2.src = '../../assets/arrowVerticale.png';

        //insert.appendChild(arrowVerticale2)

        /**
        new LeaderLine(
            document.getElementById(previousMachine.name),
            document.getElementById(machine.name),
            {color: 'red'}
        );

        if (machine.rejection === 1) {
          new LeaderLine(
              document.getElementById(machine.name),
              document.getElementById("rejection_" + machine.name),
              {color: 'red'}
          );
        }


        previousMachine = machine;
       }
         **/



      this.show = 1;

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


  },


}


</script>

<style scoped>

h1 {
  font-size: 1.4em;
  color: #56baed;
}

h2 {
  font-size: 1.2em;
  color: #56baed;
}

div {
  background-color: #fff;
  padding: 15px;
}

thead {
  color: white;
  background: #56baed;
}

.table-info-data {
  overflow: scroll;
  max-height: 200px;
}

#d1, #d2 {
  height: 50px;
}

#d1 {

  background-color: green;
}

#d2 {
  background-color: blue;
}

.machine {
  background-color: lightblue;
  color: white;

}

.arrow {
  padding-top: 15px;
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
