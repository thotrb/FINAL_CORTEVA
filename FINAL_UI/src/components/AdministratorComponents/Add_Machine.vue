<template>
  <div align="center">
    <div align="center" class="col productionName rcorners2">
      {{$t("addMachine")}}
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
      <div class="form-row">
        <div class="form-group col-md-6">
          <label for="inputEmail4">{{$t('name')}}</label>
          <input type="text" class="form-control" id="inputEmail4" v-model="machine.name"   required>
        </div>
        <div class="form-group col-md-6">
          <label for="inputPassword4">{{$t("operation")}}</label>
          <input type="text" class="form-control" id="inputPassword4" v-model="machine.operation" required >
        </div>
      </div>
      <div class="form-group">
        <label for="inputAddress">{{$t("fabricant")}}</label>
        <input type="text" class="form-control" id="inputAddress"  v-model="machine.fabricant" required>
      </div>
      <div class="form-group">
        <label for="inputAddress2">{{$t("model")}}</label>
        <input type="text" class="form-control" id="inputAddress2"  v-model="machine.modele" required>
      </div>
      <div class="form-group">
        <label for="w">{{$t("worksite")}}</label>
        <select name="m" id="w" class="form-select" v-model="machine.worksite">
          <option  v-for="w in worksites" :key="w.id" v-bind:value="w.name">
            {{w.name}}
          </option>
        </select>
      </div>

      <div class="form-group">
        <label for="l">{{$t("productionLine")}}</label>
        <select name="line" id="l" class="form-select" v-model="machine.productionline_name">
          <template v-for="line in productionlines">
            <template v-if="line.name === machine.worksite">
              <option v-bind:key="line.id" v-bind:value="line.productionline_name">
                {{line.productionline_name}}
              </option>
            </template>
          </template>
        </select>
      </div>
        <div class="form-group">
          <label for="inputAddress2">{{$t("denominationOrder")}}</label>
          <input type="text" class="form-control"   v-model="machine.denomination_ordre" required>
        </div>
      <div class="form-group">
        <label for="inputAddress2">{{$t("order")}}</label>
        <input type="number" class="form-control"   v-model="machine.ordre" required>
      </div>

      <div class="form-group">
        <label for="inputAddress2">{{$t("rejection")}}</label>
        <input type="number" class="form-control"    v-model="machine.rejection" required>
      </div>
      <button type="button" class="btn btn-primary" v-on:click="addMachine()">{{$t('add')}}</button>
    </form>

    <br/>
    <br/>


    <template v-if="machines.length > 0">

      <table class="table">
        <thead class="thead-dark">
        <tr>
          <th scope="col">{{ $t('name') }}</th>
          <th scope="col">{{ $t('operation') }}</th>
          <th scope="col">{{ $t('fabricant') }}</th>
          <th scope="col">{{ $t('model') }}</th>
          <th scope="col">{{ $t('worksite') }}</th>
          <th scope="col">{{ $t('productionLine') }}</th>
          <th scope="col">{{ $t('denominationOrder') }}</th>
          <th scope="col">{{ $t('order') }}</th>
          <th scope="col">{{ $t('rejection') }}</th>
          <th scope="col">{{ $t('action') }}</th>

        </tr>
        </thead>
        <tbody>
            <tr v-for="m in machines" :key="m.id">
              <th scope="row">{{m.name}}</th>
              <td>{{m.operation}}</td>
              <td>{{m.fabricant}}</td>
              <td>{{m.modele}}</td>
              <td>{{m.worksite}}</td>
              <td>{{m.productionline_name}}</td>
              <td>{{m.denomination_ordre}}</td>
              <td>{{m.ordre}}</td>
              <td>{{m.rejection}}</td>
              <td><button type="button" class="btn btn-danger" @click="deleteItem(m.id)">{{$t('delete')}}</button></td>

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
  name: "Add_Machine",

  data() {
    return {
      username: localStorage.getItem("username"),
      data : null,
      worksites : null,
      productionlines : null,
      userWorksite : null,
      effective : null,
      machines : [],
      machine: {

        name : null,
        operation : null,
        fabricant : null,
        modele : null,
        productionline_name : null,
        denomination_ordre : null,
        ordre : null,
        rejection : null,
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

      await axios.delete(urlAPI + 'deleteMachine/' + id);
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
          var machine2 = {
            name: null,
            operation: null,
            fabricant: null,
            modele: null,
            productionline_name: null,
            denomination_ordre: null,
            ordre: null,
            rejection: null,
            worksite: null,
          };
          var effective;
          for (i = 1; i < rows.length - 1; i++) {
            rowsSplited = rows[i].split('\r')[0].split(',');
            if (rowsSplited.length === 9) {
              machine2.name = rowsSplited[0];
              machine2.operation = rowsSplited[1];
              machine2.fabricant = rowsSplited[2];
              machine2.modele = rowsSplited[3];
              machine2.productionline_name = rowsSplited[4];
              machine2.denomination_ordre = rowsSplited[5];
              machine2.ordre = rowsSplited[6];
              machine2.rejection = rowsSplited[7];
              machine2.worksite = rowsSplited[8];
              console.log(machine2);

              await axios.put(urlAPI + 'insertMachine', machine2)
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
        console.log(this.machine);

        await axios.put(urlAPI + 'insertMachine', this.machine)
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

    axios.get(urlAPI+'administratorMachine/' + this.userWorksite)
          .then(response => (this.machines = response.data))




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