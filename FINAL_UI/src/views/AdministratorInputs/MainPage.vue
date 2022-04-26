<template>
<div class="home">
  <Navbar></Navbar>
  <br/>
  <br/>
  <div class="menu">
    <main-page></main-page>
  </div>
</div>
</template>

<script>
import MainPage from "@/components/AdministratorComponents/MainPage";
import Navbar from "@/components/AdministratorComponents/NavbarAdmin";

export default {
  name: "MainPage.vue",
  components: {Navbar, MainPage}
}
</script>

<style scoped>

.menu {

  text-align: center;}
</style>