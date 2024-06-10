<template>
  <div class="my-box">
    <UserTable />
  </div>
</template>

<script>
import UserTable from '@/views/user/components/table.vue'
export default {
  name: 'Userindex',
  components: {
    UserTable
  }
}
</script>

<style scope>
.my-box{
  padding: 10px;
}
</style>
