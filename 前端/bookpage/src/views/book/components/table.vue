<!-- eslint-disable vue/attribute-hyphenation -->
<template>
  <div>
    <!-- 这里是通过图书名查询 -->
    <el-input v-model="input" placeholder="图书名" style="width:200px;padding:0 10px 10px 0" />
    <el-button type="primary">搜索</el-button>

    <!-- 这里是通过我的作者查询 -->
    <el-input v-model="input" placeholder="作者" style="width:200px;padding:0 10px 10px 10px" />
    <el-button type="primary">搜索</el-button>

    <!-- 这里是我的多选框的部分 -->
    <el-select
      v-model="value2"
      multiple
      collapse-tags
      style="margin-left: 20px;"
      placeholder="请选择"
    >
      <el-option
        v-for="item in options"
        :key="item.value"
        :label="item.label"
        :value="item.value"
      />
    </el-select>
    <el-table
      v-loading="isLoading"
      :data="tableData"
      border
      style="width: 100%"
    >
      <el-table-column
        prop="bookName"
        label="红楼梦"
        width="180"
      />
      <el-table-column
        prop="iSBN"
        label="ISBN"
        width="180"
      />
      <el-table-column
        prop="addTime"
        label="入库时间"
      />
      <el-table-column
        prop="press"
        label="出版社"
      />
      <el-table-column
        prop="inventory"
        label="库存"
      />
      <el-table-column
        prop="outInventory"
        label="借出"
      />
      <el-table-column
        prop="cateGory"
        label="类别"
      />
      <el-table-column
        prop="price"
        label="图书价格"
      />
      <el-table-column
        label="操作"
      >
        <!-- slot-scope="scope" -->
        <template>
          <el-link class="linetext" type="primary" @click="isDialog=true">借阅</el-link>
          <el-link class="linetext" type="success">查看</el-link>
          <el-link class="linetext" type="danger">删除</el-link>

        </template>
      </el-table-column>

    </el-table>
    <viewDialog :isDialog.sync="isDialog" />
  </div>
</template>

<script>
import viewDialog from '@/views/book/components/viewDialog.vue'
export default {
  name: 'BookTable',
  components: {
    viewDialog
  },
  data() {
    return {
      isDialog: false,
      isLoading: false, // 控制表格加载状态的变量
      tableData: [{
        bookName: '红楼梦',
        iSBN: '202020-11-2222',
        addTime: '2022-12-12',
        press: '浙江大学出版社',
        inventory: '12',
        outInventory: '1',
        author: '施耐庵',
        image: '',
        price: '12.23',
        cateGory: '社会哲学'
      }, {
        bookName: '红楼梦',
        iSBN: '202020-11-2222',
        addTime: '2022-12-12',
        press: '浙江大学出版社',
        inventory: '12',
        outInventory: '1',
        author: '施耐庵',
        image: '',
        price: '12.23',
        cateGory: '社会哲学'
      }, {
        bookName: '红楼梦',
        iSBN: '202020-11-2222',
        addTime: '2022-12-12',
        press: '浙江大学出版社',
        inventory: '12',
        outInventory: '1',
        author: '施耐庵',
        image: '',
        price: '12.23',
        cateGory: '社会哲学'
      }, {
        bookName: '红楼梦',
        iSBN: '202020-11-2222',
        addTime: '2022-12-12',
        press: '浙江大学出版社',
        inventory: '12',
        outInventory: '1',
        author: '施耐庵',
        image: '',
        price: '12.23',
        cateGory: '社会哲学'
      }],
      options: [{
        value: '选项1',
        label: '黄金糕'
      }, {
        value: '选项2',
        label: '双皮奶'
      }, {
        value: '选项3',
        label: '蚵仔煎'
      }, {
        value: '选项4',
        label: '龙须面'
      }, {
        value: '选项5',
        label: '北京烤鸭'
      }],
      value1: [],
      value2: []
    }
  }
}

</script>

<style>
.linetext{
    padding:0 5px;
}
</style>
