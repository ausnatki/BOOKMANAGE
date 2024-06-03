<template>
  <div class="good-form">
    <el-page-header content="新增商品" @back="$router.back()" />

    <!-- 表单 -->
    <!-- 只要是收集用户填写的数据100%需要校验 -->
    <!-- 前端需要校验，后端更需要校验，困为后面需要把数据入库 -->
    <!-- 通过前后端校验，能不能保证数据就是合法，就是安全？ -->
    <!-- 有些数据，是不能通过前后端写代码校验的，可以使用AI -->
    <!-- AI在有时候也不行，最最最敏感的数据，需要人工校验 -->
    <!-- ref="ruleForm" ref写在组件上，肯定是为了获取组件vc -->
    <!--
      前端校验：
          1）必填与非必填
          2）数据类型
          3）数据格式，是否满足业务需要
      什么时候校验：
          1）失集时校验
          2）边输入边校验
          3）提交时校验

    -->
    <el-form
      ref="ruleForm"
      style="width: 600px; margin-top: 20px"
      :model="ruleForm"
      :rules="rules"
      label-width="100px"
      class="demo-ruleForm"
    >
      <!-- 商品名称 -->
      <!-- prop="name" 前端进行数据校验 -->
      <el-form-item label="商品名称" prop="name">
        <el-input v-model="ruleForm.name" />
      </el-form-item>

      <!-- 商品描述 -->
      <el-form-item label="商品描述" prop="desc">
        <el-input v-model="ruleForm.desc" type="texterea" rows="3" />
      </el-form-item>

      <!-- 商品分类 -->
      <el-form-item label="商品分类" prop="cate">
        <CateSelect />
      </el-form-item>

      <!-- 商品价格 -->
      <el-form-item label="商品价格" prop="price">
        <el-input-number
          v-model="ruleForm.price"
          :min="0"
          :step="0.1"
          :precision="2"
        />
      </el-form-item>

      <!-- 是否热销 -->
      <el-form-item label="是否热销" prop="hot">
        <el-switch v-model="ruleForm.hot" />
      </el-form-item>

      <!-- 商品图片 -->
      <el-form-item label="商品图片" prop="type">
        <!-- action 就是图片上传的接口 -->
        <ImgUpload />
      </el-form-item>

      <el-form-item>
        <el-button
          type="primary"
          @click="submitForm('ruleForm')"
        >提交</el-button>
        <el-button @click="resetForm('ruleForm')">重置</el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script>
import CateSelect from '@/components/CateSelect.vue'
import ImgUpload from '@/components/ImgUpload.vue'

export default {
  name: 'GoodForm',
  components: {
    CateSelect,
    ImgUpload
  },
  props: [],
  data() {
    return {
      ruleForm: {
        name: '',
        desc: '',
        cate: '',
        price: 0,
        hot: true,
        img: ''
      },
      rules: {
        // 对name这个字段进行校验
        name: [
          { required: true, message: '请输入商品名称', trigger: 'blur' },
          {
            pattern: /[\u4e00-\u9fa5]{4,8}/,
            message: '商品名称要求4~8个中文汉字',
            trigger: 'blur'
          }
        ],
        desc: [
          { required: true, message: '请填写商品介绍', trigger: 'blur' },
          {
            min: 20,
            max: 30,
            message: '商品名称要求20~30个字符',
            trigger: 'blur'
          }
        ],
        cate: [
          { required: true, message: '请选择商品品类', trigger: 'change' }
        ],
        price: [
          { required: true, message: '请填写商品价格', trigger: 'change' }
        ],
        img: [{ required: true, message: '请上传商品图片', trigger: 'change' }]
      }
    }
  },
  methods: {
    submitForm(formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          alert('submit!')
        } else {
          console.log('error submit!!')
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

<style lang="scss" scoped>
.good-form {
  box-sizing: border-box;
  padding: 20px;
}
</style>
