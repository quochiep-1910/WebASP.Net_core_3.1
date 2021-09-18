var Hidden = function () {
    this.initialize = function () {
        hiddenSiderbar();
    }
    function hiddenSiderbar() {
        $(".col-sm-3").hide();
    }
}