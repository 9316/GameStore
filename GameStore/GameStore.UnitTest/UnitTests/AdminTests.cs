using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using System.Collections.Generic;
using GameStore.WEB.Controllers;
using System.Linq;
using System.Web.Mvc;

namespace GameStore.UnitTest
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Games()
        {
            //Arrange
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(x => x.Games).Returns(new List<Game>
            {
                new Game { GameId = 1, Name = "Game1"},
                new Game { GameId = 2, Name = "Game2"},
                new Game { GameId = 3, Name = "Game3"},
                new Game { GameId = 4, Name = "Game4"},
                new Game { GameId = 5, Name = "Game5"}
            });

            AdminController controller = new AdminController(mock.Object);

            //Act
            List<Game> result = ((IEnumerable<Game>)controller.Index().ViewData.Model).ToList();

            // Assert
            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual("Game1", result[0].Name);
            Assert.AreEqual("Game2", result[1].Name);
            Assert.AreEqual("Game3", result[2].Name);
        }

        [TestMethod]
        public void Can_Edit_Game()
        {
            // Arrange
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game { GameId = 1, Name = "Game1"},
                new Game { GameId = 2, Name = "Game2"},
                new Game { GameId = 3, Name = "Game3"},
                new Game { GameId = 4, Name = "Game4"},
                new Game { GameId = 5, Name = "Game5"}
            });

            
            AdminController controller = new AdminController(mock.Object);

            // Act
            Game game1 = controller.Edit(1).ViewData.Model as Game;
            Game game2 = controller.Edit(2).ViewData.Model as Game;
            Game game3 = controller.Edit(3).ViewData.Model as Game;

            // Assert
            Assert.AreEqual(1, game1.GameId);
            Assert.AreEqual(2, game2.GameId);
            Assert.AreEqual(3, game3.GameId);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Game()
        {
            // Arrange
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                 new Game { GameId = 1, Name = "Game1"},
                 new Game { GameId = 2, Name = "Game2"},
                 new Game { GameId = 3, Name = "Game3"},
                 new Game { GameId = 4, Name = "Game4"},
                 new Game { GameId = 5, Name = "Game5"}
            });

            AdminController controller = new AdminController(mock.Object);

            // Act
            Game result = controller.Edit(6).ViewData.Model as Game;

            // Assert
        }

        // ...

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // Arrange
            Mock<IGameRepository> mock = new Mock<IGameRepository>();

            AdminController controller = new AdminController(mock.Object);

            Game game = new Game { Name = "Test" };

            // Act
            ActionResult result = controller.Edit(game);

            // Assert
            mock.Verify(m => m.SaveGame(game));

            //Assert
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // Arrange
            Mock<IGameRepository> mock = new Mock<IGameRepository>();

            AdminController controller = new AdminController(mock.Object);

            Game game = new Game { Name = "Test" };

            controller.ModelState.AddModelError("error", "error");

            // Act
            ActionResult result = controller.Edit(game);

            // Assert
            mock.Verify(m => m.SaveGame(It.IsAny<Game>()), Times.Never());

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
