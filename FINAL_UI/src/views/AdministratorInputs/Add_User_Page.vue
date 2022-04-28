<template>
  <div>
    <Navbar></Navbar>
    <div class="container">
      <button class="btn btn-primary d-flex align-items-center btn-danger" type="button"
              @click.prevent="resetPage()">
        {{$t("back")}}</button>
      <AddUser></AddUser>
    </div>
  </div>
</template>

<script>
import AddUser from "@/components/AdministratorComponents/Add_User";
import Navbar from "@/components/AdministratorComponents/NavbarAdmin";
import router from "@/router";

export default {
  name: "ProductionlinePage.vue",
  components: {Navbar, AddUser},
  methods : {
    resetPage : function () {
      router.replace("/mainPageAdministrator");

    },
  },
}
</script>

<style scoped>

</style>