import Vue from "vue";
import VueRouter from "vue-router";

Vue.use(VueRouter);

const routes = [
  {
    path: "/login",
    name: "login",
    component: () => import("../views/LoginView.vue"),
  },
  {
    path: "/",
    name: "home",
    component: () => import("../views/HomeView.vue"),
    meta: {
      isAuthRequired: true,
    },
    redirect: "/welcome",
    children: [
      {
        path: "welcome",
        name: "welcome",
        // route level code-splitting
        // this generates a separate chunk (About.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import("../views/WelcomeView.vue"),
      },
      {
        path: "agentsList",
        name: "agentsList",
        // route level code-splitting
        // this generates a separate chunk (About.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import("../views/AgentsList.vue"),
      },
      {
        path: "membersList",
        name: "membersList",
        // route level code-splitting
        // this generates a separate chunk (About.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import("../views/MembersList.vue"),
      },
    ],
  },
];

const router = new VueRouter({
  routes,
});

export default router;
