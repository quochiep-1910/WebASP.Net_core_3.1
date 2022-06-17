var sortName = "";
var sortDirection = "ASC";
$(function () {
    GetCustomers(1);
});

$("body").on("click", ".Pager .page", function () {
    GetCustomers(parseInt($(this).attr('page')));
});
$('#btnSearch').on('submit', function (e) {
    e.preventDefault();
    GetCustomers(1);
});
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
var token = getCookie("Token");
$('#btnResetOrder').on('click', function (e) {
    e.preventDefault();
    
    $.ajax({
        type: "GET",
        url: 'https://localhost:5001/api/Orders/paging?&PageIndex=1&PageSize=5',
        headers: {
            "Authorization": "Bearer " + token
        },
        success: OnSuccess,
    });
});

function GetCustomers(pageIndex) {
    var keyword = $('#Keyword').val();

    $.ajax({
        type: "GET",
        url: 'https://localhost:5001/api/Orders/paging?Keyword=' + keyword + '&PageIndex=' + pageIndex + '&PageSize=5',
        headers: {
            "Authorization": "Bearer " + token
        },
        success: OnSuccess,
    });
};
function OnSuccess(response) {
    var model = response;

    var template = $("#tplOrderTop").html();

    var html = "";
    var data = response.items;

    $.each(data, function (i, item) {
        html += Mustache.render(template, {
            id: item.id,
            ShipName: item.shipName,
            ShipAddress: item.shipAddress,
            OrderDate: item.orderDate,
            Status: item.status
        });
    });

    $("#orderTopBody").html(html);

    $(".Pager").ASPSnippets_Pager({
        ActiveCssClass: "current",
        PagerCssClass: "pager",
        PageIndex: model.pageIndex,
        PageSize: model.pageSize,
        RecordCount: model.totalRecords
    });

    $('#Keyword').val('');
};