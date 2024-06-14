<template>
  <div>
    <!-- 这里是通过图书名查询 -->
    <el-input v-model="serchBookname" placeholder="图书名" style="width:200px;padding:0 10px 10px 0" />
    <el-button type="primary" @click="ClickSerchBookName">搜索</el-button>

    <!-- 列表位置 -->
    <template>
      <el-table
        v-loading="isLoading"
        :data="filteredData"
        border
        style="width: 100%"
      >
        <el-table-column
          prop="id"
          sortable
          label="编号"
        >
          <template slot-scope="scope">
            {{ scope.$index+1+(currentPage-1)*pageSize }}
          </template>
        </el-table-column>
        <el-table-column
          prop="bookName"
          label="图书名"
        />
        <el-table-column
          prop="isbn"
          label="ISBN编号"
        />
        <el-table-column
          label="状态"
        >
          <template slot-scope="scope">
            <el-switch
              :value="reversedState(scope.row.state)"
              active-color="#13ce66"
              inactive-color="#ff4949"
              @change="ClickState(scope.row)"
            />
          </template>
        </el-table-column>
        <el-table-column
          prop="inventory"
          label="库存"
        />
        <el-table-column
          prop="inStock"
          label="在库"
        />
        <el-table-column
          prop="loanedOut"
          label="借出"
        />

        <el-table-column
          label="库存操作"
        >
          <template slot-scope="scope">
            <el-link type="primary" style="padding-right:10px" @click="ViewBorrow(scope.row.id)">查看</el-link>
            <el-link type="success" @click="ClickInventory(scope.row.id)">添加库存</el-link>
          </template>
        </el-table-column>
        <!-- 控制图书状态 isdelte字段 -->

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

    </template>

    <!-- 我的弹出层位置 -->
    <el-dialog title="借阅记录" :visible.sync="dialogTableVisible">
      <el-table v-loading="isvLoading" :data="dialogTable">
        <el-table-column label="编号">
          <template slot-scope="scope">
            {{ scope.$index+1 }}
          </template>
        </el-table-column>
        <el-table-column property="bookName" label="图书名" />
        <el-table-column property="userName" label="借阅人" />
        <el-table-column property="email" label="邮箱地址" />

        <el-table-column label="归还">
          <template slot-scope="scope">
            {{ IsBorrowText(scope.row.state) }}
          </template>
        </el-table-column>

      </el-table>
    </el-dialog>

  </div>
</template>

<script>
import { GetInventory, ChangeState, GetBorrowByBid, AddInventory } from '@/api/book.js'
export default {
  data() {
    return {
      tableData: [],
      dialogTableVisible: false,
      dialogTable: [],
      isLoading: true,
      isvLoading: true,
      serchBookname: '',
      tserchBookname: '',
      currentPage: 1, // 当前页码
      total: 20, // 总条数
      pageSize: 10 // 每页的数据条数
    }
  },
  computed: {
    filteredData() {
      // console.log(this.tableData)
      let filtered = this.tableData
      const bookname = this.tserchBookname

      filtered = filtered.slice((this.currentPage - 1) * this.pageSize, this.currentPage * this.pageSize)

      // console.log(category.length)
      // 判断是否有值
      if (bookname) {
        console.log('进行我的图书查询')
        filtered = filtered.filter(item => {
          return item.bookName.includes(bookname)
        })
      }

      // console.log(filtered)
      return filtered
    }
  },
  mounted() {
    this.InitData()
  },
  methods: {
    // 初始化数据
    async InitData() {
      await GetInventory()
        .then(result => {
          console.log()
          this.tableData = result.data
        })
        .catch(response => {
          console.error()
        }).finally(() => {
          this.isLoading = false
        })
    },
    // 修改状态
    ClickState(row) {
      ChangeState(row.id)
        .then(result => {
          if (result.result) {
            this.$message({
              type: 'success',
              message: '修改成功'
            })
            row.state = !row.state
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
    },
    reversedState(state) {
      return !state
    },
    // 查看图书的借阅记录
    ViewBorrow(BID) {
      this.isvLoading = true
      this.dialogTableVisible = true
      GetBorrowByBid(BID)
        .then(result => {
          console.log(result)
          this.dialogTable = result.data
        })
        .catch(response => {
          console.error(response)
        }).finally(() => {
          this.isvLoading = false
        })
    },
    // 判断是否归还
    IsBorrowText(state) {
      if (state) return '归还'
      else return '未归还'
    },
    // 添加的逻辑
    ClickInventory(BID) {
      this.$prompt('请输入添加的库存', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        inputPattern: /^\d+(\.\d+)?$/,
        inputErrorMessage: '请输入正确的数字'
      }).then(({ value }) => {
        AddInventory(BID, value).then(result => {
          if (result.result) {
            this.$message({
              type: 'success',
              message: '修改成功'
            })
            this.InitData()
          } else {
            this.$message({
              type: 'error',
              message: '修改失败'
            })
          }
        })
      }).catch(() => {
        this.$message({
          type: 'info',
          message: '取消输入'
        })
      })
    },
    // 搜索
    ClickSerchBookName() {
      this.tserchBookname = this.serchBookname
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
<style>

</style>
