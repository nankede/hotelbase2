(function ($, window, undefined) {
    // var d0 = new Date();
    var sarCalender = function (opts) {
        this.settings = $.extend({}, sarCalender.defaults, opts);
        this.init();
    };

    sarCalender.defaults = {
        container: '.calendar_contain',//日历容器名称
        setPos: 'body',//插入的位置
        calendarType: 1,//日历类型 1：价格日历 2：库存日历
        calendarList: [],//日历数据实体
        yearArray: [2018, 2019, 2020, 2021, 2022, 2023, 2024, 2025, 2026, 2027, 2028, 2029, 2030], //可选年份列表
        defaultYear: new Date().getFullYear(),//默认年份
        defaultMonth: new Date().getMonth(),//默认月份
        callBack: null
    };

    sarCalender.prototype = {
        init: function () {

            this.initCalendarHtml(this.settings.container);
            this.createMonthUl(this.settings.container, this.settings.defaultYear);

            this.slideMonthPanel(this.settings.container);
            // this.createDateUl(this.settings.defaultYear, this.settings.defaultMonth, ' ', this.settings.container);
            this.getDateListAjaxFn(this.settings.defaultYear, this.settings.defaultMonth);
            this.clickDate()
            // this.create();
        },
        create: function () {
            var _template = '<div class="fixbox" style="display: block; transform-origin: 0px 0px 0px; opacity: 1; transform: scale(1, 1);">\
                <div class="box">\
                    <img class="close" data-classname="close" src="https://file.40017.cn/img140017cnproduct/cn/s/2018/touch/index/close.png" alt="">\
                    <div class="content">\
                        <p class="title">景点玩乐旅游</p>\
                        <img class="erweima" src="https://file.40017.cn/img140017cnproduct/cn/s/2018/touch/index/erweima.jpg" alt="">\
                        <p class="select">选择景点-使用卡券-下单立减</p>\
                        <p class="card">长按保存/识别图片，微信扫一扫打卡</p>\
                    </div>\
                </div>\
            </div>';
            $(this.settings.setPos).append(_template);
            this.bindEvent();
        },
        bindEvent: function () {
            var that = this;
            $(that.settings.container).on("click", ".close", function () {
                $(that.settings.container).hide();
                if ($.isFunction(that.settings.callBack)) {
                    var className = $(this).attr("data-classname");
                    that.settings.callBack(className);
                }
            });
        },
        /**
         * 获取日历数据接口
         */
        getDateListAjaxFn: function (opYear, opMonth) {
            
            var that = this;
            var ajaxUrl = this.settings.ajaxUrl;
            var ajaxOptions = this.settings.ajaxOptions;
            that.settings.defaultYear = opYear;
            that.settings.defaultMonth = parseInt(opMonth);
            ajaxOptions.Month = opYear * 100 + (parseInt(opMonth) + 1)
           
            $.ajax({
                // url: ajaxUrl + "?RuleId=" + ajaxOptions.RuleId + "&Month=" + ajaxOptions.Month,
                url: ajaxUrl,
                type: 'get',
                dataType: "json",
                data: ajaxOptions,
                timeout: 20000,
                success: function (data) {
                    if (data && data.length > 0) {
                        that.settings.calendarList = data;
                        that.createDateUl(that.settings.defaultYear, that.settings.defaultMonth, ' ', that.settings.container);
                    }else{
                        that.settings.calendarList = [];
                        that.createDateUl(that.settings.defaultYear, that.settings.defaultMonth, ' ', that.settings.container);
                    }
                },
                error: function () {
                    console.log("err-getList");
                },
                complete: function () {
                    eLoading.hide();
                }
            });
        },
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
         * 创建月份列表，跟线上同步，渲染12个月
         */
        createMonthUl: function createMonthUl(id, year) {
            var that = this;
            var monthArr = [];
            var yearStr = year + '-01-01';
            for (var i = 0; i < 12; i++) {
                var start = new Date(new Date(yearStr).setDate(1));
                var next = new Date(start.setMonth(start.getMonth() + i));
                monthArr.push(this.dateformat(next, 'yyyy-MM-dd'));
            }

            var monthStr = '';
            for (var _i = 0; _i < monthArr.length; _i++) {
                var item = monthArr[_i];
                var classStr = _i == that.settings.defaultMonth ? "active" : "";
                var month = new Date(item.replace(/-/g, '/')).getMonth();
                monthStr += '<li class="' + classStr + '" data-date="' + item + '" data-month="' + month + '">' + (month + 1) + '\u6708</li>';
            }


            var t = '#' + id + ' ' + ".sarCalendar_month_ul";

            $(t).html(monthStr);
            if (that.settings.defaultMonth >= 6) {
                var slideMonthPanel_id = '#' + id + ' ' + ".sarCalendar_month_ul";
                $(slideMonthPanel_id).animate({
                    left: '-490px'
                });
            }
            that.changeMonth(that.settings.container);
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

            var me = this;

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
                dateStr += '<li data-date= ' + year + '-' + Currentmonth + '-' + today + '>' + '<em class="em_date ' + festivalClass + '">' + j + festivalStr + '</em><em class="em_price_sell em_price"></em><em class="em_price_close em_price"></em></li>';
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

            this.settings.calendarList.forEach(function (v) {
                var length = $('.sarCalendar_date_ul li').length,
                    t = '#' + id + ' ' + ".sarCalendar_date_ul" + ' ' + 'li';
                for (var i = 0; i < length; i++) {
                    if ($(t).eq(i).attr('data-date')) {
                        if (v.PriceDate.split(' ')[0] == $(t).eq(i).attr('data-date')) {
                            $(t).eq(i).attr('data-obj', JSON.stringify(v));
                            if (me.settings.calendarType == 1) {
                                $(t).eq(i).children('.em_price_sell').text('售卖:￥' + Math.floor(v.SellPrice));
                                $(t).eq(i).children('.em_price_close').text('结算:￥' + Math.floor(v.ContractPrice));
                            } else if (me.settings.calendarType == 2) {
                                $(t).eq(i).children('.em_price_sell').text('库存:￥' + Math.floor(v.Count));
                                $(t).eq(i).children('.em_price_close').text('保留:￥' + Math.floor(v.RetainCount));
                            }

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
                // $(".sarCalendar_yearText").html(selectYear + '\u5E74');
                var selectMonth = $(this).attr("data-month");
                // me.createDateUl(selectYear, selectMonth, '', id);
                me.getDateListAjaxFn(selectYear, selectMonth);
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
            var yearSelectHtml = '';

            selfObj.settings.yearArray.forEach(function (item) {
                if(item == selfObj.settings.defaultYear){
                    yearSelectHtml += '<option selected="selected">' + item + '</option>';
                }else{
                    yearSelectHtml += '<option>' + item + '</option>';
                }
            });
            var calendarHtml = '<div id="sarCalendar">\
                <div class="sarCalendar_head">\
                    <select class="sarCalendar_yearText">'+ yearSelectHtml + '</select>\
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
            $(t).attr('data-year', selfObj.defaultYear);
            this.yearChangeFn(selfObj.settings.container);
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
        clickDate: function clickDate() {
            var that = this;
            $('#' + that.settings.container).on('click', '.sarCalendar_date_ul li', function () {
                // if ($(this).hasClass('choose_date')) {
                $('.sarCalendar_date_ul li').removeClass('calendar_active');
                $(this).addClass('calendar_active');
                var selectDateInfo;
                selectDateInfo = $(this).attr('data-obj') ? JSON.parse($(this).attr('data-obj')) : { "Id": 0, "PriceDate": $(this).attr('data-date'), "SellPrice": 0, "ContractPrice": 0, "Count": 0, "RetainCount": 0 };
                // if (!selectDateInfo) {
                //     selectDateInfo = { "Id": 0, "PriceDate": $(this).attr('data-date'), "SellPrice": 0, "ContractPrice": 0, "Count": 0, "RetainCount": 0 }
                // }
                if ($.isFunction(that.settings.callBack)) {
                    that.settings.callBack(selectDateInfo);
                }

            });
        },

        /**
         * 更改年份
         */
        yearChangeFn: function (id) {
            var that = this;
            $('#' + that.settings.container).on('change', '.sarCalendar_yearText', function (e) {
                that.settings.defaultYear = $(this).val().toString();
                // that.defaultDate = 1;
                that.createMonthUl(id, $(this).val());
                // that.createDateUl(that.settings.defaultYear, that.settings.defaultMonth, '', that.settings.container);
                that.getDateListAjaxFn(that.settings.defaultYear, that.settings.defaultMonth);
            });
        },
    };

    var sarCalenderInit = function (opts) {
        return new sarCalender(opts);
    };

    window.sarCalenderInit = $.sarCalenderInit = sarCalenderInit;

})(jQuery, window, undefined);