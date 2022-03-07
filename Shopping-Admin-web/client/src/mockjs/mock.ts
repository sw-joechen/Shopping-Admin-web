import Mock from 'mockjs'

function genResult() {
  const result = {
    code: 200,
    msg: 'success'
  }
  return JSON.stringify(result)
}

if(import.meta.env.DEV){
  // 拦截该url
  Mock.mock('/api/agent/loginAgent', genResult) 
}

export default genResult