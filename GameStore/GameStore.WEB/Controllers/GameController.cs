using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.WEB.Controllers
{
    public class GameController : Controller
    {
        IGameRepository repository;
        public int pageSize = 3;

        public GameController(IGameRepository repository)
        {
            this.repository = repository;
        }

        public ViewResult List(string category, int page = 1)
        {
            GamesListViewModel model = new GamesListViewModel
            {
                Games = repository.Games.
                        Where(x=> category == null || x.Category == category).
                        OrderBy(m => m.GameId).
                        Skip((page - 1) * pageSize).
                        Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ? 
                    repository.Games.Count() : 
                    repository.Games.Where(x=> x.Category == category).Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }

        public FileContentResult GetImage(int gameId)
        {
            Game game = repository.Games.FirstOrDefault(x => x.GameId == gameId);

            if (game != null)
            {
                return File(game.ImageData, game.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}