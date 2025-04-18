using HackerNewsApi.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Json;

namespace HackerNewsApi.Respositories
{
    public class StoryRepository : IStoryRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache; 

        public StoryRepository(IHttpClientFactory httpClientFactory, IMemoryCache cache)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache; 
        }

        public async Task<IEnumerable<Story>> GetNewestStoriesAsync(int page, int pageSize, string searchTerm)
        {
            if(!_cache.TryGetValue("hn_stories", out List<Story> stories))
            {
                var client = _httpClientFactory.CreateClient();
                var ids = await client.GetFromJsonAsync<List<int>>("https://hacker-news.firebaseio.com/v0/newstories.json");

                var tasks = ids.Take(100).Select(id =>
                client.GetFromJsonAsync<Story>($"https://hacker-news.firebaseio.com/v0/item/{id}.json")
                );

                stories = (await Task.WhenAll(tasks))
                    .Where(s => s != null && !string.IsNullOrEmpty(s.Url))
                    .ToList();

                _cache.Set("hn_stories", stories, TimeSpan.FromMinutes(10));
            }
            return stories
                .Where(s => string.IsNullOrEmpty(searchTerm) || s.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public async Task<IEnumerable<Story>> GetTopStoriesAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var ids = await client.GetFromJsonAsync<List<int>>("https://hacker-news.firebaseio.com/v0/topstories.json");

            var tasks = ids.Take(50).Select(id =>
            client.GetFromJsonAsync<Story>($"https://hacker-news.firebaseio.com/v0/item/{id}.json")
            );

            var stories = (await Task.WhenAll(tasks))
                .Where(s => s != null && !string.IsNullOrEmpty(s.Url)).ToList();

            return stories; 
        }

        public async Task<Story?> GetStoryByIdAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();
            return await client.GetFromJsonAsync<Story>($"https://hacker-news.firebaseio.com/v0/item/{id}.json");
        }

    }
}
