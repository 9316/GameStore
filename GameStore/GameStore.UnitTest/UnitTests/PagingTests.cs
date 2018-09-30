using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GameStore.Domain.Abstract;
using System.Collections.Generic;
using GameStore.Domain.Entities;
using GameStore.WEB.Controllers;
using System.Linq;
using System.Web.WebPages.Html;
using GameStore.WEB.Models;
using System.Web.Mvc;
using GameStore.WEB.HtmlHelpers;

namespace GameStore.UnitTest
{
    [TestClass]
    public class PagingTests
    {

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //Arrange
            System.Web.Mvc.HtmlHelper htmlHelper = null;

            PagingInfo pageInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            Func<int, string> pageUrlDelegate = i => "Page" + i;

            //Act
            MvcHtmlString result = htmlHelper.PageLinks(pageInfo, pageUrlDelegate);

            //Assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Paginate()
        {
            //Arrange
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game { GameId = 1, Name = "Fifa" },
                new Game { GameId = 2, Name = "Need For Speed"},
                new Game { GameId = 3, Name = "Stalker" },
                new Game { GameId = 4, Name = "Pes 2019"},
                new Game { GameId = 5, Name = "Call of Dutty"}
            });

            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            //Act
            GamesListViewModel result = (GamesListViewModel)controller.List(string.Empty,2).Model;

            //Assert
            List<Game> games = result.Games.ToList();
            Assert.IsTrue(games.Count == 2);
            Assert.AreEqual(games[0].Name, "Pes 2019");
            Assert.AreEqual(games[1].Name, "Call of Dutty");
        }


        [TestMethod]
        public void Can_Sent_Pagination_View_Model()
        {
            //Arrange
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game { GameId = 1, Name = "Game1"},
                new Game { GameId = 2, Name = "Game2"},
                new Game { GameId = 3, Name = "Game3"},
                new Game { GameId = 4, Name = "Game4"},
                new Game { GameId = 5, Name = "Game5"}
            });

            //Act
            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            //Assert
            GamesListViewModel result = (GamesListViewModel)controller.List(string.Empty,2).Model;
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }
    }
}
