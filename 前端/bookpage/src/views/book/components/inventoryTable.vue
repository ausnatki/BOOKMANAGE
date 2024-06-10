<template>
  <div>
    <!-- 这里是通过图书名查询 -->
    <el-input v-model="serchBookname" placeholder="用户名" style="width:200px;padding:0 10px 10px 0" />
    <el-button type="primary" @click="ClickSerchBookName">搜索</el-button>
    <el-checkbox v-model="checked1" label="未审核" style="margin-left:20px;padding: 9px 20px 7px 10px;" border />
    <el-checkbox v-model="checked2" label="已审核" style="margin-left:-10px;padding: 9px 20px 7px 10px;" border />
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
          label="图书名"
        />
        <el-table-column
          prop="name"
          label="ISBN编号"
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
        <el-table-column
          prop="name"
          label="库存"
        />
        <el-table-column
          prop="name"
          label="在库"
        />
        <el-table-column
          prop="name"
          label="借出"
        />

        <el-table-column
          label="库存操作"
        >
          <el-link type="primary" style="padding-right:10px">查看</el-link>
          <el-link type="success">添加库存</el-link>
        </el-table-column>
        <!-- 控制图书状态 isdelte字段 -->

      </el-table>
    </template>
  </div>
</template>

<script>
import { GetAll, ChangeState } from '@/api/user'
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
  },
  methods: {
    // 初始化数据
    InitData() {
      GetAll()
        .then(result => {
          console.log()
        })
        .catch(response => {
          console.error()
        })
    },
    // 修改状态
    ClickState() {
      ChangeState(this.uid)
        .then(result => {
          if (result.result) {
            this.$message({
              type: 'success',
              message: '修改成功'
            })
          } else {
            this.$message({
              type: 'error',
              message: '修改失败'
            })
          }
        })
        .catch(response => {
          console.error(response)
          this.$message({
            type: 'error',
            message: '修改失败'
          })
        })
    }
  }
}
</script>
<style>

</style>
