<template>
  <div>
    <el-dialog
      title="注册"
      :visible.sync="dialogVisible"
      width="30%"
      :before-close="handleClose"
    >
      <el-form ref="registerForm" :model="registerForm" :rules="rules" label-width="100px" class="demo-registerForm">
        <el-form-item label="用户名" prop="username">
          <el-input v-model="registerForm.username" />
        </el-form-item>
        <el-form-item label="密码" prop="password">
          <el-input v-model="registerForm.password" type="password" />
        </el-form-item>
        <el-form-item label="邮箱" prop="email">
          <el-input v-model="registerForm.email" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="submitForm('registerForm')">注册</el-button>
          <el-button @click="resetForm('registerForm')">重置</el-button>
        </el-form-item>
      </el-form>
    </el-dialog>
  </div>
</template>

<script>
import { enroll } from '@/api/user'
export default {
  props: {
    isDialog: {
      type: Boolean,
      require: true
    }
  },
  data() {
    return {
      dialogVisible: false,
      registerForm: {
        UserName: '',
        Password: '',
        Email: ''
      },
      rules: {
        username: [
          { required: true, message: '请输入用户名', trigger: 'blur' },
          { min: 3, max: 15, message: '长度在 3 到 15 个字符', trigger: 'blur' }
        ],
        password: [
          { required: true, message: '请输入密码', trigger: 'blur' },
          { min: 6, max: 20, message: '长度在 6 到 20 个字符', trigger: 'blur' }
        ],
        email: [
          { required: true, message: '请输入邮箱', trigger: 'blur' },
          { type: 'email', message: '请输入有效的邮箱地址', trigger: 'blur' }
        ]
      }
    }
  },
  watch: {
    isDialog(newVal) {
      this.dialogVisible = newVal
    },
    dialogVisible(newVal) {
      if (newVal === true) {
        // this.initbook(this.bookid)
      }
      if (newVal === false) {
        this.$emit('update:isDialog', newVal)
        this.isLoading = true
      }
    },
    bookid(newVal) {
      // this.initbook(newVal)
    }

  },
  methods: {
    submitForm(formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          enroll(this.registerForm).then(result => {
            this.$message({
              type: 'success',
              message: '注册成功'
            })
            this.dialogVisible = false
          })
        } else {
          console.log('注册失败!!')
          return false
        }
      })
    },
    resetForm(formName) {
      this.$refs[formName].resetFields()
    }
  }
}
</script>

<style>
</style>
