<template>
  <div class="layout flex">
    <Menu />

    <div class="content w-full bg-grey1 ml-[210px] min-h-screen">
      <header class="h-14 bg-white border-b border-grey2 flex justify-end px-2">
        <div class="imgContainer flex justify-center items-center px-2">
          <img src="https://picsum.photos/200" class="w-8 h-8 rounded-full" />
        </div>
        <div class="userAccount flex items-center px-2">
          <div class="txt">{{ user.account || "joechen" }}</div>
        </div>
      </header>
      <div class="main">
        <router-view />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useUserStore } from "@/stores/user";
import Menu from "../components/Menu.vue";

const user = useUserStore();
</script>


<style>
</style>
