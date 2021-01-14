window.player = {
    play: function (playerId) {
        var player = document.getElementById(playerId);
        player.play();
    },
    pause: function (playerId) {
        var player = document.getElementById(playerId);
        player.pause();
    },
    stop: function (playerId) {
        var player = document.getElementById(playerId);
        player.pause();
        player.currentTime = 0;
    },
    change: function (playerId, source) {
        var player = document.getElementById(playerId);
        player.src = source;
    }
}

window.CustomEventHandler = function (playerId, eventName, payload) {
    var player = document.getElementById(playerId);
    if (!(player && eventName)) {
        return false
    }
    if (!player.hasOwnProperty('customEvent')) {
        player['customEvent'] = function (eventName, payload) {

            payload.currentTime = 4;
            this['value'] = getJSON(this, eventName, payload)

            var event
            if (typeof (Event) === 'function') {
                event = new Event('change')
            } else {
                event = document.createEvent('Event')
                event.initEvent('change', true, true)
            }

            this.dispatchEvent(event)
        }
    }

    player.addEventListener(eventName, function () { player.customEvent(eventName, payload) });

    // Craft a bespoke json string to serve as a payload for the event
    function getJSON(el, eventName, payload) {

        if (payload && payload.length > 0) {
            // this syntax copies just the properties we request from the source element
            // IE 11 compatible
            let data = {};
            for (var obj in payload) {
                var item = payload[obj];
                if (el[item]) {
                    data[item] = el[item]
                }
            }

            // this stringify overload eliminates undefined/null/empty values
            return JSON.stringify(
                { name: eventName, state: data }
                , function (k, v) { return (v === undefined || v == null || v.length === 0) ? undefined : v }
            )
        } else {
            return JSON.stringify(
                { name: eventName }
            )
        }
    }
}