// -------------------------------------------------
// Assembly	：	MCS.Web.WebControls
// FileName	：	WordPrint.js
// Remark	：  Word打印的客户端脚本
// -------------------------------------------------
// VERSION  	AUTHOR		DATE			CONTENT
// 1.0		张曦	    20070815		创建
// 1.0	    徐文卓	    20090112		修改：打印字体颜色问题，、无法带修订的内容。、增加打印时可调整字体颜色和大小功能、打印完毕才显示结果。
// -------------------------------------------------
$HGRootNS.WordPrint = function(element) 
{
    $HGRootNS.WordPrint.initializeBase(this, [element]);
    this._type = 0; //按钮的类型
    this._text = ""; //按钮上的文本
    this._templeteUrl = ""; //Word文档模板
    this._imageUrl = ""; //ImageButton时的按钮图片
    this._cssClass = ""; //按钮的CSS类名
    this._dataSourceList = null; //数据源集合
    this._onBeforeDataSourceItemCreate = ""; //当一个部分在Word文档中创建前，触发的客户端事件
    this._onDataSourceItemCreated = ""; //当一个部分在Word文档中创建后，触发的客户端事件
    this._onCreateWordComplete = ""; //当Word文档中创建完毕后，触发的客户端事件
    this._accessKey = "";//热键
    this._wordApp = null;
    this._wordDoc = null;
    this._splitWord = "__";//书签中的分割符号
    this._localPath = '';
    this._onPrintClick = Function.createDelegate(this, this._onPrintButtonClick); //单击事件
}

//---------------------------------------我就素那无耻的分割线---------------------------------------//

$HGRootNS.WordPrint.prototype =
{
	//按钮的类型
	get_type: function () {
		return this._type;
	},
	set_type: function (value) {
		this._type = value;
	},

	get_accessKey: function () {
		return this._accessKey;
	},
	set_accessKey: function (value) {
		this._accessKey = value;
	},

	//按钮上的文本
	get_text: function () {
		return this._text;
	},
	set_text: function (value) {
		this._text = value;
	},

	//Word文档模板
	get_templeteUrl: function () {
		return this._templeteUrl;
	},
	set_templeteUrl: function (value) {
		this._templeteUrl = value;
	},

	//ImageButton时的按钮图片
	get_imageUrl: function () {
		return this._imageUrl;
	},
	set_imageUrl: function (value) {
		this._imageUrl = value;
	},

	//按钮的CSS类名
	get_cssClass: function () {
		return this._cssClass;
	},
	set_cssClass: function (value) {
		this._cssClass = value;
	},

	//数据源集合
	get_dataSourceList: function () {
		return this._dataSourceList;
	},
	set_dataSourceList: function (value) {
		this._dataSourceList = value;
	},

	//当一个部分在Word文档中创建前，触发的客户端事件
	get_onBeforeDataSourceItemCreate: function () {
		return this._onBeforeDataSourceItemCreate;
	},
	set_onBeforeDataSourceItemCreate: function (value) {
		this._onBeforeDataSourceItemCreate = value;
	},

	//当一个部分在Word文档中创建后，触发的客户端事件
	get_onDataSourceItemCreated: function () {
		return this._onDataSourceItemCreated;
	},
	set_onDataSourceItemCreated: function (value) {
		this._onDataSourceItemCreated = value;
	},

	//当Word文档中创建完毕后，触发的客户端事件
	get_onCreateWordComplete: function () {
		return this._onCreateWordComplete;
	},
	set_onCreateWordComplete: function (value) {
		this._onCreateWordComplete = value;
	},
	//---------------------------------------我就素那无耻的分割线---------------------------------------//

	/// <summary>
	/// 控件初始化
	/// </summary>
	initialize: function () {
		$HGRootNS.WordPrint.callBaseMethod(this, 'initialize');

		$addHandler(this.get_element(), "click", this._onPrintClick);
		this._localPath = this._getTempDirName() + "\\" + this._getFileName();
	},

	/// <summary>
	/// 添加一个数据源中的项目创建前的事件
	/// </summary>
	add_beforeDataSourceItemCreate: function (handler) {
		this.get_events().addHandler("beforeDataSourceItemCreate", handler);
	},

	/// <summary>
	/// 移除一个数据源中的项目创建前的事件
	/// </summary>
	remove_beforeDataSourceItemCreate: function (handler) {
		this.get_events().removeHandler("beforeDataSourceItemCreate", handler);
	},

	/// <summary>
	/// 一个数据源中的项目创建前的事件
	/// </summary>
	_raiseBeforeDataSourceItemCreate: function (content, bookMarkName, eventElement) {
		var handlers = this.get_events().getHandler("beforeDataSourceItemCreate");
		var continueExec = true;

		if (handlers) {
			var e = new Sys.EventArgs();

			e.content = content;
			e.cancel = false;
			e.bookMarkName = bookMarkName;
			e.eventElement = eventElement;

			handlers(this, e);

			if (e.cancel)
				continueExec = false;
		}

		return continueExec;
	},

	/// <summary>
	/// 添加一个数据源中的项目创建完毕的事件
	/// </summary>    
	add_dataSourceItemCreated: function (handler) {
		this.get_events().addHandler("dataSourceItemCreated", handler);
	},

	/// <summary>
	/// 移除一个数据源中的项目创建完毕的事件
	/// </summary>
	remove_dataSourceItemCreated: function (handler) {
		this.get_events().removeHandler("dataSourceItemCreated", handler);
	},

	/// <summary>
	/// 一个数据源中的项目创建完毕的事件
	/// </summary>
	_raiseDataSourceItemCreated: function (bookMarkName, eventElement) {
		var handlers = this.get_events().getHandler("dataSourceItemCreated");
		var continueExec = true;

		if (handlers) {
			var e = new Sys.EventArgs();

			e.bookMarkName = bookMarkName;
			e.eventElement = eventElement;

			handlers(this, e);

		}

		return continueExec;
	},

	/// <summary>
	/// 添加文档创建完毕的事件
	/// </summary>
	add_createWordComplete: function (handler) {
		this.get_events().addHandler("createWordComplete", handler);
	},

	/// <summary>
	/// 移除文档创建完毕的事件
	/// </summary>
	remove_createWordComplete: function (handler) {
		this.get_events().removeHandler("createWordComplete", handler);
	},

	/// <summary>
	/// 文档创建完毕的事件
	/// </summary>
	_raiseCreateWordComplete: function (eventElement) {
		var handlers = this.get_events().getHandler("createWordComplete");
		var continueExec = true;

		if (handlers) {
			var e = new Sys.EventArgs();

			e.eventElement = eventElement;

			handlers(this, e);

		}

		return continueExec;
	},

	/// <summary>
	/// 控件销毁时执行的操作
	/// </summary>
	dispose: function () {
		Sys.UI.DomEvent.removeHandler(this.get_element(), "click", this._onPrintClick);
		this._wordApp = null;
		this._wordDoc = null;

		$HGRootNS.WordPrint.callBaseMethod(this, "dispose");
	},

	/// <summary>
	/// 单击鼠标时的处理
	/// </summary>
	_onPrintButtonClick: function () {
		this._invoke("CallBackOnPrintMethod", [], Function.createDelegate(this, this._onMethodComplete));

		return false;
	},

	GetDataSource: function () {
		this._invoke("CallBackOnPrintMethod", [], Function.createDelegate(this, this._onlySetDataSource), null, true);
	},

	_onlySetDataSource: function (result) {
		this._dataSourceList = result; //设定数据源
	},

	/// <summary>
	/// 回调成功后的处理
	/// </summary>
	/// <param name="result">回调的结果</param>
	_onMethodComplete: function (result) {
		this._dataSourceList = result; //设定数据源
		this._createWordDocument(); //生成文档
	},

	CreateDocument: function (wordApp) {
		wordDocument = wordApp.ActiveDocument;
		this._wordApp = wordApp;
		this._wordDoc = wordDocument;
		for (var i = 1; i <= wordDocument.bookmarks.count; i++)//遍历模板中的各个书签
		{
			var bookmark = wordDocument.bookmarks(i); //取得一个书签

			var typeMode = this._getTypeModeByBookmark(bookmark.name); //得到输入的类型

			if (typeMode.toLowerCase() == "table")//说明输入的是一个表格
			{
				this._createTable(bookmark);
			}
			else if (typeMode.toLowerCase() == "text")//说明输入的是一段文本
			{
				this._createTextArea(bookmark);
			}
			else if (typeMode.toLowerCase() == "file")//说明需要插入一个文件的内容
			{
				this._insertFileContent(bookmark);
			}
		}

		this._raiseCreateWordComplete(wordApp);
	},

	/// <summary>
	/// 创建Word文档内容
	/// </summary>
	_createWordDocument: function () {
		this._wordApp = this._createWordObject(); //得到Word对象
		this._closePowerWord();
		try {
			this._downloadFile(this._templeteUrl, this._localPath);
			this._wordDoc = this._wordApp.documents.open(this._localPath); //打开指定地址的模板

			this.CreateDocument(this._wordApp, this._wordDoc);
		}
		catch (e) {
			throw Error.create(e.message);
		}
		finally {
			this._wordApp.visible = true; //设置Word可见
			this._wordApp.Activate();
		}
	},

	/// <summary>
	/// 在指定的书签位置根据数据源生成一段文本
	/// </summary>
	/// <param name="bookmark">书签对象</param>
	_createTextArea: function (bookmark) {
		bookmark.select(); //选择这个书签
		//
		var bookMarkInfoList = bookmark.name.split(this._splitWord);

		if (bookMarkInfoList && bookMarkInfoList.length > 2) {
			var cData = this._getTextFromDataSourceList(bookMarkInfoList[0], bookMarkInfoList[1]);
			var content = cData.content;
			if (this._raiseBeforeDataSourceItemCreate(content, bookmark.name, this._wordApp.selection)) {

				this._wordApp.selection.Text = content;
				this._wordApp.selection.Font.Color = cData.fontColor;
				if (cData.fontSize > 0) {
					this._wordApp.selection.Font.Size = cData.fontSize;
				}
				if (typeof (bookmark) != 'unknown' && typeof (bookmark.name) != 'unknown')
					this._raiseDataSourceItemCreated(bookmark.name, this._wordApp.selection);
				this._wordApp.selection.MoveRight(1, 1); //按下向左箭头
			}
		}
	},

	/// <summary>
	/// 在指定的书签位置根据数据源中保存的路径，插入该路径指定文件的内容
	/// </summary>
	/// <param name="bookmark">书签对象</param>
	_insertFileContent: function (bookmark) {
		bookmark.select(); //选择书签

		var bookMarkInfoList = bookmark.name.split(this._splitWord); //分割书签
		if (bookMarkInfoList && bookMarkInfoList.length < 2) {
			return; //书签格式不对,返回
		}

		if (bookMarkInfoList && bookMarkInfoList.length > 2) {
			var cData = this._getTextFromDataSourceList(bookMarkInfoList[0], bookMarkInfoList[1]);
			var content = cData.content; //取得文件路径
			if (content != "" && this._raiseBeforeDataSourceItemCreate(content, bookmark.name, this._wordApp.selection)) {
				this._wordApp.selection.Font.Color = cData.fontColor;
				if (cData.fontSize > 0) {
					this._wordApp.selection.Font.Size = cData.fontSize;
				}

				var textDoc = this._wordApp.documents.open(content);
				this._wordApp.selection.wholeStory();
				this._wordApp.ActiveDocument.AcceptAllRevisions(); //接受修订。
				this._wordApp.selection.copy();
				bookmark.range.paste();
				this._wordApp.ActiveDocument.undo(); //撤销修订
				textDoc.close(0);

				//this._wordApp.selection.InsertFile(content, "", false, false, false);
				if (typeof (bookmark) != 'unknown' && typeof (bookmark.name) != 'unknown')
					this._raiseDataSourceItemCreated(bookmark.name, this._wordApp.selection);
			}
		}
	},

	/// <summary>
	/// 在指定的书签位置根据数据源生成一个表格
	/// </summary>
	/// <param name="bookmark">书签对象</param>
	_createTable: function (bookmark) {
		bookmark.select(); //选择书签

		var bookMarkInfoList = bookmark.name.split(this._splitWord); //分割书签
		if (bookMarkInfoList && bookMarkInfoList.length < 2) {
			return; //书签格式不对,返回
		}

		var tableData = this._getTableDataFromDataSourceList(bookMarkInfoList[0], bookMarkInfoList[1]); //得到表格的数据集合

		if (!tableData) {
			return; //没有数据则返回
		}

		this._wordApp.selection.EndKey(5, 1); //向后选择全部文本内容，这个文本包含了需要输出的字段，这个选择包含了回车符
		this._wordApp.selection.MoveLeft(1, 1, 1); //相当于向左按住shift移动一个字符，用途是取消回车符的选择

		var sArrFieldList = this._wordApp.selection.text.split(','); //使用半角逗号分割字段列表
		var iColCount = sArrFieldList.length; //总列数
		var iRowCount = tableData.length; //总行数

		if (!this._raiseBeforeDataSourceItemCreate(tableData, bookmark.name, this._wordApp.selection)) {
			this._wordApp.selection.text = ""; //现在的Selection选择的是需要输出的字段，所以如果取消整个表格的输入则要把这些文字清除
			return; //取消绘制表格则直接退出
		}

		this._wordApp.selection.text = "";

		this._drawTable(iRowCount, iColCount); //根据总行数绘制出表格，这时的光标停留在第一个单元格

		for (var i = 0; i < iRowCount; i++) {
			for (var k = 0; k < iColCount; k++) {
				var content = this._getData(tableData, sArrFieldList[k], i); //要输入的内容

				if (this._raiseBeforeDataSourceItemCreate(content, bookmark.name, this._wordApp.selection))//客户端事件
				{
					this._wordApp.selection.Text = content; //输入文字
					if (typeof (bookmark) != 'unknown' && typeof (bookmark.name) != 'unknown')
						this._raiseDataSourceItemCreated(bookmark.name, this._wordApp.selection);
					if (content != "")
						this._wordApp.selection.MoveRight(1, 1); //按下向左箭头
				}
				else {
					continue;
				}

				if (i != (iRowCount - 1) || k != (iColCount - 1))//不是最后一个单元格则键入Tab
				{
					this._wordApp.selection.MoveRight(12);
				}
			}
		} //END   for(var i = 0; i < sArrFieldList.length; i++)
	},

	/// <summary>
	/// 绘制一个指定行和列数量的空表格
	/// 绘制完毕后,光标停留在新增表格的第一个单元格.
	/// 同时,如果如前紧跟着已有表格下面,则会与上方表格合并
	/// </summary>
	/// <param name="iRowCount">行数量</param>
	/// <param name="iColCount">列数量</param>
	_drawTable: function (iRowCount, iColCount) {
		try {
			this._wordApp.selection.MoveUp(5, 1);
			this._wordApp.selection.InsertRowsBelow(iRowCount);
			this._wordApp.selection.MoveLeft(1, 1);
		}
		catch (e) {
			this._wordApp.ActiveDocument.Tables.Add(this._wordApp.selection.Range, iRowCount, iColCount, 1, 0);
		}
		finally {
		}
		//this._wordApp.ActiveDocument.Tables.Add(this._wordApp.selection.Range, iRowCount, iColCount, 1, 0);
	},

	/// <summary>
	/// 根据数据源名车和字段名称得到指定字段的文本
	/// </summary>
	/// <param name="dataSourceName">数据源名称</param>
	/// <param name="fieldName">字段名称</param>
	_getTextFromDataSourceList: function (dataSourceName, fieldName) {
		var fontColor;
		var fontSize;
		if (!this.get_dataSourceList()) {
			return { content: '', fontColor: 0, fontSize: 0 };
		} //如果没有数据则返回

		var textResult = "";

		for (var i = 0; i < this.get_dataSourceList().length; i++)//循环数据源集合
		{
			if (this.get_dataSourceList()[i].Name == dataSourceName)//找到了指定的DataSource
			{
				fontColor = this.get_dataSourceList()[i].ColorArgb;
				fontSize = this.get_dataSourceList()[i].FontSize;
				textResult = this.get_dataSourceList()[i].DataResult[0][fieldName];
			}
		}

		return { content: textResult, fontColor: fontColor, fontSize: fontSize };
	},

	/// <summary>
	/// 根据数据源名车和字段名称得到需要绘制TABLE的数据集合
	/// </summary>
	/// <param name="dataSourceName">数据源名称</param>
	/// <param name="fieldName">字段名称</param>
	_getTableDataFromDataSourceList: function (dataSourceName, fieldName) {
		if (!this.get_dataSourceList()) {
			return;
		} //如果没有数据则返回

		var objTableData = null;

		for (var i = 0; i < this.get_dataSourceList().length; i++)//循环数据源集合
		{
			if (this.get_dataSourceList()[i].Name == dataSourceName)//找到了指定的DataSource
			{
				objTableData = this.get_dataSourceList()[i].DataResult;
			}
		}

		return objTableData;
	},

	/// <summary>
	/// 从指定的对象中取得指定行指定字段的值
	/// </summary>
	/// <param name="dataObject">对象</param>
	/// <param name="fieldName">字段名称</param>
	/// <param name="recordIndex">指定行</param>
	_getData: function (dataObject, fieldName, recordIndex) {
		if (!dataObject) {
			return null;
		} //如果没有数据则返回

		return dataObject[recordIndex][fieldName];
	},

	/// <summary>
	/// 根据书签的名称获得输入的类型
	/// </summary>
	/// <param name="bookMarkName">书签的名字</param>
	/// <retunr>返回输入的类型，就是书签的最后一个部分</return>
	_getTypeModeByBookmark: function (bookMarkName) {
		var sArrTemp = bookMarkName.split(this._splitWord); //使用双下划线分割书签名字，最有一个部分标识书签的类型
		var result = ""; //返回值

		if (sArrTemp && sArrTemp.length > 0) {
			result = sArrTemp[sArrTemp.length - 1]; //返回最后一个部分
		}

		return result;
	},

	/// <summary>
	/// 关闭那个非常恶心、可恶、无耻、龌龊、流氓……的金山词霸的模板加载项powerword.dot,不然会出错误！
	/// </summary>
	_closePowerWord: function () {
		if (this._wordApp && this._wordApp.AddIns) {
			for (var i = 0; i < this._wordApp.AddIns.Count; i++) {
				if (this._wordApp.AddIns.Item(i + 1).Name.toLowerCase() == "powerword.dot") {
					this._wordApp.AddIns.Item(i + 1).Installed = false;
					break;
				}
			}
		}
	},

	/// <summary>
	/// 创建Word对象
	/// </summary>
	_createWordObject: function () {
		try {
			var objWord = new ActiveXObject("Word.Application");
			return objWord;
		}
		catch (e) {
			var strMsg = "您的计算机没有安装Word，或者您的浏览器为该网页没有设置本地访问权限！";
			throw strMsg;
		}
	},
	_downloadFile: function (processPageUrl, localPath) {
		this._checkParams(processPageUrl, localPath);
		try {
			var xmlHttp = this._createObject("Msxml2.XMLHTTP");
			xmlHttp.open("GET", processPageUrl, false);
			xmlHttp.send(null);

			var folderPath = localPath.substring(0, localPath.lastIndexOf("\\"));

			this._createFolder(folderPath);

			var stream = this._createObject("ADODB.Stream");
			stream.Type = 1;
			stream.Mode = 3;
			stream.Open();
			stream.Write(xmlHttp.responseBody);
			stream.saveToFile(localPath, 2);
		}
		catch (e) {
			throw Error.create("写文件失败" + localPath + "！\n可能是没有将http://"
				+ document.location.host + "加入可信站点。\n错误信息：\n" + e.message);
		}
		finally {
			stream.close();
		}
	},
	_checkParams: function (processPageUrl, localPath) {
		if (processPageUrl == null || processPageUrl == "")
			throw Error.create("请求页面地址为空");

		if (localPath == null || localPath == "")
			throw Error.create("文件路径为空");
	},
	_createFolder: function (folderName) {
		if (this._folderExists(folderName) == false) {
			var fso = this._createObject("Scripting.FileSystemObject");

			if (fso != null)
				fso.CreateFolder(folderName);
		}
	},
	_createObject: function (objectName) {
		try {
			var obj = new ActiveXObject(objectName);

			return obj;
		}
		catch (e) {
			throw Error.create(objectName + "对象创建失败！\n可能是没有将http://"
				+ document.location.host + "加入可信站点。\n错误信息：" + e.message);
		}
	},
	_getTempDirName: function () {
		var fso = this._createObject("Scripting.FileSystemObject");

		if (fso == null)
			return;

		var tempFolder = 2; //获得系统临时文件夹的系统变量

		return fso.GetSpecialFolder(tempFolder);
	},
	_getFileName: function () {
		var filename = '';
		if (this._templeteUrl != '') {
			var lastsign = this._templeteUrl.lastIndexOf('/');
			filename = this._templeteUrl.substr(lastsign + 1, this._templeteUrl.length - lastsign - 1);
		}
		return filename;
	},
	_folderExists: function (folderName) {
		var bExists = false;

		var fso = this._createObject("Scripting.FileSystemObject");

		bExists = fso.folderExists(folderName);

		return bExists;
	}
}


$HGRootNS.WordPrint.registerClass($HGRootNSName + ".WordPrint", $HGRootNS.ControlBase);