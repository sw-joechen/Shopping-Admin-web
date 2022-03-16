<template>
  <div
    :class="GetNotifyBg"
    class="notifyView rounded-lg text-white pointer-events-auto ml-auto mb-4 py-[22px] px-6 min-w-[326px] min-h-[64px]"
  >
    {{ notify.content }}
  </div>
</template>

<script>
const DISPLAY_DURATION = 2000;
export default {
  name: "notifyView",
  props: {
    notify: {
      required: true,
    },
  },
  created() {
    setTimeout(() => {
      this.$emit("close", this.notify.id);
    }, DISPLAY_DURATION);
  },
  computed: {
    GetNotifyBg() {
      return this.notify.type === "success" ? "bg-green4" : "bg-red1";
    },
  },
};
</script>

<style></style>
