window.cashFlowAuthCookies = {
    set: function (name, value, expiresInDays) {
        const expires = new Date();
        expires.setDate(expires.getDate() + expiresInDays);

        document.cookie =
            `${encodeURIComponent(name)}=${encodeURIComponent(value)}; expires=${expires.toUTCString()}; path=/; SameSite=Strict`;
    },

    get: function (name) {
        const cookieName = `${encodeURIComponent(name)}=`;
        const cookies = document.cookie.split("; ");

        for (const cookie of cookies) {
            if (cookie.startsWith(cookieName)) {
                return decodeURIComponent(cookie.substring(cookieName.length));
            }
        }

        return null;
    },

    remove: function (name) {
        document.cookie = `${encodeURIComponent(name)}=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/; SameSite=Strict`;
    }
};
