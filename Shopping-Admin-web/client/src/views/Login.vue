<template>
  <div class="login flex justify-center">
    <div class="w-full max-w-xs">
      <form class="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4">
        <div class="mb-4">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="username">帳號</label>
          <input
            v-model="account"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:border-sky-500 focus:ring-1 focus:ring-sky-500"
            id="username"
            type="text"
            placeholder="帳號"
          />
        </div>
        <div class="mb-6">
          <label class="block text-gray-700 text-sm font-bold mb-2" for="pwd">密碼</label>
          <input
            v-model="pwd"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 mb-3 leading-tight focus:outline-none focus:border-sky-500 focus:ring-1 focus:ring-sky-500"
            id="pwd"
            type="password"
            placeholder="密碼"
          />
          <!-- <p class="text-red-500 text-xs italic">Please choose a password.</p> -->
        </div>
        <div class="flex items-center justify-between">
          <button
            @click="loginHandler"
            class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:border-sky-500 focus:ring-1 focus:ring-sky-500"
            type="button"
          >登入</button>
        </div>
      </form>
    </div>
  </div>
</template>


<script lang="ts">
import { defineComponent, ref } from '@vue/composition-api'
import router from "@/router";
import { useUserStore } from "@/stores/user";
import { loginAgent } from "../APIs/Agents";

export default defineComponent({
  setup() {
    const account = ref("")
    const pwd = ref("");
    const user = useUserStore();

    const loginHandler = async () => {
      const res = await loginAgent({
        account: account.value,
        pwd: pwd.value,
      });

      if (res.code === 200) {
        user.setUser({
          account: res.data.account,
          role: res.data.role,
        });

        router.push({ name: "home" });
      }
    };

    // const registerHandler = async () => {
    //   const res = await axios({
    //     method: "post",
    //     url: "/api/agent/registerAgent",
    //     data: {
    //       account: account.value,
    //       pwd: pwd.value,
    //     },
    //   });
    //   console.log("res=>", res);
    //   if (res.data) {
    //     const data = JSON.parse(res.data);
    //   }
    // };

    return {
      account, pwd, loginHandler
    }
  },
})
</script>



<style>
</style>
