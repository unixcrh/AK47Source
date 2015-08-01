$HBRootNS.ValidatorSelectorControl = function (element)
{
    $HBRootNS.ValidatorSelectorControl.initializeBase(this, [element]);
    this._selectedValidatorsJsonValue = "";
    this._displayText = "";
}

$HBRootNS.ValidatorSelectorControl.prototype =
{
    initialize: function () {
        $HBRootNS.ValidatorSelectorControl.callBaseMethod(this, 'initialize');
    },

    dispose: function () {
        $HBRootNS.ValidatorSelectorControl.callBaseMethod(this, 'dispose');
    },

    loadClientState: function (value) {
        if (value) {
            if (value != "") {
            }
        }
    },

    saveClientState: function () {

    },

    get_selectedValidatorsJsonValue: function () {
        return this._selectedValidatorsJsonValue;
    },

    set_selectedValidatorsJsonValue: function (value) {
        this._selectedValidatorsJsonValue = value;
    },

    get_displayText: function () {
        return this._displayText;
    },

    set_displayText: function (value) {
        this._displayText = value;
    },

    get_selecotrImg: function () {
        return this._selecotrImg;
    },

    set_selecotrImg: function (value) {
        this._selecotrImg = value;
    },

    showDialog: function (arg) {
        var result = null;
        var resultStr = this._showDialog(arg);
        if (resultStr) {
            result = Sys.Serialization.JavaScriptSerializer.deserialize(resultStr);
        }
        return result;
    },

    _pseudo: function () {
    }
}

$HBRootNS.ValidatorSelectorControl.registerClass($HBRootNSName + ".ValidatorSelectorControl", $HBRootNS.DialogControlBase);