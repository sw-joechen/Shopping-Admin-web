import Vue from 'vue';
import Vuex from 'vuex';
import user from './modules/user';
import eventBus from './modules/eventBus';

Vue.use(Vuex);

export default new Vuex.Store({
  state: () => {
    return {
      forceInit: false,
    };
  },
  mutations: {
    setForceInit(state, value) {
      state.forceInit = value;
    },
  },
  modules: {
    user,
    eventBus,
  },
});
