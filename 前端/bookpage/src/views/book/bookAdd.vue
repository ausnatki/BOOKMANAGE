<template>
  <div class="my-box">
    <el-page-header content="新增商品" style=" padding: 10px;" @back="$router.back()" />
    <el-form ref="BOOKFORM" :model="BookForm" :rules="rules" label-width="100px">
      <el-form-item label="图书封面" prop="name">
        <el-upload
          class="avatar-uploader"
          action="Book/Image/imgupload"
          :show-file-list="false"
          :on-success="handleAvatarSuccess"
          :before-upload="beforeAvatarUpload"
        >
          <img v-if="imageUrl" :src="imageUrl" class="avatar">
          <i v-else class="el-icon-plus avatar-uploader-icon" />
        </el-upload>
      </el-form-item>
      <el-row :gutter="20">
        <el-col :span="7">
          <el-form-item label="书名" prop="BookName">
            <el-input v-model="BookForm.BookName" placeholder="请输入图书名" />
          </el-form-item>
        </el-col>
        <el-col :span="7">
          <el-form-item label="作者" prop="Author">
            <el-input v-model="BookForm.Author" placeholder="请输入图书作者" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row :gutter="20">
        <el-col :span="7">
          <el-form-item label="出版社" prop="Press">
            <el-input v-model="BookForm.Press" placeholder="请输入图书出版社" />
          </el-form-item>
        </el-col>
        <el-col :span="7">
          <el-form-item label="ISBN编码" prop="ISBN">
            <el-input v-model="BookForm.ISBN" placeholder="请输入ISBN编码" />
          </el-form-item>
        </el-col>
      </el-row>

      <el-form-item label="图书价格" prop="Price">
        <el-col :span="4">
          <el-input v-model="BookForm.Price" placeholder="请输入图书价格">
            <template slot="append">元</template>
          </el-input>
        </el-col>
      </el-form-item>

      <el-form-item label="图书库存" prop="Inventory">
        <el-col :span="4">
          <el-input-number v-model="BookForm.Inventory" controls-position="right" :min="1" :max="10" />
        </el-col>
      </el-form-item>
      <el-form-item label="图书类别" prop="Category">
        <el-select v-model="BookForm.Category" placeholder="请选择图书类别">
          <el-option label="社会哲学" value="shanghai" />
          <el-option label="数学" value="beijing" />
        </el-select>
      </el-form-item>

      <el-form-item>
        <el-button type="primary" @click="submitForm('BOOKFORM')">进行采购</el-button>
        <el-button @click="resetForm('BOOKFORM')">重置</el-button>
      </el-form-item>

    </el-form>
  </div>
</template>

<script>
import { AddBook } from '@/api/book.js'
export default {
  name: 'BookAdd',
  data() {
    return {
      imageUrl: '', // 这个是用来接受图片处理后的
      // 图书的表单
      BookForm: {
        BookName: '',
        ISBN: '',
        AddTime: '',
        Press: '',
        Inventory: '',
        Author: '',
        Image: '',
        Price: '',
        Category: ''
      },
      // 表单的验证规则
      rules: {
        BookName: [
          { required: true, message: '请输入图书名', trigger: 'blur' }
        ],
        Category: [
          { required: true, message: '请选择活动类别', trigger: 'change' }
        ],
        Press: [
          { required: true, message: '请输入出版社名称', trigger: 'blur' }
        ],
        Inventory: [
          { required: true, message: '请输入库存量', trigger: 'blur' }
        ],
        Author: [
          { required: true, message: '请输入图书作者', trigger: 'blur' }
        ],
        Price: [
          {
            required: true,
            message: '请输入图书价格',
            trigger: 'change'
          },
          {
            validator: (rule, value, callback) => {
              if (value <= 0) {
                callback(new Error('价格必须大于0'))
              } else if (value > 1000) {
                callback(new Error('价格不能超过1000'))
              } else {
                callback()
              }
            },
            trigger: 'blur'
          }
        ],
        ISBN: [
          {
            required: true,
            message: '请输入ISBN号',
            trigger: 'blur'
          },
          {
            pattern: /^(?:ISBN(?:-10)?:? )?(?=[0-9X]{10}$|(?=(?:[0-9]+[- ]){3})[- 0-9X]{13}$|97[89][0-9]{10}$|(?=(?:[0-9]+[- ]){4})[- 0-9]{17}$)(?:97[89][- ]?)?[0-9]{1,5}[- ]?[0-9]+[- ]?[0-9]+[- ]?[0-9X]$/,
            message: '请输入正确的ISBN号',
            trigger: 'blur'
          }
        ]

      }
    }
  },
  methods: {
    // 图片路径处理方式
    handleAvatarSuccess(res, file) {
      this.BookForm.Image = res.data.url
      console.log((this.BookForm.Image))
    },
    // 上传之前的方法
    beforeAvatarUpload(file) {
      const isJPG = file.type === 'image/jpeg'
      const isLt2M = file.size / 1024 / 1024 < 2

      if (!isJPG) {
        this.$message.error('上传头像图片只能是 JPG 格式!')
      }
      if (!isLt2M) {
        this.$message.error('上传头像图片大小不能超过 2MB!')
      }
      return isJPG && isLt2M
    },
    async submitForm(formName) {
      // 这里是表单的验证判断
      // 保存验证结果
      let valid = null

      try {
        // 等待表单验证完成
        await this.$refs[formName].validate(result => {
          valid = result
        })
        console.log(valid) // 输出 true 或 false
      } catch (error) {
        console.error(error)
      }

      if (valid === false) {
        return
      }

      this.BookForm.AddTime = '2024-06-05T12:00:00Z'
      // 表单验证通过时执行确认对话框的逻辑
      this.$confirm('是否确认入库?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        AddBook(this.BookForm)
          .then(result => {
            this.$message({
              type: 'success',
              message: '入库成功'
            })
            this.resetForm('BOOKFORM')
          })
          .catch(response => {
            console.error(response)
            this.$message({
              type: 'error',
              message: '入库失败！'
            })
          })
      }).catch(() => {
        this.$message({
          type: 'info',
          message: '已取消入库'
        })
      })
    },
    // 重置表单
    resetForm(formName) {
      this.$refs[formName].resetFields()
      this.imageUrl = ''
    }
  }
}
</script>

<style scopd>
 .avatar-uploader .el-upload {
    margin-top: 10px;
    border: 1px dashed #d9d9d9;
    border-radius: 6px;
    cursor: pointer;
    position: relative;
    overflow: hidden;
  }
  .avatar-uploader .el-upload:hover {
    border-color: #409EFF;
  }
  .avatar-uploader-icon {

    font-size: 28px;
    color: #8c939d;
    width: 178px;
    height: 220px;
    line-height: 220px;
    text-align: center;
  }
  .avatar {
    width: 178px;
    height: 220px;
    display: block;
  }
</style>
