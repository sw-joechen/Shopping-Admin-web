import request from '../Utils/request';

export const GetMembersList = (data) => {
  return request({
    method: 'post',
    url: '/api/member/getMembersList',
    data,
  })
    .then((res) => {
      return JSON.parse(res.data);
    })
    .catch((err) => {
      console.log('err: ', err);
    });
};

export const UpdateMember = (data) => {
  return request({
    method: 'post',
    url: '/api/member/updateMember',
    data,
  })
    .then((res) => {
      return JSON.parse(res.data);
    })
    .catch((err) => {
      console.log('err: ', err);
    });
};

export const Deposit = (data) => {
  return request({
    method: 'post',
    url: '/api/member/deposit',
    data,
  })
    .then((res) => {
      return JSON.parse(res.data);
    })
    .catch((err) => {
      console.log('err: ', err);
    });
};

export const GetMemberPurchaseHistory = (data) => {
  return request({
    method: 'post',
    url: '/api/member/getMemberPurchaseHistory',
    data,
  })
    .then((res) => {
      return JSON.parse(res.data);
    })
    .catch((err) => {
      console.log('err: ', err);
    });
};
