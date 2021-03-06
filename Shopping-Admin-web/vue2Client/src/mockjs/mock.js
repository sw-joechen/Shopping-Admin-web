import Mock from 'mockjs';

const getAgentsList = (payload) => {
  let tempData = [];
  if (payload.body) {
    const params = JSON.parse(payload.body);
    if (params.account) {
      tempData.push({
        id: 1,
        account: params.account,
        enabled: 1,
        createdDate: '2022-03-07T15:41:06.280',
        updatedDate: '2022-03-07T15:41:06.280',
        pwd: 'dwqopdkpoqw',
        count: 0,
      });
    }
  } else {
    tempData = [
      {
        id: 1,
        account: 'a12345',
        enabled: 0,
        createdDate: '2022-03-07T15:41:06.280',
        updatedDate: '2022-03-07T15:41:06.280',
        count: 0,
      },
      {
        id: 240,
        account: 'a11111',
        enabled: 0,
        createdDate: '2022-03-07T15:41:06.280',
        updatedDate: '2022-03-07T15:41:06.280',
        count: 0,
      },
      {
        id: 26,
        account: 'a22222',
        enabled: 0,
        createdDate: '2022-03-07T15:41:06.280',
        updatedDate: '2022-03-07T15:41:06.280',
        count: 0,
      },
      {
        id: 13,
        account: 'a33333',
        enabled: 1,
        createdDate: '2022-03-07T15:59:16.037',
        updatedDate: '2022-03-07T15:59:16.037',
        count: 2,
      },
      {
        id: 97,
        account: 'a44444',
        enabled: 1,
        createdDate: '2022-03-07T15:59:16.037',
        updatedDate: '2022-03-07T15:59:16.037',
        count: 3,
      },
    ];
  }
  const result = {
    code: 200,
    msg: 'success',
    data: tempData,
  };
  return JSON.stringify(result);
};

const GetAgentsListByStatus = () => {
  const tempData = [
    {
      id: 1,
      account: 'a12345',
      enabled: 0,
      createdDate: '2022-03-07T15:41:06.280',
      updatedDate: '2022-03-07T15:41:06.280',
      count: 0,
    },
    {
      id: 240,
      account: 'a11111',
      enabled: 0,
      createdDate: '2022-03-07T15:41:06.280',
      updatedDate: '2022-03-07T15:41:06.280',
      count: 0,
    },
    {
      id: 26,
      account: 'a22222',
      enabled: 0,
      createdDate: '2022-03-07T15:41:06.280',
      updatedDate: '2022-03-07T15:41:06.280',
      count: 0,
    },
    {
      id: 13,
      account: 'a33333',
      enabled: 1,
      createdDate: '2022-03-07T15:59:16.037',
      updatedDate: '2022-03-07T15:59:16.037',
      count: 2,
    },
    {
      id: 97,
      account: 'a44444',
      enabled: 1,
      createdDate: '2022-03-07T15:59:16.037',
      updatedDate: '2022-03-07T15:59:16.037',
      count: 3,
    },
  ];
  const result = {
    code: 200,
    msg: 'success',
    data: tempData,
  };
  return JSON.stringify(result);
};

const loginAgent = () => {
  const result = {
    code: 200,
    msg: 'success',
    data: {
      account: 'a12345',
      role: '',
    },
  };
  return JSON.stringify(result);
};

const registerAgent = () => {
  const result = {
    code: 200,
    msg: 'success',
  };
  return JSON.stringify(result);
};

const updateAgent = (payload) => {
  const params = JSON.parse(payload.body);
  const result = {
    code: 200,
    msg: 'success',
    data: {
      account: params.account,
      role: params.role,
      enabled: params.enabled,
    },
  };
  return JSON.stringify(result);
};

const UnlockAgent = (payload) => {
  const params = JSON.parse(payload.body);
  const result = {
    code: 200,
    msg: 'success',
    data: {
      account: params.account,
    },
  };
  return JSON.stringify(result);
};

const GetProductsList = () => {
  const result = {
    code: 200,
    msg: 'success',
    data: [
      {
        id: 1,
        name: 'aaaaabbbbbaaaaabbbbbaaaaabbbbbaaaaabbbbbaaaaabbbbb',
        amount: 1,
        createdDate: '2022-03-17T15:43:55.653',
        description:
          'xxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyy',
        enabled: true,
        picture: 'https://picsum.photos/200',
        price: 15,
        type: 't1',
        updatedDate: '2022-03-17T15:43:55.653',
      },
      {
        id: 14,
        name: 'test',
        amount: 1,
        createdDate: '2022-03-17T15:43:55.653',
        description: 'desc',
        enabled: true,
        picture: 'https://picsum.photos/200',
        price: 5,
        type: 't1',
        updatedDate: '2022-03-17T15:43:55.653',
      },
      {
        id: 15,
        amount: 243,
        createdDate: '2022-03-17T15:43:55.653',
        description: 'desc2',
        enabled: false,
        name: 'test2',
        picture: 'https://picsum.photos/200',
        price: 1,
        type: 'type2',
        updatedDate: '2022-03-17T15:43:55.653',
      },
      {
        id: 17,
        name: 'aaa aabb b bbaaa abbbbba aabbbbbaaaa bbbaa aa b bb',
        amount: 1,
        createdDate: '2022-03-17T15:43:55.653',
        description:
          'xxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyy',
        enabled: true,
        picture: 'https://picsum.photos/200',
        price: 100,
        type: 't1',
        updatedDate: '2022-03-17T15:43:55.653',
      },
    ],
  };
  return JSON.stringify(result);
};

const AddProduct = () => {
  // const params = JSON.parse(payload.body);
  const result = {
    code: 200,
    msg: 'success',
    data: {},
  };
  return JSON.stringify(result);
};

const GetMemberPurchaseHistory = (payload) => {
  const params = JSON.parse(payload.body);
  const result = {
    code: 200,
    msg: 'success',
    data: [
      {
        orderNumber: '22041307454400001011',
        account: params.account,
        phone: '0987654321',
        address: 'address ????????????????????????????????????87???1???',
        createdDate: '2022-04-13T07:45:44.666',
        shoppingList: [
          { id: 14, name: 'demo1', price: 101, count: 20 },
          { id: 15, name: 'demo2', price: 102, count: 100 },
          { id: 17, name: 'demo3', price: 103, count: 1000 },
        ],
      },
    ],
  };
  return JSON.stringify(result);
};

const GetMembersList = () => {
  const tempData = [];

  for (let i = 0; i < 50; i++) {
    tempData.push({
      id: i,
      account: 'b123456',
      address: 'bb',
      balance: getRandom(0, 100),
      createdDate: '2022-03-26T16:01:49.477',
      email: 'abc@123.com',
      enabled: getRandom(0, 1) ? false : true,
      gender: getRandom(0, 1),
      phone: '0978123456',
      updatedDate: '2022-03-26T16:01:49.477',
    });
  }
  const result = {
    code: 200,
    msg: 'success',
    data: tempData,
  };
  return JSON.stringify(result);
};

const UpdateMember = (payload) => {
  let tempData = {};
  if (payload.body) {
    for (var pair of payload.body.entries()) {
      console.log('pair[0]=> ', pair[0]);
      console.log('pair[1]=> ', pair[1]);
      tempData[pair[0]] = pair[1];
    }
    tempData.id = Number(tempData.id);
    tempData.enabled = tempData.enabled === 'true';
  }
  const result = {
    code: 200,
    msg: 'success',
    data: tempData,
  };
  return JSON.stringify(result);
};

if (process.env.NODE_ENV === 'development') {
  // agent
  Mock.mock('/api/agent/getAgentsList', getAgentsList);

  Mock.mock('/api/agent/getAgentsListByStatus', GetAgentsListByStatus);

  Mock.mock('/api/agent/loginAgent', loginAgent);

  Mock.mock('/api/agent/registerAgent', registerAgent);

  Mock.mock('/api/agent/updateAgent', updateAgent);

  Mock.mock('/api/agent/UnlockAgent', UnlockAgent);

  // product
  Mock.mock('/api/product/GetProductsList', GetProductsList);

  Mock.mock('/api/product/AddProduct', AddProduct);

  // member
  Mock.mock('/api/member/getMembersList', GetMembersList);

  Mock.mock('/api/member/updateMember', UpdateMember);

  Mock.mock('/api/member/getMemberPurchaseHistory', GetMemberPurchaseHistory);
}

//??????min???max???????????????
const getRandom = (min, max) => {
  return Math.floor(Math.random() * (max - min + 1)) + min;
};
