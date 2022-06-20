<template>
  <div>
    <Navbar></Navbar>
    <br/>
    <div class="container">
      <button class="btn btn-primary d-flex align-items-center btn-danger" type="button"
              @click.prevent="resetPage()">
        {{$t("back")}}</button>
      <AddTeamInfo></AddTeamInfo>
    </div>
  </div>
</template>

<script>
import AddTeamInfo from "@/components/AdministratorComponents/Add_TeamInfo";
import Navbar from "@/components/AdministratorComponents/NavbarAdmin";
import router from "@/router";

export default {
  name: "ProductionlinePage.vue",
  components: {Navbar, AddTeamInfo},
  methods : {
    resetPage : function () {
      router.replace("/mainPageAdministrator");

    },
  },
}
</script>

<style scoped>

</style>