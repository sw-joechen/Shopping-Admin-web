import Vue from 'vue'
import VueCompositionAPI, { createApp, h } from '@vue/composition-api'
import { createPinia, PiniaVuePlugin } from 'pinia'
import VueI18n from 'vue-i18n'
import './index.css'
import './mockjs/mock'
import { twI18n } from './i18n/tw'
import App from './App.vue'
import router from './router'

Vue.use(VueCompositionAPI)
Vue.use(VueI18n)

const messages = {
  tw: twI18n
}

// Create VueI18n instance with options
const i18n = new VueI18n({
  locale: 'tw', // set locale
  messages, // set locale messages
})

const app = createApp({
  router,
  pinia: createPinia(),
  i18n,
  render: () => h(App)
})
app.use(PiniaVuePlugin)

app.mount('#app')
