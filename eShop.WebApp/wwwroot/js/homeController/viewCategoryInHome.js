var CategoryHomeController = function () {
    this.initialize = function () {
        viewCategoryInHome();
    }
    function viewCategoryInHome() {
        $(".handleCategory").click(function () {
            const languageId = $('#hidCulture').val();
            var categoryId = $(this).data("id");

            $.ajax({
                url: 'https://localhost:5001/api/Products/paging?LanguageId=' + languageId + '&CategoryId=' + categoryId + '&PageIndex=1&PageSize=5',
                type: "Get",
                dataType: "json",
                success: function (res) {
                    if (res.items !== null) {
                        var template = $("#tplProduct").html();
                        var html = "";
                        var data = res.items;
                        console.log(data);
                        $.each(data, function (i, item) {
                            html += Mustache.render(template, {
                                id: item.id,
                                name: item.name,
                                price: item.price,
                                thumbnailImage: item.thumbnailImage,
                            });
                        });
                        if (html == '') {
                            html = "<span style=\"color: red;\"> Không có sản phẩm trong danh mục này </span>";
                        }
                        $("#productBody").html(html);
                    }
                },
            });
        });
    }
}