using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;

namespace Shop.Controllers;

public class HomeController : Controller {
    private readonly ShopViewModel _shopViewModel;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ShopViewModel shopViewModel, ILogger<HomeController> logger) {
        _logger = logger;
        _shopViewModel = shopViewModel;
    }

    public async Task<IActionResult> Index(string? sortBy) {
        await _shopViewModel.LoadCustomers(sortBy ?? _shopViewModel.CurrentSort);
        return View(_shopViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> SortCustomers(string sortBy) {
        await _shopViewModel.LoadCustomers(sortBy);
        return RedirectToAction("Index", new { sortBy });
    }

    public IActionResult Privacy() {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}