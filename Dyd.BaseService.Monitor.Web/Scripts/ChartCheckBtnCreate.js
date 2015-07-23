function CreateCheckBtn()
{
    this.categorylist = [];
    this.namelist = [];
    this.grouplist = [];
}

CreateCheckBtn.prototype.Init = function (categorylist, namelist, grouplist) {
    this.categorylist = categorylist;
    this.namelist = namelist;
    this.grouplist = grouplist;
}

CreateCheckBtn.prototype.CreateKeyCheck = function () {
    for (var i = 0; i < this.categorylist.length; i++)
    {
        $("#CategoryList").append("<input type='checkbox' id='key' value='" + this.categorylist[i] + "' /><label>" + this.categorylist[i] + "</label>");
    }
}

CreateCheckBtn.prototype.CreateValueCheck = function () {
    for (var i = 0; i < this.namelist.length; i++) {
        $("#NameList").append("<input type='checkbox' id='key' value='" + this.namelist[i] + "' /><label>" + this.namelist[i] + "</label>");
    }
}

CreateCheckBtn.prototype.CreateValueCheck = function () {
    for (var i = 0; i < this.grouplist.length; i++) {
        $("#GroupList").append("<input type='checkbox' id='key' value='" + this.grouplist[i] + "' /><label>" + this.grouplist[i] + "</label>");
    }
}