<template>
    <div class="table w-full">
        <table class="w-full">
            <TableHeader :header="tableHeaders" v-if="tableHeaders" />
            <TableRow v-for="(row, index) in tableRows" :key="index" :row="row" />
        </table>
    </div>
</template>

<script setup lang="ts">
import { computed, onMounted, nextTick } from "@vue/composition-api";
import TableHeader from './TableHeader.vue'
import TableRow from './TableRow.vue'
interface IColumn {
    label: string,
    key: string
}
const props = defineProps({
    columns: {
        required: true,
        type: Array as () => Array<IColumn>
    },
    data: {
        required: true,
        type: Array as () => Array<any>
    }
});

const tableHeaders = computed(() => {
    if (props.data && props.data.length) {
        return Object.keys(props.data[0]).map((header, index) => {
            return {
                index: index,
                key: header,
            };
        });
    }
})

const tableRows = computed(() => {
    return props.data.map((obj, idx) => {
        return Object.keys(obj).map((key, idx2) => {
            return {
                key: key,
                value: Object.values(obj)[idx2],
            };
        });
    });
})
</script>

<style lang="scss">
.talbe {
    table {
        thead {
            th {
                @apply text-left border-t border-b-2 border-grey2 p-2;
                div {
                    @apply font-bold;
                }
            }
        }
        tr {
            @apply even:bg-gray-300 hover:bg-gray-400;
            td {
                @apply border-t border-b border-grey2 p-2;
                div {
                    @apply font-normal;
                }
            }
        }
    }
}
</style>
