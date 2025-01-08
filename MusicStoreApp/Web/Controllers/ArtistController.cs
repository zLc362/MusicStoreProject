using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Repository;
using Service.Interface;

namespace Web.Controllers
{
    public class ArtistController : Controller
    {
        
        private readonly IArtistService _artistService;

        public ArtistController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        // GET: Artist
        public async Task<IActionResult> Index()
        {
            return View(await _artistService.GetAll());
        }

        // GET: Artist/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _artistService.GetById(id.Value);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // GET: Artist/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Artist/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Biography")] Artist artist, IFormFile Image)
        {

            if (ModelState.IsValid)
            {
                artist.Id = Guid.NewGuid();
                if (Image != null && Image.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await Image.CopyToAsync(memoryStream);
                        var imageBytes = memoryStream.ToArray();

                        var mimeType = Image.ContentType;

                        var base64Image = $"data:{mimeType};base64,{Convert.ToBase64String(imageBytes)}";
                        artist.Image = base64Image;
                    }

                }
                await _artistService.Create(artist);
                return RedirectToAction(nameof(Index));
            }
            return View(artist);
        }

        // GET: Artist/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _artistService.GetById(id.Value);
            if (artist == null)
            {
                return NotFound();
            }
            return View(artist);
        }

        // POST: Artist/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Biography")] Artist artist)
        {
            if (id != artist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _artistService.Update(artist);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ArtistExists(artist.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(artist);
        }

        // GET: Artist/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _artistService.GetById(id.Value);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // POST: Artist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var artist = await _artistService.GetById(id);
            if (artist != null)
            {
                await _artistService.Delete(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ArtistExists(Guid id)
        {
            return await _artistService.GetById(id) != null;
        }
    }
}
