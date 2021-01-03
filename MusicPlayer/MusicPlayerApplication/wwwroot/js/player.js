window.onload = function () {
    var player = document.getElementById("audio");
    var currentTimeTag = document.getElementById("current-time");
    var totalTimetag = document.getElementById("total-time");

    player.addEventListener('timeupdate', function () {
        currentTimeTag.innerHTML = ConvertToTime(player.currentTime);
        totalTimetag.innerHTML = ConvertToTime(player.duration);
    });

    function ConvertToTime(duration) {
        var sec_num = parseInt(duration, 10);
        var hours = Math.floor(sec_num / 3600);
        var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
        var seconds = sec_num - (hours * 3600) - (minutes * 60);

        if (hours < 10) { hours = "0" + hours; }
        if (minutes < 10) { minutes = "0" + minutes; }
        if (seconds < 10) { seconds = "0" + seconds; }
        return hours + ':' + minutes + ':' + seconds;
    }
}

window.player = {
    play: function (playerId) {
        var player = document.getElementById(playerId);
        player.play();
    },
    pause: function (playerId) {
        var player = document.getElementById(playerId);
        player.pause();
    }
}