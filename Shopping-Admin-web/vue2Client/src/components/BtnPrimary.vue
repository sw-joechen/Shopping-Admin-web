<template>
  <div class="btnPrimary">
    <button
      @click="ClickHandler"
      :class="getTheme"
      class="text-white font-bold py-2 px-4 rounded"
    >
      {{ label }}
    </button>
  </div>
</template>

<script>
export default {
  props: {
    label: {
      required: true,
      type: String,
    },
    theme: {
      required: false,
      type: String,
    },
  },
  computed: {
    getTheme() {
      switch (this.theme) {
        case 'red': {
          return 'bg-red-500 hover:bg-red-700 focus:ring-4 focus:ring-red-300';
        }
        case 'green': {
          return 'bg-green-500 hover:bg-green-700 focus:ring-4 focus:ring-green-300';
        }
        default: {
          return 'bg-blue-500 hover:bg-blue-700 focus:ring-4 focus:ring-blue-300';
        }
      }
    },
  },
  methods: {
    ClickHandler() {
      this.$emit('submit');
    },
  },
};
</script>

<style></style>
