<template>
    <div class="login flex justify-center">
        <div class="w-full max-w-xs">
            <form class="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4">
                <div class="mb-4">
                    <label class="block text-gray-700 text-sm font-bold mb-2" for="username">
                        帳號
                    </label>
                    <input class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:border-sky-500 focus:ring-1 focus:ring-sky-500" id="username" type="text" placeholder="帳號">
                </div>
                <div class="mb-6">
                    <label class="block text-gray-700 text-sm font-bold mb-2" for="pwd">
                        密碼
                    </label>
                    <input class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 mb-3 leading-tight focus:outline-none focus:border-sky-500 focus:ring-1 focus:ring-sky-500" id="pwd" type="password" placeholder="密碼">
                    <!-- <p class="text-red-500 text-xs italic">Please choose a password.</p> -->
                </div>
                <div class="flex items-center justify-between">
                    <button class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:border-sky-500 focus:ring-1 focus:ring-sky-500" type="button">
                        登入
                    </button>
                </div>
            </form>
        </div>
    </div>
    </template>

<script setup lang="ts">
import { ref } from "@vue/composition-api";

const account = ref()
const pwd = ref()

const loginHandler= async ()=>{

}

</script>


<style>
</style>
