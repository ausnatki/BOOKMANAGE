<template>
  <div class="mybox">
    <el-table
      v-loading="loading"
      :data="TableData"
      border
      style="width: 100%"
    >
      <el-table-column
        label="编号"
        width="100"
      >
        <template slot-scope="scope">
          {{ scope.$index+1 }}
        </template>
      </el-table-column>
      <el-table-column
        prop="id"
        label="ID"
        width="500"
      />
      <el-table-column
        prop="service"
        label="服务名称"
        width="350"
      />
      <el-table-column
        prop="address"
        label="服务Ip地址"
      />

      <el-table-column label="重试操作">
        <template slot-scope="scope">
          <el-button type="primary" plain @click="testRetry(scope.row.id)">重试</el-button>
        </template>
      </el-table-column>
      <el-table-column label="超时">
        <template slot-scope="scope">
          <el-button type="primary" plain @click="testTimeout(scope.row.id)">超时</el-button>
        </template>
      </el-table-column>
      <el-table-column label="回退">
        <template slot-scope="scope">
          <el-button type="primary" plain @click="testFallback(scope.row.id)">回退</el-button>
        </template>
      </el-table-column>
      <el-table-column label="组合策略">
        <template slot-scope="scope">
          <el-button type="primary" plain @click="testWrapPolicy(scope.row.id)">组合策略</el-button>
        </template>
      </el-table-column>
      <el-table-column label="熔断降级">
        <template slot-scope="scope">
          <el-button type="primary" plain @click="testWrapPolicCircuitBreaker(scope.row.id)">熔断降级</el-button>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script>
import { GetConsul,
  TestFallback,
  TestRetry,
  TestTimeOut,
  TestWrapPolicy,
  TestWrapPolicCircuitBreaker } from '@/api/consul.js'
export default {
  name: 'ConsulIndex',
  data() {
    return {
      TableData: null,
      loading: true
    }
  },
  mounted() {
    this.GetTableData()
  },
  methods: {
    GetTableData() {
      GetConsul().then(result => {
        // console.log(result)
        this.TableData = result.data
      }).catch(response => {
        console.error(response)
      }).finally(() => {
        this.loading = false
      })
    },
    // 重试策略方法
    testRetry(id) {
      console.log(id)
      TestRetry(id).then(result => {
        console.log(result)
        if (result.result) {
          this.$message({
            message: '重试策略成功',
            type: 'success'
          })
        }
      }).catch(response => {
        console.error(response)
        if (response.result) {
          this.$message({
            message: '重试策略失败',
            type: 'error'
          })
        }
      })
    },
    // 回退策略
    testFallback(id) {
      TestFallback(id).then(result => {
        console.log(result)
        if (result.result) {
          this.$message({
            message: '回退策略成功',
            type: 'success'
          })
        }
      }).catch(response => {
        console.error(response)
        if (response.result) {
          this.$message({
            message: '回退策略失败',
            type: 'error'
          })
        }
      })
    },
    // 超时策略
    testTimeout(id) {
      TestTimeOut(id).then(result => {
        console.log(result)
        if (result.result) {
          this.$message({
            message: '超时策略成功',
            type: 'success'
          })
        }
      }).catch(response => {
        console.error(response)
        if (response.result) {
          this.$message({
            message: '超时策略失败',
            type: 'error'
          })
        }
      })
    },
    // 组合策略
    testWrapPolicy(id) {
      TestWrapPolicy(id).then(result => {
        console.log(result)
        if (result.result) {
          this.$message({
            message: '组合策略成功',
            type: 'success'
          })
        }
      }).catch(response => {
        console.error(response)
        if (response.result) {
          this.$message({
            message: '组合策略失败',
            type: 'error'
          })
        }
      })
    },
    // 熔断降级
    testWrapPolicCircuitBreaker(id) {
      TestWrapPolicCircuitBreaker(id).then(result => {
        console.log(result)
        if (result.result) {
          this.$message({
            message: result.msg,
            type: 'success'
          })
        }
      }).catch(response => {
        console.error(response)
        if (response.result) {
          this.$message({
            message: '熔断策略失败',
            type: 'error'
          })
        }
      })
    }
  }
}
</script>

<style>
.mybox{
  padding: 10px;
}
</style>
