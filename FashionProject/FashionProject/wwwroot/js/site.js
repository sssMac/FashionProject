function RedirectModel() {
    $.ajax({
        url: '/Home/GetUser',
        type: "get",
        contentType: 'application/x-www-form-urlencoded',
        success: function (result) {
            console.log(result);
        }
    });
}

