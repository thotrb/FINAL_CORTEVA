<template>
  <div class="d-flex main-container">
    <!-- Page title - Unplanned Downtime Dashboard -->
    <div class="row container-title">
      <span>{{ $t("unplannedDowntimeDashboard") }}</span>
    </div>

    <!-- Interval, site and production line selection menu -->
    <div class="d-flex selection-menu">
      <!-- Site selection-->
      <div class="site-pl-selection">
        <label for="site-selection">{{$t("site")}} : </label>
        <select id="site-selection" v-model="site">
          <option disabled selected value>-- {{ $t("select") }} --</option>
          <template v-for="site of dataWorksite">
            <option v-bind:key="site.name" v-bind:value="site.name">
              {{ site.name }}
            </option>
          </template>
        </select>

        <!-- Production line selection -->
        <label for="pl-selection">{{ $t("productionLine") }} : </label>
        <select id="pl-selection">
          <option disabled selected value="">-- {{ $t("select") }} --</option>
          <template v-for="productionLine of dataProductionlines">
            <template v-if="productionLine.name === site">
              <option v-bind:key="productionLine.productionline_name" v-bind:value="productionLine.productionline_name">
                {{ productionLine.productionline_name }}
              </option>
            </template>
          </template>
        </select>
      </div>
      <div class="main-production-window">
        <div class="d-flex title">
          <span>{{$t("yearSelection")}}</span>
        </div>
        <div class="d-flex interval-selection">
          <span>{{$t("year")}}</span>
          <input id="year-select" type="number" min="2000" max="2099" step="1"/>
        </div>
      </div>
      <button id="pl-selection-load" type="button" class="btn btn-primary" v-on:click="productionLineSelected();">{{$t("load")}}</button>
    </div>

    <!-- Downtime table, yearly and average information container-->
    <div class="d-flex table-ya-container">
      <!-- Downtime table -->
      <div class="d-flex container-table">
        <table class="table">
          <thead>
          <tr>
            <th scope="col"></th>
            <template v-for="month of months">
              <th :key="month" scope="col">{{ $t(month) }}</th>
            </template>
          </tr>
          </thead>
          <tbody>
          <template v-for="cat of Object.keys(unplannedDowntimesCategories)">
            <tr v-bind:key="cat.id">
              <td scope="col" class="side">
                <tr class="table-row-title">
                  {{$t(unplannedDowntimesCategories[cat])}}
                </tr>
                <tr class="table-sub-row">
                  &emsp;{{ $t("duration")}}
                </tr>
                <tr class="table-sub-row">
                  &emsp;{{ $t("number")}}
                </tr>
              </td>
              <template v-for="month of months">
                <td class="table-data" :key="month">
                  <tr style="height: 10px; background-color: white; color: white">
                    -
                  </tr>
                  <tr class="table-sub-row">
                    {{downtimes[cat][month].totalDuration ? downtimes[cat][month].totalDuration.toFixed(2) : undefined }}
                  </tr>
                  <tr class="table-sub-row">
                    {{downtimes[cat][month].totalNb }}
                  </tr>
                </td>
              </template>
            </tr>
          </template>
          </tbody>
        </table>
      </div>

      <!-- Downtime yearly and average info -->
      <div class="d-flex container-yearly-avg-info">
        <template v-for="cat of ['cip', 'cov', 'bnc']">
          <div :key="cat" class="d-flex ya-info-row">
            <div class="d-flex">
              <span style="font-weight: bold">{{
                  $t("yearly" + cat.toUpperCase())
                }}</span>
              <span
              >&emsp;{{ downtimes[cat].general.yearlyDuration }}
                {{ $t("hours") }}</span
              >
              <span
              >&emsp;{{ downtimes[cat].general.yearlyNb }}
                {{ cat.toUpperCase() }}</span
              >
            </div>
            <div class="d-flex">
              <span style="font-weight: bold">{{ $t("average") }}</span>
              <span
              >&emsp;{{ downtimes[cat].general.yearlyAvg }}
                {{ $t("hours") }}</span
              >
            </div>
          </div>
        </template>
      </div>
    </div>

    <!-- Downtime graphs-->
    <div class="main-chart-container">
      <div class="chart-container">
        <canvas class="chart" id="cip-chart"></canvas>
        <p class="downtime-percent" id="cip-percent">
          <span
          >{{
              downtimes.cip.general
                  ? downtimes.cip.general.downtimePercentage
                  : "--"
            }}
            %
          </span>
          <span>{{ $t("of") }}&nbsp;</span>
          <span>{{ $t("unplannedDowntime") }}</span>
        </p>
      </div>
      <div class="chart-container">
        <canvas class="chart" id="cov-chart"></canvas>
        <p class="downtime-percent" id="cov-percent">
          <span
          >{{
              downtimes.cov.general
                  ? downtimes.cov.general.downtimePercentage
                  : "--"
            }}
            %
          </span>
          <span>{{ $t("of") }}&nbsp;</span>
          <span>{{ $t("unplannedDowntime") }}</span>
        </p>
      </div>
      <div class="chart-container">
        <canvas class="chart" id="bnc-chart"></canvas>
        <p class="downtime-percent" id="bnc-percent">
          <span
          >{{
              downtimes.bnc.general
                  ? downtimes.bnc.general.downtimePercentage
                  : "--"
            }}
            %
          </span>
          <span>{{ $t("of") }}&nbsp;</span>
          <span>{{ $t("unplannedDowntime") }}</span>
        </p>
      </div>
    </div>

    <!-- Production window -->
    <div class="d-flex production-window-container">
      <production-window/>
      <div class="d-flex pw-table-container">
        <template v-for="cat of Object.keys(unplannedDowntimesCategories)">
          <table class="table" :key="cat.id">
            <thead></thead>
            <tbody>
            <tr class="t-row">
              <td scope="col">{{ $t(unplannedDowntimesCategories[cat]) }}</td>
              <td scope="col"></td>
            </tr>
            <tr class="subrow">
              <td scope="col">&emsp;{{ $t("duration") }} ({{ $t("hours")}})</td>
              <td scope="col">{{ generalData[cat].totalDuration }}</td>
            </tr>
            <tr class="subrow last-subrow">
              <td scope="col">&emsp;{{ $t("number") }}</td>
              <td scope="col">{{ generalData[cat].totalNb }}</td>
            </tr>
            </tbody>
          </table>
        </template>
      </div>
    </div>

    <div class="row justify-content-center seq-tables-container">
      <div class="col-auto">
        <table class="table table-responsive seq-cip">
          <thead>
          <tr>
            <th scope="col">{{ $t("cleaningInPlaceCIP") }}</th>
            <template v-for="cat of Object.keys(sequencesCIP)">
              <th :key="cat.id">{{ cat }}</th>
            </template>
          </tr>
          </thead>
          <tbody>
          <tr>
            <th scope="row">{{ $t("duration") }} ({{ $t("minutes")}})</th>
            <template v-for="event of sequencesCIP">
              <td :key="event.id">{{ event.totalDuration }}</td>
            </template>
          </tr>
          <tr>
            <th scope="row">{{ $t("number") }}</th>
            <template v-for="event of sequencesCIP">
              <td :key="event.id">{{ event.number }}</td>
            </template>
          </tr>
          <tr>
            <th scope="row">{{ $t("average") }}</th>
            <template v-for="event of sequencesCIP">
              <td :key="event.id">{{ event.avgDuration }}</td>
            </template>
          </tr>
          <tr>
            <th scope="row">{{ $t("standardDeviation") }}</th>
            <template v-for="event of sequencesCIP">
              <td :key="event.id">{{ event.std.toFixed(4) }}</td>
            </template>
          </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div class="row justify-content-center seq-tables-container">
      <div class="col-auto">
        <table class="table table-responsive seq-cov">
          <thead>
          <tr>
            <th scope="col">{{ $t("changeOver") }}</th>
            <template v-for="cat of Object.keys(sequencesCOV)">
              <th :key="cat.id">{{ cat }}</th>
            </template>
          </tr>
          </thead>
          <tbody>
          <tr>
            <th scope="row">{{ $t("duration") }}</th>
            <template v-for="event of sequencesCOV">
              <td :key="event.id">{{ event.totalDuration }}</td>
            </template>
          </tr>
          <tr >
            <th scope="row">{{ $t("number") }}</th>
            <template v-for="event of sequencesCOV">
              <td :key="event.id">{{ event.number }}</td>
            </template>
          </tr>
          <tr >
            <th scope="row">{{ $t("average") }}</th>
            <template v-for="event of sequencesCOV">
              <td :key="event.id">{{ event.avgDuration }}</td>
            </template>
          </tr>
          <tr >
            <th scope="row">{{ $t("standardDeviation") }}</th>
            <template v-for="event of sequencesCOV">
              <td :key="event.id">{{ event.std.toFixed(4) }}</td>
            </template>
          </tr>
          </tbody>
        </table>
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
  name: "UnplannedDowntimes",
  data() {
    var data = {
      username: localStorage.getItem("username"),

      months: [
        "Jan",
        "Feb",
        "Mar",
        "Apr",
        "May",
        "Jun",
        "Jul",
        "Aug",
        "Sep",
        "Oct",
        "Nov",
        "Dec"
      ],
      unplannedDowntimesCategories: {
        cip: "cleaningInPlaceCIP",
        cov: "changeOverCOV",
        bnc: "batchNumberChangeBNC"
      },
      downtimes: {
        cip: {},
        cov: {},
        bnc: {}
      },
      generalData: {
        cip: {},
        cov: {},
        bnc: {}
      },
      chartObjects: {
        cip: undefined,
        cov: undefined,
        bnc: undefined,
        created: false
      },
      site: "",
      productionLine: "",
      team: "",
      sequencesCIP: {},
      sequencesCOV: {},
      dataWorksite : null,
      dataProductionlines : null,
      dataTeams : null
    };

    //Populate downtimes array
    for (let month of data.months) {
      data.downtimes.cip[month] = {
        totalNb: undefined,
        totalDuration: undefined,
        events: []
      };
      data.downtimes.cov[month] = {
        totalNb: undefined,
        totalDuration: undefined,
        events: []
      };
      data.downtimes.bnc[month] = {
        totalNb: undefined,
        totalDuration: undefined,
        events: []
      };
    }
    data.downtimes.cip.general = {
      yearlyDuration: undefined,
      yearlyNb: undefined,
      yearlyAvg: undefined,
      downtimePercentage: "--"
    };
    data.downtimes.cov.general = {
      yearlyDuration: undefined,
      yearlyNb: undefined,
      yearlyAvg: undefined,
      downtimePercentage: "--"
    };
    data.downtimes.bnc.general = {
      yearlyDuration: undefined,
      yearlyNb: undefined,
      yearlyAvg: undefined,
      downtimePercentage: "--"
    };

    //Populate years array
    for (let i = data.startYear; i <= data.currentYear; i++) data.years.push(i);
    data.yearsAfterFrom = data.years;

    return data;
  },

  methods: {
    calculateYearsAfterFrom: function() {
      const selectedYear = document.getElementById("select-year-from").value;
      this.yearsAfterFrom = [];
      for (let i = selectedYear; i <= this.currentYear; i++)
        this.yearsAfterFrom.push(i);
    },

    productionLineSelected: function() {
      this.resolveAfter(300).then(() => {
        if (document.getElementById("pl-selection").value && document.getElementById("year-select").value) {
          this.chargeCurrentYearData();
        }
      });
    },

    getMonth: function(dateString) {
      return parseInt(dateString.substring(5, 7));
    },

    createDowntimeObject: function() {
      for (let month of this.months) {
        this.downtimes.cip[month] = {
          totalNb: 0,
          totalDuration: 0,
          events: []
        };
        this.downtimes.cov[month] = {
          totalNb: 0,
          totalDuration: 0,
          events: []
        };
        this.downtimes.bnc[month] = {
          totalNb: 0,
          totalDuration: 0,
          events: []
        };
      }
      this.downtimes.cip.general = {
        yearlyDuration: undefined,
        yearlyNb: undefined,
        yearlyAvg: undefined,
        downtimePercentage: "--"
      };
      this.downtimes.cov.general = {
        yearlyDuration: undefined,
        yearlyNb: undefined,
        yearlyAvg: undefined,
        downtimePercentage: "--"
      };
      this.downtimes.bnc.general = {
        yearlyDuration: undefined,
        yearlyNb: undefined,
        yearlyAvg: undefined,
        downtimePercentage: "--"
      };
    },

    chargeCurrentYearData: async function () {
      const selectedPL = document.getElementById("pl-selection").value;
      const selectedYear = document.getElementById("year-select").value;
      const team = 'allTeams'
      

      await axios.get(urlAPI + 'UnplannedDowntimeEvents/' + selectedPL + '/' + selectedYear + '/' + selectedYear + '/' + team)
          .then(async (response) => {
                this.unplannedDowntimeEvents = response.data;
                this.resolveAfter(1000);
                this.createDowntimeObject();
                console.log(response);

                let totalDuration = {
                  cip: 0,
                  cov: 0,
                  bnc: 0
                };
                let totalNb = {
                  cip: 0,
                  cov: 0,
                  bnc: 0
                };
                let labels = {
                  cip: {},
                  cov: {},
                  bnc: {}
                };

                let totalDowntimeDuration = 0;

            let CIPsToSkip = [];
            for (let type of ["cip", "cov", "bnc"]) {
                  for (let event of this.unplannedDowntimeEvents[type.toUpperCase()]) {
                    let eventDurationInHours = event.total_duration / 60;
                    let eventDurationLabelCoef = Math.floor(event.total_duration / 10);
                    let durationIntervalChartLabel = `${10 * eventDurationLabelCoef}-${10 * eventDurationLabelCoef + 9} min`;
                    const monthCreated = this.getMonth(event.created_at);
                    const month = this.months[monthCreated - 1];

                    //Search for overlapping CIP
                    if (type === 'cip' && !event.finished) {
                      let olCIP = await axios.get(urlAPI + "getOverlappedCIP/" + event.productionline + "/" + event.created_at.split('T')[0] + "/" + event.OLE);
                      console.log(`Overlapping CIP for ${event.id} : ${olCIP.id}`)
                      olCIP = olCIP.data;
                      if (olCIP.length > 0) {
                        eventDurationInHours += (olCIP[0].total_duration/60);
                        eventDurationLabelCoef += Math.floor(olCIP[0].total_duration/10);
                        durationIntervalChartLabel = `${10 * eventDurationLabelCoef}-${10 * eventDurationLabelCoef + 9} min`;
                        CIPsToSkip.push(olCIP[0].id);
                      }             
                    }

                    //Skip CIP if it is overlapping
                    if (type === 'cip' && CIPsToSkip.includes(event.id)) continue;

                    console.log("Event duration in hours: " + eventDurationInHours);
                    console.log("Type", type);
                    console.log("Month", month);
                  
                    if (!Object.keys(labels[type]).includes(durationIntervalChartLabel)){
                      labels[type][durationIntervalChartLabel] = 1;
                    } else {
                      labels[type][durationIntervalChartLabel]++;
                    }
                    this.downtimes[type][month].events.push(event);
                    this.downtimes[type][month].totalNb++;
                    this.downtimes[type][month].totalDuration += eventDurationInHours;
                    totalDuration[type] += eventDurationInHours;
                    totalNb[type]++;
                  }

                  totalDowntimeDuration += totalDuration[type];
                  //Nb.: Or (||) operator returns last velue when both are falsy
                  this.downtimes[type].general.yearlyDuration = totalDuration[type].toFixed(2);
                  this.downtimes[type].general.yearlyNb = totalNb[type];
                  this.downtimes[type].general.yearlyAvg = (totalDuration[type] / totalNb[type] || 0).toFixed(2);
                  //Create charts if they dont exist already
                  if (!this.chartObjects.created){
                    this.createCharts();
                  }

                  //Sort labels alphabetically
                  labels[type] = Object.keys(labels[type])
                      .sort()
                      .reduce((acc, key) => {
                        acc[key] = labels[type][key];
                        return acc;
                      }, {});

                  //Update chart data
                  this.chartObjects[type].data.labels = Object.keys(labels[type]);
                  this.chartObjects[type].data.datasets[0].data = [];
                  for (let label of Object.keys(labels[type])) {
                    this.chartObjects[type].data.datasets[0].data.push(labels[type][label]);
                  }
                  this.chartObjects[type].update();
                }

                for (let type of ["cip", "cov", "bnc"]) {
                  let downtimePercent = (totalDuration[type] / totalDowntimeDuration) * 100;
                  if (downtimePercent) {
                    downtimePercent = downtimePercent.toFixed(2);
                    this.downtimes[type].general.downtimePercentage = downtimePercent;
                  }
                }
            })

      this.chargeGeneralData();
    },

    chargeGeneralData: async function () {
      const selectedPL = document.getElementById("pl-selection").value;
      const dateFrom = document.getElementById("select-date-from").value;
      const dateTo = document.getElementById("select-date-to").value;
      const team = "allTeams";

      if (selectedPL && dateFrom && dateTo) {
        await axios.get(urlAPI + 'UnplannedDowntimeEventsDate/' + selectedPL + '/' + dateFrom + '/' + dateTo + '/' + team)
          .then(async response => {
            let ude = response.data;
            let CIPsToSkip = {};

            for (let cat of ["cip", "cov", "bnc"]) {
              this.generalData[cat] = {totalDuration: 0, totalNb: 0};

              for (let event of ude[cat.toUpperCase()]) {
                //Search for overlapping CIP
                if (cat == 'cip' && !event.finished) {
                  let olCIP = await axios.get(urlAPI + "getOverlappedCIP/" + event.productionline + "/" + event.created_at.split('T')[0] + "/" + event.OLE);
                  olCIP = olCIP?.data[0];
                
                  if (olCIP) {
                    CIPsToSkip[olCIP.id] = {starting: event, ending: olCIP};
                    console.log("CIPsToSkip", CIPsToSkip);
                  }              
                }
                
                this.generalData[cat].totalDuration += parseInt(event.total_duration)/60;

                //Skip CIP if it is overlapping, duration already counted 
                if (cat == 'cip' && Object.keys(CIPsToSkip).includes(event.id + '')) continue;
                else this.generalData[cat].totalNb++;
              }
   
              this.generalData[cat].totalDuration = this.generalData[cat].totalDuration.toFixed(2);
            }

            this.sequencesCIP = {};
            let sequeceCIPEvents = ude["seqCIP"].reduce((acc, event) => {
              let eventId = event.id + '';
              if (Object.keys(CIPsToSkip).includes(eventId)) {
                return acc;
              }
              else if (acc.filter(e => e.id == event.id).length == 0) {
                acc.push(event);
              }
              return acc;
            }, []);
            
            Object.values(CIPsToSkip).forEach(cip => {
              sequeceCIPEvents = sequeceCIPEvents.map(e => {
                if (e.id == cip.starting.id) {
                  e.total_duration += cip.ending.total_duration;
                }
                return e;
              });
            });

            for (let event of sequeceCIPEvents) {
              const pair = event.previous_bulk + "/" + event.bulk;
              if (!this.sequencesCIP[pair]) {
                this.sequencesCIP[pair] = {
                  totalDuration: 0,
                  number: 0,
                  avgDuration: 0,
                  std: 0,
                  durations: []
                };
              }
              this.sequencesCIP[pair].durations.push(event.total_duration);
              this.sequencesCIP[pair].totalDuration += event.total_duration;
              this.sequencesCIP[pair].number++;
              this.sequencesCIP[pair].avgDuration = (this.sequencesCIP[pair].totalDuration / this.sequencesCIP[pair].number).toFixed(2);

              //Calculate standard deviation
              let std = 0;
              let mean = this.sequencesCIP[pair].avgDuration;
              for (let duration of this.sequencesCIP[pair].durations) {
                std += (duration - mean) ** 2;
              }
              std /= this.sequencesCIP[pair].durations.length;
              std **= 0.5;

              this.sequencesCIP[pair].std = std;
            }

            this.sequencesCOV = {};
            let sequencesCOVEvents = ude["seqCOV"].reduce((acc, event) => {
              if (acc.filter(e => e.id == event.id).length == 0) {
                acc.push(event);
              }
              return acc;
            }, []);
            for (let event of sequencesCOVEvents) {
              const type = event.size + "L";
              if (!this.sequencesCOV[type]) {
                this.sequencesCOV[type] = {
                  totalDuration: 0,
                  number: 0,
                  avgDuration: 0,
                  std: 0,
                  durations: []
                };
              }
              this.sequencesCOV[type].durations.push(event.total_duration);
              this.sequencesCOV[type].totalDuration += event.total_duration;
              this.sequencesCOV[type].number++;
              this.sequencesCOV[type].avgDuration =
                  (this.sequencesCOV[type].totalDuration /
                      this.sequencesCOV[type].number).toFixed(2);

              //Calculate standard deviation
              let std = 0;
              let mean = this.sequencesCOV[type].avgDuration;
              for (let duration of this.sequencesCOV[type].durations) {
                std += (duration - mean) ** 2;
              }
              std /= this.sequencesCOV[type].durations.length;
              std **= 0.5;

              this.sequencesCOV[type].std = std;
            }
          });
      }

      
    },

    resolveAfter: function(milliseconds) {
      return new Promise(resolve => {
        setTimeout(() => resolve(), milliseconds);
      });
    },

    createCharts: function() {
      for (let type of ["cip", "cov", "bnc"]) {
        const chartName = type + "-chart";
        this.chartObjects[type] = new Chart(chartName, {
          type: "bar",
          data: {
            labels: [],
            datasets: [
              {
                label: type.toUpperCase(),
                data: [],
                backgroundColor: "rgb(112, 184, 232)"
              }
            ]
          },
          options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
              title: {
                display: true,
                text: this.$t(this.unplannedDowntimesCategories[type])
              }
            }
          }
        });
      }
      this.chartObjects.created = true;
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

    //Add event listener to production window
    document.getElementById("select-date-to").onchange = this.chargeGeneralData;
    document.getElementById("select-date-from").onchange = this.chargeGeneralData;


    //Load chart.js into vue component
    let chartJs = document.createElement("script");
    chartJs.setAttribute("src", "https://cdn.jsdelivr.net/npm/chart.js");
    document.head.appendChild(chartJs);

    document.getElementById("year-select").value = new Date().getFullYear()
  },



  components: {
    ProductionWindow
  },

  watch: {
    "$i18n.locale": function() {
      for (let type of ["cip", "cov", "bnc"]) {
        if (this.chartObjects[type]) {
          this.chartObjects[type].options.plugins.title.text = this.$t(
              this.unplannedDowntimesCategories[type]
          );
          this.chartObjects[type].update();
        }
      }
    }
  }
};
</script>
<style scoped>
div.main-container {
  flex-direction: column;
  background-color: white;
  padding: 20px;
  min-width: 1200px;
  border-radius: 5px;
  margin: 20px 0px;
}

div.container-title {
  justify-content: center;
}

div.container-title > span {
  font-size: 30px;
  font-weight: bold;
  color: black;
}

div.selection-menu {
  flex-direction: row;
  padding: 20px 0px;
  border-bottom: solid 1px;
  align-items: center;
  justify-content: space-evenly;
}

div.site-pl-selection {
  min-width: 200px;
}

div.site-pl-selection > div {
  align-items: center;
}

div.site-pl-selection select {
  width: 100%;
}

div.site-pl-selection label {
  margin: 10px 0px 0px 0px;
}

div.table-ya-container {
  margin-top: 20px;
  justify-content: center;
}

div.container-table tr.table-sub-row {
  color: gray;
}

div.container-yearly-avg-info {
  flex-direction: column;
  justify-content: space-around;
  align-items: center;
  margin: 0px 30px;
  padding-left: 10px;
  border: solid 1px;
  border-radius: 5px;
}

div.container-yearly-avg-info div.ya-info-row > div {
  flex-direction: column;
  margin: 0px 50px 15px 0px;
}

div.container-table td.table-data > tr {
  text-align: center;
}

div.main-chart-container {
  margin-top: 20px;
  display: flex;
  justify-content: center;
  height: 350px;
  border-bottom: 1px solid;
}

div.chart-container {
  width: 25% !important;
  height: 300px;
  margin: 0px 10px;
}

p.downtime-percent {
  text-align: center;
  margin: 10px 0px;
  font-size: 16px;
}

div.production-window-container {
  display: flex;
  flex-direction: column;
  border: solid 1px;
  border-radius: 5px;
  margin-top: 10px;
  width: 800px;
  align-self: center;
}

tr.subrow > td {
  padding: 0px !important;
  border: none;
  color: gray;
  font-weight: bold;
}

tr.last-subrow > td {
  padding: 0px 0px 0.75rem 0px !important;
}

tr.t-row > td {
  padding: 0.75rem 0px 0px 0px;
  color: black;
  font-weight: bold;
}

div.pw-table-container {
  padding: 0px 10px;
}

div.table-ya-container thead,
div.seq-tables-container thead {
  color: white;
  background: #56baed;
}

div.pw-table-container th {
  border: none;
}

div.pw-table-container table.table {
  margin: 0px 10px;
}

div.production-window {
  border: none !important;
}

div.container-table th {
  border-top: none;
  border-bottom: none;
}

div.container-table th:first-of-type,
div.seq-tables-container th:first-of-type {
  border-top-left-radius: 7px;
  border-bottom-left-radius: 7px;
}

div.container-table th:last-of-type,
div.seq-tables-container th:last-of-type {
  border-top-right-radius: 7px;
  border-bottom-right-radius: 7px;
}

div.seq-tables-container table {
  text-align: center;
}

table.seq-cip {
  margin-top: 20px;
  margin-bottom: 20px;
}

div.main-production-window {
  flex-direction: column;
  border: solid 1px;
  border-radius: 5px;
  padding: 10px 20px;
  height: 91px;
}

div.main-production-window > div {
  justify-content: center;
}

div.main-production-window > div.title span {
  font-size: 20px;
  font-weight: bold;
  margin-bottom: 10px;
}

div.main-production-window > div.interval-selection > input {
  margin: 0px 10px;
  width: 35%;
  font-size: 12px;
}

div.main-production-window > div.interval-selection > * {
  font-size: 17px;
}

div.main-production-window input#year-select {
  min-width: 60px;
}
</style>
