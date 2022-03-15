// 帳號規則: 英文開頭, 英數皆可 限6~20字元
const accountRegex = new RegExp("^[A-Za-z][A-Za-z0-9]{5,19}");

export const isAccountValid = (account) => {
  return accountRegex.test(account);
};

// 密碼規則: 6 位數以上，並且至少包含大寫字母、小寫字母、數字各一
const pwdRegex = new RegExp(
  "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{6,}$"
);

export const isPwdValid = (pwd) => {
  return pwdRegex.test(pwd);
};
