using Bogus;
using Google.Apis.YouTube.v3.Data;
using Google.Apis.YouTube.v3;

namespace MusicPlayer.UnitTest
{
    public static class FakerUtils
    {
        public static Faker<SearchListResponse> SearchListResponseFaker => new Faker<SearchListResponse>()
            .RuleFor(r => r.Items, faker => SearchResultFaker.Generate(2));

        public static Faker<SearchResult> SearchResultFaker => new Faker<SearchResult>()
            .RuleFor(r => r.Id, faker => ResourceIdFaker.Generate());

        public static Faker<ResourceId> ResourceIdFaker => new Faker<ResourceId>()
            .RuleFor(r => r.ChannelId, faker => faker.Random.String2(2))
            .RuleFor(r => r.ETag, faker => faker.Random.String2(2))
            .RuleFor(r => r.Kind, faker => faker.Random.String2(2))
            .RuleFor(r => r.PlaylistId, faker => faker.Random.String2(2))
            .RuleFor(r => r.VideoId, faker => faker.Random.String2(2));

        public static Faker<SearchResource.ListRequest> ListRequestFaker => new Faker<SearchResource.ListRequest>();
    }
}
