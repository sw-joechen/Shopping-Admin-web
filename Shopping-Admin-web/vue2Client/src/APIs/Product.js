import request from "../Utils/request";

export const GetProductsList = (data) => {
  return request({
    method: "post",
    url: "/api/product/GetProductsList",
    data,
  })
    .then((res) => {
      return JSON.parse(res.data);
    })
    .catch((err) => {
      console.log("err: ", err);
    });
};

export const AddProduct = (data) => {
  return request({
    method: "post",
    headers: {
      "Content-Type": "multipart/form-data",
    },
    url: "/api/product/AddProduct",
    data,
  })
    .then((res) => {
      return JSON.parse(res.data);
    })
    .catch((err) => {
      console.log("err: ", err);
    });
};

export const UpdateProduct = (data) => {
  console.log("data=> ", JSON.stringify(data));
  return request({
    method: "post",
    headers: {
      "Content-Type": "multipart/form-data",
    },
    url: "/api/product/UpdateProduct",
    data,
  })
    .then((res) => {
      return JSON.parse(res.data);
    })
    .catch((err) => {
      console.log("err: ", err);
    });
};
