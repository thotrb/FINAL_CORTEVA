<template>
  <div>
    <Navbar></Navbar>
    <br/>

    <div class="container">
      <button class="btn btn-primary d-flex align-items-center btn-danger" type="button"
              @click.prevent="resetPage()">
        {{$t("back")}}</button>
      <AddDowntime></AddDowntime>
    </div>
  </div>
</template>

<script>
import AddDowntime from "@/components/AdministratorComponents/Add_DowntimeReason";
import Navbar from "@/components/AdministratorComponents/NavbarAdmin";
import router from "@/router";

export default {
  name: "MainPage.vue",
  components: {Navbar, AddDowntime},
  methods : {
    resetPage : function () {
      router.replace("/mainPageAdministrator");

    },
  },
}
</script>

<style scoped>

</style>