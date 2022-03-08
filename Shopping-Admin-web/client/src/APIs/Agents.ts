import {request, IReqResult} from '../utils/request'

export interface IAccount{
    account:string,
    pwd:string
}

export interface ILoginAgentRes extends IReqResult{
    data:{
        account:string,
        role:string
    }
}

export const loginAgent = (data:IAccount):Promise<ILoginAgentRes>=>{
    return request({
        method:'post',
        url: '/api/agent/loginAgent',
        data
    })
    .then((res)=>{
        return JSON.parse(res.data)
    })
    .catch((err)=>{
        console.log('err: ',err)
    })
}