import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

/* Layout */
import Layout from '@/layout'

// /* Router Modules */
// import componentsRouter from './modules/components'
// import chartsRouter from './modules/charts'
// import tableRouter from './modules/table'
// import nestedRouter from './modules/nested'

// import bak from './bak'

/**
 * Note: sub-menu only appear when route children.length >= 1
 * Detail see: https://panjiachen.github.io/vue-element-admin-site/guide/essentials/router-and-nav.html
 *
 * hidden: true                   if set true, item will not show in the sidebar(default is false)
 * alwaysShow: true               if set true, will always show the root menu
 *                                if not set alwaysShow, when item has more than one children route,
 *                                it will becomes nested mode, otherwise not show the root menu
 * redirect: noRedirect           if set noRedirect will no redirect in the breadcrumb
 * name:'router-name'             the name is used by <keep-alive> (must set!!!)
 * meta : {
    roles: ['admin','editor']    control the page roles (you can set multiple roles)
    title: 'title'               the name show in sidebar and breadcrumb (recommend set)
    icon: 'svg-name'/'el-icon-x' the icon show in the sidebar
    noCache: true                if set true, the page will no be cached(default is false)
    affix: true                  if set true, the tag will affix in the tags-view
    breadcrumb: false            if set false, the item will hidden in breadcrumb(default is true)
    activeMenu: '/example/list'  if set path, the sidebar will highlight the path you set
  }
 */

/**
 * constantRoutes
 * a base page that does not have permission requirements
 * all roles can be accessed
 */
// 静态路由
export const constantRoutes = [

  {
    path: '/redirect',
    component: Layout,
    hidden: true,
    children: [{
      path: '/redirect/:path(.*)',
      component: () => import('@/views/redirect/index')
    }]
  },
  {
    path: '/login',
    component: () => import('@/views/login/index'),
    hidden: true
  },
  {
    path: '/auth-redirect',
    component: () => import('@/views/login/auth-redirect'),
    hidden: true
  },
  {
    path: '/404',
    component: () => import('@/views/error-page/404'),
    hidden: true
  },
  {
    path: '/401',
    component: () => import('@/views/error-page/401'),
    hidden: true
  },
  {
    path: '/',
    component: Layout,
    redirect: '/dashboard',
    children: [{
      path: 'dashboard',
      component: () => import('@/views/dashboard/index'),
      name: 'Dashboard',
      meta: {
        title: 'dashboard',
        icon: 'dashboard',
        affix: true
      }
    }]
  },
  {
    path: '/guide',
    component: Layout,
    redirect: '/guide/index',
    children: [{
      path: 'index',
      component: () => import('@/views/guide/index'),
      name: 'Guide',
      meta: {
        title: 'guide',
        icon: 'guide',
        noCache: true
      }
    }]
  },
  {
    path: '/profile',
    component: Layout,
    redirect: '/profile/index',
    hidden: true,
    children: [{
      path: 'index',
      component: () => import('@/views/profile/index'),
      name: 'Profile',
      meta: {
        title: 'profile',
        icon: 'user',
        noCache: true
      }
    }]
  }
]

/**
 * asyncRoutes
 * the routes that need to be dynamically loaded based on user roles
 */
// 动态路由规则
export const asyncRoutes = [

  {
    path: '/user',
    component: Layout,
    redirect: '/user/list',
    alwaysShow: true,
    name: 'UserManage',
    meta: {
      title: '用户管理',
      icon: 'list',
      roles: ['admin', 'superadmin']
    },
    children: [{
      path: 'list',
      component: () => import('@/views/user/index'),
      name: 'userlist',
      meta: {
        title: '用户列表',
        icon: 'guide',
        noCache: true,
        roles: ['admin', 'superadmin']
      }
    }
    ]
  },
  // {
  //   path: '/consul',
  //   component: Layout,
  //   redirect: '/consul/index',
  //   alwaysShow: true,
  //   name: 'ConsulManager',
  //   meta: {
  //     title: 'Consul模块',
  //     icon: 'list',
  //     roles: ['superadmin']
  //   },
  //   children: [{
  //     path: 'index',
  //     component: () => import('@/views/consolelist/index'),
  //     name: 'consulIndex',
  //     meta: {
  //       title: 'Consul实验',
  //       icon: 'guide',
  //       noCache: true,
  //       roles: ['superadmin']
  //     }
  //   }
  //   ]
  // },
  {
    path: '/book',
    component: Layout,
    redirect: '/book/list',
    alwaysShow: true,
    name: 'BookList',
    meta: {
      title: '图书',
      icon: 'list',
      roles: ['admin', 'superadmin', 'user']
    },
    children: [{
      path: 'list',
      component: () => import('@/views/book/index'),
      name: 'list',
      meta: {
        title: '图书列表',
        icon: 'guide',
        noCache: true,
        roles: ['admin', 'user', 'superadmin']
      }
    },
    {
      path: 'inventory',
      component: () => import('@/views/book/inventory'),
      name: 'bookInventory',
      meta: {
        title: '图书库存',
        icon: 'guide',
        noCache: true,
        roles: ['admin', 'superadmin']
      }
    },
    {
      path: 'add',
      component: () => import('@/views/book/bookAdd'),
      name: 'bookAdd',
      meta: {
        title: '图书添加',
        icon: 'guide',
        noCache: true,
        roles: ['admin', 'superadmin']
      }
    }
    ]
  },
  {
    path: '/borrow',
    component: Layout,
    redirect: '/borrow/list',
    alwaysShow: true,
    name: 'BorrowList',
    meta: {
      title: '借阅',
      icon: 'list',
      roles: ['admin', 'superadmin', 'user']
    },
    children: [{
      path: 'list',
      component: () => import('@/views/borrow/index'),
      name: 'borrowpage',
      meta: {
        title: '我的借阅',
        icon: 'guide',
        noCache: true,
        roles: ['admin', 'superadmin', 'user']
      }
    },
    {
      path: 'censorlist',
      component: () => import('@/views/borrow/censorlist'),
      name: 'censorpage',
      meta: {
        title: '借阅审查',
        icon: 'guide',
        noCache: true,
        roles: ['admin', 'superadmin']
      }
    },
    {
      path: 'managelist',
      component: () => import('@/views/borrow/indexManage'),
      name: 'borrowpageManage',
      meta: {
        title: '借阅列表',
        icon: 'guide',
        noCache: true,
        roles: ['admin', 'superadmin']
      }
    }
    ]
  }

  // 自带模块

  // componentsRouter,
  // chartsRouter,
  // nestedRouter,
  // tableRouter,
  // ...bak,
]

const createRouter = () => new Router({
  // mode: 'history', // require service support
  scrollBehavior: () => ({
    y: 0
  }),
  routes: constantRoutes
})

const router = createRouter()

// Detail see: https://github.com/vuejs/vue-router/issues/1234#issuecomment-357941465
export function resetRouter() {
  const newRouter = createRouter()
  router.matcher = newRouter.matcher // reset router
}

export default router
