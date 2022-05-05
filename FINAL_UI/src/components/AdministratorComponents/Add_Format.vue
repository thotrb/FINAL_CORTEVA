<template>
  <div align="center">
    <div align="center" class="col productionName rcorners2">
      {{$t("addFormat")}}
    </div>

    <br/>
    <br/>


    <div>
      <label for="csv">{{$t('select.csvFileToUse')}}</label>
      <input type="file" id="csv" name="profile_pic"
             accept=".csv">
      <p id="fileDisplayArea"></p>
      <button type="button" class="btn btn-primary" v-on:click="readFile()">{{ $t('load') }}</button>

    </div>

    <br/>
    <br/>

    <form id="needs-validation" novalidate>
      <div class="form-group">
        <label for="inputEmail4">{{$t('format')}} (L) </label>
        <input type="number" step=".01" class="form-control" id="inputEmail4" v-model="format.format"   required>
      </div>
      <div class="form-group">
        <label for="d">{{$t("shape")}}</label>
        <select name="line" id="d" class="form-select" v-model="format.shape">
          <option value="round">{{ $t('round') }}</option>
          <option value="square">{{ $t('square') }}</option>
        </select>
      </div>
      <div class="form-group">
        <label for="l">{{$t("productionLine")}}</label>
        <select name="line" id="l" class="form-select" v-model="format.productionlineName">
          <option  v-for="line in productionlines" :key="line.id" v-bind:value="line.productionline_name">
            {{line.productionline_name}}
          </option>
        </select>
      </div>
      <div class="form-group">
        <label for="inputEmail">{{$t('mat1')}}</label>
        <input type="text" class="form-control" id="inputEmail" v-model="format.mat1">
      </div>
      <div class="form-group">
        <label for="inputEmail1">{{$t('mat2')}}</label>
        <input type="text" class="form-control" id="inputEmail1" v-model="format.mat2">
      </div>
      <div class="form-group">
        <label for="inputEmail2">{{$t('mat3')}}</label>
        <input type="text" class="form-control" id="inputEmail2" v-model="format.mat3">
      </div>
      <div class="form-group">
        <label for="inputEmai">{{$t('designRate')}}</label>
        <input type="number" class="form-control" id="inputEmai" v-model="format.design_rate"   required>
      </div>
      <button type="button" class="btn btn-primary" v-on:click="addMachine()">{{$t('add')}}</button>
    </form>

    <br/>
    <br/>


    <template v-if="formats.length > 0">

      <table class="table">
        <thead class="thead-dark">
        <tr>
          <th scope="col">{{ $t('productionLine') }}</th>
          <th scope="col">{{ $t('format') }}</th>
          <th scope="col">{{ $t('shape') }}</th>
          <th scope="col">{{ $t('mat1') }}</th>
          <th scope="col">{{ $t('mat2') }}</th>
          <th scope="col">{{ $t('mat3') }}</th>
          <th scope="col">{{ $t('designRate') }}</th>

        </tr>
        </thead>
        <tbody>
            <tr v-for="d in formats" :key="d.id">
              <th scope="row">{{d.productionlineName}}</th>
              <td>{{d.format}}</td>
              <td>{{d.shape}}</td>
              <td>{{d.mat1}}</td>
              <td>{{d.mat2}}</td>
              <td>{{d.mat3}}</td>
              <td>{{d.design_rate}}</td>

            </tr>
        </tbody>
      </table>
    </template>


  </div>
</template>

<script>



import axios from "axios";
import {urlAPI} from "@/variables";

export default {
  name: "Add_Format",

  data() {
    return {
      username: localStorage.getItem("username"),
      data : null,
      worksites :null,
      productionlines : null,
      userWorksite : null,
      effective : null,
      formats : [],
      format: {
        format : null,
        shape : null,
        mat1 : 'None',
        mat2 : 'None',
        mat3 : 'None',
        design_rate : null,
        productionlineName : null,

      },
    }
  },
  methods : {
    resolveAfter15Second: function () {
      return new Promise(resolve => {
        setTimeout(() => {
          resolve('resolved');
        }, 1500);
      });
    },

    readFile : function () {
      var textType = /.csv/;
      var doc = document.getElementById("csv").files[0];

      if (doc.type.match(textType)) {

        //console.log(doc);
        var reader = new FileReader();
        reader.readAsText(doc);
        reader.onload = async function (e) {
          var rows = e.target.result.split('\n');
          var rowsSplited = null;

          var i;
          var format2 = {
            format: null,
            shape: null,
            mat1: 'None',
            mat2: 'None',
            mat3: 'None',
            design_rate: null,
            productionlineName: null,

          };
          var effective;
          for (i = 1; i < rows.length - 1; i++) {
            rowsSplited = rows[i].split('\r')[0].split(',');
            if (rowsSplited.length === 7) {
              format2.format = rowsSplited[0];
              format2.shape = rowsSplited[1];
              format2.mat1 = rowsSplited[2];
              format2.mat2 = rowsSplited[3];
              format2.mat3 = rowsSplited[4];
              format2.design_rate = rowsSplited[5];
              format2.productionlineName = rowsSplited[6];
              console.log(format2);

              await axios.put(urlAPI + 'insertFormat', format2)
                  .then(response => (effective = response))
              console.log(effective)
            }
          }
          location.reload();

        }
      }else{
        var fileDisplayArea = document.getElementById('fileDisplayArea');
        fileDisplayArea.innerText = this.$t('fileNotSupported');

      }

    },

    addMachine : async function () {
      var form = document.getElementById('needs-validation');
      if (form.checkValidity() === false) {
        event.preventDefault();
        event.stopPropagation();
        console.log("PAS OK");

      } else {
        this.format.format = this.format.format + 'L';
        if (this.format.mat1 === '') {
          this.format.mat1 = 'None';
        }
        if (this.format.mat2 === '') {
          this.format.mat2 = 'None';
        }
        if (this.format.mat3 === '') {
          this.format.mat3 = 'None';
        }
        console.log("OK");
        console.log(this.format);


        await axios.put(urlAPI + 'insertFormat', this.format)
            .then(response => (this.effective = response))

        console.log('Effectif : ' + this.effective);
        location.reload();

      }


      form.classList.add('was-validated');
    },


  },
  async mounted() {
    axios.get(urlAPI+'user/'+this.username)
        .then(response => (this.data = response.data))

    await this.resolveAfter15Second();

    this.userWorksite  = this.data[0][0].worksite_name;


    await axios.get(urlAPI+'sites')
        .then(response => (this.productionlines = response.data))
    this.worksites = this.productionlines[0];
    this.productionlines = this.productionlines[1];
    console.log(this.productionlines);


    axios.get(urlAPI+'administratorFormat/' + this.userWorksite)
          .then(response => (this.formats = response.data))


  }
}
</script>

<style scoped>

.rcorners2 {
  margin-top : 20px;
  border-radius: 25px;
  border: 2px solid lightblue;
  padding: 20px;
}

</style>