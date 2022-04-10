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
$('#btnResetCategory').on('click', function (e) {
    e.preventDefault();
    var language = $('#selectCurrentLanguageId').find(":selected").val();
    $.ajax({
        type: "GET",
        url: 'https://localhost:5001/api/Categories/paging?LanguageId=' + language+'&PageIndex=1&PageSize=5',
        success: OnSuccess,
    });
  
});
$('#selectCurrentLanguageId').on('change', function (e) {
    e.preventDefault();
    GetCustomers(1);
});
function GetCustomers(pageIndex) {
    var keyword = $('#Keyword').val();
    var language = $('#selectCurrentLanguageId').find(":selected").val();
    $.ajax({
        type: "GET",
        url: 'https://localhost:5001/api/Categories/paging?Keyword=' + keyword + '&LanguageId=' + language+'&PageIndex=' + pageIndex + '&PageSize=5',
        success: OnSuccess,
    });
};
function OnSuccess(response) {
    var model = response;

    var template = $("#tplCategoryTop").html();

    var html = "";
    var data = response.items;

    $.each(data, function (i, item) {
        html += Mustache.render(template, {
            id: item.id,
            Name: item.name,
            SeoDescription: item.seoDescription,
            SeoAlias: item.seoAlias,
            SortOrder: item.sortOrder,
            IsShowOnHome: item.isShowOnHome,
            ParentId: item.parentId,
            CategoryId: item.categoryId
        });
    });

    $("#categoryTopBody").html(html);

    $(".Pager").ASPSnippets_Pager({
        ActiveCssClass: "current",
        PagerCssClass: "pager",
        PageIndex: model.pageIndex,
        PageSize: model.pageSize,
        RecordCount: model.totalRecords
    });

    $('#Keyword').val('');
};