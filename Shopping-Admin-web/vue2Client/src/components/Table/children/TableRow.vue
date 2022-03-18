<template>
  <tr class="tableRow">
    <td v-for="(cell, index) in row" :key="index">
      <div class="layout flex" v-if="cell.key === 'operation'">
        <div class="btnContainer">
          <BtnPrimary
            :disabled="isDisabled('edit')"
            v-if="isEditBtnRequired"
            :label="$t('common.edit')"
            @submit="editHandler"
          />
        </div>
        <div class="btnContainer">
          <BtnPrimary
            :disabled="isDisabled('unlock')"
            v-if="isUnlockBtnRequired"
            :label="$t('common.unlock')"
            @submit="unlockHandler"
          />
        </div>
      </div>

      {{ contentFilter(cell) }}
    </td>
  </tr>
</template>

<script>
import { DateTime } from "luxon";
import BtnPrimary from "../../BtnPrimary.vue";
export default {
  name: "tableRow",
  components: {
    BtnPrimary,
  },
  props: {
    row: {
      required: true,
      type: Array,
    },
    isEditBtnRequired: {
      required: false,
      default: true,
    },
    isUnlockBtnRequired: {
      required: false,
      default: true,
    },
    diabledList: {
      required: true,
      type: Array,
    },
  },
  methods: {
    isDisabled(btnType) {
      return this.diabledList.some((el) => el === btnType);
    },
    unlockHandler() {
      this.$emit("unlock", this.row);
    },
    editHandler() {
      this.$emit("edit", this.row);
    },
    contentFilter(payload) {
      const { key, value } = payload;
      switch (key) {
        case "enabled": {
          return this.enabledTransferer(payload);
        }
        case "createdDate":
        case "updatedDate": {
          return this.dateFormatter(payload);
        }
        case "operation": {
          break;
        }
        default:
          return value;
      }
    },
    // 多語系啟、禁用
    enabledTransferer(payload) {
      const { key, value } = payload;
      if (key === "enabled" && typeof value === "number") {
        return value === 1
          ? this.$t(`common.tableHeader.enabled`)
          : this.$t(`common.tableHeader.disabled`);
      }
      return value;
    },
    // 日期格式化
    dateFormatter(payload) {
      const date = new Date(payload.value);
      const dt = DateTime.fromJSDate(date);
      return dt.toFormat("yyyy/MM/dd HH:mm:ss");
    },
  },
};
</script>

<style lang="scss">
.tableRow {
  @apply even:bg-gray-300 hover:bg-gray-400 even:border-b odd:bg-white border-gray-400 p-2;
  td {
    @apply p-3 text-left;
    .btnContainer {
      @apply mr-2;
    }
  }
}
</style>
