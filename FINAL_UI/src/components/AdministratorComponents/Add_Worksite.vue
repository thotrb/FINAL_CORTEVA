<template>
  <div align="center">
    <div align="center" class="col productionName rcorners2">
      {{$t("addWorksite")}}
    </div>

    <br/>
    <br/>

    <template v-if="errorRequest!==0">
      <div align="center" class="col productionName rcorners3">
        {{$t("anErrorHasOccured")}} <br>
        {{$t("worksiteMustBeUnique")}} : {{worksiteProbleme}}
      </div>

      <br/>
      <br/>
    </template>


    <div>
      <label for="csv">{{$t('select.csvFileToUse')}}</label>
      <input type="file" id="csv" name="profile_pic"
             accept=".csv">
      <p id="fileDisplayArea"></p>
      <button type="button" class="btn btn-primary" v-on:click="readFile(function(worksite) {errorRequest = 1; worksiteProbleme = worksite;})">{{ $t('load') }}</button>

    </div>

    <br/>
    <br/>

    <form id="needs-validation" novalidate>
      <div class="form-group">
        <label for="inputEmail4">{{$t('name')}} </label>
        <input type="text" class="form-control" id="inputEmail4" v-model="worksite.name"   required>
      </div>

      <button type="button" class="btn btn-primary" v-on:click="addMachine()">{{$t('add')}}</button>
    </form>

    <br/>
    <br/>


    <template v-if="worksites.length > 0">

      <table class="table">
        <thead class="thead-dark">
        <tr>
          <th scope="col">{{ $t('name') }}</th>
          <th scope="col">{{ $t('action') }}</th>

        </tr>
        </thead>
        <tbody>
            <tr v-for="d in worksites" :key="d.id">
              <th scope="row">{{d.name}}</th>
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
  name: "Add_Worksite",

  data() {
    return {
      username: localStorage.getItem("username"),
      data : null,
      worksites :[],
      productionlines : null,
      userWorksite : null,
      effective : null,
      errorRequest : 0,
      worksiteProbleme : '',
      worksite: {
        name : null
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

      await axios.delete(urlAPI + 'deleteWorksite/' + id);
      location.reload();
    },

    readFile : function (callback) {
      var textType = /.csv/;
      var doc = document.getElementById("csv").files[0];

      if (doc.type.match(textType)) {

        //console.log(doc);
        var reader = new FileReader();
        reader.readAsText(doc);
        reader.onload = async function (e) {
          var rows = e.target.result.split('\n');
          var rowsSplited = null;

          var i = 1;
          var worksite2 = {
            name: null,

          };

          this.errorRequest = 0;
          while (i < rows.length - 1 && this.errorRequest === 0) {

            rowsSplited = rows[i].split('\r')[0].split(',');
            if (rowsSplited.length === 1) {
              worksite2.name = rowsSplited[0];
              console.log(worksite2);


              await axios.put(urlAPI + 'insertWorksite', worksite2)
                  .then(response => {
                    this.effective = response;
                    console.log(response)
                    if (response.status === 500) {
                      console.log("Je passe");
                    } else {
                      this.effective = response;
                    }
                  })
                  .catch(error => {
                    console.log("catch error");
                    console.log(error);
                    this.errorRequest = 1;
                    this.loginProbleme = worksite2.name;
                    callback(worksite2.name);
                  })
            }
            i++;
          }
          if(this.errorRequest === 0){
            location.reload();
          }
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
        console.log(this.user);



        await axios.put(urlAPI + 'insertWorksite', this.worksite)
            .then(response => {this.effective = response; console.log(response)
              if(response.status === 500){
                console.log("Je passe");
              }else {
                this.effective = response;
                location.reload();
              }
            })
            .catch(error => {console.log("je passe"); console.log(error); this.errorRequest=1;this.worksiteProbleme = this.worksite.name})

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

.rcorners3 {
  margin-top : 20px;
  border-radius: 25px;
  border: 4px solid red;
  padding: 20px;
  color : red;
  font-weight: bold;
}

</style>