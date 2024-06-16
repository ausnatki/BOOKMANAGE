<template>
  <el-dialog

    :visible.sync="dialogVisible"
    width="25%"
  >
    <div v-loading="isLoading">
      <el-row :gutter="10">
        <el-col :span="20" :offset="2">
          <el-card shadow="always" class="imagebox">
            <div class="avatar-container">
              <el-avatar :size="80" src="https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif" />
            </div>
          </el-card>
        </el-col>
      </el-row>

      <el-row :gutter="20">
        <el-col :span="18" :offset="7" style="padding-bottom:10px">
          <span class="mylable">用户名：</span>
          <span class="mytext">{{ form.UserName }}</span>
          <el-button type="primary" icon="el-icon-edit" size="mini" circle @click="ClickUserEdit" />
        </el-col>
      </el-row>
      <el-row :gutter="20">
        <el-col :span="10" :offset="2">
          <div class="mylable">登录名：</div>
          <span class="mytext">{{ form.LoginName }}</span>
        <!-- <el-button type="primary" icon="el-icon-edit" size="mini" circle /> -->
        </el-col>
        <el-col :span="10" :offset="2">
          <div class="mylable">密码：</div>
          <el-link class="mytext" target="_blank" @click="ClickPasswordEdit">{{ form.Password }}</el-link>
        <!-- <el-button type="primary" icon="el-icon-edit" size="mini" circle /> -->
        </el-col>
      </el-row>
      <el-row :gutter="20">
        <el-col :span="10" :offset="2">
          <div class="mylable">Email：</div>
          <span class="mytext">{{ form.Email }}</span>
        <!-- <el-button type="primary" icon="el-icon-edit" size="mini" circle /> -->
        </el-col>
        <el-col :span="10" :offset="2" style="">
          <div class="mylable">角色：</div>
          <span class="mytext">{{ RoleText(roles) }}</span>
        <!-- <el-button type="primary" icon="el-icon-edit" size="mini" circle /> -->
        </el-col>
      </el-row>
      <el-row :gutter="20">
        <el-col :span="20" :offset="2">
          <el-button type="success" style="width:100%;margin-top:35px">确定</el-button>
        </el-col>
      </el-row>
    </div>
  </el-dialog>
</template>

<script>
import { GetInfoById, EditInfo } from '@/api/user'
import { mapGetters } from 'vuex'
export default {
  props: {
    digflage: {
      type: Boolean,
      require: true
    }
  },
  data() {
    return {
      isLoading: true,
      dialogVisible: false,
      form: {
        Id: '',
        UserName: '',
        LoginName: '',
        Email: '',
        Password: ''
      }
    }
  },
  computed: {
    ...mapGetters([
      'uid',
      'roles'
    ])
  },
  watch: {
    digflage(newVal) {
      this.dialogVisible = newVal
    },
    dialogVisible(newVal) {
      if (newVal === false) {
        this.$emit('update:digflage', newVal)
      }
    }
  },
  mounted() {
    this.InitUserInfo()
  },
  methods: {
    handleClose(done) {
      this.$confirm('确认关闭？')
        .then(_ => {
          done()
        })
        .catch(_ => {})
    },
    // 点击用户修改
    async ClickUserEdit() {
      await this.$prompt('请输入新用户名', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        inputPattern: /^.{0,20}$/,
        inputErrorMessage: '邮箱长度必须超过20个字符'
      }).then(({ value }) => {
        this.form.UserName = value
        this.EditInfo()
      }).catch(() => {
        this.$message({
          type: 'info',
          message: '取消输入'
        })
      })
    },
    // 修改用户密码
    async ClickPasswordEdit() {
      await this.$prompt('请输入新密码', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        inputPattern: /^.{0,20}$/,
        inputErrorMessage: '邮箱长度必须超过20个字符'
      }).then(({ value }) => {
        this.form.Password = value
        this.EditInfo()
      }).catch(() => {
        this.$message({
          type: 'info',
          message: '取消输入'
        })
      })
    },
    // 初始化数据
    async InitUserInfo() {
      await GetInfoById(this.uid).then(result => {
        console.log(result.data)
        this.form.Email = result.data.email
        this.form.LoginName = result.data.loginName
        this.form.UserName = result.data.userName
        this.form.Password = result.data.password
        this.form.Id = result.data.id
      }).catch(response => {
        console.error(response)
      }).finally(() => {
        this.isLoading = false
      })
    },
    // 修改数据
    async EditInfo() {
      await EditInfo(this.form).then(result => {
        // console.log(result.data.result)
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
        }
      }).catch(response => {
        this.$message({
          type: 'success',
          message: '修改失败'
        })
      })
    },
    // 显示用户角色文本
    RoleText(role) {
      if (role.includes('superadmin')) {
        return '超级管理员'
      }

      if (role.includes('admin')) {
        return '管理员'
      }

      if (role.includes('user')) {
        return '普通用户'
      }

      if (role.includes('visitor')) {
        return '访客'
      }
    }
  }
}
</script>

<style scoped>
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
.imagebox {
  height: 200px;
  background: url('../temp/IMG_5726.jpg');
  background-size: 100%;
 background-position: center center;
}
.avatar-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100%;
}
.mylable {
  font-weight: 600;
  color: gray;
  font-size: 17px;
  padding-bottom: 4px;
  font-family: "Helvetica Neue",Helvetica,"PingFang SC","Hiragino Sans GB","Microsoft YaHei","微软雅黑",Arial,sans-serif;
}
.mytext {
  font-family: "Helvetica Neue",Helvetica,"PingFang SC","Hiragino Sans GB","Microsoft YaHei","微软雅黑",Arial,sans-serif;
  font-size: 16px;
  padding-right: 8px;
  max-width: 100%; /* 确保元素宽度不超过父元素 */
  white-space: nowrap; /* 禁止换行 */
  overflow: hidden;
  text-overflow: ellipsis;
}
</style>
