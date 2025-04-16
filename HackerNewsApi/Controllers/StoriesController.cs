﻿using HackerNewsApi.Respositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoriesController : ControllerBase
    {
        private readonly IStoryRepository _repository;

        public StoriesController(IStoryRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("top")]
        public async Task<IActionResult>GetTopStories()
        {
            var stories = await _repository.GetTopStoriesAsync();
            return Ok(stories);
        }
    }
}
