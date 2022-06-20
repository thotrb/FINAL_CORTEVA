<template>
  <div>
    <Navbar></Navbar>
    <br/>

    <div class="container">
      <button class="btn btn-primary d-flex align-items-center btn-danger" type="button"
                                   @click.prevent="resetPage()">
      {{$t("back")}}</button>
      <AddFormat></AddFormat>
    </div>
  </div>
</template>

<script>
import AddFormat from "@/components/AdministratorComponents/Add_Format";
import Navbar from "@/components/AdministratorComponents/NavbarAdmin";
import router from "@/router";

export default {
  name: "MainPage.vue",
  components: {Navbar, AddFormat},
  methods : {
    resetPage : function () {
      router.replace("/mainPageAdministrator");

    },
  },
}
</script>

<style scoped>

</style>