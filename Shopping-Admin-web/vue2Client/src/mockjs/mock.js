import Mock from "mockjs";

const getAgentsList = () => {
  const result = {
    code: 200,
    msg: "success",
    data: [
      {
        id: 1,
        account: "a12345",
        enabled: 0,
        createdDate: "2022/3/7 下午 03:41:06",
        updatedDate: "2022/3/7 下午 03:41:06",
      },
      {
        id: 240,
        account: "a11111",
        enabled: 0,
        createdDate: "2022/3/7 下午 03:54:53",
        updatedDate: "2022/3/7 下午 03:54:53",
      },
      {
        id: 26,
        account: "a22222",
        enabled: 0,
        createdDate: "2022/3/7 下午 03:59:16",
        updatedDate: "2022/3/7 下午 03:59:16",
      },
      {
        id: 13,
        account: "a33333",
        enabled: 1,
        createdDate: "2022/3/7 下午 04:40:51",
        updatedDate: "2022/3/7 下午 04:40:51",
      },
      {
        id: 97,
        account: "a33333",
        enabled: 1,
        createdDate: "2022/3/7 下午 04:40:51",
        updatedDate: "2022/3/7 下午 04:40:51",
      },
    ],
  };
  return JSON.stringify(result);
};

const loginAgent = () => {
  const result = {
    code: 200,
    msg: "success",
    data: {
      account: "a12345",
      role: "",
    },
  };
  return JSON.stringify(result);
};

const registerAgent = () => {
  const result = {
    code: 200,
    msg: "success",
  };
  return JSON.stringify(result);
};

if (process.env.NODE_ENV) {
  // 拦截该url
  Mock.mock("/api/agent/getAgentsList", getAgentsList);

  Mock.mock("/api/agent/loginAgent", loginAgent);

  Mock.mock("/api/agent/registerAgent", registerAgent);
}
