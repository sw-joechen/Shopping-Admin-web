import Vue from 'vue';
import VueRouter from 'vue-router';
import Cookies from 'js-cookie';

Vue.use(VueRouter);

const routes = [
  {
    path: '/login',
    name: 'login',
    component: () => import('../views/LoginView.vue'),
  },
  {
    path: '/',
    name: 'home',
    component: () => import('../views/HomeView.vue'),
    meta: {
      isAuthRequired: true,
    },
    redirect: '/welcome',
    children: [
      {
        path: 'welcome',
        name: 'welcome',
        // route level code-splitting
        // this generates a separate chunk (About.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import('../views/WelcomeView.vue'),
      },
      {
        path: 'agentsList',
        name: 'agentsList',
        component: () => import('../views/AgentsList.vue'),
      },
      {
        path: 'membersList',
        name: 'membersList',
        component: () => import('../views/MembersList.vue'),
      },
      {
        path: 'productsList',
        name: 'productsList',
        component: () => import('../views/ProductsList.vue'),
      },
      {
        path: 'purchaseHistory',
        name: 'purchaseHistory',
        component: () => import('../views/PurchaseHistory.vue'),
      },
    ],
  },
];

const router = new VueRouter({
  routes,
});

router.beforeEach(async (to, from, next) => {
  const cookie = Cookies.get('admin_account');

  if (to.matched.some((record) => record.meta.isAuthRequired)) {
    if (cookie) {
      next();
    } else {
      // cookie沒東西, 導到登入頁
      next({ name: 'login' });
    }
  } else if (to.name === 'login' && cookie) {
    // 已登入狀態進入login會被導回首頁
    next({ name: 'home' });
  } else {
    next();
  }
});

export default router;
