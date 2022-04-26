<template>
  <div class="container">
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

      <!--
      <div class="col">
        <router-link to="/login">
          <button class="btn btn-primary">{{ $t("choiceLoginAdministrator") }}
          </button>
        </router-link>
      </div>
      -->
    </div>
  </div>
</template>

<script>
import router from "@/router";
export default {
  name: "ChoiceLogin",
  methods: {

    loginChoice: function(choice) {
      sessionStorage.setItem("loginChoice", choice);
      router.replace('/login');
    },

    },

}
</script>

<style scoped>

</style>