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
$('#btnResetProduct').on('click', function (e) {
    e.preventDefault();
    language
    $.ajax({
        type: "GET",
        url: 'https://localhost:5001/api/Products/paging?LanguageId=' + language + '&PageIndex=1&PageSize=5',
        success: OnSuccess,
    });
    $('#selectCategoryId').val('--chọn danh mục--')
});
$('#selectCategoryId').on('change', function (e) {
    e.preventDefault();
    GetCustomers(1);
});
$('#selectCurrentLanguageId').on('change', function (e) {
    e.preventDefault();
    GetCustomers(1);
});
function GetCustomers(pageIndex) {
    var keyword = $('#Keyword').val();
    var categoryId = $('#selectCategoryId').find(":selected").val();
    var language = $('#selectCurrentLanguageId').find(":selected").val();
  
    if (categoryId == '--chọn danh mục--') {
        categoryId = '';
    }

    $.ajax({
        type: "GET",
        url: 'https://localhost:5001/api/Products/paging?Keyword=' + keyword + '&LanguageId=' + language+'&CategoryId=' + categoryId + '&PageIndex=' + pageIndex + '&PageSize=5',
        success: OnSuccess,
    });
};
function OnSuccess(response) {
    var model = response;

    var template = $("#tplProductTop").html();

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

    $('#Keyword').val('');
};