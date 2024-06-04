<template>
  <div class="my-box">
    <Booktable />
  </div>
</template>

<script>
import Booktable from '@/views/book/components/table.vue'
export default {
  name: 'BookIndex',
  components: {
    Booktable
  },
  data() {
    return {
      falge: false // 这里是控制我的弹出层的标签
    }
  }
}
</script>

<style>
  .my-box{
    padding: 10px;
  }
</style>
