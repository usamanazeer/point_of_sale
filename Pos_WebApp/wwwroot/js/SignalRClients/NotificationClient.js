//"use strict";

//var hubUrl = `http://localhost:5000/notificationHub?UserId=${PosUser.UserId}&RoleId=${PosUser.RoleId}&CompanyId=${PosUser.CompanyId}`;
//var connection = new signalR.HubConnectionBuilder().withUrl(hubUrl).build();
//Object.defineProperty(WebSocket, 'OPEN', { value: 1, });
//async function start() {
//    try {
//        await connection.start();
//        console.log("SignalR Connected.");
//        //SendGroupNotification(`company:${PosUser.CompanyId}__role:${PosUser.RoleId}`, PosUser, "Hello Users");
//    } catch (err) {
//        console.log("SingnalR Error: ",err);
//        setTimeout(start, 5000);
//    }
//};
//connection.onclose(start);

//// Start the connection.
//start();
//function SendGroupNotification(groupName, sender, message) {
//    try {
//        connection.invoke("SendGroupNotification", groupName, JSON.stringify(sender), message);
//    } catch (err) {
//        console.log("SingnalR Error: ", err);
//    }
//};



//connection.on("ReceiveGroupNotification", function (sender, message) { 
//    console.log("Sender: ", sender );
//    console.log("Message: ", message);
//    //console.log("Message: ", message);
//}); 
