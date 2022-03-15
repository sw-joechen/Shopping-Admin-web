import Mock from "mockjs";

const getAgentsList = (payload) => {
  let tempData = [];
  if (payload.body) {
    const params = JSON.parse(payload.body);
    if (params.account) {
      tempData.push({
        id: 1,
        account: params.account,
        enabled: 1,
        createdDate: "2022-03-07T15:41:06.280",
        updatedDate: "2022-03-07T15:41:06.280",
        pwd: "dwqopdkpoqw",
      });
    }
  } else {
    tempData = [
      {
        id: 1,
        account: "a12345",
        enabled: 0,
        createdDate: "2022-03-07T15:41:06.280",
        updatedDate: "2022-03-07T15:41:06.280",
      },
      {
        id: 240,
        account: "a11111",
        enabled: 0,
        createdDate: "2022-03-07T15:41:06.280",
        updatedDate: "2022-03-07T15:41:06.280",
      },
      {
        id: 26,
        account: "a22222",
        enabled: 0,
        createdDate: "2022-03-07T15:41:06.280",
        updatedDate: "2022-03-07T15:41:06.280",
      },
      {
        id: 13,
        account: "a33333",
        enabled: 1,
        createdDate: "2022-03-07T15:59:16.037",
        updatedDate: "2022-03-07T15:59:16.037",
      },
      {
        id: 97,
        account: "a33333",
        enabled: 1,
        createdDate: "2022-03-07T15:59:16.037",
        updatedDate: "2022-03-07T15:59:16.037",
      },
    ];
  }
  const result = {
    code: 200,
    msg: "success",
    data: tempData,
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

const updateAgent = (payload) => {
  const params = JSON.parse(payload.body);
  const result = {
    code: 200,
    msg: "success",
    data: {
      account: params.account,
      role: params.role,
      enabled: params.enabled,
    },
  };
  return JSON.stringify(result);
};

if (process.env.NODE_ENV === "development") {
  // 拦截该url
  Mock.mock("/api/agent/getAgentsList", getAgentsList);

  Mock.mock("/api/agent/loginAgent", loginAgent);

  Mock.mock("/api/agent/registerAgent", registerAgent);

  Mock.mock("/api/agent/updateAgent", updateAgent);
}
