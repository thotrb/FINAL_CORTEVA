<template>
  <div class="d-flex main-container">

    <!-- Interval, site and production line selection menu -->
    <div class="d-flex selection-menu">
      <!-- Site and production line selection-->
      <div class="d-flex site-pl-selection">
        <div class="d-flex" style="flex-direction: column;">
        
          <label for="site-selection">Site: </label>
          <select id="site-selection" v-model="site">
            <option disabled selected value>-- Select --</option>
            <template v-for="site of dataWorksite">
              <option v-bind:key="site.name" v-bind:value="site.name">{{site.name}}</option>
            </template>
          </select>
     
          <label for="pl-selection">{{$t("productionLine")}}: </label>
          <select id="pl-selection">
            <option disabled selected value>-- Select --</option>
            <template v-for="productionLine of dataProductionlines">
              <template v-if="productionLine.name === site">
                <option v-bind:key="productionLine.productionline_name" v-bind:value="productionLine.productionline_name">
                  {{productionLine.productionline_name}}
                </option>
              </template>
            </template>
          </select>
        
          <!-- Team selection -->
          <label for="team">{{$t("team")}}: </label>
          <select name="team" id="team-selection" class="form-select">
            <option disabled selected value="">-- {{ $t("select") }} --</option>
            <option value="allTeams" id="all-teams-option">{{$t("allTeams")}}</option>
            <template v-for="team in dataTeams">
              <template v-if="team.worksite_name === site">
                <option v-bind:key="team.type" v-bind:value="team.type">{{team.type}}</option>
              </template>
            </template> 
          </select>
          </div>
      </div>
      <production-window style="width: 400px;"/>
      <button id="pl-selection-load" type="button" class="btn btn-primary" v-on:click="productionLineSelected();" style="height: 50%; align-self: center;">{{$t("load")}}</button>
    </div>


    <div class="d-flex content-panel">
      <div class="machines-shutdown-panel">
        <div class="title-container">
          <span class="content-title">{{$t("machinesShutdowns")}}</span>
        </div>
        <div class="chart-container">
          <canvas class="chart" id="machines-shutdown-chart"></canvas>
        </div>
        <div class="d-flex table-container">
          <table class="table">
            <thead>
            <tr>
              <th scope="col" class="center-text">{{$t("machineShutdown")}}</th>
              <th scope="col" class="center-text">{{$t("totalDowntime")}}</th>
              <th scope="col" class="center-text">{{$t("frequency")}}</th>
              <th scope="col" class="center-text">{{$t("averageDuration")}}</th>
            </tr>
            </thead>
            <tbody>
            <template v-for="machine of Object.values(shutdowns.machines)">
              <tr class="t-row" :key="machine.id">
                <td scope="col" style="display: flex;">{{$t(machine.machine)}}</td>
                <td scope="col" class="center-text">{{machine.totalDowntime}}</td>
                <td scope="col" class="center-text">{{machine.frequency}}</td>
                <td scope="col"></td>
              </tr>
              <template v-for="cause of Object.values(machine.causes)">
                <tr class="subrow" :key="cause.id">
                  <td scope="col" style="display: flex;">&emsp;{{$t(cause.cause)}}</td>
                  <td scope="col" class="center-text">{{cause.totalDowntime}}</td>
                  <td scope="col" class="center-text">{{cause.frequency}}</td>
                  <td scope="col" class="center-text">{{cause.avgDuration}}</td>
                </tr>
                <template v-for="comment of Object.values(cause.comments)">
                  <tr class="sub-subrow" :key="comment.id">
                    <td scope="col" style="display: flex;">&emsp;&emsp;{{comment.comment}}</td>
                    <td scope="col" class="center-text">{{comment.totalDowntime}}</td>
                    <td scope="col" class="center-text">{{comment.frequency}}</td>
                    <td scope="col" class="center-text">{{comment.avgDuration}}</td>
                  </tr>
                </template>
              </template>
            </template>
            </tbody>
          </table>
        </div>
        <div class="d-flex downtime-percent-container">
          <p class="downtime-percent">{{percentages.machines}} % {{$t("of")}} {{$t("unplannedDowntime")}}</p>
        </div>
      </div>
      <div class="external-shutdown-panel">
        <div class="title-container">
          <span class="content-title">{{$t("externalShutdowns")}}</span>
        </div>
        <div class="chart-container">
          <canvas class="chart" id="external-shutdown-chart"></canvas>
        </div>
        <div class="d-flex table-container">
          <table class="table">
            <thead>
            <tr class="t-row">
              <th scope="col" class="center-text">{{$t("externalShutdown")}}</th>
              <th scope="col" class="center-text">{{$t("totalDowntime")}}</th>
              <th scope="col" class="center-text">{{$t("frequency")}}</th>
              <th scope="col" class="center-text">{{$t("averageDuration")}}</th>
            </tr>
            </thead>
            <tbody>
            <template v-for="external of Object.values(shutdowns.external)">
              <tr class="t-row" :key="external.id">
                <td scope="col">{{$t(external.cause)}}</td>
                <td scope="col" class="center-text">{{external.totalDowntime}}</td>
                <td scope="col" class="center-text">{{external.frequency}}</td>
                <td scope="col" class="center-text">{{external.avgDuration}}</td>
              </tr>
            </template>
            </tbody>
          </table>
        </div>
        <div class="d-flex downtime-percent-container">
          <p class="downtime-percent">{{percentages.external}} % {{$t("of")}} {{$t("unplannedDowntime")}}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import {urlAPI} from "@/variables";
import Chart from "chart.js/auto";
import ProductionWindow from "@/components/DashboardComponents/ProductionWindow";


export default {
  name: "UnplannedDowntimesShutdowns",

  data() {

    var data = {
      username: localStorage.getItem("username"),

      chartObjects: {
        external: undefined,
        machines: undefined,
        created: false
      },
      shutdowns: {
        external: {},
        machines: {}
      },
      percentages: {
        external: "--",
        machines: "--"
      },
      startYear: 2000,
      currentYear: (new Date()).getFullYear(),
      site: '',
      productionLine: '',

      dataWorksite : null,
      dataProductionlines : null,
      unplannedDowntimeEvents : null,
      dataTeams: null
    }

    return data;
  },

  methods: {
    resolveAfter: function(milliseconds) {
      return new Promise(resolve => {
        setTimeout(() => resolve(), milliseconds);
      });
    },

    productionLineSelected: function() {
      setTimeout(() => {
        if (document.getElementById("pl-selection").value 
          && document.getElementById("select-date-from").value
          && document.getElementById("select-date-to").value) 
        {
          this.chargeData();
        }
      });
    },

    createDowntimeObject: function() {

    },

    chargeData: async function () {
      const selectedPL = document.getElementById('pl-selection').value;
      const dateFrom = document.getElementById('select-date-from').value;
      const dateTo = document.getElementById('select-date-to').value;
      const team = document.getElementById('team-selection').value;

      await axios.get(urlAPI + 'UnplannedDowntimeEventsDate/' + selectedPL + '/' + dateFrom + '/' + dateTo + '/' + team)
          .then(response => {
            this.unplannedDowntimeEvents = response.data;
            this.createDowntimeObject();
            if (!this.chartObjects.created) this.createCharts();

            //Used to calculate machine shutdown percentages
            let totalMachineDowntime = 0;
            //Range machine shutdown events by implicated machine and shutdown cause
            let machinesShutdownsRanged = this.unplannedDowntimeEvents["machines"].reduce((acc, event) => {
              //Machines key doesn't exist yet
              if (!acc[event.implicated_machine]) {
                  acc[event.implicated_machine] = {
                  totalDowntime: 0,
                  frequency: 0,
                  machine: event.implicated_machine,
                  causes: {}
                };
              }
              //Cause key doesn't exist yet for current machine
              if (!event.component) event.component = "N/A";
              if (!acc[event.implicated_machine].causes[event.component]) {
                acc[event.implicated_machine].causes[event.component] = {
                  totalDowntime: 0,
                  frequency: 0,
                  avgDuration: 0,
                  cause: event.component,
                  comments: {}
                };
              }

              if (!event.comment) event.comment = "N/A";
              if (!acc[event.implicated_machine].causes[event.component].comments[event.comment]) {
                acc[event.implicated_machine].causes[event.component].comments[event.comment] = {
                  totalDowntime: 0,
                  frequency: 0,
                  avgDuration: 0,
                  comment: event.comment
                };
              }

              totalMachineDowntime += event.total_duration;

              //Update machine related data
              acc[event.implicated_machine].totalDowntime += event.total_duration;
              acc[event.implicated_machine].frequency++;

              //Update reason related data for current machine
              acc[event.implicated_machine].causes[event.component].totalDowntime += event.total_duration;
              acc[event.implicated_machine].causes[event.component].frequency++;
              acc[event.implicated_machine].causes[event.component].avgDuration =
                  Math.floor(acc[event.implicated_machine].causes[event.component].totalDowntime
                      / acc[event.implicated_machine].causes[event.component].frequency);

              //Update comment related data for current reason and machine
              acc[event.implicated_machine].causes[event.component].comments[event.comment].totalDowntime += event.total_duration;
              acc[event.implicated_machine].causes[event.component].comments[event.comment].frequency++;
              acc[event.implicated_machine].causes[event.component].comments[event.comment].avgDuration =
                  Math.floor(acc[event.implicated_machine].causes[event.component].comments[event.comment].totalDowntime
                      / acc[event.implicated_machine].causes[event.component].comments[event.comment].frequency);

              return acc;
            }, {});

            console.log(machinesShutdownsRanged);

            //Used to calculate external shutdown percentage
            let totalExternalDowntime = 0;
            //Range external shutdown events by shutdown cause
            let externalShutdownsRanged = this.unplannedDowntimeEvents["external"].reduce((acc, event) => {
              if (!event.component) event.component = "N/A";
              if (!acc[event.component]) {
                acc[event.component] = {
                  totalDowntime: 0,
                  frequency: 0,
                  avgDuration: 0,
                  cause: event.component
                }
              }

              totalExternalDowntime += event.total_duration;
              acc[event.component].totalDowntime += event.total_duration;
              acc[event.component].frequency++;
              acc[event.component].avgDuration = Math.floor(acc[event.component].totalDowntime / acc[event.component].frequency);

              return acc;
            }, {});


            let machinesShutdownsSorted = Object.keys(machinesShutdownsRanged).reduce((acc, cur) => {
              acc.push([cur, machinesShutdownsRanged[cur]]);
              return acc;
            }, [])
              .sort((a, b) => b[1].totalDowntime - a[1].totalDowntime)
              .reduce((acc, cur) => {
                acc[cur[0]] = cur[1];
                return acc;
              }, {});
            
            let externalShutdownsSorted = Object.keys(externalShutdownsRanged).reduce((acc, cur) => {
              acc.push([cur, externalShutdownsRanged[cur]]);
              return acc;
            }, [])
              .sort((a, b) => b[1].totalDowntime - a[1].totalDowntime)
              .reduce((acc, cur) => {
                acc[cur[0]] = cur[1];
                return acc;
              }, {});


            this.shutdowns.machines = machinesShutdownsSorted;
            this.shutdowns.external = externalShutdownsSorted;
            this.percentages.machines = 100 * totalMachineDowntime / (totalMachineDowntime + totalExternalDowntime);
            this.percentages.machines = (this.percentages.machines ? this.percentages.machines.toFixed(2) : "--");
            this.percentages.external = 100 * totalExternalDowntime / (totalMachineDowntime + totalExternalDowntime);
            this.percentages.external = (this.percentages.external ? this.percentages.external.toFixed(2) : "--");

            //Update charts' data
            for (let shtdCat of ['external', 'machines']) {
              //Delete old values
              this.chartObjects[shtdCat].data.labels = [];
              this.chartObjects[shtdCat].data.datasets[0].data = [];
              this.chartObjects[shtdCat].data.datasets[1].data = [];

              //Insert new values
              for (let shutdownEvent of Object.values(this.shutdowns[shtdCat])) {
                if (shtdCat === "machines") this.chartObjects[shtdCat].data.labels.push(this.$t(shutdownEvent.machine));
                else this.chartObjects[shtdCat].data.labels.push(this.$t(shutdownEvent.cause));
                this.chartObjects[shtdCat].data.datasets[0].data.push(shutdownEvent.totalDowntime);
                this.chartObjects[shtdCat].data.datasets[1].data.push(shutdownEvent.frequency);
              }
              this.chartObjects[shtdCat].update();
            }
          });
    },

    createCharts: function() {
      this.chartObjects.created = true;
      for (let ch of ['external', 'machines']) {
        this.chartObjects[ch] = new Chart(ch + '-shutdown-chart', {
          type: 'bar',
          data: {
            labels: [],
            datasets: [{
              label: this.$t("timeInMinutes"),
              backgroundColor: 'rgb(112, 184, 232)',
              data: []
            },
              {
                label: this.$t("number"),
                backgroundColor: 'rgb(246, 184, 192)',
                data: [],
                yAxisID: "freq"
              }
            ]
          },
          options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
              time: {
                axis: 'y',
                title: {
                  display: true,
                  text: this.$t("timeInMinutes")
                },
                position: 'left'
              },
              freq: {
                axis: 'y',
                title: {
                  display: true,
                  text: this.$t("number")
                },
                position: 'right'
              }
            },
            plugins: {
              legend: {
                display: true
              }
            }
          },
        });
      }
    }
  },

  async mounted() {
    if (sessionStorage.getItem("language") !== null) {
      this.$i18n.locale = sessionStorage.getItem("language");
    }
    if(sessionStorage.getItem("user-status") == 1){
      await axios.get(urlAPI + 'sites/'+this.username)
          .then(response => (this.dataSite = response.data))
    }else{
      await axios.get(urlAPI + 'sites')
          .then(response => (this.dataSite = response.data))
    }


    this.dataWorksite = this.dataSite[0];
    this.dataProductionlines = this.dataSite[1];
    this.dataTeams = this.dataSite[2];

    //Load chart.js into vue component
    let chartJs = document.createElement('script');
    chartJs.setAttribute('src', 'https://cdn.jsdelivr.net/npm/chart.js');
    document.head.appendChild(chartJs);
  },
  components: {
    ProductionWindow
  },
  watch: {
    '$i18n.locale': function() {
      for (let shtdCat of ['external', 'machines']) {
        if (this.chartObjects[shtdCat]) {
          this.chartObjects[shtdCat].data.datasets[0].label = this.$t("timeInMinutes");
          this.chartObjects[shtdCat].data.datasets[1].label = this.$t("number");
          this.chartObjects[shtdCat].options.scales.time.title.text = this.$t("timeInMinutes");
          this.chartObjects[shtdCat].options.scales.freq.title.text = this.$t("number");
          this.chartObjects[shtdCat].update();
        }
      }
    }
  }
}
</script>
<style scoped>

div.main-container {
  flex-direction: column;
  background-color: white;
  padding: 20px;
  min-width: 1000px;
  border-radius: 5px;
  margin: 20px 0px;
}

div.selection-menu {
  flex-direction: row;
  padding: 20px 0px;
  border-bottom: solid 1px;
  justify-content: space-evenly;
}

div.site-pl-selection {
  flex-direction: column;
  justify-content: space-evenly;
  min-width: 200px;
}

div.site-pl-selection > div{
  align-items: center;
}

div.site-pl-selection select {
  width: 100%;
}

div.site-pl-selection label {
  margin: 0px 10px 0px 0px;
}

div.title-container {
  margin-top: 10px;
  display: flex;
  justify-content: center;
}

span.content-title {
  font-size: 20px;
  font-weight: bold;
}

div.content-panel > div {
  display: flex;
  flex-direction: column;
  width: 50%;
}

div.chart-container {
  padding: 15px;
  height: 300px;
}

div.table-container {
  justify-content: center;
  padding: 15px 30px;
}

div.table-container .center-text {
  text-align: center;
}

tr.subrow > td {
  padding: 0px 0px 0px 5px !important;
  border: none;
  color: rgb(85, 85, 85);
  font-weight: bold;
  font-size: 15px;
}

tr.sub-subrow > td {
  padding: 0px 0px 0px 5px !important;
  border: none;
  color: gray;
  font-weight: bold;
  font-size: 14px;
}


tr.t-row > td {
  padding: 0.75rem 0px 0.75px 5px;
  color: black;
  font-weight: bold;
  font-size: 17px;
}


div.downtime-percent-container {
  justify-content: center;
}

p.downtime-percent {
  font-size: 16px;
}

thead {
  color: white;
  background: #56baed;
}

</style>
