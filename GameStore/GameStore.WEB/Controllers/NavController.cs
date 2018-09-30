using GameStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.WEB.Controllers
{
    public class NavController : Controller
    {
        IGameRepository gameRepository;

        public NavController(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> categories = gameRepository.Games
                 .Select(game => game.Category)
                 .Distinct()
                 .OrderBy(x => x);

            return PartialView("FlexMenu", categories);
        }
    }
}