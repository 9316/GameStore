using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.WEB.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        IGameRepository gameRepository;

        public AdminController(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        public ViewResult Index()
        {
            return View(gameRepository.Games);
        }

        public ViewResult Create()
        {
            return View("Edit", new Game());
        }

        public ViewResult Edit(int gameId)
        {
            Game game = gameRepository.Games.FirstOrDefault(x => x.GameId == gameId);

            return View(game);
        }

        [HttpPost]
        public ActionResult Edit(Game game, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image !=null)
                {
                    game.ImageMimeType = image.ContentType;
                    game.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(game.ImageData, 0, image.ContentLength);
                }

                gameRepository.SaveGame(game);
                TempData["message"] = string.Format("Changes in the game \"{0}\" were saved", game.Name);

                return RedirectToAction("Index");
            }
            else
            {
                return View(game);
            }
        }

        [HttpPost]
        public ActionResult Delete(int gameId)
        {
            Game deleteGame = gameRepository.DeleteGame(gameId);

            if (deleteGame != null)
            {
                TempData["message"] = string.Format("Game \"{0}\" was deleted", deleteGame.Name);
            }

            return RedirectToAction("Index");
        }
    }
}