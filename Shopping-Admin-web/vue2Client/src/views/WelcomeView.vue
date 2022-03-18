<template>
  <div class="welcome">
    <div class="txt text-3xl font-bold">
      Welcome {{ $store.state.user || "" }}
      <button class="border border-red-600" @click="clickHandler">
        click me
      </button>
    </div>
  </div>
</template>

<script>
export default {
  name: "WelcomeView",
  methods: {
    clickHandler() {
      this.$store.commit("eventBus/Push", {
        type: "success",
        content: "測試成功",
      });
    },
  },
};
</script>

<style></style>
