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
                {{ GenderFormatter(item.gender) }}
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

          <td class="enabled bodyTd">
            <div class="max-w-xs flex justify-center">
              <SwitchView :toggle="item.enabled" @toggle="EditHandler(item)" />
            </div>
          </td>

          <td class="deposit bodyTd">
            <div class="max-w-xs flex justify-center">
              <BtnSubmit
                :label="$t('common.tableHeader.deposit')"
                @submit="EditDepositHandler(item)"
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
import SwitchView from '@/components/SwtichView.vue';
import { UpdateMember } from '@/APIs/Member';
import ErrorCodeList from '@/ErrorCodeList';
import BtnSubmit from '@/components/BtnPrimary.vue';
export default {
  name: 'memberListTable',
  components: {
    SwitchView,
    BtnSubmit,
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
        'address',
        'phone',
        'gender',
        'email',
        'balance',
        'updatedDate',
        'enabled',
        'deposit',
      ],
    };
  },
  methods: {
    keypressHandler(event) {
      return event.charCode >= 48 && event.charCode <= 57
        ? true
        : event.preventDefault();
    },
    GenderFormatter(inputGender) {
      if (inputGender === 0) {
        return this.$t('memberList.female');
      } else {
        return this.$t('memberList.male');
      }
    },
    // 將日期格式化
    DateFormatter(inputDate) {
      const date = new Date(inputDate);
      const dt = DateTime.fromJSDate(date);
      return dt.toFormat('yyyy/MM/dd HH:mm:ss');
    },

    async EditHandler(payload) {
      const fd = new FormData();
      fd.append('account', payload.account);
      fd.append('enabled', !payload.enabled);
      const res = await UpdateMember(fd);
      if (res.code === 200) {
        this.$emit('editComplete');

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

    async EditDepositHandler(payload) {
      this.$emit('editDeposit', payload);
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
