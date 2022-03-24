<template>
  <div
    class="switchView flex items-center justify-center w-full cursor-pointer"
  >
    <label class="flex items-center cursor-pointer">
      <!-- toggle -->
      <div class="relative">
        <!-- input -->
        <input
          type="checkbox"
          class="sr-only"
          :value="toggle"
          @click="ClickHandler"
        />
        <!-- line -->
        <div class="w-10 h-4 bg-gray-400 rounded-full shadow-inner"></div>
        <!-- dot -->
        <div
          class="dot absolute w-6 h-6 bg-white rounded-full shadow -left-1 -top-1 transition"
          :class="{ 'transform translate-x-full bg-blue-400': toggle }"
        ></div>
      </div>
    </label>
  </div>
</template>

<script>
export default {
  name: 'swtichView',
  props: {
    toggle: {
      type: Boolean || null,
      required: true,
    },
  },
  methods: {
    ClickHandler() {
      this.$emit('toggle', !this.toggle);
    },
  },
};
</script>

<style></style>
