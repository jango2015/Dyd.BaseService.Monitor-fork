function CreateMainChart()
{
    this.category = [];
    this.columns = [];
    this.data = [];
}

CreateMainChart.prototype.Init = function (data, url) {
    var i = 0;
    for (var d in data) {
        if (i == 0) {
            this.CreateColumns(data[d]);
        }
        this.category.push(d);
        var tr = data[d];
        for (var m in tr) {
            this.data[tr[m].Tkey].data.push(tr[m].Tvalue);
        }
        i++;
    }
    this.FillChart(this.data);
    this.CreateSelDate();
    this.CreateRadio();
}

CreateMainChart.prototype.CreateColumns = function (data) {
    for (var d in data) {
        this.columns.push(data[d].Tkey);
        this.data[data[d].Tkey] = { "name": data[d].Tkey, "data": [] };
    }
}

CreateMainChart.prototype.FillChart = function (data) {
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
            categories: this.category
        },
        series: sees
    });
}

CreateMainChart.prototype.CreateSelDate = function () {
    $("#Select").html("");
    var selA = document.createElement("select");
    selA.id = "SelDateOne";
    var selB = document.createElement("select");
    selB.id = "SelDateTwo";
    for (var c in this.category) {
        selA.options.add(new Option(this.category[c], this.category[c]));
        selB.options.add(new Option(this.category[c], this.category[c]));
    }
    $("#Select").append("<label>时间A：</label>");
    $("#Select").append(selA);
    $("#Select").append("<label>时间B：</label>");
    $("#Select").append(selB);
}

CreateMainChart.prototype.CreateRadio = function () {
    $("#Radio").html("");
    $("#Radio").append("<label>条件：</label>");
    $("#Radio").append("<ul></ul>");
    for (var n in this.columns) {
        $("#Radio").find("ul").append("<li><input type='radio' name='columnname' value=" + this.columns[n] + " /><label>" + this.columns[n] + "</label></li>");
    }
}

function CreateSubChart()
{
    this.name = "";
    this.category = [];
    this.columns = [];
    this.data = [];
}

CreateSubChart.prototype.Init = function (data, name) {
    this.name = name;
    for (var d in data) {
        this.data[d] = { "name": d, data: [] };
        var tr = data[d];
        for (var dr in tr) {
            this.data[d].data.push(tr[dr].Tvalue);
        }
    }
    this.CreateChart();
}

CreateSubChart.prototype.CreateChart = function () {
    var sees = [];
    for (var i in this.data) {
        sees.push(this.data[i]);
    }
    $('#ChartDetialChart').highcharts({
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
            text: this.name,
            x: -20
        },
        subtitle: {
            text: this.name,
            x: -20
        },
        plotOptions: {
            series: {
                cursor: 'pointer',
            }
        },
        xAxis: {
            categories: ""
        },
        series: sees
    });
}