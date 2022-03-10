<template>
    <tr class="tableRow">
        <td v-for="(cell,index) in row" :key="index">{{ enabledTransferer(cell) }}</td>
    </tr>
</template>

<script setup lang="ts">

const props = defineProps({
    row: {
        required: true,
        type: Array as () => Array<any>
    }
})
</script>

<!-- 特別用來處理i18n -->
<script lang="ts">
import { defineComponent, ref } from '@vue/composition-api'
export default defineComponent({
    methods: {
        enabledTransferer(payload: {
            key: string,
            value: any
        }) {
            const { key, value } = payload
            if (key === "enabled" && typeof value === "number") {
                return value === 1 ? this.$t(`agentList.table.enabled`) : this.$t(`agentList.table.disabled`)
            }
            return value

        }
    }
})
</script>

<style lang="scss">
.tableRow {
    @apply even:bg-gray-300 hover:bg-gray-400 even:border-b odd:bg-white border-gray-400 p-2;
    td {
        @apply p-3;
    }
}
</style>
