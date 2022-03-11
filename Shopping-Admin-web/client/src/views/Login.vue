<template>
  <div class="login flex justify-center items-center w-screen h-screen bg-green2">
    <div class="w-full max-w-xs">
      <div class="bg-transparent">
        <div class="title mb-4 flex justify-center">
          <div class="txt text-white text-3xl font-bold">登入</div>
        </div>
        <div class="mb-4">
          <label class="block text-white text-sm font-bold mb-2" for="username">帳號</label>
          <input
            v-model="account"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:border-sky-500 focus:ring-1 focus:ring-sky-500"
            id="username"
            type="text"
            placeholder="帳號"
          />
        </div>
        <div class="mb-6">
          <label class="block text-white text-sm font-bold mb-2" for="pwd">密碼</label>
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


<script lang="ts">
import { defineComponent, ref } from '@vue/composition-api'
import router from "@/router";
import { useUserStore } from "@/stores/user";
import { loginAgent } from "../APIs/Agents";
import BtnLogin from '../components/BtnSubmit.vue'

export default defineComponent({
  components: {
    BtnLogin
  },
  setup() {
    const account = ref("")
    const pwd = ref("");
    const user = useUserStore();

    const LoginHandler = async () => {
      if (!account.value.length || !pwd.value.length) {
        return alert("帳號密碼不可空白")
      }
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
      } else {
        alert("帳號密碼有誤")
      }
    };

    return {
      account, pwd, LoginHandler
    }
  },
})
</script>



<style>
</style>
