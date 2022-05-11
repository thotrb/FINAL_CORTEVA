<template>
  <div align="center">
    <div align="center" class="col productionName rcorners2">
      {{$t("addUser")}}
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
        <label for="inputEmail4">{{$t('login')}} </label>
        <input type="text" class="form-control" id="inputEmail4" v-model="user.login"   required>
      </div>

      <div class="form-group">
        <label for="inputEmail">{{$t('password')}} </label>
        <input type="text" class="form-control" id="inputEmail" v-model="user.password"   required>
      </div>


      <div class="form-group">
        <label for="inpu">{{$t('lastName')}} </label>
        <input type="text" class="form-control" id="inpu" v-model="user.lastname"   required>
      </div>

      <div class="form-group">
        <label for="inp">{{$t('firstName')}} </label>
        <input type="text" class="form-control" id="inp" v-model="user.firstname"   required>
      </div>

      <div class="form-group">
        <label for="d">{{$t("status")}}</label>
        <select name="line" id="d" class="form-select" v-model="user.status">
          <option value="0">{{ $t('operator') }}</option>
          <option value="1">{{ $t('supervisor') }}</option>
          <option value="2">{{ $t('administrator') }}</option>
        </select>
      </div>


      <div class="form-group">
        <label for="w">{{$t("worksite")}}</label>
        <select name="m" id="w" class="form-select" v-model="user.worksite_name">
          <option  v-for="w in worksites" :key="w.id" v-bind:value="w.name">
            {{w.name}}
          </option>
        </select>
      </div>

      <div class="form-group">
        <label for="l">{{$t("productionLine")}}</label>
        <select name="line" id="l" class="form-select" v-model="user.productionline">
          <template v-for="line in productionlines">
            <template v-if="line.name === user.worksite_name">
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


    <template v-if="users.length > 0">

      <table class="table">
        <thead class="thead-dark">
        <tr>
          <th scope="col">{{ $t('login') }}</th>
          <th scope="col">{{ $t('firstName') }}</th>
          <th scope="col">{{ $t('lastName') }}</th>
          <th scope="col">{{ $t('status') }}</th>
          <th scope="col">{{ $t('worksite') }}</th>
          <th scope="col">{{ $t('productionLine') }}</th>
          <th scope="col">{{ $t('action') }}</th>


        </tr>
        </thead>
        <tbody>
            <tr v-for="d in users" :key="d.id">
              <th scope="row">{{d.login}}</th>
              <td>{{d.firstname}}</td>
              <td>{{d.lastname}}</td>
              <td>{{d.status}}</td>
              <td>{{d.worksite_name}}</td>
              <td>{{d.productionline}}</td>
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
  name: "Add_User",

  data() {
    return {
      username: localStorage.getItem("username"),
      data : null,
      worksites :null,
      productionlines : null,
      userWorksite : null,
      effective : null,
      users : [],
      user: {
        login : null,
        password : null,
        worksite_name : null,
        lastname : null,
        firstname : null ,
        status : null,
        productionline : null,

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

      await axios.delete(urlAPI + 'deleteUser/' + id);
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
          var user2 = {
            login: null,
            password: null,
            worksite_name: null,
            lastname: null,
            firstname: null,
            status: null,
            productionline: null,

          };
          var effective;
          for (i = 1; i < rows.length - 1; i++) {
            rowsSplited = rows[i].split('\r')[0].split(',');
            if (rowsSplited.length === 7) {
              user2.login = rowsSplited[0];
              user2.password = rowsSplited[1];
              user2.worksite_name = rowsSplited[2];
              user2.lastname = rowsSplited[3];
              user2.firstname = rowsSplited[4];
              user2.status = rowsSplited[5];
              user2.productionline = rowsSplited[6];
              console.log(user2);

              await axios.put(urlAPI + 'insertUser', user2)
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
        console.log(this.user);


        await axios.put(urlAPI + 'insertUser', this.user)
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


    axios.get(urlAPI+'administratorUsers/' + this.userWorksite)
          .then(response => (this.users = response.data))


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