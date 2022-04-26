<template>
  <div align="center">
    <div align="center" class="col productionName rcorners2">
      {{$t("addMachine")}}
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
          <label for="l">{{$t("productionLine")}}</label>
        <select name="line" id="l" class="form-select" v-model="machine.productionline_name">
            <option  v-for="line in productionlines" :key="line.id" v-bind:value="line.productionline_name">
              {{line.productionline_name}}
            </option>
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

    addMachine : function () {
      var form = document.getElementById('needs-validation');
          if (form.checkValidity() === false) {
            event.preventDefault();
            event.stopPropagation();
            console.log("PAS OK");

          }else {
            this.machine.worksite = this.userWorksite;
            console.log("OK");
            console.log(this.machine);

            axios.put(urlAPI + 'insertMachine', this.machine)
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