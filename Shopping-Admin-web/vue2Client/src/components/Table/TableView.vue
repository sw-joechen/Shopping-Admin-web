<template>
  <div class="tableView w-full">
    <table class="w-full">
      <TableHeader :header="tableHeaders" v-if="tableHeaders" />
      <TableRow v-for="(row, index) in tableRows" :key="index" :row="row" />
    </table>
  </div>
</template>

<script>
import TableHeader from "./children/TableHeader.vue";
import TableRow from "./children/TableRow.vue";
import { DateTime } from "luxon";

export default {
  name: "tableView",
  components: {
    TableHeader,
    TableRow,
  },
  props: {
    datas: {
      required: true,
      type: Array,
      // default: () => [],
    },
  },
  computed: {
    tableHeaders() {
      if (this.datas.length) {
        return Object.keys(this.datas[0]).map((header, index) => {
          return {
            index: index,
            key: header,
          };
        });
      }
      return [];
    },
    tableRows() {
      return this.datas.map((obj) => {
        return Object.keys(obj).map((key, idx2) => {
          if (key === "createdDate" || key === "updatedDate") {
            const date = new Date(Object.values(obj)[idx2]);
            const dt = DateTime.fromJSDate(date);

            return {
              key: key,
              value: dt.toFormat("yyyy/MM/dd HH:mm"),
            };
          }
          return {
            key: key,
            value: Object.values(obj)[idx2],
          };
        });
      });
    },
  },
};
</script>

<style></style>
