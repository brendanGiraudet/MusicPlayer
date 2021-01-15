using System;

namespace MusicPlayerApplication.Components.Player
{
    public class AudioEventData
    {
        public string Name { get; set; } = AudioEvents.NotSet.ToString();
        public AudioEvents EventName =>
            (AudioEvents)Enum.Parse(typeof(AudioEvents), Name, true);
        public AudioState State { get; set; }
    }
}
