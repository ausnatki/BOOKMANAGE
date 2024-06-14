<template>
  <div class="my-box">
    <CensorTable />
  </div>
</template>

<script>
import CensorTable from '@/views/borrow/components/censorTable.vue'
export default {
  components: {
    CensorTable
  }
}
</script>

<style scoped>
.my-box{
    padding: 10px;
}
</style>
