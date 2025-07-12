"use strict";

var hubUrl = `http://localhost:5000/salesHub`;
var connection = new signalR.HubConnectionBuilder().withUrl(hubUrl).build();
Object.defineProperty(WebSocket, 'OPEN', { value: 1, });
async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected to salesHub.");
    } catch (err) {
        console.log("SingnalR Error: ", err);
        setTimeout(start, 5000);
    }
};
connection.onclose(start);

// Start the connection.
start();


connection.on("SalesOccured", function () {
    try {
        LoadLastWeekSalesReport();
        LoadLastSixMonthsSalesReport();
    } catch (e) {

    }

});
//document.getElementById("sendButton").addEventListener("click", function (event) {

//    

//});