<template>
  <tr class="tableRow">
    <td v-for="(cell, index) in row" :key="index">
      <!-- {{ enabledTransferer(cell) }} -->
      {{ cell }}
    </td>
  </tr>
</template>

<script>
export default {
  name: "tableRow",
  props: {
    row: {
      required: true,
      type: Array,
    },
  },
  methods: {
    // enabledTransferer(payload) {
    //   const { key, value } = payload;
    //   if (key === "enabled" && typeof value === "number") {
    //     return value === 1
    //       ? this.$t(`agentList.table.enabled`)
    //       : this.$t(`agentList.table.disabled`);
    //   }
    //   return value;
    // },
  },
};
</script>

<style lang="scss">
.tableRow {
  @apply even:bg-gray-300 hover:bg-gray-400 even:border-b odd:bg-white border-gray-400 p-2;
  td {
    @apply p-3;
  }
}
</style>
