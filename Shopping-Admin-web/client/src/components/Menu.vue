<template>
  <div
    class="
      menu
      overflow-auto
      fixed
      flex flex-col
      border-t border-r border-grey2
      w-[210px]
      h-screen
      py-10
    "
  >
    <router-link
      v-for="(el, index) in routes"
      :key="index"
      :to="el.to"
      class="
        h-14
        leading-[56px]
        text-center text-green2
        font-bold
        text-lg
        hover:text-white hover:bg-green1
        transition-colors
        duration-300
      "
    >
      {{ el.name }}
    </router-link>
  </div>
</template>

<script setup lang="ts">
import { ref } from "@vue/composition-api";

const routes = ref([
  { name: "首頁", to: { name: "home" } },
  { name: "後台帳號管理", to: { name: "agentsList" } },
  { name: "用戶帳號管理", to: { name: "membersList" } },
]);
</script>

<style>
</style>
