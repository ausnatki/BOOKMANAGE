<template>
  <div class="my-box">
    <BowTable />
  </div>
</template>

<script>
import BowTable from '@/views/borrow/components/table.vue'
export default {
  name: 'BorrowedUser',
  components: {
    BowTable
  }
}
</script>

<style>
  .my-box{
    padding: 10px;
  }
</style>
