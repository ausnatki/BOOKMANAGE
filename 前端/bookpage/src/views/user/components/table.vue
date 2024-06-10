<template>
  <div>
    <!-- 这里是通过图书名查询 -->
    <el-input v-model="serchBookname" placeholder="用户名" style="width:200px;padding:0 10px 10px 0" />
    <el-button type="primary" @click="ClickSerchBookName">搜索</el-button>

    <template>
      <el-table
        :data="tableData"
        border
        style="width: 100%"
      >
        <el-table-column
          prop="date"
          sortable
          label="编号"
        />
        <el-table-column
          prop="date"
          label="用户名"
        />
        <el-table-column
          prop="date"
          label="登录号"
        />
        <el-table-column
          prop="name"
          label="密码"
        />
        <el-table-column
          label="状态"
        >
          <template slot-scope="scope">
            <el-switch
              v-model="value"
              active-color="#13ce66"
              inactive-color="#ff4949"
            />
          </template>
        </el-table-column>
      </el-table>
    </template>
  </div>
</template>

<script>
export default {
  data() {
    return {
      tableData: [{
        date: '2016-05-022016-05-02',
        name: '王小虎',
        address: '上海市普陀区金沙江路 1518 弄'
      }, {
        date: '2016-05-04',
        name: '王小虎',
        address: '上海市普陀区金沙江路 1517 弄'
      }, {
        date: '2016-05-01',
        name: '王小虎',
        address: '上海市普陀区金沙江路 1519 弄'
      }, {
        date: '2016-05-03',
        name: '王小虎',
        address: '上海市普陀区金沙江路 1516 弄'
      }]
    }
  }
}
</script>
<style>

</style>
