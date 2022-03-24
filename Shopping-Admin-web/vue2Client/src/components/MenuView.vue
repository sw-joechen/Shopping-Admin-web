<template>
  <div
    class="menu overflow-auto fixed flex flex-col border-t border-r border-grey2 w-[210px] h-screen py-10"
  >
    <router-link
      v-for="(el, index) in routes"
      :key="index"
      :to="el.to"
      class="h-14 leading-[56px] text-center text-green2 font-bold text-lg hover:text-white hover:bg-green1 transition-colors duration-300"
      :class="{ 'bg-green1 !text-white': isCurrentRouteActive(el.to.name) }"
    >
      {{ el.name }}
    </router-link>
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
    isCurrentRouteActive(route) {
      if (route === this.$route.name) {
        return true;
      }
      return false;
    },
  },
};
</script>

<style></style>
