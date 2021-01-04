window.player = {
    play: function (playerId) {
        var player = document.getElementById(playerId);
        player.play();
    },
    pause: function (playerId) {
        var player = document.getElementById(playerId);
        player.pause();
    },
    converttotime: function (duration) {
        var sec_num = parseInt(duration, 10);
        var hours = Math.floor(sec_num / 3600);
        var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
        var seconds = sec_num - (hours * 3600) - (minutes * 60);

        if (hours < 10) { hours = "0" + hours; }
        if (minutes < 10) { minutes = "0" + minutes; }
        if (seconds < 10) { seconds = "0" + seconds; }
        return hours + ':' + minutes + ':' + seconds;
    },
    timeupdate: function () {
        var audioplayer = document.getElementById("audioPlayer");
        var currentTimeTag = document.getElementById("current-time");
        var totalTimetag = document.getElementById("total-time");
        var progressbarTag = document.getElementById("songProgressbar");

        audioplayer.addEventListener('timeupdate', function () {
            currentTimeTag.innerHTML = window.player.converttotime(audioplayer.currentTime);
            totalTimetag.innerHTML = window.player.converttotime(audioplayer.duration);
            progressbarTag.value = '' + audioplayer.currentTime;
            progressbarTag.max = '' + audioplayer.duration;
        });
    }
}