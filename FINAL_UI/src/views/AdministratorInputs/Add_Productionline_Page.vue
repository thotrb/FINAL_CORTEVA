<template>
  <div>
    <Navbar></Navbar>
    <div class="container">
      <button class="btn btn-primary d-flex align-items-center btn-danger" type="button"
              @click.prevent="resetPage()">
        {{$t("back")}}</button>
      <AddProductionline></AddProductionline>
    </div>
  </div>
</template>

<script>
import AddProductionline from "@/components/AdministratorComponents/Add_Productionline";
import Navbar from "@/components/AdministratorComponents/NavbarAdmin";
import router from "@/router";

export default {
  name: "ProductionlinePage.vue",
  components: {Navbar, AddProductionline},
  methods : {
    resetPage : function () {
      router.replace("/mainPageAdministrator");

    },
  },
}
</script>

<style scoped>

</style>