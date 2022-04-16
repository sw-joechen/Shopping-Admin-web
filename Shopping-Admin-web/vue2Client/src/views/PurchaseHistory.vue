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

import { GetMemberPurchaseHistory } from '@/APIs/Member';
// import { GetProductsList } from '@/APIs/Product';
import ErrorCodeList from '@/ErrorCodeList';
export default {
  name: 'purchaseHistory',
  components: {
    BtnSubmit,
    TableView,
  },
  data() {
    return {
      queryData: {
        account: '',
        startDate: '',
        dueDate: '',
      },
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
  methods: {
    async SearchHandler() {
      const res = await GetMemberPurchaseHistory({
        account: this.queryData.account,
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
