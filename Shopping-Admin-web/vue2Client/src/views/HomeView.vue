<template>
  <div class="home">
    <img alt="Vue logo" src="../assets/logo.png" />
    <span>中文字測試</span>
    <HelloWorld msg="Welcome to Your Vue.js App" />
  </div>
</template>

<script>
// @ is an alias to /src
import HelloWorld from "@/components/HelloWorld.vue";

export default {
  name: "HomeView",
  components: {
    HelloWorld,
  },
  methods: {
    demo() {
      console.log("eqwewq");
    },
  },
};
</script>
