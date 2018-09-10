if (!Array.prototype.find) {
  Array.prototype.find = function(predicate) {
    'use strict';
    if (this == null) {
      throw new TypeError('Array.prototype.find called on null or undefined');
    }
    if (typeof predicate !== 'function') {
      throw new TypeError('predicate must be a function');
    }
    var list = Object(this);
    var length = list.length >>> 0;
    var thisArg = arguments[1];
    var value;

    for (var i = 0; i < length; i++) {
      value = list[i];
      if (predicate.call(thisArg, value, i, list)) {
        return value;
      }
    }
    return undefined;
  };
}

if (!Array.prototype.findIndex) {
  Array.prototype.findIndex = function(predicate) {
    if (this === null) {
      throw new TypeError('Array.prototype.findIndex called on null or undefined');
    }
    if (typeof predicate !== 'function') {
      throw new TypeError('predicate must be a function');
    }
    var list = Object(this);
    var length = list.length >>> 0;
    var thisArg = arguments[1];
    var value;

    for (var i = 0; i < length; i++) {
      value = list[i];
      if (predicate.call(thisArg, value, i, list)) {
        return i;
      }
    }
    return -1;
  };
}
;(function($, w) {
	var u = {};

	u.addDays = function(date, days) {
		date = date == null ? new Date() : new Date(date);
		date.setDate(date.getDate() + days);
		return date;
	};

	u.formatDate = function(date) {
		date = date == null ? new Date() : new Date(date);
		var year = date.getFullYear();
		var month = date.getMonth() + 1;
		var day = date.getDate();

		month = month < 10 ? '0' + month : month;
		day = day < 10 ? '0' + day : day;

		return {
			h: year + '-' + month + '-' + day,
			s: year + '/' + month + '/' + day
		};
	};

	u.getQueryString = function() {
		var qs = (location.search.length > 0 ? location.search.substring(1) : '');
	    var args = {};
	    var items = qs.length ? qs.split('&') : [];
	    var item = null;
	    var name = null;
	    var value = null;
	    var i = 0;
	    var len = items.length;
	    for (i = 0; i < len; i++) {
	        item = items[i].split('=');
	        name = decodeURIComponent(item[0]);
	        value = decodeURIComponent(item[1]);
	        if (name.length) {
	            args[name] = value;
	        }
	    }
	    return args;
	};

	u.createRootVariable = function() {
	    var app = {
	        /**
	         * 全局的配置对象，包含各个模块共用的常量
	         * !!!不允许隐性为config里添加属性
	         * !!!所有用到的config属性必须在该对象里先进行声明
	         * @type {Object}
	         */
	        config: {},

	        /**
	         * 全局的DOM事件，每个部分的DOM事件请写在自己的模块里
	         * 格式为：
	         * 'selector1&selector2': {
	         *     eventName1: function(root, e) {
	         *     },
	         *     eventName2: function(root, e) {
	         *     }
	         * }
	         * 上述代码表示：将选择器为selector2的元素的eventName1、eventName2事件委托到选择器为selector1的元素上
	         * event handler默认参数有两个，分别是root和e
	         * 
	         * @type {Object}
	         */
	        events: {},

	        /**
	         * 包含将页面拆分开的所有模块
	         * @type {Object}
	         */
	        modules: {},

	        /**
	         * 全局的初始化函数，会发布global.init事件，其他订阅该事件，实现各自模块的初始化
	         * 
	         */
	        init: function () {
	            var root = this;

	            //委托事件
	            root._delegate(root.events);

	            //创建各个js模块
	            root._loadModuleJs();

	            //发布全局的初始化事件
	            root.pubsub.fire('root.init', null);
	        },

	        /**
	         * 包含所有模块可用的公共工具函数
	         */
	        helpers: {},

	        /**
	         * 包含root的处理函数
	         * @type {Object}
	         */
	        handles: {},

	        /**
	         * 挂载每个模块的js对象的创建方法
	         * 
	         * 对应每个模块js的创建方法demo
	         * createModuleNameBoxModule 方法demo，该方法写在单个模块对应的js里面
	         *
	         * rootName.moduleCreateFns.createModuleNameBoxModule = function(root) {
	         *     root.modules.moduleNameBox = new root.Module(root, 'moduleNameBox');
	         *     var moduleRoot = root.modules.moduleNameBox;
	         *       
	         *     //不允许隐形为config里添加属性
	         *     //所有用到的config属性必须在该对象里面先进行声明
	         *     moduleRoot.config = {};
	         *
	         *     //dom事件必须委托到该section下
	         *     //一般情况下不允许委托到其他地方，尤其是类名下面
	         *     moduleRoot.events = {};
	         *     moduleRoot.handles = {};
	         *
	         *     //在这里放模块初始化时要做的操作
	         *     moduleRoot.pubsub.on('moduleNameBox.init', 'moduleNameBox', function() {
	         *     });  
	         * };
	         */
	        moduleCreateFns: {},

	        /**
	         * 用于生成每个模块的js对象
	         * @param {object} root 
	         * @param {string} name 
	         */
	        Module: function (root, name) {
	            var that = this;

	            this.config = {};

	            this.init = function () {
	                //每个模块默认调用委托事件的方法
	                root._delegate(that.events);
	            };

	            this.events = {};

	            this.handles = {};

	            this.pubsub = root._pubsub();

	            root.pubsub.on('root.init', name, function () {
	                that.init();

	                that.pubsub.fire(name + '.init');
	            });
	        },

	        /**
	         * 同步加载各个模块的html
	         * @param  {string} selector 选择器
	         * @param  {string} url      页面地址
	         */
	        loadModuleHtml: function (selector, url) {
	            $.ajax({
	                url: url,
	                cache: false,
	                async: false,
	                success: function (html) {
	                    $(selector).append(html);
	                }
	            });
	        },

	        /**
	         * 调用moduleCreateFns下的方法
	         */
	        _loadModuleJs: function () {
	            var root = this;
	            var fns = root.moduleCreateFns;

	            for (var key in fns) {
	                if (fns.hasOwnProperty(key)) {
	                    fns[key](root);
	                }
	            }
	        },

	        /**
	         * DOM事件委托，接受selector&selector的形式
	         * @private
	         */
	        _delegate: function (events) {
	            var root = this;
	            var events = events || {};
	            var eventObjs, fn, queryStr, type, parentNode, parentQuery, childQuery;

	            for (queryStr in events) {
	                if (events.hasOwnProperty(queryStr)) {
	                    eventObjs = events[queryStr];
	                }

	                for (type in eventObjs) {
	                    if (eventObjs.hasOwnProperty(type)) {
	                        fn = eventObjs[type];
	                        parentQuery = queryStr.split('&')[0];
	                        childQuery = queryStr.split('&')[1];
	                        if (parentQuery === 'window') {
	                            parentNode = $(window);
	                        }
	                        else {
	                            parentNode = $(parentQuery) || $('body');
	                        }

	                        if (parentQuery === childQuery) {
	                            parentNode.on(type, (function (fn) {
	                                return function (e) {
	                                    var args = Array.prototype.slice.call(arguments, 0);
	                                    var newThis = e.currentTarget;
	                                    args.unshift(root);
	                                    fn.apply(newThis, args);
	                                };
	                            })(fn));
	                        }
	                        else {
	                            parentNode.delegate(childQuery, type, (function (fn) {
	                                return function (e) {
	                                    var args = Array.prototype.slice.call(arguments, 0);
	                                    var newThis = e.currentTarget;
	                                    args.unshift(root);
	                                    fn.apply(newThis, args);
	                                };
	                            })(fn));
	                        }
	                    }
	                }
	            }
	        },
	        /**
	         * 发布订阅
	         * @return {[type]} [description]
	         */
	        _pubsub: function () {
	            return {
	                /**
	                 *注册事件
	                 *
	                 * @public
	                 * @param {string} [eventName] [事件名]
	                 * @param {string} [listenerName] [需要添加事件的对象]
	                 * @param {function()} [handler] [触发事件的相应处理函数]
	                 * @return {object} [实例对象]
	                 */
	                on: function (eventName, listenerName, handler) {
	                    if (!this._events) {
	                        this._events = {};
	                    }
	                    if (!this._events[eventName]) {
	                        this._events[eventName] = {};
	                    }
	                    if (this._events[eventName][listenerName] == null && typeof handler === 'function') {
	                        this._events[eventName][listenerName] = handler;
	                    }
	                    return this;
	                },

	                /**
	                 *触发事件
	                 *
	                 * @public
	                 * @param {string} [eventName] [事件名，由listenerName和eventName组成]
	                 * @return {object} [实例对象]
	                 */
	                fire: function (eventName, listenerName) {
	                    if (!this._events || !this._events[eventName]) { return; }

	                    var args = Array.prototype.slice.call(arguments, 2) || [];
	                    var listeners = this._events[eventName];
	                    if (listenerName == null) {
	                        for (var key in listeners) {
	                            listeners[key].apply(this, args);
	                        }
	                    }
	                    else {
	                        if (listeners.hasOwnProperty(listenerName)) {
	                            listeners[listenerName].apply(this, args);
	                        }
	                        else {
	                            return;
	                        }
	                    }

	                    return this;
	                },

	                /**
	                 *注销事件
	                 *
	                 *@public
	                 *@param {string} [eventName] [事件名]
	                 *@param {string} [listenerName] [需要添加事件的对象]
	                 *@return {object} [实例对象]
	                 */
	                off: function (eventName, listenerName) {
	                    if (!eventName && !listenerName) {
	                        this._events = {};
	                    }
	                    if (eventName && !listenerName) {
	                        delete this._events[eventName];
	                    }

	                    if (eventName && listenerName) {
	                        delete this._events[eventName];
	                    }
	                    return this;
	                }
	            };
	        }
	    };

	    //
	    app.pubsub = app._pubsub();

	    return app;
	};

	w.eui = {}
	w.eui.util = u;

})(jQuery, window);
;(function($, w) {

w.eAlert = function(options) {
    var opts = $.extend(true, {}, defaults, options);
    ea.init(opts);
};
w.eAlert.close = function($eAlert) {
    $eAlert.find('.eui-alert-close').trigger('click');
};

var defaults = {
    title: '',
    text: '',
    type: 'warning',
    alertHook: {
        type: '',
        value: ''
    },
    icon: {
        show: true
    },
    cancelButton: {
        show: true,
        text: '取消',
        hook: {
            type: '',
            value: ''
        }
    },
    confirmButton: {
        show: true,
        text: '确定',
        hook: {
            type: '',
            value: ''
        }
    },
    onClose: null,
    onCancel: null,
    onConfirm: null,
    buttons: []
};

var ea = {};
ea.render = function(opts) {
    var titleMap = {
        info: '提示',
        warning: '警告',
        error: '错误',
        success: '成功'
    };
    var title = opts.title == '' ? titleMap[opts.type] : opts.title;
    var text = opts.text;
    var alertIdAttr = opts.alertHook.type === 'id' ? opts.alertHook.value : '';
    var alertClassAttr = opts.alertHook.type === 'class' ? opts.alertHook.value : '';
    alertClassAttr += ' eui-alert-' + opts.type;


    var iconHtml = opts.icon.show ? '<div class="eui-alert-cont-icon"></div>' : '';


    var buttonsHtml = '';
    if (opts.buttons.length > 0) {
        opts.buttons.forEach(function(button, index) {
            var classAttr = button.hook.type === 'class' ? button.hook.value : '';
            var idAttr = button.hook.type === 'id' ? button.hook.value : '';
            buttonsHtml += '' +
                    '<button type="button" class="eui-btn eui-btn-secondary ' + classAttr + '" id="' + idAttr + '">' +
                        button.text +
                    '</button>';
        });
    }
    else {
        var cancelButtonClassAttr = opts.cancelButton.hook.type === 'class' ? opts.cancelButton.hook.value : '';
        var cancelButtonIdAttr = opts.cancelButton.hook.type === 'id' ? opts.cancelButton.hook.value : '';
        var cancelButtonHtml = opts.cancelButton.show ?
                '<button type="button" class="eui-btn eui-btn-cancel eui-alert-cancelBtn ' + cancelButtonClassAttr + '" id="' + cancelButtonIdAttr + '">' + opts.cancelButton.text + '</button>' : '';

        var confirmButtonClassAttr = opts.confirmButton.hook.type === 'class' ? opts.confirmButton.hook.value : '';
        var confirmButtonIdAttr = opts.confirmButton.hook.type === 'id' ? opts.confirmButton.hook.value : '';
        var confirmButtonHtml = opts.confirmButton.show ?
                '<button type="button" class="eui-btn eui-btn-secondary eui-alert-confirmBtn ' + confirmButtonClassAttr + '" id="' + confirmButtonIdAttr + '">' + opts.confirmButton.text + '</button>' : '';

        buttonsHtml = cancelButtonHtml + confirmButtonHtml;
    }


    var alertHtml = '' +
            '<div class="eui-alert ' + alertClassAttr + '" id="' + alertIdAttr + '">' +
                '<div class="eui-alert-mask"></div>' +
                '<div class="eui-alert-cont">' +
                    '<div class="eui-alert-cont-hd">' +
                        '<h3 class="eui-alert-title">' + title + '</h3>' +
                        '<button type="button" class="eui-alert-close"></button>' +
                    '</div>' +
                    '<div class="eui-alert-cont-bd">' +
                        iconHtml +
                        '<div class="eui-alert-cont-text">' + text + '</div>' +
                    '</div>' +
                    '<div class="eui-alert-cont-ft">' + buttonsHtml + '</div>' +
                '</div>' +
            '</div>';

    $('body').append(alertHtml);
    var $eAlert = $('body').find(' > .eui-alert:last-child');
    return $eAlert;
};
ea.bindEvent = function(opts, $eAlert) {
    $eAlert.delegate('.eui-alert-close', 'click', function(event) {
        $eAlert.addClass('eui-alert-removing');
        setTimeout(function() {
            $eAlert.remove();
        }, 300);
        var cbThis = { $eAlert: $eAlert, event: event };
        if ($.isFunction(opts.onClose)) { opts.onClose.call(cbThis); }
    });

    if (opts.buttons.length === 0) {
        $eAlert.delegate('.eui-alert-cancelBtn', 'click', function(event) {
            var $button = $(event.currentTarget);
            var cbThis = { $eAlert: $eAlert, $button: $button, event: event };
            if ($.isFunction(opts.onCancel)) {
                opts.onCancel.call(cbThis);
            }
            else {
                $eAlert.find('.eui-alert-close').trigger('click');
            }
        });

        $eAlert.delegate('.eui-alert-confirmBtn', 'click', function(event) {
            var $button = $(event.currentTarget);
            var cbThis = { $eAlert: $eAlert, $button: $button, event: event };
            if ($.isFunction(opts.onConfirm)) {
                opts.onConfirm.call(cbThis);
            }
            else {
                $eAlert.find('.eui-alert-close').trigger('click');
            }
        });
    }
    else {
        $eAlert.delegate('.eui-alert-cont-ft > button', 'click', function(event) {
            var $button = $(event.currentTarget);
            var index = $eAlert.find('.eui-alert-cont-ft > button').index($button);
            var cbThis = { $eAlert: $eAlert, $button: $button, event: event };
            if ($.isFunction(opts.buttons[index].cb)) {
                opts.buttons[index].cb.call(cbThis);
            }
        });
    }
};
ea.init = function(opts) {
    var $eAlert = ea.render(opts);
    ea.bindEvent(opts, $eAlert);
};

})(jQuery, window);
;(function($, w) {
w.eMsg = function(options) {
    var opts = $.extend(true, {}, defaults, options);
    em.init(opts);
};

var defaults = {
    text: '',
    delay: 0,
    timer: 3000
};

var em = {};
em.render = function(opts) {
    var msgHtml = '' +
            '<div class="eui-msg">' +
                '<div class="eui-msg-mask"></div>' +
                '<div class="eui-msg-cont">' +
                    opts.text +
                '</div>' +
            '</div>';

    setTimeout(function() {
        $('body').append(msgHtml);
        var $eMsg = $('body').find(' > .eui-msg:last-child');
        
        setTimeout(function() {
            $eMsg.addClass('eui-msg-removing');
            
            setTimeout(function() {
                $eMsg.remove();
            }, 300);

        }, opts.timer);

    }, opts.delay);
};
em.init = function(opts) {
    em.render(opts);
};

})(jQuery, window);
/**
	* @弹框插件
	* @author hzh23613
*/
;(function($,window,document){
	var eDialogId;
	var EDialog = function(ele){
		this.element = $(ele);
		this.state = '';
	};

	EDialog.prototype = {
		init:function(){
			this.destroy();
			this.setDialogId();
			this.bindEvent();
		},
		setDialogId:function(){
			eDialogId = 'eui-dialog-';
            eDialogId = this.element.attr('id') != null && (this.element.attr('id')).trim() !== '' ? this.element.attr('id') : eDialogId + EDialog.index;
            this.element.attr('id', eDialogId);
		},
		getDialogId:function(){
			//console.log(eDialogId);
			return eDialogId ;
		},	
		//删除已经初始化的实例
		destroy:function(){
			var hasExist = window.eDialogInstances[this.element.attr('id')] != null;
            if (hasExist) {
                this.element.undelegate('.eui-dialog-btn-close', 'click');
                delete window.eDialogInstances[this.element.attr('id')];
            }
		},
		show:function(){
			if(!this.element.hasClass('show')){
				this.element.addClass('show');
				if($('body').find('.eui-dialog-mask.show').length === 0){
					this.element.find('.eui-dialog-mask').addClass('show');
				}
				this.state = 'show';
				this.element.trigger('change',this.state);
			}
		},
		hide:function(){
			if(this.element.hasClass('show')){
				this.element.removeClass('show');
				if($('body').find('.eui-dialog.show').length === 0){
					this.element.find('.eui-dialog-mask').removeClass('show');
				}	        
				this.state = 'hide';
				this.element.trigger('change',this.state);
            }
		},
		bindEvent:function(){
			var that = this;
			var element = that.element; 
			element.delegate('.eui-dialog-btn-close','click', function(e){
				that.hide();
			});
		}
	};

	EDialog.index = 0;

    /**
     * 如果需要实例化的元素是单个，返回单个对象
     * 如果需要实例化的元素是多个，返回多个对象组成的数组
     */
	$.fn.eDialog = function(state){
		var eDialog,id;
		if(this.length === 1){
			++EDialog.index;
			eDialog = new EDialog(this);
			eDialog.init();
			if (state === 'show') {
				eDialog.show();
			}
			else if (state === 'hide') {
				eDialog.hide();
			}

			id = eDialog.getDialogId();
			window.eDialogInstances[id] = eDialog;

		}else if(this.length >1){
			this.each(function(index,item){
				++EDialog.index;
				eDialog = new EDialog(item);
				eDialog.init();
				if (state === 'show') {
					eDialog.show();
				}
				else if (state === 'hide') {
					eDialog.hide();
				}

				id = eDialog.getDialogId();
				window.eDialogInstances[id] = eDialog;
			});
		}
		//console.log(window.eDialogInstances);
	};



	//全局存放的实例对象，方便快速获取所有的实例
	window.eDialogInstances = {};
})(jQuery,window,document);
/**
 * 搜索  走不走异步
 * icon 用other属性
 * 如果有原select有id的话，就生成一个id
 * 返回一个实例对象，以便扩展
 * 监听select变化 select的value 子节点 option的属性
 * jq动态修改val，节点，自定义属性
 * 暴露出一个update方法
 * trigger change
 * val prop 方法要手动触发一下change事件
 */


;(function($, w) {
    var EDropDown = function(options, element) {
        var defaults = {
            showSearch: false,
            searchPlaceholder: '输入搜索'       
        };

        var $element = $(element);
        var $eDropDown;
        var eDropDownId = 'eui-dropdown-';
        var observer;

        var opts = $.extend({}, defaults, options);

        function init() {
            $eDropDown = $element.next('.eui-dropdown');
            //判断$elemt是不是select，否的话报错
            destory();

            renderHtml();

            bindEvent();
        }

        /**
         * 销毁已存在的实例
         * 由于Jq中remove时便可以解绑元素上的事件
         * 所以不再手动解绑事件，直接remove
         * 或者说不销毁还是用现有的实例
         */
        function destory() {
            var hasEDropDown = $eDropDown.length > 0;
            if (hasEDropDown) {
                //解绑dom监听
                window.eDropDownInst[$eDropDown.attr('id')].observer.disconnect();
                delete window.eDropDownInst[$eDropDown.attr('id')];
                $eDropDown.remove();
                $element.off('change', handleChangeEvent);
            }
        }

        function renderHtml() {
            eDropDownId = ($element.attr('id') || '').trim() !== '' ? 
                    (eDropDownId + $element.attr('id')) : (eDropDownId + EDropDown.index);
            var selectId = ($element.attr('id') || '').trim() !== '' ? 
                    ($element.attr('id')) : ('eui-select-' + EDropDown.index);
            var eDropdownHtml = `
                    <div class="eui-dropdown" id="${eDropDownId}">
                        <button type="button" class="eui-dropdown-btn"></button>
                        <div class="eui-dropdown-panel">
                            <div class="eui-dropdown-search" style="${opts.showSearch ? '' : 'display: none;'}">
                                <input type="text" class="eui-input" placeholder=${opts.searchPlaceholder}></input>
                            </div>
                            <div class="eui-dropdown-list">
                                <ul class="eui-dropdown-list-initial"></ul>
                            </div>
                        </div>
                    </div>`;

            $element.after(eDropdownHtml);
            $eDropDown = $element.next('.eui-dropdown');
            $element.addClass('eui-select-hide').attr('id', selectId);
            updateEDropDownList();
        }

        function bindEvent() {

            $eDropDown.delegate('.eui-dropdown-list li', 'click', function() {
                var value = $(this).attr('data-value');
                setValue(value);
                updateOptionList($(this));

                $eDropDown.find('.eui-dropdown-btn').trigger('click');              
            });

            $eDropDown.delegate('.eui-dropdown-btn', 'click', function() {
                $('.eui-dropdown').not($eDropDown[0]).find('.eui-dropdown-btn').removeClass('eui-dropdown-btn-active');
                $(this).toggleClass('eui-dropdown-btn-active');
            });

            $eDropDown.delegate('.eui-dropdown-search > input', 'input', function(event) {
                var $input = $(event.currentTarget);
                var inputVal = $input.val().trim();

                setTimeout(function() {
                    var inputValAfter500s = $input.val().trim();

                    //此时认为用户没有在输入
                    if (inputValAfter500s === inputVal) {

                        search(inputVal);
                    }
                }, 500);
            });

            //监听select元素的子节点的变化, 来动态更新dropdown的内容
            //todo:对select的监听要手动进行解绑
            observer= new MutationObserver(function(mutations) {
                //console.log(mutations);
                updateEDropDownList();
            });
            observer.observe($element[0], {
                childList: true,
                attributes: true,
                subtree: true
            });

            //jq的val方法并不会触发上面的监听，所以单独绑定change事件来触发
            $element.on('change', handleChangeEvent);
        }
        
        function handleChangeEvent() {
            //console.log('change');
            setValue($element.val());            
        }

        function updateEDropDownList() {
            var dropdownInitialListHtml = '';
            $element.find('> option').each(function(index, item) {
                var otherDataAttrList = Object.keys(item.dataset).map(function(key) {
                    return 'data-' + key + '=' + '"' + item.dataset[key] + '"';
                });

                dropdownInitialListHtml += `
                        <li data-value="${$(item).val()}" title="${$(item).text()}"  ${otherDataAttrList.join(' ')}>
                            ${$(item).text()}
                        </li>`;

            });
            $eDropDown.find('.eui-dropdown-list-initial').html(dropdownInitialListHtml);
            setValue($element.val());           
        }

        function updateOptionList($listItem) {
            var value = $listItem.attr('data-value');
            if ($element.find('option[value="' + value + '"]').length > 0) {
                $element.val(value);
            }
            else {
                var otherDataAttrList = Object.keys($listItem[0].dataset).map(function(key) {
                    return 'data-' + key + '=' + '"' + $listItem[0].dataset[key] + '"';
                });
                var optionHtml = '' +
                        '<option value="' + value + '" ' + otherDataAttrList.join(' ') + '>' +
                            $listItem.text() +
                        '</option>';
                $element.append(optionHtml);
                $element.val(value);
            }
            $element.trigger('change');
        }

        function setValue(value) {
            var optionEle = $element.find('option[value="' + value + '"]');
            $eDropDown.find('.eui-dropdown-btn').text(optionEle.text()).attr('data-value', value);
            $eDropDown.find('.eui-dropdown-list-item-active').removeClass('eui-dropdown-list-item-active');
            $eDropDown.find('.eui-dropdown-list li[data-value="' + value + '"]').addClass('eui-dropdown-list-item-active');
        }

        function search(inputVal) {
            var $list = $eDropDown.find('.eui-dropdown-list > ul');
            var $searchedListItem = null;
            if (inputVal === '') {
                $list.find('> li').removeClass('eui-dropdown-item-hidden');
            }
            else {
                $searchedListItem = $list.find(`> li[title*=${inputVal}]`);
                $list.find('> li').addClass('eui-dropdown-item-hidden');                  
                $searchedListItem.removeClass('eui-dropdown-item-hidden');
            }
        };


        init();


        //对外的属性和方法
        return {
            id: eDropDownId,
            observer: observer
        };

    };

    //index用于唯一标志每个EDropDown实例
    EDropDown.index = 0;

    /**
     * 构造函数
     * 如果需要实例化的元素是单个，返回单个对象
     * 如果需要实例化的元素是多个，返回多个对象组成的数组
     */
    $.fn.eDropDown = function(options) {
        var result;
        if (this.length === 1) {
            ++EDropDown.index;
            result = new EDropDown(options, this);
            window.eDropDownInst[result.id] = result;
        }
        else if (this.length > 1) {
            result = [];
            this.each(function() {
                var item = this;
                ++EDropDown.index;
                var edropDownInstance = new EDropDown(options, item);
                result.push(edropDownInstance);
                window.eDropDownInst[edropDownInstance.id] = edropDownInstance;
            });
        }

        return result;
    };
    
    //全局存放的实例对象，方便快速获取所有的实例
    window.eDropDownInst = {};

    $(function() {
        $('body').on('click', function(event) {
            var clickEle = $(event.target);
            var canRemoveClass = clickEle.parents('.eui-dropdown').length === 0;
            if (canRemoveClass) {
                $('.eui-dropdown-btn').removeClass('eui-dropdown-btn-active');
            }       
        });
    });

})(jQuery, window);
;(function($, w) {

var defaults = {
    type: 'checkbox',
    placeholder: '请选择',
    showButton: true,
    allCheckbox: { //checkbox type时有用  当返回的数据里面包含全部时，可以把这个设置为false，但是value要设置
        show: true,
        value: -1,
        text: '全部'
    },
    source: '', //string or array
    sourceRequestType: 'GET',
    defaults: '', // string or array
    disables: '', // string or array
    resultItem: {
        value: '',
        text: '',
        other: ''
    },
    showSearch: false,
    searchPlaceholder: '输入搜索',
    onSearch: null,
    onCheck: null, //每次勾选一个checkbox时的回调
    onConfirm: null, //选中的到了input上面时的回调
    onCancel: null,
    onClear: null,
    onRender: null,
    onBeforeSend: null,
    onGetData: null
};

//eMultiSelect type checkbox
var emc = {};
emc.destroy = function($element) {
    var $eMultiSelect = $element.find('.eui-multiSelect');
    var id = $eMultiSelect.attr('id');
    $element.find('.eui-multiSelect').remove();
    delete w.eMultiSelectInst[id];
};

emc.render = function(opts, $element) {
    var eMultiSelectHtml = `
        <div class="eui-multiSelect" data-type="${opts.type}" data-index=${EMultiSelect.index} 
                id="eui-multiSelect-${$element.attr('id') || EMultiSelect.index}">
            <div class="eui-multiSelect-hd">
                <div class="eui-multiSelect-result eui-input eui-multiSelect-result-empty" placeholder="${opts.placeholder}">
                    ${opts.type === 'button' ? '<ul></ul>' : ''}
                </div>
                <button type="button" class="eui-btn"></button>
            </div>
            <div class="eui-multiSelect-bd">
                <div class="eui-multiSelect-search" style="${opts.showSearch ? '' : 'display: none;'}">
                    <input type="text" class="eui-input" placeholder=${opts.searchPlaceholder}>
                </div>
                <div class="eui-multiSelect-panel">
                    <ul></ul>
                </div>
                <div class="eui-multiSelect-handle" style="${opts.showButton ? '' : 'display: none;'}">
                    <button type="button" class="eui-btn eui-btn-secondary eui-multiSelect-confirm">确定</button>
                    <button type="button" class="eui-btn eui-btn-cancel eui-multiSelect-cancel">取消</button>
                    <button type="button" class="eui-btn eui-btn-cancel eui-multiSelect-clear">清空</button>
                </div>
            </div>
        </div>`;

    $element.html(eMultiSelectHtml);
    var $eMultiSelect = $element.find(' > .eui-multiSelect');

    if ($.isArray(opts.source)) {
        emc.renderList(opts, $eMultiSelect, opts.source);
    }
    else {
        emc.getList(opts, $eMultiSelect);
    }

    return $eMultiSelect;
};

emc.getList = function(opts, $eMultiSelect) {
    var ajaxSendData = {};
    //发送异步前的回调，必须要return ajaxSendData
    if ($.isFunction(opts.onBeforeSend)) {
        var cbThis = { $eMultiSelect: $eMultiSelect };
        ajaxSendData = opts.onBeforeSend.call(cbThis, ajaxSendData);
    }

    var xhr = $.ajax({
        url: opts.source,
        type: opts.sourceRequestType,
        data: ajaxSendData,
        dataType: 'json',
        timeout: 20000,
        beforeSend: function() {
        },
        success: function(data) {
            data = data || [];

            //获取数据后的回调，方便对数据进行格式处理
            //该回调必须显示return 一个数组类型的值
            if ($.isFunction(opts.onGetData)) {
                var cbThis = { $eMultiSelect: $eMultiSelect, xhr: xhr };
                data = opts.onGetData.call(cbThis, data);
            }

            //渲染列表
            emc.renderList(opts, $eMultiSelect, data);
        },
        error: function(a) {
        }
    });
};

emc.handleData = function(opts, $eMultiSelect, data) {
    var allIndex = data.findIndex(function(item) {
        return ('' + item[opts.resultItem.value]) === ('' + opts.allCheckbox.value);
    });
    var hasAll = allIndex > -1;

    if (opts.allCheckbox.show) {
        if (hasAll) {
            data[allIndex][opts.resultItem.value] = opts.allCheckbox.value;
            data[allIndex][opts.resultItem.text] = opts.allCheckbox.text;
        }
        else {
            var allItem = {};
            allItem[opts.resultItem.value] = opts.allCheckbox.value;
            allItem[opts.resultItem.text] = opts.allCheckbox.text;
            data.unshift(allItem);
        }
    }
    else {
        if (hasAll) {
            data.splice(allIndex, 1);
        }
    }

    return data || [];
};

emc.renderList = function(opts, $eMultiSelect, data) {
    var listHtml = ``;
    var eMultiSelectIndex = $eMultiSelect.attr('data-index');
    var allCheckboxId = `#eui-multiSelect-${eMultiSelectIndex}-checkbox-${opts.allCheckbox.value}`;
    var $list = $eMultiSelect.find('.eui-multiSelect-panel > ul');
    var otherAttrList = (opts.resultItem.other || '').split(',').filter(function(attr) {
        return (attr || '').trim() !== '';
    });
    var defaultList = $.isArray(opts.defaults) ?
        opts.defaults :
        (opts.defaults || '').split(',').map(function(defaultValue) {
            var defaultItem = {};
            defaultItem[opts.resultItem.value] = defaultValue;
            return defaultItem;
        });
    var disableList = $.isArray(opts.disables) ?
        opts.disables :
        (opts.disables || '').split(',').map(function(disableValue) {
            var disableItem = {};
            disableItem[opts.resultItem.value] = disableValue;
            return disableItem;
        });
    var defaultValue = [];
    var defaultText = [];

    emc.handleData(opts, $eMultiSelect, data).forEach(function(item) {
        var otherAttr = otherAttrList.map(function(attr) {
            return `data-${attr}=${item[attr]}`;
        }).join(' ');

        var checked = defaultList.find(function(defaultItem) {
            return ('' + defaultItem[opts.resultItem.value]) ===  ('' + item[opts.resultItem.value]);
        }) != null;

        var disabled = disableList.find(function(disableItem) {
            return ('' + disableItem[opts.resultItem.value]) ===  ('' + item[opts.resultItem.value]);
        }) != null;

        if (checked) {
            defaultValue.push(item[opts.resultItem.value]);
            defaultText.push(item[opts.resultItem.text]);
        }

        listHtml += `
            <li>
                <input type="checkbox" name="eui-multiSelect-${eMultiSelectIndex}-checkbox" 
                    class="${opts.type === 'checkbox' ? 'eui-checkbox' : 'eui-checkbox-variant'}"
                    id="eui-multiSelect-${eMultiSelectIndex}-checkbox-${item[opts.resultItem.value]}" 
                    value="${item[opts.resultItem.value]}" title="${item[opts.resultItem.text]}"
                    ${otherAttr} ${checked ? 'checked' : ''} ${disabled ? 'disabled' : ''}>
                <label for="eui-multiSelect-${eMultiSelectIndex}-checkbox-${item[opts.resultItem.value]}" 
                    title=${item[opts.resultItem.text]}>${item[opts.resultItem.text]}</label>
            </li>
        `;
    });

    defaultValue = defaultValue.join().trim();
    defaultText = defaultText.join().trim();

    $list.html(listHtml);

    //判断是否要选中全部
    if (('' + defaultValue) === ('' + opts.allCheckbox.value)) {
        $eMultiSelect.find('.eui-multiSelect-panel input[type="checkbox"]:not(:disabled)').prop('checked', true);
    }
    else {
        var checkedLength = $list.find(`input[type="checkbox"]:not(${allCheckboxId}):checked`).length;
        var checkboxLength = $list.find(`input[type="checkbox"]:not(${allCheckboxId})`).length;
        if (checkboxLength === checkedLength) {
            $eMultiSelect.find('.eui-multiSelect-panel input[type="checkbox"]:not(:disabled)').prop('checked', true);
        }
    }

    //设置选择内容
    var $checkedbox = emc.getCheckedbox(opts, $eMultiSelect);
    emc.setCheckedValue(opts, $eMultiSelect, $checkedbox, true);

    //渲染完成的回调
    if ($.isFunction(opts.onRender)) {
        var cbThis = { $eMultiSelect: $eMultiSelect };
        opts.onRender.call(cbThis);
    }
};

emc.changeCheckboxState = function(opts, $eMultiSelect, $checkbox, checked, forbidCb) {
    $checkbox.prop('checked', checked);

    var $list = $eMultiSelect.find('.eui-multiSelect-panel > ul');
    var eMultiSelectIndex = $eMultiSelect.attr('data-index');
    var allCheckboxId = `#eui-multiSelect-${eMultiSelectIndex}-checkbox-${opts.allCheckbox.value}`;
    var $allCheckbox = $list.find(allCheckboxId);
    var exsistAllCheckbox = $allCheckbox.length > 0;
    var checkboxIsAllCheckbox = `#${$checkbox.attr('id')}` === allCheckboxId;
    if (exsistAllCheckbox) {
        if (checkboxIsAllCheckbox) {
            $list.find('input[type="checkbox"]:not(:disabled)').prop('checked', checked);
        }
        else {
            var checkedLength = $list.find(`input[type="checkbox"]:not(${allCheckboxId}):checked`).length;
            var checkboxLength = $list.find(`input[type="checkbox"]:not(${allCheckboxId})`).length;
            if (checkedLength === checkboxLength) {
                $allCheckbox.prop('checked', true);
            }
            else {
                $allCheckbox.prop('checked', false);
            }
        }
    }

    var cbThis = { $eMultiSelect: $eMultiSelect, $checkbox: $checkbox };
    var canCallCb = $.isFunction(opts.onCheck) && (typeof forbidCb === 'undefined' || forbidCb === false);
    if (canCallCb) { opts.onCheck.call(cbThis); }
};

emc.getCheckedbox = function(opts, $eMultiSelect) {
    var $list = $eMultiSelect.find('.eui-multiSelect-panel > ul');
    var eMultiSelectIndex = $eMultiSelect.attr('data-index');
    var allCheckboxId = `#eui-multiSelect-${eMultiSelectIndex}-checkbox-${opts.allCheckbox.value}`;
    var $allCheckbox = $list.find(allCheckboxId);
    var exsistAllCheckbox = $allCheckbox.length > 0;
    var $checkedbox = $list.find(`input[type="checkbox"]:not(${allCheckboxId}):checked`);

    if (exsistAllCheckbox) {
        var checkedLength = $list.find(`input[type="checkbox"]:not(${allCheckboxId}):checked`).length;
        var checkboxLength = $list.find(`input[type="checkbox"]:not(${allCheckboxId})`).length;
        if (checkedLength === checkboxLength) {
            $checkedbox = $allCheckbox;
        }
    }

    return $checkedbox;
};

emc.setCheckedValue = function(opts, $eMultiSelect, $checkedbox, forbidCb) {
    var $result = $eMultiSelect.find('.eui-multiSelect-result');
    var checkedboxList = $.makeArray($checkedbox);
    var value = checkedboxList.map(function(checkbox) {
        return $(checkbox).val();
    }).join();
    var text = checkedboxList.map(function(checkbox) {
        var checkboxId = $(checkbox).attr('id');
        return $(checkbox).siblings(`label[for=${checkboxId}]`).text();
    }).join();

    //显隐placeholder
    if (value === '') {
        $result.addClass('eui-multiSelect-result-empty');
    }
    else {
        $result.removeClass('eui-multiSelect-result-empty');
    }

    //分不同的类别给result添加展示的数据
    if (opts.type === 'checkbox') {
        $result.text(text).attr('title', text);        
    }
    else if(opts.type === 'button') {
        var resultListHtml = ``;
        (value || '').split(',').filter(function(valueItem) {
            return ('' + valueItem).trim() !== '';
        }).forEach(function(valueItem, valueIndex) {
            resultListHtml += `<li value="${valueItem}">${(text || '').split(',')[valueIndex]}</li>`;
        });
        $result.find('ul').html(resultListHtml);        
    }

    $result.attr('value', value).attr('text', text);
   

    var cbThis = { $eMultiSelect: $eMultiSelect, $checkedbox: $checkedbox, value: value, text: text };
    var canCallCb = $.isFunction(opts.onConfirm) && (typeof forbidCb === 'undefined' || forbidCb === false);
    if (canCallCb) { opts.onConfirm.call(cbThis); }
};

emc.cancelCheckedbox = function(opts, $eMultiSelect) {
    var $list = $eMultiSelect.find('.eui-multiSelect-panel > ul');
    var $selectResult = $eMultiSelect.find('.eui-multiSelect-result');
    var value = ($selectResult.attr('value') || '').trim();

    $list.find('input[type="checkbox"]').prop('checked', false);

    if (('' + value) === ('' + opts.allCheckbox.value)) {
        $list.find('input[type="checkbox"]').prop('checked', true);
    }
    else if (value !== '') {
        value.split(',').forEach(function(valueItem) {
            $list.find(`input[type="checkbox"][value=${valueItem}]`).prop('checked', true);
        });
    }

    var cbThis = { $eMultiSelect: $eMultiSelect };
    if ($.isFunction(opts.onCancel)) { opts.onCancel.call(cbThis); }

    $eMultiSelect.trigger('close');
};

emc.clearCheckedbox = function(opts, $eMultiSelect) {
    var $list = $eMultiSelect.find('.eui-multiSelect-panel > ul');
    $list.find('input[type="checkbox"]:not(:disabled)').prop('checked', false);    

    var $checkedbox = emc.getCheckedbox(opts, $eMultiSelect);
    emc.setCheckedValue(opts, $eMultiSelect, $checkedbox, true);

    var cbThis = { 
        $eMultiSelect: $eMultiSelect,
        $checkedbox: emc.getCheckedbox(opts, $eMultiSelect),
        value: $eMultiSelect.find('.eui-multiSelect-result').attr('value'),
        text: $eMultiSelect.find('.eui-multiSelect-result').attr('text')
    };
    if ($.isFunction(opts.onClear)) { opts.onClear.call(cbThis); }

    $eMultiSelect.trigger('close');
};

emc.search = function(opts, $eMultiSelect, inputVal) {
    var $list = $eMultiSelect.find('.eui-multiSelect-panel > ul');
    var $searchedCheckbox = null;
    if (inputVal === '') {
        $list.find('> li').removeClass('eui-multiSelect-item-hidden');
    }
    else {
        $searchedCheckbox = $list.find(`input[type="checkbox"][title*=${inputVal}]`);
        if ($searchedCheckbox.length > 0) {
            $list.find('> li').addClass('eui-multiSelect-item-hidden');
            $searchedCheckbox.parent().removeClass('eui-multiSelect-item-hidden');
        }
        else {
            $list.find('> li').addClass('eui-multiSelect-item-hidden');
        }
    }

    var cbThis = { $eMultiSelect: $eMultiSelect, $searchedCheckbox: $searchedCheckbox };
    if ($.isFunction(opts.onSearch)) { opts.onSearch.call(cbThis); }
};

emc.bindEvent = function(opts, $eMultiSelect) {
    $eMultiSelect.delegate('.eui-multiSelect-panel input[type="checkbox"]', 'change', function(event) {
        var $checkbox = $(event.currentTarget);
        var checked = $checkbox.prop('checked');

        emc.changeCheckboxState(opts, $eMultiSelect, $checkbox, checked);

        //当没有“确认”按钮时，每次checkbox状态改变后，都会设置value值
        if (!opts.showButton) {
            var $checkedbox = emc.getCheckedbox(opts, $eMultiSelect);
            emc.setCheckedValue(opts, $eMultiSelect, $checkedbox);
        }
    });

    $eMultiSelect.delegate('.eui-multiSelect-search .eui-input', 'input', function(event) {
        var $input = $(event.currentTarget);
        var inputVal = $input.val().trim();

        setTimeout(function() {
            var inputValAfter500s = $input.val().trim();

            //此时认为用户没有在输入
            if (inputValAfter500s === inputVal) {

                emc.search(opts, $eMultiSelect, inputVal);
            }
        }, 500);
    });

    $eMultiSelect.delegate('.eui-multiSelect-handle .eui-multiSelect-confirm', 'click', function(event) {
        $eMultiSelect.trigger('confirm');
    });

    $eMultiSelect.delegate('.eui-multiSelect-handle .eui-multiSelect-cancel', 'click', function(event) {
        $eMultiSelect.trigger('cancel');      
    });

    $eMultiSelect.delegate('.eui-multiSelect-handle .eui-multiSelect-clear', 'click', function(event) {
        $eMultiSelect.trigger('clear');
    });

    $eMultiSelect.delegate('.eui-multiSelect-result', 'click', function(event) {
        var $clickEle = $(event.currentTarget);

        //当点击已选项的li时，不在该事件回调里面做处理
        if ($clickEle.parent().is('li')) {
            return false;
        }
        else {
            $eMultiSelect.toggleClass('eui-multiSelect-open');
        }
    });

    $eMultiSelect.delegate('.eui-multiSelect-result li', 'click', function(event) {
        var $clickEle = $(event.currentTarget);
        var value = $(this).attr('value');
        var $panel = $eMultiSelect.find('.eui-multiSelect-panel');
        var $checkbox = $panel.find(`input[type="checkbox"][value=${value}]`);
        
        $checkbox.prop('checked', false);

        $clickEle.remove();
        emc.changeCheckboxState(opts, $eMultiSelect, $checkbox, false, true);

        var $checkedbox = emc.getCheckedbox(opts, $eMultiSelect);
        emc.setCheckedValue(opts, $eMultiSelect, $checkedbox);  
    });

    $eMultiSelect.delegate('.eui-multiSelect-hd > .eui-btn', 'click', function(event) {
        $eMultiSelect.toggleClass('eui-multiSelect-open');
    });

    $eMultiSelect.on('confirm', function(event) {
        var $checkedbox = emc.getCheckedbox(opts, $eMultiSelect);
        emc.setCheckedValue(opts, $eMultiSelect, $checkedbox);

        $eMultiSelect.trigger('close');
    });

    $eMultiSelect.on('cancel', function(event) {
        emc.cancelCheckedbox(opts, $eMultiSelect);
    });

    $eMultiSelect.on('clear', function(event) {
        emc.clearCheckedbox(opts, $eMultiSelect);
    });

    $eMultiSelect.on('open', function(event) {
        $eMultiSelect.addClass('eui-multiSelect-open');
    });

    $eMultiSelect.on('close', function(event) {
        $eMultiSelect.removeClass('eui-multiSelect-open');
    });
};

emc.init = function(opts, $element) {
    var exsist = $element.find('.eui-multiSelect').length > 0;
    if (exsist) {
        emc.destroy($element);
    }

    var $eMultiSelect = emc.render(opts, $element);
    emc.bindEvent(opts, $eMultiSelect);
};


var EMultiSelect = function(options, $element) {
    ++EMultiSelect.index;
    var opts = $.extend(true, {}, defaults, options);

    if (opts.type === 'checkbox' || opts.type === 'button') {
        emc.init(opts, $element);
    }
    else {
        throw new Error('eMultiSelect type is not right');
    }

    return {
        id: `eui-multiSelect-${$element.attr('id') || EMultiSelect.index}`,
        selector: $element.selector,
        opts: opts,
        render: emc.renderList
    };
};

EMultiSelect.index = 0;

w.eMultiSelectInst = {};

/**
 * 构造函数
 * 如果需要实例化的元素是单个，返回单个对象
 * 如果需要实例化的元素是多个，返回多个对象组成的数组
 */
$.fn.eMultiSelect = function(options) {
    var result;
    if (this.length === 1) {
        result = new EMultiSelect(options, this);
        w.eMultiSelectInst[result.id] = result;
    }
    else if (this.length > 1) {
        result = [];
        this.each(function() {
            var item = this;
            var eMultiSelectInstance = new EMultiSelect(options, item);
            w.eMultiSelectInst[eMultiSelectInstance.id] = eMultiSelectInstance;
            result.push(eMultiSelectInstance);
        });
    }

    return result;
};

$.fn.eMultiSelect.getValue = function(selector) {
    var valueList = [];
    var eMultiSelectList = $.makeArray($(selector).find('.eui-multiSelect'));
    eMultiSelectList.forEach(function(eMultiSelect) {
        valueList.push({
            value: $(eMultiSelect).find('.eui-multiSelect-result').attr('value'),
            id: $(eMultiSelect).attr('id')
        });
    });

    if (valueList.length === 0) {
        throw new Error('not find eMultiSelectInstance');
    }
    else if (valueList.length === 1) {
        return valueList[0].value;
    }
    return valueList;
};

$.fn.eMultiSelect.getText = function(selector) {
    var textList = [];
    var eMultiSelectList = $.makeArray($(selector).find('.eui-multiSelect'));
    eMultiSelectList.forEach(function(eMultiSelect) {
        textList.push({
            text: $(eMultiSelect).find('.eui-multiSelect-result').attr('text'),
            id: $(eMultiSelect).attr('id')
        });
    });

    if (textList.length === 0) {
        throw new Error('not find eMultiSelectInstance');
    }
    else if (textList.length === 1) {
        return textList[0].text;
    }
    return textList;
};

$.fn.eMultiSelect.render = function(selector, data, defaults) {
    var instList = w.eMultiSelectInst;
    var instListFromSelector = [];
    for(var key in instList) {
        if (instList.hasOwnProperty(key) && instList[key].selector === selector) {
            instListFromSelector.push(instList[key]);
        }
    }

    instListFromSelector.forEach(function(inst) {
        inst.opts.defaults = typeof defaults === 'undefined' ? inst.opts.defaults : defaults; 
        inst.render(inst.opts, $(`#${inst.id}`), data || []);
    });
};


$(function() {
    $('body').on('click', function(event) {
        var $clickEle = $(event.target);
        var isClickEleInEMultiSelect = $clickEle.hasClass('eui-multiSelect') || $clickEle.parents('.eui-multiSelect').length > 0;
        if (!isClickEleInEMultiSelect) {
            $('.eui-multiSelect').trigger('close');
        }
    });
});

})(jQuery, window);
/**
 * @模糊搜索插件
 * 目前包括：仅显示搜索框的和支持搜索及搜索条件的两种
 * @author hzh23613
 */

;(function($,window,docment){
    var eSearchListClass = '.eSearchList';
    var ESearch = function(ele,opt){
        this.element = $(ele);
        var defaults = {
            'placeholder':'请输入搜索关键词',//搜索框placeholder
            'onlySearch':false,//是否仅支持搜索
            'isAjaxSearch':false,//是否是异步搜索框
            'searchData':[],//固定的搜索数据
            'searchAjaxURL':'',//异步查找数据地址
            'searchKey':'',//搜索的参数  包括非异步与异步两种
            'ajaxType':'GET',//异步发送的方式 GET or POST,默认POST
            'ajaxSearchData':{},//其余的异步数据 不包括searchKey 会在异步发送时自动拼接到data中
            'searchItem':{              
                id:'id',
                text:'text',
                other:''
            },//返回数据的字段格式  other代表其他字段
            'onBeforeSend': null, //暴露异步发送前的时机，方便一些对传参的处理
            'onSearch':null,//仅支持搜索功能时的回调函数  唯一参数为匹配返回数据
            'onSelected':null,//点击列表的回调
            'onGetData': null//异步获取数据后的回调，方便处理不同格式的返回数据
        };
        this.options = $.extend({},defaults,opt);//将一个空对象做为第一个参数
    };

    ESearch.prototype={
        init:function(){
            var options = this.options;
            this.destroyDOM();
            this.renderDOM();
            this.bindEvent();
        },
        destroyDOM:function() {
            var $eSearchList = this.element.find(eSearchListClass);
            if($eSearchList.length > 0){
                $eSearchList.remove();
            }
        },
        renderDOM:function(){
            var placeholder = this.options.placeholder;
            var ulHtml = '';
            if(!this.options.onlySearch){
                ulHtml = '<ul class="eSearchList"></ul>';
            }
            var mainHTML = ''+
                '<div class="eSearchWrapper">'+
                    '<div class="eui-form-item">'+
                        '<div class="eui-form-item-control">'+
                            '<input type="text" class="eui-input" placeholder="'+ placeholder+'">'+
                            '<i class="input-icon search-icon"></i>'+
                        '</div>'+
                    '</div>'+
                    ulHtml
                +'</div>';
            this.element.html(mainHTML);
        },
        bindEvent:function(){
            var that = this;
            var options = that.options;
            var $inputObj = that.element.find('.eui-input');

            $inputObj.on('focus', { 'options': options }, focusSearchInput);
            $inputObj.on('keyup',{'options':options}, keyupSearchInput);
            $inputObj.on('input', { 'options': options }, inputSearchInput);

            if(!options.onlySearch){
                that.element.on('click','.eSearchList>li',{ 'options': options },function(event){
                    clickSearchLi(event);
                });
            }

            $(document).click(function (e) {
                var obj = $(e.target);
                if (obj.parents('.eSearchWrapper').length == 0) {
                    var eSearchList = that.element.find('ul');
                    if(eSearchList&& eSearchList.is(':visible')){
                        eSearchList.slideUp(500);
                    }
                }
            });

            $('.eSearchWrapper').on('click','.input-icon',function(e){
                var $target = $(e.target);
                $('.eSearchWrapper').find('input').val('');
                if($target.hasClass('remove-icon')){
                    $target.removeClass('remove-icon').addClass('search-icon');
                }
            });
        }
    };

    //数据匹配
    function getSearchResult($obj,options){
        var key = options.searchKey;
        var inputVal = $obj.val().trim();
        var result = [];
        if(!options.isAjaxSearch){//存在默认数据  无需发送异步
            var data = options.searchData;
            var len = data.length;
            if($.isArray(data) && len !== 0){//固定的数据是非空数组类型
                for(var i = 0; i < len; i++){
                    if(data[i][key].indexOf(inputVal) > -1){
                        result.push(data[i]);
                    }
                }
            }
            handleSearchResult($obj,options,result);
        }else{
            var dataStr = '';
            var obj = options.ajaxSearchData;
            if(!options.ajaxSearchData){//不存在ajax数据
                dataStr = options.searchKey + '=' + encodeURIComponent(inputVal);
            }else{
                obj[options.searchKey] = encodeURIComponent(inputVal);
                dataStr = $.param(obj);
            }

            //暴露出接口，方便在发送异步前对发送的数据做处理
            if (options.onBeforeSend != null) {
                obj = options.onBeforeSend(obj);
                dataStr = $.param(obj);
            }            

            $.ajax({
                url: options.searchAjaxURL,
                type:options.ajaxType,
                data: dataStr,
                success: function (data) {
                    result = data;
                    if (typeof data != '$object') {
                        result = eval(data);
                        if (options.onGetData != null) {
                            result = options.onGetData(result);
                        }
                    }
                    handleSearchResult($obj,options,result);
                },
                error: function (err) {
                }
            })
        }
    }

    //处理数据
    function handleSearchResult(obj,options,result){
        if(options.onlySearch){//仅支持搜索功能
            options.onSearch && options.onSearch(result);
        }else{
            initQueryList(obj, result, options);
        }
    }

    //拼接搜索结果列表
    function initQueryList(queryObj, dataList, options) {
        var nameKey = options.searchItem.text;
        var idKey = options.searchItem.id;
        var listWrapper = $(eSearchListClass).empty();
        if (!dataList) {
            listWrapper.append('<li>搜索不到该数据……</li>');
            listWrapper.find('li:last').addClass('disable');
        } else {
            var listLength = dataList.length;

            if (listLength > 0) {
                for (var index = 0; index < listLength; index++) {
                    var otherDataAttr = '';

                    if($.isArray(options.searchItem.other)){
                        otherDataAttr = options.searchItem.other.split(',').map(function(otherKey_item) {
                            return 'data-' + otherKey_item + '="' + dataList[index][otherKey_item] + '"'; 
                        }).join(' ');
                    }

                    listWrapper.append('<li ' + otherDataAttr+' data-id='+ dataList[index][idKey] +'>' + dataList[index][nameKey] + '</li>');
                }
            } else {
                listWrapper.append('<li>搜索不到该数据……</li>');
                listWrapper.find('li:last').addClass('disable');
            }

        }
        if (listWrapper.is(':hidden')) {
            listWrapper.slideDown('500').scrollTop(0);
        }
    }

    function inputSearchInput(event){
        var options = event.data.options;
        var $target = $(event.target);
        var inputVal = $target.val();
        setTimeout(function() {
            var inputValNow = $target.val().trim();
            if (inputVal !== inputValNow) {
                return;
            }
            if($.trim($target.val()) !== '') {
                getSearchResult($target,options);
            }else{
                if(options.onlySearch){
                    options.onSearch && options.onSearch([]);
                }else{
                    if($target.siblings('ul').is(':visible')){
                        $target.siblings('ul').slideUp(500);
                    }
                }
            }
        },500);
    }

    function focusSearchInput(event){
        var options = event.data.options;
        var $target = $(event.target);
        var inputVal = $target.val();
        if($.trim($target.val()) !== '') {
            getSearchResult($target,options);
        }else{
            if(options.onlySearch){
                options.onSearch && options.onSearch([]);
            }else{
                if($target.siblings('ul').is(':visible')){
                    $target.siblings('ul').slideUp(500);
                }
            }
        }
    }
    function keyupSearchInput(event){
        var $target = $(event.target);
        var inputVal = $target.val();
        if($.trim($target.val()) !== ''){
            $target.siblings('.input-icon').removeClass('search-icon').addClass('remove-icon');
        }else{
            $target.siblings('.input-icon').removeClass('remove-icon').addClass('search-icon');
        }
    }

    function clickedSearchLi() {
        $('.eSearchWrapper .input-icon').removeClass('search-icon').addClass('remove-icon');
    }   


    //点击搜索列表方法
    function clickSearchLi(event){
        if(event.target.tagName.toLowerCase() == 'li' || $(event.target).parents('li').length == 1){
            var obj = $(event.target);
            var $eSearchList = obj.parent();
            var options = event.data.options;
            if(!obj.hasClass('disable')){
                var text = obj.text();
                var dataId = obj.attr('data-id');
                var inputObj = $('.eSearchWrapper input[type="text"]');
                inputObj.val(text);
                inputObj.attr('data-id',dataId).blur();
                clickedSearchLi();

                $eSearchList.slideUp('500', function () {
                    $eSearchList.empty();
                });

                if (event.data.options.onSelected) {
                    event.data.options.onSelected.call(obj);
                }
            }
        } 
    }

    //在插件中使用eSearch对象
    $.fn.eSearch = function(options) {    
        var eSearch = new ESearch(this,options);
        eSearch.init();
        return eSearch;
    };
})( jQuery ,window,document ); 

;(function($, w) {

var defaults = {
    placeholder: '',
    searchSource: '',//支持字符串和数组类型
    searchKey: '',
    searchType: 'GET',
    resultItem: {
        value: '',
        text: '',
        other: ''
    },
    multiple: false,
    showClear: false,
    showResult: true,
    onGetResult: null,
    onFocus: null,
    onBlur: null,
    onBeforeSend: null,
    onGetData: null,
    onRender: null,
    onSelected: null,
    onDelete: null,
    onClear: null
};

var es = {};
es.destroy = function($element) {
    var $eSearch2 = $element.find('.eui-search2');
    var id = $eSearch2.attr('id');
    $element.find('.eui-search2').remove();
    delete w.eSearch2Inst[id];
};
es.render = function(opts, $element) {
    var eSearch2Html = `
        <div class="eui-search2" id="eui-search2-${$element.attr('id') || ESearch2.index}">
            <div class="eui-search2-hd">
                <div class="eui-search2-input">
                    <input type="text" class="eui-input" placeholder="${opts.placeholder}">
                    ${opts.showClear ? '<button type="button" class="eui-btn" style="display: none;"></button>' : ''}
                </div>
            </div>
            <div class="eui-search2-bd">
                <div class="eui-search2-list">
                    <ul></ul>
                </div>
            </div>
        </div>`;

    $element.html(eSearch2Html);
    var $eSearch2 = $element.find(' > .eui-search2');
    return $eSearch2;
};
es.renderSearchList = function(opts, $eSearch2, data) {
    var searchListHtml = '';
    var otherAttrList = (opts.resultItem.other || '').split(',').filter(function(attr) {
        return (attr || '').trim() !== '';
    });

    (data || []).forEach(function(item) {
        var otherAttr = otherAttrList.map(function(attr) {
            return `data-${attr}=${item[attr]}`;
        }).join(' ');
        searchListHtml += `
            <li ${otherAttr} data-id="${item[opts.resultItem.value]}">${item[opts.resultItem.text]}</li>
        `;
    });
    $eSearch2.find('.eui-search2-list > ul').html(searchListHtml);

    if ($.isFunction(opts.onRender)) {
        var cbThis = { $eSearch2: $eSearch2 };
        opts.onRender.call(cbThis);
    }
};
es.showMsgInList = function(msg, $eSearch2) {
    var msgHtml = `<li class="eui-search2-list-msg">${msg}</li>`;
    $eSearch2.find('.eui-search2-list > ul').html(msgHtml);
};
es.handleUserInputEvent = function(opts, $eSearch2, $input) {
    var inputVal = $input.val().trim();

    //用户输入的时候，要把value值清空掉
    $input.attr('data-value', '').attr('title', '');

    if (inputVal === '') {
        $input.siblings('.eui-btn').hide();
    }
    else {
        $input.siblings('.eui-btn').show();
    }

    setTimeout(function() {
        var inputValAfter500s = $input.val().trim();

        //此时认为用户没有在输入
        if (inputValAfter500s === inputVal) {
            es.search(opts, $eSearch2);
        }
    }, 500);
};
es.search = function(opts, $eSearch2) {
    //展开搜索列表
    $eSearch2.trigger('open');

    if ($.isArray(opts.searchSource)) {
        es.searchFromData(opts, $eSearch2);
    }
    else {
        es.searchFromAjax(opts, $eSearch2);
    }
};
es.searchFromAjax = function(opts, $eSearch2) {
    var eSearch2InstId = $eSearch2.attr('id');

    if (es.searchFromAjax[eSearch2InstId] == null) {
        es.searchFromAjax[eSearch2InstId] = {};
    }

    var xhr = es.searchFromAjax[eSearch2InstId].xhr;
    if (xhr != null && xhr.readyState !== 4) {
        xhr.abort();
    }

    var ajaxSendData = {};
    var inputVal = $eSearch2.find('.eui-search2-input .eui-input').val().trim();
    ajaxSendData[opts.searchKey] = encodeURIComponent(inputVal);

    //发送异步前的回调，必须要return ajaxSendData
    if ($.isFunction(opts.onBeforeSend)) {
        var cbThis = { $eSearch2: $eSearch2 };
        ajaxSendData = opts.onBeforeSend.call(cbThis, ajaxSendData);
    }

    //当返回 false 时，取消发送异步
    if (ajaxSendData === false) {
        return;
    }

    xhr = $.ajax({
        url: opts.searchSource,
        type: opts.searchType,
        data: ajaxSendData,
        dataType: 'json',
        timeout: 20000,
        beforeSend: function () {
            if (opts.showResult) {
                es.showMsgInList('努力搜索中...', $eSearch2);
            }
        },
        success: function (data) {
            var cbThis = { $eSearch2: $eSearch2, xhr: xhr };
            //获取数据后的回调，方便对数据进行格式处理
            //该回调必须显示return 一个数组类型的值
            if ($.isFunction(opts.onGetData)) {

                data = opts.onGetData.call(cbThis, data);
            }

            if (!opts.showResult) {
                if ($.isFunction(opts.onGetResult)) {
                    opts.onGetResult.call(cbThis, data);
                }
                return;
            }

            if (data == null || data.length === 0) {
                es.showMsgInList('没有搜到任何数据', $eSearch2);
            }
            else {
                es.renderSearchList(opts, $eSearch2, data);
            }

        },
        error: function () {
            if (opts.showResult) {
                es.showMsgInList('搜索出错了', $eSearch2);
            }
            else {
                if ($.isFunction(opts.onGetResult)) {
                    var cbThis = { $eSearch2: $eSearch2, xhr: xhr };
                    opts.onGetResult.call(cbThis, null);
                }
            }
        }
    });
};
es.searchFromData = function(opts, $eSearch2) {
    var inputVal = $eSearch2.find('.eui-search2-input .eui-input').val().trim();
    var searchedData = (opts.searchSource || []).filter(function(item) {
        return (item[opts.searchKey] || '').indexOf(inputVal) > -1;
    });

    if (!opts.showResult) {
        if ($.isFunction(opts.onGetResult)) {
            var cbThis = { $eSearch2: $eSearch2 };
            opts.onGetResult.call(cbThis, searchedData);
        }
        return;
    }

    if (searchedData.length === 0) {
        es.showMsgInList('没有搜到任何数据', $eSearch2);
    }
    else {
        es.renderSearchList(opts, $eSearch2, searchedData);
    }
};
es.setSelectedValue = function(opts, $eSearch2, $selectedItem) {
    var selectedItemValue = $selectedItem.attr(`data-id`);
    var selectedItemText = $selectedItem.text();
    var exsistSelectedItem = $eSearch2.find(`.eui-search2-multiSelectedItem[data-value="${selectedItemValue}"]`).length > 0;
    //将选中的添加到搜索框
    //
    if (opts.multiple && !exsistSelectedItem) {
        var selectedListItemHtml = `<div class="eui-search2-multiSelectedItem" data-value="${selectedItemValue}">${selectedItemText}</div>`;
        $eSearch2.find('.eui-search2-input').before(selectedListItemHtml);
    }
    else {
        var $input = $eSearch2.find('.eui-search2-input > .eui-input');
        $input.val(selectedItemText).attr('data-value', selectedItemValue).attr('title', selectedItemText);
        $input.siblings('.eui-btn').show();
    }

    //
    $eSearch2.trigger('close');

    if ($.isFunction(opts.onSelected)) {
        var cbThis = {
            $eSearch2: $eSearch2,
            $selectedItem: $selectedItem,
            value: es.getValue(opts, $eSearch2),
            text: es.getText(opts, $eSearch2)
        };
        opts.onSelected.call(cbThis);
    }
};
es.getValue = function(opts, $eSearch2) {
    var value;
    if (opts.multiple) {
        value = $.makeArray($eSearch2.find('.eui-search2-multiSelectedItem')).map(function(li) {
            return $(li).attr('data-value');
        }).join();
    }
    else {
        value = $eSearch2.find('.eui-search2-input > .eui-input').attr('data-value');
    }

    return value == null ? '' : value;
};
es.getText = function(opts, $eSearch2) {
    var text;
    if (opts.multiple) {
        text = $.makeArray($eSearch2.find('.eui-search2-multiSelectedItem')).map(function(li) {
            return $(li).text();
        }).join();
    }
    else {
        text = $eSearch2.find('.eui-search2-input > .eui-input').val();
    }

    return text;
};
es.deleteMultiSelectedItem = function(opts, $eSearch2, $deletedItem) {
    $deletedItem.remove();

    if ($.isFunction(opts.onDelete)) {
        var cbThis = {
            $eSearch2: $eSearch2,
            $deletedItem: $deletedItem,
            value: es.getValue(opts, $eSearch2),
            text: es.getText(opts, $eSearch2)
        };
        opts.onDelete.call(cbThis);
    }
};
es.clearInput = function(opts, $eSearch2, forbidCb) {
    var $input = $eSearch2.find('.eui-search2-input > .eui-input');

    $input.val('').attr('data-value', '').attr('title', '');
    $input.siblings('.eui-btn').hide();

    var canCallCb = $.isFunction(opts.onClear) && (typeof forbidCb === 'undefined' || forbidCb === false);
    if (canCallCb) {
        var cbThis = { $eSearch2: $eSearch2 };
        opts.onClear.call(cbThis);
    }
};
es.bindEvent = function(opts, $eSearch2) {
    $eSearch2.delegate('.eui-search2-input > .eui-input', 'focus', function(event) {
        var cbThis = { $eSearch2: $eSearch2, event: event };
        if ($.isFunction(opts.onFocus)) { opts.onFocus.call(cbThis); }
    });

    $eSearch2.delegate('.eui-search2-input > .eui-input', 'blur', function(event) {
        var cbThis = { $eSearch2: $eSearch2, event: event };
        if ($.isFunction(opts.onBlur)) { opts.onBlur.call(cbThis); }
    });

    $eSearch2.delegate('.eui-search2-input > .eui-input', 'input', function(event) {
        var $input = $(event.currentTarget);
        es.handleUserInputEvent(opts, $eSearch2, $input);
    });

    $eSearch2.delegate('.eui-search2-list > ul > li:not(.eui-search2-list-msg)', 'click', function(event) {
        var $selectedItem = $(event.currentTarget);
        es.setSelectedValue(opts, $eSearch2, $selectedItem);
    });

    $eSearch2.delegate('.eui-search2-multiSelectedItem', 'click', function(event) {
        var $deletedItem = $(event.currentTarget);
        es.deleteMultiSelectedItem(opts, $eSearch2, $deletedItem);
    });

    $eSearch2.delegate('.eui-search2-input > .eui-btn', 'click', function(event) {
        $eSearch2.trigger('clear');
    });

    $eSearch2.on('search', function(event) {
        es.search(opts, $eSearch2);
    });

    $eSearch2.on('clear', function(event) {
        es.clearInput(opts, $eSearch2);
    });

    $eSearch2.on('open', function(event) {
        $eSearch2.addClass('eui-search2-open');
    });

    $eSearch2.on('close', function(event) {
        $eSearch2.removeClass('eui-search2-open');

        if (opts.multiple) {
            es.clearInput(opts, $eSearch2, true);
        }
    });
};
es.init = function(opts, $element) {
    var exsist = $element.find('.eui-search2').length > 0;
    if (exsist) {
        es.destroy($element);
    }

    var $eSearch2 = es.render(opts, $element);
    es.bindEvent(opts, $eSearch2);
};


var ESearch2 = function(options, $element) {
    ++ESearch2.index;
    var opts = $.extend(true, {}, defaults, options);
    es.init(opts, $element);

    return {
        id: `eui-search2-${$element.attr('id') || ESearch2.index}`,
        selector: $element.selector,
        opts: opts,
    };
};

ESearch2.index = 0;

w.eSearch2Inst = {};

/**
 * 构造函数
 * 如果需要实例化的元素是单个，返回单个对象
 * 如果需要实例化的元素是多个，返回多个对象组成的数组
 */
$.fn.eSearch2 = function(options) {
    var result;
    if (this.length === 1) {
        result = new ESearch2(options, this);
        w.eSearch2Inst[result.id] = result;
    }
    else if (this.length > 1) {
        result = [];
        this.each(function() {
            var item = this;
            var eSearch2Instance = new ESearch2(options, item);
            w.eSearch2Inst[eSearch2Instance.id] = eSearch2Instance;
            result.push(eSearch2Instance);
        });
    }

    return result;
};

$.fn.eSearch2.getValue = function(selector) {
    var instList = w.eSearch2Inst;
    var instListFromSelector = [];
    for(var key in instList) {
        if (instList.hasOwnProperty(key) && instList[key].selector === selector) {
            instListFromSelector.push(instList[key]);
        }
    }

    var valueList = [];
    instListFromSelector.forEach(function(inst) {
        valueList.push({
            value: es.getValue(inst.opts, $(`#${inst.id}`)),
            id: inst.id
        });
    });

    if (valueList.length === 0) {
        throw new Error('not find eMultiSelectInstance');
    }
    else if (valueList.length === 1) {
        return valueList[0].value;
    }
    return valueList;
};

$.fn.eSearch2.getText = function(selector) {
    var instList = w.eSearch2Inst;
    var instListFromSelector = [];
    for(var key in instList) {
        if (instList.hasOwnProperty(key) && instList[key].selector === selector) {
            instListFromSelector.push(instList[key]);
        }
    }

    var textList = [];
    instListFromSelector.forEach(function(inst) {
        textList.push({
            text: es.getText(inst.opts, $(`#${inst.id}`)),
            id: inst.id
        });
    });

    if (textList.length === 0) {
        throw new Error('not find eMultiSelectInstance');
    }
    else if (textList.length === 1) {
        return textList[0].text;
    }
    return textList;
};

//defaults是一个数组
$.fn.eSearch2.setDefaults = function(selector, defaults) {
    var instList = w.eSearch2Inst;
    var instListFromSelector = [];
    for(var key in instList) {
        if (instList.hasOwnProperty(key) && instList[key].selector === selector) {
            instListFromSelector.push(instList[key]);
        }
    }

    instListFromSelector.forEach(function(inst) {
        defaults = defaults || [];
        var $eSearch2 = $(`#${inst.id}`);

        if (inst.opts.multiple) {
            var selectedListItemHtml = defaults.map(function(defaultItem) {
                return `<div class="eui-search2-multiSelectedItem" data-value="${defaultItem.value}">${defaultItem.text}</div>`;
            }).join('');
            
            $eSearch2.find('.eui-search2-input').before(selectedListItemHtml);
        }
        else {
            var $input = $eSearch2.find('.eui-search2-input > .eui-input');
            var selectedItemText = defaults[0].text || '';
            var selectedItemValue = defaults[0].value || '';
            $input.val(selectedItemText).attr('data-value', selectedItemValue).attr('title', selectedItemText);
            $input.siblings('.eui-btn').show();
        }        
    });    
}


$(function() {
    $('body').on('click', function(event) {
        var $clickEle = $(event.target);
        var isClickEleInEsearch2 = $clickEle.hasClass('eui-search2') || $clickEle.parents('.eui-search2').length > 0;
        if (!isClickEleInEsearch2) {
            $('.eui-search2').trigger('close');
        }
    });
});

})(jQuery, window);
/**
 * @pc端查看图片
 * @author gp10856
 *
 * #使用方法：
 * #1)引入eShow.js和eShow.css
 * #2)调用eShow(srcArray)方法，srcArray是由所要查看图片的链接构成的数组参数
 */
;(function(w, $) {
    //保存着当前查看的图片的url
    var _imgSrc = '';
    var _imgSrcs = [];
    var _deg = 0;
    //用来区分是否是由拖拽而触发的click事件
    var _isDrag = false;
    var _transformStyle = {
        rotate: 'rotateZ(0deg)',
        scale: 'scale(1)',
        translate3d: 'translate3d(0,0,0)'
    };

    function eShow(imgSrcs) {
        _imgSrcs = imgSrcs || [];
        _imgSrc = _imgSrcs[0];
        
        $('body').css('overflow-y', 'hidden');

        render();
        bindEvent();
    }

    /**
     * 渲染html结构
     */
    function render() {
        var pageBtnHtml = '';
        if (_imgSrcs.length > 1) {
            pageBtnHtml = '' + 
                    '<button type="button" id="dialog-showImgPc-prev" style="display: none;"></button>' +
                    '<button type="button" id="dialog-showImgPc-next"></button>';
        }

        var dialogHtml = '' +
                '<div id="dialog-showImgPc">' +
                    '<div id="dialog-showImgPc-imgBox"  data-scale="1" style="left: 50%; top: 50%;">' + 
                        '<img src="' + _imgSrcs[0] + '" alt="" style="transform: rotate(0deg);">' +
                    '</div>' +
                    '<div id="dialog-showImgPc-scaleBox">' +
                        
                        //'<span id="showImgPc-scaleBox-scaleNum">100%</span>' +
                        '<button type="button" id="showImgPc-scaleBox-zoomOut"></button>' +
                        '<button type="button" id="showImgPc-scaleBox-zoomIn"></button>' +  

                        '<button type="button" id="showImgPc-scaleBox-rotateRight"></button>' +
                        '<button type="button" id="showImgPc-scaleBox-rotateLeft"></button>' + 
                        
                    '</div>' +
                    pageBtnHtml +
                    '<button type="button" id="dialog-showImgPc-close"></button>' +
                '</div>';

        $('body').append(dialogHtml);
        //var imgBox = $('#dialog-showImgPc-imgBox');
        //var imgBoxWidth = imgBox.width() + 'px';
        //imgBox.find('>img').css('width', imgBoxWidth);
    }

    /**
     * 绑定所有的dom事件
     */
    function bindEvent() {
        $('#dialog-showImgPc').on('click', function(e) {
            if (_isDrag) {
                _isDrag = false;
                return;
            }
            destroy();
        });

        var isFirefox = navigator.userAgent.indexOf('Firefox') !== -1; 
        if (isFirefox) {
            $('#dialog-showImgPc-imgBox').on('DOMMouseScroll', function (e) {
                wheelZoom(e, this, true);
            });
        } else {
            $('#dialog-showImgPc-imgBox').on('mousewheel', function (e) {
                wheelZoom(e, this);
            });
        }

        moveImg();
            

        $('#dialog-showImgPc-prev').on('click', showPrevImg);

        $('#dialog-showImgPc-next').on('click', showNextImg);

        $('#showImgPc-scaleBox-rotateLeft').on('click', rotateImgLeft);

        $('#showImgPc-scaleBox-rotateRight').on('click', rotateImgRight);

        $('#showImgPc-scaleBox-zoomIn').on('click', function() {
            event.stopPropagation();
            zoomPic(-1, $('#dialog-showImgPc-imgBox'));
        });

        $('#showImgPc-scaleBox-zoomOut').on('click', function() {
            event.stopPropagation();
            zoomPic(1, $('#dialog-showImgPc-imgBox'));
        });

        $('#dialog-showImgPc-close').on('click', function () {
            destroy();
        });
    }

    /**
     * 关闭弹框时的销毁事件，主要是移除弹框dom元素，解绑dom事件，重置默认值
     * @return {[type]} [description]
     */
    function destroy() {
        _deg = 0;
        _imgSrcs = [];
        
        unBindEvent();
        closeDialog();
    }

    /**
     * 解绑dom事件
     * @return {[type]} [description]
     */
    function unBindEvent() {}

    /**
     * 移除弹框的dom元素
     */
    function closeDialog() {
        $('#dialog-showImgPc').remove();
        $('body').css('overflow-y', 'auto');
    }

    /**
     * 旋转图片
     * @param  {object} event 事件对象
     */
    function rotateImgLeft(event) {
        event.stopPropagation();

        _deg = _deg + 90;
        if (_deg == 360) {
            _deg = 0;
        }
        _transformStyle.rotate = 'rotateZ(' + _deg + 'deg)';

        $('#dialog-showImgPc-imgBox > img').css(
            {
                '-webkit-transform': _transformStyle.rotate + ' ' + _transformStyle.scale + ' ' + _transformStyle.translate3d,
                '-moz-transform': _transformStyle.rotate + ' ' + _transformStyle.scale + ' ' + _transformStyle.translate3d,
                'transform': _transformStyle.rotate + ' ' + _transformStyle.scale + ' ' + _transformStyle.translate3d
            }
        );
    }

    function rotateImgRight(event) {
        event.stopPropagation();

        _deg = _deg - 90;
        if (_deg == -360) {
            _deg = 0;
        }
        _transformStyle.rotate = 'rotateZ(' + _deg + 'deg)';

        $('#dialog-showImgPc-imgBox > img').css(
            {
                '-webkit-transform': _transformStyle.rotate + ' ' + _transformStyle.scale + ' ' + _transformStyle.translate3d,
                '-moz-transform': _transformStyle.rotate + ' ' + _transformStyle.scale + ' ' + _transformStyle.translate3d,
                'transform': _transformStyle.rotate + ' ' + _transformStyle.scale + ' ' + _transformStyle.translate3d
            }
        );
    }

    /**
     * 显示前一张图片
     * @param  {object} event 事件对象
     */
    function showPrevImg(event) {
        event.stopPropagation();
        var index;
        for (var i = 0; i < _imgSrcs.length; i++) {
            if (_imgSrcs[i] == _imgSrc) {
                index = i;
            }
        }
        if (index != 0) {            
            $('#dialog-showImgPc-imgBox > img').attr('src', _imgSrcs[--index]);
            _imgSrc = $('#dialog-showImgPc-imgBox > img').attr('src');

            if (index <= 0) {
                $('#dialog-showImgPc-prev').hide();
            }
            else {
                $('#dialog-showImgPc-prev').show();
            }
            $('#dialog-showImgPc-next').show();
        }
    }

    /**
     * 显示后一张图片
     * @param  {object} event 事件对象
     */
    function showNextImg(event) {
        event.stopPropagation();
        var index;
        for (var i = 0; i < _imgSrcs.length; i++) {
            if (_imgSrcs[i] == _imgSrc) {
                index = i;
            }
        }
        if (index != _imgSrcs.length) {
            $('#dialog-showImgPc-imgBox > img').attr('src', _imgSrcs[++index]);
            _imgSrc = $('#dialog-showImgPc-imgBox > img').attr('src');

            if (index >= _imgSrcs.length - 1) {
                $('#dialog-showImgPc-next').hide();
            }
            else {
                $('#dialog-showImgPc-next').show();
            }
            $('#dialog-showImgPc-prev').show();
        }
    }

    /**
     * 拖拽图片
     */
    function moveImg() {
        var imgBox = $('#dialog-showImgPc-imgBox')[0];
        var target = null;
        var isMove = false;
        imgBox.onmousedown = function(e) {
            e.preventDefault();
            e.stopPropagation();
            var nowLeft = imgBox.style.left;
            var nowTop = imgBox.style.top;
            if (imgBox.style.left == '50%') {
                nowLeft = $(window).width() / 2;
                nowTop = $(window).height() / 2;
            }
            var downX = e.screenX;
            var downY = e.screenY;
            target = e.target;
            if (target == imgBox.childNodes[0]) {
                document.onmousemove = function (e) {
                    var delX = e.screenX - downX;
                    var delY = e.screenY - downY;
                    imgBox.style.left = (parseFloat(nowLeft) + delX) + 'px';
                    imgBox.style.top = (parseFloat(nowTop) + delY) + 'px';
                    isMove = true;
                    _isDrag = true;
                };
                document.onmouseup = function (e) {
                    e.stopPropagation();
                    document.onmousemove = null;
                    document.onmouseup = null;
                    isMove = false;
                    target = null;
                };
            }
        };
    }

    function wheelZoom(e, obj, isFirefox) {
        obj.style.maxWidth = 'none';
        obj.style.maxHeight = 'none';
        var zoomDetail = e.originalEvent.wheelDelta;
        if (isFirefox) {
            zoomDetail = -e.originalEvent.detail;
        }
        zoomPic(zoomDetail, $(obj));
    }

    var zoomTimer = 0;
    function zoomPic(zoomDetail, obj) {
        var scale = Number($(obj).attr('data-scale'));
        if (zoomDetail > 0) {
            scale = scale + 0.05;
        } else {
            scale = scale - 0.05;
        }
        if (scale > 2) {
            scale = 2;
        } else if (scale < 0.1) {
            scale = 0.1;
        }
        obj.attr('data-scale', scale);
        $('#showImgPc-scaleBox-scaleNum').html((scale * 100).toFixed(0) + '%');
        var newTransform = 'translate(-50%,-50%) scale(' + scale + ')';
        obj.css({ '-webkit-transform': newTransform, '-moz-transform': newTransform, 'transform': newTransform });
    }

    w.eShow = eShow;
})(window, jQuery);
/**
	*@排序
	通过手动触发change事件传递按钮当前的排序状态
	*@author hzh23613
 */
;(function($, window, docment){
    var ESort = function(ele){
        this.element = $(ele);
        this.state = '';//排序状态  默认为无排序
    };

    var eSortId;
	ESort.prototype = {
		init:function(){
			this.destroy();
			this.setId();
			this.bindEvent();
		},
		setId:function(){
			eSortId = 'eui-sort-';
			eSortId = this.element.attr('id') != null && (this.element.attr('id')).trim() !== '' ? this.element.attr('id'): eSortId + ESort.index;
            this.element.attr('id',eSortId);
		},
		getId:function(){
            return this.element.attr('id');
		},
        destroy:function(){
			var that = this;
			var handleSortBtnClick = that.handleSortBtnClick;
            var hasExist = window.eSortIns[this.element.attr('id')] != null;
            if (hasExist) {
                this.element.off('click',handleSortBtnClick);
                delete window.eSortIns[this.element.attr('id')];
            }
        },
		bindEvent:function(){
			var that = this;
			var handleSortBtnClick = that.handleSortBtnClick;
			that.element.on('click',that,handleSortBtnClick);
		},
		handleSortBtnClick:function(e){
			var obj = e.data;
			var element = obj.element;
			if(!element.hasClass('eui-btn-sort-active')){
                element.addClass('eui-btn-sort-active').addClass('eui-btn-sort-asce');
                obj.state = 'asce';
            }
            else {
                if(element.hasClass('eui-btn-sort-asce')){
                    element.removeClass('eui-btn-sort-asce').addClass('eui-btn-sort-desc');
                    obj.state = 'desc';
                }else{
                    element.removeClass('eui-btn-sort-desc').addClass('eui-btn-sort-asce');
                    obj.state = 'asce';
                }
            }

            element.trigger('change', obj.state);
		}
	};

	ESort.index = 0;

	$.fn.eSort = function(){
		var eSort,id;
		if(this.length === 1){
			++ESort.index;
			eSort = new ESort(this);
            eSort.init();
			window.eSortIns[eSort.id] = eSort;
		}
		else if(this.length > 0 ){
			this.each(function(){
				var item = this;
				++ESort.index;
				eSort = new ESort(item);
				eSort.init();
				id = eSort.getId();
				window.eSortIns[id] = eSort;
			})
		}
	};

	window.eSortIns = {};
})(jQuery, window, document);

/**
 */


;(function($, w) {
    var ETab = function(options, element) {
        var defaults = {
            tabDirection: 'horizontal',
            tabStyle: 1,
            tabType: 1,
            animationType: 1,
            activeIndex: 1
        };

        var $eTab = $(element);
        var eTabId = 'eui-tab-';
        var eTabClassName = '';

        var opts = $.extend({}, defaults, options);

        function init() {
            destory();

            renderHtml();

            bindEvent();

            $eTab.find('> .eui-tab-btn-list > li:nth-child(' + opts.activeIndex + ')').trigger('click');
        }

        /**
         * 销毁已存在的实例
         * 由于Jq中remove时便可以解绑元素上的事件
         * 所以不再手动解绑事件，直接remove
         * 或者说不销毁还是用现有的实例
         */
        function destory() {
            var hasExist = w.eTabInst[$eTab.attr('id')] != null;
            if (hasExist) {
                $eTab.undelegate('> .eui-tab-btn-list > li', 'click');
                $eTab.removeClass(w.eTabInst[$eTab.attr('id')].className);
                delete w.eTabInst[$eTab.attr('id')];
            }
        }

        function renderHtml() {
            eTabClassName = 'eui-tab-type-' + opts.tabType + 
                    ' eui-tab-animationType-' + opts.animationType +
                    ' eui-tab-style-' + opts.tabStyle +
                    ' eui-tab-' + opts.tabDirection;
            eTabId = $eTab.attr('id') != null && ($eTab.attr('id')).trim() !== '' ? $eTab.attr('id') : eTabId + ETab.index;
            $eTab.addClass(eTabClassName).attr('id', eTabId);
        }

        function bindEvent() {
            $eTab.delegate('> .eui-tab-btn-list > li', 'click', handleTabBtnClick);
        }

        function handleTabBtnClick() {
            $(this).siblings('li.eui-tab-btn-active').removeClass('eui-tab-btn-active');
            $(this).addClass('eui-tab-btn-active');
            $eTab.trigger('change', $(this));

            var contList = $(this).parent().parent().find(' > .eui-tab-cont-list');
            var tab_btn = $(this);
            var tab_btn_index = $(this).parent().find('> li').index(tab_btn);

            switch('' + opts.tabType) {
                case '1': {
                    if ('' + opts.animationType === '1') {
                        var offset = tab_btn_index * (-100) + '%';
                        var position = opts.tabDirection === 'horizontal' ? 'left' : 'top';
                        contList.find('> ul').css(position, offset);
                    }
                    contList.find('> ul > .eui-tab-cont-active').removeClass('eui-tab-cont-active');
                    contList.find('> ul > li:nth-child(' + (tab_btn_index + 1) + ')').addClass('eui-tab-cont-active');
                    break;
                }
                case '2': {
                    break;
                }
            }          
        }


        init();


        //对外的属性和方法
        return {
            id: eTabId,
            className: eTabClassName
        };

    };

    //index用于唯一标志每个EDropDown实例
    ETab.index = 0;

    /**
     * 构造函数
     * 如果需要实例化的元素是单个，返回单个对象
     * 如果需要实例化的元素是多个，返回多个对象组成的数组
     */
    $.fn.eTab = function(options) {
        var result;
        if (this.length === 1) {
            ++ETab.index;
            result = new ETab(options, this);
            w.eTabInst[result.id] = result;
        }
        else if (this.length > 1) {
            result = [];
            this.each(function() {
                var item = this;
                ++ETab.index;
                result = new ETab(options, item);
                result.push(result);
                w.eTabInst[result.id] = result;
            });
        }

        return result;
    };
    
    //全局存放的实例对象，方便快速获取所有的实例
    w.eTabInst = {};

})(jQuery, window);
/**
 * 考虑最后不是立即上传的效果
 */


;(function($, w) {
    var EUpload = function(options, element) {
        var defaults = {
            type: 'img',
            accept: '*',
            multiple: false,
            maxSize: 0,
            maxLength: 100,
            uploadUrl: '',
            uploadKey: '',
            uploadNow: true,
            uploadTip: '',
            onSelected: null,
            onBeforeSend: null,
            onProgress: null,
            onSuccess: null,
            onFail: null,
            onDelete: null
        };

        var $element = $(element);
        var $eUpload = null;

        var opts = $.extend({}, defaults, options);

        function init() {
            //禁止重复初始化
            var hasExist = $element.find('.eui-upload-img').length > 0;
            if (hasExist) {
                return;
            }

            renderHtml();

            bindEvent();
        }

        function renderHtml() {
            var imgHtml = '' +
                    '<div class="eui-upload-img">' +
                        '<ul class="eui-upload-img-list"></ul>' +
                        '<div class="eui-upload-img-btn">' +
                            '<input type="file" accept="' + opts.accept + '">' +
                        '</div>' +
                        '<p class="eui-upload-img-tip">' + opts.uploadTip + '</p>' +
                    '</div>';

            $element.html(imgHtml);
            $eUpload = $element.find(' > .eui-upload-img');
        }

        function bindEvent() {
            $eUpload.delegate('input[type="file"]', 'change', function() {

                var inputFileEle = $eUpload.find('input[type="file"]');
                var file = this.files[0];

                if (!file) { return; }

                if (opts.maxSize > 0 && file.size > opts.maxSize) {
                    eMsg({
                        text: '选择的文件大小已超过了最大限制'
                    });
                    inputFileEle.replaceWith(inputFileEle.clone(true));
                    return;
                }

                var that = { file: file };
                var canUpload = true;

                if (opts.onSelected != null) { 
                    canUpload = opts.onSelected.call(that);
                }

                if (canUpload === false) {
                    inputFileEle.replaceWith(inputFileEle.clone(true));
                    return;
                }

                //当选择了最大个数的图片后隐藏上传按钮
                var canHideImgBtn = $eUpload.find(' > .eui-upload-img-list > li').length === opts.maxLength - 1;
                if (canHideImgBtn) {
                    $eUpload.find('.eui-upload-img-btn').hide();
                }

                //创建li,构造进度
                var liHtml = '' +
                        '<li class="eui-upload-uploading">' +
                            '<span class="eui-upload-progress">上传中</span>' +
                            '<button type="button" class="eui-upload-deleteBtn" title="删除"></button>' +
                        '</li>';
                var newLi = $eUpload.find('.eui-upload-img-list').append(liHtml).find('li:last-child');
                readImgDataUrl(file, function() {
                    var imgUrl = this.target.result;
                    newLi.css('background-image', 'url(' + imgUrl + ')');
                });
                startUpload(file, newLi);
            });

            $eUpload.delegate('.eui-upload-deleteBtn', 'click', function(e) {
                e.stopPropagation();
                var deleteBtn = e.target;
                var inputFileEle = $eUpload.find('input[type="file"]');

                $(deleteBtn).parent().remove();
                $eUpload.find('.eui-upload-img-btn').show();
                inputFileEle.replaceWith(inputFileEle.clone(true));
            });
        }

        function startUpload(file, element) {
            var uploadUrl = opts.uploadUrl;
            var fd = new FormData();
            var xhr = new XMLHttpRequest();
            fd.append(opts.uploadKey, file);

            xhr.onload = function(event) {
                var that = $.extend({ xhr: xhr }, { event: event }, {file: file}, {element: element});
                if ((xhr.status >= 200 && xhr.status < 300) || xhr.status == 304) {
                    //上传成功,进行成功的操作
                    if (opts.onSuccess != null) { opts.onSuccess.call(that); }
                }
                else {
                    //失败，进行失败的操作
                    if (opts.onFail != null) { opts.onFail.call(that); }
                }
            };

            xhr.upload.onprogress = function(event) {
                var progress = (event.loaded / event.total);
                var that = $.extend(
                    { xhr: xhr }, 
                    { event: event }, 
                    { file: file }, 
                    { element: element }, 
                    { progress: progress }
                );

                if (opts.onProgress != null) { opts.onProgress.call(that); }
            };
            xhr.open('post', uploadUrl, true);
            xhr.send(fd);
        }

        function readImgDataUrl(file, cb) {
            var reader = new FileReader();
            reader.onload = function(event) {
                var _this = event;
                if (cb) { cb.call(_this); }
            };
            reader.readAsDataURL(file);
        }


        init();

    };

    /**
     * 构造函数
     * 如果需要实例化的元素是单个，返回单个对象
     * 如果需要实例化的元素是多个，返回多个对象组成的数组
     */
    $.fn.eUpload = function(options) {
        var result;
        if (this.length === 1) {
            result = new EUpload(options, this);
        }
        else if (this.length > 1) {
            result = [];
            this.each(function() {
                var item = this;
                var eUploadInstance = new EUpload(options, item);
                result.push(eUploadInstance);
            });
        }

        return result;
    };

})(jQuery, window);

;(function($, w) {

w.eLoading = {
    show: function($wrapper) {
        el.render($wrapper);
    },
    hide: function($wrapper) {
        el.remove($wrapper);
    }
};

var el = {};
el.render = function($wrapper) {
    if (typeof $wrapper === 'undefined') {
        $wrapper = $('body');
    }
    else {
        $wrapper.addClass('eui-box-fixed-loading');
    }  

    var loadingHtml = '' +
            '<div class="eui-loading">' +
                '<div class="eui-loading-mask"></div>' +
                '<div class="eui-loading-cont">' +
                    '<div class="eui-loading-cont-rect1"></div>' +
                    '<div class="eui-loading-cont-rect2"></div>' +
                    '<div class="eui-loading-cont-rect3"></div>' +
                    '<div class="eui-loading-cont-rect4"></div>' +
                    '<div class="eui-loading-cont-rect5"></div>' +
                '</div>' +
            '</div>';
    if ($wrapper.find('> .eui-loading').length === 0) {
        $wrapper.append(loadingHtml);
    }
};
el.remove = function($wrapper) {
    $wrapper = $wrapper || $('body');
    var $loadings = $wrapper.find('> .eui-loading');
    $loadings.addClass('eui-loading-removing');

    setTimeout(function() {
        $loadings.remove();
        $wrapper.removeClass('eui-box-fixed-loading'); 
    }, 500);
};

})(jQuery, window);
/**
 * @version 1.1
 * @author xuejun
 * @class
 * 分页控件, 使用原生的JavaScript代码编写. 重写onclick方法, 获取翻页事件,
 * 可用来向服务器端发起AJAX请求.
 *
 * @param {String} id: HTML节点的id属性值, 控件将显示在该节点中.
 * @returns {PagerView}: 返回分页控件实例.
 *
 * @example
 * ### HTML:
 * &lt;div id="pager"&gt;&lt;/div&gt;
 *
 * ### JavaScript:
 * var pager = new PagerView('pager', {showSearch: true});
 * pager.index = 3; // 当前是第3页
 * pager.size = 16; // 每页显示16条记录
 * pager.itemCount = 100; // 一共有100条记录
 *
 * pager.onclick = function(index){
 *  alert('click on page: ' + index);
 *  // display data...
 * };
 *
 * pager.render();
 *
 */
;(function(window) {
window.PagerView = function(id, options) {
    var self = this;
    this.id = id;
    this.options = typeof options === 'undefined' ? {} : options;

    this._pageCount = 0; // 总页数
    this._start = 1; // 起始页码
    this._end = 1; // 结束页码

    /**
     * 当前控件所处的HTML节点引用.
     * @type DOMElement
     */
    this.container = null;
    /**
     * 当前页码, 从1开始
     * @type int
     */
    this.index = 1;
    /**
     * 每页显示记录数
     * @type int
     */
    this.size = 10;
    /**
     * 显示的分页按钮数量
     * @type int
     */
    this.maxButtons = 5;
    /**
     * 记录总数
     * @type int
     */
    this.itemCount = 0;

    /**
     *是否显示详细分页信息
     *@type bool
     */
    this.showDetails = true;

    /**
     *是否显示首页、末页
     *@type bool
     */
    this.showFirstLast = true;

    /**
     * 是否显示搜索跳转
     * @type bool
     */
    this.showSearch = typeof this.options.showSearch === 'undefined' ? false : this.options.showSearch;

    /**
     *需执行方法名
     * @type int
     */
    this.methodName = 'GetStrFormat';

    this.firstButtonText = '首页';
    this.lastButtonText = '末页';
    this.previousButtonText = '上一页';
    this.nextButtonText = '下一页';

    /**
     * 控件使用者重写本方法, 获取翻页事件, 可用来向服务器端发起AJAX请求.
     * 如果要取消本次翻页事件, 重写回调函数返回 false.
     * @param {int} index: 被点击的页码.
     * @returns {Boolean} 返回false表示取消本次翻页事件.
     * @event
     */
    this.onclick = function(index) {
        // modified by fj10854
        // 使参数能够使用"xxx.yyy.someMethod"这样传递方法名
        var methodName = this.methodName,
            nameSpaces = methodName.split('.'),
            parentMethodName = nameSpaces.shift(),
            parentMethod = window[parentMethodName];

        var temp = '';

        while (temp = nameSpaces.shift()) {
            parentMethod = parentMethod[temp] || (parentMethod[temp] = {});
        }

        if (typeof parentMethod === 'function') {
            parentMethod.call(null, index);
            return true;
        } else {
            return false;
        }
    };

    /**
     * 内部方法.
     */
    this._onclick = function(index) {
        var old = self.index;

        self.index = index;
        if (self.onclick(index) !== false) {
            self.render();
        } else {
            self.index = old;
        }
    };

    /**
     * 在显示之前计算各种页码变量的值.
     */
    this._calculate = function() {
        self._pageCount = parseInt(Math.ceil(self.itemCount / self.size));
        self.index = parseInt(self.index);
        if (self.index > self._pageCount) {
            self.index = self._pageCount;
        }
        if (self.index < 1) {
            self.index = 1;
        }

        self._start = Math.max(1, self.index - parseInt(self.maxButtons / 2));
        self._end = Math.min(self._pageCount, self._start + self.maxButtons - 1);
        self._start = Math.max(1, self._end - self.maxButtons + 1);
    };

    /**
     * 获取作为参数的数组落在相应页的数据片段.
     * @param {Array[Object]} rows
     * @returns {Array[Object]}
     */
    this.page = function(rows) {
        self._calculate();

        var s_num = (self.index - 1) * self.size;
        var e_num = self.index * self.size;

        return rows.slice(s_num, e_num);
    };

    /**
     * 渲染控件.
     */
    this.render = function() {
        var div = document.getElementById(self.id);
        div.view = self;
        self.container = div;

        self._calculate();

        var str = '';
        str += '<div class="PagerView">\n';
        if (self._pageCount > 1) {
            if (self.index != 1) {
                if (this.showFirstLast) {
                    str += '<a href="javascript://1"><span>' + this.firstButtonText + '</span></a>';
                }
                str += '<a href="javascript://' + (self.index - 1) + '"><span>' + this.previousButtonText + '</span></a>';
            } else {
                if (this.showFirstLast) {
                    str += '<span>' + this.firstButtonText + '</span>';
                }
                str += '<span>' + this.previousButtonText + '</span>';
            }
        }
        for (var i = self._start; i <= self._end; i++) {
            if (i == this.index) {
                str += '<span class="on">' + i + '</span>';
            } else {
                str += '<a href="javascript://' + i + '"><span>' + i + '</span></a>';
            }
        }
        if (self._pageCount > 1) {
            if (self.index != self._pageCount) {
                str += '<a href="javascript://' + (self.index + 1) + '"><span>' + this.nextButtonText + '</span></a>';
                if (this.showFirstLast) {
                    str += '<a href="javascript://' + self._pageCount + '"><span>' + this.lastButtonText + '</span></a>';
                }
            } else {
                str += '<span>' + this.nextButtonText + '</span>';
                if (this.showFirstLast) {
                    str += '<span>' + this.lastButtonText + '</span>';
                }
            }
        }
        if (this.showSearch && self._pageCount > 1) {
            str += '<div class="pagerView-search">跳转到<input type="number" class="eui-input"><button type="button" class="eui-btn eui-btn-primary">GO</button></div>';
        }
        if (this.showDetails && self.itemCount > 0) {
            str += ' 共' + self._pageCount + '页, ' + self.itemCount + '条记录 ';
        }
        str += '</div><!-- /.pagerView -->\n';

        self.container.innerHTML = str;

        var a_list = self.container.getElementsByTagName('a');
        for (var i = 0; i < a_list.length; i++) {
            a_list[i].onclick = function() {
                var index = this.getAttribute('href');
                if (index != undefined && index != '') {
                    index = parseInt(index.replace('javascript://', ''));
                    self._onclick(index);
                }
                return false;
            };
        }

        var goBtn = self.container.getElementsByTagName('button')[0];
        if (goBtn != null) {
            var pageInput = self.container.getElementsByTagName('input')[0];
            goBtn.onclick = function() {
                var index = pageInput.value;
                if (index != null && ('' + index).trim() !== '') {
                    self._onclick(index);
                }
            }
        }
    };
};
})(window);

;(function($, w){
    var ETip = function(options, element, timer) {
        var defaults = {
            type: 'hover',
            text: '',
            direction:'',
            hook:'',
            eTipMaxHeight:''
        };
        
        var opts = $.extend({}, defaults, options);

        var hideDelayTimer;
        var scrollPos;
        var scrollLeftValue;

        function init() {
            var tipText = $(element).attr('title');
            $(element).removeAttr('title').attr('data-title', tipText);           
            bindEvent();
        }
        function bindEvent() {     
            var eTipIndex = ETip.index;
            if(opts.type == 'click') {
                var clickCount = 0;
                $(element).on('click',function(event){
                    (event || window.event).stopPropagation();
                    clickCount++;
                    if(clickCount % 2 ==0){
                        clickCount=0;
                    }
                    if($('.eui-tip[id=eui-tip-'+eTipIndex+']').length > 0){
                        setTimeout(function() {
                            $('.eui-tip[id=eui-tip-'+eTipIndex+']').remove();
                        }, 100);
                        return;
                    }                  
                    if(clickCount === 1){
                        renderHtml(eTipIndex,'clickCause'); 
                    }                               
                }); 
            } else if(opts.type == 'hover' || opts.type == '') {
                $(element).on('mouseenter mouseleave',function(event){
                    if(event.type == "mouseenter"){ 
                        clearTimeout(hideDelayTimer);
                        if($('.eui-tip[id=eui-tip-'+eTipIndex+']').length > 0){
                            return;
                        } 
                        renderHtml(eTipIndex);   
                    } 
                    else if(event.type == "mouseleave"){
                        hideDelayTimer = setTimeout(function() {
                            $('.eui-tip[id=eui-tip-'+eTipIndex+']').remove();
                        }, 100);             
                    }
                });            
            } else if(opts.type == 'autoHover') {
                if($('.eui-tip[id=eui-tip-'+eTipIndex+']').length > 0){
                    return;
                }   
                renderHtml(eTipIndex,'ShowCause');
                if(timer === 0) {
                    return;             
                }  
                setTimeout(function() {
                    $('.eui-tip[id=eui-tip-'+eTipIndex+']').remove();
                }, timer || 3000);
            }
        }
        function renderHtml(eTipIndex, otherParam) {
            var tipText = opts.text.trim() || $(element).attr('data-title') || '';
            if(tipText == '') {
                return;
            }
            var eTipCont = '';
            var bodyScrollTop = $('body').scrollTop();
            var bodyScrollLeft = $('body').scrollLeft();
            eTipCont = `<div class="eui-tip ${opts.hook.split(',').join(' ')}" id=${'eui-tip-'+eTipIndex}>
                                <div class="eui-tip-inner" style="max-height:${opts.eTipMaxHeight ? opts.eTipMaxHeight+'px' : 'none'}">${tipText}</div>
                                <i class="eui-tip-icon"></i>
                            </div>`;    
            $('body').append(eTipCont);
            setTipPosition(eTipIndex, otherParam, bodyScrollTop, bodyScrollLeft);
        }
        function setTipPosition(eTipIndex,clickCause, bodyScrollTop, bodyScrollLeft){
            var eTipEle = $('.eui-tip[id=eui-tip-'+eTipIndex+']');

            var iconDirection = '';

            var elementLeft = $(element).offset().left;
            var elementTop = $(element).offset().top;

            var elementWidth = $(element).outerWidth();
            var elementHeight = $(element).outerHeight();

            var eTipWidth = eTipEle.outerWidth();
            var eTipHeight = eTipEle.outerHeight();

            var TipInRight = {
                                'left': elementLeft + elementWidth + 5 + 'px',
                                'top': elementTop - 5 +'px'
                            };
            var TipInBottom = {
                                'left': elementLeft + 'px',
                                'top': elementTop + elementHeight + 5 + 'px'
                            }
            var TipInLeft = {
                                'left': elementLeft - eTipWidth - 5 + 'px',
                                'top': elementTop - 5 + 'px'
                            }
            var TipInTop = {
                                'left': elementLeft + 'px',
                                'top': elementTop - eTipHeight - 5 + 'px'
                            }

            //判断内容在视口内能否显示完全
            elementTop = elementTop - bodyScrollTop; 
            elementLeft = elementLeft - bodyScrollLeft; 

            if(!opts.direction) {
                //右
                if((elementLeft + elementWidth + eTipWidth <= $(w).width()) 
                    && elementTop + eTipHeight -5 <= $(w).height()) {
                    eTipEle.css(TipInRight);
                    iconDirection = 'right';
                } 
                //左
                else if(elementLeft >= eTipWidth
                        && elementTop + eTipHeight -5 <= $(w).height()) {    
                    eTipEle.css(TipInLeft);
                    iconDirection = 'left';
                }
                //下
                else if(elementTop + elementHeight + eTipHeight <= $(w).height()
                     && elementLeft + eTipWidth < $(w).width()){
                    eTipEle.css(TipInBottom);
                    iconDirection = 'bottom';
                }
                //上
                else {
                    eTipEle.css(TipInTop);
                    if(eTipHeight !=$('.eui-tip[id=eui-tip-'+eTipIndex+']').outerHeight()) {
                        eTipHeight = $('.eui-tip[id=eui-tip-'+eTipIndex+']').outerHeight(); 
                        eTipEle.css({
                            'top': elementTop+$('body').scrollTop() - eTipHeight - 5 + 'px'
                        });
                    }
                    iconDirection = 'top';
                } 
            } else {
                switch(opts.direction) {
                    case 'right':
                        eTipEle.css(TipInRight);
                        iconDirection = 'right';
                        break;
                    case 'left':
                        eTipEle.css(TipInLeft);
                        iconDirection = 'left';
                        break;
                    case 'bottom':
                        eTipEle.css(TipInBottom);
                        iconDirection = 'bottom';
                        break;
                    case 'top':
                        eTipEle.css(TipInTop);
                        if(eTipHeight !=$('.eui-tip[id=eui-tip-'+eTipIndex+']').outerHeight()) {
                            eTipHeight = $('.eui-tip[id=eui-tip-'+eTipIndex+']').outerHeight(); 
                            eTipEle.css({
                                'top': elementTop+$('body').scrollTop() - eTipHeight - 5 + 'px'
                            });
                        }
                        iconDirection = 'top';
                        break;
                }
            }
            eTipEle.find('.eui-tip-icon').addClass('eui-tip-icon-'+iconDirection);
            // $(element).attr('_currEleTip','_currEleTip'+eTipIndex);

            if(!clickCause) {
                tipHoverFn(eTipIndex);
            }

            //多次初始化规避(hover)
            if($(element).attr('_currEleTip')) {
                var num = $(element).attr('_currEleTip').match(/\d+/).join();
                if(num != eTipIndex) {
                    $('#eui-tip-'+num).remove();
                    $(element).attr('_currEleTip','_currEleTip'+eTipIndex);
                }
            } else {
                $(element).attr('_currEleTip','_currEleTip'+eTipIndex);
            }
                  
            
        }
        function tipHoverFn(eTipIndex) {
            $('.eui-tip[id=eui-tip-'+eTipIndex+']').hover(function(){
                clearTimeout(hideDelayTimer);
            }, function(){
                hideDelayTimer = setTimeout(function() {
                    $('.eui-tip[id=eui-tip-'+eTipIndex+']').remove();
                }, 100);   
            });
        }

        init();

        return {
            id: 'eui-tip-'+ETip.index,
        }

    };

    ETip.index = 0;
    w.eTipInstance = {};

    // $.fn.eTip = function(options){
    $.fn.eTip = function(options, selector, timer){    
        var result;
        if(this.length === 1) {
            ++ETip.index;           
            // result = new ETip(options, this);
            result = new ETip(options, $(this), timer);
            w.eTipInstance[result.id] = result;
        } else if (this.length > 1) {
            result = [];
            this.each(function() {
                var item = this;
                ++ETip.index;
                // var eTipInst = new ETip(options, item);
                var eTipInst = new ETip(options, item, timer);
                w.eTipInstance[eTipInst.id] = eTipInst;
                result.push(eTipInst);
            });
        }
        return $(this);
    };
    

    $.fn.eTip.show = function(selector, text, direction, timer,hook,eTipMaxHeight){
        var result;
        var options = {
            type: 'autoHover',
            text:text,
            direction:direction,
            hook:hook,
            eTipMaxHeight:eTipMaxHeight,
        };

        if($(selector).length === 1) {
            if($(selector).attr('_currEleTip')) {
                var num = $(selector).attr('_currEleTip').match(/\d+/).join();
                if($('#eui-tip-'+num).length > 0) {
                    $('#eui-tip-'+num).remove();
                }
            }
            $(selector).eTip(options, $(selector), timer);
        } else if ($(selector).length > 1) {
            result = [];
            $(selector).each(function() { 
                var item = this;
                if($(item).attr('_currEleTip')) {
                    var num = $(item).attr('_currEleTip').match(/\d+/).join();
                    if($('#eui-tip-'+num).length > 0) {
                        $('#eui-tip-'+num).remove();
                    }
                }
                $(selector).eTip(options, item, timer);
            });
        }
        return result;
    };
    $.fn.eTip.hide = function(selector){
        $(selector).each(function(index, item) { 
            var _currEleTip = $(item).attr('_currEleTip');
            if(_currEleTip){
                var num = _currEleTip.match(/\d+/).join();
                setTimeout(function() {
                    $('#eui-tip-'+num).remove();
                }, 100);  
            }    
        });     
    };

})(jQuery, window);

/*
 * JQuery zTree core v3.5.28
 * http://treejs.cn/
 *
 * Copyright (c) 2010 Hunter.z
 *
 * Licensed same as jquery - MIT License
 * http://www.opensource.org/licenses/mit-license.php
 *
 * email: hunter.z@263.net
 * Date: 2017-01-20
 */
(function(q){var H,I,J,K,L,M,u,r={},v={},w={},N={treeId:"",treeObj:null,view:{addDiyDom:null,autoCancelSelected:!0,dblClickExpand:!0,expandSpeed:"fast",fontCss:{},nameIsHTML:!1,selectedMulti:!0,showIcon:!0,showLine:!0,showTitle:!0,txtSelectedEnable:!1},data:{key:{children:"children",name:"name",title:"",url:"url",icon:"icon"},simpleData:{enable:!1,idKey:"id",pIdKey:"pId",rootPId:null},keep:{parent:!1,leaf:!1}},async:{enable:!1,contentType:"application/x-www-form-urlencoded",type:"post",dataType:"text",
url:"",autoParam:[],otherParam:[],dataFilter:null},callback:{beforeAsync:null,beforeClick:null,beforeDblClick:null,beforeRightClick:null,beforeMouseDown:null,beforeMouseUp:null,beforeExpand:null,beforeCollapse:null,beforeRemove:null,onAsyncError:null,onAsyncSuccess:null,onNodeCreated:null,onClick:null,onDblClick:null,onRightClick:null,onMouseDown:null,onMouseUp:null,onExpand:null,onCollapse:null,onRemove:null}},x=[function(b){var a=b.treeObj,c=f.event;a.bind(c.NODECREATED,function(a,c,g){j.apply(b.callback.onNodeCreated,
[a,c,g])});a.bind(c.CLICK,function(a,c,g,l,h){j.apply(b.callback.onClick,[c,g,l,h])});a.bind(c.EXPAND,function(a,c,g){j.apply(b.callback.onExpand,[a,c,g])});a.bind(c.COLLAPSE,function(a,c,g){j.apply(b.callback.onCollapse,[a,c,g])});a.bind(c.ASYNC_SUCCESS,function(a,c,g,l){j.apply(b.callback.onAsyncSuccess,[a,c,g,l])});a.bind(c.ASYNC_ERROR,function(a,c,g,l,h,f){j.apply(b.callback.onAsyncError,[a,c,g,l,h,f])});a.bind(c.REMOVE,function(a,c,g){j.apply(b.callback.onRemove,[a,c,g])});a.bind(c.SELECTED,
function(a,c,g){j.apply(b.callback.onSelected,[c,g])});a.bind(c.UNSELECTED,function(a,c,g){j.apply(b.callback.onUnSelected,[c,g])})}],y=[function(b){var a=f.event;b.treeObj.unbind(a.NODECREATED).unbind(a.CLICK).unbind(a.EXPAND).unbind(a.COLLAPSE).unbind(a.ASYNC_SUCCESS).unbind(a.ASYNC_ERROR).unbind(a.REMOVE).unbind(a.SELECTED).unbind(a.UNSELECTED)}],z=[function(b){var a=h.getCache(b);a||(a={},h.setCache(b,a));a.nodes=[];a.doms=[]}],A=[function(b,a,c,d,e,g){if(c){var l=h.getRoot(b),f=b.data.key.children;
c.level=a;c.tId=b.treeId+"_"+ ++l.zId;c.parentTId=d?d.tId:null;c.open=typeof c.open=="string"?j.eqs(c.open,"true"):!!c.open;c[f]&&c[f].length>0?(c.isParent=!0,c.zAsync=!0):(c.isParent=typeof c.isParent=="string"?j.eqs(c.isParent,"true"):!!c.isParent,c.open=c.isParent&&!b.async.enable?c.open:!1,c.zAsync=!c.isParent);c.isFirstNode=e;c.isLastNode=g;c.getParentNode=function(){return h.getNodeCache(b,c.parentTId)};c.getPreNode=function(){return h.getPreNode(b,c)};c.getNextNode=function(){return h.getNextNode(b,
c)};c.getIndex=function(){return h.getNodeIndex(b,c)};c.getPath=function(){return h.getNodePath(b,c)};c.isAjaxing=!1;h.fixPIdKeyValue(b,c)}}],t=[function(b){var a=b.target,c=h.getSetting(b.data.treeId),d="",e=null,g="",l="",i=null,n=null,k=null;if(j.eqs(b.type,"mousedown"))l="mousedown";else if(j.eqs(b.type,"mouseup"))l="mouseup";else if(j.eqs(b.type,"contextmenu"))l="contextmenu";else if(j.eqs(b.type,"click"))if(j.eqs(a.tagName,"span")&&a.getAttribute("treeNode"+f.id.SWITCH)!==null)d=j.getNodeMainDom(a).id,
g="switchNode";else{if(k=j.getMDom(c,a,[{tagName:"a",attrName:"treeNode"+f.id.A}]))d=j.getNodeMainDom(k).id,g="clickNode"}else if(j.eqs(b.type,"dblclick")&&(l="dblclick",k=j.getMDom(c,a,[{tagName:"a",attrName:"treeNode"+f.id.A}])))d=j.getNodeMainDom(k).id,g="switchNode";if(l.length>0&&d.length==0&&(k=j.getMDom(c,a,[{tagName:"a",attrName:"treeNode"+f.id.A}])))d=j.getNodeMainDom(k).id;if(d.length>0)switch(e=h.getNodeCache(c,d),g){case "switchNode":e.isParent?j.eqs(b.type,"click")||j.eqs(b.type,"dblclick")&&
j.apply(c.view.dblClickExpand,[c.treeId,e],c.view.dblClickExpand)?i=H:g="":g="";break;case "clickNode":i=I}switch(l){case "mousedown":n=J;break;case "mouseup":n=K;break;case "dblclick":n=L;break;case "contextmenu":n=M}return{stop:!1,node:e,nodeEventType:g,nodeEventCallback:i,treeEventType:l,treeEventCallback:n}}],B=[function(b){var a=h.getRoot(b);a||(a={},h.setRoot(b,a));a[b.data.key.children]=[];a.expandTriggerFlag=!1;a.curSelectedList=[];a.noSelection=!0;a.createdNodes=[];a.zId=0;a._ver=(new Date).getTime()}],
C=[],D=[],E=[],F=[],G=[],h={addNodeCache:function(b,a){h.getCache(b).nodes[h.getNodeCacheId(a.tId)]=a},getNodeCacheId:function(b){return b.substring(b.lastIndexOf("_")+1)},addAfterA:function(b){D.push(b)},addBeforeA:function(b){C.push(b)},addInnerAfterA:function(b){F.push(b)},addInnerBeforeA:function(b){E.push(b)},addInitBind:function(b){x.push(b)},addInitUnBind:function(b){y.push(b)},addInitCache:function(b){z.push(b)},addInitNode:function(b){A.push(b)},addInitProxy:function(b,a){a?t.splice(0,0,
b):t.push(b)},addInitRoot:function(b){B.push(b)},addNodesData:function(b,a,c,d){var e=b.data.key.children;a[e]?c>=a[e].length&&(c=-1):(a[e]=[],c=-1);if(a[e].length>0&&c===0)a[e][0].isFirstNode=!1,i.setNodeLineIcos(b,a[e][0]);else if(a[e].length>0&&c<0)a[e][a[e].length-1].isLastNode=!1,i.setNodeLineIcos(b,a[e][a[e].length-1]);a.isParent=!0;c<0?a[e]=a[e].concat(d):(b=[c,0].concat(d),a[e].splice.apply(a[e],b))},addSelectedNode:function(b,a){var c=h.getRoot(b);h.isSelectedNode(b,a)||c.curSelectedList.push(a)},
addCreatedNode:function(b,a){(b.callback.onNodeCreated||b.view.addDiyDom)&&h.getRoot(b).createdNodes.push(a)},addZTreeTools:function(b){G.push(b)},exSetting:function(b){q.extend(!0,N,b)},fixPIdKeyValue:function(b,a){b.data.simpleData.enable&&(a[b.data.simpleData.pIdKey]=a.parentTId?a.getParentNode()[b.data.simpleData.idKey]:b.data.simpleData.rootPId)},getAfterA:function(b,a,c){for(var d=0,e=D.length;d<e;d++)D[d].apply(this,arguments)},getBeforeA:function(b,a,c){for(var d=0,e=C.length;d<e;d++)C[d].apply(this,
arguments)},getInnerAfterA:function(b,a,c){for(var d=0,e=F.length;d<e;d++)F[d].apply(this,arguments)},getInnerBeforeA:function(b,a,c){for(var d=0,e=E.length;d<e;d++)E[d].apply(this,arguments)},getCache:function(b){return w[b.treeId]},getNodeIndex:function(b,a){if(!a)return null;for(var c=b.data.key.children,d=a.parentTId?a.getParentNode():h.getRoot(b),e=0,g=d[c].length-1;e<=g;e++)if(d[c][e]===a)return e;return-1},getNextNode:function(b,a){if(!a)return null;for(var c=b.data.key.children,d=a.parentTId?
a.getParentNode():h.getRoot(b),e=0,g=d[c].length-1;e<=g;e++)if(d[c][e]===a)return e==g?null:d[c][e+1];return null},getNodeByParam:function(b,a,c,d){if(!a||!c)return null;for(var e=b.data.key.children,g=0,l=a.length;g<l;g++){if(a[g][c]==d)return a[g];var f=h.getNodeByParam(b,a[g][e],c,d);if(f)return f}return null},getNodeCache:function(b,a){if(!a)return null;var c=w[b.treeId].nodes[h.getNodeCacheId(a)];return c?c:null},getNodeName:function(b,a){return""+a[b.data.key.name]},getNodePath:function(b,a){if(!a)return null;
var c;(c=a.parentTId?a.getParentNode().getPath():[])&&c.push(a);return c},getNodeTitle:function(b,a){return""+a[b.data.key.title===""?b.data.key.name:b.data.key.title]},getNodes:function(b){return h.getRoot(b)[b.data.key.children]},getNodesByParam:function(b,a,c,d){if(!a||!c)return[];for(var e=b.data.key.children,g=[],l=0,f=a.length;l<f;l++)a[l][c]==d&&g.push(a[l]),g=g.concat(h.getNodesByParam(b,a[l][e],c,d));return g},getNodesByParamFuzzy:function(b,a,c,d){if(!a||!c)return[];for(var e=b.data.key.children,
g=[],d=d.toLowerCase(),l=0,f=a.length;l<f;l++)typeof a[l][c]=="string"&&a[l][c].toLowerCase().indexOf(d)>-1&&g.push(a[l]),g=g.concat(h.getNodesByParamFuzzy(b,a[l][e],c,d));return g},getNodesByFilter:function(b,a,c,d,e){if(!a)return d?null:[];for(var g=b.data.key.children,f=d?null:[],i=0,n=a.length;i<n;i++){if(j.apply(c,[a[i],e],!1)){if(d)return a[i];f.push(a[i])}var k=h.getNodesByFilter(b,a[i][g],c,d,e);if(d&&k)return k;f=d?k:f.concat(k)}return f},getPreNode:function(b,a){if(!a)return null;for(var c=
b.data.key.children,d=a.parentTId?a.getParentNode():h.getRoot(b),e=0,g=d[c].length;e<g;e++)if(d[c][e]===a)return e==0?null:d[c][e-1];return null},getRoot:function(b){return b?v[b.treeId]:null},getRoots:function(){return v},getSetting:function(b){return r[b]},getSettings:function(){return r},getZTreeTools:function(b){return(b=this.getRoot(this.getSetting(b)))?b.treeTools:null},initCache:function(b){for(var a=0,c=z.length;a<c;a++)z[a].apply(this,arguments)},initNode:function(b,a,c,d,e,g){for(var f=
0,h=A.length;f<h;f++)A[f].apply(this,arguments)},initRoot:function(b){for(var a=0,c=B.length;a<c;a++)B[a].apply(this,arguments)},isSelectedNode:function(b,a){for(var c=h.getRoot(b),d=0,e=c.curSelectedList.length;d<e;d++)if(a===c.curSelectedList[d])return!0;return!1},removeNodeCache:function(b,a){var c=b.data.key.children;if(a[c])for(var d=0,e=a[c].length;d<e;d++)h.removeNodeCache(b,a[c][d]);h.getCache(b).nodes[h.getNodeCacheId(a.tId)]=null},removeSelectedNode:function(b,a){for(var c=h.getRoot(b),
d=0,e=c.curSelectedList.length;d<e;d++)if(a===c.curSelectedList[d]||!h.getNodeCache(b,c.curSelectedList[d].tId))c.curSelectedList.splice(d,1),b.treeObj.trigger(f.event.UNSELECTED,[b.treeId,a]),d--,e--},setCache:function(b,a){w[b.treeId]=a},setRoot:function(b,a){v[b.treeId]=a},setZTreeTools:function(b,a){for(var c=0,d=G.length;c<d;c++)G[c].apply(this,arguments)},transformToArrayFormat:function(b,a){if(!a)return[];var c=b.data.key.children,d=[];if(j.isArray(a))for(var e=0,g=a.length;e<g;e++)d.push(a[e]),
a[e][c]&&(d=d.concat(h.transformToArrayFormat(b,a[e][c])));else d.push(a),a[c]&&(d=d.concat(h.transformToArrayFormat(b,a[c])));return d},transformTozTreeFormat:function(b,a){var c,d,e=b.data.simpleData.idKey,g=b.data.simpleData.pIdKey,f=b.data.key.children;if(!e||e==""||!a)return[];if(j.isArray(a)){var h=[],i={};for(c=0,d=a.length;c<d;c++)i[a[c][e]]=a[c];for(c=0,d=a.length;c<d;c++)i[a[c][g]]&&a[c][e]!=a[c][g]?(i[a[c][g]][f]||(i[a[c][g]][f]=[]),i[a[c][g]][f].push(a[c])):h.push(a[c]);return h}else return[a]}},
m={bindEvent:function(b){for(var a=0,c=x.length;a<c;a++)x[a].apply(this,arguments)},unbindEvent:function(b){for(var a=0,c=y.length;a<c;a++)y[a].apply(this,arguments)},bindTree:function(b){var a={treeId:b.treeId},c=b.treeObj;b.view.txtSelectedEnable||c.bind("selectstart",u).css({"-moz-user-select":"-moz-none"});c.bind("click",a,m.proxy);c.bind("dblclick",a,m.proxy);c.bind("mouseover",a,m.proxy);c.bind("mouseout",a,m.proxy);c.bind("mousedown",a,m.proxy);c.bind("mouseup",a,m.proxy);c.bind("contextmenu",
a,m.proxy)},unbindTree:function(b){b.treeObj.unbind("selectstart",u).unbind("click",m.proxy).unbind("dblclick",m.proxy).unbind("mouseover",m.proxy).unbind("mouseout",m.proxy).unbind("mousedown",m.proxy).unbind("mouseup",m.proxy).unbind("contextmenu",m.proxy)},doProxy:function(b){for(var a=[],c=0,d=t.length;c<d;c++){var e=t[c].apply(this,arguments);a.push(e);if(e.stop)break}return a},proxy:function(b){var a=h.getSetting(b.data.treeId);if(!j.uCanDo(a,b))return!0;for(var a=m.doProxy(b),c=!0,d=0,e=a.length;d<
e;d++){var g=a[d];g.nodeEventCallback&&(c=g.nodeEventCallback.apply(g,[b,g.node])&&c);g.treeEventCallback&&(c=g.treeEventCallback.apply(g,[b,g.node])&&c)}return c}};H=function(b,a){var c=h.getSetting(b.data.treeId);if(a.open){if(j.apply(c.callback.beforeCollapse,[c.treeId,a],!0)==!1)return!0}else if(j.apply(c.callback.beforeExpand,[c.treeId,a],!0)==!1)return!0;h.getRoot(c).expandTriggerFlag=!0;i.switchNode(c,a);return!0};I=function(b,a){var c=h.getSetting(b.data.treeId),d=c.view.autoCancelSelected&&
(b.ctrlKey||b.metaKey)&&h.isSelectedNode(c,a)?0:c.view.autoCancelSelected&&(b.ctrlKey||b.metaKey)&&c.view.selectedMulti?2:1;if(j.apply(c.callback.beforeClick,[c.treeId,a,d],!0)==!1)return!0;d===0?i.cancelPreSelectedNode(c,a):i.selectNode(c,a,d===2);c.treeObj.trigger(f.event.CLICK,[b,c.treeId,a,d]);return!0};J=function(b,a){var c=h.getSetting(b.data.treeId);j.apply(c.callback.beforeMouseDown,[c.treeId,a],!0)&&j.apply(c.callback.onMouseDown,[b,c.treeId,a]);return!0};K=function(b,a){var c=h.getSetting(b.data.treeId);
j.apply(c.callback.beforeMouseUp,[c.treeId,a],!0)&&j.apply(c.callback.onMouseUp,[b,c.treeId,a]);return!0};L=function(b,a){var c=h.getSetting(b.data.treeId);j.apply(c.callback.beforeDblClick,[c.treeId,a],!0)&&j.apply(c.callback.onDblClick,[b,c.treeId,a]);return!0};M=function(b,a){var c=h.getSetting(b.data.treeId);j.apply(c.callback.beforeRightClick,[c.treeId,a],!0)&&j.apply(c.callback.onRightClick,[b,c.treeId,a]);return typeof c.callback.onRightClick!="function"};u=function(b){b=b.originalEvent.srcElement.nodeName.toLowerCase();
return b==="input"||b==="textarea"};var j={apply:function(b,a,c){return typeof b=="function"?b.apply(O,a?a:[]):c},canAsync:function(b,a){var c=b.data.key.children;return b.async.enable&&a&&a.isParent&&!(a.zAsync||a[c]&&a[c].length>0)},clone:function(b){if(b===null)return null;var a=j.isArray(b)?[]:{},c;for(c in b)a[c]=b[c]instanceof Date?new Date(b[c].getTime()):typeof b[c]==="object"?j.clone(b[c]):b[c];return a},eqs:function(b,a){return b.toLowerCase()===a.toLowerCase()},isArray:function(b){return Object.prototype.toString.apply(b)===
"[object Array]"},isElement:function(b){return typeof HTMLElement==="object"?b instanceof HTMLElement:b&&typeof b==="object"&&b!==null&&b.nodeType===1&&typeof b.nodeName==="string"},$:function(b,a,c){a&&typeof a!="string"&&(c=a,a="");return typeof b=="string"?q(b,c?c.treeObj.get(0).ownerDocument:null):q("#"+b.tId+a,c?c.treeObj:null)},getMDom:function(b,a,c){if(!a)return null;for(;a&&a.id!==b.treeId;){for(var d=0,e=c.length;a.tagName&&d<e;d++)if(j.eqs(a.tagName,c[d].tagName)&&a.getAttribute(c[d].attrName)!==
null)return a;a=a.parentNode}return null},getNodeMainDom:function(b){return q(b).parent("li").get(0)||q(b).parentsUntil("li").parent().get(0)},isChildOrSelf:function(b,a){return q(b).closest("#"+a).length>0},uCanDo:function(){return!0}},i={addNodes:function(b,a,c,d,e){if(!b.data.keep.leaf||!a||a.isParent)if(j.isArray(d)||(d=[d]),b.data.simpleData.enable&&(d=h.transformTozTreeFormat(b,d)),a){var g=k(a,f.id.SWITCH,b),l=k(a,f.id.ICON,b),p=k(a,f.id.UL,b);if(!a.open)i.replaceSwitchClass(a,g,f.folder.CLOSE),
i.replaceIcoClass(a,l,f.folder.CLOSE),a.open=!1,p.css({display:"none"});h.addNodesData(b,a,c,d);i.createNodes(b,a.level+1,d,a,c);e||i.expandCollapseParentNode(b,a,!0)}else h.addNodesData(b,h.getRoot(b),c,d),i.createNodes(b,0,d,null,c)},appendNodes:function(b,a,c,d,e,g,f){if(!c)return[];var j=[],n=b.data.key.children,k=(d?d:h.getRoot(b))[n],m,Q;if(!k||e>=k.length-c.length)e=-1;for(var s=0,R=c.length;s<R;s++){var o=c[s];g&&(m=(e===0||k.length==c.length)&&s==0,Q=e<0&&s==c.length-1,h.initNode(b,a,o,d,
m,Q,f),h.addNodeCache(b,o));m=[];o[n]&&o[n].length>0&&(m=i.appendNodes(b,a+1,o[n],o,-1,g,f&&o.open));f&&(i.makeDOMNodeMainBefore(j,b,o),i.makeDOMNodeLine(j,b,o),h.getBeforeA(b,o,j),i.makeDOMNodeNameBefore(j,b,o),h.getInnerBeforeA(b,o,j),i.makeDOMNodeIcon(j,b,o),h.getInnerAfterA(b,o,j),i.makeDOMNodeNameAfter(j,b,o),h.getAfterA(b,o,j),o.isParent&&o.open&&i.makeUlHtml(b,o,j,m.join("")),i.makeDOMNodeMainAfter(j,b,o),h.addCreatedNode(b,o))}return j},appendParentULDom:function(b,a){var c=[],d=k(a,b);!d.get(0)&&
a.parentTId&&(i.appendParentULDom(b,a.getParentNode()),d=k(a,b));var e=k(a,f.id.UL,b);e.get(0)&&e.remove();e=i.appendNodes(b,a.level+1,a[b.data.key.children],a,-1,!1,!0);i.makeUlHtml(b,a,c,e.join(""));d.append(c.join(""))},asyncNode:function(b,a,c,d){var e,g;if(a&&!a.isParent)return j.apply(d),!1;else if(a&&a.isAjaxing)return!1;else if(j.apply(b.callback.beforeAsync,[b.treeId,a],!0)==!1)return j.apply(d),!1;if(a)a.isAjaxing=!0,k(a,f.id.ICON,b).attr({style:"","class":f.className.BUTTON+" "+f.className.ICO_LOADING});
var l={};for(e=0,g=b.async.autoParam.length;a&&e<g;e++){var p=b.async.autoParam[e].split("="),n=p;p.length>1&&(n=p[1],p=p[0]);l[n]=a[p]}if(j.isArray(b.async.otherParam))for(e=0,g=b.async.otherParam.length;e<g;e+=2)l[b.async.otherParam[e]]=b.async.otherParam[e+1];else for(var m in b.async.otherParam)l[m]=b.async.otherParam[m];var P=h.getRoot(b)._ver;q.ajax({contentType:b.async.contentType,cache:!1,type:b.async.type,url:j.apply(b.async.url,[b.treeId,a],b.async.url),data:b.async.contentType.indexOf("application/json")>
-1?JSON.stringify(l):l,dataType:b.async.dataType,success:function(e){if(P==h.getRoot(b)._ver){var g=[];try{g=!e||e.length==0?[]:typeof e=="string"?eval("("+e+")"):e}catch(l){g=e}if(a)a.isAjaxing=null,a.zAsync=!0;i.setNodeLineIcos(b,a);g&&g!==""?(g=j.apply(b.async.dataFilter,[b.treeId,a,g],g),i.addNodes(b,a,-1,g?j.clone(g):[],!!c)):i.addNodes(b,a,-1,[],!!c);b.treeObj.trigger(f.event.ASYNC_SUCCESS,[b.treeId,a,e]);j.apply(d)}},error:function(c,d,e){if(P==h.getRoot(b)._ver){if(a)a.isAjaxing=null;i.setNodeLineIcos(b,
a);b.treeObj.trigger(f.event.ASYNC_ERROR,[b.treeId,a,c,d,e])}}});return!0},cancelPreSelectedNode:function(b,a,c){var d=h.getRoot(b).curSelectedList,e,g;for(e=d.length-1;e>=0;e--)if(g=d[e],a===g||!a&&(!c||c!==g))if(k(g,f.id.A,b).removeClass(f.node.CURSELECTED),a){h.removeSelectedNode(b,a);break}else d.splice(e,1),b.treeObj.trigger(f.event.UNSELECTED,[b.treeId,g])},createNodeCallback:function(b){if(b.callback.onNodeCreated||b.view.addDiyDom)for(var a=h.getRoot(b);a.createdNodes.length>0;){var c=a.createdNodes.shift();
j.apply(b.view.addDiyDom,[b.treeId,c]);b.callback.onNodeCreated&&b.treeObj.trigger(f.event.NODECREATED,[b.treeId,c])}},createNodes:function(b,a,c,d,e){if(c&&c.length!=0){var g=h.getRoot(b),l=b.data.key.children,l=!d||d.open||!!k(d[l][0],b).get(0);g.createdNodes=[];var a=i.appendNodes(b,a,c,d,e,!0,l),j,n;d?(d=k(d,f.id.UL,b),d.get(0)&&(j=d)):j=b.treeObj;j&&(e>=0&&(n=j.children()[e]),e>=0&&n?q(n).before(a.join("")):j.append(a.join("")));i.createNodeCallback(b)}},destroy:function(b){b&&(h.initCache(b),
h.initRoot(b),m.unbindTree(b),m.unbindEvent(b),b.treeObj.empty(),delete r[b.treeId])},expandCollapseNode:function(b,a,c,d,e){var g=h.getRoot(b),l=b.data.key.children,p;if(a){if(g.expandTriggerFlag)p=e,e=function(){p&&p();a.open?b.treeObj.trigger(f.event.EXPAND,[b.treeId,a]):b.treeObj.trigger(f.event.COLLAPSE,[b.treeId,a])},g.expandTriggerFlag=!1;if(!a.open&&a.isParent&&(!k(a,f.id.UL,b).get(0)||a[l]&&a[l].length>0&&!k(a[l][0],b).get(0)))i.appendParentULDom(b,a),i.createNodeCallback(b);if(a.open==c)j.apply(e,
[]);else{var c=k(a,f.id.UL,b),g=k(a,f.id.SWITCH,b),n=k(a,f.id.ICON,b);a.isParent?(a.open=!a.open,a.iconOpen&&a.iconClose&&n.attr("style",i.makeNodeIcoStyle(b,a)),a.open?(i.replaceSwitchClass(a,g,f.folder.OPEN),i.replaceIcoClass(a,n,f.folder.OPEN),d==!1||b.view.expandSpeed==""?(c.show(),j.apply(e,[])):a[l]&&a[l].length>0?c.slideDown(b.view.expandSpeed,e):(c.show(),j.apply(e,[]))):(i.replaceSwitchClass(a,g,f.folder.CLOSE),i.replaceIcoClass(a,n,f.folder.CLOSE),d==!1||b.view.expandSpeed==""||!(a[l]&&
a[l].length>0)?(c.hide(),j.apply(e,[])):c.slideUp(b.view.expandSpeed,e))):j.apply(e,[])}}else j.apply(e,[])},expandCollapseParentNode:function(b,a,c,d,e){a&&(a.parentTId?(i.expandCollapseNode(b,a,c,d),a.parentTId&&i.expandCollapseParentNode(b,a.getParentNode(),c,d,e)):i.expandCollapseNode(b,a,c,d,e))},expandCollapseSonNode:function(b,a,c,d,e){var g=h.getRoot(b),f=b.data.key.children,g=a?a[f]:g[f],f=a?!1:d,j=h.getRoot(b).expandTriggerFlag;h.getRoot(b).expandTriggerFlag=!1;if(g)for(var k=0,m=g.length;k<
m;k++)g[k]&&i.expandCollapseSonNode(b,g[k],c,f);h.getRoot(b).expandTriggerFlag=j;i.expandCollapseNode(b,a,c,d,e)},isSelectedNode:function(b,a){if(!a)return!1;var c=h.getRoot(b).curSelectedList,d;for(d=c.length-1;d>=0;d--)if(a===c[d])return!0;return!1},makeDOMNodeIcon:function(b,a,c){var d=h.getNodeName(a,c),d=a.view.nameIsHTML?d:d.replace(/&/g,"&amp;").replace(/</g,"&lt;").replace(/>/g,"&gt;");b.push("<span id='",c.tId,f.id.ICON,"' title='' treeNode",f.id.ICON," class='",i.makeNodeIcoClass(a,c),"' style='",
i.makeNodeIcoStyle(a,c),"'></span><span id='",c.tId,f.id.SPAN,"' class='",f.className.NAME,"'>",d,"</span>")},makeDOMNodeLine:function(b,a,c){b.push("<span id='",c.tId,f.id.SWITCH,"' title='' class='",i.makeNodeLineClass(a,c),"' treeNode",f.id.SWITCH,"></span>")},makeDOMNodeMainAfter:function(b){b.push("</li>")},makeDOMNodeMainBefore:function(b,a,c){b.push("<li id='",c.tId,"' class='",f.className.LEVEL,c.level,"' tabindex='0' hidefocus='true' treenode>")},makeDOMNodeNameAfter:function(b){b.push("</a>")},
makeDOMNodeNameBefore:function(b,a,c){var d=h.getNodeTitle(a,c),e=i.makeNodeUrl(a,c),g=i.makeNodeFontCss(a,c),l=[],k;for(k in g)l.push(k,":",g[k],";");b.push("<a id='",c.tId,f.id.A,"' class='",f.className.LEVEL,c.level,"' treeNode",f.id.A,' onclick="',c.click||"",'" ',e!=null&&e.length>0?"href='"+e+"'":""," target='",i.makeNodeTarget(c),"' style='",l.join(""),"'");j.apply(a.view.showTitle,[a.treeId,c],a.view.showTitle)&&d&&b.push("title='",d.replace(/'/g,"&#39;").replace(/</g,"&lt;").replace(/>/g,
"&gt;"),"'");b.push(">")},makeNodeFontCss:function(b,a){var c=j.apply(b.view.fontCss,[b.treeId,a],b.view.fontCss);return c&&typeof c!="function"?c:{}},makeNodeIcoClass:function(b,a){var c=["ico"];a.isAjaxing||(c[0]=(a.iconSkin?a.iconSkin+"_":"")+c[0],a.isParent?c.push(a.open?f.folder.OPEN:f.folder.CLOSE):c.push(f.folder.DOCU));return f.className.BUTTON+" "+c.join("_")},makeNodeIcoStyle:function(b,a){var c=[];if(!a.isAjaxing){var d=a.isParent&&a.iconOpen&&a.iconClose?a.open?a.iconOpen:a.iconClose:
a[b.data.key.icon];d&&c.push("background:url(",d,") 0 0 no-repeat;");(b.view.showIcon==!1||!j.apply(b.view.showIcon,[b.treeId,a],!0))&&c.push("width:0px;height:0px;")}return c.join("")},makeNodeLineClass:function(b,a){var c=[];b.view.showLine?a.level==0&&a.isFirstNode&&a.isLastNode?c.push(f.line.ROOT):a.level==0&&a.isFirstNode?c.push(f.line.ROOTS):a.isLastNode?c.push(f.line.BOTTOM):c.push(f.line.CENTER):c.push(f.line.NOLINE);a.isParent?c.push(a.open?f.folder.OPEN:f.folder.CLOSE):c.push(f.folder.DOCU);
return i.makeNodeLineClassEx(a)+c.join("_")},makeNodeLineClassEx:function(b){return f.className.BUTTON+" "+f.className.LEVEL+b.level+" "+f.className.SWITCH+" "},makeNodeTarget:function(b){return b.target||"_blank"},makeNodeUrl:function(b,a){var c=b.data.key.url;return a[c]?a[c]:null},makeUlHtml:function(b,a,c,d){c.push("<ul id='",a.tId,f.id.UL,"' class='",f.className.LEVEL,a.level," ",i.makeUlLineClass(b,a),"' style='display:",a.open?"block":"none","'>");c.push(d);c.push("</ul>")},makeUlLineClass:function(b,
a){return b.view.showLine&&!a.isLastNode?f.line.LINE:""},removeChildNodes:function(b,a){if(a){var c=b.data.key.children,d=a[c];if(d){for(var e=0,g=d.length;e<g;e++)h.removeNodeCache(b,d[e]);h.removeSelectedNode(b);delete a[c];b.data.keep.parent?k(a,f.id.UL,b).empty():(a.isParent=!1,a.open=!1,c=k(a,f.id.SWITCH,b),d=k(a,f.id.ICON,b),i.replaceSwitchClass(a,c,f.folder.DOCU),i.replaceIcoClass(a,d,f.folder.DOCU),k(a,f.id.UL,b).remove())}}},scrollIntoView:function(b){if(b){if(!Element.prototype.scrollIntoViewIfNeeded)Element.prototype.scrollIntoViewIfNeeded=
function(a){function b(a,d,e,f){return{left:a,top:d,width:e,height:f,right:a+e,bottom:d+f,translate:function(g,h){return b(g+a,h+d,e,f)},relativeFromTo:function(h,i){var j=a,l=d,h=h.offsetParent,i=i.offsetParent;if(h===i)return g;for(;h;h=h.offsetParent)j+=h.offsetLeft+h.clientLeft,l+=h.offsetTop+h.clientTop;for(;i;i=i.offsetParent)j-=i.offsetLeft+i.clientLeft,l-=i.offsetTop+i.clientTop;return b(j,l,e,f)}}}for(var d,e=this,g=b(this.offsetLeft,this.offsetTop,this.offsetWidth,this.offsetHeight);j.isElement(d=
e.parentNode);){var f=d.offsetLeft+d.clientLeft,h=d.offsetTop+d.clientTop,g=g.relativeFromTo(e,d).translate(-f,-h);d.scrollLeft=!1===a||g.left<=d.scrollLeft+d.clientWidth&&d.scrollLeft<=g.right-d.clientWidth+d.clientWidth?Math.min(g.left,Math.max(g.right-d.clientWidth,d.scrollLeft)):(g.right-d.clientWidth+g.left)/2;d.scrollTop=!1===a||g.top<=d.scrollTop+d.clientHeight&&d.scrollTop<=g.bottom-d.clientHeight+d.clientHeight?Math.min(g.top,Math.max(g.bottom-d.clientHeight,d.scrollTop)):(g.bottom-d.clientHeight+
g.top)/2;g=g.translate(f-d.scrollLeft,h-d.scrollTop);e=d}};b.scrollIntoViewIfNeeded()}},setFirstNode:function(b,a){var c=b.data.key.children;if(a[c].length>0)a[c][0].isFirstNode=!0},setLastNode:function(b,a){var c=b.data.key.children,d=a[c].length;if(d>0)a[c][d-1].isLastNode=!0},removeNode:function(b,a){var c=h.getRoot(b),d=b.data.key.children,e=a.parentTId?a.getParentNode():c;a.isFirstNode=!1;a.isLastNode=!1;a.getPreNode=function(){return null};a.getNextNode=function(){return null};if(h.getNodeCache(b,
a.tId)){k(a,b).remove();h.removeNodeCache(b,a);h.removeSelectedNode(b,a);for(var g=0,j=e[d].length;g<j;g++)if(e[d][g].tId==a.tId){e[d].splice(g,1);break}i.setFirstNode(b,e);i.setLastNode(b,e);var p,g=e[d].length;if(!b.data.keep.parent&&g==0)e.isParent=!1,e.open=!1,g=k(e,f.id.UL,b),j=k(e,f.id.SWITCH,b),p=k(e,f.id.ICON,b),i.replaceSwitchClass(e,j,f.folder.DOCU),i.replaceIcoClass(e,p,f.folder.DOCU),g.css("display","none");else if(b.view.showLine&&g>0){var n=e[d][g-1],g=k(n,f.id.UL,b),j=k(n,f.id.SWITCH,
b);p=k(n,f.id.ICON,b);e==c?e[d].length==1?i.replaceSwitchClass(n,j,f.line.ROOT):(c=k(e[d][0],f.id.SWITCH,b),i.replaceSwitchClass(e[d][0],c,f.line.ROOTS),i.replaceSwitchClass(n,j,f.line.BOTTOM)):i.replaceSwitchClass(n,j,f.line.BOTTOM);g.removeClass(f.line.LINE)}}},replaceIcoClass:function(b,a,c){if(a&&!b.isAjaxing&&(b=a.attr("class"),b!=void 0)){b=b.split("_");switch(c){case f.folder.OPEN:case f.folder.CLOSE:case f.folder.DOCU:b[b.length-1]=c}a.attr("class",b.join("_"))}},replaceSwitchClass:function(b,
a,c){if(a){var d=a.attr("class");if(d!=void 0){d=d.split("_");switch(c){case f.line.ROOT:case f.line.ROOTS:case f.line.CENTER:case f.line.BOTTOM:case f.line.NOLINE:d[0]=i.makeNodeLineClassEx(b)+c;break;case f.folder.OPEN:case f.folder.CLOSE:case f.folder.DOCU:d[1]=c}a.attr("class",d.join("_"));c!==f.folder.DOCU?a.removeAttr("disabled"):a.attr("disabled","disabled")}}},selectNode:function(b,a,c){c||i.cancelPreSelectedNode(b,null,a);k(a,f.id.A,b).addClass(f.node.CURSELECTED);h.addSelectedNode(b,a);
b.treeObj.trigger(f.event.SELECTED,[b.treeId,a])},setNodeFontCss:function(b,a){var c=k(a,f.id.A,b),d=i.makeNodeFontCss(b,a);d&&c.css(d)},setNodeLineIcos:function(b,a){if(a){var c=k(a,f.id.SWITCH,b),d=k(a,f.id.UL,b),e=k(a,f.id.ICON,b),g=i.makeUlLineClass(b,a);g.length==0?d.removeClass(f.line.LINE):d.addClass(g);c.attr("class",i.makeNodeLineClass(b,a));a.isParent?c.removeAttr("disabled"):c.attr("disabled","disabled");e.removeAttr("style");e.attr("style",i.makeNodeIcoStyle(b,a));e.attr("class",i.makeNodeIcoClass(b,
a))}},setNodeName:function(b,a){var c=h.getNodeTitle(b,a),d=k(a,f.id.SPAN,b);d.empty();b.view.nameIsHTML?d.html(h.getNodeName(b,a)):d.text(h.getNodeName(b,a));j.apply(b.view.showTitle,[b.treeId,a],b.view.showTitle)&&k(a,f.id.A,b).attr("title",!c?"":c)},setNodeTarget:function(b,a){k(a,f.id.A,b).attr("target",i.makeNodeTarget(a))},setNodeUrl:function(b,a){var c=k(a,f.id.A,b),d=i.makeNodeUrl(b,a);d==null||d.length==0?c.removeAttr("href"):c.attr("href",d)},switchNode:function(b,a){a.open||!j.canAsync(b,
a)?i.expandCollapseNode(b,a,!a.open):b.async.enable?i.asyncNode(b,a)||i.expandCollapseNode(b,a,!a.open):a&&i.expandCollapseNode(b,a,!a.open)}};q.fn.zTree={consts:{className:{BUTTON:"button",LEVEL:"level",ICO_LOADING:"ico_loading",SWITCH:"switch",NAME:"node_name"},event:{NODECREATED:"ztree_nodeCreated",CLICK:"ztree_click",EXPAND:"ztree_expand",COLLAPSE:"ztree_collapse",ASYNC_SUCCESS:"ztree_async_success",ASYNC_ERROR:"ztree_async_error",REMOVE:"ztree_remove",SELECTED:"ztree_selected",UNSELECTED:"ztree_unselected"},
id:{A:"_a",ICON:"_ico",SPAN:"_span",SWITCH:"_switch",UL:"_ul"},line:{ROOT:"root",ROOTS:"roots",CENTER:"center",BOTTOM:"bottom",NOLINE:"noline",LINE:"line"},folder:{OPEN:"open",CLOSE:"close",DOCU:"docu"},node:{CURSELECTED:"curSelectedNode"}},_z:{tools:j,view:i,event:m,data:h},getZTreeObj:function(b){return(b=h.getZTreeTools(b))?b:null},destroy:function(b){if(b&&b.length>0)i.destroy(h.getSetting(b));else for(var a in r)i.destroy(r[a])},init:function(b,a,c){var d=j.clone(N);q.extend(!0,d,a);d.treeId=
b.attr("id");d.treeObj=b;d.treeObj.empty();r[d.treeId]=d;if(typeof document.body.style.maxHeight==="undefined")d.view.expandSpeed="";h.initRoot(d);b=h.getRoot(d);a=d.data.key.children;c=c?j.clone(j.isArray(c)?c:[c]):[];b[a]=d.data.simpleData.enable?h.transformTozTreeFormat(d,c):c;h.initCache(d);m.unbindTree(d);m.bindTree(d);m.unbindEvent(d);m.bindEvent(d);c={setting:d,addNodes:function(a,b,c,f){function h(){i.addNodes(d,a,b,m,f==!0)}a||(a=null);if(a&&!a.isParent&&d.data.keep.leaf)return null;var k=
parseInt(b,10);isNaN(k)?(f=!!c,c=b,b=-1):b=k;if(!c)return null;var m=j.clone(j.isArray(c)?c:[c]);j.canAsync(d,a)?i.asyncNode(d,a,f,h):h();return m},cancelSelectedNode:function(a){i.cancelPreSelectedNode(d,a)},destroy:function(){i.destroy(d)},expandAll:function(a){a=!!a;i.expandCollapseSonNode(d,null,a,!0);return a},expandNode:function(a,b,c,f,n){function m(){var b=k(a,d).get(0);b&&f!==!1&&i.scrollIntoView(b)}if(!a||!a.isParent)return null;b!==!0&&b!==!1&&(b=!a.open);if((n=!!n)&&b&&j.apply(d.callback.beforeExpand,
[d.treeId,a],!0)==!1)return null;else if(n&&!b&&j.apply(d.callback.beforeCollapse,[d.treeId,a],!0)==!1)return null;b&&a.parentTId&&i.expandCollapseParentNode(d,a.getParentNode(),b,!1);if(b===a.open&&!c)return null;h.getRoot(d).expandTriggerFlag=n;!j.canAsync(d,a)&&c?i.expandCollapseSonNode(d,a,b,!0,m):(a.open=!b,i.switchNode(this.setting,a),m());return b},getNodes:function(){return h.getNodes(d)},getNodeByParam:function(a,b,c){return!a?null:h.getNodeByParam(d,c?c[d.data.key.children]:h.getNodes(d),
a,b)},getNodeByTId:function(a){return h.getNodeCache(d,a)},getNodesByParam:function(a,b,c){return!a?null:h.getNodesByParam(d,c?c[d.data.key.children]:h.getNodes(d),a,b)},getNodesByParamFuzzy:function(a,b,c){return!a?null:h.getNodesByParamFuzzy(d,c?c[d.data.key.children]:h.getNodes(d),a,b)},getNodesByFilter:function(a,b,c,f){b=!!b;return!a||typeof a!="function"?b?null:[]:h.getNodesByFilter(d,c?c[d.data.key.children]:h.getNodes(d),a,b,f)},getNodeIndex:function(a){if(!a)return null;for(var b=d.data.key.children,
c=a.parentTId?a.getParentNode():h.getRoot(d),f=0,i=c[b].length;f<i;f++)if(c[b][f]==a)return f;return-1},getSelectedNodes:function(){for(var a=[],b=h.getRoot(d).curSelectedList,c=0,f=b.length;c<f;c++)a.push(b[c]);return a},isSelectedNode:function(a){return h.isSelectedNode(d,a)},reAsyncChildNodes:function(a,b,c){if(this.setting.async.enable){var j=!a;j&&(a=h.getRoot(d));if(b=="refresh"){for(var b=this.setting.data.key.children,n=0,m=a[b]?a[b].length:0;n<m;n++)h.removeNodeCache(d,a[b][n]);h.removeSelectedNode(d);
a[b]=[];j?this.setting.treeObj.empty():k(a,f.id.UL,d).empty()}i.asyncNode(this.setting,j?null:a,!!c)}},refresh:function(){this.setting.treeObj.empty();var a=h.getRoot(d),b=a[d.data.key.children];h.initRoot(d);a[d.data.key.children]=b;h.initCache(d);i.createNodes(d,0,a[d.data.key.children],null,-1)},removeChildNodes:function(a){if(!a)return null;var b=a[d.data.key.children];i.removeChildNodes(d,a);return b?b:null},removeNode:function(a,b){a&&(b=!!b,b&&j.apply(d.callback.beforeRemove,[d.treeId,a],!0)==
!1||(i.removeNode(d,a),b&&this.setting.treeObj.trigger(f.event.REMOVE,[d.treeId,a])))},selectNode:function(a,b,c){function f(){if(!c){var b=k(a,d).get(0);i.scrollIntoView(b)}}if(a&&j.uCanDo(d)){b=d.view.selectedMulti&&b;if(a.parentTId)i.expandCollapseParentNode(d,a.getParentNode(),!0,!1,f);else if(!c)try{k(a,d).focus().blur()}catch(h){}i.selectNode(d,a,b)}},transformTozTreeNodes:function(a){return h.transformTozTreeFormat(d,a)},transformToArray:function(a){return h.transformToArrayFormat(d,a)},updateNode:function(a){a&&
k(a,d).get(0)&&j.uCanDo(d)&&(i.setNodeName(d,a),i.setNodeTarget(d,a),i.setNodeUrl(d,a),i.setNodeLineIcos(d,a),i.setNodeFontCss(d,a))}};b.treeTools=c;h.setZTreeTools(d,c);b[a]&&b[a].length>0?i.createNodes(d,0,b[a],null,-1):d.async.enable&&d.async.url&&d.async.url!==""&&i.asyncNode(d);return c}};var O=q.fn.zTree,k=j.$,f=O.consts})(jQuery);

/*
 * JQuery zTree excheck v3.5.28
 * http://treejs.cn/
 *
 * Copyright (c) 2010 Hunter.z
 *
 * Licensed same as jquery - MIT License
 * http://www.opensource.org/licenses/mit-license.php
 *
 * email: hunter.z@263.net
 * Date: 2017-01-20
 */
(function(m){var p,q,r,o={event:{CHECK:"ztree_check"},id:{CHECK:"_check"},checkbox:{STYLE:"checkbox",DEFAULT:"chk",DISABLED:"disable",FALSE:"false",TRUE:"true",FULL:"full",PART:"part",FOCUS:"focus"},radio:{STYLE:"radio",TYPE_ALL:"all",TYPE_LEVEL:"level"}},v={check:{enable:!1,autoCheckTrigger:!1,chkStyle:o.checkbox.STYLE,nocheckInherit:!1,chkDisabledInherit:!1,radioType:o.radio.TYPE_LEVEL,chkboxType:{Y:"ps",N:"ps"}},data:{key:{checked:"checked"}},callback:{beforeCheck:null,onCheck:null}};p=function(c,
a){if(a.chkDisabled===!0)return!1;var b=g.getSetting(c.data.treeId),d=b.data.key.checked;if(k.apply(b.callback.beforeCheck,[b.treeId,a],!0)==!1)return!0;a[d]=!a[d];e.checkNodeRelation(b,a);d=n(a,j.id.CHECK,b);e.setChkClass(b,d,a);e.repairParentChkClassWithSelf(b,a);b.treeObj.trigger(j.event.CHECK,[c,b.treeId,a]);return!0};q=function(c,a){if(a.chkDisabled===!0)return!1;var b=g.getSetting(c.data.treeId),d=n(a,j.id.CHECK,b);a.check_Focus=!0;e.setChkClass(b,d,a);return!0};r=function(c,a){if(a.chkDisabled===
!0)return!1;var b=g.getSetting(c.data.treeId),d=n(a,j.id.CHECK,b);a.check_Focus=!1;e.setChkClass(b,d,a);return!0};m.extend(!0,m.fn.zTree.consts,o);m.extend(!0,m.fn.zTree._z,{tools:{},view:{checkNodeRelation:function(c,a){var b,d,h,i=c.data.key.children,l=c.data.key.checked;b=j.radio;if(c.check.chkStyle==b.STYLE){var f=g.getRadioCheckedList(c);if(a[l])if(c.check.radioType==b.TYPE_ALL){for(d=f.length-1;d>=0;d--)b=f[d],b[l]&&b!=a&&(b[l]=!1,f.splice(d,1),e.setChkClass(c,n(b,j.id.CHECK,c),b),b.parentTId!=
a.parentTId&&e.repairParentChkClassWithSelf(c,b));f.push(a)}else{f=a.parentTId?a.getParentNode():g.getRoot(c);for(d=0,h=f[i].length;d<h;d++)b=f[i][d],b[l]&&b!=a&&(b[l]=!1,e.setChkClass(c,n(b,j.id.CHECK,c),b))}else if(c.check.radioType==b.TYPE_ALL)for(d=0,h=f.length;d<h;d++)if(a==f[d]){f.splice(d,1);break}}else a[l]&&(!a[i]||a[i].length==0||c.check.chkboxType.Y.indexOf("s")>-1)&&e.setSonNodeCheckBox(c,a,!0),!a[l]&&(!a[i]||a[i].length==0||c.check.chkboxType.N.indexOf("s")>-1)&&e.setSonNodeCheckBox(c,
a,!1),a[l]&&c.check.chkboxType.Y.indexOf("p")>-1&&e.setParentNodeCheckBox(c,a,!0),!a[l]&&c.check.chkboxType.N.indexOf("p")>-1&&e.setParentNodeCheckBox(c,a,!1)},makeChkClass:function(c,a){var b=c.data.key.checked,d=j.checkbox,h=j.radio,i="",i=a.chkDisabled===!0?d.DISABLED:a.halfCheck?d.PART:c.check.chkStyle==h.STYLE?a.check_Child_State<1?d.FULL:d.PART:a[b]?a.check_Child_State===2||a.check_Child_State===-1?d.FULL:d.PART:a.check_Child_State<1?d.FULL:d.PART,b=c.check.chkStyle+"_"+(a[b]?d.TRUE:d.FALSE)+
"_"+i,b=a.check_Focus&&a.chkDisabled!==!0?b+"_"+d.FOCUS:b;return j.className.BUTTON+" "+d.DEFAULT+" "+b},repairAllChk:function(c,a){if(c.check.enable&&c.check.chkStyle===j.checkbox.STYLE)for(var b=c.data.key.checked,d=c.data.key.children,h=g.getRoot(c),i=0,l=h[d].length;i<l;i++){var f=h[d][i];f.nocheck!==!0&&f.chkDisabled!==!0&&(f[b]=a);e.setSonNodeCheckBox(c,f,a)}},repairChkClass:function(c,a){if(a&&(g.makeChkFlag(c,a),a.nocheck!==!0)){var b=n(a,j.id.CHECK,c);e.setChkClass(c,b,a)}},repairParentChkClass:function(c,
a){if(a&&a.parentTId){var b=a.getParentNode();e.repairChkClass(c,b);e.repairParentChkClass(c,b)}},repairParentChkClassWithSelf:function(c,a){if(a){var b=c.data.key.children;a[b]&&a[b].length>0?e.repairParentChkClass(c,a[b][0]):e.repairParentChkClass(c,a)}},repairSonChkDisabled:function(c,a,b,d){if(a){var h=c.data.key.children;if(a.chkDisabled!=b)a.chkDisabled=b;e.repairChkClass(c,a);if(a[h]&&d)for(var i=0,l=a[h].length;i<l;i++)e.repairSonChkDisabled(c,a[h][i],b,d)}},repairParentChkDisabled:function(c,
a,b,d){if(a){if(a.chkDisabled!=b&&d)a.chkDisabled=b;e.repairChkClass(c,a);e.repairParentChkDisabled(c,a.getParentNode(),b,d)}},setChkClass:function(c,a,b){a&&(b.nocheck===!0?a.hide():a.show(),a.attr("class",e.makeChkClass(c,b)))},setParentNodeCheckBox:function(c,a,b,d){var h=c.data.key.children,i=c.data.key.checked,l=n(a,j.id.CHECK,c);d||(d=a);g.makeChkFlag(c,a);a.nocheck!==!0&&a.chkDisabled!==!0&&(a[i]=b,e.setChkClass(c,l,a),c.check.autoCheckTrigger&&a!=d&&c.treeObj.trigger(j.event.CHECK,[null,c.treeId,
a]));if(a.parentTId){l=!0;if(!b)for(var h=a.getParentNode()[h],f=0,k=h.length;f<k;f++)if(h[f].nocheck!==!0&&h[f].chkDisabled!==!0&&h[f][i]||(h[f].nocheck===!0||h[f].chkDisabled===!0)&&h[f].check_Child_State>0){l=!1;break}l&&e.setParentNodeCheckBox(c,a.getParentNode(),b,d)}},setSonNodeCheckBox:function(c,a,b,d){if(a){var h=c.data.key.children,i=c.data.key.checked,l=n(a,j.id.CHECK,c);d||(d=a);var f=!1;if(a[h])for(var k=0,m=a[h].length;k<m;k++){var o=a[h][k];e.setSonNodeCheckBox(c,o,b,d);o.chkDisabled===
!0&&(f=!0)}if(a!=g.getRoot(c)&&a.chkDisabled!==!0){f&&a.nocheck!==!0&&g.makeChkFlag(c,a);if(a.nocheck!==!0&&a.chkDisabled!==!0){if(a[i]=b,!f)a.check_Child_State=a[h]&&a[h].length>0?b?2:0:-1}else a.check_Child_State=-1;e.setChkClass(c,l,a);c.check.autoCheckTrigger&&a!=d&&a.nocheck!==!0&&a.chkDisabled!==!0&&c.treeObj.trigger(j.event.CHECK,[null,c.treeId,a])}}}},event:{},data:{getRadioCheckedList:function(c){for(var a=g.getRoot(c).radioCheckedList,b=0,d=a.length;b<d;b++)g.getNodeCache(c,a[b].tId)||(a.splice(b,
1),b--,d--);return a},getCheckStatus:function(c,a){if(!c.check.enable||a.nocheck||a.chkDisabled)return null;var b=c.data.key.checked;return{checked:a[b],half:a.halfCheck?a.halfCheck:c.check.chkStyle==j.radio.STYLE?a.check_Child_State===2:a[b]?a.check_Child_State>-1&&a.check_Child_State<2:a.check_Child_State>0}},getTreeCheckedNodes:function(c,a,b,d){if(!a)return[];for(var h=c.data.key.children,i=c.data.key.checked,e=b&&c.check.chkStyle==j.radio.STYLE&&c.check.radioType==j.radio.TYPE_ALL,d=!d?[]:d,
f=0,k=a.length;f<k;f++){if(a[f].nocheck!==!0&&a[f].chkDisabled!==!0&&a[f][i]==b&&(d.push(a[f]),e))break;g.getTreeCheckedNodes(c,a[f][h],b,d);if(e&&d.length>0)break}return d},getTreeChangeCheckedNodes:function(c,a,b){if(!a)return[];for(var d=c.data.key.children,h=c.data.key.checked,b=!b?[]:b,i=0,e=a.length;i<e;i++)a[i].nocheck!==!0&&a[i].chkDisabled!==!0&&a[i][h]!=a[i].checkedOld&&b.push(a[i]),g.getTreeChangeCheckedNodes(c,a[i][d],b);return b},makeChkFlag:function(c,a){if(a){var b=c.data.key.children,
d=c.data.key.checked,h=-1;if(a[b])for(var i=0,e=a[b].length;i<e;i++){var f=a[b][i],g=-1;if(c.check.chkStyle==j.radio.STYLE)if(g=f.nocheck===!0||f.chkDisabled===!0?f.check_Child_State:f.halfCheck===!0?2:f[d]?2:f.check_Child_State>0?2:0,g==2){h=2;break}else g==0&&(h=0);else if(c.check.chkStyle==j.checkbox.STYLE)if(g=f.nocheck===!0||f.chkDisabled===!0?f.check_Child_State:f.halfCheck===!0?1:f[d]?f.check_Child_State===-1||f.check_Child_State===2?2:1:f.check_Child_State>0?1:0,g===1){h=1;break}else if(g===
2&&h>-1&&i>0&&g!==h){h=1;break}else if(h===2&&g>-1&&g<2){h=1;break}else g>-1&&(h=g)}a.check_Child_State=h}}}});var m=m.fn.zTree,k=m._z.tools,j=m.consts,e=m._z.view,g=m._z.data,n=k.$;g.exSetting(v);g.addInitBind(function(c){c.treeObj.bind(j.event.CHECK,function(a,b,d,h){a.srcEvent=b;k.apply(c.callback.onCheck,[a,d,h])})});g.addInitUnBind(function(c){c.treeObj.unbind(j.event.CHECK)});g.addInitCache(function(){});g.addInitNode(function(c,a,b,d){if(b){a=c.data.key.checked;typeof b[a]=="string"&&(b[a]=
k.eqs(b[a],"true"));b[a]=!!b[a];b.checkedOld=b[a];if(typeof b.nocheck=="string")b.nocheck=k.eqs(b.nocheck,"true");b.nocheck=!!b.nocheck||c.check.nocheckInherit&&d&&!!d.nocheck;if(typeof b.chkDisabled=="string")b.chkDisabled=k.eqs(b.chkDisabled,"true");b.chkDisabled=!!b.chkDisabled||c.check.chkDisabledInherit&&d&&!!d.chkDisabled;if(typeof b.halfCheck=="string")b.halfCheck=k.eqs(b.halfCheck,"true");b.halfCheck=!!b.halfCheck;b.check_Child_State=-1;b.check_Focus=!1;b.getCheckStatus=function(){return g.getCheckStatus(c,
b)};c.check.chkStyle==j.radio.STYLE&&c.check.radioType==j.radio.TYPE_ALL&&b[a]&&g.getRoot(c).radioCheckedList.push(b)}});g.addInitProxy(function(c){var a=c.target,b=g.getSetting(c.data.treeId),d="",h=null,e="",l=null;if(k.eqs(c.type,"mouseover")){if(b.check.enable&&k.eqs(a.tagName,"span")&&a.getAttribute("treeNode"+j.id.CHECK)!==null)d=k.getNodeMainDom(a).id,e="mouseoverCheck"}else if(k.eqs(c.type,"mouseout")){if(b.check.enable&&k.eqs(a.tagName,"span")&&a.getAttribute("treeNode"+j.id.CHECK)!==null)d=
k.getNodeMainDom(a).id,e="mouseoutCheck"}else if(k.eqs(c.type,"click")&&b.check.enable&&k.eqs(a.tagName,"span")&&a.getAttribute("treeNode"+j.id.CHECK)!==null)d=k.getNodeMainDom(a).id,e="checkNode";if(d.length>0)switch(h=g.getNodeCache(b,d),e){case "checkNode":l=p;break;case "mouseoverCheck":l=q;break;case "mouseoutCheck":l=r}return{stop:e==="checkNode",node:h,nodeEventType:e,nodeEventCallback:l,treeEventType:"",treeEventCallback:null}},!0);g.addInitRoot(function(c){g.getRoot(c).radioCheckedList=[]});
g.addBeforeA(function(c,a,b){c.check.enable&&(g.makeChkFlag(c,a),b.push("<span ID='",a.tId,j.id.CHECK,"' class='",e.makeChkClass(c,a),"' treeNode",j.id.CHECK,a.nocheck===!0?" style='display:none;'":"","></span>"))});g.addZTreeTools(function(c,a){a.checkNode=function(a,b,c,g){var f=this.setting.data.key.checked;if(a.chkDisabled!==!0&&(b!==!0&&b!==!1&&(b=!a[f]),g=!!g,(a[f]!==b||c)&&!(g&&k.apply(this.setting.callback.beforeCheck,[this.setting.treeId,a],!0)==!1)&&k.uCanDo(this.setting)&&this.setting.check.enable&&
a.nocheck!==!0))a[f]=b,b=n(a,j.id.CHECK,this.setting),(c||this.setting.check.chkStyle===j.radio.STYLE)&&e.checkNodeRelation(this.setting,a),e.setChkClass(this.setting,b,a),e.repairParentChkClassWithSelf(this.setting,a),g&&this.setting.treeObj.trigger(j.event.CHECK,[null,this.setting.treeId,a])};a.checkAllNodes=function(a){e.repairAllChk(this.setting,!!a)};a.getCheckedNodes=function(a){var b=this.setting.data.key.children;return g.getTreeCheckedNodes(this.setting,g.getRoot(this.setting)[b],a!==!1)};
a.getChangeCheckedNodes=function(){var a=this.setting.data.key.children;return g.getTreeChangeCheckedNodes(this.setting,g.getRoot(this.setting)[a])};a.setChkDisabled=function(a,b,c,g){b=!!b;c=!!c;e.repairSonChkDisabled(this.setting,a,b,!!g);e.repairParentChkDisabled(this.setting,a.getParentNode(),b,c)};var b=a.updateNode;a.updateNode=function(c,g){b&&b.apply(a,arguments);if(c&&this.setting.check.enable&&n(c,this.setting).get(0)&&k.uCanDo(this.setting)){var i=n(c,j.id.CHECK,this.setting);(g==!0||this.setting.check.chkStyle===
j.radio.STYLE)&&e.checkNodeRelation(this.setting,c);e.setChkClass(this.setting,i,c);e.repairParentChkClassWithSelf(this.setting,c)}}});var s=e.createNodes;e.createNodes=function(c,a,b,d,g){s&&s.apply(e,arguments);b&&e.repairParentChkClassWithSelf(c,d)};var t=e.removeNode;e.removeNode=function(c,a){var b=a.getParentNode();t&&t.apply(e,arguments);a&&b&&(e.repairChkClass(c,b),e.repairParentChkClass(c,b))};var u=e.appendNodes;e.appendNodes=function(c,a,b,d,h,i,j){var f="";u&&(f=u.apply(e,arguments));
d&&g.makeChkFlag(c,d);return f}})(jQuery);

/*
 * JQuery zTree exedit v3.5.28
 * http://treejs.cn/
 *
 * Copyright (c) 2010 Hunter.z
 *
 * Licensed same as jquery - MIT License
 * http://www.opensource.org/licenses/mit-license.php
 *
 * email: hunter.z@263.net
 * Date: 2017-01-20
 */
(function(v){var J={event:{DRAG:"ztree_drag",DROP:"ztree_drop",RENAME:"ztree_rename",DRAGMOVE:"ztree_dragmove"},id:{EDIT:"_edit",INPUT:"_input",REMOVE:"_remove"},move:{TYPE_INNER:"inner",TYPE_PREV:"prev",TYPE_NEXT:"next"},node:{CURSELECTED_EDIT:"curSelectedNode_Edit",TMPTARGET_TREE:"tmpTargetzTree",TMPTARGET_NODE:"tmpTargetNode"}},x={onHoverOverNode:function(b,a){var c=m.getSetting(b.data.treeId),d=m.getRoot(c);if(d.curHoverNode!=a)x.onHoverOutNode(b);d.curHoverNode=a;f.addHoverDom(c,a)},onHoverOutNode:function(b){var b=
m.getSetting(b.data.treeId),a=m.getRoot(b);if(a.curHoverNode&&!m.isSelectedNode(b,a.curHoverNode))f.removeTreeDom(b,a.curHoverNode),a.curHoverNode=null},onMousedownNode:function(b,a){function c(b){if(B.dragFlag==0&&Math.abs(O-b.clientX)<e.edit.drag.minMoveSize&&Math.abs(P-b.clientY)<e.edit.drag.minMoveSize)return!0;var a,c,n,k,i;i=e.data.key.children;M.css("cursor","pointer");if(B.dragFlag==0){if(g.apply(e.callback.beforeDrag,[e.treeId,l],!0)==!1)return r(b),!0;for(a=0,c=l.length;a<c;a++){if(a==0)B.dragNodeShowBefore=
[];n=l[a];n.isParent&&n.open?(f.expandCollapseNode(e,n,!n.open),B.dragNodeShowBefore[n.tId]=!0):B.dragNodeShowBefore[n.tId]=!1}B.dragFlag=1;t.showHoverDom=!1;g.showIfameMask(e,!0);n=!0;k=-1;if(l.length>1){var j=l[0].parentTId?l[0].getParentNode()[i]:m.getNodes(e);i=[];for(a=0,c=j.length;a<c;a++)if(B.dragNodeShowBefore[j[a].tId]!==void 0&&(n&&k>-1&&k+1!==a&&(n=!1),i.push(j[a]),k=a),l.length===i.length){l=i;break}}n&&(I=l[0].getPreNode(),R=l[l.length-1].getNextNode());D=o("<ul class='zTreeDragUL'></ul>",
e);for(a=0,c=l.length;a<c;a++)n=l[a],n.editNameFlag=!1,f.selectNode(e,n,a>0),f.removeTreeDom(e,n),a>e.edit.drag.maxShowNodeNum-1||(k=o("<li id='"+n.tId+"_tmp'></li>",e),k.append(o(n,d.id.A,e).clone()),k.css("padding","0"),k.children("#"+n.tId+d.id.A).removeClass(d.node.CURSELECTED),D.append(k),a==e.edit.drag.maxShowNodeNum-1&&(k=o("<li id='"+n.tId+"_moretmp'><a>  ...  </a></li>",e),D.append(k)));D.attr("id",l[0].tId+d.id.UL+"_tmp");D.addClass(e.treeObj.attr("class"));D.appendTo(M);A=o("<span class='tmpzTreeMove_arrow'></span>",
e);A.attr("id","zTreeMove_arrow_tmp");A.appendTo(M);e.treeObj.trigger(d.event.DRAG,[b,e.treeId,l])}if(B.dragFlag==1){s&&A.attr("id")==b.target.id&&u&&b.clientX+G.scrollLeft()+2>v("#"+u+d.id.A,s).offset().left?(n=v("#"+u+d.id.A,s),b.target=n.length>0?n.get(0):b.target):s&&(s.removeClass(d.node.TMPTARGET_TREE),u&&v("#"+u+d.id.A,s).removeClass(d.node.TMPTARGET_NODE+"_"+d.move.TYPE_PREV).removeClass(d.node.TMPTARGET_NODE+"_"+J.move.TYPE_NEXT).removeClass(d.node.TMPTARGET_NODE+"_"+J.move.TYPE_INNER));
u=s=null;K=!1;h=e;n=m.getSettings();for(var y in n)if(n[y].treeId&&n[y].edit.enable&&n[y].treeId!=e.treeId&&(b.target.id==n[y].treeId||v(b.target).parents("#"+n[y].treeId).length>0))K=!0,h=n[y];y=G.scrollTop();k=G.scrollLeft();i=h.treeObj.offset();a=h.treeObj.get(0).scrollHeight;n=h.treeObj.get(0).scrollWidth;c=b.clientY+y-i.top;var p=h.treeObj.height()+i.top-b.clientY-y,q=b.clientX+k-i.left,H=h.treeObj.width()+i.left-b.clientX-k;i=c<e.edit.drag.borderMax&&c>e.edit.drag.borderMin;var j=p<e.edit.drag.borderMax&&
p>e.edit.drag.borderMin,F=q<e.edit.drag.borderMax&&q>e.edit.drag.borderMin,x=H<e.edit.drag.borderMax&&H>e.edit.drag.borderMin,p=c>e.edit.drag.borderMin&&p>e.edit.drag.borderMin&&q>e.edit.drag.borderMin&&H>e.edit.drag.borderMin,q=i&&h.treeObj.scrollTop()<=0,H=j&&h.treeObj.scrollTop()+h.treeObj.height()+10>=a,N=F&&h.treeObj.scrollLeft()<=0,Q=x&&h.treeObj.scrollLeft()+h.treeObj.width()+10>=n;if(b.target&&g.isChildOrSelf(b.target,h.treeId)){for(var E=b.target;E&&E.tagName&&!g.eqs(E.tagName,"li")&&E.id!=
h.treeId;)E=E.parentNode;var S=!0;for(a=0,c=l.length;a<c;a++)if(n=l[a],E.id===n.tId){S=!1;break}else if(o(n,e).find("#"+E.id).length>0){S=!1;break}if(S&&b.target&&g.isChildOrSelf(b.target,E.id+d.id.A))s=v(E),u=E.id}n=l[0];if(p&&g.isChildOrSelf(b.target,h.treeId)){if(!s&&(b.target.id==h.treeId||q||H||N||Q)&&(K||!K&&n.parentTId))s=h.treeObj;i?h.treeObj.scrollTop(h.treeObj.scrollTop()-10):j&&h.treeObj.scrollTop(h.treeObj.scrollTop()+10);F?h.treeObj.scrollLeft(h.treeObj.scrollLeft()-10):x&&h.treeObj.scrollLeft(h.treeObj.scrollLeft()+
10);s&&s!=h.treeObj&&s.offset().left<h.treeObj.offset().left&&h.treeObj.scrollLeft(h.treeObj.scrollLeft()+s.offset().left-h.treeObj.offset().left)}D.css({top:b.clientY+y+3+"px",left:b.clientX+k+3+"px"});c=a=0;if(s&&s.attr("id")!=h.treeId){var z=u==null?null:m.getNodeCache(h,u);i=(b.ctrlKey||b.metaKey)&&e.edit.drag.isMove&&e.edit.drag.isCopy||!e.edit.drag.isMove&&e.edit.drag.isCopy;k=!!(I&&u===I.tId);F=!!(R&&u===R.tId);j=n.parentTId&&n.parentTId==u;n=(i||!F)&&g.apply(h.edit.drag.prev,[h.treeId,l,z],
!!h.edit.drag.prev);k=(i||!k)&&g.apply(h.edit.drag.next,[h.treeId,l,z],!!h.edit.drag.next);i=(i||!j)&&!(h.data.keep.leaf&&!z.isParent)&&g.apply(h.edit.drag.inner,[h.treeId,l,z],!!h.edit.drag.inner);j=function(){s=null;u="";w=d.move.TYPE_INNER;A.css({display:"none"});if(window.zTreeMoveTimer)clearTimeout(window.zTreeMoveTimer),window.zTreeMoveTargetNodeTId=null};if(!n&&!k&&!i)j();else if(F=v("#"+u+d.id.A,s),x=z.isLastNode?null:v("#"+z.getNextNode().tId+d.id.A,s.next()),p=F.offset().top,q=F.offset().left,
H=n?i?0.25:k?0.5:1:-1,N=k?i?0.75:n?0.5:0:-1,y=(b.clientY+y-p)/F.height(),(H==1||y<=H&&y>=-0.2)&&n?(a=1-A.width(),c=p-A.height()/2,w=d.move.TYPE_PREV):(N==0||y>=N&&y<=1.2)&&k?(a=1-A.width(),c=x==null||z.isParent&&z.open?p+F.height()-A.height()/2:x.offset().top-A.height()/2,w=d.move.TYPE_NEXT):i?(a=5-A.width(),c=p,w=d.move.TYPE_INNER):j(),s){A.css({display:"block",top:c+"px",left:q+a+"px"});F.addClass(d.node.TMPTARGET_NODE+"_"+w);if(T!=u||U!=w)L=(new Date).getTime();if(z&&z.isParent&&w==d.move.TYPE_INNER&&
(y=!0,window.zTreeMoveTimer&&window.zTreeMoveTargetNodeTId!==z.tId?(clearTimeout(window.zTreeMoveTimer),window.zTreeMoveTargetNodeTId=null):window.zTreeMoveTimer&&window.zTreeMoveTargetNodeTId===z.tId&&(y=!1),y))window.zTreeMoveTimer=setTimeout(function(){w==d.move.TYPE_INNER&&z&&z.isParent&&!z.open&&(new Date).getTime()-L>h.edit.drag.autoOpenTime&&g.apply(h.callback.beforeDragOpen,[h.treeId,z],!0)&&(f.switchNode(h,z),h.edit.drag.autoExpandTrigger&&h.treeObj.trigger(d.event.EXPAND,[h.treeId,z]))},
h.edit.drag.autoOpenTime+50),window.zTreeMoveTargetNodeTId=z.tId}}else if(w=d.move.TYPE_INNER,s&&g.apply(h.edit.drag.inner,[h.treeId,l,null],!!h.edit.drag.inner)?s.addClass(d.node.TMPTARGET_TREE):s=null,A.css({display:"none"}),window.zTreeMoveTimer)clearTimeout(window.zTreeMoveTimer),window.zTreeMoveTargetNodeTId=null;T=u;U=w;e.treeObj.trigger(d.event.DRAGMOVE,[b,e.treeId,l])}return!1}function r(b){if(window.zTreeMoveTimer)clearTimeout(window.zTreeMoveTimer),window.zTreeMoveTargetNodeTId=null;U=T=
null;G.unbind("mousemove",c);G.unbind("mouseup",r);G.unbind("selectstart",k);M.css("cursor","auto");s&&(s.removeClass(d.node.TMPTARGET_TREE),u&&v("#"+u+d.id.A,s).removeClass(d.node.TMPTARGET_NODE+"_"+d.move.TYPE_PREV).removeClass(d.node.TMPTARGET_NODE+"_"+J.move.TYPE_NEXT).removeClass(d.node.TMPTARGET_NODE+"_"+J.move.TYPE_INNER));g.showIfameMask(e,!1);t.showHoverDom=!0;if(B.dragFlag!=0){B.dragFlag=0;var a,i,j;for(a=0,i=l.length;a<i;a++)j=l[a],j.isParent&&B.dragNodeShowBefore[j.tId]&&!j.open&&(f.expandCollapseNode(e,
j,!j.open),delete B.dragNodeShowBefore[j.tId]);D&&D.remove();A&&A.remove();var p=(b.ctrlKey||b.metaKey)&&e.edit.drag.isMove&&e.edit.drag.isCopy||!e.edit.drag.isMove&&e.edit.drag.isCopy;!p&&s&&u&&l[0].parentTId&&u==l[0].parentTId&&w==d.move.TYPE_INNER&&(s=null);if(s){var q=u==null?null:m.getNodeCache(h,u);if(g.apply(e.callback.beforeDrop,[h.treeId,l,q,w,p],!0)==!1)f.selectNodes(x,l);else{var C=p?g.clone(l):l;a=function(){if(K){if(!p)for(var a=0,c=l.length;a<c;a++)f.removeNode(e,l[a]);w==d.move.TYPE_INNER?
f.addNodes(h,q,-1,C):f.addNodes(h,q.getParentNode(),w==d.move.TYPE_PREV?q.getIndex():q.getIndex()+1,C)}else if(p&&w==d.move.TYPE_INNER)f.addNodes(h,q,-1,C);else if(p)f.addNodes(h,q.getParentNode(),w==d.move.TYPE_PREV?q.getIndex():q.getIndex()+1,C);else if(w!=d.move.TYPE_NEXT)for(a=0,c=C.length;a<c;a++)f.moveNode(h,q,C[a],w,!1);else for(a=-1,c=C.length-1;a<c;c--)f.moveNode(h,q,C[c],w,!1);f.selectNodes(h,C);a=o(C[0],e).get(0);f.scrollIntoView(a);e.treeObj.trigger(d.event.DROP,[b,h.treeId,C,q,w,p])};
w==d.move.TYPE_INNER&&g.canAsync(h,q)?f.asyncNode(h,q,!1,a):a()}}else f.selectNodes(x,l),e.treeObj.trigger(d.event.DROP,[b,e.treeId,l,null,null,null])}}function k(){return!1}var i,j,e=m.getSetting(b.data.treeId),B=m.getRoot(e),t=m.getRoots();if(b.button==2||!e.edit.enable||!e.edit.drag.isCopy&&!e.edit.drag.isMove)return!0;var p=b.target,q=m.getRoot(e).curSelectedList,l=[];if(m.isSelectedNode(e,a))for(i=0,j=q.length;i<j;i++){if(q[i].editNameFlag&&g.eqs(p.tagName,"input")&&p.getAttribute("treeNode"+
d.id.INPUT)!==null)return!0;l.push(q[i]);if(l[0].parentTId!==q[i].parentTId){l=[a];break}}else l=[a];f.editNodeBlur=!0;f.cancelCurEditNode(e);var G=v(e.treeObj.get(0).ownerDocument),M=v(e.treeObj.get(0).ownerDocument.body),D,A,s,K=!1,h=e,x=e,I,R,T=null,U=null,u=null,w=d.move.TYPE_INNER,O=b.clientX,P=b.clientY,L=(new Date).getTime();g.uCanDo(e)&&G.bind("mousemove",c);G.bind("mouseup",r);G.bind("selectstart",k);b.preventDefault&&b.preventDefault();return!0}};v.extend(!0,v.fn.zTree.consts,J);v.extend(!0,
v.fn.zTree._z,{tools:{getAbs:function(b){b=b.getBoundingClientRect();return[b.left+(document.body.scrollLeft+document.documentElement.scrollLeft),b.top+(document.body.scrollTop+document.documentElement.scrollTop)]},inputFocus:function(b){b.get(0)&&(b.focus(),g.setCursorPosition(b.get(0),b.val().length))},inputSelect:function(b){b.get(0)&&(b.focus(),b.select())},setCursorPosition:function(b,a){if(b.setSelectionRange)b.focus(),b.setSelectionRange(a,a);else if(b.createTextRange){var c=b.createTextRange();
c.collapse(!0);c.moveEnd("character",a);c.moveStart("character",a);c.select()}},showIfameMask:function(b,a){for(var c=m.getRoot(b);c.dragMaskList.length>0;)c.dragMaskList[0].remove(),c.dragMaskList.shift();if(a)for(var d=o("iframe",b),f=0,i=d.length;f<i;f++){var j=d.get(f),e=g.getAbs(j),j=o("<div id='zTreeMask_"+f+"' class='zTreeMask' style='top:"+e[1]+"px; left:"+e[0]+"px; width:"+j.offsetWidth+"px; height:"+j.offsetHeight+"px;'></div>",b);j.appendTo(o("body",b));c.dragMaskList.push(j)}}},view:{addEditBtn:function(b,
a){if(!(a.editNameFlag||o(a,d.id.EDIT,b).length>0)&&g.apply(b.edit.showRenameBtn,[b.treeId,a],b.edit.showRenameBtn)){var c=o(a,d.id.A,b),r="<span class='"+d.className.BUTTON+" edit' id='"+a.tId+d.id.EDIT+"' title='"+g.apply(b.edit.renameTitle,[b.treeId,a],b.edit.renameTitle)+"' treeNode"+d.id.EDIT+" style='display:none;'></span>";c.append(r);o(a,d.id.EDIT,b).bind("click",function(){if(!g.uCanDo(b)||g.apply(b.callback.beforeEditName,[b.treeId,a],!0)==!1)return!1;f.editNode(b,a);return!1}).show()}},
addRemoveBtn:function(b,a){if(!(a.editNameFlag||o(a,d.id.REMOVE,b).length>0)&&g.apply(b.edit.showRemoveBtn,[b.treeId,a],b.edit.showRemoveBtn)){var c=o(a,d.id.A,b),r="<span class='"+d.className.BUTTON+" remove' id='"+a.tId+d.id.REMOVE+"' title='"+g.apply(b.edit.removeTitle,[b.treeId,a],b.edit.removeTitle)+"' treeNode"+d.id.REMOVE+" style='display:none;'></span>";c.append(r);o(a,d.id.REMOVE,b).bind("click",function(){if(!g.uCanDo(b)||g.apply(b.callback.beforeRemove,[b.treeId,a],!0)==!1)return!1;f.removeNode(b,
a);b.treeObj.trigger(d.event.REMOVE,[b.treeId,a]);return!1}).bind("mousedown",function(){return!0}).show()}},addHoverDom:function(b,a){if(m.getRoots().showHoverDom)a.isHover=!0,b.edit.enable&&(f.addEditBtn(b,a),f.addRemoveBtn(b,a)),g.apply(b.view.addHoverDom,[b.treeId,a])},cancelCurEditNode:function(b,a,c){var r=m.getRoot(b),k=b.data.key.name,i=r.curEditNode;if(i){var j=r.curEditInput,a=a?a:c?i[k]:j.val();if(g.apply(b.callback.beforeRename,[b.treeId,i,a,c],!0)===!1)return!1;i[k]=a;o(i,d.id.A,b).removeClass(d.node.CURSELECTED_EDIT);
j.unbind();f.setNodeName(b,i);i.editNameFlag=!1;r.curEditNode=null;r.curEditInput=null;f.selectNode(b,i,!1);b.treeObj.trigger(d.event.RENAME,[b.treeId,i,c])}return r.noSelection=!0},editNode:function(b,a){var c=m.getRoot(b);f.editNodeBlur=!1;if(m.isSelectedNode(b,a)&&c.curEditNode==a&&a.editNameFlag)setTimeout(function(){g.inputFocus(c.curEditInput)},0);else{var r=b.data.key.name;a.editNameFlag=!0;f.removeTreeDom(b,a);f.cancelCurEditNode(b);f.selectNode(b,a,!1);o(a,d.id.SPAN,b).html("<input type=text class='rename' id='"+
a.tId+d.id.INPUT+"' treeNode"+d.id.INPUT+" >");var k=o(a,d.id.INPUT,b);k.attr("value",a[r]);b.edit.editNameSelectAll?g.inputSelect(k):g.inputFocus(k);k.bind("blur",function(){f.editNodeBlur||f.cancelCurEditNode(b)}).bind("keydown",function(a){a.keyCode=="13"?(f.editNodeBlur=!0,f.cancelCurEditNode(b)):a.keyCode=="27"&&f.cancelCurEditNode(b,null,!0)}).bind("click",function(){return!1}).bind("dblclick",function(){return!1});o(a,d.id.A,b).addClass(d.node.CURSELECTED_EDIT);c.curEditInput=k;c.noSelection=
!1;c.curEditNode=a}},moveNode:function(b,a,c,r,k,i){var j=m.getRoot(b),e=b.data.key.children;if(a!=c&&(!b.data.keep.leaf||!a||a.isParent||r!=d.move.TYPE_INNER)){var g=c.parentTId?c.getParentNode():j,t=a===null||a==j;t&&a===null&&(a=j);if(t)r=d.move.TYPE_INNER;j=a.parentTId?a.getParentNode():j;if(r!=d.move.TYPE_PREV&&r!=d.move.TYPE_NEXT)r=d.move.TYPE_INNER;if(r==d.move.TYPE_INNER)if(t)c.parentTId=null;else{if(!a.isParent)a.isParent=!0,a.open=!!a.open,f.setNodeLineIcos(b,a);c.parentTId=a.tId}var p;
t?p=t=b.treeObj:(!i&&r==d.move.TYPE_INNER?f.expandCollapseNode(b,a,!0,!1):i||f.expandCollapseNode(b,a.getParentNode(),!0,!1),t=o(a,b),p=o(a,d.id.UL,b),t.get(0)&&!p.get(0)&&(p=[],f.makeUlHtml(b,a,p,""),t.append(p.join(""))),p=o(a,d.id.UL,b));var q=o(c,b);q.get(0)?t.get(0)||q.remove():q=f.appendNodes(b,c.level,[c],null,-1,!1,!0).join("");p.get(0)&&r==d.move.TYPE_INNER?p.append(q):t.get(0)&&r==d.move.TYPE_PREV?t.before(q):t.get(0)&&r==d.move.TYPE_NEXT&&t.after(q);var l=-1,v=0,x=null,t=null,D=c.level;
if(c.isFirstNode){if(l=0,g[e].length>1)x=g[e][1],x.isFirstNode=!0}else if(c.isLastNode)l=g[e].length-1,x=g[e][l-1],x.isLastNode=!0;else for(p=0,q=g[e].length;p<q;p++)if(g[e][p].tId==c.tId){l=p;break}l>=0&&g[e].splice(l,1);if(r!=d.move.TYPE_INNER)for(p=0,q=j[e].length;p<q;p++)j[e][p].tId==a.tId&&(v=p);if(r==d.move.TYPE_INNER){a[e]||(a[e]=[]);if(a[e].length>0)t=a[e][a[e].length-1],t.isLastNode=!1;a[e].splice(a[e].length,0,c);c.isLastNode=!0;c.isFirstNode=a[e].length==1}else a.isFirstNode&&r==d.move.TYPE_PREV?
(j[e].splice(v,0,c),t=a,t.isFirstNode=!1,c.parentTId=a.parentTId,c.isFirstNode=!0,c.isLastNode=!1):a.isLastNode&&r==d.move.TYPE_NEXT?(j[e].splice(v+1,0,c),t=a,t.isLastNode=!1,c.parentTId=a.parentTId,c.isFirstNode=!1,c.isLastNode=!0):(r==d.move.TYPE_PREV?j[e].splice(v,0,c):j[e].splice(v+1,0,c),c.parentTId=a.parentTId,c.isFirstNode=!1,c.isLastNode=!1);m.fixPIdKeyValue(b,c);m.setSonNodeLevel(b,c.getParentNode(),c);f.setNodeLineIcos(b,c);f.repairNodeLevelClass(b,c,D);!b.data.keep.parent&&g[e].length<
1?(g.isParent=!1,g.open=!1,a=o(g,d.id.UL,b),r=o(g,d.id.SWITCH,b),e=o(g,d.id.ICON,b),f.replaceSwitchClass(g,r,d.folder.DOCU),f.replaceIcoClass(g,e,d.folder.DOCU),a.css("display","none")):x&&f.setNodeLineIcos(b,x);t&&f.setNodeLineIcos(b,t);b.check&&b.check.enable&&f.repairChkClass&&(f.repairChkClass(b,g),f.repairParentChkClassWithSelf(b,g),g!=c.parent&&f.repairParentChkClassWithSelf(b,c));i||f.expandCollapseParentNode(b,c.getParentNode(),!0,k)}},removeEditBtn:function(b,a){o(a,d.id.EDIT,b).unbind().remove()},
removeRemoveBtn:function(b,a){o(a,d.id.REMOVE,b).unbind().remove()},removeTreeDom:function(b,a){a.isHover=!1;f.removeEditBtn(b,a);f.removeRemoveBtn(b,a);g.apply(b.view.removeHoverDom,[b.treeId,a])},repairNodeLevelClass:function(b,a,c){if(c!==a.level){var f=o(a,b),g=o(a,d.id.A,b),b=o(a,d.id.UL,b),c=d.className.LEVEL+c,a=d.className.LEVEL+a.level;f.removeClass(c);f.addClass(a);g.removeClass(c);g.addClass(a);b.removeClass(c);b.addClass(a)}},selectNodes:function(b,a){for(var c=0,d=a.length;c<d;c++)f.selectNode(b,
a[c],c>0)}},event:{},data:{setSonNodeLevel:function(b,a,c){if(c){var d=b.data.key.children;c.level=a?a.level+1:0;if(c[d])for(var a=0,f=c[d].length;a<f;a++)c[d][a]&&m.setSonNodeLevel(b,c,c[d][a])}}}});var I=v.fn.zTree,g=I._z.tools,d=I.consts,f=I._z.view,m=I._z.data,o=g.$;m.exSetting({edit:{enable:!1,editNameSelectAll:!1,showRemoveBtn:!0,showRenameBtn:!0,removeTitle:"remove",renameTitle:"rename",drag:{autoExpandTrigger:!1,isCopy:!0,isMove:!0,prev:!0,next:!0,inner:!0,minMoveSize:5,borderMax:10,borderMin:-5,
maxShowNodeNum:5,autoOpenTime:500}},view:{addHoverDom:null,removeHoverDom:null},callback:{beforeDrag:null,beforeDragOpen:null,beforeDrop:null,beforeEditName:null,beforeRename:null,onDrag:null,onDragMove:null,onDrop:null,onRename:null}});m.addInitBind(function(b){var a=b.treeObj,c=d.event;a.bind(c.RENAME,function(a,c,d,f){g.apply(b.callback.onRename,[a,c,d,f])});a.bind(c.DRAG,function(a,c,d,f){g.apply(b.callback.onDrag,[c,d,f])});a.bind(c.DRAGMOVE,function(a,c,d,f){g.apply(b.callback.onDragMove,[c,
d,f])});a.bind(c.DROP,function(a,c,d,f,e,m,o){g.apply(b.callback.onDrop,[c,d,f,e,m,o])})});m.addInitUnBind(function(b){var b=b.treeObj,a=d.event;b.unbind(a.RENAME);b.unbind(a.DRAG);b.unbind(a.DRAGMOVE);b.unbind(a.DROP)});m.addInitCache(function(){});m.addInitNode(function(b,a,c){if(c)c.isHover=!1,c.editNameFlag=!1});m.addInitProxy(function(b){var a=b.target,c=m.getSetting(b.data.treeId),f=b.relatedTarget,k="",i=null,j="",e=null,o=null;if(g.eqs(b.type,"mouseover")){if(o=g.getMDom(c,a,[{tagName:"a",
attrName:"treeNode"+d.id.A}]))k=g.getNodeMainDom(o).id,j="hoverOverNode"}else if(g.eqs(b.type,"mouseout"))o=g.getMDom(c,f,[{tagName:"a",attrName:"treeNode"+d.id.A}]),o||(k="remove",j="hoverOutNode");else if(g.eqs(b.type,"mousedown")&&(o=g.getMDom(c,a,[{tagName:"a",attrName:"treeNode"+d.id.A}])))k=g.getNodeMainDom(o).id,j="mousedownNode";if(k.length>0)switch(i=m.getNodeCache(c,k),j){case "mousedownNode":e=x.onMousedownNode;break;case "hoverOverNode":e=x.onHoverOverNode;break;case "hoverOutNode":e=
x.onHoverOutNode}return{stop:!1,node:i,nodeEventType:j,nodeEventCallback:e,treeEventType:"",treeEventCallback:null}});m.addInitRoot(function(b){var b=m.getRoot(b),a=m.getRoots();b.curEditNode=null;b.curEditInput=null;b.curHoverNode=null;b.dragFlag=0;b.dragNodeShowBefore=[];b.dragMaskList=[];a.showHoverDom=!0});m.addZTreeTools(function(b,a){a.cancelEditName=function(a){m.getRoot(this.setting).curEditNode&&f.cancelCurEditNode(this.setting,a?a:null,!0)};a.copyNode=function(a,b,k,i){if(!b)return null;
if(a&&!a.isParent&&this.setting.data.keep.leaf&&k===d.move.TYPE_INNER)return null;var j=this,e=g.clone(b);if(!a)a=null,k=d.move.TYPE_INNER;k==d.move.TYPE_INNER?(b=function(){f.addNodes(j.setting,a,-1,[e],i)},g.canAsync(this.setting,a)?f.asyncNode(this.setting,a,i,b):b()):(f.addNodes(this.setting,a.parentNode,-1,[e],i),f.moveNode(this.setting,a,e,k,!1,i));return e};a.editName=function(a){a&&a.tId&&a===m.getNodeCache(this.setting,a.tId)&&(a.parentTId&&f.expandCollapseParentNode(this.setting,a.getParentNode(),
!0),f.editNode(this.setting,a))};a.moveNode=function(a,b,k,i){function j(){f.moveNode(e.setting,a,b,k,!1,i)}if(!b)return b;if(a&&!a.isParent&&this.setting.data.keep.leaf&&k===d.move.TYPE_INNER)return null;else if(a&&(b.parentTId==a.tId&&k==d.move.TYPE_INNER||o(b,this.setting).find("#"+a.tId).length>0))return null;else a||(a=null);var e=this;g.canAsync(this.setting,a)&&k===d.move.TYPE_INNER?f.asyncNode(this.setting,a,i,j):j();return b};a.setEditable=function(a){this.setting.edit.enable=a;return this.refresh()}});
var O=f.cancelPreSelectedNode;f.cancelPreSelectedNode=function(b,a){for(var c=m.getRoot(b).curSelectedList,d=0,g=c.length;d<g;d++)if(!a||a===c[d])if(f.removeTreeDom(b,c[d]),a)break;O&&O.apply(f,arguments)};var P=f.createNodes;f.createNodes=function(b,a,c,d,g){P&&P.apply(f,arguments);c&&f.repairParentChkClassWithSelf&&f.repairParentChkClassWithSelf(b,d)};var W=f.makeNodeUrl;f.makeNodeUrl=function(b,a){return b.edit.enable?null:W.apply(f,arguments)};var L=f.removeNode;f.removeNode=function(b,a){var c=
m.getRoot(b);if(c.curEditNode===a)c.curEditNode=null;L&&L.apply(f,arguments)};var Q=f.selectNode;f.selectNode=function(b,a,c){var d=m.getRoot(b);if(m.isSelectedNode(b,a)&&d.curEditNode==a&&a.editNameFlag)return!1;Q&&Q.apply(f,arguments);f.addHoverDom(b,a);return!0};var V=g.uCanDo;g.uCanDo=function(b,a){var c=m.getRoot(b);if(a&&(g.eqs(a.type,"mouseover")||g.eqs(a.type,"mouseout")||g.eqs(a.type,"mousedown")||g.eqs(a.type,"mouseup")))return!0;if(c.curEditNode)f.editNodeBlur=!1,c.curEditInput.focus();
return!c.curEditNode&&(V?V.apply(f,arguments):!0)}})(jQuery);


//暴露出来动态更新树形结构的方法
//updates
//getValue
//getText

;(function($, w) {

var defaults = {
    source: '', //string or array
    sourceRequestType: 'GET',
    defaults: '', // string or array
    disables: '', // string or array
    multiple: false,
    isSimple: false,
    resultItem: {
        text: 'text',
        value: 'id',
        children: 'children',

        //simple时设置
        idKey: 'id',
        pIdKey: 'pId',
        rootPId: null
    },
    edit: {},
    showTitle: true,
    onCheck: null,
    onRender: null,
    onBeforeSend: null,
    onGetData: null,
    beforeDrag: null,
    beforeDragOpen: null,
    beforeDrop: null,
    beforeEditName: null,
    beforeRemove: null,
    beforeRename: null,
    onDrag: null,
    onDragMove: null,
    onDrop: null,
    onRemove: null,
    onRename: null,
    addHoverDom: null,
    removeHoverDom: null
};


var et = {};
et.destroy = function($element) {
    var $eTree = $element.find('.eui-tree');
    var id = $eTree.attr('id');
    var zTreeObj = $.fn.zTree.getZTreeObj(id);

    zTreeObj.destroy();
    delete w.eTreeInst[id];
};

et.render = function(opts, $element) {
    var eTreeHtml = `<ul class="eui-tree ztree" id="eui-tree-${$element.attr('id') || ETree.index}"></ul>`;

    $element.html(eTreeHtml);
    var $eTree = $element.find('> .eui-tree');

    if ($.isArray(opts.source)) {
        //将renderTree变成异步操作，让eTree的实例对象先于render方法返回
        setTimeout(function() {
            et.renderTree(opts, $eTree, opts.source);
        }, 0);
    }
    else {
        et.getTreeData(opts, $eTree);
    }

    return $eTree;
};

et.getTreeData = function(opts, $eTree) {
    var ajaxSendData = {};
    //发送异步前的回调，必须要return ajaxSendData
    if ($.isFunction(opts.onBeforeSend)) {
        var cbThis = { $eTree: $eTree };
        ajaxSendData = opts.onBeforeSend.call(cbThis, ajaxSendData);
    }

    var xhr = $.ajax({
        url: opts.source,
        type: opts.sourceRequestType,
        data: ajaxSendData,
        dataType: 'json',
        timeout: 20000,
        beforeSend: function() {
        },
        success: function(data) {
            data = data || [];

            //获取数据后的回调，方便对数据进行格式处理
            //该回调必须显示return 一个数组类型的值
            if ($.isFunction(opts.onGetData)) {
                var cbThis = { $eTree: $eTree, xhr: xhr };
                data = opts.onGetData.call(cbThis, data);
            }

            //渲染树
            et.renderTree(opts, $eTree, data);
        },
        error: function(a) {
        }
    });    
};

et.renderTree = function(opts, $eTree, data) {
    var setting = {
        check: {
            enable: true,
            chkStyle: (opts.multiple ? 'checkbox' : 'radio'),
            chkDisabledInherit: true,
            radioType: 'all'
        },
        data: {
            key: {
                children: opts.resultItem.children,
                name: opts.resultItem.text
            },
            simpleData: {
                enable: (opts.isSimple ? true : false),
                idKey: opts.resultItem.idKey,
                pIdKey: opts.resultItem.pIdKey,
                rootPId: null
            }
        },
        edit: opts.edit,
        callback: {
            onCheck: function(event, treeId, treeNode) {
                var zTreeObj = $.fn.zTree.getZTreeObj(treeId);
                var checkedStuff = et.getCheckedStuff(opts, zTreeObj);

                if ($.isFunction(opts.onCheck)) {
                    var cbThis = { 
                        $eTree: $eTree,
                        zTreeObj: $.fn.zTree.getZTreeObj(treeId),
                        treeNode: treeNode, 
                        checkedNodes: checkedStuff.nodes, 
                        value: checkedStuff.value, 
                        text: checkedStuff.text 
                    };
                    opts.onCheck.call(cbThis, data);
                }
            },
            onClick: function(event, treeId, treeNode, clickFlag) {
                //单选时，由于隐藏了radio按钮，所以改成点击文字时会手动触发勾选
                if (!opts.multiple) {
                    var zTreeObj = $.fn.zTree.getZTreeObj(treeId);
                    zTreeObj.checkNode(treeNode, undefined, undefined, true);
                }
            },
            beforeDrag: opts.beforeDrag,
            beforeDragOpen: opts.beforeDragOpen,
            beforeDrop: opts.beforeDrop,
            beforeEditName: opts.beforeEditName,
            beforeRemove: opts.beforeRemove,
            beforeRename: opts.beforeRename,
            onDrag: opts.onDrag,
            onDragMove: opts.onDragMove,
            onDrop: opts.onDrop,
            onRemove: opts.onRemove,
            onRename: opts.onRename           
        },
        view: {
            nameIsHTML: true,
            showTitle: opts.showTitle,
            addHoverDom: opts.addHoverDom,
            removeHoverDom: opts.removeHoverDom
        }
    };    
    var zTreeObj = $.fn.zTree.init($eTree, setting, data);

    et.setDefaults(opts, zTreeObj);
    et.setDisables(opts, zTreeObj);

    var checkedStuff = et.getCheckedStuff(opts, zTreeObj);

    //
    if ($.isFunction(opts.onRender)) {
        var cbThis = { 
            $eTree: $eTree, 
            zTreeObj: zTreeObj,
            checkedNodes: checkedStuff.nodes, 
            value: checkedStuff.value, 
            text: checkedStuff.text
        };
        opts.onRender.call(cbThis);
    }
};

//获取选中的信息，返回：选中的节点，选中的value值，选中的text值
et.getCheckedStuff = function(opts, zTreeObj) {
    var FULL_CHECKED_STATE = 2;
    var HALF_CHECKED_STATE = 1;
    var checkedNodes = [];
    var value, text;


    //多选时获取选中的节点，要过滤掉半选中的节点
    if (opts.multiple) {
        checkedNodes = zTreeObj.getNodesByFilter(function(node) {
            return (node.checked === true || 
                    node.check_Child_State === HALF_CHECKED_STATE || 
                    node.check_Child_State === FULL_CHECKED_STATE);
        }).filter(function(node) {
            return node.parentTId == null;
        });
        var cachedFullCheckedNodes = [];
        et.getFullCheckedNodes(checkedNodes, cachedFullCheckedNodes, opts);
        checkedNodes = cachedFullCheckedNodes;
    }
    //单选时获取选中的节点
    else {
        checkedNodes = zTreeObj.getNodesByFilter(function(node) {
            return node.checked === true;
        });
    }


    //simple data时，获取value key的方式不一样
    var nodeValueKey;
    if (opts.isSimple) {
        nodeValueKey = opts.resultItem.idKey;
    }
    else {
        nodeValueKey = opts.resultItem.value;
    }
    value = checkedNodes.map(function(node) {
        return node[nodeValueKey];
    }).join();
    text = checkedNodes.map(function(node) {
        return node[opts.resultItem.text];
    }).join();

    return {
        nodes: checkedNodes,
        value: value,
        text: text
    };  
};

et.getFullCheckedNodes = function(checkedNodes, cachedFullCheckedNodes, opts) {
    var FULL_CHECKED_STATE = 2;
    var NO_CHILDS = -1;
    var HALF_CHECKED_STATE = 1;
    var NO_CHECKED_CHILDS = 0;

    for (var i = 0; i < checkedNodes.length; i++) {
        var parentNode = checkedNodes[i];

        var fullChecked_hasChilds = parentNode.check_Child_State == FULL_CHECKED_STATE;
        var fullChecked_noChilds = parentNode.check_Child_State == NO_CHILDS && parentNode.checked === true;
        var halfChecked = parentNode.check_Child_State == HALF_CHECKED_STATE || 
                (parentNode.check_Child_State == NO_CHECKED_CHILDS && parentNode.checked === true);

        if (fullChecked_hasChilds || fullChecked_noChilds) {
            cachedFullCheckedNodes.push(parentNode)
            continue;
        }
        else if (halfChecked) {
            et.getFullCheckedNodes(parentNode[opts.resultItem.children], cachedFullCheckedNodes, opts);
        }
    }
};

et.setDefaults = function(opts, zTreeObj) {
    var defaultValueList = (opts.defaults || '').split(',').filter(function(defaultValue) {
        return ('' + defaultValue).trim() !== '';
    });
    var nodeValueKey;

    //simple data时，获取value key的方式不一样    
    if (opts.isSimple) {
        nodeValueKey = opts.resultItem.idKey;
    }
    else {
        nodeValueKey = opts.resultItem.value;
    }

    zTreeObj.getNodesByFilter(function(node) {
        return defaultValueList.find(function(defaultValue) {
            return ('' + node[nodeValueKey]) === defaultValue;
        }) != null;
    }).forEach(function(node) {
        zTreeObj.checkNode(node, true, true);
    });
};
    
et.setDisables = function(opts, zTreeObj) {
    var disableValueList = (opts.disables || '').split(',').filter(function(disableValue) {
        return ('' + disableValue).trim() !== '';
    });
    var nodeValueKey;

    //simple data时，获取value key的方式不一样    
    if (opts.isSimple) {
        nodeValueKey = opts.resultItem.idKey;
    }
    else {
        nodeValueKey = opts.resultItem.value;
    }

    zTreeObj.getNodesByFilter(function(node) {
        return disableValueList.find(function(disableValue) {
            return ('' + node[nodeValueKey]) === disableValue;
        }) != null;
    }).forEach(function(node) {
        zTreeObj.setChkDisabled(node, true, true, true);
    });
};

et.bindEvent = function(opts, $eTree) {
};

et.init = function(opts, $element) {
    var exsist = $element.find('.eui-tree').length > 0;
    if (exsist) {
        et.destroy($element);
    }

    var $eTree = et.render(opts, $element);
    et.bindEvent(opts, $eTree);
};

et.getInstListFromSelector = function(selector) {
    var instList = w.eTreeInst;
    var instListFromSelector = [];
    for(var key in instList) {
        if (instList.hasOwnProperty(key) && instList[key].selector === selector) {
            instListFromSelector.push(instList[key]);
        }
    }

    return instListFromSelector;   
};

var ETree = function(options, $element) {
    ++ETree.index;
    var opts = $.extend(true, {}, defaults, options);

    et.init(opts, $element);

    return {
        id: `eui-tree-${$element.attr('id') || ETree.index}`,
        selector: $element.selector,
        opts: opts
    };
};

ETree.index = 0;

w.eTreeInst = {};

/**
 * 构造函数
 * 如果需要实例化的元素是单个，返回单个对象
 * 如果需要实例化的元素是多个，返回多个对象组成的数组
 */
$.fn.eTree = function(options) {
    var result;
    if (this.length === 1) {
        result = new ETree(options, this);
        w.eTreeInst[result.id] = result;
    }
    else if (this.length > 1) {
        result = [];
        this.each(function() {
            var item = this;
            var eTreeInstance = new ETree(options, item);
            w.eTreeInst[eTreeInstance.id] = eTreeInstance;
            result.push(eTreeInstance);
        });
    }

    return result;
};

$.fn.eTree.getValue = function(selector) {
    var valueList = [];
    var instListFromSelector = et.getInstListFromSelector(selector);

    instListFromSelector.forEach(function(inst) {
        var zTreeObj = $.fn.zTree.getZTreeObj(inst.id);
        valueList.push({
            value: et.getCheckedStuff(inst.opts, zTreeObj).value,
            id: inst.id
        });
    });

    if (valueList.length === 0) {
        throw new Error('not find eTreeInstance');
    }
    else if (valueList.length === 1) {
        return valueList[0].value;
    }
    return valueList;
};

$.fn.eTree.getText = function(selector) {
    var textList = [];
    var instListFromSelector = et.getInstListFromSelector(selector);

    instListFromSelector.forEach(function(inst) {
        var zTreeObj = $.fn.zTree.getZTreeObj(inst.id);
        textList.push({
            text: et.getCheckedStuff(inst.opts, zTreeObj).text,
            id: inst.id
        });
    });

    if (textList.length === 0) {
        throw new Error('not find eTreeInstance');
    }
    else if (textList.length === 1) {
        return textList[0].text;
    }
    return textList;
};

$.fn.eTree.getFullCheckedNodes = function(selector) {
    var nodeList = [];
    var instListFromSelector = et.getInstListFromSelector(selector);

    instListFromSelector.forEach(function(inst) {
        var zTreeObj = $.fn.zTree.getZTreeObj(inst.id);
        nodeList.push({
            nodes: et.getCheckedStuff(inst.opts, zTreeObj).nodes,
            id: inst.id
        });
    });

    if (nodeList.length === 1) {
        return nodeList[0].nodes;
    }
    
    return nodeList;
};

$.fn.eTree.render = function(selector, source) {
    var instListFromSelector = et.getInstListFromSelector(selector);

    instListFromSelector.forEach(function(inst) {
        inst.opts.source = source;
        et.init(inst.opts, $(`#${inst.id}`).parent());
    });
};

$.fn.eTree.setDefaults = function(selector, defaults) {
    var instListFromSelector = et.getInstListFromSelector(selector);

    instListFromSelector.forEach(function(inst) {
        inst.opts.defaults = defaults;
        var zTreeObj = $.fn.zTree.getZTreeObj(inst.id);
        et.setDefaults(inst.opts, zTreeObj);
    });    
};


})(jQuery, window);
/**
 * 暴露出来的方法：
 * updateTree
 * getValue
 * getText
 */

;(function($, w) {

var defaults = {
    placeholder: '请选择',
    showButton: true,
    fixed: false,
    source: '', //string or array
    sourceRequestType: 'GET',
    defaults: '', // string or array
    disables: '', // string or array
    multiple: false,
    isSimple: false,
    resultItem: {
        text: 'text',
        value: 'id',
        children: 'children',

        //simple时设置
        idKey: 'id',
        pIdKey: 'pId',
        rootPId: null
    },
    showSearch: false,
    searchPlaceholder: '输入搜索',
    onCheck: null, //每次勾选一个checkbox时的回调
    onConfirm: null, //选中的到了input上面时的回调
    onCancel: null,
    onClear: null,
    onDelete: null,
    onRender: null,
    onBeforeSend: null,
    onGetData: null
};

//eMultiSelect type checkbox
var ets = {};
ets.destroy = function($element) {
    var $eTreeSelect = $element.find('.eui-treeSelect');
    var id = $eTreeSelect.attr('id');
    $element.find('.eui-treeSelect').remove();
    delete w.eTreeSelectInst[id];
};

ets.render = function(opts, $element) {
    var eTreeSelectHtml = `
        <div class="eui-treeSelect" data-type="${opts.multiple ? 'multiple' : 'single'}"
                id="eui-treeSelect-${$element.attr('id') || ETreeSelect.index}" data-fixed="${opts.fixed}">
            <div class="eui-treeSelect-hd">
                <div class="eui-treeSelect-result eui-input eui-treeSelect-result-empty" placeholder="${opts.placeholder}">
                </div>
                <button type="button" class="eui-btn"></button>
            </div>
            <div class="eui-treeSelect-bd">
                <div class="eui-treeSelect-search" style="${opts.showSearch ? '' : 'display: none;'}">
                    <input type="text" class="eui-input" placeholder=${opts.searchPlaceholder}>
                </div>
                <div class="eui-treeSelect-panel" id="eui-treeSelect-panel-${$element.attr('id') || ETreeSelect.index}"></div>
                <div class="eui-treeSelect-handle" style="${opts.showButton ? '' : 'display: none;'}">
                    <button type="button" class="eui-btn eui-btn-secondary eui-treeSelect-confirm">确定</button>
                    <button type="button" class="eui-btn eui-btn-cancel eui-treeSelect-cancel">取消</button>
                    <button type="button" class="eui-btn eui-btn-cancel eui-treeSelect-clear">清空</button>
                </div>
            </div>
        </div>`;

    $element.html(eTreeSelectHtml);
    var $eTreeSelect = $element.find(' > .eui-treeSelect');

    ets.renderTreeSelect(opts, $eTreeSelect);

    return $eTreeSelect;
};

ets.renderTreeSelect = function(opts, $eTreeSelect) {
    var zTreeObj;
    var treeSelectPanelId = $eTreeSelect.find('.eui-treeSelect-panel').attr('id');
    $(`#${treeSelectPanelId}`).eTree({
        source: opts.source, //string or array
        sourceRequestType: opts.sourceRequestType,
        defaults: opts.defaults, // string or array
        disables: opts.disables, // string or array
        multiple: opts.multiple,
        isSimple: opts.isSimple,
        resultItem: {
            text: opts.resultItem.text,
            value: opts.resultItem.value,
            children: opts.resultItem.children,

            //simple时设置
            idKey: opts.resultItem.idKey,
            pIdKey: opts.resultItem.pIdKey,
            rootPId: opts.resultItem.rootPId
        },
        onCheck: function() {
            if ($.isFunction(opts.onCheck)) {
                opts.onCheck.call(this);
            }

            //不显示“确定”按钮时，check后要触发confirm操作
            if (!opts.showButton) {
                ets.confirmTreeSelect(opts, $eTreeSelect, zTreeObj);
            }
        },
        onBeforeSend: opts.onBeforeSend,
        onGetData: opts.onGetData,
        onRender: function() {
            var treeWrapperId = $eTreeSelect.find('.eui-treeSelect-panel > .eui-tree').attr('id');
            zTreeObj = $.fn.zTree.getZTreeObj(treeWrapperId);

            //由于数据来源可能是异步，而这两个方法都要取zTreeObj
            //所以，绑定事件和设置默认已选值的操作要放在 eTree的render回调里面
            ets.bindEvent(opts, $eTreeSelect, zTreeObj);
            var result = ets.setTreeSelectResult(opts, $eTreeSelect);

            //在eTree的渲染回调里面 回调treeSelect的onRender
            if ($.isFunction(opts.onRender)) {
                var cbThis = {
                    $eTreeSelect: $eTreeSelect,
                    zTreeObj: zTreeObj,
                    checkedNodeList: result.checkedNodeList,
                    value: result.value,
                    text: result.text
                };
                opts.onRender.call(cbThis);
            }
        }
    });
};

ets.setTreeSelectResult = function(opts, $eTreeSelect) {
    var $treeSelectResult = $eTreeSelect.find('.eui-treeSelect-result');
    var checkedStuff = ets.getCheckedStuff(opts, $eTreeSelect);
    var checkedNodeList = checkedStuff.nodes || [];
    var value = checkedStuff.value || '';
    var text = checkedStuff.text || '';

    //显隐placeholder
    if (value.trim() === '') {
        $treeSelectResult.addClass('eui-treeSelect-result-empty');
    }
    else {
        $treeSelectResult.removeClass('eui-treeSelect-result-empty');
    }

    //多选和单选，呈现的结果形式不同
    if (opts.multiple) {
        var resultListHtml = `<ul>`;

        (checkedStuff.value || '').split(',').filter(function(nodeValue) {
            return ('' + nodeValue).trim() !== '';
        }).forEach(function(nodeValue, index) {
            resultListHtml += `<li value="${nodeValue}">${checkedStuff.text.split(',')[index]}</li>`;
        });

        resultListHtml += `</ul>`;

        $treeSelectResult.attr('value', checkedStuff.value).attr('text', checkedStuff.text).html(resultListHtml);
    }
    else {
        $treeSelectResult.attr('value', value).attr('text', text).attr('title', text).html(text);
    }

    return {
        checkedNodeList: checkedNodeList,
        value: value,
        text: text
    };
};

ets.getCheckedStuff = function(opts, $eTreeSelect) {
    var treeSelectPanelId = $eTreeSelect.find('.eui-treeSelect-panel').attr('id');
    var checkedNodeList = $.fn.eTree.getFullCheckedNodes(`#${treeSelectPanelId}`);
    var valueKey = opts.isSimple ? opts.resultItem.idKey : opts.resultItem.value;
    var textKey = opts.resultItem.text;
    var valueList = [];
    var textList = [];

    checkedNodeList.forEach(function(checkedNode) {
        valueList.push(checkedNode[valueKey]);
        textList.push(checkedNode[textKey]);
    });

    return {
        nodes: checkedNodeList,
        value: valueList.join(),
        text: textList.join()
    };
};

ets.cancelTreeSelect = function(opts, $eTreeSelect, zTreeObj) {
    //多选和单选时，取消全部勾选的方法不一样
    if (opts.multiple) {
        zTreeObj.checkAllNodes(false);
    }
    else {
        zTreeObj.getNodesByFilter(function(node) {
            return node.checked === true;
        }).forEach(function(node) {
            zTreeObj.checkNode(node, false);
        });
    }

    var valueKey = opts.isSimple ? opts.resultItem.idKey : opts.resultItem.value;
    var valueList = ($eTreeSelect.find('.eui-treeSelect-result').attr('value') || '').split(',').filter(function(value) {
        return ('' + value).trim() !== '';
    });

    valueList.forEach(function(value) {
        var checkedNode = zTreeObj.getNodesByFilter(function(node) {
            return ('' + node[valueKey]) === value;
        }, true)

        zTreeObj.checkNode(checkedNode, true, true);
    });

    var checkedStuff = ets.getCheckedStuff(opts, $eTreeSelect);

    if ($.isFunction(opts.onCancel)) {
        var cbThis = {
            $eTreeSelect: $eTreeSelect,
            zTreeObj: zTreeObj,
            checkedNodeList: checkedStuff.nodes,
            value: checkedStuff.value,
            text: checkedStuff.text
        };
        opts.onCancel.call(cbThis);
    }
};

ets.clearTreeSelect = function(opts, $eTreeSelect, zTreeObj) {
    var valueKey = opts.isSimple ? opts.resultItem.idKey : opts.resultItem.value;
    var valueList = ($eTreeSelect.find('.eui-treeSelect-result').attr('value') || '').split(',').filter(function(value) {
        return ('' + value).trim() !== '';
    });

    valueList.forEach(function(value) {
        //筛选出，value值相等，并且没有被禁用的node
        var checkedNode = zTreeObj.getNodesByFilter(function(node) {
            return ('' + node[valueKey]) === value &&
                (node.chkDisabled == null || ( node.chkDisabled != null && node.chkDisabled === false));
        }, true);

        if (checkedNode != null) {
            zTreeObj.checkNode(checkedNode, false, true);
        }
    });

    var result = ets.setTreeSelectResult(opts, $eTreeSelect);

    if ($.isFunction(opts.onClear)) {
        var cbThis = {
            $eTreeSelect: $eTreeSelect,
            zTreeObj: zTreeObj,
            checkedNodeList: result.checkedNodeList,
            value: result.value,
            text: result.text
        };
        opts.onClear.call(cbThis);
    }
};

ets.confirmTreeSelect = function(opts, $eTreeSelect, zTreeObj) {
    var result = ets.setTreeSelectResult(opts, $eTreeSelect);

    if ($.isFunction(opts.onConfirm)) {
        var cbThis = {
            $eTreeSelect: $eTreeSelect,
            zTreeObj: zTreeObj,
            checkedNodeList: result.checkedNodeList,
            value: result.value,
            text: result.text
        };
        opts.onConfirm.call(cbThis);
    }
};

ets.deleteTreeSelectResultItem = function(opts, $eTreeSelect, zTreeObj, $treeSelectResultItem) {
    $treeSelectResultItem.remove();

    var valueKey = opts.isSimple ? opts.resultItem.idKey : opts.resultItem.value;
    var textKey = opts.resultItem.text;
    var cancelCheckedNode = zTreeObj.getNodesByFilter(function(node) {
        return ('' + node[valueKey]) === $treeSelectResultItem.attr('value');
    }, true);

    zTreeObj.checkNode(cancelCheckedNode, false, true, false);

    var result = ets.setTreeSelectResult(opts, $eTreeSelect);

    if ($.isFunction(opts.onDelete)) {
        var cbThis = {
            $eTreeSelect: $eTreeSelect,
            zTreeObj: zTreeObj,
            checkedNodeList: result.checkedNodeList,
            value: result.value,
            text: result.text
        };
        opts.onDelete.call(cbThis);
    }
};

ets.search = function(opts, $eTreeSelect, zTreeObj, inputVal) {
    //

    var textKey = opts.resultItem.text;
    var searchedNodeList = zTreeObj.getNodesByFilter(function(node) {
        return node[textKey].indexOf(inputVal) > -1;
    });

    if (searchedNodeList.length === 0 || (inputVal || '').trim() === '') {
        zTreeObj.expandAll(false);
        return;
    }

    //当有多个node符合条件时，只定位第一个node
    var firstNode = searchedNodeList[0];
    var needExpandNode = firstNode.isParent ? firstNode : firstNode.getParentNode();
    zTreeObj.expandNode(needExpandNode, true, false, true);
};

//fixed定位时，该函数用来设置
//1、eui-treeSelect-bd的min-width;
//2、eui-treeSelect-bd的max-height，因为fixed定位超出浏览器高度时不会自动出现滚动条，
//   所以要设置max-height最大到浏览器底部边缘，防止被淹没
ets.setFixedStuff = function(opts, $eTreeSelect) {
    var $treeSelectHd = $eTreeSelect.find('.eui-treeSelect-hd');
    var $treeSelectBd = $eTreeSelect.find('.eui-treeSelect-bd');
    var treeSelectHdWidth = $treeSelectHd.outerWidth();
    var treeSelectHdHeight = $treeSelectHd.outerHeight();
    var treeSelectBdMarginTop = 5;
    var treeSelectBdMaxHeight = $(window).height() - $treeSelectHd.offset().top - treeSelectHdHeight - treeSelectBdMarginTop;

    $treeSelectBd.css({
        'min-width': treeSelectHdWidth + 'px',
        'max-height': treeSelectBdMaxHeight + 'px'
    });
};


ets.bindEvent = function(opts, $eTreeSelect, zTreeObj) {
    $eTreeSelect.delegate('.eui-treeSelect-handle .eui-treeSelect-confirm', 'click', function(event) {
        ets.confirmTreeSelect(opts, $eTreeSelect, zTreeObj);
        $eTreeSelect.trigger('close');
    });

    $eTreeSelect.delegate('.eui-treeSelect-handle .eui-treeSelect-cancel', 'click', function(event) {
        ets.cancelTreeSelect(opts, $eTreeSelect, zTreeObj);
        $eTreeSelect.trigger('close');
    });

    $eTreeSelect.delegate('.eui-treeSelect-handle .eui-treeSelect-clear', 'click', function(event) {
        ets.clearTreeSelect(opts, $eTreeSelect, zTreeObj);
        $eTreeSelect.trigger('close');
    });

    $eTreeSelect.delegate('.eui-treeSelect-result', 'click', function(event) {
        var $clickEle = $(event.currentTarget);

        //当点击已选项的li时，不在该事件回调里面做处理
        if ($clickEle.parent().is('li')) {
            return false;
        }


        //控制面板的展开与收起
        if ($eTreeSelect.hasClass('eui-treeSelect-open')) {
            $eTreeSelect.trigger('close');
        }
        else {
            $eTreeSelect.trigger('open');
        }
    });

    $eTreeSelect.delegate('.eui-treeSelect-result li', 'click', function(event) {
        var $clickEle = $(event.currentTarget);
        ets.deleteTreeSelectResultItem(opts, $eTreeSelect, zTreeObj, $clickEle);
    });

    $eTreeSelect.delegate('.eui-treeSelect-search .eui-input', 'input', function(event) {
        var $input = $(event.currentTarget);
        var inputVal = $input.val().trim();

        setTimeout(function() {
            var inputValAfter500ms = $input.val().trim();

            //此时认为用户没有在输入
            if (inputValAfter500ms === inputVal) {

                ets.search(opts, $eTreeSelect, zTreeObj, inputValAfter500ms);
            }
        }, 500);
    });

    $eTreeSelect.delegate('.eui-treeSelect-hd > .eui-btn', 'click', function(event) {
        //控制面板的展开与收起
        if ($eTreeSelect.hasClass('eui-treeSelect-open')) {
            $eTreeSelect.trigger('close');
        }
        else {
            $eTreeSelect.trigger('open');
        }
    });

    $eTreeSelect.on('open', function(event) {
        if (opts.fixed) {
            $('body').addClass('eui-body-fixed-treeSelect');
            ets.setFixedStuff(opts, $eTreeSelect);
        }

        $eTreeSelect.addClass('eui-treeSelect-open');
    });

    $eTreeSelect.on('close', function(event) {
        if (opts.fixed) {
            $('body').removeClass('eui-body-fixed-treeSelect');
        }

        $eTreeSelect.removeClass('eui-treeSelect-open');
    });
};

ets.init = function(opts, $element) {
    var exsist = $element.find('.eui-treeSelect').length > 0;
    if (exsist) {
        ets.destroy($element);
    }

    ets.render(opts, $element);
};

ets.getInstListFromSelector = function(selector) {
    var instList = w.eTreeSelectInst;
    var instListFromSelector = [];
    for(var key in instList) {
        if (instList.hasOwnProperty(key) && instList[key].selector === selector) {
            instListFromSelector.push(instList[key]);
        }
    }

    return instListFromSelector;
};


var ETreeSelect = function(options, $element) {
    ++ETreeSelect.index;
    var opts = $.extend(true, {}, defaults, options);

    ets.init(opts, $element);

    return {
        id: `eui-treeSelect-${$element.attr('id') || ETreeSelect.index}`,
        selector: $element.selector,
        opts: opts
    };
};

ETreeSelect.index = 0;

w.eTreeSelectInst = {};

/**
 * 构造函数
 * 如果需要实例化的元素是单个，返回单个对象
 * 如果需要实例化的元素是多个，返回多个对象组成的数组
 */
$.fn.eTreeSelect = function(options) {
    var result;
    if (this.length === 1) {
        result = new ETreeSelect(options, this);
        w.eTreeSelectInst[result.id] = result;
    }
    else if (this.length > 1) {
        result = [];
        this.each(function() {
            var item = this;
            var eTreeSelectInstance = new ETreeSelect(options, item);
            w.eTreeSelectInst[eTreeSelectInstance.id] = eTreeSelectInstance;
            result.push(eTreeSelectInstance);
        });
    }

    return result;
};

$.fn.eTreeSelect.getValue = function(selector) {
    var valueList = [];
    var instListFromSelector = ets.getInstListFromSelector(selector);

    instListFromSelector.forEach(function(inst) {
        valueList.push({
            value: $(`#${inst.id}`).find('.eui-treeSelect-result').attr('value'),
            id: inst.id
        });
    });

    if (valueList.length === 0) {
        throw new Error('not find eTreeSelectInstance');
    }
    else if (valueList.length === 1) {
        return valueList[0].value;
    }
    return valueList;
};

$.fn.eTreeSelect.getText = function(selector) {
    var textList = [];
    var instListFromSelector = ets.getInstListFromSelector(selector);

    instListFromSelector.forEach(function(inst) {
        textList.push({
            text: $(`#${inst.id}`).find('.eui-treeSelect-result').attr('text'),
            id: inst.id
        });
    });

    if (textList.length === 0) {
        throw new Error('not find eTreeSelectInstance');
    }
    else if (textList.length === 1) {
        return textList[0].text;
    }
    return textList;
};

$.fn.eTreeSelect.render = function(selector, source) {
    var instListFromSelector = ets.getInstListFromSelector(selector);

    instListFromSelector.forEach(function(inst) {
        var treeSelectWrapperId = $(`#${inst.id}`).parent().attr('id');
        inst.opts.source = source;
        $(`#${treeSelectWrapperId}`).eTreeSelect(inst.opts);
    });
};

$.fn.eTreeSelect.setDefaults = function(selector, defaults) {
    var instListFromSelector = ets.getInstListFromSelector(selector);

    instListFromSelector.forEach(function(inst) {
        var $eTreeSelect = $(`#${inst.id}`);
        var treeSelector = `#${$eTreeSelect.find('.eui-treeSelect-panel').attr('id')}`;
        inst.opts.defaults = defaults;
        $.fn.eTree.setDefaults(treeSelector, defaults);
        ets.setTreeSelectResult(inst.opts, $eTreeSelect);
    });
};

$(function() {
    $('body').on('click', function(event) {
        var $clickEle = $(event.target);
        var isClickEleInETreeSelect = $clickEle.hasClass('eui-treeSelect') || $clickEle.parents('.eui-treeSelect').length > 0;
        if (!isClickEleInETreeSelect) {
            $('.eui-treeSelect').trigger('close');
        }
    });
});

})(jQuery, window);
/**
 * @fileOverview 滚动加载
 * @author gp10856
 */

;(function($, w) {

var defaults = {
    url: '',
    type: 'GET',
    dataType: 'json',
    timeout: 20000,
    onBeforeSend: null,
    onSuccess: null,
    onError: null,

    _currPage: 0,
    _totalPage: 0,
    _isLoadPageError: false,
    _isForcedLoadPage: false,
    _isLoadingPage: false
};

var esl = {};
esl.destroy = function(opts, $element) {
    var idAttr = $element.attr('id');

    $element.removeClass('eui-scrollLoad');
    delete w.eScrollLoadInst[idAttr];
};

esl.render = function(opts, $element) {
    $element.addClass('eui-scrollLoad');
    esl.loadPage(opts, $element);
};

esl.bindEvent = function(opts, $element) {
    $element.on('scroll', function(event) {
        if (esl.hasScrollToBottom(opts, $element)) {
            esl.loadPage(opts, $element);
        }
    });

    $element.delegate('.eui-scrollLoad-errorBtn', 'click', function(event) {
        opts._isLoadPageError = false;
        $.fn.eScrollLoad.loadPage(`#${$element.attr('id')}`);
    });
};

esl.init = function(opts, $element) {
    var hasIdAttr = $element.attr('id') != null && $element.attr('id').trim() !== '';
    if (!hasIdAttr) {
        throw new Error('被初始化时，该元素必须要设置id属性')
    }

    var exsist = $element.hasClass('.eui-scrollLoad');
    if (exsist) {
        esl.destroy(opts, $element);
    }

    esl.render(opts, $element);
    esl.bindEvent(opts, $element);
};

esl.loadPage = function(opts, $element, pageIndex) {
    var isLoadedAllPage = opts._totalPage > 0 && opts._currPage >= opts._totalPage;
    opts._isForcedLoadPage = typeof pageIndex === 'undefined' ? false : true;

    if ((opts._isLoadingPage || isLoadedAllPage || opts._isLoadPageError) && !opts._isForcedLoadPage) {
        return;
    }



    var ajaxSendData = {};
    //发送异步前的回调，必须要return ajaxSendData
    //该回调必需要传
    if ($.isFunction(opts.onBeforeSend)) {
        opts._currPage = opts._isForcedLoadPage ? pageIndex : (opts._currPage + 1);
        opts._isLoadPageError = false;
        opts._isForcedLoadPage = false;

        //这边要给出page相关信息，让使用者自己拼发异步参数
        var cbThis = { $wrapper: $element, opts: opts };
        ajaxSendData = opts.onBeforeSend.call(cbThis, ajaxSendData);
    }

    //当返回 false 时，取消发送异步
    if (ajaxSendData === false) {
        return;
    }

    $.ajax({
        url: opts.url,
        type: opts.type,
        data: JSON.stringify(ajaxSendData),
        dataType: 'json',
        timeout: 20000,
        contentType: 'application/json',
        beforeSend: function () {
            opts._isLoadingPage = true;
            esl.util.showLoading(opts, $element);
        },
        success: function (data) {
            var successCbResult;
            esl.util.hideLoading(opts, $element);
            opts._isLoadingPage = false;

            if ($.isFunction(opts.onSuccess)) {
                var cbThis = { $wrapper: $element, opts: opts, util: esl.util };

                //success回调必须设置totalpage
                //可以在回调里面设置一些状态（比如请求成功但是查询表失败），设置页码
                //在回调里面拼接html
                successCbResult = opts.onSuccess.call(cbThis, data);
            }

            //如果回调返回false，那么中止后续操作
            if (successCbResult === false) {
                return;
            }

            var isLoadedAllPage = opts._currPage >= opts._totalPage;
            if (isLoadedAllPage) {
                esl.util.showTip(opts, $element,'暂无更多数据~');
            }
            else if (!esl.hasScrollBar(opts, $element)) {
                esl.loadPage(opts, $element);               
            }
        },
        error: function () {
            esl.handleLoadPageError();

            if ($.isFunction(opts.onError)) {
                var cbThis = { $wrapper: $element };
                opts.onError.call(cbThis);
            }
        }
    });
};

esl.handleLoadPageError = function(opts, $element) {
    esl.util.hideLoading(opts, $element);

    //标志出错状态
    opts._isLoadPageError = true;

    //标志loading状态
    opts._isLoadingPage = false;

    //更改页码
    opts._currPage = opts._currPage > 0 ? (opts._currPage - 1) : 0;


    //显示出错文案
    var errorTip = `
        <button type="button" class="eui-scrollLoad-errorBtn">
            出错了，点我重新加载
        </button>`;
    esl.util.showTip(opts, $element, errorTip);
};

esl.hasScrollBar = function(opts, $element) {
    return $element.get(0).scrollHeight > $element.innerHeight();
};

esl.hasScrollToBottom = function(opts, $element) {
    var element = $element[0];
    return element.scrollHeight - element.scrollTop - element.clientHeight < 1;
};

esl.util = {};
esl.util.showLoading = function(opts, $element, tip) {
    esl.util.showTip(opts, $element, '努力加载中...');
};

esl.util.hideLoading = function(opts, $element) {
    $element.find('.eui-scrollLoad-tip').remove();
};

esl.util.showTip = function(opts, $element, tip) {
    $element.find('.eui-scrollLoad-tip').remove();
    $element.append(`<div class="eui-scrollLoad-tip">${tip}</div>`);
};

esl.util.handleLoadPageError = esl.handleLoadPageError;


esl.api = {};
esl.api.refresh = function(opts, $element, $list) {
    //重置配置项
    opts._currPage = 0;
    opts._totalPage = 0;
    opts._isLoadPageError = false;
    opts._isForcedLoadPage = false;
    opts._isLoadingPage = false;

    //清空列表
    $list.empty();

    //加载数据
    esl.loadPage(opts, $element);
};
esl.api.loadPage = function(opts, $element, pageIndex) {
    esl.loadPage(opts, $element, pageIndex);
}




var EScrollLoad = function(options, $element) {
    ++EScrollLoad.index;
    var opts = $.extend(true, {}, defaults, options);
    esl.init(opts, $element);

    return {
        id: $element.attr('id'),
        opts: opts
    };
};

EScrollLoad.index = 0;

w.eScrollLoadInst = {};

/**
 * 构造函数
 * 如果需要实例化的元素是单个，返回单个对象
 * 如果需要实例化的元素是多个，返回多个对象组成的数组
 */
$.fn.eScrollLoad = function(options) {
    this.each(function() {
        var inst = new EScrollLoad(options, $(this));
        w.eScrollLoadInst[inst.id] = inst;
    });
};

$.fn.eScrollLoad.refresh = function(selector, $list) {
    $.makeArray($(selector)).forEach(function(element) {
        var $element = $(element);
        var idAttr = $element.attr('id');
        var instList = w.eScrollLoadInst;

        for(var key in instList) {
            if (instList.hasOwnProperty(key) && instList[key].id === idAttr) {
                esl.api.refresh(instList[key].opts, $element, $list);
            }
        }       
    });
};

$.fn.eScrollLoad.loadPage = function(selector, pageIndex) {
    $.makeArray($(selector)).forEach(function(element) {
        var $element = $(element);
        var idAttr = $element.attr('id');
        var instList = w.eScrollLoadInst;

        for(var key in instList) {
            if (instList.hasOwnProperty(key) && instList[key].id === idAttr) {
                esl.api.loadPage(instList[key].opts, $element, pageIndex);
            }
        }       
    });
};

})(jQuery, window);
;(function($,w){
    //多次初始化，但是却只拨打一次
    //这2个变量是为挂断电话准备
    var initMore = 0;
    var callOnce = 0;
    //多次初始化，但是却只拨打一次
    //这2个变量是为重复请求准备
    var repeatMore = 0;
    var repeatOne = 0;
    var defaults = {
        phoneNumber:'',
        tips:'请输入手机号',
        isRight:true,
        jobNumber:'',
        environment:'',
        appId:'',
        token:'',
        isSmsBtn:0,
        smsConfig:{
            title: '模板',
            width: '650px',
            height: '410px',
            lock: true,
            max: false,
            url:'',
        },
        getCallStatus: 0,
        getCallTypeUrl: {
            url:'',
        },
        setCallTypeUrl: {
            url:'',
            params: '',
            type:'get',
        },
        onNormalCall: null,
        onNormalLogin: null,
        onNormalHangUpFail: null,
        onNormalCallOn: null,
        onNormalCallError: null,
        onNormalHangUp: null,
        onCoolPadCall: null,
        onGeneralCall: null
    };
    // 全局变量-定时器
    var callTimer = null;

    var ep = {
        event: function(opts,$ele){
            var that = this;
            
            $ele.delegate(".eui-phone-set","click",function(){
                var $clickEle = $(this);
                if (!$clickEle.hasClass("disabled")) {
                    if ($ele.find(".eui-phone-box").hasClass("show")) {
                        $ele.find(".eui-phone-box").removeClass("show");
                    }else{
                        $ele.find(".eui-phone-box").addClass("show");
                    };
                };
            });

            $ele.delegate(".eui-phone-type","click",function(){
                var $clickEle = $(this);
                $clickEle.addClass("eui-phone-selected").siblings(".eui-phone-selected").removeClass("eui-phone-selected");
            });

            $ele.delegate(".eui-dialog-btn-close","click",function(){
                $ele.find(".eui-phone-box").removeClass("show");
            });

            $ele.delegate(".eui-phone-btn","click",function(){
                var type = $ele.find(".eui-phone-selected").attr("data-type"),
                    des = $ele.find(".eui-phone-selected").attr("data-des");
                var status = $ele.find(".calling");
                that.setCallType(opts,$ele,type,des);
            });
            
            $ele.delegate(".eui-phone-sms","click",function(){
                var con = opts.smsConfig;
                var tit = con.title,
                    w = con.width,
                    h = con.height,
                    lock = con.lock,
                    max = con.max,
                    content = 'url:' + con.url;
                if ($.isFunction($.dialog)) {
                    $.dialog({
                        title: tit,
                        width: w,
                        height: h,
                        lock: lock,
                        max: max,
                        content: content
                    });
                }else{
                    eMsg({
                        text: '请于eui之前加载$.dialog插件',
                        timer: 2000
                    });
                };
            });

            //拨打电话
            $ele.delegate(".calling .eui-phone-button:not(.eui-phone-none)","click",function(){
                var $clickEle = $(this),
                    type = $clickEle.attr("data-type"),
                    isCall = 'isCalling';
                if (opts.phoneNumber == '') {
                    eMsg({
                        text: opts.tips,
                        timer: 1000
                    });
                    return;
                };
                $clickEle.attr("disabled",true);
                //普通拨打
                if (type == 1) {
                    // 正在拨打状态调整
                    isCall = 'isCalling';
                    that.resetCallStatus($ele,type,isCall,opts);
                    //判断拨打电话窗口是否打开
                    CallCenterWebSpBar.isHubOpened({ jobnum: opts.jobNumber })
                    .done(function (data) {
                        console.info(JSON.stringify(data));
                        //true-窗口已打开，false-窗口未打开
                        if (data.isSuccess) {
                            repeatOne = repeatMore;
                            that.initNormalCall($ele,type,isCall,opts);
                        }else{
                            // 打开窗口-监听登录事件
                            CallCenterWebSpBar.openWebSoftPhoneWindow()
                            .done(function (msg) {
                                console.info(JSON.stringify(msg));
                                if (msg.isSuccess) {
                                    //不管初始化几次，最终拨打只打最后一次
                                    //后面检测是否登录的方法有用到
                                    callOnce = initMore;
                                    repeatOne = repeatMore;
                                    that.callTo($ele,type,isCall,opts);
                                }else{
                                    $("body").find(".eui-alert").remove();
                                    eAlert({
                                        title: '坐席拨打失败',
                                        text: `
                                            <div id="warm_tips">
                                                请确认以下几点事项：<br/>
                                                1.<span>Avaya one-X</span>是否已登录？<br/>
                                                2.关闭<span>右下角</span>拨打电话小窗口<br/>
                                                3.请检查是否申请过<span>坐席账户</span>?<br/>
                                                4.电脑的<span>网络信号</span>是否正常？（请勿使用无线网）<br/>
                                                其他问题，请联系<span>吐槽小强</span>！
                                            </div>
                                            `,
                                        icon: {
                                            show: false     
                                        },
                                        cancelButton: {
                                            show: false,
                                        },
                                        onConfirm: function() {
                                            eAlert.close(this.$eAlert);
                                        },
                                    });
                                    var isCall = 'hangUp',
                                        type = 1;
                                    that.resetCallStatus($ele,type,isCall,opts); 
                                };
                            })
                            .fail(function (msg) {
                                console.warn(JSON.stringify(msg));
                                if (msg.isSuccess) {
                                    //不管初始化几次，最终拨打只打最后一次
                                    //后面检测是否登录的方法有用到
                                    callOnce = initMore;
                                    repeatOne = repeatMore;
                                    that.callTo($ele,type,isCall,opts);
                                }else{
                                    $("body").find(".eui-alert").remove();
                                    eAlert({
                                        title: '坐席拨打失败',
                                        text: `
                                            <div id="warm_tips">
                                                请确认以下几点事项：<br/>
                                                1.<span>Avaya one-X</span>是否已登录？<br/>
                                                2.关闭<span>右下角</span>拨打电话小窗口<br/>
                                                3.请检查是否申请过<span>坐席账户</span>?<br/>
                                                4.电脑的<span>网络信号</span>是否正常？（请勿使用无线网）<br/>
                                                其他问题，请联系<span>吐槽小强</span>！
                                            </div>
                                            `,
                                        icon: {
                                            show: false     
                                        },
                                        cancelButton: {
                                            show: false,
                                        },
                                        onConfirm: function() {
                                            eAlert.close(this.$eAlert);
                                        },
                                    });
                                    var isCall = 'hangUp',
                                        type = 1;
                                    that.resetCallStatus($ele,type,isCall,opts); 
                                };
                            });
                        }
                    }).fail(function (data) {
                        console.info(JSON.stringify(data));
                        //true-窗口已打开，false-窗口未打开
                        if (data.isSuccess) {
                            repeatOne = repeatMore;
                            that.initNormalCall($ele,type,isCall,opts);
                        }else{
                            // 打开窗口-监听登录事件
                            CallCenterWebSpBar.openWebSoftPhoneWindow()
                            .done(function (msg) {
                                console.warn(JSON.stringify(msg));
                                if (msg.isSuccess) {
                                    //不管初始化几次，最终拨打只打最后一次
                                    //后面检测是否登录的方法有用到
                                    callOnce = initMore;
                                    repeatOne = repeatMore;
                                    that.callTo($ele,type,isCall,opts);
                                }else{
                                    $("body").find(".eui-alert").remove();
                                    eAlert({
                                        title: '坐席拨打失败',
                                        text: `
                                            <div id="warm_tips">
                                                请确认以下几点事项：<br/>
                                                1.<span>Avaya one-X</span>是否已登录？<br/>
                                                2.关闭<span>右下角</span>拨打电话小窗口<br/>
                                                3.请检查是否申请过<span>坐席账户</span>?<br/>
                                                4.电脑的<span>网络信号</span>是否正常？（请勿使用无线网）<br/>
                                                其他问题，请联系<span>吐槽小强</span>！
                                            </div>
                                            `,
                                        icon: {
                                            show: false     
                                        },
                                        cancelButton: {
                                            show: false,
                                        },
                                        onConfirm: function() {
                                            eAlert.close(this.$eAlert);
                                        },
                                    });
                                    var isCall = 'hangUp',
                                        type = 1;
                                    that.resetCallStatus($ele,type,isCall,opts); 
                                };
                            })
                            .fail(function (msg) {
                                console.warn(JSON.stringify(msg));
                                if (msg.isSuccess) {
                                    //不管初始化几次，最终拨打只打最后一次
                                    //后面检测是否登录的方法有用到
                                    callOnce = initMore;
                                    repeatOne = repeatMore;
                                    that.callTo($ele,type,isCall,opts);
                                }else{
                                    $("body").find(".eui-alert").remove();
                                    eAlert({
                                        title: '坐席拨打失败',
                                        text: `
                                            <div id="warm_tips">
                                                请确认以下几点事项：<br/>
                                                1.<span>Avaya one-X</span>是否已登录？<br/>
                                                2.关闭<span>右下角</span>拨打电话小窗口<br/>
                                                3.请检查是否申请过<span>坐席账户</span>?<br/>
                                                4.电脑的<span>网络信号</span>是否正常？（请勿使用无线网）<br/>
                                                其他问题，请联系<span>吐槽小强</span>！
                                            </div>
                                            `,
                                        icon: {
                                            show: false     
                                        },
                                        cancelButton: {
                                            show: false,
                                        },
                                        onConfirm: function() {
                                            eAlert.close(this.$eAlert);
                                        },
                                    });
                                    var isCall = 'hangUp',
                                        type = 1;
                                    that.resetCallStatus($ele,type,isCall,opts); 
                                };
                            });
                        };
                    });
                };
                // 小顾拨打和通用拨打-开发人员对接即可
                // 小顾拨打
                if (type == 2) {
                    if ($.isFunction(opts.onCoolPadCall)) {
                        opts.onCoolPadCall.call();
                    }
                };
                // 通用拨打
                if (type == 3) {
                    if ($.isFunction(opts.onGeneralCall)) {
                        opts.onGeneralCall.call();
                    }
                };
            });

            //挂断电话
            $ele.delegate(".eui-phone-hang-up","click",function(){
                var $clickEle = $(this),
                    type = $clickEle.attr("data-type"),
                    isCall = 'hangUp';
                // 只有坐席电话才有挂断按钮，另外两种在手机上面操作
                CallCenterWebSpBar.hangup().done(function (retdata) {
                    console.info(JSON.stringify(retdata));
                }).fail(function (msg) {
                    console.warn(JSON.stringify(msg));
                });
                //如果挂断事件一直不返回，那么5s后自动设置挂断状态
                setTimeout(function(){
                    var tar = $ele.find(".eui-phone").hasClass("eui-phone-ready");
                    if (!tar) {
                        if ($.isFunction(opts.onNormalHangUpFail)) {
                            var msg = {
                                isSuccess:false,
                                message:'超时'
                            };
                            opts.onNormalHangUpFail.call((msg));
                        };
                        that.resetCallStatus($ele,type,isCall,opts,true);
                    };
                },6000);
                clearInterval(callTimer);
            });

            // 挂断会触发
            CallCenterWebSpBar.on('CallCleared', function (msg) {
                clearInterval(callTimer);
                if (msg.isAnswered) {
                    isCall = 'hangUp';
                    var type = 1;
                    that.resetCallStatus($ele,type,isCall,opts);
                    if ($.isFunction(opts.onNormalHangUp)) {
                        opts.onNormalHangUp.call((msg));
                    };
                }else{
                    isCall = 'hangUp';
                    var type = 1;
                    that.resetCallStatus($ele,type,isCall,opts);
                    if ($.isFunction(opts.onNormalHangUp)) {
                        opts.onNormalHangUp.call((msg));
                    };
                };
                var type = 1,
                    isCall = 'hangUp';
                that.resetCallStatus($ele,type,isCall,opts);
            });

            /*
                这里是一些和电话相关的事件
            */
            //暴露挂断事件
            // 一键禁止
            $ele.on("forBid",function(){
                var type = that.getDataType($ele),
                    $tar = $ele.find(".calling");
                $tar.find(".eui-phone-button").attr("disabled",true);
            });
            // 解除禁止
            $ele.on("unForBid",function(){
                var type = that.getDataType($ele),
                    $tar = $ele.find(".calling");
                $tar.find(".eui-phone-button").attr("disabled",false);
            });
            // 挂断
            $ele.on("hangUp",function(){
                var type = that.getDataType($ele);
                type == 1 && $ele.find(".eui-phone-hang-up").trigger("click");
            });
            // 拨打电话
            $ele.on("toCall",function(){
                var type = that.getDataType($ele),
                    $tar = $ele.find(".calling");
                $tar.find(".eui-phone-button:not(.eui-phone-none)").trigger("click");
            });
            // 关闭拨打电话窗口
            $ele.on("closeCall",function(){
                CallCenterWebSpBar.closeWebSoftPhoneWindow();
            });

            // 正在拨打事件
            $ele.on("calling",function(event){              
                var type = that.getDataType($ele),
                    isCall = 'isCalling';
                that.resetCallStatus($ele,type,isCall,opts);
            });

            //接通事件
            // 小顾和通用拨打状态调整和坐席不同，直接重置为初始状态即可。
            $ele.on("callSuccess",function(){
                var type = that.getDataType($ele),
                    isCall = 'hangUp';
                that.resetCallStatus($ele,type,isCall,opts);
            });

            //拨打失败事件
            $ele.on("callFail",function(){
                var type = that.getDataType($ele),
                    isCall = 'hangUp';
                that.resetCallStatus($ele,type,isCall,opts);
            });
            
            $("body").delegate("#closeCallWindow","click",function(){
                if (typeof CallCenterWebSpBar != 'undefined') {
                    eMsg({
                        text: '窗口已关闭',
                        timer: 500
                    });
                    CallCenterWebSpBar.closeWebSoftPhoneWindow();
                };
            });

            $("body").on("click", function(event) {
                var $clickEle = $(event.target);
                var isClickEleInePhone = $clickEle.hasClass("eui-phone-set") || $clickEle.hasClass("eui-phone-box") || $clickEle.parents(".eui-phone-box").length > 0;
                if (!isClickEleInePhone) {
                    $ele.find(".eui-phone-box").removeClass("show");
                };
            });
        },
        destroy: function($ele){
            $ele.unbind();
            $ele.find(".eui-phone").remove();
        },
        callTo: function($ele,type,isCall,opts){
            var that = this;
            if (initMore == callOnce) {
                CallCenterWebSpBar.on('AgentLogin', function (data) {
                    if(data.isSuccess == true){
                        setTimeout(function(){
                            if ($.isFunction(opts.onNormalLogin)) {
                                opts.onNormalLogin.call((data));
                            };
                            that.initNormalCall($ele,type,isCall,opts);
                        },800);
                    }else{
                        $("body").find(".eui-alert").remove();
                        eAlert({
                            title: '坐席拨打失败',
                            text: `
                                <div id="warm_tips">
                                    请确认以下几点事项：<br/>
                                    1.<span>Avaya one-X</span>是否已登录？<br/>
                                    2.关闭<span>右下角</span>拨打电话小窗口<br/>
                                    3.电脑的<span>网络信号</span>是否正常？（请勿使用无线网）<br/>
                                    其他问题，请联系<span>吐槽小强</span>！
                                </div>
                                `,
                            icon: {
                                show: false     
                            },
                            cancelButton: {
                                show: false,
                            },
                            onConfirm: function() {
                                eAlert.close(this.$eAlert);
                            },
                        });
                        var isCall = 'hangUp',
                            type = 1;
                        that.initNormalCall($ele,type,isCall,opts);
                    };
                });
            }else{
                $("body").find(".eui-alert").remove();
                eAlert({
                    title: '坐席拨打失败',
                    text: `
                        <div id="warm_tips">
                            请确认以下几点事项：<br/>
                            1.<span>Avaya one-X</span>是否已登录？<br/>
                            2.关闭<span>右下角</span>拨打电话小窗口<br/>
                            3.电脑的<span>网络信号</span>是否正常？（请勿使用无线网）<br/>
                            其他问题，请联系<span>吐槽小强</span>！
                        </div>
                        `,
                    icon: {
                        show: false     
                    },
                    cancelButton: {
                        show: false,
                    },
                    onConfirm: function() {
                        eAlert.close(this.$eAlert);
                    },
                });
                var isCall = 'hangUp',
                    type = 1;
                that.initNormalCall($ele,type,isCall,opts);
            }
        },
        initNormalCall: function($ele,type,isCall,opts){
            var that = this;
            initMore++;
            
            // type = 1;

            //calledNum只能包含数字，不能有横线-，小括号（），等
            // 由于通信啥的特殊性，代码会走到done和fail里面，所以两处代码一样，具体@wmy09534
            if (repeatOne == repeatMore) {
                CallCenterWebSpBar.makeCall({ calledNum: opts.phoneNumber})
                .done(function (msg) {
                    console.warn(JSON.stringify(msg));

                    if ($.isFunction(opts.onNormalCall)) {
                        opts.onNormalCall.call((msg));
                    };
                    if (msg.isSuccess) {
                        isCall = 'isCalling';
                        defaults.getCallStatus = '1';
                    }else{
                        isCall = 'hangUp';
                        defaults.getCallStatus = '0';
                        if ($.isFunction(opts.onNormalCallError)) {
                            opts.onNormalCallError.call((msg));
                        };
                        $("body").find(".eui-alert").remove();
                        eAlert({
                            title: '坐席拨打失败',
                            text: `
                                <div id="warm_tips">
                                    请确认以下几点事项：<br/>
                                    1.<span>Avaya one-X</span>是否已登录？<br/>
                                    2.关闭<span>右下角</span>拨打电话小窗口<br/>
                                    3.电脑的<span>网络信号</span>是否正常？（请勿使用无线网）<br/>
                                    其他问题，请联系<span>吐槽小强</span>！
                                </div>
                                `,
                            icon: {
                                show: false     
                            },
                            cancelButton: {
                                show: false,
                            },
                            onConfirm: function() {
                                eAlert.close(this.$eAlert);
                            },
                        });
                    };
                    that.resetCallStatus($ele,type,isCall,opts);
                })
                .fail(function (msg) {
                    console.warn(JSON.stringify(msg));
                    if ($.isFunction(opts.onNormalCall)) {
                        opts.onNormalCall.call((msg));
                    };
                    if (msg.isSuccess) {
                        isCall = 'isCalling';
                        defaults.getCallStatus = '1';
                        
                    }else{
                        isCall = 'hangUp';
                        defaults.getCallStatus = '0';
                        if ($.isFunction(opts.onNormalCallError)) {
                            opts.onNormalCallError.call((msg));
                        };
                        $("body").find(".eui-alert").remove();
                        eAlert({
                            title: '坐席拨打失败',
                            text: `
                                <div id="warm_tips">
                                    请确认以下几点事项：<br/>
                                    1.<span>Avaya one-X</span>是否已登录？<br/>
                                    2.关闭<span>右下角</span>拨打电话小窗口<br/>
                                    3.电脑的<span>网络信号</span>是否正常？（请勿使用无线网）<br/>
                                    其他问题，请联系<span>吐槽小强</span>！
                                </div>
                                `,
                            icon: {
                                show: false     
                            },
                            cancelButton: {
                                show: false,
                            },
                            onConfirm: function() {
                                eAlert.close(this.$eAlert);
                            },
                        });
                    };
                    type = 1;
                    that.resetCallStatus($ele,type,isCall,opts);
                    // that.resetCallStatus($ele,type,isCall,opts);
                });
            };
            repeatMore++;
            // 坐席手机电话-接通事件
            CallCenterWebSpBar.on('CallEstablished', function (msg) {
                if (msg.isAnswered) {
                    isCall = 'called';
                    that.getCallTime($ele,0);
                    if ($.isFunction(opts.onNormalCallOn)) {
                        opts.onNormalCallOn.call((msg));
                    };
                    that.resetCallStatus($ele,type,isCall,opts);
                }else{
                    if ($.isFunction(opts.onNormalCallError)) {
                        opts.onNormalCallError.call((msg));
                    };
                };
            });
        },
        // 修改电话状态
        calling: function($ele,type,isCall,opts){
            this.resetCallStatus($ele,type,isCall,opts);
        },
        // 获取当前激活的电话按钮
        getDataType: function($ele){
            var tar = $ele.find(".calling");
            var TYPE = tar.find("button:not(.eui-phone-none)").attr("data-type");
            return TYPE;
        },
        // 通过接口获取电话类型
        getCallType: function(opts,ePhone){
            var $ele = ePhone;
            var get = opts.getCallTypeUrl;
            $.ajax({
                url:get.url,
                type: 'get',
                datatype: 'json',
                success: function(data){
                    // 修改电话类型
                    if (data) {
                        var type = data.DefaultCallType || 1,
                            tarBtn = $ele.find(".calling"),
                            tarLi = $ele.find(".eui-phone-list"),
                            tarText = $ele.find(".eui-phone-description");
                        type == 1 && tarText.html("请外拨");
                        type == 2 && tarText.html("小顾已就绪 请外拨");
                        type == 3 && tarText.html("坐席已就绪 请外拨");
                        tarBtn.find("button").each(function(){
                            var $clickEle = $(this);
                            ($clickEle.attr("data-type") == type) && ($clickEle.removeClass("eui-phone-none").siblings().addClass("eui-phone-none"));
                        });
                        tarLi.find("li").each(function(){
                            var $clickEle = $(this);
                            ($clickEle.attr("data-type") == type) && ($clickEle.addClass("eui-phone-selected").siblings().removeClass("eui-phone-selected"));
                        });
                    }else{
                        console.log('电话初始化类型失败');
                    };
                },
                error: function(error){
                    console.log('电话初始化类型失败' + error.status);
                }
            });
        },
        // 设置电话类型
        setCallType: function(opts,$ele,type,des){
            var set = opts.setCallTypeUrl,
                url = '';
            if (set.url.indexOf('?') > -1) {
                url = set.url + '&' + set.params + '=' + type
            }else{
                url = set.url + '?' + set.params + '=' + type
            };
            $.ajax({
                url: url,
                type: set.type,
                datatype: 'json',
                success: function(data){
                    if (data) {
                        var status = $ele.find(".calling");
                        status.find(".eui-phone-button").each(function(){
                            var $clickEle = $(this);
                            ($clickEle.attr("data-type") == type) && ($clickEle.removeClass("eui-phone-none").siblings().addClass("eui-phone-none"));
                        });
                        $ele.find(".eui-phone-box").removeClass("show")
                            .end().find(".eui-phone-description").html(des)
                            .end().find(".eui-phone-hang-up");
                    }else{
                        $ele.find(".eui-phone-box").removeClass("show")
                        eMsg({
                            text: '抱歉，拨打方式设置失败',
                            timer: 1000
                        });
                    };
                },
                error: function(error){
                    $ele.find(".eui-phone-box").removeClass("show")
                            .end().find(".eui-phone-description").html(des)
                            .end().find(".eui-phone-hang-up");
                    console.error('拨打方式设置失败' + error.status);
                    eMsg({
                        text: '抱歉，拨打方式设置失败',
                        timer: 1000
                    });
                }
            });
        },
        // 修改电话状态
        resetCallStatus: function($ele,callType,isCall,opts,bool){
            var that = this;
            // 拨打电话需要修改各种状态，包括但不限于禁止切换类型-修改拨打背景和文字。
            // callType表示拨打的电话类型
            var callTxt = $ele.find(".eui-phone-description");
            if (isCall == 'isCalling') {
                $ele.find(".eui-phone-set").addClass("disabled")
                    .end().addClass("eui-phone-iscalling").removeClass("eui-phone-ready");
                if (callType == 1) {
                    $ele.find(".calling .eui-phone-button").attr("disabled",true)
                        .end().find(".eui-phone-hang-up").removeClass("eui-phone-none");
                };
                callType == 1 && callTxt.html("正在呼出 请注意接听......");
                callType == 2 && callTxt.html("<p class='eui-phone-others'>小顾努力拨打中，请注意接听</p><p class='eui-phone-others'>如需挂断，请在手机上操作</p>");
                callType == 3 && callTxt.html("<p class='eui-phone-others'>坐席努力拨打中，请注意接听</p><p class='eui-phone-others'>如需挂断，请在手机上操作</p>");
                // 修改拨打电话状态
                defaults.getCallStatus = '1';
            };
            if (isCall == 'called') {
                $ele.find(".eui-phone-set").addClass("disabled")
                    .end().addClass("eui-phone-connected").removeClass("eui-phone-ready").removeClass("eui-phone-iscalling");
                if (callType == 1) {
                    $ele.find(".calling .eui-phone-button").attr("disabled",true)
                        .end().find(".eui-phone-hang-up").removeClass("eui-phone-none");
                        callTxt.html("已接通 请注意说辞");
                    setTimeout(function(){
                        $ele.find(".eui-phone-call-time").removeClass("eui-phone-none");
                        callTxt.html("正在通话");
                    },2000);
                };
                defaults.getCallStatus = '1';
            };
            if (isCall == 'hangUp') {
                $ele.addClass("eui-phone-callend").removeClass("eui-phone-iscalling").removeClass("eui-phone-connected")
                    .end().find(".eui-phone-hang-up").addClass("eui-phone-none");
                if (callType == 1) {
                    var allTime = $ele.find(".eui-phone-call-time").text();
                    callTxt.html("通话已结束 总" + allTime);
                    $ele.find(".eui-phone-good").addClass("eui-phone-none")
                        .end().find(".eui-phone-call-time").addClass("eui-phone-none");
                    if (bool) {
                        that.changeCallStatus($ele,callTxt,bool);
                    }else{
                        that.changeCallStatus($ele,callTxt);
                    };
                }else{
                    $ele.find(".calling .eui-phone-button").attr("disabled",false)
                        .end().find(".eui-phone-set").removeClass("disabled")
                        .end().addClass("eui-phone-ready").removeClass("eui-phone-iscalling").removeClass("eui-phone-callend").removeClass("eui-phone-connected")
                    callType == 2 && callTxt.html("小顾已就绪 请外拨");
                    callType == 3 && callTxt.html("坐席已就绪 请外拨");
                };
                defaults.getCallStatus = '0';
            };
        },
        changeCallStatus: function($ele,callTxt,bool){
            if (bool) {
                $ele.find(".calling .eui-phone-button").attr("disabled",false)
            }else{
                setTimeout(function(){
                    $ele.find(".calling .eui-phone-button").attr("disabled",false)
                },4000);
            };
            $ele.find(".eui-phone-set").removeClass("disabled")
                .end().find(".eui-phone-call-time").addClass("eui-phone-none").text('时长 00:00')
                .end().addClass("eui-phone-ready").removeClass("eui-phone-iscalling").removeClass("eui-phone-callend").removeClass("eui-phone-connected");
            callTxt.html("请外拨");
        },
        renderHtml: function(opts,$ele){
            var isRight = !opts.isRight ? 'eui-phone-tips-right' : '';
            var ePhoneHtml = `
                        <div class="eui-phone eui-phone-ready" id="eui-phone-${$ele.attr("id") || EPhone.index}">
                            <div class="eui-phone-hd">
                                <div class="eui-phone-call calling">
                                    <button data-type="1" class="eui-phone-button eui-phone-seat ">坐席拨打</button>
                                    <button data-type="2" class="eui-phone-button eui-phone-coolpad eui-phone-none">小顾酷派版</button>
                                    <button data-type="3" class="eui-phone-button eui-phone-general eui-phone-none">通用拨打</button>
                                </div>
                                ${opts.isSmsBtn == 1 ? '<div class="eui-phone-call"><button class="eui-phone-button eui-phone-sms">短信</button></div>' : ''}
                                <div class="eui-phone-set"></div>
                                <div class="eui-phone-status">
                                    <span class="eui-phone-good eui-phone-shake eui-phone-none"></span>
                                    <span class="eui-phone-description">请外拨</span>
                                    <span class="eui-phone-call-time eui-phone-none">时长 00:00</span>
                                </div>
                                <div class="eui-phone-call"><button data-type="1" class="eui-phone-button eui-phone-hang-up eui-phone-none">挂断</button></div>
                            </div>
                            <div class="eui-phone-bd">
                                <div class="eui-dialog eui-phone-box">
                                    <div class="eui-dialog-cont">
                                        <div class="eui-dialog-hd">
                                            <h3>请选择常用拨打方式</h3>
                                            <button class="eui-dialog-btn-close"></button>
                                        </div>
                                        <div class="eui-dialog-bd" style="overflow: initial;">
                                            <ul class="eui-phone-list">
                                                <li data-type="1" class="eui-phone-type eui-phone-selected" data-des="请外拨">
                                                    坐席拨打
                                                    <span class="">（有井星或者讯鸟账号的）</span>
                                                </li>
                                                <li data-type="2" class="eui-phone-type" data-des="小顾已就绪 请外拨">
                                                    小顾酷派版
                                                    <span class="">（有公司定制的酷派手机的）</span>
                                                </li>
                                                <li data-type="3" class="eui-phone-type" data-des="坐席已就绪 请外拨">
                                                    通用拨打
                                                    <span class="">（下载小顾通用版即可用）</span>
                                                    <span class="eui-phone-tips">
                                                        <div class="eui-phone-tips-box ${isRight}">
                                                            使用注意点：<br>
                                                            1、下载小顾通用版，按照指示维护小顾坐席手机号码<br>
                                                            2、被叫号码与小顾坐席号码不可以是同一个<br>
                                                        </div>
                                                    </span>
                                                </li>
                                            </ul>
                                        </div>
                                        <div class="eui-dialog-ft">
                                            <button type="button" class="eui-btn eui-btn-secondary eui-phone-btn">确定</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    `;
            $ele.html(ePhoneHtml);
            var $tar = $ele.find(".eui-phone");
            return $tar;
        },
        getCallTime: function($ele,second){
            clearInterval(callTimer);
            var h = 0, m = 0, s = 0, returnStr = '';
            callTimer = setInterval(function () {
                second++;
                h = Math.floor(second / 60 / 60 % 24);
                m = Math.floor(second / 60 % 60);
                s = Math.floor(second % 60);
                h = h < 10 ? "0" + h : h;
                m = m < 10 ? "0" + m : m;
                s = s < 10 ? "0" + s : s;
                var returnStr = '时长 ' + h + ":" + m + ":" + s;
                if (h == "00") {
                    returnStr = '时长 ' + m + ":" + s;
                };
                $ele.find(".eui-phone-call-time").text(returnStr).attr("data-second", second);
                if(second >= 60) {
                    $ele.find(".eui-phone-good").removeClass("eui-phone-none");
                };
            }, 1000);
        },
        init: function(opts,$ele){
            var exsist = $ele.find(".eui-phone").length;
                exsist > 0 && this.destroy($ele);
            var ePhone = this.renderHtml(opts,$ele);
                this.event(opts,ePhone);
            //根据该请求，判断电话类型
            this.getCallType(opts,ePhone);
            var num = opts.jobNumber + '',
                envir = opts.environment + '',
                appid = opts.appId + '',
                token = opts.token + '';
            CallCenterWebSpBar.init(num, envir, appid, token);
        }
    };
    
    var EPhone = function(opts,$ele){
        ++EPhone.index;
        var opts = $.extend({},defaults,opts);
        ep.init(opts,$ele);
        return {
            id: `eui-phone-${$ele.attr("id") || EPhone.index} `,
            selector: $ele.selector,
            opts: opts
        }
    };
    EPhone.index = 0;

    $.fn.ePhone = function(opts){
        var result;
        if (this.length == 1) {
            return result = new EPhone(opts,this);
        }else if (this.length > 1){
            result = [];
            this.each(function(){
                var $clickEle = $(this);
                var eachItem = new EPhone(opts,$clickEle);
                result.push(eachItem);
            });
            return result;
        };
    };
    //获取拨打电话的状态，1 || 0。
    $.fn.ePhone.getCallStatus = function(){
        return defaults.getCallStatus
    };
})(jQuery,window);