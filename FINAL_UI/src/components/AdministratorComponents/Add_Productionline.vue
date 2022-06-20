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
      <button type="button" class="btn btn-primary" v-on:click="readFile(function(pline) {errorRequest = 1; pLineProbleme = pline;})">{{ $t('load') }}</button>
    </div>

    <br/>
    <br/>

    <template v-if="errorRequest!==0">
      <div align="center" class="col productionName rcorners3">
        {{$t("anErrorHasOccured")}} <br>
        {{$t("productionLineNameMustBeUnique")}} : {{pLineProbleme}}
      </div>

      <br/>
      <br/>
    </template>


    <form id="needs-validation" novalidate>

      <div class="form-group">
        <label for="w">{{$t("worksite")}}</label>
        <select name="m" id="w" class="form-select" v-model="productionLine.worksite_name" required>
          <option  v-for="w in worksites" :key="w.id" v-bind:value="w.name">
            {{w.name}}
          </option>
        </select>
      </div>

      <div class="form-group">
        <label for="inputEmail4">{{$t('productionLine')}}</label>
        <input type="text" class="form-control" id="inputEmail4" v-model="productionLine.productionline_name"  required>
      </div>



      <!--

      <div class="form-group">
        <label for="upload-pic" style="cursor:pointer; height: 200px; weight:200px; margin-bottom: 50px">Select an image</label>
        <input class="upload-pic" type="file" name="photo" id="upload-pic" accept="image/" ref="fileInput" @change="uploadImage($event)" required/>
      </div>

      <div class="">
        <div id="imagePreview" :style="{'background-image' : `url(${url})`}"></div>
      </div>

      -->

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
          <th scope="col">{{ $t('action') }}</th>

        </tr>
        </thead>
        <tbody>
            <tr v-for="d in productionlines" :key="d.id">
              <th scope="row">{{d.name}}</th>
              <td>{{d.productionline_name}}</td>
              <td><button type="button" class="btn btn-danger" @click="deleteItem(d.id)">{{$t('delete')}}</button></td>

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

      message: "",

      errorRequest : 0,
      pLineProbleme : '',

      url : "@/assets/logo.png",

      productionLine: {
        productionline_name : null,
        worksite_name : null
      },
    }
  },
  methods : {


    onFileChange : function(e) {

      console.log(e);
      const file = e.target.files[0];
      this.url = URL.createObjectURL(file);
      var imagefile = document.querySelector("#imageUpload");
      console.log(file);
      console.log(imagefile.files[0]);
      let formData = new FormData();
      formData.append("image", file);
      this.uploadImage(formData);
    },

    uploadImage : async function (e) {
      this.file = e.target.files[0];
      this.fileSrc = window.URL.createObjectURL(this.file);
    },


    resolveAfter15Second: function () {
      return new Promise(resolve => {
        setTimeout(() => {
          resolve('resolved');
        }, 1500);
      });
    },

    deleteItem : async function (id) {

      await axios.delete(urlAPI + 'deleteProductionLine/' + id);
      location.reload();
    },


    readFile : function (callback) {
      var textType = /.csv/;
      var doc = document.getElementById("csv").files[0];

      if (doc.type.match(textType)) {

        //console.log(doc);
        var reader = new FileReader();
        reader.readAsText(doc);
        reader.onload = async function (e) {
          var rows = e.target.result.split('\n');
          var rowsSplited = null;

          var i = 1;
          var productionLine2 = {
            productionline_name: '',
            worksite_name: '',
          };

          this.errorRequest = 0;
          while (i < rows.length - 1 && this.errorRequest === 0) {

            rowsSplited = rows[i].split('\r')[0].split(',');
            if (rowsSplited.length === 2) {
              productionLine2.productionline_name = rowsSplited[0];
              productionLine2.worksite_name = rowsSplited[1];

              console.log(productionLine2);

              await axios.put(urlAPI + 'insertProductionline', productionLine2)
                  .then(response => {
                    this.effective = response;
                    console.log(response)
                    if (response.status === 500) {
                      console.log("Je passe");
                    } else {
                      this.effective = response;
                    }
                  })
                  .catch(error => {
                    console.log("catch error");
                    console.log(error);
                    this.errorRequest = 1;
                    this.pLineProbleme = productionLine2.productionline_name;
                    callback(productionLine2.productionline_name);
                  })
            }
            i++;
          }
          if(this.errorRequest === 0){
            location.reload();
          }
        }
      }else{
        var fileDisplayArea = document.getElementById('fileDisplayArea');
        fileDisplayArea.innerText = this.$t('fileNotSupported');


      }

    },


    addMachine : async function () {
      var form = document.getElementById('needs-validation');
      if (form.checkValidity() === false) {
        event.preventDefault();
        event.stopPropagation();
        console.log("PAS OK");

      } else {
        console.log("OK");
        console.log(this.productionLine);

        await axios.put(urlAPI + 'insertProductionline', this.productionLine)
            .then(response => {this.effective = response; console.log(response)
              if(response.status === 500){
                console.log("Je passe");
              }else {
                this.effective = response;
                location.reload();
              }
            })
            .catch(error => {console.log("je passe"); console.log(error); this.errorRequest=1;this.pLineProbleme = this.productionLine.productionline_name})

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
.rcorners3 {
  margin-top : 20px;
  border-radius: 25px;
  border: 4px solid red;
  padding: 20px;
  color : red;
  font-weight: bold;
}


</style>