<template>
<tr class="tableHeader">
    <th v-for="(el, index) in header"
        :key="index"
        :header="el">
        <td>{{el.key}}</td>
    </th>
</tr>
</template>

<script setup lang="ts">
const props = defineProps({
    header:{
        required:true,
        type:Array as ()=> Array<{index:number,key:string}>
    }
})
</script>

<style lang="scss">
.tableHeader{
    @apply text-left border-t border-b-2 border-grey2 p-2 bg-white;
    th{
        @apply p-3
    }  
}      
</style>
