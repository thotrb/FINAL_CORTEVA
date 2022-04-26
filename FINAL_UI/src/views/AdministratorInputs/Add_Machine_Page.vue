<template>
  <div>
    <Navbar></Navbar>
    <div class="container">
      <button class="btn btn-primary d-flex align-items-center btn-danger" type="button"
              @click.prevent="resetPage()">
        {{$t("back")}}</button>
      <AddMachine></AddMachine>
    </div>
  </div>
</template>

<script>
import AddMachine from "@/components/AdministratorComponents/Add_Machine";
import Navbar from "@/components/AdministratorComponents/NavbarAdmin";
import router from "@/router";

export default {
  name: "MainPage.vue",
  components: {Navbar, AddMachine},
  methods : {
    resetPage : function () {
      router.replace("/mainPageAdministrator");

    },
  },
}
</script>

<style scoped>

</style>!