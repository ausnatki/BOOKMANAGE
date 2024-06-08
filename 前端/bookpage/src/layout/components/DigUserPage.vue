<template>
  <el-dialog
    :visible.sync="dialogVisible"
    width="25%"
  >

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
      <el-col :span="12" :offset="7" style="padding-bottom:10px">
        <span class="mylable">用户名：</span>
        <span class="mytext">{{ form.name }}</span>
        <el-button type="primary" icon="el-icon-edit" size="mini" circle />
      </el-col>
    </el-row>
    <el-row :gutter="20">
      <el-col :span="10" :offset="2">
        <div class="mylable">登录名：</div>
        <span class="mytext">{{ form.name }}</span>
        <!-- <el-button type="primary" icon="el-icon-edit" size="mini" circle /> -->
      </el-col>
      <el-col :span="10" :offset="2">
        <div class="mylable">密码：</div>
        <span class="mytext">{{ form.password }}</span>
        <!-- <el-button type="primary" icon="el-icon-edit" size="mini" circle /> -->
      </el-col>
    </el-row>
    <el-row :gutter="20">
      <el-col :span="10" :offset="2">
        <div class="mylable">Email：</div>
        <span class="mytext">{{ form.email }}</span>
        <!-- <el-button type="primary" icon="el-icon-edit" size="mini" circle /> -->
      </el-col>
      <el-col :span="10" :offset="2" style="">
        <div class="mylable">角色：</div>
        <span class="mytext">{{ form.role }}</span>
        <!-- <el-button type="primary" icon="el-icon-edit" size="mini" circle /> -->
      </el-col>
    </el-row>
    <el-row :gutter="20">
      <el-col :span="20" :offset="2">
        <el-button type="success" style="width:100%;margin-top:35px">确定</el-button>
      </el-col>
    </el-row>
  </el-dialog>
</template>

<script>
export default {
  props: {
    digflage: {
      type: Boolean,
      require: true
    }
  },
  data() {
    return {
      dialogVisible: false,
      form: {
        name: '测试名称',
        email: 'ausnatki@outlook.com',
        password: '123456',
        role: 'admin'
      }
    }
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
  methods: {
    handleClose(done) {
      this.$confirm('确认关闭？')
        .then(_ => {
          done()
        })
        .catch(_ => {})
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
