using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Tiles.Models;
using Tiles.Models.Data;

namespace Tiles.Controllers
{
    /// <summary>
    /// Cooming soon
    /// </summary>
    public class PostController : Controller
    {
        public PostsRepository PostsRepository { get; }

        public PostController(PostsRepository postsRepository) => 
            PostsRepository = postsRepository;


        public ActionResult Index() =>
            View(PostsRepository.AllPosts);


        public async Task<ActionResult> Details(string id)
        {
            var post = await PostsRepository.GetPost(id);

            if (post == null)
                return NotFound();

            return View(post);
        }


        [Authorize]
        [HttpGet]
        public ActionResult Create() =>
            View();


        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create(Post post)
        {
            if (!ModelState.IsValid)
                return View();

            post.PublishDate = DateTime.Now;

            await PostsRepository.SavePost(post);

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            var post = await PostsRepository.GetPost(id);
            if (post == null)
                return NotFound();

            return View(post);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Edit(Post post)
        {
            if (!ModelState.IsValid)
                return View(post);

            await PostsRepository.UpdatePost(post);

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            var post = await PostsRepository.GetPost(id);
            if (post == null)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}