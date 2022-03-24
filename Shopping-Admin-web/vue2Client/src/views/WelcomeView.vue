<template>
  <div class="welcome">
    <div class="txt text-3xl font-bold">
      Welcome {{ $store.state.user.account }}
    </div>
  </div>
</template>

<script>
export default {
  name: 'WelcomeView',
};
</script>

<style></style>
