var sortName = "";
var sortDirection = "ASC";
$(function () {
    GetCustomers(1);
});

$("body").on("click", ".Pager .page", function () {
    GetCustomers(parseInt($(this).attr('page')));
});
function GetCustomers(pageIndex) {
    $.ajax({
        type: "GET",
        url: 'https://localhost:5001/api/Products/GetTopProductSelling?LanguageId=vi-VN&PageIndex=' + pageIndex + '&PageSize=5',
        success: OnSuccess,
    });
};
function OnSuccess(response) {
    var model = response;

    var template = $("#tplProductTop").html();
    var keyword = $('#Keyword').val();
    console.log(keyword);
    var html = "";
    var data = response.items;
   
    $.each(data, function (i, item) {
        html += Mustache.render(template, {
            id: item.id,
            Name: item.name,
            Price: item.price,
            ThumbnailImage: item.thumbnailImage,
            Stock: item.stock,
            OriginalPrice: item.originalPrice,
            ViewCount: item.viewCount
        });
    });

    $("#productTopBody").html(html);

    $(".Pager").ASPSnippets_Pager({
        ActiveCssClass: "current",
        PagerCssClass: "pager",
        PageIndex: model.pageIndex,
        PageSize: model.pageSize,
        RecordCount: model.totalRecords
    });
};