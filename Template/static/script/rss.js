
function update() {
    $.get('/RssHandler.ashx',
        function (data) {
            console.log(data);
            $('#rss-content').html('');
            let response = JSON.parse(data);
            let listTitle = response[0].sum;
            let htmlAppendData = '';
            for (let i = 0; i < Object.keys(response).length; i++) {
                let aTag = response[i].sum.substr(0, response[i].sum.indexOf('</br>'));
                let href = response[i].sum.substr(response[i].sum.indexOf('href'), response[i].sum.indexOf('><img'));
                href = href.substr(0, href.indexOf('html')) + "html"
                let des = response[i].sum.substr(response[i].sum.indexOf('</br>'));
                let title = response[i].title
                htmlAppendData = htmlAppendData + `<div class="card mt-3" style="">
                                      <div class="card-body">
                                        <div class="card-title">${aTag}</div>
                                        <span class="card-text">${title}</span>
                                        <br>
                                        <p class="card-text">${des}</p>
                                        <br>
                                        <br>
                                        <a ${href}" class="btn btn-primary">Xem chi tiết</a>
                                      </div>
                                    </div>`
                console.log(href.trim(-2));
            }
            $('#rss-content').append(htmlAppendData);
        })
}



$(document).ready(function () {
    update();
});