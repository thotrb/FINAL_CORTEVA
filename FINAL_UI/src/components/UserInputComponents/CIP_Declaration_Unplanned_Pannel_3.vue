<template>
  <div>
    <div align="center" class="productionName rcorners2" >
      {{title}}

    </div>

    <br/>

    <form id="form">

      <div class="form-group row">
        <label class="col-sm-2 col-form-label rcorners1" for="previousBulk">{{$t("previousBulk")}}</label>
        <div class="col-sm-10">
          <input type="text" id="previousBulk" class="form-control-plaintext rcorners2" v-model="CIP_Event.previous_bulk" readonly>
        </div>
      </div>


      <div class="form-group row cip-hidden" >
        <label class="col-sm-2 col-form-label rcorners1" for="totalDuration">{{$t("totalDuration(Minutes)")}}</label>
        <div class="col-sm-10">
          <input type="number" id="totalDuration" class="form-control-plaintext rcorners2"  v-model="CIP_Event.total_duration">
        </div>
      </div>

      <div class="form-group row">
        <label class="col-sm-2 col-form-label rcorners1" for="completed">{{$t("completed")}}</label>
        <input type="checkbox" id="completed" style="margin-top: 20px; width: 50px;" checked v-model="CIP_Event.finished">
      </div>

      <div class="form-group row cip-hidden">
        <label for="comments" class="col-sm-2 rcorners1">{{$t("comments")}}</label>
        <div class="col-sm-10">
          <textarea id="comments" class="form-control-plaintext rcorners2" v-model="CIP_Event.comment"></textarea>
        </div>
      </div>


      <div align="right">
        <button class="btn btn-primary border-danger align-items-right btn-danger" type="button"
                @click.prevent="backOrigin()">
          {{$t("cancel")}}
        </button>
      </div>
      <div class="d-flex flex-row justify-content-between align-items-center bg-white">
        <button class="btn btn-primary d-flex align-items-center btn-danger" type="button"
                @click.prevent="backPage()">
          {{$t("back")}}
        </button>

        <button class="btn btn-primary border-success align-items-center btn-success" type="button"
                @click.prevent="validateInformations()">
          OK
        </button>
      </div>
    </form>


  </div>

</template>

<script>
import router from "@/router";
import axios from "axios";
import { urlAPI} from "@/variables";


export default {
  name: "CIP_Declaration_Unplanned_Pannel_3",

  data() {
    return {
      url: sessionStorage.getItem("url"),
      productionName: sessionStorage.getItem("productionName"),
      downtimeType: sessionStorage.getItem("downtimeType"),
      indice: sessionStorage.getItem("indice"),

      parameters: [],
      printedStep: 0,
      title: 'CIP',

      CIP_Event: {

        OLE: '',

        productionline: sessionStorage.getItem("productionName"),

        previous_bulk: '',

        predicted_duration:  0,

        total_duration: 0,

        shift : '',

        //commentaire
        comment :'',
        finished: true,
      },


    }
  },

  methods: {

    validateInformations : function(){

      this.CIP_Event.OLE = sessionStorage.getItem("poNumber");
      this.CIP_Event.shift = sessionStorage.getItem("typeTeam");

      console.log(this.CIP_Event);
      
        if(this.CIP_Event.previous_bulk !== null && this.CIP_Event.total_duration !== null){
          axios.post(urlAPI + "unplannedEvent/CIP", this.CIP_Event);
          this.backOrigin();

        }else{
          this.errorMessage();
        }
  

    },

    errorMessage : function(){
      var h1 = document.getElementsByClassName("error");
      if(h1.length <= 0){
        let error = document.createElement('h1');
        error.setAttribute("class", "error");
        error.innerHTML = this.$t("errorInput");
        error.setAttribute("style", "color:red;")
        error.setAttribute("align", "center");
        let br = document.createElement('br');

        let form = document.getElementById("form");
        form.insertBefore(br, form.firstChild);
        form.insertBefore(error, form.firstChild);
      }

    },

    backOrigin : function(){


      router.replace('/homePage');


    },




    resolveAfter05Second: function () {
      return new Promise(resolve => {
        setTimeout(() => {
          resolve('resolved');
        }, 500);
      });
    },

    backPage: function () {


      router.replace('/eventDeclaration/'+this.productionName+'/unplannedDowntime');


    }


  },

  mounted() {
    if(sessionStorage.getItem("language") !== null){
      this.$i18n.locale = sessionStorage.getItem("language");
    }

    let PONumber = sessionStorage.getItem("poNumber");
    let productionLine = sessionStorage.getItem("productionName");
    let currentGMID = sessionStorage.getItem("GMID");
    let APIAddress = urlAPI + "previousBulk/" + productionLine + "/" + PONumber + "/" + currentGMID;
    axios.get(APIAddress).then(response => {
      if (response.data[0] && response.data[0].bulk) {
        this.CIP_Event.previous_bulk = response.data[0].bulk;
      }
    });
  },
}
</script>


<style scoped>

input#endTime:hover {
  cursor: not-allowed;
}
.productionName {
  left: 0;
  top: 0;
  min-width: 150px;
  max-width: 250px;

  margin-bottom: 50px;
}

.rcorners1 {
  border-radius: 25px;
  background: lightblue;
  padding: 20px;
  margin-bottom: 30px;
  width: 180px;

}


.rcorners2 {
  border-radius: 25px;
  border: 2px solid lightblue;
  padding: 20px;
}

.wrapper {
  display: grid;
  grid-template-columns: 1fr 1fr 1fr;
}


button {
  color: white;
  margin-top: 20px;


}

input {
  width: 40%;

}


#comments {
  height: 150px;
  width: 70%;
  border-radius: 25px;
  border: 2px solid lightblue;
  padding: 20px;

}

h1 {
  color:red;
}

</style>
