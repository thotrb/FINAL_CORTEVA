<template>
  <div>
    <Navbar></Navbar>
    <br/>
    <div class="container">
      <button class="btn btn-primary d-flex align-items-center btn-danger" type="button"
              @click.prevent="resetPage()">
        {{$t("back")}}</button>
      <AddComponent></AddComponent>
    </div>
  </div>
</template>

<script>
import AddComponent from "@/components/AdministratorComponents/Add_MachineComponent";
import Navbar from "@/components/AdministratorComponents/NavbarAdmin";
import router from "@/router";

export default {
  name: "MainPage.vue",
  components: {Navbar, AddComponent},
  methods : {
    resetPage : function () {
      router.replace("/mainPageAdministrator");

    },
  },
}
</script>

<style scoped>

</style>