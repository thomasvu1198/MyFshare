let userID = null;
$(document).ready(function () {
    $.get('/update.ashx',
        function (data) {
            if (data != 'false') {
                let obj = JSON.parse(data);
                $('#backpack').show();
                $('#userInfo').show();
                $('#username_display').show();
                $('#username_display').text((obj[0].username));
                $('#welcomeText').text(`Chào mừng ${obj[0].username}`);
                userID = obj[0].id;
                let gold = (obj[0].gold);
                $('#backpack').text(`Ngân lượng: ${gold}`);
                //$('#login').hide();
                GetUserFile();

            }
            else {
                $('#username_display').text('Khách');
                $('#backpack').hide();
                $('#userInfo').hide();

            }
        });
});

function GetUserFile() {
    $.post('/GetUserFile.ashx',
        {
            userID: userID
        },
        function (data) {
            $('#UserFile').empty();
            console.log(data);
            htmlContext = '';
            dataLength = data.length;
            data = JSON.parse(data)
            for (rowData of data) {
                htmlContext = htmlContext + `<li class="list-group-item d-flex justify-content-between align-items-center">
                        ${rowData.fileName} 
                        <button class="btn btn-download badge bg-primary"><i class="fas fa-download"></i></button>
                    <p style='display:none;'>${rowData.fileName}</p></li>`;
            }
            $('#UserFile').html(htmlContext);
            let allDownloadButtons = document.getElementsByClassName('btn-download');
            for (button of allDownloadButtons) {
                button.addEventListener('click', function () {
                    let fileName = this.parentElement.getElementsByTagName('p')[0].innerHTML;
                    let username = $('#username_display').val();
                    $.post('/Download.ashx',
                        {
                            fileName: fileName,
                            username: username
                        },
                        function (response) {
                            var a = document.createElement("a"); //Create <a>
                            a.href = "data:image/png;base64," + response; //Image Base64 Goes here
                            a.download = `[Tnut Đồ Thư Quán] ${fileName}`; //File name Here
                            a.click(); //Downloaded file
                        }
                    )

                })
            }
        })
}

$('#upload-btn').on('click', function () {
        let formData = new FormData();
        formData.append('file', $('#upload-input')[0].files[0]);
        $.ajax({
            type: 'post',
            url: '/UploadFile.ashx',
            data: formData,
            success: function (data) {
                if (data == 'check') {
                    $.get('/update.ashx',
                        function (data) {
                            if (data != 'false') {
                                let obj = JSON.parse(data);
                                $('#backpack').show();
                                $('#backpack').text(`Ngân lượng: ${obj[0].gold}`);
                                $('#userInfo').show();
                                $('#username_display').show();
                                $('#username_display').text((obj[0].username));
                                $('#welcomeText').text(`Chào mừng ${obj[0].username}`);
                                userID = obj[0].id;
                                //$('#login').hide();
                                alert('Upload Done')

                            }
                            else {
                                $('#username_display').text('Khách');
                                $('#backpack').hide();
                                $('#userInfo').hide();

                            }
                        });
                }
            },
            processData: false,
            contentType: false,
        });
})