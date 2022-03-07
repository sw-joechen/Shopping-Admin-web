<template>
  <div class="about">
    <h1>This is an about page</h1>

    <label>account</label>
    <input v-model="account"/>
    <label>pwd</label>
    <input v-model="pwd"/>
    <button @click="registerHandler">registerAgent</button>
    <button @click="loginHandler">loginAgent</button>
  </div>
</template>

<script setup lang="ts">
import { ref } from '@vue/composition-api'
import axios from 'axios'

const account = ref()
const pwd = ref()
  const registerHandler = async()=>{    
    const res = await axios({
        method: 'post',
        url: 'api/member/registerAgent',
        data: {
            account: account.value,
            pwd: pwd.value
        }
    })
    console.log('res=>',res)
    if (res.data) {
        const data = JSON.parse(res.data)
    }
  }

  const loginHandler = async ()=>{
    const res = await axios({
        method: 'post',
        url: 'api/member/loginAgent',
        data: {
            account: account.value,
            pwd: pwd.value
        }
    })
    console.log('res=>',res)
    if (res.data) {
        const data = JSON.parse(res.data)

    }
  }
</script>

<style>
@media (min-width: 1024px) {
  .about {
    min-height: 100vh;
    display: flex;
    align-items: center;
  }
}
</style>
