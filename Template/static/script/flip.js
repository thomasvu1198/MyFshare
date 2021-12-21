const coin = document.querySelector('#coin');
const button = document.querySelector('#flip');
const status = document.querySelector('#status');
const gratzText = 'Chúc mừng! Bạn được + 1000 ngân lượng';

let headsCount = 0;
let tailsCount = 0;


function CallbackFunct(callback, ms) {
    setTimeout(callback, ms);
}

function processResult(result) {
    status.innerText = result;
}



button.addEventListener('click', flipCoin);

function flipCoin() {
    let choice = $('#choice option:selected').val();
    let amount = $('#value').val();
    if (amount == '') {
        alert('Đổ tiền vào bạn ơi!!!')
    }
    else {
        coin.setAttribute('class', '');
        let random = Math.random();
        let result = random < 0.5 ? 'heads' : 'tails';
        let gratzAlert = (result == 'heads' ? 'Mặt trước ' : 'Mặt sau ');
        let resultToINT = (result == 'heads' ? '0' : '1');
        CallbackFunct(function () {
            coin.setAttribute('class', 'animate-' + result);
            CallbackFunct(processResult.bind(null, gratzAlert), 3000);
            setTimeout(function () {
                let status;
                status = resultToINT != choice ? 0 : 1;
                if (gold >= amount) {
                    $.post('/Flip.ashx',
                        {
                            amount: amount,
                            status: status
                        },
                        function (response) {
                            console.log(response);
                            if (response == 'check') {
                                if (status == 0) {
                                    $('#msgFalse').attr('style', 'display:flex !important');
                                    $('#msgSuccess').attr('style', 'display:none !important');
                                }
                                else {
                                    $('#msgFalse').attr('style', 'display:none !important');
                                    $('#msgSuccess').attr('style', 'display:flex !important');
                                    $('#status').text('');
                                    $('#status').text(gratzAlert + '!!!, Chúc mừng bạn đã được + ' + amount + ' ngân lượng');
                                }

                                update();
                            }
                            else {
                                alert('Lỗi gì đó rồi!!!');
                            }
                        }
                    )
                }
                else {
                    alert('Ngân lượng không đủ rồi');
                }

            }, 3000);

        }, 100);


    }
}

let userID;
let gold;
function update() {
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
                gold = obj[0].gold;
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
}

$(document).ready(function () {
    update();
});



