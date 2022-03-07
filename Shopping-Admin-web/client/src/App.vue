<script setup lang="ts">
import HelloWorld from '@/components/HelloWorld.vue'
</script>

<template>
  <div id="app">
    <header>
      <div class="wrapper">
        <nav>
          <router-link to="/">Home</router-link>
          <span class="select-none"> | </span>
          <router-link to="/about">About</router-link>
          <span class="select-none"> | </span>
          <router-link to="/login">Login</router-link>
        </nav>
      </div>
    </header>

    <router-view />
  </div>
</template>

<style>
@import '@/assets/base.css';
</style>
