import Vue from 'vue'
import VueRouter from 'vue-router'
import ChoiceLoginPage from '../views/ChoiceLoginPage.vue'
import Login from '../views/UserInputs/LoginPage.vue'
import HomePage from '../views/UserInputs/HomePage.vue'
import TeamInfo from "@/views/UserInputs/TeamInfo";
import Unplanned_Planned_Pannel1 from "@/views/UserInputs/Unplanned_Planned_Pannel1";
import Planned_Event_Declaration_Pannel2 from "@/views/UserInputs/Planned_Event_Declaration_Pannel2";
import DowntimesDashboard from "@/views/Dashboards/DowntimesDashboard";
import Planned_Event_Declaration_Pannel3 from "@/views/UserInputs/Planned_Event_Declaration_Pannel3";
import CIP_Declaration_Unplanned_Panel3 from "@/views/UserInputs/CIP_Declaration_Unplanned_Panel3";
import ClientChanging_Declaration_Unplanned_Panel3
  from "@/views/UserInputs/ClientChanging_Declaration_Unplanned_Panel3";
import FormatChanging_Declaration_Unplanned_Panel3
  from "@/views/UserInputs/FormatChanging_Declaration_Unplanned_Panel3";
import UnplannedDowntime_Declaration_Unplanned_Panel3
  from "@/views/UserInputs/UnplannedDowntime_Declaration_Unplanned_Panel3";
import End_PO_Declaration from "@/views/UserInputs/End_PO_Declaration";
import PackagingLineID_Dashboard from "@/views/Dashboards/PackagingLineID_Dashboard";
import QualityLosses_Dashboard from "@/views/Dashboards/QualityLosses_Dashboard";
import ProductionReport_Dashboard from "@/views/Dashboards/ProductionReport_Dashboard";
import UnplannedSpeedlosses_Dashboard from "@/views/Dashboards/UnplannedSpeedlosses_Dashboard";
import "chart.js/auto";
import 'chart.js';
import OLE_Dashboard from "@/views/Dashboards/OLE_Dashboard";
import UnplannedDowtimes_Dashboard from "@/views/Dashboards/UnplannedDowtimes_Dashboard";
import UnplannedDowntimes_Shutdowns_Dashboard from "@/views/Dashboards/UnplannedDowntimes_Shutdowns_Dashboard";
import axios from 'axios'

axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem("auth-token")}`;
axios.interceptors.response.use(response => response, error => {
  if (error.response || error.response.status == 401) {
    localStorage.removeItem('username');
    localStorage.removeItem('auth-token');
    router.replace("/");
  }
});

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'ChoiceLoginPage',
    component: ChoiceLoginPage
  },
  {
    path: '/login',
    name: 'Login',
    component: Login
  },
  {
    path: '/teamInfo',
    name: 'TeamInfo',
    component : TeamInfo
  },
  {
    path: '/homePage',
    name: 'HomePage',
    component: HomePage
  },
  {
    path: '/eventDeclaration/:productionLine',
    name: 'IncidentDeclaration',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: Unplanned_Planned_Pannel1
  },
  {
    path: '/eventDeclaration/:productionLine/:downtimeType',
    name: 'Unplanned_Planned_Choice_Reason_Pannel2',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: Planned_Event_Declaration_Pannel2
  },

  {
    path: '/eventDeclaration/:productionLine/plannedDowntime/:reason',
    name: 'Unplanned_Pannel1',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: Planned_Event_Declaration_Pannel3
  },

  {
    path: '/eventDeclaration/:productionLine/unplannedDowntime/CIP',
    name: 'CIPDeclaration',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: CIP_Declaration_Unplanned_Panel3
  },

  {
    path: '/eventDeclaration/:productionLine/unplannedDowntime/packNumberChanging',
    name: 'ChangingClient',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: ClientChanging_Declaration_Unplanned_Panel3
  },

  {
    path: '/eventDeclaration/:productionLine/unplannedDowntime/formatChanging',
    name: 'ChangingFormat',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: FormatChanging_Declaration_Unplanned_Panel3
  },

  {
    path: '/eventDeclaration/:productionLine/unplannedDowntime/unplannedDowntime',
    name: 'UnplannedDowntimeDeclaration',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: UnplannedDowntime_Declaration_Unplanned_Panel3
  },

  {
    path: '/endPO/:productionLine/endPO',
    name: 'EndPODeclaration',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: End_PO_Declaration
  },
  {
    path: '/Dashboard/downtimesReport',
    name: 'DowntimesReportDashboard',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: DowntimesDashboard
  },

  {
    path: '/Dashboard/packagingLineID',
    name: 'PackagingLineIDDashboard',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: PackagingLineID_Dashboard
  },

  {
    path: '/Dashboard/qualityLossesDashboard',
    name: 'QualityLossesDashboard',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: QualityLosses_Dashboard
  },

  {
    path: '/Dashboard/productionDashboard',
    name: 'ProductionReportDashboard',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: ProductionReport_Dashboard
  },
  {
    path: '/Dashboard/overallLineEffectivness',
    name: 'OLEDashboard',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: OLE_Dashboard
  },

  {
    path: '/Dashboard/unplannedDowntimeDashboard',
    name: 'UnplannedDowntimesDashboard',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: UnplannedDowtimes_Dashboard
  },


  {
    path: '/Dashboard/unplannedDowntimeShutdowns',
    name: 'UnplannedDowntimeShutdownsDashboard',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: UnplannedDowntimes_Shutdowns_Dashboard
  },
  {
    path: '/Dashboard/unplannedDowntimeSpeedLosses',
    name: 'UnplannedSpeedlossesDashboard',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: UnplannedSpeedlosses_Dashboard
  },

]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

router.beforeEach((to, from, next) => {
  // redirect to login page if not logged in and trying to access a restricted page
  const token = localStorage.getItem('auth-token');
  const publicPages = ['/login', '/'];
  const authRequired = !publicPages.includes(to.path);
  const loggedIn = localStorage.getItem('username');

  if (authRequired && !token) {
    return next('/');
  }
  if (authRequired && !loggedIn) {
    return next('/');
  }
  next();
})

export default router
