<template>
  <div class="home">
    <navbar-saisie></navbar-saisie>

    <template v-if="userWorksite !== null">

      <form id="needs-validation" novalidate>
        <div class="form-group row">
          <label class="col-sm-2 col-form-label rcorners1" for="site">{{$t("site")}}</label>
          <div class="col-sm-10">
            <input type="text" id="site" readonly class="form-control-plaintext rcorners2"
                   v-bind:value="userWorksite.worksite_name">
          </div>
        </div>

        <!--
        <div class="form-group row">
          <label for="crewLeader" class="col-sm-2 rcorners1">{{$t("crewLeader")}}</label>
          <select name="crewL" id="crewLeader" class="rcorners2">
                <option v-for="leader in crewLeaders" :key="leader.id" v-bind:value="leader.firstname+' '+leader.lastname">
                  {{leader.firstname}} {{leader.lastname}}
                </option>
          </select>
        </div>
        -->

        <div class="form-group row">

          <label for="typeTeam" class="col-sm-2 rcorners1">{{$t("team")}}</label>
          <select name="Leader" id="typeTeam" class="rcorners2" v-model="selected">
              <option v-for="shift in shiftCrew" :key="shift.shift" v-bind:value="shift.type">
                {{shift.type}}
              </option>
          </select>
        </div>

        <div class="form-group row">

          <label for="typeTeam" class="col-sm-2 rcorners1">{{$t("date")}}</label>
          <div class="col-sm-10">
            <input type="date" id="date"
                   class="form-control-plaintext rcorners2"
                   v-model="datePO" required>
          </div>
        </div>


        <div class="form-group row">
          <label for="workingDebut" class="col-sm-2 rcorners1">{{$t("startTime")}}</label>
          <template  v-for="shift in shiftCrew" >
            <div class="col-sm-10" v-bind:key="shift.type">
                <input v-if="shift.type===selected" type="text" id="workingDebut" readonly
                       class="form-control-plaintext rcorners2"
                       v-bind:value="shift.workingDebut">
            </div>
          </template>

        </div>

        <div class="form-group row">
          <label for="workingEnd" class="col-sm-2 rcorners1">{{$t("endTime")}}</label>
            <div class="col-sm-10" v-for="shift in shiftCrew"  v-bind:key="shift.type">
              <template v-if="shift.type===selected">
                <input type="text" id="workingEnd" readonly
                       class="form-control-plaintext rcorners2"
                       v-bind:value="shift.workingEnd">
              </template>
            </div>
        </div>

            <div v-for="productionLine in productionLParam" :key="productionLine.productionline_name" class="row production">
              <div class="col">
                <p align="center" class="form-control-plaintext rcorners1">
                  {{$t("line")}} {{productionLine.productionline_name}}
                </p>
              </div>

              <div class="col">
                <input type="text" class="form-control-plaintext rcorners2 D-Code" name="D-Code/GMID"
                       placeholder="D-Code/GMID" required>
              </div>

              <div class="col">
                <input type="text" class="form-control-plaintext rcorners2 PO" name="PO"
                       placeholder="PO" required>
              </div>
            </div>

            <br/>

            <!--</template>-->


        <div align="right">

          <button class="btn btn-primary border-success align-items-center btn-success" type="button"
                  v-on:click="nextPage()">
            OK
          </button>

        </div>
      </form>

    </template>

  </div>

</template>

<script>
import NavbarSaisie from "@/components/UserInputComponents/NavbarSaisie";
import router from "@/router";
import { urlAPI} from "@/variables";

import axios from 'axios';

export default {
  name: "TeamInfo",
  components: {
    NavbarSaisie,
  },
  data() {
    return {
      username: localStorage.getItem("username"),
      parameters: [],
      selected: '',

      userWorksite : null,
      POCheck : null,
      crewLeaders : null,
      shiftCrew: null,
      productionLParam: null,
      effective: null,
      user: null,
      data: null,
      currentYear : new Date().getFullYear(),
      currentMonth : new Date().getMonth(),
      currentDay : new Date().getDay(),
      datePO: new Date().toISOString().substr(0, 10),

    };
  },

  async mounted() {

    axios.get(urlAPI+'user/'+this.username)
        .then(response => (this.data = response.data))

    await this.resolveAfter15Second();

    this.userWorksite  = this.data[0][0];
    this.crewLeaders =  this.data[1];
    this.shiftCrew = this.data[2];
    this.productionLParam = this.data[3];

    console.log(this.userWorksite);
    console.log(this.shiftCrew);
    console.log(this.productionLParam);

    if(sessionStorage.getItem("eventsPerProductionline") !== null){
      sessionStorage.eventsPerProductionline =  null;
    }else{
      sessionStorage.setItem("eventsPerProductionline", null);
    }

  },


  methods: {

    resolveAfter15Second: function () {
      return new Promise(resolve => {
        setTimeout(() => {
          resolve('resolved');
        }, 1500);
      });

    },

    resolveAfter05Second: function () {
      return new Promise(resolve => {
        setTimeout(() => {
          resolve('resolved');
        }, 500);
      });


    },

    backLogin: function () {
      window.location.href = this.url + 'menu';
    },

    nextPage: async function () {

      var form = document.getElementById('needs-validation');
      if (form.checkValidity() === false) {
        event.preventDefault();
        event.stopPropagation();
        console.log("PAS OK");

      }else{

        //this.addSessionValue("productionA", this.productionA);
        var DCODES = document.getElementsByClassName('D-Code');
        var dcodesTab = [];

        for (let i = 0; i < DCODES.length; i++) {
          dcodesTab.push(''+DCODES[i].value);
        }

        if (sessionStorage.getItem("GMID") === null) {
          sessionStorage.GMID = dcodesTab;
        } else {
          sessionStorage.setItem("GMID", dcodesTab);
        }

         if (sessionStorage.getItem("dateInput") === null) {
          sessionStorage.dateInput = this.datePO;
        } else {
          sessionStorage.setItem("dateInput",  this.datePO);
        }

        var POs = document.getElementsByClassName('PO');
        var poTab = [];
        var productionlinesTab = [];
        var typeTeam = document.getElementById('typeTeam');
        var valueTypeTeam = typeTeam.options[typeTeam.selectedIndex].value;


        for(let i=0; i<this.productionLParam.length; i++){
          productionlinesTab.push(this.productionLParam[i].productionline_name);
        }

        /**
         for (let i = 0; i < this.user[3].length; i++) {
        if (this.user[3][i].worksiteID === this.user[0][0].worksiteID) {
          productionlinesTab.push(this.user[3][i].productionline_name);
        }
      }*/

            // eslint-disable-next-line no-unused-vars
        var productionLineUser =null;

        for (let i = 0; i < POs.length; i++) {
          poTab.push(POs[i].value);

          var number = POs[i].value;

          if(number !== "" && dcodesTab[i] !==  ""){

            //Teste le PO à partir du number
            await axios.get(urlAPI + 'PO/' +number+'/'+valueTypeTeam)
                .then(response => (this.POCheck = response.data))


            console.log(this.POCheck);

            if (this.POCheck === 0) {
              let element = {
                number: number,
                GMIDCode: dcodesTab[i],
                productionline_name: productionlinesTab[i],
              };

              productionLineUser = productionlinesTab[i];



              console.log(element);
              console.log(this.datePO);



              axios.put(urlAPI + 'PO/insertPO/PO', {
                number: number,
                GMIDCode: dcodesTab[i],
                productionline_name: this.userWorksite.productionline,
                shift: valueTypeTeam,
                created_at : this.datePO
              })
                  .then(response => (this.effective = response))


              console.log('Effectif : ' + this.effective);


            }

          }
        }



        if (sessionStorage.getItem("pos") === null) {
          sessionStorage.pos = poTab;
        } else {
          sessionStorage.setItem("pos", poTab);
        }

        if (sessionStorage.getItem("worksite") === null) {
          sessionStorage.worksite = this.userWorksite.worksite_name;
        } else {
          sessionStorage.setItem("worksite",  this.userWorksite.worksite_name);
        }

        if (sessionStorage.getItem("worksiteUserID") === null) {
          sessionStorage.worksiteUserID = this.userWorksite.id;
        } else {
          sessionStorage.setItem("worksiteUserID", this.userWorksite.id);
        }



        var nbProd = document.getElementsByClassName('production').length;


        if (sessionStorage.getItem("nbProductionlines") === null) {
          sessionStorage.nbProductionlines = nbProd;
        } else {
          sessionStorage.setItem("nbProductionlines", nbProd);
        }


        if (sessionStorage.getItem("prodlines") === null) {
          sessionStorage.prodlines = productionlinesTab;
        } else {
          sessionStorage.setItem("prodlines", productionlinesTab);
        }




        /**
        var selectCrewLeader = document.getElementById('crewLeader');
        var valueCrewLeader = selectCrewLeader.options[selectCrewLeader.selectedIndex].value;


        if (sessionStorage.getItem("crewLeader") === null) {
          sessionStorage.crewLeader = valueCrewLeader;
        } else {
          sessionStorage.setItem("crewLeader", valueCrewLeader);
        }

         **/


        if (sessionStorage.getItem("typeTeam") === null) {
          sessionStorage.typeTeam = valueTypeTeam;
        } else {
          sessionStorage.setItem("typeTeam", valueTypeTeam);
        }


        if (sessionStorage.getItem("workingEnd") === null) {
          sessionStorage.workingEnd = document.getElementById('workingEnd').value;
        } else {
          sessionStorage.setItem("workingEnd", document.getElementById('workingEnd').value);
        }

        if (sessionStorage.getItem("workingDebut") === null) {
          sessionStorage.workingDebut = document.getElementById('workingDebut').value;
        } else {
          sessionStorage.setItem("workingDebut", document.getElementById('workingDebut').value);
        }

        if (sessionStorage.getItem("site") === null) {
          sessionStorage.site = document.getElementById('site').value;
        } else {
          sessionStorage.setItem("site", document.getElementById('site').value);
        }

        router.replace('/homePage')





      }




    }

  },
}
</script>

<style scoped>
form {
  padding: 2%;
}

.rcorners1 {
  border-radius: 25px;
  background: lightblue;
  padding: 20px;

}

.rcorners2 {
  border-radius: 25px;
  border: 2px solid lightblue;
  padding: 20px;
}

select option {
  padding-left: 20px;
}

.btn:focus {
  outline: 0 !important;
  box-shadow: none !important
}

.btn:active {
  outline: 0 !important;
  box-shadow: none !important
}

</style>