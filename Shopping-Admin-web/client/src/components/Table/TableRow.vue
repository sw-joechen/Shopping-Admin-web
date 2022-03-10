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

const enabledTransferer = (payload: {
    key: string,
    value: any
}) => {
    const { key, value } = payload
    if (key === "enabled" && typeof value === "number") {
        return value === 1 ? 'enalbed' : 'disabled'
    }
    return value
}
</script>

<style lang="scss">
.tableRow {
    @apply even:bg-gray-300 hover:bg-gray-400 even:border-b odd:bg-white border-gray-400 p-2;
    td {
        @apply p-3;
    }
}
</style>
