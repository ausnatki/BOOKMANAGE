<template>
  <div>
    <!-- 这里是通过图书名查询 -->
    <el-input v-model="serchUserName" placeholder="用户名" style="width:200px;padding:0 10px 10px 0" />
    <el-button type="primary" @click="ClickSerchUserName">搜索</el-button>
    <el-checkbox v-model="checked1" label="未审核" style="margin-left:20px;padding: 9px 20px 7px 10px;" border />
    <el-checkbox v-model="checked2" label="已审核" style="margin-left:-10px;padding: 9px 20px 7px 10px;" border />
    <template>
      <el-table
        :data="filteredData"
        border
        style="width: 100%"
      >
        <el-table-column
          prop="date"
          sortable
          label="编号"
        >
          <template slot-scope="scope">
            {{ scope.$index+1 }}
          </template>
        </el-table-column>
        <el-table-column
          prop="userName"
          label="用户名"
        />
        <el-table-column
          prop="loginName"
          label="登录号"
        />
        <el-table-column
          prop="password"
          label="密码"
        />
        <el-table-column
          label="状态"
        >
          <template slot-scope="scope">
            <el-switch
              v-model="scope.row.role"
              active-color="#13ce66"
              inactive-color="#ff4949"
              @change="ChangeRole(scope.row)"
            />
          </template>
        </el-table-column>
      </el-table>
    </template>
  </div>
</template>

<script>
import { GetAll, ChangeState } from '@/api/user'
export default {
  data() {
    return {
      tableData: [],
      serchUserName: '',
      tserchUserName: '',
      checked1: false,
      checked2: false
    }
  },
  computed: {
    filteredData() {
      // console.log(this.tableData)
      let filtered = this.tableData
      const username = this.tserchUserName
      const checked1 = this.checked1
      const checked2 = this.checked2
      // console.log(filtered)
      if (username) {
        // console.log('进行我的作者查询')
        filtered = filtered.filter(item => {
          return item.userName.includes(username)
        })
      }
      // 根据 checked1 和 checked2 进行过滤
      if (checked1 && !checked2) {
        filtered = filtered.filter(item => {
          return !item.role // 未审核
        })
      } else if (!checked1 && checked2) {
        filtered = filtered.filter(item => {
          return item.role // 已审核
        })
      } else if (checked1 && checked2) {
        // 显示所有项目
        // 不需要过滤
      }

      return filtered
    }
  },
  mounted() {
    this.Initdata()
  },
  methods: {
    // 初始化数据
    async Initdata() {
      console.log('进入了初始化函数中')
      await GetAll()
        .then(result => {
          console.log(result)
          this.tableData = result.data
        })
        .catch(response => {
          console.error(response)
        })
    },
    // 改变数据
    ChangeRole(row) {
      console.log(row.id)
      ChangeState(row.id)
        .then(result => {
          if (result.result === true) {
            this.$message({
              type: 'success',
              message: '修改成功'
            })
          } else {
            this.$message({
              type: 'error',
              message: '修改失败'
            })
            row.role = !row.role
          }
        }).catch(response => {
          this.$message({
            type: 'error',
            message: '修改失败'
          })
          row.role = !row.role
        })
    },
    ClickSerchUserName() {
      this.tserchUserName = this.serchUserName
    }
  }
}
</script>
<style>

</style>
