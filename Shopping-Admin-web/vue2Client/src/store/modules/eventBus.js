const state = () => ({
  notificationList: [],
});

const getters = {
  NotificationListGetter(state) {
    return state.notificationList;
  },
};

const actions = {};

const mutations = {
  // payload:{type:"success" | "error", content:string}
  Push(state, payload) {
    state.notificationList.push(payload);
  },
  Shift(state) {
    const result = state.shift();
    return result;
  },
};

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
};
