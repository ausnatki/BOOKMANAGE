<template>
  <div>
    <!-- 这里是通过图书名查询 -->
    <el-input v-model="serchBookname" placeholder="图书名" style="width:200px;padding:0 10px 10px 0" />
    <el-button type="primary" @click="ClickSerchBookName">搜索</el-button>

    <!-- 这里是通过我的作者查询 -->
    <el-input v-model="serchBookauth" placeholder="作者" style="width:200px;padding:0 10px 10px 10px" />
    <el-button type="primary" @click="ClickSerchBookAuth">搜索</el-button>

    <el-table
      v-loading="isLoading"
      :data="filteredData"
      :default-sort="{prop: 'id', order: 'descending'}"
      border
      style="width: 100%"
    >
      <el-table-column
        prop="id"
        label="编号"
        width="180"
      >
        <template slot-scope="scope">
          {{ scope.$index+1+(currentPage-1)*pageSize }}
        </template>
      </el-table-column>
      <el-table-column
        prop="book.bookName"
        label="图书名称"
        width="180"
      />
      <el-table-column
        prop="book.author"
        label="图书作者"
      />
      <el-table-column
        prop="borrowedTime"
        label="借阅时间"
      >
        <template slot-scope="scope">
          {{ DoTime(scope.row.borrowedTime) }}
        </template>
      </el-table-column>
      <el-table-column
        prop="repaidTime"
        label="归还时间"
      >
        <template slot-scope="scope">
          {{ DoTime(scope.row.repaidTime) }}
        </template>
      </el-table-column>
      <el-table-column
        prop="sysUser.userName"
        label="借阅用户"
      />
      <el-table-column
        label="是否归还"
      >
        <template slot-scope="scope">
          <el-tag :type="SwitchStateTyep(scope.row.state)">{{ SwitchStateText(scope.row.state) }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="操作"
      >
        <template slot-scope="scope">
          <el-link class="linetext" type="primary" @click="ClickRenewal(scope.row)">续借</el-link>
        </template>
      </el-table-column>
    </el-table>
    <el-pagination
      align="center"
      :current-page="currentPage"
      :page-sizes="[1, 5, 10, 20]"
      :page-size="pageSize"
      layout="total, sizes, prev, pager, next, jumper"
      :total="tableData.length"
      @size-change="handleSizeChange"
      @current-change="handleCurrentChange"
    />
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import { GetBorrowed, Renewal } from '@/api/borrowed.js'
export default {
  data() {
    return {
      isLoading: true, // 控制表格加载状态的变量
      tableData: [],
      serchBookname: '',
      tserchBookname: '',
      serchBookauth: '',
      tserchBookauth: '',
      currentPage: 1, // 当前页码
      total: 20, // 总条数
      pageSize: 10 // 每页的数据条数
    }
  },
  computed: {
    ...mapGetters([
      'name',
      'avatar',
      'roles',
      'uid'
    ]),
    filteredData() {
      // console.log(this.tableData)
      let filtered = this.tableData
      const bookname = this.tserchBookname
      const auth = this.tserchBookauth
      filtered = filtered.slice((this.currentPage - 1) * this.pageSize, this.currentPage * this.pageSize)

      // 判断是否有值
      if (bookname) {
        console.log('进行我的图书查询')
        filtered = filtered.filter(item => {
          return item.book.bookName.includes(bookname)
        })
      }
      // console.log(filtered)
      if (auth) {
        console.log('进行我的作者查询')
        filtered = filtered.filter(item => {
          return item.book.author.includes(auth)
        })
      }

      return filtered
    }
  },
  mounted() {
    this.Initdata()
  },
  methods: {
    // 点击归还
    async ClickRenewal(data) {
      // 创建一个data的深拷贝
      // var tdata = JSON.parse(JSON.stringify(data))

      //   // 现在可以安全地修改tdata的属性，而不会影响data
      //   tdata.book = null
      //   tdata.sysUser = null

      if (data.state === true) {
        this.$message({
          type: 'warning',
          message: '已经归还了'
        })
        return
      }

      await this.$confirm('确定要归还图书吗, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      })
        .then(() => {
          Renewal(data).then(result => {
            this.$message({
              type: 'success',
              message: '归还成功'
            })
            // data.state = true
          }).catch(response => {
            this.$message({
              type: 'error',
              message: '归还失败'
            })
          })
        })
        .catch(() => {
          this.$message({
            type: 'info',
            message: '已取消删除'
          })
        })
    },
    // 初始化数据
    Initdata() {
      GetBorrowed(this.uid).then(result => {
        this.tableData = result.data
      }).catch(response => {
        console.error(response)
      }).finally(() => {
        this.isLoading = false
      })
    },
    // 时间处理方法
    DoTime(timestamp) {
    //   console.log(timestamp)
      // 将时间戳字符串转换为日期对象
      var dateTime = new Date(timestamp)

      // 提取年、月、日、小时和分钟
      var year = dateTime.getFullYear()
      var month = dateTime.getMonth() + 1 // 月份是从0开始计数的，所以要加1
      var day = dateTime.getDate()
      var hours = dateTime.getHours()
      var minutes = dateTime.getMinutes()

      // 格式化成想要的格式，这里只保留到分钟
      var formattedTimestamp = year + '-' + (month < 10 ? '0' : '') + month + '-' + (day < 10 ? '0' : '') + day + ' ' + (hours < 10 ? '0' : '') + hours + ':' + (minutes < 10 ? '0' : '') + minutes

      return formattedTimestamp
    },
    // 通过图书名搜索
    ClickSerchBookName() {
      this.tserchBookname = this.serchBookname
    },
    // 通过作者来搜索
    ClickSerchBookAuth() {
      this.tserchBookauth = this.serchBookauth
    },
    // 状态栏样式判断
    SwitchStateTyep(state) {
    //   console.log(state)
      switch (state) {
        case true: return 'success'
        case false: return 'danger'
        default : return 'info'
      }
    },
    // 状态栏文版
    SwitchStateText(state) {
      switch (state) {
        case true: return '已归还'
        case false: return '未归还'
        default : return '未知状态'
      }
    },
    // 每页条数改变时触发 选择一页显示多少行
    handleSizeChange(val) {
      // console.log(`每页 ${val} 条`)
      this.currentPage = 1
      this.pageSize = val
    },
    // 当前页改变时触发 跳转其他页
    handleCurrentChange(val) {
      // console.log(`当前页: ${val}`)
      this.currentPage = val
    }
  }
}
</script>

<style scoped>
.linetext{
    padding:0 5px;
}
</style>
