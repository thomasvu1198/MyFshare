let userID;
let gold;
$(document).ready(function () {
    $.get('/update.ashx',
        function (data) {
            if (data != 'false') {
                let obj = JSON.parse(data);
                $('#backpack').show();
                $('#backpack').text(`Ngân lượng: ${obj[0].gold}`);
                gold = obj[0].gold;
                $('#userInfo').show();
                $('#username_display').show();
                $('#username_display').text((obj[0].username));
                $('#welcomeText').text(`Chào mừng ${obj[0].username}`);
                userID = obj[0].id;

                $('#loginBTN').hide();
                $('#LoginModal').hide();
                $('.modal-backdrop ').remove();

            }
            else {
                $('#username_display').text('Khách');
                $('#backpack').hide();
                $('#userInfo').hide();

            }
        })
    try {
        GetAllData();
    }
    catch (error) {
        console.log(error);
    }

});



function GetAllData() {
    $.get('/GetAllData.ashx',
        function (data) {
            $('#Library').empty();
            htmlContext = '';
            dataLength = data.length;
            data = JSON.parse(data)
            for (rowData of data) {
                htmlContext = htmlContext + `<li class="list-group-item d-flex justify-content-between align-items-center">
                        ${rowData.fileName} by ${rowData.username}
                        <button class="btn btn-download badge bg-primary"><i class="fas fa-download"></i></button>
                    <p style='display:none;'>${rowData.fileName}</p></li>`;
            }
            $('#Library').html(htmlContext);
            let allDownloadButtons = document.getElementsByClassName('btn-download');
            for (button of allDownloadButtons) {
                button.addEventListener('click', function () {
                    let fileName = this.parentElement.getElementsByTagName('p')[0].innerHTML;
                    let username = $('#username_display').html();
                    if (username == 'Khách' || username == '') {
                        alert('Đăng nhập đi');
                    }
                    else {
                        $.post('/Download.ashx',
                            {
                                fileName: fileName,
                            },
                            function (response) {
                                if (response != 'false') {
                                    var a = document.createElement("a");
                                    a.href = "data:image/png;base64," + response;
                                    a.download = `[Tnut Đồ Thư Quán] ${fileName}`;
                                    a.click();
                                }
                                else {
                                    alert('Đăng nhập đi');
                                }

                            }
                        )
                    }
                })
            }
        });
}

$('#login').on('click', function () {
    let username = $('#username_input').val();
    let pass = $('#password_input').val();
    $.post('/login.ashx',
        {
            pass: pass,
            username: username
        },
        function (data) {
            if (data != 'false') {
                let obj = JSON.parse(data);
                $('#backpack').show();
                $('#backpack').text(`Ngân lượng: ${obj[0].gold}`);
                $('#userInfo').show();
                $('#username_display').show();
                $('#username_display').text((obj[0].username));
                $('#welcomeText').text(`Chào mừng ${obj[0].username}`);
                //$('#login').hide();

                $('#loginBTN').hide();
                $('#LoginModal').toggle();
                $('.modal-backdrop ').remove();
                userID = obj[0].id;

            }
            else {
                alert('Sai tên đăng nhập hoặc mật khẩu');
            }
        });
})