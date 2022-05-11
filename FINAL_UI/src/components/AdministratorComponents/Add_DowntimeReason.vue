<template>
  <div align="center">
    <div align="center" class="col productionName rcorners2">
      {{$t("addDowntimeReason")}}
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
        <label for="w">{{$t("worksite")}}</label>
        <select name="m" id="w" class="form-select" v-model="downtimeReason.worksite">
          <option  v-for="w in worksites" :key="w.id" v-bind:value="w.name">
            {{w.name}}
          </option>
        </select>
      </div>

      <div class="form-group">
        <label for="l">{{$t("productionLine")}}</label>
        <select name="line" id="l" class="form-select" v-model="downtimeReason.production_line">
          <template v-for="line in productionlines">
            <template v-if="line.name === downtimeReason.worksite">
              <option v-bind:key="line.id" v-bind:value="line.productionline_name">
                {{line.productionline_name}}
              </option>
            </template>
          </template>
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
          <th scope="col">{{ $t('action') }}</th>
        </tr>
        </thead>
        <tbody>
            <tr v-for="d in downtimeReasons" :key="d.id">
              <th scope="row">{{d.reason}}</th>
              <td>{{d.downtimeType}}</td>
              <td>{{d.worksite}}</td>
              <td>{{d.production_line}}</td>
              <td><button type="button" class="btn btn-danger" @click="deleteItem(d.id)">{{$t('delete')}}</button></td>
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
      worksites : null,
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

    deleteItem : async function (id) {

      await axios.delete(urlAPI + 'deleteDowntime/' + id);
      location.reload();
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
          var downtimeReason2 = {
            reason: '',
            downtimeType: '',
            worksite: '',
            production_line: '',
          };
          var effective;
          for (i = 1; i < rows.length - 1; i++) {
            rowsSplited = rows[i].split('\r')[0].split(',');
            if (rowsSplited.length === 4) {
              downtimeReason2.reason = rowsSplited[0];
              downtimeReason2.downtimeType = rowsSplited[1];
              downtimeReason2.worksite = rowsSplited[2];
              downtimeReason2.production_line = rowsSplited[3];
              console.log(downtimeReason2);

              await axios.put(urlAPI + 'insertDowntimeReason', downtimeReason2)
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
        console.log("OK");
        console.log(this.downtimeReason);

        await axios.put(urlAPI + 'insertDowntimeReason', this.downtimeReason)
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
    await axios.get(urlAPI+'administratorMachine/' + this.userWorksite)
        .then(response => (this.machinesAvailable = response.data))


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