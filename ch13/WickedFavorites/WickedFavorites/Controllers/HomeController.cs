using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Wicked.Favorites.Models;
using System.Collections.Generic;

namespace Wicked.Favorites.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDependencies<HomeController> _d;

        public HomeController(IDependencies<HomeController> dependencies)
        {
            _d = dependencies;
        }

        public IActionResult Index()
        {
            var favoritesFromDb = _d.Context.Favorites.Include(x => x.Category)
                                                      .OrderBy(x => x.Category.Name)
                                                      .ThenBy(x => x.Votes)
                                                      .ThenBy(x => x.Name)
                                                      .ToList();
            var favoritesToDisplay = _d.Mapper.Map<IEnumerable<FavoriteModel>>(favoritesFromDb);
            return View(favoritesToDisplay);
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
}
