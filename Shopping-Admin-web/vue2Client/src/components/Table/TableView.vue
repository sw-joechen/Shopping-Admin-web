<template>
  <div class="agentsTable pt-5">
    <table class="w-full">
      <thead>
        <tr
          class="text-left border-t border-b-2 border-grey2 p-2 bg-blue-400 text-white text-xl;"
        >
          <th
            class="py-3 px-1 min-w-[120px] text-center"
            scope="col"
            v-for="col in columns"
            :key="col"
          >
            {{ $t(`common.tableHeader.${col}`) }}
          </th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="(item, index) in datas"
          :key="index"
          class="even:bg-gray-300 hover:bg-gray-400 even:border-b odd:bg-white border-gray-400 p-2"
        >
          <td class="id bodyTd">
            <div class="max-w-xs text-center">
              <span class="break-all">
                {{ item.id }}
              </span>
            </div>
          </td>

          <td class="account bodyTd">
            <div class="max-w-xs text-center">
              <span class="break-all">
                {{ item.account }}
              </span>
            </div>
          </td>

          <td class="enabled bodyTd">
            <div class="max-w-xs text-center">
              <span class="break-all">
                {{
                  item.enabled
                    ? $t('common.tableHeader.enabled')
                    : $t('common.tableHeader.disabled')
                }}
              </span>
            </div>
          </td>

          <td class="createdDate bodyTd">
            <div class="max-w-xs text-center">
              <span class="break-all">
                {{ DateFormatter(item.createdDate) }}
              </span>
            </div>
          </td>

          <td class="updatedDate bodyTd">
            <div class="max-w-xs text-center">
              <span class="break-all">
                {{ DateFormatter(item.updatedDate) }}
              </span>
            </div>
          </td>

          <td class="count bodyTd">
            <div class="max-w-xs text-center">
              <BtnPrimary
                v-if="item.count >= 3"
                :label="$t('common.unlock')"
                @submit="UnlockHandler(item)"
              />
            </div>
          </td>

          <td class="opertation bodyTd">
            <div class="max-w-xs flex justify-center">
              <BtnPrimary
                class="mr-2"
                :label="$t('common.edit')"
                @submit="EditHandler(item)"
              />
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import { DateTime } from 'luxon';
import { UnlockAgent } from '@/APIs/Agent';
import errorList from '@/ErrorCodeList';
import BtnPrimary from '../BtnPrimary.vue';

export default {
  name: 'tableView',
  components: {
    BtnPrimary,
  },
  props: {
    datas: {
      required: true,
      type: Array,
      // default: () => [],
    },
  },
  data: () => {
    return {
      columns: [
        'id',
        'account',
        'enabled',
        'createdDate',
        'updatedDate',
        'count',
        'operation',
      ],
    };
  },
  methods: {
    // 將日期格式化
    DateFormatter(inputDate) {
      const date = new Date(inputDate);
      const dt = DateTime.fromJSDate(date);
      return dt.toFormat('yyyy/MM/dd HH:mm:ss');
    },

    async UnlockHandler(payload) {
      const res = await UnlockAgent({
        account: payload.account,
      });
      if (res.code === 200) {
        this.$emit('unlockCompleted', {
          account: payload.account,
        });
        this.$store.commit('eventBus/Push', {
          type: 'success',
          content: '解鎖成功',
        });
      } else {
        this.$store.commit('eventBus/Push', {
          type: 'error',
          content: errorList[res.code],
        });
      }
    },

    EditHandler(payload) {
      this.$emit('edit', payload);
    },
  },
};
</script>

<style lang="scss">
.agentsTable {
  .bodyTd {
    @apply py-3 px-1 text-left;
  }
}
</style>
