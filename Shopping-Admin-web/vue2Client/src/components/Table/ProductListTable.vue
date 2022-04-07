<template>
  <div class="productListTable mt-2">
    <table class="w-full">
      <thead>
        <tr
          class="text-left border-t border-b-2 border-grey2 p-2 bg-blue-400 text-white text-xl;"
        >
          <th
            class="py-3 px-1 min-w-[120px]"
            :class="
              col === 'name' || col === 'description'
                ? 'text-left'
                : 'text-center'
            "
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

          <td class="name bodyTd">
            <div class="max-w-xs text-left">
              <span class="break-all">
                {{ item.name }}
              </span>
            </div>
          </td>

          <td class="description bodyTd">
            <div class="max-w-xs text-left">
              <span class="break-all">
                {{ item.description }}
              </span>
            </div>
          </td>

          <td class="price bodyTd">
            <div class="max-w-xs text-center">
              <span class="break-all">
                {{ item.price }}
              </span>
            </div>
          </td>

          <td class="picture bodyTd">
            <div class="max-w-xs text-center">
              <div class="imgContainer flex justify-center">
                <img class="w-16" :src="item.picture" />
              </div>
            </div>
          </td>

          <td class="amount bodyTd">
            <div class="max-w-xs text-center">
              <span class="break-all">
                {{ item.amount }}
              </span>
            </div>
          </td>

          <td class="type bodyTd">
            <div class="max-w-xs text-center">
              <span class="break-all">
                {{ item.type }}
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

          <td class="updatedDate bodyTd">
            <div class="max-w-xs text-center">
              <span class="break-all">
                {{ DateFormatter(item.updatedDate) }}
              </span>
            </div>
          </td>

          <td class="opertation bodyTd py-2 min-w-[150px]">
            <div class="max-w-xs flex justify-center">
              <BtnPrimary
                class="mr-2"
                :label="$t('common.edit')"
                @submit="EditHandler(item)"
              />

              <BtnPrimary
                class="mr-2"
                :label="$t('common.delete')"
                theme="red"
                @submit="DeleteHandler(item.id)"
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
import BtnPrimary from '@/components/BtnPrimary.vue';
import { DelProduct } from '@/APIs/Product';
import ErrorCodeList from '@/ErrorCodeList';
export default {
  name: 'productListTable',
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
        'name',
        'description',
        'price',
        'picture',
        'amount',
        'type',
        'enabled',
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
    EditHandler(payload) {
      this.$emit('edit', payload);
    },
    async DeleteHandler(targetID) {
      const result = confirm('確定要刪除商品嗎');

      if (result) {
        const fd = new FormData();
        fd.append('id', targetID);
        const res = await DelProduct(fd);
        if (res.code === 200) {
          this.$emit('delComplete');
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
      }
    },
  },
};
</script>

<style></style>
