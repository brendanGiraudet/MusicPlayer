export function play(playerId) {
    var player = document.getElementById(playerId);
    player.play();
}

export function pause(playerId) {
    var player = document.getElementById(playerId);

    player.pause();
}

export function stop(playerId) {
    var player = document.getElementById(playerId);
    player.pause();

    player.currentTime = 0;
}

export function change(playerId, source) {
    var player = document.getElementById(playerId);
    player.src = source;
}

export function updateCurrentTime(playerId, currentTime) {
    var player = document.getElementById(playerId);
    player.currentTime = currentTime;
}

// Configuration des événements audio principaux
export function configureAudio(playerId, dotNetInstance, timeUpdateCallback, endedCallback) {
    var player = document.getElementById(playerId);

    // Met à jour le temps actuel et la durée via un callback Blazor
    player.addEventListener("timeupdate", () => {

        var currentTime = 0;
        if (player.currentTime > 0)
            currentTime = player.currentTime;

        var duration = 1;
        if (player.duration > 0)
            duration = player.duration;

        if (timeUpdateCallback !== undefined)
            dotNetInstance.invokeMethodAsync(timeUpdateCallback, currentTime, duration);
    });

    // Notifie Blazor lorsque la chanson est terminée
    player.addEventListener("ended", () => {
        if (endedCallback !== undefined)
            dotNetInstance.invokeMethodAsync(endedCallback);
    });
}