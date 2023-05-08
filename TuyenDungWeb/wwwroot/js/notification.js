
var connection = new signalR.HubConnectionBuilder().withUrl("/adminNotificationsHub").build();

connection.on("ReceiveNotification", function (message, notificationCount, notifications) {
    console.log("Received message: " + message);
    $('.count-notification').text(notificationCount);
    notifications.forEach(function (notification) {
        $('.notification-ul').append('<a href="#"><li>' + notification + '</li></a>');
        //append notification to notification list and click to redirect to notification detail

    });
    });

connection.start();
$('.notification').click(function () {
$('.notification-list').toggle();
}
);


