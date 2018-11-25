
var canlendarMainFn = {
    defaultDate: new Date(), //初始化默认加载当前年月下的日历
    defaultYear: new Date().getFullYear(), //当前年份
    defaultMonth: new Date().getMonth(), //当前月份
    chooseDate: '', //点击选择日历选择的日期，头部日历
    calendarDate: '', //弹出框日历，选则的时间
    packageIds: [], // 获取资源的价格日历框需要传入选中的资源id集合
    pCalendarList:[],
    /**
     * 获取星期
     */
    getWeek: function getWeek(day) {
        var week = "";
        switch (day) {
            case 0:
                week = "日";
                break;
            case 1:
                week = "一";
                break;
            case 2:
                week = "二";
                break;
            case 3:
                week = "三";
                break;
            case 4:
                week = "四";
                break;
            case 5:
                week = "五";
                break;
            case 6:
                week = "六";
                break;
        }
        return week;
    },
    /**
     *
     * 日期格式化
     * @param {any} fmt
     * @returns
     */
    dateformat: function dateformat(date, fmt) {
        var o = {
            "M+": date.getMonth() + 1, //月份
            "d+": date.getDate(), //日
            "h+": date.getHours(), //小时
            "m+": date.getMinutes(), //分
            "s+": date.getSeconds(), //秒
            "q+": Math.floor((date.getMonth() + 3) / 3), //季度
            "S": date.getMilliseconds() //毫秒
        };
        if (/(y+)/.test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (date.getFullYear() + "").substr(4 - RegExp.$1.length));
        }
        for (var k in o) {
            if (new RegExp("(" + k + ")").test(fmt)) {
                fmt = fmt.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
            }
        }
        return fmt;
    },
    /**
     * 获取日期对象，应对`2017-11-07`Safari不兼容的情况。
     * @param  {[type]} time [description]
     * @return {[type]}      [description]
     */
    getDate: function getDate() {
        var time = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : new Date();

        if (time == '') return new Date();
        if (typeof time == 'string') {
            time = time.replace(/\-/ig, '/');
        }
        return new Date(time);
    },

    /**
    * 差几天
    * @param  {[type]} start [description]
    * @param  {[type]} end   [description]
    * @return {[type]}       [description]
    */
    diff: function diff(start, end) {
        start = this.getDate(start).getTime();
        end = this.getDate(end).getTime();

        var days = (end - start) / (24 * 60 * 60 * 1000);
        if (days >= 0) {
            days = Math.ceil(days);
        } else {
            days = Math.floor(days);
        }

        // 修复同天不同时的情况。
        if ((days == 1 || days == -1) && new Date(start).getDate() == new Date(end).getDate()) {
            days = 0;
        }
        return days;
    },

    /**
     * 创建月份列表，跟线上同步，渲染10个月
     */
    createMonthUl: function createMonthUl(id) {
        var monthArr = [];
        for (var i = 0; i < 12; i++) {
            var start = new Date(new Date().setDate(1));
            var next = new Date(start.setMonth(start.getMonth() + i));
            monthArr.push(this.dateformat(next, 'yyyy-MM-dd'));
        }

        var monthStr = '';
        for (var _i = 0; _i < monthArr.length; _i++) {
            var item = monthArr[_i];
            var classStr = !_i ? "active" : "";
            var month = new Date(item.replace(/-/g, '/')).getMonth();
            monthStr += '<li class="' + classStr + '" data-date="' + item + '" data-month="' + month + '">' + (month + 1) + '\u6708</li>';
        }
        var t = '#' + id + ' ' + ".sarCalendar_month_ul";
        $(t).html(monthStr);
    },
    /**
     * 获取公历节日信息
     */
    getFestivalInfoNS: function getFestivalInfoNS(date) {
        var festivalNsList = {
            "11": "元旦",
            // "214":"情人节",
            // "38":"妇女节",
            // "312":"植树节",
            // "41":"愚人节",
            "51": "劳动节",
            "54": "青年节",
            // "512":"护士节",
            "61": "儿童节",
            "71": "建党节",
            "81": "建军节",
            "910": "教师节",
            // "928":"孔子诞辰",
            "101": "国庆节",
            // "106":"老人节",
            // "1024":"联合国日",
            "1224": "平安夜",
            "1225": "圣诞节"
        };

        return festivalNsList[date] ? festivalNsList[date] : "";
    },
    /**
     * 获取农历节日信息
     */
    getFestival: function getFestival(date) {
        var festivals = { "2016-2-7": "除夕", "2016-2-8": "春节", "2016-2-22": "元宵", "2016-4-4": "清明", "2016-6-9": "端午", "2016-8-9": "七夕", "2016-9-15": "中秋", "2016-10-19": "重阳", "2017-1-27": "除夕", "2017-1-28": "春节", "2017-2-11": "元宵", "2017-4-4": "清明", "2017-5-30": "端午", "2017-8-28": "七夕", "2017-10-4": "中秋", "2017-10-28": "重阳", "2018-2-15": "除夕", "2018-2-16": "春节", "2018-3-2": "元宵", "2018-4-5": "清明", "2018-6-18": "端午", "2018-8-17": "七夕", "2018-9-24": "中秋", "2018-10-17": "重阳", "2019-2-4": "除夕", "2019-2-5": "春节", "2019-2-19": "元宵", "2019-4-5": "清明", "2019-6-7": "端午", "2019-8-7": "七夕", "2019-9-13": "中秋", "2019-10-7": "重阳", "2020-1-24": "除夕", "2020-1-25": "春节", "2020-2-8": "元宵", "2020-4-4": "清明", "2020-6-25": "端午", "2020-8-25": "七夕", "2020-10-1": "中秋", "2020-10-25": "重阳" };
        return festivals[date] ? festivals[date] : "";
    },
    /**
     * 创建日期列表
     * year:当前选择的年份
     * month:当前选择的月份
     */
    createDateUl: function createDateUl(year, month) {
        var day = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : '';
        var id = arguments[3];

        var me = canlendarMainFn;

        var firstDay = new Date(year, month, 1); //当前月第一天的日期
        var weekDay = firstDay.getDay(); //第一天是星期几
        //求当前月一共有多少天
        //new Date(year,month+1,0) ： month+1是下一个月，day为0代表的是上一个月的最后一天，即我们所需的当前月的最后一天。
        //getDate（）则返回这个日期对象是一个月中的第几天，我们由最后一天得知这个月一共有多少天
        year = parseInt(year);
        nextMonth = parseInt(month) + 1;
        var days = new Date(year, nextMonth, 0).getDate();

        var Currentmonth = nextMonth < 10 ? '0' + nextMonth : nextMonth; //为了拼值（当前月份）

        var dateStr = "";
        //输出第一天之前的空格
        for (var i = 0; i < weekDay; i++) {
            dateStr += "<li></li>";
        }
        for (var j = 1; j <= days; j++) {
            var today = j < 10 ? '0' + j : j; //当前日期（几号）
            var date = year + '-' + nextMonth + '-' + j;

            var festivalStr = me.getFestival(date) ? '<i class="i_festival">' + me.getFestival(date) + '</i>' : "";
            if (!festivalStr) {
                festivalStr = me.getFestivalInfoNS(nextMonth.toString() + j) ? '<i class="i_festival">' + me.getFestivalInfoNS(nextMonth.toString() + j) + '</i>' : "";
            }
            var festivalClass = festivalStr ? 'festival' : '';
            dateStr += '<li data-date= ' + year + '-' + Currentmonth + '-' + today + '>' + '<em class="em_date ' + festivalClass + '">' + j + festivalStr + '</em><em class="em_price"></em></li>';
        }
        //输出最后一天之后的空格
        var lastEmpty = 42 - (weekDay + days);
        for (var k = 0; k < lastEmpty; k++) {
            dateStr += "<li></li>";
        }

        var t = '#' + id + ' ' + ".sarCalendar_date_ul";
        $(t).html(dateStr);

        //获取结束日期
        var date2 = new Date(new Date()),
            year_end = '',
            month_end = "",
            date_end = "";
        date2.setMonth(new Date().getMonth() + 5);
        year_end = date2.getFullYear() < 10 ? '0' + date2.getFullYear() : date2.getFullYear();
        month_end = date2.getMonth() + 1 < 10 ? '0' + (date2.getMonth() + 1) : date2.getMonth() + 1;
        date_end = date2.getDate() < 10 ? '0' + date2.getDate() : date2.getDate();

        var endDate = year_end + '-' + month_end + '-' + date_end;

        var href = '';
        var calendarParams = {};

        this.pCalendarList.forEach(function (v) {
            var length = $('.sarCalendar_date_ul li').length,
                t = '#' + id + ' ' + ".sarCalendar_date_ul" + ' ' + 'li';
            for (var i = 0; i < length; i++) {
                if ($(t).eq(i).attr('data-date')) {
                    if (v.PriceDate.split(' ')[0] == $(t).eq(i).attr('data-date')) {
                        $(t).eq(i).children('.em_price').text('￥' + Math.floor(v.MinSalePrice));
                        $(t).eq(i).addClass('choose_date');
                    }
                }
            }
        });
    },
    /**
     * 更改月份
     */
    changeMonth: function changeMonth(id) {

        var me = this;

        //拼接区分是那边的点击事件（头部日历，弹窗框日历）
        var t = '#' + id + ' ' + ".sarCalendar_month_ul li";
        $(t).on("click", function () {
            $(t).removeClass("active");
            $(this).addClass("active");
            var selectYear = new Date($(this).data('date').replace(/-/g, '/')).getFullYear();
            $(".sarCalendar_yearText").html(selectYear + '\u5E74');
            var selectMonth = $(this).attr("data-month");
            me.createDateUl(selectYear, selectMonth, '', id);
            //更改选择日期
            me.clickDate();
        });
    },
    /**
     * 切换月份面板
     */
    slideMonthPanel: function slideMonthPanel(id) {
        var slideMonthPanel_id = '#' + id + ' ' + ".sarCalendar_month_ul";
        //前滑
        $(".sarCalendar_month_prev").on("click", function () {
            $(slideMonthPanel_id).animate({
                left: '0'
            });
        });
        //后滑
        $(".sarCalendar_month_next").on("click", function () {
            $(slideMonthPanel_id).animate({
                left: '-490px'
            });
        });
    },

    /**
     * 初始化日历结构
     */
    initCalendarHtml: function initCalendarHtml(id) {
        var selfObj = this;
        var calendarHtml = '<div id="sarCalendar">\
            <div class="sarCalendar_head">\
                <span class="sarCalendar_yearText"></span>\
                <div class="sarCalendar_month">\
                    <em class="sarCalendar_month_prev"></em>\
                    <div class="sarCalendar_month_bar">\
                        <ul class="sarCalendar_month_ul"></ul>\
                    </div>\
                    <em class="sarCalendar_month_next"></em>\
                </div>\
            </div>\
            <div class="sarCalendar_content">\
                <ul class="sarCalendar_week_ul">\
                    <li class="weekend_li">日</li>\
                    <li>一</li>\
                    <li>二</li>\
                    <li>三</li>\
                    <li>四</li>\
                    <li>五</li>\
                    <li class="weekend_li">六</li>\
                </ul>\
                <ul class="sarCalendar_date_ul"></ul>\
            </div>\
        </div>';
        $("#" + id).html(calendarHtml);
        var t = '#' + id + ' ' + '.sarCalendar_yearText';
        $(t).text(selfObj.defaultYear + '年');
        $(t).attr('data-year', selfObj.defaultYear);
    },

    //修改日期格式
    changeDate: function changeDate(date) {
        var d = new Date(date);
        var yy1 = d.getFullYear();
        var mm1 = d.getMonth() + 1;
        var dd1 = d.getDate();
        if (mm1 < 10) {
            mm1 = '0' + mm1;
        }
        if (dd1 < 10) {
            dd1 = '0' + dd1;
        }
        return yy1 + '-' + mm1 + '-' + dd1;
    },

    /**
    * 点击日历方法
    */
    clickDate: function clickDate(id) {

        $('.sarCalendar_date_ul li').on('click', function () {
            if ($(this).hasClass('choose_date')) {
                var selfObj = canlendarMainFn;
                if ($(this).parents('#calendar_contain').length) {
                    //如果是头部日历
                    selfObj.chooseDate = $(this).attr('data-date'); //将当前选择的日期存起来
                    // me.getLinePackagetoResources($(this).attr('data-date'));
                    $(this).addClass('calendar_active').siblings().removeClass('calendar_active');
                }

                if ($(this).parents('#calendar_contain_dialog').length) {
                    //如果是套餐弹窗框，选择完日历，让弹窗框消失,且套餐弹出框出现
                    selfObj.calendarDate = $(this).attr('data-date');
                    $('.calender_dialog').css('display', 'none');
                    // me.getDialogContent(selfObj.calendarDate);
                }
            }
            window._tcTraObj && window._tcTraObj._tcTrackEvent('tczbypc', 'tczbypc_order_details', 'price_calendar', '^' + $('.lineID_span em').data('lineid') + '^\u4EF7\u683C\u65E5\u5386\u6846^' + $(this).data('date') + '^');
        });
        $('.calender_dialog').on('click', function () {
            $(this).hide();
        });
        $('.calender_dialog_content').on('click', function (e) {
            e.stopPropagation();
        });
    },

    /**
     * 初始化
     * id 日历容器ID
     */
    init: function init(id) {
        canlendarMainFn.initCalendarHtml(id);
        canlendarMainFn.createMonthUl(id);
        canlendarMainFn.createDateUl(canlendarMainFn.defaultYear, canlendarMainFn.defaultMonth, ' ', id);
        canlendarMainFn.changeMonth(id);
        canlendarMainFn.slideMonthPanel(id);
        canlendarMainFn.clickDate();
    }
};