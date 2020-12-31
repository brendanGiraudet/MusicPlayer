using Bogus;
using MusicPlayerApplication.Settings;

namespace MusicPlayer.UnitTest
{
    public static class FakerUtils
    {
        public static Faker<YoutubeDlSettings> YoutubeDlSettingsFaker => new Faker<YoutubeDlSettings>()
            .RuleFor(r => r.MusicPath, faker => faker.Random.String2(2))
            .RuleFor(r => r.Proxy, faker => faker.Random.String2(2));
    }
}
