<template>
  <div class="memberListTable">
    <table class="w-full pt-5">
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

          <td class="address bodyTd">
            <div class="max-w-xs text-center">
              <span class="break-all">
                {{ item.address }}
              </span>
            </div>
          </td>

          <td class="phone bodyTd">
            <div class="max-w-xs text-center">
              <span class="break-all">
                {{ item.phone }}
              </span>
            </div>
          </td>

          <td class="gender bodyTd">
            <div class="max-w-xs text-center">
              <span class="break-all">
                {{ item.gender }}
              </span>
            </div>
          </td>

          <td class="email bodyTd">
            <div class="max-w-xs text-center">
              <span class="break-all">
                {{ item.email }}
              </span>
            </div>
          </td>

          <td class="balance bodyTd">
            <div class="max-w-xs text-center">
              <span class="break-all">
                {{ item.balance }}
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

          <td class="opertation bodyTd">
            <div class="max-w-xs flex justify-center">
              <BtnPrimary
                class="mr-2"
                :label="
                  item.enabled
                    ? $t('common.tableHeader.disabled')
                    : $t('common.tableHeader.enabled')
                "
                :theme="item.enabled ? 'red' : 'green'"
                @submit="EditHandler(item)"
              />
            </div>
          </td>

          <!-- <td class="createdDate bodyTd">
            <div class="max-w-xs text-center">
              <span class="break-all">
                {{ DateFormatter(item.createdDate) }}
              </span>
            </div>
          </td> -->
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import { DateTime } from 'luxon';
import BtnPrimary from '@/components/BtnPrimary.vue';
import { UpdateMember } from '@/APIs/Member';
import ErrorCodeList from '@/ErrorCodeList';
export default {
  name: 'memberListTable',
  components: {
    BtnPrimary,
  },
  props: {
    datas: {
      required: true,
      type: Array,
    },
  },
  data: () => {
    return {
      columns: [
        'id',
        'account',
        'enabled',
        'address',
        'phone',
        'gender',
        'email',
        'balance',
        'updatedDate',
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

    async EditHandler(payload) {
      const fd = new FormData();
      fd.append('id', payload.id);
      fd.append('account', payload.account);
      fd.append('address', payload.address);
      fd.append('phone', payload.phone);
      fd.append('gender', payload.gender);
      fd.append('email', payload.email);
      fd.append('enabled', !payload.enabled);
      const res = await UpdateMember(fd);
      if (res.code === 200) {
        this.$emit('edit', res.data);

        this.$store.commit('eventBus/Push', {
          type: 'success',
          content: this.$t('common.success'),
        });
      } else {
        this.$store.commit('eventBus/Push', {
          type: 'error',
          content: ErrorCodeList[res.code],
        });
      }
    },
  },
};
</script>

<style lang="scss">
.memberListTable {
  .bodyTd {
    @apply py-3 px-1 text-left;
  }
}
</style>
