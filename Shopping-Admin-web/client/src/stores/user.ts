import { defineStore } from 'pinia'
import Cookies from 'js-cookie'
import { DateTime } from "luxon";

export const useUserStore = defineStore({
    id: 'user',
    state: () => ({
        account: Cookies.get('admin_account') || "",
        role: Cookies.get('admin_role') || "",
        isLoggedIn: Cookies.get('admin_account') ? true : false
    }),
    actions: {
        setUser(payload: {
            account: string,
            role: string
        }) {
            const { account, role } = payload
            this.account = account
            this.role = role
            this.isLoggedIn = true

            // TODO: 有效時間拉到server給
            const expiredDatetime = DateTime.now().plus({ day: 7 }).toISO()
            Cookies.set('admin_account', account, { expires: new Date(expiredDatetime) })
        }
    }
})
