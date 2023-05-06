
var connection = new signalR.HubConnectionBuilder().withUrl("/adminNotificationsHub").build();

connection.on("ReceiveNotification", function (message, notificationCount) {
    console.log("Received message: " + message);
    $('.count-notification').text(notificationCount);
    });

connection.start();

