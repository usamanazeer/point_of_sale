function GetRandomColor() {
    var r = Math.floor(Math.random() * 255);
    var g = Math.floor(Math.random() * 255);
    var b = Math.floor(Math.random() * 255);
    return "rgb(" + r + "," + g + "," + b + ")";
};

function RenderSalesReportGraph(report, id) {
    //console.log(report);
    var html = `<div class="card">
        <div class="card-header">
            <h4 class="card-title"></h4>
        </div>
        <div class="card-content">
            <div class="card-body chartjs">
                <canvas id="${report.reportId}" height="100"></canvas>
            </div>
        </div>
    </div>`;
    if (report.salesDataList == null || report.salesDataList.length === 0) {
        html = `<div class="card">
        <div class="card-header">
            <h4 class="card-title"></h4>
        </div>
        <div class="card-content">
            <div class="card-body chartjs">
                <h5 class="red">No Data Found.</h5>
            </div>
        </div>
    </div>`;
        $("#" + id).html(html);
    }
    else {
        $("#" + id).html(html);
        var labels = report.labels;
        var DataSets = report.salesDataList;
        var title = "";
        var cardTitle = "";

        if (report.dateGroupByFilter.toLowerCase() == "day") {
            if (report.reportQuantityValue) {
                cardTitle = report.reportTitle ? report.reportTitle : "Sold Quantity Report By Days";
                title = "Sold Quantity/Day";
            } else {
                cardTitle = report.reportTitle ? report.reportTitle : "Sale Report By Days";
                title = "Sales/Day";
                
            }
        }
        else if (report.dateGroupByFilter.toLowerCase() == "month") {
            if (report.reportQuantityValue) {
                cardTitle = report.reportTitle ? report.reportTitle : "Sold Quantity Monthly Report";
                title = "Sold Quantity/Month";
            } else {
                cardTitle = report.reportTitle ? report.reportTitle : "Sale Monthly Report";
                title = "Sales/Month";
                
            }
        }
        else if (report.dateGroupByFilter.toLowerCase() == "year") {
            if (report.reportQuantityValue) {
                cardTitle = report.reportTitle ? report.reportTitle : "Sold Quantity Yearly Report";
                title = "Sold Quantity/Year";
            } else {
                cardTitle = report.reportTitle ? report.reportTitle : "Sale Yearly Report";
                title = "Sales/Year";
                
            }
        }
        if (report.waiterName) {
            title += `,   Waiter: ${report.waiterName}`;
        }


        var startDate = new Date(report.startDate);
        var endDate = new Date(report.endDate);
        title += `,    From: ${startDate.getDate().toString() +
            " " +
            startDate.getMonthShortName().toString() +
            ", " +
            startDate.getFullYear().toString()} to ${endDate.getDate().toString() +
            " " +
            endDate.getMonthShortName().toString() +
            ", " +
            endDate.getFullYear().toString()}`;
        if ($('#OrderType').val() != undefined && $('#OrderType').val() !== "") {
            title += `,    Order Type: ${$("#OrderType option:selected").text()}`;
        }
        if ($('#PaymentType').val() != undefined && $('#PaymentType').val() !== "") {
            title += `,    Payment Type: ${$("#PaymentType option:selected").text()}`;
        }

        var titleText = `${cardTitle}`;

        $(`#${id}>.card>.card-header>.card-title`).html(titleText);
        var dataSets = [];
        for (var i in DataSets) {
            var data = DataSets[i];
            var color = GetRandomColor();
            var dataSet = {
                label: "",
                data: [],
                borderWidth: 1,
                yAxisID: "y-axis-1",
                backgroundColor: color,
                borderColor: color,
                fill: false
            };

            for (var j = 0; j < data.length; j++) {
                if (j == 0) {
                    if (data[j].itemName == null || data[j].itemName == "") {
                        dataSet.label = "Sales";
                    } else {
                        dataSet.label = data[j].itemName;
                    }
                }
                if (report.reportQuantityValue) {
                    dataSet.data.push(data[j].totalQuantityRounded);
                    
                } else {
                    dataSet.data.push(data[j].totalSalesRounded);
                }
            }
            dataSets.push(dataSet);
        }
        var canvas = document.getElementById(report.reportId);

        var itemsSoldChart = new Chart(canvas, {
            type: report.chartType,
            data: {
                labels: labels,
                datasets: dataSets
            },
            options: {
                responsive: true,
                title: {
                    display: true,
                    text: title
                },
                legend: {
                    display: true
                },

                scales: {
                    xAxes: [{
                        stacked: true
                    }],
                    yAxes: [{
                        stacked: true,
                        type: "linear",
                        display: true,
                        position: "left",
                        id: "y-axis-1",
                        gridLines: {
                            drawOnChartArea: false
                        },
                        //ticks: {
                        //    min: 0,
                        //    max: 1000000,
                        //    stepSize: 100000,

                        //}
                    }],
                }
            }
        });
    }
}