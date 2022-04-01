var AddingCommas = function () {
    this.initialize = function () {
        regsiterEvents();
    }
    function regsiterEvents() {
        $(document).ready(function () {
            //$("input[type='text']").keyup(function (event) {
            //    // skip for arrow keys
            //    if (event.which >= 37 && event.which <= 40) {
            //        event.preventDefault();
            //    }
            //    var $this = $(this);
            //    var num = $this.val().replace(/,/gi, "").split("").reverse().join("");

            //    var num2 = RemoveRougeChar(num.replace(/(.{3})/g, "$1,").split("").reverse().join(""));
            //    // the following line has been simplified. Revision history contains original.
            //    $this.val(num2);
            //});
        });
        $('.AddingCommas').each(function () {
            var item = $(this).text();
            var num = Number(item).toLocaleString('en');

            if (Number(item) < 0) {
                num = num.replace('-', '');
                $(this).addClass('negAddCommas');
            } else {
                $(this).addClass('enAddCommas');
            }

            $(this).text(num);
        });
        function RemoveRougeChar(convertString) {
            if (convertString.substring(0, 1) == ",") {
                return convertString.substring(1, convertString.length)
            }
            return convertString;
        }
    }
}