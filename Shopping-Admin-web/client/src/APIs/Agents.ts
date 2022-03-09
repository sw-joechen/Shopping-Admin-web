import { request, IReqResult } from '../utils/request'

export interface IAccount {
    account: string,
    pwd: string
}

export interface ILoginAgentRes extends IReqResult {
    data: {
        account: string,
        role: string
    }
}

export interface IGetAgentsListRes extends IReqResult {
    data: IAgent[]
}

export interface IAgent {
    account: string,
    enabled: number,
    createdDate: Date,
    updatedDate: Date,
    role: string
}

export const registerAgent = (data: IAccount): Promise<IReqResult> => {
    return request({
        method: 'post',
        url: '/api/agent/registerAgent',
        data
    })
        .then((res) => {
            return JSON.parse(res.data)
        })
        .catch((err) => {
            console.log('err: ', err)
        })
}

export const loginAgent = (data: IAccount): Promise<ILoginAgentRes> => {
    return request({
        method: 'post',
        url: '/api/agent/loginAgent',
        data
    })
        .then((res) => {
            return JSON.parse(res.data)
        })
        .catch((err) => {
            console.log('err: ', err)
        })
}

export const getAgentsList = (): Promise<IGetAgentsListRes> => {
    return request({
        method: 'post',
        url: '/api/agent/getAgentsList',
    })
        .then((res) => {
            return JSON.parse(res.data)
        })
        .catch((err) => {
            console.log('err: ', err)
        })
}