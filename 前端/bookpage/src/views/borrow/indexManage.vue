<template>
  <div class="my-box">
    <ManageTable />
  </div>
</template>

<script>
import ManageTable from '@/views/borrow/components/manageTable.vue'
export default {
  name: 'BorrowedUser',
  components: {
    ManageTable
  }
}
</script>

<style>
  .my-box{
    padding: 10px;
  }
</style>
