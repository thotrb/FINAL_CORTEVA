<template>
  <div align="center">
    <div align="center" class="col productionName rcorners2">
      {{$t("addProduct")}}
    </div>

    <br/>
    <br/>


    <div>
      <label for="csv">{{$t('select.csvFileToUse')}}</label>
      <input type="file" id="csv" name="profile_pic"
             accept=".csv">
      <p id="fileDisplayArea"></p>
      <button type="button" class="btn btn-primary" v-on:click="readFile(function(GMID) {errorRequest = 1; GMIDProbleme = GMID;})">{{ $t('load') }}</button>

    </div>

    <br/>
    <br/>

    <template v-if="errorRequest!==0">
      <div align="center" class="col productionName rcorners3">
        {{$t("anErrorHasOccured")}} <br>
        {{$t("GMIDMustBeUnique")}} : {{GMIDProbleme}}
      </div>

      <br/>
      <br/>
    </template>


    <form id="needs-validation" novalidate>
      <div class="row">
        <div class="col">
          <label for="a">{{$t('product')}}</label>
          <input type="text" class="form-control" id="a" v-model="product.product"   required>
        </div>
        <div class="col">
          <label for="b">GMID</label>
          <input type="text" class="form-control" id="b" v-model="product.GMID"   required>
        </div>
      </div>

      <div class="row">
        <div class="col">
          <label for="c">{{$t('bulk')}}</label>
          <input type="text" class="form-control" id="c" v-model="product.bulk"   required>
        </div>
        <div class="col">
          <label for="d">{{$t('family')}}</label>
          <input type="text" class="form-control" id="d" v-model="product.family"   required>
        </div>
      </div>

      <div class="row">
        <div class="col">
          <label for="e">GIFAP</label>
          <input type="text" class="form-control" id="e" v-model="product.GIFAP"   required>
        </div>
        <div class="col">
          <label for="f">{{$t('description')}}</label>
          <input type="text" class="form-control" id="f" v-model="product.description"   required>
        </div>
      </div>

      <br/>
      <div class="form-group">
        <label for="g">{{$t("formulationType")}}</label>
        <select name="line" id="g" class="form-select" v-model="product.formulationType" required>
          <option value="Solid">{{ $t('solid') }}</option>
          <option value="Liquid">{{ $t('liquid') }}</option>
          <option value="Sparkling">{{ $t('sparkling') }}</option>
        </select>
      </div>

      <div class="row">
        <div class="col">
          <label for="h">{{$t('size')}}</label>
          <input type="number" step="0.01" class="form-control" id="h" v-model="product.size"   required>
        </div>
        <div class="col">
          <label for="i">{{$t('idealRate')}}</label>
          <input type="number" step="0.01" class="form-control" id="i" v-model="product.idealRate"   required>
        </div>
      </div>
      <br/>
      <div class="form-group">
        <label for="j">{{$t('bottlesPerCase')}}</label>
        <input type="number" class="form-control" id="j" v-model="product.bottlesPerCase"  required>
      </div>
      <button type="button" class="btn btn-primary" v-on:click="addMachine()">{{$t('add')}}</button>
    </form>

    <br/>
    <br/>


    <template v-if="products.length > 0">

      <table class="table">
        <thead class="thead-dark">
        <tr>
          <th scope="col">{{ $t('product') }}</th>
          <th scope="col">GMID</th>
          <th scope="col">{{ $t('bulk') }}</th>
          <th scope="col">{{ $t('family') }}</th>
          <th scope="col">GIFAP</th>
          <th scope="col">{{ $t('description') }}</th>
          <th scope="col">{{ $t('formulationType') }}</th>
          <th scope="col">{{ $t('size') }}</th>
          <th scope="col">{{ $t('idealRate') }}</th>
          <th scope="col">{{ $t('bottlesPerCase') }}</th>
          <th scope="col">{{ $t('action') }}</th>

        </tr>
        </thead>
        <tbody>
            <tr v-for="d in products" :key="d.id">
              <th scope="row">{{d.product}}</th>
              <td>{{d.GMID}}</td>
              <td>{{d.bulk}}</td>
              <td>{{d.family}}</td>
              <td>{{d.GIFAP}}</td>
              <td>{{d.description}}</td>
              <td>{{d.formulationType}}</td>
              <td>{{d.size}}</td>
              <td>{{d.idealRate}}</td>
              <td>{{d.bottlesPerCase}}</td>
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
  name: "Add_Product",

  data() {
    return {
      username: localStorage.getItem("username"),
      data : null,
      productionlines : null,
      userWorksite : null,
      effective : null,
      products : [],
      GMIDProbleme : '',
      errorRequest : 0,
      product: {
        product : null,
        GMID : null,
        bulk : null,
        family : null,
        GIFAP : null,
        description : null,
        formulationType : null,
        size : null,
        idealRate : 0.00,
        bottlesPerCase : null,
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

    readFile : function (callback) {
      var textType = /.csv/;
      var doc = document.getElementById("csv").files[0];

      if (doc.type.match(textType)) {

        var reader = new FileReader();
        reader.readAsText(doc);
        reader.onload = async function (e) {
          var rows = e.target.result.split('\n');
          var rowsSplited = null;

          var i = 1;
          var product2 = {
            product: null,
            GMID: null,
            bulk: null,
            family: null,
            GIFAP: null,
            description: null,
            formulationType: null,
            size: null,
            idealRate: null,
            bottlesPerCase: null,
          };

          this.errorRequest = 0;
          console.log(rows);

          while (i < rows.length - 1 && this.errorRequest === 0) {
            rowsSplited = rows[i].split('\r')[0].split(',');
            console.log(rowsSplited);

            if (rowsSplited.length === 10) {
              product2.product = rowsSplited[0];
              product2.GMID = rowsSplited[1];
              product2.family = rowsSplited[2];
              product2.GIFAP = rowsSplited[3];
              product2.description = rowsSplited[4];
              product2.formulationType = rowsSplited[5];
              product2.size = rowsSplited[6];
              product2.idealRate = rowsSplited[7];
              product2.bottlesPerCase = rowsSplited[8];
              product2.bulk = rowsSplited[9];
              console.log(product2);

              await axios.put(urlAPI + 'insertProduct', product2)
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
                    this.GMIDProbleme = product2.GMID;
                    callback(product2.GMID);
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


    deleteItem : async function (id) {

      await axios.delete(urlAPI + 'deleteProduct/' + id);
      location.reload();
    },


    addMachine : async function () {
      var form = document.getElementById('needs-validation');
      if (form.checkValidity() === false) {
        event.preventDefault();
        event.stopPropagation();
        console.log("PAS OK");

      } else {




        await axios.put(urlAPI + 'insertProduct', this.product)
            .then(response => {this.effective = response; console.log(response)
              if(response.status === 500){
                console.log("Je passe");
              }else {
                this.effective = response;
                location.reload();
              }
            })
            .catch(error => {console.log("je passe"); console.log(error); this.errorRequest=1;this.GMIDProbleme = this.product.GMID})

      }


      form.classList.add('was-validated');
    },


  },
  async mounted() {
    await axios.get(urlAPI+'user/'+this.username)
        .then(response => (this.data = response.data))

    await this.resolveAfter15Second();

    this.userWorksite  = this.data[0][0].worksite_name;


    axios.get(urlAPI+'getProductionLines/' + this.userWorksite)
        .then(response => (this.productionlines = response.data))


    axios.get(urlAPI+'AdministratorProducts')
          .then(response => (this.products = response.data))


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