<template>
  <div align="center">
    <div align="center" class="col productionName rcorners2">
      {{$t("addTeamInfo")}}
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
        <label for="inputEmail4">{{$t('workingDebut')}}</label>
        <input type="number" step="O.1" class="form-control" id="inputEmail4" v-model="teamInfo.workingDebut"   required>
      </div>

      <div class="form-group">
        <label for="inputEmail">{{$t('workingEnd')}}</label>
        <input type="number" step="O.1" class="form-control" id="inputEmail" v-model="teamInfo.workingEnd"   required>
      </div>

      <div class="form-group">
        <label for="i">{{$t('name')}}</label>
        <input type="text" class="form-control" id="i" v-model="teamInfo.type"   required>
      </div>


      <div class="form-group">
        <label for="w">{{$t("worksite")}}</label>
        <select name="m" id="w" class="form-select" v-model="teamInfo.worksite_name">
          <option  v-for="w in worksites" :key="w.id" v-bind:value="w.name">
            {{w.name}}
          </option>
        </select>
      </div>


      <button type="button" class="btn btn-primary" v-on:click="addMachine()">{{$t('add')}}</button>
    </form>

    <br/>
    <br/>


    <template v-if="teamInfos.length > 0">

      <table class="table">
        <thead class="thead-dark">
        <tr>
          <th scope="col">{{ $t('workingDebut') }}</th>
          <th scope="col">{{ $t('workingEnd') }}</th>
          <th scope="col">{{ $t('name') }}</th>
          <th scope="col">{{ $t('worksite') }}</th>
          <th scope="col">{{ $t('action') }}</th>

        </tr>
        </thead>
        <tbody>
            <tr v-for="d in teamInfos" :key="d.id">
              <th scope="row">{{d.workingDebut}}</th>
              <td>{{d.workingEnd}}</td>
              <td>{{d.type}}</td>
              <td>{{d.worksite_name}}</td>
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
  name: "Add_TeamInfo",

  data() {
    return {
      username: localStorage.getItem("username"),
      data : null,
      productionlines : null,
      userWorksite : null,
      effective : null,
      teamInfos : [],
      worksites : null,
      teamInfo: {
        workingDebut : null,
        workingEnd : null,
        type : null,
        worksite_name : null,
        state : 0,
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

      await axios.delete(urlAPI + 'deleteTeamInfo/' + id);
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
          var teamInfo2 = {
            workingDebut: null,
            workingEnd: null,
            type: null,
            worksite_name: null,
            state: 0,
          };
          var effective;
          for (i = 1; i < rows.length - 1; i++) {
            rowsSplited = rows[i].split('\r')[0].split(',');
            if (rowsSplited.length === 5) {
              teamInfo2.workingDebut = rowsSplited[0];
              teamInfo2.workingEnd = rowsSplited[1];
              teamInfo2.type = rowsSplited[2];
              teamInfo2.worksite_name = rowsSplited[3];
              console.log(teamInfo2);

              await axios.put(urlAPI + 'insertTeamInfo', teamInfo2)
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

        await axios.put(urlAPI + 'insertTeamInfo', this.teamInfo)
            .then(response => (this.effective = response))

        console.log('Effectif : ' + this.effective);
        location.reload();

      }


      form.classList.add('was-validated');
    },


  },
  async mounted() {
    await axios.get(urlAPI+'user/'+this.username)
        .then(response => (this.data = response.data))

    await this.resolveAfter15Second();

    this.userWorksite  = this.data[0][0].worksite_name;


    await axios.get(urlAPI+'sites')
        .then(response => (this.productionlines = response.data))
    this.worksites = this.productionlines[0];
    this.productionlines = this.productionlines[1];
    console.log(this.productionlines);
    await axios.get(urlAPI+'administratorTeamInfo/' + this.userWorksite)
        .then(response => (this.teamInfos = response.data))

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