using System.Diagnostics;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAlbumService _albumService;

    public HomeController(ILogger<HomeController> logger, IAlbumService albumService)
    {
        _logger = logger;
        _albumService = albumService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _albumService.GetAll());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}