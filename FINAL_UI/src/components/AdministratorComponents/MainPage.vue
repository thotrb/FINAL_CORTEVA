<template>
<div id="pa">

    <div class="col">
      <button
          class="btn btn-primary border-info btn-lg btn-block align-items-center btn-info"
          type="button" @click.prevent="goTo('machines')">
        {{$t("machines")}}
      </button>

      <button
          class="btn btn-primary border-info btn-lg btn-block align-items-center btn-info"
          type="button" @click.prevent="goTo('machinesComponent')">
        {{$t("machinesComponent")}}
      </button>

      <button
          class="btn btn-primary border-info btn-lg btn-block align-items-center btn-info"
          type="button" @click.prevent="goTo('downtimeReason')">
        {{$t("downtimeReason")}}
      </button>

      <button
          class="btn btn-primary border-info btn-lg btn-block align-items-center btn-info"
          type="button" @click.prevent="goTo('formats')">
        {{$t("formats")}}
      </button>

      <button
          class="btn btn-primary border-info btn-lg btn-block align-items-center btn-info"
          type="button" @click.prevent="goTo('products')">
        {{$t("products")}}
      </button>

    </div>
  </div>
</template>

<script>
import router from "@/router";

export default {
  name: "MainPage.vue",
  methods : {
    goTo: function (choice) {
      sessionStorage.setItem("addingChoice", choice);
      router.replace('/addData/'+choice);
    },
  },
}
</script>

<style scoped>


#pa {
  margin: auto;
  width: 50%;
  padding: 10px;}
</style>