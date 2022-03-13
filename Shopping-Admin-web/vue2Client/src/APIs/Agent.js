import request from "../Utils/request";

export const registerAgent = (data) => {
  return request({
    method: "post",
    url: "/api/agent/registerAgent",
    data,
  })
    .then((res) => {
      return JSON.parse(res.data);
    })
    .catch((err) => {
      console.log("err: ", err);
    });
};

export const LoginAgent = (data) => {
  return request({
    method: "post",
    url: "/api/agent/loginAgent",
    data,
  })
    .then((res) => {
      return JSON.parse(res.data);
    })
    .catch((err) => {
      console.log("err: ", err);
    });
};

export const getAgentsList = () => {
  return request({
    method: "post",
    url: "/api/agent/getAgentsList",
  })
    .then((res) => {
      return JSON.parse(res.data);
    })
    .catch((err) => {
      console.log("err: ", err);
    });
};
