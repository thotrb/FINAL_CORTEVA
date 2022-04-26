<template>
  <div align="center">
    <div align="center" class="col productionName rcorners2">
      {{$t("addFormat")}}
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

    addMachine : function () {
      var form = document.getElementById('needs-validation');
          if (form.checkValidity() === false) {
            event.preventDefault();
            event.stopPropagation();
            console.log("PAS OK");

          }else {
            this.format.worksite = this.userWorksite;
            this.format.format = this.format.format + 'L';
            if(this.format.mat1 === ''){
              this.format.mat1 = 'None';
            }
            if(this.format.mat2 === ''){
              this.format.mat2 = 'None';
            }
            if(this.format.mat3 === ''){
              this.format.mat3 = 'None';
            }
            console.log("OK");
            console.log(this.format);


            axios.put(urlAPI + 'insertFormat', this.format)
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


    axios.get(urlAPI+'getProductionLines/' + this.userWorksite)
        .then(response => (this.productionlines = response.data))


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