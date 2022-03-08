import { defineStore } from 'pinia'
import Cookies from 'js-cookie'
import { DateTime } from "luxon";

export const useUserStore = defineStore({
    id: 'user',

    // TODO: 從cookie做續登
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

            const expiredDatetime = DateTime.now().plus({ minute: 1 }).toISO()
            Cookies.set('admin_account', account, { expires: new Date(expiredDatetime) })
        }
    }
})
