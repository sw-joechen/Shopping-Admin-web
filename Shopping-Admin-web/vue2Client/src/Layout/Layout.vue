<template>
  <div class="layout flex">
    <Menu />

    <div
      class="content w-full bg-grey1 ml-[210px] min-h-screen overflow-x-scroll"
    >
      <header class="h-14 bg-white border-b border-grey2 flex justify-end px-2">
        <div class="wrapper flex">
          <div class="imgContainer flex justify-center items-center px-2">
            <img src="https://picsum.photos/200" class="w-8 h-8 rounded-full" />
          </div>
          <div class="userAccount flex items-center px-2">
            <div class="txt">{{ $store.state.user.account }}</div>
          </div>
          <div class="logoutContainer flex items-center px-2">
            <LogOutBtn label="登出" @submit="ToggleHandler" />
          </div>
        </div>
      </header>
      <div class="main">
        <ConfirmDialog
          v-if="isShowLogOutDialog"
          :is-show-dialog="isShowLogOutDialog"
          title="確定要登出嗎"
          @toggle="ToggleHandler"
          @submit="LogOutHandler"
        />
        <router-view />
      </div>
    </div>
  </div>
</template>

<script>
import Menu from '@/components/MenuView.vue';
import LogOutBtn from '@/components/BtnPrimary.vue';
import ConfirmDialog from '@/components/Dialogs/DialogView.vue';
export default {
  name: 'LayoutView',
  components: {
    Menu,
    LogOutBtn,
    ConfirmDialog,
  },
  data: () => {
    return {
      isShowLogOutDialog: false,
    };
  },
  methods: {
    ToggleHandler() {
      this.isShowLogOutDialog = !this.isShowLogOutDialog;
    },
    LogOutHandler() {
      this.ToggleHandler();
      this.$store.commit('user/clearUser');
      this.$router.replace({ name: 'login' });
    },
  },
};
</script>

<style></style>
