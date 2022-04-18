<template>
  <div class="purchaseHistory p-5">
    <!-- input query -->
    <div class="query flex">
      <!-- 會員帳號 -->
      <div class="inputGroup flex">
        <label class="whitespace-nowrap pr-3 leading-[42px]">
          {{ $t('common.tableHeader.account') }}
        </label>
        <input
          type="text"
          class="input"
          v-model="queryData.account"
          @keypress.enter="SearchHandler"
        />
      </div>

      <!-- 日期選擇 -->
      <DatePicker v-model="dateTime" range class="datePicker mr-2" />

      <BtnSubmit
        :label="$t('common.search')"
        class="pr-2"
        @submit="SearchHandler"
      />
    </div>

    <hr class="mb-2" />

    <TableView :datas="purchaseHistories" />
  </div>
</template>

<script>
import BtnSubmit from '@/components/BtnPrimary.vue';
import TableView from '@/components/Table/PurchaseHistoryTable.vue';
import DatePicker from 'vue2-datepicker';
import 'vue2-datepicker/index.css';

import { GetMemberPurchaseHistory } from '@/APIs/Member';
import ErrorCodeList from '@/ErrorCodeList';
import { DateTime } from 'luxon';

export default {
  name: 'purchaseHistory',
  components: {
    BtnSubmit,
    TableView,
    DatePicker,
  },
  data() {
    return {
      queryData: {
        account: '',
        startDate: '',
        dueDate: '',
      },
      dateTime: [this.getSevenDaysAgo(), this.getEndOfToday()],
      /**
       * {
       *    orderNumber:string,
       *    account:string,
       *    phone:string,
       *    address:string,
       *    createdDate:string,
       *    shoppingList:{
       *      id:number,
       *      name:string,
       *      price:number
       *      count:number
       *    }[]
       * }[]
       */
      purchaseHistories: [],
    };
  },
  computed: {
    formatStartDateTime() {
      if (!this.dateTime[0]) return null;

      const date = new Date(this.dateTime[0]);
      return DateTime.fromJSDate(date).toUTC().toISO();
    },
    formatEndDateTime() {
      if (!this.dateTime[1]) return null;

      const date = new Date(this.dateTime[1]);
      return DateTime.fromJSDate(date).endOf('day').toUTC().toISO();
    },
  },
  created() {
    this.SearchHandler();
  },
  methods: {
    getEndOfToday() {
      return new Date(new Date().setHours(23, 59, 59, 999));
    },
    getSevenDaysAgo() {
      return new Date(
        new Date(new Date().setDate(new Date().getDate() - 7)).setHours(
          0,
          0,
          0,
          0
        )
      );
    },
    async SearchHandler() {
      const res = await GetMemberPurchaseHistory({
        account: this.queryData.account ? this.queryData.account : null,
        startDate: this.formatStartDateTime,
        dueDate: this.formatEndDateTime,
      });
      if (res.code === 200) {
        this.purchaseHistories = res.data;
      } else {
        return this.$store.commit('eventBus/Push', {
          type: 'error',
          content: ErrorCodeList[res.code],
        });
      }
    },
  },
};
</script>

<style lang="scss">
.purchaseHistory {
  .datePicker {
    & input {
      @apply rounded-lg h-[42px];
    }
  }
  input::-webkit-outer-spin-button,
  input::-webkit-inner-spin-button {
    -webkit-appearance: none;
    margin: 0;
  }
  .inputGroup {
    @apply mr-3 pb-5;
    .label {
      @apply min-w-[60px] text-left;
    }
    .input {
      @apply bg-white border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:ring-1 focus:border-blue-500 block w-full p-2.5 outline-none;
    }
  }
}
</style>
