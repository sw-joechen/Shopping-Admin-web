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
import { GetProductsList } from '@/APIs/Product';
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
    // {"orderNumber":"22041307454400001011","account":"test001","phone":"0987654321","address":"地址","createdDate":"2022/4/13 上午 07:45:44","shoppingList":[{"id":14,"count":1},{"id":15,"count":1},{"id":17,"count":1}]}[]
    async SearchHandler() {
      let tempPurchaseHistories = [];
      const res = await GetMemberPurchaseHistory({
        account: this.queryData.account,
      });
      if (res.code === 200) {
        tempPurchaseHistories = res.data;
      } else {
        return this.$store.commit('eventBus/Push', {
          type: 'error',
          content: ErrorCodeList[res.code],
        });
      }

      console.log(
        'tempPurchaseHistories=> ',
        JSON.parse(JSON.stringify(tempPurchaseHistories))
      );

      // 取得商品列表
      // TODO: 改成呼叫批次指定商品id的接口
      let tempProductList = [];
      const response = await GetProductsList();
      if (response.code === 200) {
        tempProductList = response.data;
      } else {
        return this.$store.commit('eventBus/Push', {
          type: 'error',
          content: ErrorCodeList[response.code],
        });
      }

      // 迭代個人歷史訂單的每個商品, 找出每個商品的完整資訊
      tempPurchaseHistories.forEach((history) => {
        let productIncludingDetailList = [];
        history.shoppingList.forEach((product) => {
          const result = tempProductList.find((el) => el.id === product.id);
          if (result) {
            productIncludingDetailList.push({
              id: result.id,
              name: result.name,
              price: result.price,
              count: product.count,
            });
          }
        });
        history.shoppingList = productIncludingDetailList;
      });
      this.purchaseHistories = JSON.parse(
        JSON.stringify(tempPurchaseHistories)
      );
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
