import Mock from 'mockjs'

function genResult() {
  const result = {
    code: 200,
    msg: 'success',
    data:{
      account:"a33333",
      role:""
    }
  }
  return JSON.stringify(result)
}

if(import.meta.env.DEV){
  // 拦截该url
  Mock.mock('/api/agent/loginAgent', genResult) 
}

export default genResult