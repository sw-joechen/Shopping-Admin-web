import request from '../Utils/request';

export const RegisterAgent = (data) => {
  return request({
    method: 'post',
    url: '/api/agent/registerAgent',
    data,
  })
    .then((res) => {
      return JSON.parse(res.data);
    })
    .catch((err) => {
      console.log('err: ', err);
    });
};

export const LoginAgent = (data) => {
  return request({
    method: 'post',
    url: '/api/agent/loginAgent',
    data,
  })
    .then((res) => {
      return JSON.parse(res.data);
    })
    .catch((err) => {
      console.log('err: ', err);
    });
};

export const GetAgentsList = () => {
  return request({
    method: 'post',
    url: '/api/agent/getAgentsList',
  })
    .then((res) => {
      return JSON.parse(res.data);
    })
    .catch((err) => {
      console.log('err: ', err);
    });
};

export const GetAgentByAccount = (data) => {
  return request({
    method: 'post',
    url: '/api/agent/getAgentByAccount',
    data,
  })
    .then((res) => {
      return JSON.parse(res.data);
    })
    .catch((err) => {
      console.log('err: ', err);
    });
};

export const GetAgentsListByStatus = (data) => {
  return request({
    method: 'post',
    url: '/api/agent/getAgentsListByStatus',
    data,
  })
    .then((res) => {
      return JSON.parse(res.data);
    })
    .catch((err) => {
      console.log('err: ', err);
    });
};

export const UpdateAgent = (data) => {
  return request({
    method: 'post',
    url: '/api/agent/updateAgent',
    data,
  })
    .then((res) => {
      return JSON.parse(res.data);
    })
    .catch((err) => {
      console.log('err: ', err);
    });
};

export const UnlockAgent = (data) => {
  return request({
    method: 'post',
    url: '/api/agent/UnlockAgent',
    data,
  })
    .then((res) => {
      return JSON.parse(res.data);
    })
    .catch((err) => {
      console.log('err: ', err);
    });
};
