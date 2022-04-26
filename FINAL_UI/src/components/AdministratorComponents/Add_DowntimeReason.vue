<template>
  <div align="center">
    <div align="center" class="col productionName rcorners2">
      {{$t("addDowntimeReason")}}
    </div>

    <br/>
    <br/>

    <form id="needs-validation" novalidate>
      <div class="form-group">
        <label for="inputEmail4">{{$t('reason')}}</label>
        <input type="text" class="form-control" id="inputEmail4" v-model="downtimeReason.reason"   required>
      </div>
      <div class="form-group">
        <label for="d">{{$t("downtimeType")}}</label>
        <select name="line" id="d" class="form-select" v-model="downtimeReason.downtimeType">
          <option value="plannedDowntime">{{ $t('plannedDowntime') }}</option>
          <option value="unplannedDowntime">{{ $t('unplannedDowntime') }}</option>
        </select>
      </div>
      <div class="form-group">
          <label for="l">{{$t("productionLine")}}</label>
        <select name="line" id="l" class="form-select" v-model="downtimeReason.production_line">
            <option  v-for="line in productionlines" :key="line.id" v-bind:value="line.productionline_name">
              {{line.productionline_name}}
            </option>
          </select>
      </div>
      <button type="button" class="btn btn-primary" v-on:click="addMachine()">{{$t('add')}}</button>
    </form>

    <br/>
    <br/>


    <template v-if="downtimeReasons.length > 0">

      <table class="table">
        <thead class="thead-dark">
        <tr>
          <th scope="col">{{ $t('reason') }}</th>
          <th scope="col">{{ $t('downtimeType') }}</th>
          <th scope="col">{{ $t('worksite') }}</th>
          <th scope="col">{{ $t('productionLine') }}</th>
        </tr>
        </thead>
        <tbody>
            <tr v-for="d in downtimeReasons" :key="d.id">
              <th scope="row">{{d.reason}}</th>
              <td>{{d.downtimeType}}</td>
              <td>{{d.worksite}}</td>
              <td>{{d.production_line}}</td>
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
  name: "Add_DowntimeReason",

  data() {
    return {
      username: localStorage.getItem("username"),
      data : null,
      productionlines : null,
      userWorksite : null,
      effective : null,
      downtimeReasons : [],
      downtimeReason: {
        reason : null,
        downtimeType : null,
        fabricant : null,
        production_line : null,
        worksite : null,
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
            this.downtimeReason.worksite = this.userWorksite;
            console.log("OK");
            console.log(this.downtimeReason);

            axios.put(urlAPI + 'insertDowntimeReason', this.downtimeReason)
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


    axios.get(urlAPI+'administratorDowntimeReason/' + this.userWorksite)
          .then(response => (this.downtimeReasons = response.data))


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