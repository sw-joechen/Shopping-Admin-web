import Vue from 'vue'
import VueRouter from 'vue-router'
import HomeView from '../views/HomeView.vue'
import Cookies from 'js-cookie';

Vue.use(VueRouter)

const router = new VueRouter({
    mode: 'history',
    base: import.meta.env.BASE_URL,
    routes: [
        {
            path: '/login',
            name: 'login',
            component: () => import('../views/Login.vue')
        },
        {
            path: '/',
            name: 'home',
            component: HomeView,
            meta: {
                isAuthRequired: true
            },
            redirect: '/welcome',
            children: [
                {
                    path: 'welcome',
                    name: 'welcome',
                    // route level code-splitting
                    // this generates a separate chunk (About.[hash].js) for this route
                    // which is lazy-loaded when the route is visited.
                    component: () => import('../views/Welcome.vue')
                },
                {
                    path: 'agentsList',
                    name: 'agentsList',
                    // route level code-splitting
                    // this generates a separate chunk (About.[hash].js) for this route
                    // which is lazy-loaded when the route is visited.
                    component: () => import('../views/AgentsList.vue')
                },
                {
                    path: 'membersList',
                    name: 'membersList',
                    // route level code-splitting
                    // this generates a separate chunk (About.[hash].js) for this route
                    // which is lazy-loaded when the route is visited.
                    component: () => import('../views/MembersList.vue')
                },
            ]
        },
    ]
})

router.beforeEach(async (to, from, next) => {
    // 從cookie撈使用者資料, 若有的話做自動登入
    // TODO: 變成取token
    const cookie = Cookies.get('admin_account')

    if (to.matched.some(record => record.meta.isAuthRequired)) {
        if (cookie) {
            next()
        } else {
            // cookie沒東西, 導到登入頁
            next({ name: 'login' })
        }
    } else if (to.name === 'login' && cookie) {
        // 已登入狀態進入login會被導回首頁
        next({ name: 'home' })
    } else {
        next()
    }
})

export default router
