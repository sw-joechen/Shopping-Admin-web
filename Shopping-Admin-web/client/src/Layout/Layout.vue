<template>
  <div class="layout flex">
    <Menu />

    <div class="content w-full bg-grey1 ml-[210px] min-h-screen">
      <header class="h-14 bg-white border-b border-grey2 flex justify-end px-2">
        <div class="wrapper flex">
          <div class="imgContainer flex justify-center items-center px-2">
            <img src="https://picsum.photos/200" class="w-8 h-8 rounded-full" />
          </div>
          <div class="userAccount flex items-center px-2">
            <div class="txt">{{ user.account }}</div>
          </div>
          <div class="logoutContainer flex items-center px-2">
            <LogOutBtn label="登出" @submit="toggleHandler" />
          </div>
        </div>
      </header>
      <div class="main">
        <ConfirmDialog
          :is-show-dialog="isShowLogOutDialog"
          title="確定要登出嗎"
          @toggle="toggleHandler"
          @submit="logOutHandler"
        ></ConfirmDialog>
        <router-view />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useUserStore } from "@/stores/user";
import Menu from "../components/Menu.vue";
import LogOutBtn from '../components/BtnSubmit.vue'
import ConfirmDialog from '../components/Dialogs/FormDialog.vue'
// import router from "@/router";
const user = useUserStore()

let isShowLogOutDialog = $ref(false)
const toggleHandler = () => {
  isShowLogOutDialog = !isShowLogOutDialog
}

const logOutHandler = () => {
  toggleHandler()
  user.clearUser()

  // router.push({ name: 'login' })
  location.replace(`${window.location.origin}`)
}
</script>


<style>
</style>
