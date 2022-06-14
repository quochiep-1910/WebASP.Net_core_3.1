var OrderController = function () {
    this.initialize = function () {
        regsiterEvents();
    };
    function getCookie(cname) {
        let name = cname + "=";
        let decodedCookie = decodeURIComponent(document.cookie);
        let ca = decodedCookie.split(';');
        for (let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }
    function regsiterEvents() {
        var token = getCookie("Token");

        var userId = document.getElementById("UserId").value;
        $.ajax({
            type: "GET",
            url: "https://localhost:5001/api/Orders/GetTotalOrderById?id=" + userId,
            headers: {
                "Authorization": "Bearer " + token
            },
            success: OnSuccess,
        });
        function formatDate(date) {
            var d = new Date(date),
                day = '' + d.getDate(),
                month = '' + (d.getMonth() + 1),
                year = d.getFullYear();
           

            if (month.length < 2) month = '0' + month;
            if (day.length < 2) day = '0' + day;
            
            return [month, day, year].join('-');
        }
      
        function OnSuccess(response) {
        
            var template = $("#tplLoadingTimeline").html();

            var html = "";
            var data = response.orderDetails;
            console.log(data)
            $.each(data, function (i, item) {
                html += Mustache.render(template, {
                    description: item.description,
                    name: item.name,
                    orderDate: formatDate( item.orderDate),
                    orderId: item.orderId,
                    price: item.price,
                    productId: item.productId,
                    quantity: item.quantity,
                    shipAddress: item.shipAddress,
                    shipEmail: item.shipEmail,
                    shipName: item.shipName,
                    shipPhoneNumber: item.shipPhoneNumber,
                    status: item.status,
                    thumbnailImage: item.thumbnailImage,
                    userId: item.userId,
                });
            });

            $("#timelineMustacheBody").html(html);
        }
    }
};