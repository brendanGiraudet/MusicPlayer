using System;

namespace MusicPlayerApplication.Extensions;

public static class DoubleExtensions
{
    public static string AsTextTime(this double time)
    {
        if(time < 1) return $"00:00:00";
        
        var sec_num = Math.Round(time);
        var hours = Math.Floor(sec_num / 3600);
        var minutes = Math.Floor((sec_num - (hours * 3600)) / 60);
        var seconds = sec_num - (hours * 3600) - (minutes * 60);

        string stringHours = hours.ToString().PadLeft(2, '0');
        string stringMinutes = minutes.ToString().PadLeft(2, '0');
        string stringSeconds = seconds.ToString().PadLeft(2, '0');

        return $"{stringHours}:{stringMinutes}:{stringSeconds}";
    }

    public static decimal AsDecimal(this double duration)
    {
        return Convert.ToDecimal(duration);
    }
}