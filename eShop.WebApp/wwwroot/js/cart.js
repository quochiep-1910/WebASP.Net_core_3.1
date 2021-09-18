var CartController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }
    function registerEvents() {
        $('body').on('click', '.btn-plus', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            const quantity = parseInt($('#txt_quantity_' + id).val()) + 1;
            $('#txt_quantity_' + id).val(quantity);
            updateCart(id, quantity);
        });

        $('body').on('click', '.btn-minus', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            const quantity = parseInt($('#txt_quantity_' + id).val()) - 1;
            $('#txt_quantity_' + id).val(quantity);

            updateCart(id, quantity);
        });
        $('body').on('click', '.btn-remove', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            updateCart(id, 0);
        });
        $('#btnUpdate').off('click').on('click', function () {
            document.location.reload(true);
        });
        $('#chkUserLoginInfo').off('click').on('click', function (e) {
            if ($(this).prop('checked'))
                getLoginUser();
            else {
                $('#txtName').val('');
                $('#txtEmail').val('');
                $('#txtPhone').val('');
            }
        });
    }

    function updateCart(id, quantity) {
        const culture = $('#hidCulture').val();
        $.ajax({
            type: "POST",
            url: "/" + culture + '/Cart/UpdateCart',
            data: {
                id: id,
                quantity: quantity
            },
            success: function (res) {
                loadData();
            },
            error: function (err) {
                console.log(err);
            }
        });
    }
    function loadData() {
        const culture = $('#hidCulture').val();
        $.ajax({
            type: "GET",
            url: "/" + culture + '/Cart/Index',
            success: function (res) {
            }
        });
    }

    //Load data
    function getLoginUser() {
        const culture = $('#hidCulture').val();
        $.ajax({
            type: "GET",
            url: "/" + culture + '/Cart/GetUser',
            dataType: 'json',
            success: function (res) {
                var user = res;
                $('#txtName').val(user.userName);
                $('#txtEmail').val(user.email);
                $('#txtPhone').val(user.phoneNumber);
            }
        });
    }
}