var UserController = function () {
    this.initialize = function () {
        regsiterEvents();
    }
    function regsiterEvents() {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 2000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })
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
        $('body').on('click', '.Lockout', function (e) {
            e.preventDefault();

            const userid = $(this).data('id');
            var _self = $(this); //Store reference
            var token = getCookie("Token");

            $.ajax({
                type: "POST",
                url: "https://localhost:5001/api/Users/Unlock?userid=" + userid,
                headers: {
                    "Authorization": "Bearer " + token
                },
                contentType: "application/json",
                success: function (res) {
                    Toast.fire({
                        icon: 'success',
                        title: 'Mở khoá tài khoản thành công'
                    })
                    _self.removeClass('Lockout badge bg-red');
                    _self.addClass('EnabledLockout badge bg-green');
                    _self.text("Active")
                },

                error: function (err) {
                    console.log(err);
                }
            });
        });
        $('body').on('click', '.EnabledLockout', function (e) {
            e.preventDefault();

            const userid = $(this).data('id');
            var _self = $(this); //Store reference
            var token = getCookie("Token");

            (async () => {
                let flatpickrInstance;

                await Swal.fire({
                    title: 'Please enter departure date',
                    html: '<input class="swal2-input" id="expiry-date">',
                    stopKeydownPropagation: false,
                    preConfirm: () => {
                        if (flatpickrInstance.selectedDates[0] < new Date()) {
                            Swal.showValidationMessage(`The departure date can't be in the past`)
                        }
                    },
                    willOpen: () => {
                        flatpickrInstance = flatpickr(

                            Swal.getPopup().querySelector('#expiry-date'), {
                            dateFormat: "d/m/Y"
                        }
                        )
                    }
                })

                function convert(str) {
                    var date = new Date(str),
                        mnth = ("0" + (date.getMonth() + 1)).slice(-2),
                        day = ("0" + date.getDate()).slice(-2);
                    return [mnth, day, date.getFullYear()].join("-");
                }
                var time = convert(flatpickrInstance.selectedDates);
                var awd = "https://localhost:5001/api/Users/LockUser?userid=" + userid + "&endDate=" + time;
                $.ajax({
                    type: "POST",
                    url: "https://localhost:5001/api/Users/LockUser?userid=" + userid + "&endDate=" + time,
                    headers: {
                        "Authorization": "Bearer " + token
                    },
                    contentType: "application/json",
                    success: function (res) {
                        Toast.fire({
                            icon: 'success',
                            title: 'Khoá tài khoản thành công'
                        })
                        _self.removeClass('EnabledLockout badge bg-green');
                        _self.addClass('Lockout badge bg-red');
                        _self.text("Locked")
                    },

                    error: function (err) {
                        console.log(err);
                    }
                });
            })();
        });
    }
}