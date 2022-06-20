<template>
  <div>
    <Navbar></Navbar>
    <br/>
    <div class="container">
      <button class="btn btn-primary d-flex align-items-center btn-danger" type="button"
              @click.prevent="resetPage()">
        {{$t("back")}}</button>
      <AddWorksite></AddWorksite>
    </div>
  </div>
</template>

<script>
import AddWorksite from "@/components/AdministratorComponents/Add_Worksite";
import Navbar from "@/components/AdministratorComponents/NavbarAdmin";
import router from "@/router";

export default {
  name: "ProductionlinePage.vue",
  components: {Navbar, AddWorksite},
  methods : {
    resetPage : function () {
      router.replace("/mainPageAdministrator");

    },
  },
}
</script>

<style scoped>

</style>