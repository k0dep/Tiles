using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Tiles.Models;
using Tiles.Models.Data;

namespace Tiles.Controllers
{


    /// <summary>
    /// Cooming soon
    /// </summary>
    [Authorize]
    public class PostController : Controller
    {
        public PostsRepository PostsRepository { get; private set; }

        public PostController(PostsRepository postsRepository)
        {
            PostsRepository = postsRepository;
        }

        public ActionResult Index()
        {
            return View(PostsRepository.AllPosts);
        }

        public async Task<ActionResult> Details(string id)
        {
            var post = await PostsRepository.GetPost(id);

            if (post == null)
                return NotFound();

            return View(post);
        }

        public ActionResult Create()
        {
            PostsRepository.SavePost(new Post("New post", "Post content", DateTime.Now, new List<string>(){"new post"}));

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Post/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Post/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Post/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Post/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}