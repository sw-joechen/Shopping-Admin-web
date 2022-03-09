import Mock from 'mockjs'

const getAgentsList = () => {
  const result = {
    code: 200,
    msg: "success",
    data: [
      { account: "a12345", enabled: 0, createdDate: "2022/3/7 下午 03:41:06", updatedDate: "2022/3/7 下午 03:41:06", role: "" },
      { account: "a11111", enabled: 0, createdDate: "2022/3/7 下午 03:54:53", updatedDate: "2022/3/7 下午 03:54:53", role: "" },
      { account: "a22222", enabled: 0, createdDate: "2022/3/7 下午 03:59:16", updatedDate: "2022/3/7 下午 03:59:16", role: "" },
      { account: "a33333", enabled: 0, createdDate: "2022/3/7 下午 04:40:51", updatedDate: "2022/3/7 下午 04:40:51", role: "" }
    ]
  }
  return JSON.stringify(result)
}

const loginAgent = () => {
  const result = {
    code: 200,
    msg: "success",
    data: {
      account: "a12345",
      role: ""
    }
  }
  return JSON.stringify(result)
}


const registerAgent = () => {
  const result = {
    code: 200,
    msg: "success"
  }
  return JSON.stringify(result)
}

if (import.meta.env.DEV) {
  // 拦截该url
  Mock.mock('/api/agent/getAgentsList', getAgentsList)

  Mock.mock('/api/agent/loginAgent', loginAgent)

  Mock.mock('/api/agent/registerAgent', registerAgent)
}
