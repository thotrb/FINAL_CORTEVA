<template>
  <div align="center">
    <div align="center" class="col productionName rcorners2">
      {{$t("addMachineComponent")}}
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
          <label for="inputEmail4">{{$t('name')}}</label>
          <input type="text" class="form-control" id="inputEmail4" v-model="machineComponent.name"   required>
        </div>



      <div class="form-group">
        <label for="w">{{$t("worksite")}}</label>
        <select name="m" id="w" class="form-select" v-model="machineComponent.worksite">
          <option  v-for="w in worksites" :key="w.id" v-bind:value="w.name">
            {{w.name}}
          </option>
        </select>
      </div>

        <div class="form-group">
          <label for="l">{{$t("productionLine")}}</label>
          <select name="line" id="l" class="form-select" v-model="machineComponent.productionLine" required>
            <template v-for="line in productionlines">
              <template v-if="line.name === machineComponent.worksite">
                <option v-bind:key="line.id" v-bind:value="line.productionline_name">
                  {{line.productionline_name}}
                </option>
              </template>
            </template>
          </select>
        </div>

      <div class="form-group">
        <label for="li">{{$t("affiliatedMachine")}}</label>
        <select name="m" id="li" class="form-select" v-model="machineComponent.machineName" required>
          <template v-for="machine in machinesAvailable">
            <template v-if="machine.worksite === machineComponent.worksite && machine.productionline_name === machineComponent.productionLine">
              <option :key="machine.id" v-bind:value="machine.name">
                {{$t(machine.name)}}
              </option>
            </template>
          </template>
          <option value="other">
            {{$t("other")}}
          </option>
        </select>
      </div>

      <div class="form-group">
        <label for="m">{{$t("otherMachine")}}</label>
        <select name="line" id="m" class="form-select" v-model="machineComponent.other_machine" required>
          <option value="0">
            0
          </option>
          <option value="1">
            1
          </option>
        </select>
      </div>

        <button type="button" class="btn btn-primary" v-on:click="addMachine()">{{$t('add')}}</button>
    </form>

    <br/>
    <br/>


    <template v-if="machineComponents.length > 0">

      <table class="table">
        <thead class="thead-dark">
        <tr>
          <th scope="col">{{ $t('name') }}</th>
          <th scope="col">{{ $t('machineName') }}</th>
          <th scope="col">{{ $t('productionLine') }}</th>
          <th scope="col">{{ $t('otherMachine') }}</th>
          <th scope="col">{{ $t('worksite') }}</th>
          <th scope="col">{{ $t('action') }}</th>

        </tr>
        </thead>
        <tbody>
            <tr v-for="d in machineComponents" :key="d.id">
                <th scope="row">{{d.name}}</th>
                <td>{{d.machineName}}</td>
                <td>{{d.productionLine}}</td>
                <td>{{d.other_machine}}</td>
                <td>{{d.worksite}}</td>
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
  name: "Add_MachineComponent",

  data() {
    return {
      username: localStorage.getItem("username"),
      data : null,
      productionlines : null,
      userWorksite : null,
      effective : null,
      machineComponents : [],
      worksites : null,
      machinesAvailable : null,
      machineComponent: {
        name : null,
        machineName : 'filler',
        worksite : null,
        other_machine : 0,
        productionLine : null,
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
          var machineComponent2 = {
            name: '',
            machineName: '',
            worksite: '',
            other_machine: 0,
            productionLine: '',
          };
          var effective;
          for (i = 1; i < rows.length - 1; i++) {
            rowsSplited = rows[i].split('\r')[0].split(',');
            if (rowsSplited.length === 5) {
              machineComponent2.name = rowsSplited[0];
              machineComponent2.machineName = rowsSplited[1];
              machineComponent2.other_machine = rowsSplited[2];
              machineComponent2.worksite = rowsSplited[3];
              machineComponent2.productionLine = rowsSplited[4];
              console.log(machineComponent2);

              await axios.post(urlAPI + 'insertMachineComponent', machineComponent2)
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

    deleteItem : async function (id) {

      await axios.delete(urlAPI + 'deleteMachineComponent/' + id);
      location.reload();
    },

    addMachine : async function () {
      var form = document.getElementById('needs-validation');
      if (form.checkValidity() === false) {
        event.preventDefault();
        event.stopPropagation();
        console.log("PAS OK");

      } else {

        console.log("OK");
        console.log(this.machineComponent);


        await axios.post(urlAPI + 'insertMachineComponent', this.machineComponent)
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
    await axios.get(urlAPI+'administratorMachine/' + this.userWorksite)
        .then(response => (this.machinesAvailable = response.data))

    console.log(this.machinesAvailable)
    await axios.get(urlAPI+'machine_component/' + this.userWorksite)
          .then(response => (this.machineComponents = response.data))

    console.log(this.machineComponents);


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