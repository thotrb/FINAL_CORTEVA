<template>


  <div class="container" align="center">
    <div align="center" class="col productionName rcorners2">
      <h1 style="color: steelblue">Calista</h1>
    </div>
    <br>
    <div class="row">
      <div class="col">
          <button class="btn btn-primary" v-on:click="loginChoice('operator')">{{ $t("choiceLoginOperator") }}</button>
      </div>

      <div class="col">
          <button  class="btn btn-primary" v-on:click="loginChoice('supervisor')">{{ $t("choiceLoginSupervisor") }}
          </button>
      </div>

      <div class="col">
        <button  class="btn btn-primary" v-on:click="loginChoice('administrator')">{{ $t("choiceLoginAdministrator") }}
        </button>
      </div>


    </div>
  </div>

</template>

<script>
import router from "@/router";
export default {
  name: "ChoiceLogin",
  data() {
    return {
      choiceLoginOperator: this.$t("choiceLoginOperator"),
      choiceLoginSupervisor : this.$t("choiceLoginSupervisor"),
      choiceLoginAdministrator : this.$t("choiceLoginAdministrator"),
    }
  },
  methods: {

    loginChoice: function(choice) {
      sessionStorage.setItem("loginChoice", choice);
      router.replace('/login');
    },

    },

}
</script>

<style scoped>


.productionName {
  min-width: 150px;
  max-width: 250px;
  margin-bottom: 50px;
}


.rcorners2 {
  border-radius: 25px;
  border: 2px solid lightblue;
  padding: 30px;
}
</style>