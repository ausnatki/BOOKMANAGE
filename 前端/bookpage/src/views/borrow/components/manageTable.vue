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
          {{ scope.$index+1 }}
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
          <el-link class="linetext" type="primary" @click="ClickRepiad(scope.row)">归还</el-link>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script>

export default {

}
</script>

<style scoped>

</style>
