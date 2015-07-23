function HighChartPack() {
    this.AllData = [];
    this.title = undefined;//名称
    this.namelist = [];//数据的字段名List（如：CPU）
    this.model = [];
    this.catelist = [];
}

HighChartPack.prototype.Init = function (NameList, model, catelist, title) {
    this.model = eval(model);
    this.title = title;
    this.namelist = NameList;
    this.catelist = catelist;
    this.CreateData();
    var checkbox = new CreateCheckBtn();
    checkbox.Init(this.namelist);
    this.CreateBtn();
}

HighChartPack.prototype.CreateBtn = function () {
    $("#ChartBtn").html("");
    var Pack = this;
    var CreateChart = document.createElement("Input");
    CreateChart.type = "button";
    CreateChart.id = 'CreateChart';
    CreateChart.value = "创建图表";
    CreateChart.className = "btn1";
    CreateChart.onclick = function () { Pack.CreateNewChart() };
    $("#ChartBtn").append(CreateChart);
}

HighChartPack.prototype.CreateNewChart = function () {
    var gList = $("[name=GroupList]:checked");
    var data = [];
    this.categorylist = [];
    for (var g = 0; g < gList.length; g++) {
        data[gList[g].value] = { name: gList[g].value, data: [] };
        data[gList[g].value].data = this.AllData[gList[g].value];
    }
    this.FillChart(data);
}

HighChartPack.prototype.CreateData = function () {
    var data = this.model;
    for (var i = 0; i < data.length; i++) {
        var tr = data[i];
        if(this.AllData[tr["Tkey"]]==undefined)
        {
            this.AllData[tr["Tkey"]] = [];
        }
        this.AllData[tr["Tkey"]].push(tr["Avalue"]);
        this.AllData[tr["Tkey"]].push(tr["Bvalue"]);
    }
}

HighChartPack.prototype.FillChart = function (data) {
    var sees = [];
    for (var i in data) {
        sees.push(data[i]);
    }
    $('#maychart').highcharts({
        yAxis: {
            min: 0,
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }]
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle',
            borderWidth: 0
        },
        tooltip: {
            crosshairs: true,
            shared: true
        },
        title: {
            text: this.title,
            x: -20
        },
        subtitle: {
            text: this.title,
            x: -20
        },
        plotOptions: {
            series: {
                cursor: 'pointer',
            }
        },
        xAxis: {
            categories: this.catelist
        },
        series: sees
    });
}

//============================================================
function CreateCheckBtn() {
    this.GroupList = [];
}

CreateCheckBtn.prototype.Init = function (NameList) {
    this.NameList = NameList;
    this.CreateNameCheck();
}

CreateCheckBtn.prototype.CreateNameCheck = function () {
    $("#NameList").html("");
    var it = this;
    var Listr = "";
    for (var i in this.NameList) {
        var sel = "";
        if (this.NameList[i].value == $("#key").val()) {
            sel = "checked=checked";
        }
        Listr += "<li><input type='radio' name='NameList' " + sel + " onchange='ChangeRadio(this.value)' value='" + this.NameList[i].value + "' /><label>" + this.NameList[i].name + "</label></li>";
    }
    $("#NameList").append("<ul>" + Listr + "</ul>");
}

function ChangeRadio(value) {
    $("#key").val(value);
    $("#searchForm").submit();
}


function CheckAll()
{
    $("[name=GroupList]").prop("checked", $("#CheckAll").prop("checked"));
}