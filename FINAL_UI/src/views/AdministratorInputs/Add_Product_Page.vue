<template>
  <div>
    <Navbar></Navbar>
    <br/>
    <div class="container">
      <button class="btn btn-primary d-flex align-items-center btn-danger" type="button"
              @click.prevent="resetPage()">
        {{$t("back")}}</button>
      <AddProduct></AddProduct>
    </div>
  </div>
</template>

<script>
import AddProduct from "@/components/AdministratorComponents/Add_Product";
import Navbar from "@/components/AdministratorComponents/NavbarAdmin";
import router from "@/router";

export default {
  name: "MainPage.vue",
  components: {Navbar, AddProduct},
  methods : {
    resetPage : function () {
      router.replace("/mainPageAdministrator");

    },
  },
}
</script>

<style scoped>

</style>