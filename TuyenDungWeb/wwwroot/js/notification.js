
var connection = new signalR.HubConnectionBuilder().withUrl("/adminNotificationsHub").build();

connection.on("ReceiveNotification", function (message, notificationCount, notifications, loaiThongBao) {
    //console.log("Received message: " + message);
    //$('.count-notification').text(notificationCount);
    //notifications.forEach(function (notification) {
    //    $('.notification-ul').append('<a href="#"><li>' + notification + '</li></a>');
    //    //append notification to notification list and click to redirect to notification detail

    //});
    if (loaiThongBao == "thông báo đăng tuyển") {
        $.get('/Company/Home/GetNotificationItems', function (data) {
            var wishlistContainer = $('<ul class="notification-ul"></ul>');
            $.each(data, function (index, item) {
                var itemElement = $('<li class="notificationlist-item" data-notification-id="' + item.id + '"></li>');
                var titleElement = $('<div class="notificationlist-item__title"></div>');
                var linkElement = $('<a href="/Admin/jobposttemp/Upsert/' + item.id + '">' + item.message + '</a>');

                titleElement.append(linkElement);
                itemElement.append(titleElement);
                wishlistContainer.append(itemElement);
            });

            $('.count-notification').text(data.length);
            $('.notification-ul').replaceWith(wishlistContainer);
            var countElement = $('.count-notification');
            countElement.text(data.length);
        });
    }
    else if (loaiThongBao == "thông báo duyệt đăng tuyển") {
        $.get('/Company/Home/GetNotificationItemsForCompanyAfterApprove', function (data) {
            var wishlistContainer = $('<ul class="notification-ul"></ul>');
            $.each(data, function (index, item) {
                var itemElement = $('<li class="notificationlist-item" data-notification-id="' + item.item.id + '"></li>');
                var titleElement = $('<div class="notificationlist-item__title"></div>');
                var linkElement = $('<a href="/ViecLam/' + item.jobPost.urlHandle + '">' + item.item.message + '</a>');

                titleElement.append(linkElement);
                itemElement.append(titleElement);
                wishlistContainer.append(itemElement);
            });
            $('.count-notification').text(data.length);
            $('.notification-ul').replaceWith(wishlistContainer);
            var countElement = $('.count-notification');
            countElement.text(data.length);
        });
    }

    else if (loaiThongBao == "thông báo ứng tuyển") {
        $.get('/Company/Home/GetNotificationItemsForCompanyAfterApply', function (data) {
            var wishlistContainer = $('<ul class="notification-ul2"></ul>');
            $.each(data, function (index, item) {
                var itemElement = $('<li class="notificationlist-item" data-notification-id="' + item.id + '"></li>');
                var titleElement = $('<div class="notificationlist-item__title"></div>');
                var linkElement = $('<a href="/Company/ProfileUserApply/Update/' + item.id + '">' + item.name + ' ' + " đã ứng tuyển vào " + ' ' + item.jobTitle + '</a>');

                titleElement.append(linkElement);
                itemElement.append(titleElement);
                wishlistContainer.append(itemElement);
            });

            $('.count-notification2').text(data.length);
            $('.notification-ul2').replaceWith(wishlistContainer);
            var countElement = $('.count-notification2');
            countElement.text(data.length);
        });
    }
    else if (loaiThongBao == "thông báo duyệt ứng tuyển") {
        $.get('/Company/Home/GetNotificationItemsForCompanyAfterApproveApply', function (data) {
            var wishlistContainer = $('<ul class="notification-ul2"></ul>');
            $.each(data, function (index, item) {
                var itemElement = $('<li class="notificationlist-item" data-notification-id="' + item.id + '"></li>');
                var titleElement = $('<div class="notificationlist-item__title"></div>');
                var linkElement = $('<a href="/ViecLam/' + item.jobPost.urlHandle + '">' + item.message + '  ' + item.jobTitle + '</a>');

                titleElement.append(linkElement);
                itemElement.append(titleElement);
                wishlistContainer.append(itemElement);
            });

            $('.count-notification2').text(data.length);
            $('.notification-ul2').replaceWith(wishlistContainer);
            var countElement = $('.count-notification2');
            countElement.text(data.length);
        });
    }
    else if (loaiThongBao == "tin nhắn") {

        $('#discussion').append('<li><strong>' + htmlEncode(message)
            + '</strong>: ' + htmlEncode(notifications) + '</li>');
            $('#message').focus();
        function htmlEncode(value) {
            var encodedValue = $('<div />').text(value).html();
            return encodedValue;
        }
    }

});



connection.start();
$('.notification').click(function () {
$('.notification-list').toggle();
}
);


