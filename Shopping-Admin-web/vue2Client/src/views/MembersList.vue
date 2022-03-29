<template>
  <div class="membersList p-5">
    <!-- input query -->
    <div class="query flex">
      <!-- 會員帳號 -->
      <div class="inputGroup flex">
        <label class="whitespace-nowrap pr-3 leading-[42px]">
          {{ $t('common.tableHeader.account') }}
        </label>
        <input type="text" class="input" v-model="queryData.account" />
      </div>

      <!-- 啟用/禁用 -->
      <OptionSelector
        class="px-2"
        :options="options"
        :value.sync="queryData.enabled"
      />
      <BtnSubmit
        :label="$t('common.search')"
        class="pr-2"
        @submit="SearchHandler"
      />
    </div>

    <hr class="mb-2" />

    <TableView :datas="memberList" @edit="EditHandler" />
  </div>
</template>

<script>
import BtnSubmit from '@/components/BtnPrimary.vue';
import OptionSelector from '@/components/OptionSelector.vue';
import TableView from '@/components/Table/MemberListTable.vue';

import { GetMembersList } from '@/APIs/Member';
import ErrorCodeList from '@/ErrorCodeList';

const EOptions = {
  all: 0,
  enabled: 1,
  disabled: 2,
};

export default {
  name: 'membersList',
  components: {
    BtnSubmit,
    OptionSelector,
    TableView,
  },
  data: () => {
    return {
      options: [
        {
          label: '全部',
          value: EOptions.all,
        },
        {
          label: '啟用',
          value: EOptions.enabled,
        },
        {
          label: '禁用',
          value: EOptions.disabled,
        },
      ],
      queryData: {
        account: '',
        enabled: 0,
      },
      memberList: [],
    };
  },
  created() {
    this.SearchHandler();
  },
  methods: {
    async SearchHandler() {
      const fd = new FormData();
      if (this.queryData.enabled !== 0) {
        fd.append('enabled', this.queryData.enabled === EOptions.enabled);
      }

      if (this.queryData.account.length) {
        fd.append('account', this.queryData.account);
      }

      const res = await GetMembersList(fd);
      if (res.code === 200) {
        this.memberList = res.data;
      } else {
        this.memberList = [];
        this.$store.commit('eventBus/Push', {
          type: 'error',
          content: ErrorCodeList[res.code],
        });
      }
    },
    EditHandler(payload) {
      const target = this.memberList.find((member) => member.id === payload.id);
      if (target) {
        target.enabled = payload.enabled;
      }
    },
  },
};
</script>

<style lang="scss">
.membersList {
  .inputGroup {
    @apply mr-3 pb-5;
    .input {
      @apply bg-white border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:ring-1 focus:border-blue-500 block w-full p-2.5 outline-none;
    }
  }
}
</style>
