<!-- eslint-disable vue/attribute-hyphenation -->
<template>
  <div>
    <!-- 这里是通过图书名查询 -->
    <el-input v-model="serchBookname" placeholder="图书名" style="width:200px;padding:0 10px 10px 0" />
    <el-button type="primary" @click="ClickSerchBookName">搜索</el-button>

    <!-- 这里是通过我的作者查询 -->
    <el-input v-model="serchBookauth" placeholder="作者" style="width:200px;padding:0 10px 10px 10px" />
    <el-button type="primary" @click="ClickSerchBookAuth">搜索</el-button>

    <!-- 这里是我的多选框的部分 -->
    <el-select
      v-model="serchCategory"
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
      :data="filteredData"
      :default-sort="{prop: 'id', order: 'descending'}"
      border
      style="width: 100%"
    >

      <el-table-column
        sortable
        prop="id"
        label="编号"
        width="180"
      >
        <template slot-scope="scope">
          {{ scope.$index+1+(currentPage-1)*pageSize }}
        </template>
      </el-table-column>
      <el-table-column
        prop="bookName"
        label="图书名称"
        width="180"
      />
      <el-table-column
        prop="isbn"
        label="ISBN"
        width="180"
      />
      <el-table-column
        prop="addTime"
        label="入库时间"
      >
        <template slot-scope="scope">
          {{ DoTime(scope.row.addTime) }}
        </template>
      </el-table-column>
      <el-table-column
        prop="press"
        label="出版社"
      />
      <el-table-column
        prop="author"
        label="作者"
      />
      <el-table-column
        prop="category"
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
        <template slot-scope="scope">
          <el-link class="linetext" type="primary" @click="ClickBorrow(scope.row.id)">借阅</el-link>
          <!-- <el-link class="linetext" type="success">查看</el-link>
          <el-link class="linetext" type="danger">删除</el-link> -->

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
    <viewDialog :bookid.sync="bookid" :isDialog.sync="isDialog" />
  </div>
</template>

<script>
import { GetBook } from '@/api/book.js'
import viewDialog from '@/views/book/components/viewDialog.vue'
export default {
  name: 'BookTable',
  components: {
    viewDialog
  },
  data() {
    return {
      isDialog: false,
      isLoading: true, // 控制表格加载状态的变量
      bookid: 0, // 传入弹出层的值
      tableData: [],
      options: [{
        value: '社会哲学',
        label: '社会哲学'
      }, {
        value: '数学',
        label: '数学'
      }, {
        value: '软件编程',
        label: '软件编程'
      }, {
        value: '悬疑侦探',
        label: '悬疑侦探'
      }],
      serchBookname: '',
      tserchBookname: '',
      serchBookauth: '',
      tserchBookauth: '',
      serchCategory: [],
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
      const auth = this.tserchBookauth
      const category = this.serchCategory
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
      if (auth) {
        console.log('进行我的作者查询')
        filtered = filtered.filter(item => {
          return item.author.includes(auth)
        })
      }
      // console.log(filtered)
      if (category.length) {
        console.log('进行我的类别查询')
        filtered = filtered.filter(item => {
          return category.includes(item.category)
        })
      }
      // console.log(filtered)
      return filtered
    }
  },
  mounted() {
    this.InitBookList()
  },
  methods: {
    // 获取图书列表的方法
    InitBookList() {
      var flage = false
      GetBook().then(result => {
        flage = true
        // console.log(result)

        this.tableData = result.data
      }).catch(response => {
        console.error('错误原因' + response)
      }).finally(() => {
        if (flage === true) this.isLoading = false
      })
    },
    // 时间处理方法
    DoTime(timestamp) {
      console.log(timestamp)
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
    // 点击借阅后的方法发
    ClickBorrow(id) {
      this.bookid = id
      this.isDialog = true
    },
    // 通过图书名搜索
    ClickSerchBookName() {
      this.tserchBookname = this.serchBookname
    },
    // 通过作者来搜索
    ClickSerchBookAuth() {
      this.tserchBookauth = this.serchBookauth
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
.linetext{
    padding:0 5px;
}
</style>
