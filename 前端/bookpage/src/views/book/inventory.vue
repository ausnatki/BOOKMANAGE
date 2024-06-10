<template>
  <div class="my-box">
    <InventoryTable />
  </div>
</template>

<script>
import InventoryTable from '@/views/book/components/inventoryTable.vue'
export default {
  name: 'BookInventory',
  components: {
    InventoryTable
  }
}
</script>

<style scope>
.my-box{
    padding: 10px;
}
</style>
