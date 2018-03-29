using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tiles.Models;
using Tiles.Models.Data;

namespace Tiles.Controllers
{
    public class TileController : Controller
    {
        public TileRepository TilesRepository { get; }


        public TileController(TileRepository tilesRepository)
        {
            TilesRepository = tilesRepository;
        }


        public async Task<IActionResult> Index()
        {
            return View(await TilesRepository.AllTiles());
        }


        [Authorize]
        public IActionResult Create()
        {
            return View();
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Tile tile)
        {
            if (!ModelState.IsValid)
                return View();

            await TilesRepository.SaveTile(tile);

            return Redirect("~/");
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();

            await TilesRepository.DeleteTile(id);

            return Redirect("~/");
        }

        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();

            var tile = await TilesRepository.FindTile(id);
            if(tile == null)
                return NotFound();

            return View(tile);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(Tile tile)
        {
            if (!ModelState.IsValid)
                return View(tile);

            await TilesRepository.EditTile(tile);

            return Redirect("~/");
        }
    }
}