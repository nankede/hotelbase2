var Untitl = {
    DateFormt: function (obj, format) {
        if (!obj) return '';
        obj = obj.replace('Date', '').replace('(', '').replace(')', '').replace(/\//g, '');
        obj = parseInt(obj);
        if (obj < 0) return '';
        obj = new Date(obj);
        format = format || 'yyyy-MM-dd HH:mm:ss';
        var date = {
            "M+": obj.getMonth() + 1,
            "d+": obj.getDate(),
            "h+": obj.getHours() % 12 == 0 ? 12 : obj.getHours() % 12,
            "H+": obj.getHours(),
            "m+": obj.getMinutes(),
            "s+": obj.getSeconds(),
            "q+": Math.floor((obj.getMonth() + 3) / 3),
            "S+": obj.getMilliseconds()
        };
        if (/(y+)/i.test(format)) {
            format = format.replace(RegExp.$1, (obj.getFullYear() + '').substr(4 - RegExp.$1.length));
        }
        for (var k in date) {
            if (new RegExp("(" + k + ")").test(format)) {
                format = format.replace(RegExp.$1, RegExp.$1.length == 1
                    ? date[k] : ("00" + date[k]).substr(("" + date[k]).length));
            }
        }
        return format;
    },

    Post: function (url, data, succFun, failFun) {
        $.ajax({
            url: url,
            type: 'post',
            dataType: "json",
            data: data,
            timeout: 20000,
            success: function (data) {
                if (succFun && typeof (succFun) == 'function') {
                    succFun(data)
                }
            },
            error: function () {
                console.log("err:" + url);
                if (failFun && typeof (failFun) == 'function') {
                    failFun()
                }
            },
            complete: function () {
                eLoading.hide();
            }
        });
    },
    Get: function (url, data, succFun, failFun) {
        $.ajax({
            url: url,
            type: 'get',
            dataType: "json",
            data: data,
            timeout: 20000,
            success: function (data) {
                if (succFun && typeof (succFun) == 'function') {
                    succFun(data)
                }
            },
            error: function () {
                console.log("err:" + url);
                if (failFun && typeof (failFun) == 'function') {
                    failFun()
                }
            },
            complete: function () {
                eLoading.hide();
            }
        });
    },
    //»ñÈ¡×Öµä
    GetDicList: function (pId, succFun) {
        $.ajax({
            url: '/system/getdiclistbypcode/?pCode=' + pId,
            type: 'get',
            dataType: "json",
            timeout: 20000,
            success: function (data) {
                if (succFun && typeof (succFun) == 'function') {
                    succFun(data)
                }
            },
            error: function () {
                console.log("err-save");
                if (failFun && typeof (failFun) == 'function') {
                    failFun()
                }
            },
            complete: function () {
                eLoading.hide();
            }
        });
    }

};