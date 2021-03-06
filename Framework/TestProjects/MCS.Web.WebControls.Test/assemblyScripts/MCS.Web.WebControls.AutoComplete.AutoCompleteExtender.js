
// -------------------------------------------------
// Assembly	：	MCS.Web.WebControls
// FileName	：	AutoCompleteExtender.js
// Remark	：  自动完成的客户端脚本
// -------------------------------------------------
// VERSION  	AUTHOR		DATE			CONTENT
// 1.0		张曦	    20070815		创建
// -------------------------------------------------

$HGRootNS.AutoCompleteExtender = function (element) {
	$HGRootNS.AutoCompleteExtender.initializeBase(this, [element]);
	this._isAutoComplete = true; //是否打开自动完成功能
	this._autoValidateOnChange = true;
	this._minimumPrefixLength = 3; //输入多少个字符后开始自动完成，默认为3
	this._completionInterval = 1000; //自动完成间隔。默认1000毫秒。输入停止后多长时间开始自动完成，单位：毫秒。1000毫秒=1秒
	this._completionBodyCssClass = ""; //自动完成部分的主体区域边框颜色
	this._itemFontColor = "#003399"; //自动完成的选择项目默认字体颜色(只作为默认值)
	this._completionBodyBorderColor = "#003399";
	this._itemHoverFontColor = "#003399"; //鼠标移动到自动完成的选项项目上时的字体颜色(只作为默认值)
	this._itemHoverBackgroundColor = "#FFE6A0"; //鼠标移动到自动完成的选项项目上时的背景色(只作为默认值)
	this._itemHoverCssClass = ""; //鼠标移动到自动完成的项目上的Style
	this._elementDefaultStyle = null; //保存当前输入框的风格，用来从出错后的风格还原
	this._requireValidation = false; //控件是否启用验证,如果启用则在输入内容无法完全匹配到数据源中的某一项
	this._maxCompletionRecordCount = -1; //控件自动完成出来的列表中显示的最大记录数量。默认为-1，表示全部数据
	this._maxPopupWindowHeight = 260; //控件自动完成弹出的选择窗口的最大高度，默认为260px。
	this._AutoCompletePopup = null; //弹出窗口对象
	this._timer = null; //计时器
	this._focusHandler = null; //得到焦点的Handler
	this._blurHandler = null; //失去焦点的Handler
	this._keyDownHandler = null; //按下键盘的Handler
	this._keyUpHandler = null; //键盘弹起的Handler
	this._pasteHandler = null; //粘贴文本的Handler
	this._changeHandler = null; //文本改变的Handler
	this._tickHandler = null;
	this._autoCallBack = false; //是否自动回调页面
	this._selectIndex = -1; //当前选择的索引
	this._currentPrefix = ""; //当前的输入
	this._cache = null; //缓存输入的内容对应的结果
	this._enableCaching = true; //是否启用缓存
	this._dataList = null; //数据
	this._drawingList = null; //正在绘制的数据,这里的数据已经经过处理，直接使用
	this._dataTextFieldList = []; //提供文本显示的数据源属性集合
	this._compareFieldName = []; //在哪些字段中匹配输入的项目，只要有一个字段符合条件则认为该匹配成功
	this._dataValueField = ""; //制定那个字段为项目的Value值
	this._dataTextFormatString = ""; //显示内容的格式化字符串
	this._divHeight = ""; //弹出控件主区域的高度
	this._showFlag = false; //记录弹出窗口是否正在显示
	this._selectValue = ""; //选择的Value值
	this._text = ""; //选择或者输入的文本值
	this._originalText = ""; //Original Text in input
	this._errorCssClass = ""; //当启用输入验证，并且输入的内容无法完全匹配到数据源中的某一项后的CssClass
	this._itemCssClass = ""; //自动完成的项目CssClass
	this._elementY = 0; //控件的Y值
	this._tmpInnerHTML = "";
	this._eventContext = ""; //事件的上下文
	this._isTextChanged = false; //当前是否需要回调获取数据
	this._tickHandler = Function.createDelegate(this, this._onTimerTick);
	this._focusHandler = Function.createDelegate(this, this._onGotFocus); //得到焦点
	this._blurHandler = Function.createDelegate(this, this._onLostFocus); //失去焦点
	this._keyDownHandler = Function.createDelegate(this, this._onKeyDown); //按下键盘
	this._keyUpHandler = Function.createDelegate(this, this._setTextChanged); //键盘弹起
	this._pasteHandler = Function.createDelegate(this, this._setTextChanged); //粘贴文本
	this._propertyChangeHandler = Function.createDelegate(this, this._propertyChange); //Property Change

	this._beforeShow$delegate = Function.createDelegate(this, this._setSize); //弹出popup前
	this._timer = new $HGRootNS.Timer();
	this._isInvoking = false;

	this._waitingImage = "";
	this._checkImage = "";
	this._checkImageElement = null;
	this._waitingImageElement = null;

	this._imageContainer = null;

	this._showCheckImage = false;

	this._checkImageElementEvents =
    {
    	click: Function.createDelegate(this, this._onCheckImageClick)
    };

	//选择项目的处理事件
	this._item$delegates =
    {
    	mouseover: Function.createDelegate(this, this._item_onmouseover),
    	click: Function.createDelegate(this, this._item_onclick)
    }
}

//---------------------------------------我就素那无耻的分割线---------------------------------------//

$HGRootNS.AutoCompleteExtender.prototype =
{
	_onCheckImageClick: function () {
		this._validate();
	},

	_canShowCheckImage: function () {
		var result = this.get_showCheckImage();

		if (result) {
			var elt = this.get_element();

			if (elt) {
				result = (!elt.readOnly && !elt.disabled) || (elt.contentEditable == true);
			}
		}

		return result;
	},

	get_showCheckImage: function () {
		return this._showCheckImage;
	},

	set_showCheckImage: function (value) {
		this._showCheckImage = value;

		if (this._imageContainer != null) {
			if (value)
				this._imageContainer.style.display = "inline";
			else
				this._imageContainer.style.display = "none";
		}
	},

	get_checkImage: function () {
		return this._checkImage;
	},

	set_checkImage: function (value) {
		this._checkImage = value;
	},

	get_waitingImage: function () {
		return this._waitingImage;
	},

	set_waitingImage: function (value) {
		this._waitingImage = value;
	},

	//是否打开自动完成功能
	get_isAutoComplete: function () {
		return this._isAutoComplete;
	},
	set_isAutoComplete: function (value) {
		this._isAutoComplete = value;
	},

	get_autoValidateOnChange: function () {
		return this._autoValidateOnChange;
	},

	set_autoValidateOnChange: function (value) {
		this._autoValidateOnChange = value;
	},

	//输入多少个字符后开始自动完成，默认为3
	get_minimumPrefixLength: function () {
		return this._minimumPrefixLength;
	},
	set_minimumPrefixLength: function (value) {
		this._minimumPrefixLength = value;
	},

	//自动完成间隔。默认1000毫秒。输入停止后多长时间开始自动完成，单位：毫秒。1000毫秒=1秒
	get_completionInterval: function () {
		return this._completionInterval;
	},
	set_completionInterval: function (value) {
		this._completionInterval = value;
	},

	//自动完成部分的主体区域边框颜色
	get_completionBodyBorderColor: function () {
		return this._completionBodyBorderColor;
	},
	set_completionBodyBorderColor: function (value) {
		this._completionBodyBorderColor = value;
	},

	//自动完成的选择项目默认字体颜色
	get_itemFontColor: function () {
		return this._itemFontColor;
	},
	set_itemFontColor: function (value) {
		this._itemFontColor = value;
	},

	//鼠标移动到自动完成的选项项目上时的字体颜色
	get_itemHoverFontColor: function () {
		return this._itemHoverFontColor;
	},
	set_itemHoverFontColor: function (value) {
		this._itemHoverFontColor = value;
	},

	//鼠标移动到自动完成的选项项目上时的背景色
	get_itemHoverBackgroundColor: function () {
		return this._itemHoverBackgroundColor;
	},
	set_itemHoverBackgroundColor: function (value) {
		this._itemHoverBackgroundColor = value;
	},

	//鼠标移动到自动完成的项目上的Style
	get_itemHoverCssClass: function () {
		return this._itemHoverCssClass;
	},
	set_itemHoverCssClass: function (value) {
		this._itemHoverCssClass = value;
	},

	//是否自动回调页面
	get_autoCallBack: function () {
		return this._autoCallBack;
	},
	set_autoCallBack: function (value) {
		this._autoCallBack = value;
	},

	//控件是否启用验证,如果启用则在输入内容无法完全匹配到数据源中的某一项
	get_requireValidation: function () {
		return this._requireValidation;
	},
	set_requireValidation: function (value) {
		this._requireValidation = value;
	},

	//控件自动完成出来的列表中显示的最大记录数量。默认为-1，表示全部数据
	get_maxCompletionRecordCount: function () {
		return this._maxCompletionRecordCount;
	},
	set_maxCompletionRecordCount: function (value) {
		this._maxCompletionRecordCount = value;
	},

	//控件自动完成弹出的选择窗口的最大高度，默认为260px。如果记录的内容小于等于这个值，
	//弹出窗口的高度自适应，如果大于这个值，则显示滚动条
	get_maxPopupWindowHeight: function () {
		return this._maxPopupWindowHeight;
	},
	set_maxPopupWindowHeight: function (value) {
		this._maxPopupWindowHeight = value;
	},

	//当前的输入
	get_currentPrefix: function () {
		return this._currentPrefix;
	},
	set_currentPrefix: function (value) {
		this._currentPrefix = value;
	},

	//是否启用缓存
	get_enableCaching: function () {
		return this._enableCaching;
	},
	set_enableCaching: function (value) {
		this._enableCaching = value;
	},

	//客户端数据源
	get_dataList: function () {
		return this._dataList;
	},
	set_dataList: function (value) {
		this._dataList = value;
	},

	//提供文本显示的数据源属性集合
	get_dataTextFieldList: function () {
		return this._dataTextFieldList;
	},
	set__dataTextFieldList: function (value) {
		this._dataTextFieldList = value;
	},

	//在哪些字段中匹配输入的项目，只要有一个字段符合条件则认为该匹配成功
	get_compareFieldName: function () {
		return this._compareFieldName;
	},
	set_compareFieldName: function (value) {
		this._compareFieldName = value;
	},

	//指定那个字段为项目的Value值
	get_dataValueField: function () {
		return this._dataValueField;
	},
	set_dataValueField: function (value) {
		this._dataValueField = value;
	},

	//显示内容的格式化字符串，如：姓名：{0}
	get_dataTextFormatString: function () {
		return this._dataTextFormatString;
	},
	set_dataTextFormatString: function (value) {
		this._dataTextFormatString = value;
	},

	//正在绘制的数据,这里的数据已经经过处理，直接使用
	get_drawingList: function () {
		return this._drawingList;
	},
	set_drawingList: function (value) {
		this._drawingList = value;
	},

	//记录弹出窗口是否处于可见状态
	get_showFlag: function () {
		return this._showFlag;
	},
	set_showFlag: function (value) {
		this._showFlag = value;
		if (!this._showFlag) {
			this._selectIndex = -1;
		}
	},

	//当启用输入验证，并且输入的内容无法完全匹配到数据源中的某一项后的CssClass
	get_errorCssClass: function () {
		return this._errorCssClass;
	},
	set_errorCssClass: function (value) {
		this._errorCssClass = value;
	},

	//自动完成的项目CssClass
	get_itemCssClass: function () {
		return this._itemCssClass;
	},
	set_itemCssClass: function (value) {
		this._itemCssClass = value;
	},

	//上下文
	get_eventContext: function () {
		return this._eventContext;
	},

	set_eventContext: function (value) {
		this._eventContext = value;
	},

	//是否正在调用
	get_isInvoking: function () {
		return this._isInvoking;
	},

	set_isInvoking: function (value) {
		this._isInvoking = value;
	},

	//回调时的上下文
	get_callBackContext: function () {
		return this._callBackContext;
	},

	set_callBackContext: function (value) {
		this._callBackContext = value;
	},


	//添加auto列表弹出时事件
	add_popShowing: function (handler) {
		this.get_events().addHandler("popShowing", handler);
	},

	//去掉auto列表弹出时事件
	remove_popShowing: function (handler) {
		this.get_events().removeHandler("popShowing", handler);
	},

	//触发auto列表弹出时事件
	raisepopShowing: function (items) {
		var handlers = this.get_events().getHandler("popShowing");

		if (handlers) {
			var e = new Sys.EventArgs();
			e.items = items;
			handlers(this, e);
		}
	},

	add_beforeInvoke: function (handler) {
		this.get_events().addHandler("beforeInvoke", handler);
	},

	remove_beforeInvoke: function (handler) {
		this.get_events().removeHandler("beforeInvoke", handler);
	},

	raiseBeforeInvoke: function () {
		var handlers = this.get_events().getHandler("beforeInvoke");

		if (handlers) {
			handlers(this, this);
		}
	},

	/// <summary>
	/// 控件初始化
	/// </summary>
	initialize: function () {
		$HGRootNS.AutoCompleteExtender.callBaseMethod(this, 'initialize');

		var element = this.get_element();

		this.initializeTimer(this._timer); //初始化Timer
		this.initializeTextBox(element); //初始化目标控件
		this.initializeImages(element);
	},

	/// <summary>
	/// 控件销毁时执行的操作
	/// </summary>
	dispose: function () {
		var element = this.get_element();
		var blurEventName = (element.tagName == "INPUT" ? "change" : "blur");

		Sys.UI.DomEvent.removeHandler(element, "focus", this._focusHandler);
		Sys.UI.DomEvent.removeHandler(element, blurEventName, this._blurHandler);
		Sys.UI.DomEvent.removeHandler(element, "keydown", this._keyDownHandler);
		Sys.UI.DomEvent.removeHandler(element, "keyup", this._keyUpHandler);
		Sys.UI.DomEvent.removeHandler(element, "paste", this._pasteHandler);
		Sys.UI.DomEvent.removeHandler(element, "propertychange", this._propertyChangeHandler);

		if (this._timer) {
			this._timer.dispose();
			this._timer = null;
		}

		$HGRootNS.AutoCompleteExtender.callBaseMethod(this, "dispose");
	},

	/// <summary>
	/// 添加一个项目选择完毕的事件
	/// </summary>    
	add_itemSelected: function (handler) {
		this.get_events().addHandler("itemSelected", handler);
	},

	/// <summary>
	/// 移除一个项目选择完毕的事件
	/// </summary>
	remove_itemSelected: function (handler) {
		this.get_events().removeHandler("itemSelected", handler);
	},

	/// <summary>
	/// 选择一个项目完毕的事件
	/// </summary>
	_raiseItemSelected: function (id, object) {
		var handlers = this.get_events().getHandler("itemSelected");
		var continueExec = true;

		if (handlers) {
			var e = new Sys.EventArgs();

			e.id = id;
			e.selectedObject = object;
			e.cancel = false;
			e.textField = "a";
			e.valueField = "b";
			//e.eventElement = eventElement;            
			handlers(this, e);
			if (e.cancel)
				continueExec = false;
		}

		return continueExec;
	},

	add_valueValidated: function (handler) {
		this.get_events().addHandler("valueValidated", handler);
	},

	/// <summary>
	/// 移除一个项目选择完毕的事件
	/// </summary>
	remove_valueValidated: function (handler) {
		this.get_events().removeHandler("valueValidated", handler);
	},

	/// <summary>
	/// 选择一个项目完毕的事件
	/// </summary>
	_raiseValueValidated: function (id, dataList) {
		var handlers = this.get_events().getHandler("valueValidated");
		var continueExec = true;

		if (handlers) {
			var e = new Sys.EventArgs();

			e.id = id;

			if (!dataList || !dataList.length)
				dataList = [];

			e.dataList = dataList;
			e.cancel = false;

			handlers(this, e);
			if (e.cancel)
				continueExec = false;
		}

		return continueExec;
	},

	/// <summary>
	/// 初始化计时器
	/// </summary>
	initializeTimer: function (timer) {
		timer.set_interval(this._completionInterval);
		timer.add_tick(this._tickHandler);
	},

	/// <summary>
	/// 初始化文本框
	/// </summary>
	initializeTextBox: function (element) {
		element.autocomplete = "off";

		$addHandler(element, "focus", this._focusHandler);

		var blurEventName = (element.tagName == "INPUT" ? "change" : "blur");

		$addHandler(element, blurEventName, this._blurHandler);

		$addHandler(element, "keydown", this._keyDownHandler);
		$addHandler(element, "keyup", this._keyUpHandler);
		$addHandler(element, "paste", this._pasteHandler);
		$addHandler(element, "propertychange", this._propertyChangeHandler);
	},

	initializeImages: function (element) {
		this._imageContainer = document.createElement("span");

		element.insertAdjacentElement("afterEnd", this._imageContainer);

		if (this._canShowCheckImage())
			this._imageContainer.style.display = "inline";
		else
			this._imageContainer.style.display = "none";

		this._checkImageElement = document.createElement("img");
		this._checkImageElement.src = this._checkImage;
		this._checkImageElement.style.cursor = "hand";
		$addHandlers(this._checkImageElement, this._checkImageElementEvents);

		this._imageContainer.appendChild(this._checkImageElement);

		this._waitingImageElement = document.createElement("img");
		this._waitingImageElement.src = this._waitingImage;
		this._waitingImageElement.style.display = "none";

		this._imageContainer.appendChild(this._waitingImageElement);
	},

	/// <summary>
	/// 当popup窗口打开时设置popup窗口的高度
	/// </summary>
	_setSize: function (popupwin, e) {
		if (e.height > this.get_maxPopupWindowHeight()) {
			this._divHeight = this.get_maxPopupWindowHeight();
			e.height = this.get_maxPopupWindowHeight();
			popupwin.get_popupDocument().body.document.all('listPanl').style.height = this._divHeight;
		}
		else {
			this._divHeight = "";
		}

		this._elementY = 0;
		this._getElementTop(this.get_element());
		var eleY = window.screenTop + this._elementY + this.get_element().clientHeight + e.height - document.body.scrollTop;
		//alert(aa);
		if (eleY > window.screen.availHeight) {
			e.y = 0 - e.height;
		}
	},

	_getElementTop: function (value) {
		if (value != null) {
			this._elementY = this._elementY + value.offsetTop;
			this._getElementTop(value.offsetParent);
		}
	},

	/// <summary>
	/// 创建弹出窗口中的内容
	/// </summary>
	_createPopupDocument: function () {
		var iWidth = this.get_element().clientWidth;

		var listPanl = $HGDomElement.createElementFromTemplate(
        {
        	nodeName: "div",
        	properties:
                {
                	border: 1,
                	id: "listPanl",
                	style:
                    {
                    	overflowX: "hidden",
                    	overflowY: "auto",
                    	width: iWidth,
                    	border: "1px solid " + this._completionBodyBorderColor
                    }
                }
        },
            this._AutoCompletePopup.get_popupBody(),
            null,
            this._AutoCompletePopup.get_popupDocument()
        );


		listPanl.innerHTML = "";
		this._createList(listPanl); //绘制选项列表的Item
	},

	/// <summary>
	/// 绘制选项列表
	/// </summary>
	_createList: function (listPanl) {
		var fsShowTextString = ""; //保存这个Item显示的具体内容,如果指定了格式，则匹配,否则直接输出
		var fsItemValue = ""; //保存Item的Value

		if (this.get_drawingList() && this.get_drawingList().length > 0) {
			if (typeof (this.get_drawingList()[0]) == "object") {//传过来的是一个对象
				for (var i = 0; i < this.get_drawingList().length; i++) {//循环所有数据
					if (this.get_dataTextFieldList() && this.get_dataTextFieldList().length > 0) {//指定了显示的数据字段
						var arrayList = new Array(this.get_dataTextFieldList().length); //定义数组保存制定字段的具体内容

						fsShowTextString = ""; //初始化变量
						fsItemValue = ""; //初始化变量
						for (var j = 0; j < this.get_dataTextFieldList().length; j++) {//循环指定的字段,并向上面定义的数组中赋值
							arrayList[j] = this.get_drawingList()[i][this.get_dataTextFieldList()[j]];
							fsShowTextString += this.get_drawingList()[i][this.get_dataTextFieldList()[j]];
						}

						if (this.get_dataValueField() && this.get_dataValueField() != "") {//如果指定了Value字段
							fsItemValue = this.get_drawingList()[i][this.get_dataValueField()];
						}

						if (this.get_dataTextFormatString() && this.get_dataTextFormatString() != "") {//如果指定了格式，则匹配
							Array.insert(arrayList, 0, this.get_dataTextFormatString());
							fsShowTextString = String.format.apply(String, arrayList)//格式化字符串
						}
					}
					else {//没有制定则显示全部数据
						var arrayList = new Array(this.get_drawingList()[i].length); //定义数组保存制定字段的具体内容

						fsShowTextString = ""; //初始化变量
						fsItemValue = ""; //初始化变量
						for (var fsField in this.get_drawingList()[i]) {//遍历dataList中的每一个字段
							arrayList[i] = this.get_drawingList()[i][fsField];
							fsShowTextString += this.get_drawingList()[i][fsField];
						}

						if (this.get_dataValueField() && this.get_dataValueField() != "") {//如果指定了Value字段
							fsItemValue = this.get_drawingList()[i][this.get_dataValueField()];
						}

						if (this.get_dataTextFormatString() && this.get_dataTextFormatString() != "") {//如果指定了格式，则匹配
							fsShowTextString = String.format(this.get_dataTextFormatString(), arrayList); //格式化字符串
						}
					}

					var cssClass = "AutoComplete_ContextMenuItem";
					if (this._itemCssClass && this._itemCssClass != "") {
						cssClass = this._itemCssClass;
					}
					this._AutoCompletePopup.createItem(fsShowTextString, fsItemValue, this._item$delegates, i, listPanl, cssClass);
					//createItem : function(itemText,itemValue,itemEvents,currentIndex,parentElement,itemCssClass) 
					//this._createItem(fsShowTextString, fsItemValue, i, listPanl);//创建Item
				}
			}
			else {//传过来的是一个数组
				//现在只处理字符串的一维数组
				for (var i = 0; i < this.get_drawingList().length; i++) {
					this._AutoCompletePopup.createItem(this.get_drawingList()[i], "", this._item$delegates, i, listPanl, cssClass);
				}
			}
		}
	},

	/// <summary>
	/// 显示自动完成的Popup窗口
	/// </summary>
	_showCompletionList: function (prefixText, completionItems, cacheResults) {

		//如果使用缓存，则将结果保存到缓存
		if (cacheResults && this.get_enableCaching()) {
			if (!this._cache) {
				this._cache = {};
			}
			this._cache[prefixText] = completionItems;
		}
		this.raisepopShowing(completionItems);
		this.set_drawingList(completionItems); //设置绘制的数据

		var iWidth = this.get_element().clientWidth;
		this._AutoCompletePopup = $create($HGRootNS.PopupControl, { width: iWidth, positionElement: this.get_element(), positioningMode: $HGRootNS.PositioningMode.BottomLeft }, { beforeShow: this._beforeShow$delegate }, {}, null);
		$HGDomElement.set_currentDocument(this._AutoCompletePopup.get_popupDocument());
		$HGDomElement.set_currentDocument(document);

		this._createPopupDocument(); //创建Popup中的具体内容

		this._AutoCompletePopup.show(); //Show出来
		this._timer.set_enabled(false);
		this.set_showFlag(true); //窗口已经弹出
		if (null != this.get_element().tagName && this.get_element().tagName == "SPAN") {
			//////this.get_element().innerHTML = this._tmpInnerHTML;
		}
		if (completionItems.length > 0)
			this._highlightItem(0);
	},

	_innerTextFilter: function (sText) {
		return sText.substring(sText.lastIndexOf(';') + 1).trim();
	},

	_replaceLastInputText: function (sText) {
		//var sTmpText = this.get_element().innerText

		for (var i = 0; i < this.get_element().childNodes.length; i++) {
			if (this.get_element().childNodes[i].nodeName == "#text")//文字节点
			{
				this.get_element().childNodes[i].data = sText;
			}
		}

		//sTmpText = sTmpText.substring(0,sTmpText.lastIndexOf(';') + 1) + sText;
		//this.get_element().innerText = sTmpText;
	},

	/// <summary>
	/// 输入停止到时的处理
	/// </summary>
	_onTimerTick: function (sender, eventArgs) {
		if (this._isInvoking) return;

		//this._text = this.get_element().value;
		if (null != this.get_element().tagName && this.get_element().tagName == "SPAN") {
			this._text = this._innerTextFilter(this.get_element().innerText);
			this._tmpInnerHTML = this.get_element().innerHTML;
		}
		else {
			this._text = this.get_element().value;
		}

		if (this._text == "　")//不是空格，绝对不是
		{
			return;
		}

		if (this._isAutoComplete == false) {
			return;
		}

		//		if (this._AutoCompletePopup) {
		//			this._AutoCompletePopup.get_popupBody().innerHTML = "";
		//		}	//SZ Comment, 2010/7/5

		if (this._text.trim().length < this._minimumPrefixLength) {//输入的内容长度大于等于设定的长度则开始处理
			this._currentPrefix = null;
			this._hideCompletionList();
			return;
		}

		if (this._autoCallBack) {//是否存在回调的方法,存在则回调取数据                
			//if (this._isTextChanged) {
			if (this._originalText != this._text) {
				this._currentPrefix = this._text;

				try {
					if (this._cache && this._cache[this._text]) {
						this._showCompletionList(this._text, this._cache[this._text], /* cacheResults */false);

						return;
					}

					this._invokeRemoteMethod(this._onMethodComplete);
				}
				finally {
					this._isTextChanged = false;
					this._originalText = this._text;
				}
			}
		}
		else if (this._dataList) {//不存在回调的方法则判断是否有客户端数据源，如果有则从客户端取数据
			this._currentPrefix = this._text;
			this._transactDataList();
		}
	},

	/// <summary>
	/// 处理客户端数据源中的数据
	///     客户端数据源中包含全部的数据，需要根据 this._currentPrefix 中保存的
	///     当前输入的前缀去匹配，从中筛选出符合条件的数据保存到 this._drawingList 
	///     中，然后的处理跟invoke后台方法的一样了！NB吧！！
	/// </summary>
	_transactDataList: function () {
		var fiRecordCount = 0; //记录当前记录数量
		if (!this._dataList) {
			return;
		} //没有数据则返回
		if (typeof (this._dataList[0]) == "object") {//传过来的是一个对象
			if (this._dataTextFieldList.length < 1) {
				return;
			} //没有制定匹配字段则返回
			var result = [];
			this._darwingList = null; //初始化待绘制数据列表

			for (var i = 0; i < this._dataList.length; i++) {//循环全部的数据
				var compareFlag = false; //是否匹配,匹配则添加到待绘制列表
				for (var k = 0; k < this._compareFieldName.length; k++) {//循环各个字段
					if (this._dataList[i][this._compareFieldName[k]].toString().indexOf(this._currentPrefix) == 0) {
						compareFlag = true;
						break;
					}
				}
				if (compareFlag) {//如果该项目是匹配的项目则添加
					fiRecordCount++;
					if (this._maxCompletionRecordCount < 0 || fiRecordCount <= this._maxCompletionRecordCount)//在限定的数量范围内
						Array.insert(result, result.length, this._dataList[i]);
					else
						break; //达到了最大数量，跳出循环
				}
			}

			if (result && result.length > 0) {
				this._showCompletionList(this._currentPrefix, result, /* cacheResults */true);
			}
		}
		else {//传来的就是一个数组
			var result = [];
			this._darwingList = null; //初始化待绘制数据列表

			for (var i = 0; i < this._dataList.length; i++) {//循环全部的数据
				var compareFlag = false; //是否匹配,匹配则添加到待绘制列表
				if (this._dataList[i].indexOf(this._currentPrefix) == 0) {
					compareFlag = true;
				}
				if (compareFlag) {//如果该项目是匹配的项目则添加
					fiRecordCount++;
					if (this._maxCompletionRecordCount < 0 || fiRecordCount <= this._maxCompletionRecordCount)//在限定的数量范围内
						Array.insert(result, result.length, this._dataList[i]);
					else
						break;
				}
			}

			if (result && result.length > 0) {
				this._showCompletionList(this._currentPrefix, result, /* cacheResults */true);
			}
		}
	},

	/// <summary>
	/// 鼠标移动到选择项目上时的操作
	/// </summary>
	_item_onmouseover: function (e) {
		this._highlightItem(e.target.indexValue);
		this._selectIndex = e.target.indexValue;
		e.stopPropagation();
	},

	//当选择项目被点击的时候的操作
	_item_onclick: function (e) {
		var elt = this.get_element(); //得到输入框的对象 

		if (this._raiseItemSelected(e.target.value, this._drawingList[e.target.indexValue])) {

			//elt.value = e.target.innerText;

			if (null != elt.tagName && elt.tagName == "SPAN") {
				this._replaceLastInputText(e.target.innerText);
			}
			else {
				elt.value = e.target.innerText;
			}
		}

		if (null != elt.tagName && elt.tagName == "SPAN") {
			this._text = this._innerTextFilter(elt.innerText); //设置当前文本
		}
		else {
			this._text = elt.value; //设置当前文本
		}

		this._selectValue = e.target.value; //设置当前的Value值
		this._selectIndex = e.target.indexValue; //这个是索引值，只是在当前数据源中的

		this._originalText = this._text;
		this._hideCompletionList();
	},

	/// <summary>
	/// 得到焦点时的处理
	/// </summary>
	_onGotFocus: function (ev) {
		this._timer.set_enabled(true);
	},

	/// <summary>
	/// 如果有滚动条，则设置位置以保证用远可以看到当前高亮的项目
	/// 他很无耻，我很无奈……
	/// </summary>
	_setScroll: function () {
		var item = this._AutoCompletePopup.get_popupDocument().body.document.all("div_Item_" + this._selectIndex); //得到当前选择的项目
		var listPanl = this._AutoCompletePopup.get_popupDocument().body.document.all('listPanl'); //得到当前的框架Div
		if (item && (item.offsetTop + item.offsetHeight) > (listPanl.scrollTop + this._maxPopupWindowHeight)) {//如果选择项目的下边界超出了当前显示的下边界,这个是向下选的时候
			listPanl.scrollTop += (item.offsetTop + item.offsetHeight) - (listPanl.scrollTop + this._maxPopupWindowHeight);
		}
		else if (item && item.offsetTop < listPanl.scrollTop) {//当选项的上边界超出当前显示的区域的上边界，这个是向上选择的处理
			listPanl.scrollTop -= listPanl.scrollTop - item.offsetTop;
		}
	},

	/// <summary>
	/// 设置高亮项目
	/// </summary>
	_highlightItem: function (newSelectIndex) {
		if (this.get_showFlag()) {//如果窗口有显示
			var nowItem = null; //当前的项目
			var newItem = this._AutoCompletePopup.get_popupDocument().body.document.all("div_Item_" + newSelectIndex); //新的项目

			if (this._selectIndex > -1) {//去除当前的高亮显示
				nowItem = this._AutoCompletePopup.get_popupDocument().body.document.all("div_Item_" + this._selectIndex); //当前的项目
				if (this._itemHoverCssClass != "") {//如果设置了Style
					Sys.UI.DomElement.removeCssClass(nowItem, this._itemHoverCssClass);
				}
				else {
					Sys.UI.DomElement.removeCssClass(nowItem, "AutoComplete_ContextMenuItemhover");
					Sys.UI.DomElement.addCssClass(nowItem, "AutoComplete_ContextMenuItem");
				}

				if (this._itemCssClass != "") {
					Sys.UI.DomElement.addCssClass(nowItem, this._itemCssClass);
				}
				else {
					Sys.UI.DomElement.addCssClass(newItem, "AutoComplete_ContextMenuItem");
					//                    nowItem.style.color = this.get_itemFontColor();
					//                    nowItem.style.backgroundColor = "#FFFFFF";
					//                    nowItem.style.borderColor = "#FFFFFF"
				}
			}

			//设置新的高亮项目
			if (this._itemHoverCssClass != "") {//如果设置了Style
				Sys.UI.DomElement.addCssClass(newItem, this._itemHoverCssClass);
				//                newItem.style.color = "";
				//                newItem.style.backgroundColor = "";
				//                newItem.style.borderColor = "";
			}
			else {
				Sys.UI.DomElement.removeCssClass(newItem, "AutoComplete_ContextMenuItem");
				Sys.UI.DomElement.addCssClass(newItem, "AutoComplete_ContextMenuItemhover");

				//                newItem.style.color = this.get_itemHoverFontColor();
				//                newItem.style.borderColor = this.get_itemHoverBackgroundColor();
				//                newItem.style.backgroundColor = this.get_itemHoverBackgroundColor();                
			}

			//设置当前选择的索引值
			this._selectIndex = newSelectIndex;

			this._setScroll(); //设置滚动条
		}
	},

	/// <summary>
	/// 按下键盘的处理
	/// </summary>
	_onKeyDown: function (ev) {
		var k = ev.keyCode ? ev.keyCode : ev.rawEvent.keyCode;
		if (k === Sys.UI.Key.esc) {//ESC
			this._hideCompletionList();
			ev.preventDefault();
		}
		else if (k === Sys.UI.Key.up) {//向上箭头,按下后下移一个项目，如果最后一个则回到第一个
			var nowSelectIndex;
			if (this.get_drawingList() != null && this.get_drawingList().length > 0) {
				if (this._selectIndex > 0) {
					nowSelectIndex = this._selectIndex - 1;
				}
				else {
					nowSelectIndex = this.get_drawingList().length - 1;
				}

				this._highlightItem(nowSelectIndex);
			}
			ev.preventDefault();
		}
		else if (k === Sys.UI.Key.down) {//向下箭头,按下后上移一个项目，如果第一个则回到最后一个
			var nowSelectIndex;
			if (this.get_drawingList() != null && this.get_drawingList().length > 0) {
				if (this._selectIndex < (this.get_drawingList().length - 1)) {
					nowSelectIndex = this._selectIndex + 1;
				}
				else {
					nowSelectIndex = 0;
				}

				this._highlightItem(nowSelectIndex);
			}
			ev.preventDefault();
		}
		else if (k === Sys.UI.Key.enter) {//回车，选择当前项目值

			var needReturn = false; //是否需要出发默认的回车，如果在弹出选择的时候按回车为选择项目，没有弹出则默认的回车，该提交提交，该咋地咋地
			//this._text = this.get_element().value;//保存值
			if (null != this.get_element().tagName && this.get_element().tagName == "SPAN") {
				this._text = this._innerTextFilter(this.get_element().innerText);
			}
			else {
				this._text = this.get_element().value; //保存值
			}

			needReturn = !this._showFlag;
			if (this._selectIndex !== -1) {
				nowItem = this._AutoCompletePopup.get_popupDocument().body.document.all("div_Item_" + this._selectIndex); //当前的项目
				nowItem.click();
				ev.preventDefault();
			}

			return needReturn;
		}
		else if (k === Sys.UI.Key.tab) {//Tab
			if (this._selectIndex > -1) {
				nowItem = this._AutoCompletePopup.get_popupDocument().body.document.all("div_Item_" + this._selectIndex); //当前的项目
				//nowItem.click;
			}
		}
		else if (k === Sys.UI.Key.left || k === Sys.UI.Key.right) {
			return;
		}
		else {//其他
			this._hideCompletionList();
			Sys.UI.DomElement.removeCssClass(this.get_element(), this._errorCssClass);
			this._selectValue = ""; //在输入框中手工输入信息，清空当前的Value值
			this._timer.set_enabled(false);
			this._timer.set_enabled(true);
		}
	},

	/// <summary>
	/// 失去焦点时的处理
	/// </summary>
	_onLostFocus: function () {
		this._timer.set_enabled(false);
		this._hideCompletionList();

		if (null != this.get_element().tagName && this.get_element().tagName == "SPAN") {
			this._text = this._innerTextFilter(this.get_element().innerText);
			this._currentPrefix = this.get_element().innerText;
		}
		else {
			this._text = this.get_element().value;
			this._currentPrefix = this.get_element().value;
		}

		//if (this._requireValidation && this._originalText != this._text) {//如果需要验证则执行验证方法
		if (this._requireValidation) {//如果需要验证则执行验证方法
			this._validate();
		}

		this._originalText = this._text;
	},

	applyTextChange: function () {
		this._text = "";
		var element = this.get_element();

		if (element.tagName == "INPUT")
			this._text = element.value;
		else
			this._text = element.innerText;

		this._originalText = this._text;
	},

	/// <summary>
	/// 隐藏自动完成框
	/// </summary>
	_hideCompletionList: function () {
		if (this._AutoCompletePopup && this.get_showFlag()) {
			this._AutoCompletePopup.hide();
		}
		this.set_showFlag(false);
	},

	/// <summary>
	/// Invoke调用成功后的处理
	/// </summary>
	_onMethodComplete: function (result, context, methodName) {
		this._isInvoking = false;
		//if (result != null && result.length > 0)
		if (result != null) {
			if (result.length > 0)
				this._showCompletionList(context, result, /* cacheResults */true);
			else
				this._validateInput(result);
		}

		this._waitingImageElement.style.display = "none";
		this._checkImageElement.style.display = "inline";
	},

	_onCallBackPageMethodError: function () {
		this._isInvoking = false;

		this._waitingImageElement.style.display = "none";
		this._checkImageElement.style.display = "inline";
	},

	/// <summary>
	/// 加载ClientState
	///     ClientState中保存的是一个长度为2的一维数组
	///         第一个为输入框中的文本
	///         第二个为选中项目的Value，如果手工输入不是选择则为 空
	///         第三个为DataList数据源
	/// </summary>
	/// <param name="clientState">序列化后的clientState</param>
	loadClientState: function (value) {
		if (value) {
			var elt = this.get_element(); //得到输入框的对象
			var fsArray = Sys.Serialization.JavaScriptSerializer.deserialize(value);

			if (fsArray && fsArray.length > 0) {
				this._text = fsArray[0];
				if (fsArray.length > 1 && fsArray[1]) {
					this._selectValue = fsArray[1];
				}
				else {
					this._selectValue = "";
				}

				if (fsArray.length > 2 && fsArray[2]) {
					this._dataList = fsArray[2];
				}
				else {
					this._dataList = null;
				}

				if (fsArray.length > 3 && fsArray[3]) {
					this._eventContext = fsArray[3];
				}
				else {
					this._eventContext = null;
				}
			}
			else {
				this._text = "";
				this._selectValue = "";
				this._dataList = null;
				this._eventContext = null;
			}

			//elt.value = this._text;
			//this._currentPrefix = elt.value;

			if (null != elt.tagName && elt.tagName == "SPAN") {
				//                this._replaceLastInputText(this._text);
				//              this._currentPrefix = this._innerTextFilter(elt.innerText);
			}
			else {
				//elt.value = this._text;
				this._originalText = elt.value;
				this._text = this._originalText;
				this._currentPrefix = elt.value;
			}
		}
	},

	/// <summary>
	/// 保存ClientState
	///     ClientState中保存的是一个长度为3的一维数组
	///         第一个为输入框中的文本
	///         第二个为选中项目的Value，如果手工输入不是选择则为 String.Empty
	///         第三个为DataList数据源
	/// </summary>
	/// <returns>序列化后的CLientState字符串</returns>
	saveClientState: function () {
		//var fsCS = {this.get_selectIndex(), this.get_text()};
		var fsCS = new Array(3);
		fsCS[0] = this._text;
		fsCS[1] = this._selectValue;
		fsCS[2] = this._dataList;
		fsCS[3] = this._eventContext;
		return Sys.Serialization.JavaScriptSerializer.serialize(fsCS);
	},

	/// <summary>
	/// 验证输入的合法性
	/// 当输入信息而不是选择自动完成项目的时候执行此验证
	/// 在数据源中判断输入的内容是否在制定的验证字段中有完全匹配的数据，如果没有则为验证失败
	/// 如果有多个匹配项则只匹配到第一个。后面的自动忽略。并格式化文本框中的文本为制定的Text
	/// </summary>
	_validate: function () {
		if (this._isInvoking)
			return;

		var text = "";

		if (null != this.get_element().tagName && this.get_element().tagName == "SPAN") {
			text = this._innerTextFilter(this.get_element().innerText);
		}
		else {
			text = this.get_element().value;
		}

		if (this._autoCallBack) {//是否存在回调的方法,存在则回调取数据
			if (text != "" && this.get_autoValidateOnChange()) {
				this._invokeRemoteMethod(this._onValidateInvokeComplete);
			}
			else {
				this._validateInput([]);
			}
		}
		else if (this._dataList) {//不存在回调的方法则判断是否有客户端数据源，如果有则从客户端取数据
			this._validateInClientDataSource();
		}

		this._originalText = this._text;
	},

	_invokeRemoteMethod: function (completeMethod) {
		this.raiseBeforeInvoke();

		this._staticInvoke("CallBackPageMethod", [this._currentPrefix, this._maxCompletionRecordCount],
					    Function.createDelegate(this, completeMethod),
					    Function.createDelegate(this, this._onCallBackPageMethodError));
		//this._invoke("CallBackPageMethod", [this._currentPrefix, this._maxCompletionRecordCount], Function.createDelegate(this, completeMethod),
		//					Function.createDelegate(this, this._onCallBackPageMethodError));

		this._isInvoking = true;
		this._waitingImageElement.style.display = "inline";
		this._checkImageElement.style.display = "none";
	},

	/// <summary>
	/// 验证数据时invoke服务端方法成功后的操作
	/// </summary>
	_onValidateInvokeComplete: function (result) {
		this._isInvoking = false;
		this._waitingImageElement.style.display = "none";
		this._checkImageElement.style.display = "inline";
		this._validateInput(result);
	},

	/// <summary>
	/// 从客户端数据源验证数据
	/// </summary>
	_validateInClientDataSource: function () {
		this._validateInput(this._dataList);
	},

	/// <summary>
	/// 验证输入的内容
	/// </summary>
	_validateInput: function (result) {
		if (this._raiseValueValidated(this.get_element(), result)) {
			var typeError = true; //记录是否验证失败
			if (result && result.length > 0) {//如果得到了结果则开始处理
				if (typeof (result[0]) == "object") {//传过来的是一个对象
					for (var i = 0; i < result.length; i++) {//循环所有数据
						for (var k = 0; k < this._compareFieldName.length; k++)//循环所有的匹配字段
						{
							var fsShowTextString = ""; //显示的文本
							if (this.get_dataTextFieldList() && this.get_dataTextFieldList().length > 0) {//指定了显示的字段，则显示指定内容
								var arrayList = new Array(this.get_dataTextFieldList().length); //定义数组，保存字段的具体内容
								for (var j = 0; j < this.get_dataTextFieldList().length; j++) {//循环指定的字段
									arrayList[j] = result[i][this.get_dataTextFieldList()[j]]; //数据数组，如果有格式用来进行格式化
									fsShowTextString += result[i][this.get_dataTextFieldList()[j]]; //先全拼上，如果有格式再覆盖，没有格式就可以直接输出了
								}
								if (this.get_dataTextFormatString() && this.get_dataTextFormatString() != "") {//如果指定了格式，则匹配
									Array.insert(arrayList, 0, this.get_dataTextFormatString());
									fsShowTextString = String.format.apply(String, arrayList)//格式化字符串
								}
							}
							else {//没有指定显示的字段则为显示全部数据
								for (var fsField in result[i]) {//遍历dataList中的每一个字段
									arrayList[i] = result[i][fsField];
									fsShowTextString += result[i][fsField];
								}
							}

							if (result[i][this._compareFieldName[k]] == this._text || fsShowTextString == this._text) {//如果匹配到数据
								typeError = false; //设定为没有错误

								//this.get_element().value = fsShowTextString;//设置显示内容
								if (null != this.get_element().tagName && this.get_element().tagName == "SPAN") {
									this._replaceLastInputText(fsShowTextString); //设置显示内容
								}
								else {
									this.get_element().value = fsShowTextString; //设置显示内容
								}
								this._text = fsShowTextString; //设置text
								this._selectIndex = -1;
								if (this.get_dataValueField() && this.get_dataValueField() != "") {//如果指定了Value字段
									this._selectValue = result[i][this.get_dataValueField()]; //设置value值
								}
								break; //跳出循环
							}
						}
						//如果检测到了数据就是说没有错误，直接跳出
						if (!typeError) {
							break;
						}
					}
				}
				else {//传过来的是一个数组
					//现在只处理字符串的一维数组
					for (var i = 0; i < result.length; i++) {
						if (result[i] == this._text) {//找到了匹配项目
							//this.get_element().value = result[i];
							if (null != this.get_element().tagName && this.get_element().tagName == "SPAN") {
								this._replaceLastInputText(result[i]);
							}
							else {
								this.get_element().value = result[i];
							}
							this._text = result[i];
							this._selectValue = "";
							this._selectIndex = -1;
							typeError = false;
							break;
						}
					}
				}
			}

			if (typeError) {
				this._setErrorStyle(); //设置出错风格
				var elt = this.get_element();
				if (elt.onTypeError) {//如果文本框由onTypeError的JS代码，则执行
					eval(elt.onTypeError);
				}
			}
		}
	},

	/// <summary>
	/// 设置出错风格
	/// </summary>
	_setErrorStyle: function () {
		Sys.UI.DomElement.addCssClass(this.get_element(), this._errorCssClass);
	},

	_propertyChange: function (e) {
		if (e.rawEvent.propertyName == "readOnly" || e.rawEvent.propertyName == "disabled")
			this.set_showCheckImage(!e.handlingElement[e.rawEvent.propertyName]);
	},

	//设置文本是否变化标示
	_setTextChanged: function (e) {
		this._currentPrefix = this.get_element().innerText;
		if (!this._isTextChanged) {
			if (this._text != this._currentPrefix) {
				this._isTextChanged = true;
			}
		}
	},

	RI: function () {
	}
}

$HGRootNS.AutoCompleteExtender.registerClass($HGRootNSName + ".AutoCompleteExtender", $HGRootNS.BehaviorBase);

$HGRootNS.AutoCompleteControl = function (element) {
	$HGRootNS.AutoCompleteControl.initializeBase(this, [element]);

	this._targetControlClientID = null;
	this._autoCompleteExtenderClientID = null;

	this._value = "";
	this._text = this.get_targetControlText();
}

$HGRootNS.AutoCompleteControl.prototype = {

	initialize: function () {
		$HGRootNS.AutoCompleteControl.callBaseMethod(this, "initialize");

		this.get_extender().add_itemSelected(Function.createDelegate(this, this._onItemSelected));
		this.get_extender().add_valueValidated(Function.createDelegate(this, this._onValidated));


	},

	dispose: function () {
		$HGRootNS.AutoCompleteControl.callBaseMethod(this, "dispose");
	},

	get_targetControlClientID: function () {
		return this._targetControlClientID;
	},

	set_targetControlClientID: function (value) {
		this._targetControlClientID = value;
	},

	get_autoCompleteExtenderClientID: function () {
		return this._autoCompleteExtenderClientID;
	},

	set_autoCompleteExtenderClientID: function (value) {
		this._autoCompleteExtenderClientID = value;
	},

	get_extender: function () {
		var result = null;

		if (this._autoCompleteExtenderClientID != null && this._autoCompleteExtenderClientID != "")
			result = $find(this._autoCompleteExtenderClientID);

		return result;
	},

	get_targetControl: function () {
		var result = null;

		if (this._targetControlClientID != null && this._targetControlClientID != "")
			result = $get(this._targetControlClientID);

		return result;
	},

	_onItemSelected: function (extender, e) {
		var selectedObject = e.selectedObject;

		this.set_selectedObject(selectedObject);

		this.raiseValueChanged(selectedObject);

		e.cancel = true;
	},

	_onValidated: function (extender, e) {
		var selectedObject = { Value: "", Text: "" };
		var raiseEvent = false;

		if (e.dataList.length > 0) {
			selectedObject = e.dataList[0];
			raiseEvent = true;
		}
		else {
			selectedObject.Value = this._value;
			selectedObject.Text = this._text;
		}

		this.set_selectedObject(selectedObject);

		if (raiseEvent)
			this.raiseValueChanged(selectedObject);

		e.cancel = true;
	},

	get_targetControlText: function () {
		var result = "";
		var targetControl = this.get_targetControl();

		if (targetControl) {
			if (targetControl.tagName == "INPUT")
				result = targetControl.value;
			else
				result = targetControl.innerText;
		}

		return result;
	},

	set_targetControlText: function (value) {
		var targetControl = this.get_targetControl();

		if (targetControl) {
			if (targetControl.tagName == "INPUT")
				targetControl.value = value;
			else
				targetControl.innerText = value;
		}
	},

	get_value: function () {
		return this._value;
	},

	set_value: function (value) {
		this._value = value;
	},

	get_text: function () {
		return this._text;
	},

	set_text: function (value) {
		this._text = value;
	},

	set_selectedObject: function (selectedObject) {
		this.set_targetControlText(selectedObject.Text);
		this.get_extender()._selectValue = selectedObject.Value;

		this._value = selectedObject.Value;
		this._text = selectedObject.Text;
	},

	saveClientState: function () {
		var state = [this.get_value(), this.get_text()];

		return Sys.Serialization.JavaScriptSerializer.serialize(state);
	},

	add_valueChanged: function (handler) {
		this.get_events().addHandler("valueChanged", handler);
	},

	remove_valueChanged: function (handler) {
		this.get_events().removeHandler("valueChanged", handler);
	},

	raiseValueChanged: function (object) {
		var handlers = this.get_events().getHandler("valueChanged");

		if (handlers) {
			var e = new Sys.EventArgs();

			e.selectedObject = object;

			handlers(this, e);
		}
	},

	RI: function () {
	}
}

$HGRootNS.AutoCompleteControl.registerClass($HGRootNSName + ".AutoCompleteControl", $HGRootNS.ControlBase);