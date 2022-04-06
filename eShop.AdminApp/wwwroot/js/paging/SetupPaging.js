var PagingSetup = function () {
    this.initialize = function () {
        regsiterEvents();
    }
    function regsiterEvents() {
        $(document).ready(function () {
            $(function () {
                var container = $('#demo');
                var data = {};
                //$.ajax({
                //    type: 'GET',
                //    url: 'https://localhost:5001/api/Products/GetTopProductSelling?LanguageId=vi-VN&CategoryId=1&PageIndex=1&PageSize=4',
                //    success: function (res) {
                //        data = res
                //        console.log(data);
                //    }
                //});
                container.pagination({
                    dataSource: 'https://localhost:5001/api/Products/GetTopProductSelling?LanguageId=vi-VN',
                    //dataSource: function () {
                    //    $.ajax({
                    //        type: 'GET',
                    //        url: 'https://localhost:5001/api/Products/GetTopProductSelling?LanguageId=vi-VN&CategoryId=1&PageIndex=1&PageSize=4',
                    //        success: function (res) {
                    //            data=res;
                    //        }
                    //    });
                    //},
                    locator: 'items',

                    totalNumber: 20,
                    pageSize: 3,
                    showPageNumbers: false,
                    showNavigator: true,
                    ajax: {
                        beforeSend: function () {
                            container.prev().html('Loading data from backend ...');
                        }
                    },
                    callback: function (response, pagination) {
                        console.log(data);
                        var dataHtml = '<ul>';
                        var pageStart = (pagination.pageNumber - 1) * pagination.pageSize;
                        var pageEnd = pageStart + pagination.pageSize;
                        var pageItems = response.slice(pageStart, pageEnd);
                        $.each(pageItems, function (index, item) {
                            dataHtml += '<li>' + item.name + '</li>';
                        });

                        dataHtml += '</ul>';

                        container.prev().html(dataHtml);
                    }
                })
            })
        }
        )
    }
}