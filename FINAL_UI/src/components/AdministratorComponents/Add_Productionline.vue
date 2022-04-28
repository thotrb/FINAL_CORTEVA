<template>
  <div align="center">
    <div align="center" class="col productionName rcorners2">
      {{$t("addProductionline")}}
    </div>

    <br/>
    <br/>

    <div>
      <label for="csv">{{$t('select.csvFileToUse')}}</label>
      <input type="file" id="csv" name="profile_pic"
             accept=".csv">
      <p id="fileDisplayArea"></p>
      <button type="button" class="btn btn-primary" v-on:click="readFile()">{{ $t('load') }}</button>

    </div>

    <br/>
    <br/>


    <form id="needs-validation" novalidate>

      <div class="form-group">
        <label for="w">{{$t("worksite")}}</label>
        <select name="m" id="w" class="form-select" v-model="productionLine.worksite_name">
          <option  v-for="w in worksites" :key="w.id" v-bind:value="w.name">
            {{w.name}}
          </option>
        </select>
      </div>

      <div class="form-group">
        <label for="inputEmail4">{{$t('productionLine')}}</label>
        <input type="text" class="form-control" id="inputEmail4" v-model="productionLine.productionline_name"  required>
      </div>


      <button type="button" class="btn btn-primary" v-on:click="addMachine()">{{$t('add')}}</button>
    </form>

    <br/>
    <br/>


    <template v-if="productionlines.length > 0">

      <table class="table">
        <thead class="thead-dark">
        <tr>
          <th scope="col">{{ $t('worksite') }}</th>
          <th scope="col">{{ $t('productionLine') }}</th>
        </tr>
        </thead>
        <tbody>
            <tr v-for="d in productionlines" :key="d.id">
              <th scope="row">{{d.name}}</th>
              <td>{{d.productionline_name}}</td>
            </tr>
        </tbody>
      </table>
    </template>


  </div>
</template>

<script>



import axios from "axios";
import {urlAPI} from "@/variables";

export default {
  name: "Add_Productionline",

  data() {
    return {
      data : null,
      productionlines : [],
      userWorksite : null,
      effective : null,
      worksites : null,
      productionLine: {
        productionline_name : null,
        worksite_name : null
      },
    }
  },
  methods : {
    resolveAfter15Second: function () {
      return new Promise(resolve => {
        setTimeout(() => {
          resolve('resolved');
        }, 1500);
      });
    },

    readFile : function () {
      var textType = /.csv/;
      var doc = document.getElementById("csv").files[0];

      if (doc.type.match(textType)) {

        //console.log(doc);
        var reader = new FileReader();
        reader.readAsText(doc);
        reader.onload = function (e) {
          var rows = e.target.result.split('\n');
          var rowsSplited = null;

          var i;
          var productionLine2 = {
            productionline_name: '',
            worksite_name: '',
          };
          var effective;
          for (i = 1; i < rows.length - 1; i++) {
            rowsSplited = rows[i].split('\r')[0].split(',');
            if(rowsSplited.length === 2){
              productionLine2.productionline_name = rowsSplited[0];
              productionLine2.worksite_name = rowsSplited[1];

              console.log(productionLine2);

              axios.put(urlAPI + 'insertProductionline', productionLine2)
                  .then(response => (effective = response))
              console.log(effective)
            }
          }
          location.reload();

        }
      }else{
        var fileDisplayArea = document.getElementById('fileDisplayArea');
        fileDisplayArea.innerText = this.$t('fileNotSupported');


      }

    },

    addMachine : function () {
      var form = document.getElementById('needs-validation');
          if (form.checkValidity() === false) {
            event.preventDefault();
            event.stopPropagation();
            console.log("PAS OK");

          }else {
            console.log("OK");
            console.log(this.productionLine);

            axios.put(urlAPI + 'insertProductionline', this.productionLine)
                .then(response => (this.effective = response))

            console.log('Effectif : ' + this.effective);
            location.reload();

          }



          form.classList.add('was-validated');
    },


  },
  async mounted() {


    await axios.get(urlAPI+'sites')
        .then(response => (this.productionlines = response.data))
    this.worksites = this.productionlines[0];
    this.productionlines = this.productionlines[1];
    console.log(this.productionlines);




  }
}
</script>

<style scoped>

.rcorners2 {
  margin-top : 20px;
  border-radius: 25px;
  border: 2px solid lightblue;
  padding: 20px;
}

</style>