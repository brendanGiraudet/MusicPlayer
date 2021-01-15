using System.Collections.Generic;

namespace MusicPlayerApplication.Components.Player
{
    public class AudioState
    {
        public double CurrentTime { get; set; }
        public double Duration { get; set; }

        public static string[] GetPayload()
        {
            var list = new List<string>();

            list.Add(FormatAsPayload(nameof(CurrentTime)));
            list.Add(FormatAsPayload(nameof(Duration)));

            return list.ToArray();

            static string FormatAsPayload(string name)
                => $"{name.Substring(0, 1).ToLower()}{name.Substring(1)}";
        }
    }
}
