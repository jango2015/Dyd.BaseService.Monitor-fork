function CreateChartStream(model, namelist) {
    this.namelist = [];
    this.model = [];
    this.title = "";
    this.node = "";
    this.category = [];
    this.xseries = [];
    this.len = 0;
    this.pointIndex = 0;
    this.retio = 10;
    this.layer = 0;
}
//初始化
CreateChartStream.prototype.Init = function (model, node, namelist, title) {
    this.namelist = namelist;
    this.model = model;
    this.title = title;
    this.node = node;
    this.CheckDataLen();
    this.CreateData(0, this.len);
    this.FillChart();
    this.CreateBtn();
    this.CreateHtml();
}
//检测数据的量
CreateChartStream.prototype.CheckDataLen = function () {
    this.len = this.model.length;
    if (this.len > 1000) {
        this.len = 1000;
        alert("数据量大于1000行,请合理选择范围");
    }
    this.layer = this.len / 10;
}
//处理数据
CreateChartStream.prototype.CreateData = function (index, len) {
    this.xseries = [];
    for (var i in this.namelist) {
        this.xseries.push({
            name: this.namelist[i],
            data: []
        });
    }
    this.category = [];
    for (var i = index; i < len; i++) {
        var tr = this.model[i];
        this.category.push(i);
        for (var k in this.namelist) {
            this.xseries[k].data.push({ name: tr[this.node], y: tr[this.namelist[k]] });
        }
    }
}
//创建按钮
CreateChartStream.prototype.CreateBtn = function () {
    var btn = document.createElement("input");
    var it = this;
    btn.type = "button";
    btn.onclick = function () { it.CreateInit() };
    btn.className = "btn1";
    btn.value = "恢复";
    $("#ChartBtn").html("").append(btn);
}
//画图
CreateChartStream.prototype.FillChart = function (category) {
    $("#Retio").val(this.layer);
    var it = this;
    var sees = [];
    $("#maychart").html("");
    for (var i in this.xseries) {
        sees.push(this.xseries[i]);
    }
    $('#maychart').highcharts({
        title: {
            text: this.title,
            x: -20
        },
        subtitle: {
            text: this.title,
            x: -20
        },
        xAxis: {
            categories: category==undefined?"":category
        },
        yAxis: {
            title: {
                text: ""
            },
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
        plotOptions: {
            series: {
                cursor: 'pointer',
                point: {
                    events: {
                        click: function (e) {
                            if (confirm("查询该SQL详情？")) {
                                window.location.href = "/TimeWatchCustomer/TimeWatchLog/Chart?key=" + e.point.name + "&type=1";
                            }
                            else {
                                it.pointIndex = e.point.category;
                                it.layer = 1;
                                it.Amplification(1);
                            }
                        }
                    }
                },
            }
        },
        tooltip: {
            crosshairs: true,
            shared: true
        },
        series: sees
    });
}
//恢复最初的图
CreateChartStream.prototype.CreateInit = function () {
    this.layer = this.len / 10;
    this.CreateData(0, this.len);
    this.FillChart();
}
//放大指定位置
CreateChartStream.prototype.Amplification = function (layer) {
    var startindex = 0;
    var endindex = this.len;
    var pointindex = this.pointIndex;
    if (pointindex > layer * 10) {
        startindex = pointindex - layer * 10;
    }
    if ((pointindex + layer * 10) < this.len) {
        endindex = pointindex + layer * 10
    }
    this.CreateData(startindex, endindex);
    this.FillChart(this.category);
    this.AmplificationRetio
}

CreateChartStream.prototype.CreateHtml = function () {
    var it = this;
    var addBtn = document.createElement("input"); addBtn.className = "btn1"; addBtn.type = "button"; addBtn.value = "缩小"; addBtn.onclick = function () { it.AddRetio(); }
    var subBtn = document.createElement("input"); subBtn.className = "btn1"; subBtn.type = "button"; subBtn.value = "放大"; subBtn.onclick = function () { it.SubRetio(); }
    var text = document.createElement("input"); text.type = "text"; text.id = "Retio"; text.value = this.layer; text.onkeyup = function () { if (this.value > it.len / 10) { this.value = it.len / 10 } else if (this.value < 0) { this.value = 0; } }; text.onblur = function () { it.ChangeRetio() };
    $("#ChartAmp").append(addBtn);
    $("#ChartAmp").append(text);
    $("#ChartAmp").append(subBtn);
}

CreateChartStream.prototype.AddRetio = function () {
    if (this.layer*10 < this.len) {
        this.layer++;
        this.Amplification(this.layer);
    }
}

CreateChartStream.prototype.ChangeRetio = function () {
    if ($("#Retio").val() != this.layer) {
        this.layer = $("#Retio").val();
        this.Amplification(this.layer);
    }
}

CreateChartStream.prototype.SubRetio = function () {
    if (this.layer * 10 > 10) {
        this.layer--;
        this.Amplification(this.layer);
    }
}