<template>
  <div
    class="menu overflow-auto fixed flex flex-col border-t border-r border-grey2 w-[210px] h-screen pb-10"
  >
    <div class="text-green2 font-bold py-2 text-xl">阿進購物管理後台</div>
    <a
      v-for="(el, index) in routes"
      :key="index"
      :to="el.to"
      class="select-none cursor-pointer h-14 leading-[56px] text-center text-green2 font-bold text-lg hover:text-white hover:bg-green1 transition-colors duration-300"
      :class="{ 'bg-green1 !text-white': IsCurrentRouteActive(el.to.name) }"
      @click="ClickHandler(el.to)"
    >
      {{ el.name }}
    </a>
  </div>
</template>

<script>
export default {
  name: 'menuView',
  data: () => {
    return {
      routes: [
        { name: '首頁', to: { name: 'welcome' } },
        { name: '後台帳號管理', to: { name: 'agentsList' } },
        { name: '用戶帳號管理', to: { name: 'membersList' } },
        { name: '商品管理', to: { name: 'productsList' } },
      ],
    };
  },
  methods: {
    IsCurrentRouteActive(route) {
      if (route === this.$route.name) {
        return true;
      }
      return false;
    },
    ClickHandler(to) {
      if (to.name === this.$route.name) {
        this.$store.commit('setForceInit', true);
      }
      this.$router
        .push({
          name: to.name,
          params: {
            timestamp: new Date(),
          },
        })
        .catch(() => {});
    },
  },
};
</script>

<style></style>
