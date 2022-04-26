<template>
  <div align="center">
    <div align="center" class="col productionName rcorners2">
      {{$t("addMachineComponent")}}
    </div>

    <br/>
    <br/>

    <form id="needs-validation" novalidate>
      <div class="form-group">
        <label for="inputEmail4">{{$t('name')}}</label>
        <input type="text" class="form-control" id="inputEmail4" v-model="machineComponent.name"   required>
      </div>
      <div class="form-group">
        <label for="l">{{$t("productionLine")}}</label>
        <select name="line" id="l" class="form-select" v-model="machineComponent.productionLine">
          <option  v-for="line in productionlines" :key="line.id" v-bind:value="line.productionline_name">
            {{line.productionline_name}}
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
          <th scope="col">{{ $t('worksite') }}</th>

        </tr>
        </thead>
        <tbody>
            <tr v-for="d in machineComponents" :key="d.id">
              <template v-if="d.other_machine===0">
                <th scope="row">{{d.name}}</th>
                <td>{{d.machineName}}</td>
                <td>{{d.productionLine}}</td>
                <td>{{d.worksite}}</td>
              </template>
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

    addMachine : function () {
      var form = document.getElementById('needs-validation');
          if (form.checkValidity() === false) {
            event.preventDefault();
            event.stopPropagation();
            console.log("PAS OK");

          }else {
            this.machineComponent.worksite = this.userWorksite;

            console.log("OK");
            console.log(this.machineComponent);


            axios.post(urlAPI + 'insertMachineComponent', this.machineComponent)
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


    axios.get(urlAPI+'machine_component/' + this.userWorksite)
          .then(response => (this.machineComponents = response.data))


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