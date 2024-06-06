<template>
  <div class="my-box">
    <el-dialog
      title="借阅"
      :visible.sync="dialogVisible"
      width="50%"
    >
      <el-form ref="form" v-loading="isLoading" :model="form" label-width="80px">
        <el-row :gutter="20">
          <!-- 图书封面 -->
          <el-col :span="6" offset="1">
            <div class="bookimage">
              <el-image style="width:100%;height:100%" :src="DoImage(form.image)" />
            </div>
          </el-col>
          <el-col :span="15">
            <el-row :gutter="20">
              <!-- 图书名 -->
              <el-col :span="12">
                <el-form-item label="书名">
                  <el-input v-model="form.bookName" readonly />
                </el-form-item>
              </el-col>
            </el-row>
            <el-row :gutter="20">
              <!-- 图书作者 -->
              <el-col :span="12">
                <el-form-item label="作者">
                  <el-input v-model="form.author" readonly />
                </el-form-item>
              </el-col>
            </el-row>
            <el-row :gutter="20">
              <!-- 图书出版社 -->
              <el-col :span="12">
                <el-form-item label="出版社">
                  <el-input v-model="form.press" readonly />
                </el-form-item>
              </el-col>
              <!-- 图书类别 -->
              <el-col :span="12">
                <el-form-item label="图书类别">
                  <el-input v-model="form.category" readonly />
                </el-form-item>
              </el-col>
            </el-row>
            <el-row :gutter="18">
              <!-- 图书ISBN -->
              <el-col :span="12">
                <el-form-item label="ISBN">
                  <el-input v-model="form.isbn" readonly />
                </el-form-item>
              </el-col>
              <!-- 图书库存 -->
              <el-col :span="12">
                <el-form-item label="库存">
                  <el-input v-model="outinventory" readonly />
                </el-form-item>
              </el-col>
            </el-row>
            <el-row :gutter="20">
              <!-- 图书价格 -->
              <el-col :span="12">
                <el-form-item label="价格">
                  <el-input v-model="form.price" readonly />
                </el-form-item>
              </el-col>
              <!-- 按钮 -->
              <el-col :span="10" :offset="1">
                <el-button type="primary" @click="dialogVisible = false">借 阅</el-button>
                <el-button @click="dialogVisible = false">取 消</el-button>
              </el-col>
            </el-row>
          </el-col>
        </el-row>
      </el-form>
    </el-dialog>
  </div>
</template>

<script>
import { GetById } from '@/api/book.js'
export default {
  name: 'ViewBook',
  props: {
    isDialog: {
      type: Boolean,
      require: true
    },
    bookid: {
      type: Number,
      require: true,
      default: 0 // 设置默认值为0
    }
  },
  data() {
    return {
      isLoading: true, // 控制表格加载状态的变量
      dialogVisible: false,
      form: {
        // name: '福尔摩斯'
      },
      outinventory: 0
    }
  },
  watch: {
    isDialog(newVal) {
      this.dialogVisible = newVal
    },
    dialogVisible(newVal) {
      if (newVal === false) {
        this.$emit('update:isDialog', newVal)
        this.isLoading = true
      }
    },
    bookid(newVal) {
      this.initbook(newVal)
    }

  },
  methods: {
    // 初始化数据
    initbook(id) {
      GetById(id).then(result => {
        console.log(result)
        this.form = result.data.book
        this.outinventory = result.data.book.inventory - result.data.outInventory
      }).catch(respnse => {
        console.error(Response)
      }).finally(() => {
        this.isLoading = false
      })
    },
    // 点击借阅
    ClickBorrow() {

    },
    // 拼接图片路径
    DoImage(image) {
      return 'Book/Image/' + image
    }
  }
}
</script>

<style scope>
  .el-row {
    margin-bottom: 20px;
  }
  .el-col {
    border-radius: 4px;
  }
  .bg-purple-dark {
    background: #99a9bf;
  }
  .bg-purple {
    background: #d3dce6;
  }
  .bg-purple-light {
    background: #e5e9f2;
  }
  .grid-content {
    border-radius: 4px;
    min-height: 36px;
  }
  .row-bg {
    padding: 10px 0;
    background-color: #f9fafc;
  }
  .bookimage{
    width: 230px;
    height: 345px;
  }
</style>
