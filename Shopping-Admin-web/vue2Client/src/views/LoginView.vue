<template>
  <div
    class="login flex justify-center items-center w-screen h-screen bg-green2"
  >
    <div class="w-full max-w-xs">
      <div class="bg-transparent">
        <div class="title mb-4 flex justify-center">
          <div class="txt text-white text-3xl font-bold">登入</div>
        </div>
        <div class="mb-4">
          <label class="block text-white text-sm font-bold mb-2" for="username"
            >帳號</label
          >
          <input
            v-model="account"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:border-sky-500 focus:ring-1 focus:ring-sky-500"
            id="username"
            type="text"
            placeholder="帳號"
          />
        </div>
        <div class="mb-6">
          <label class="block text-white text-sm font-bold mb-2" for="pwd"
            >密碼</label
          >
          <input
            v-model="pwd"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 mb-3 leading-tight focus:outline-none focus:border-sky-500 focus:ring-1 focus:ring-sky-500"
            id="pwd"
            type="password"
            placeholder="密碼"
          />
          <!-- <p class="text-red-500 text-xs italic">Please choose a password.</p> -->
        </div>
        <div class="flex items-center justify-center">
          <BtnLogin label="登入" @submit="LoginHandler" />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import BtnLogin from "@/components/BtnPrimary.vue";
import { LoginAgent } from "../APIs/Agent";
export default {
  name: "loginView",
  components: {
    BtnLogin,
  },
  data: () => {
    return {
      account: "",
      pwd: "",
    };
  },
  methods: {
    async LoginHandler() {
      if (!this.account.length || !this.pwd.length) {
        return alert("帳號密碼不可空白");
      }
      const res = await LoginAgent({
        account: this.account,
        pwd: this.pwd,
      });

      if (res.code === 200) {
        this.$store.commit("user/setUser", {
          account: res.data.account,
          role: res.data.role,
        });

        this.$router.push({ name: "home" });
      } else {
        alert("帳號密碼有誤");
      }
    },
  },
};
</script>

<style></style>
