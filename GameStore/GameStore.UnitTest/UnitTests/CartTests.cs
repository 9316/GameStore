using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameStore.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using Moq;
using GameStore.Domain.Abstract;
using GameStore.WEB.Controllers;
using System.Web.Mvc;

namespace GameStore.UnitTest
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            //Arrange
            Game game1 = new Game { GameId = 1, Name = "Game1" };
            Game game2 = new Game { GameId = 2, Name = "Game2" };

            Cart cart = new Cart();

            //Act
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            List<CartLine> results = cart.Lines.ToList();

            //Assert
            Assert.AreEqual(results.Count, 2);
            Assert.AreEqual(results[0].Game, game1);
            Assert.AreEqual(results[1].Game, game2);


        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            //Arrange
            Game game1 = new Game { GameId = 1, Name = "Game1" };
            Game game2 = new Game { GameId = 2, Name = "Game2" };

            Cart cart = new Cart();

            // Act
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game1, 5);
            List<CartLine> results = cart.Lines.OrderBy(c => c.Game.GameId).ToList();

            // Assert
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Quantity, 6);    // 6 экземпляров добавлено в корзину
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            //Arrange
            Game game1 = new Game { GameId = 1, Name = "Game1" };
            Game game2 = new Game { GameId = 2, Name = "Game2" };
            Game game3 = new Game { GameId = 3, Name = "Game3" };

            Cart cart = new Cart();

            //Act
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game3, 1);
            cart.AddItem(game2, 1);

            cart.RemoveLine(game2);

            //Assert
            Assert.AreEqual(cart.Lines.Where(x => x.Game == game2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);

        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            //Arrange
            Game game1 = new Game { GameId = 1, Name = "Game1", Price = 100 };
            Game game2 = new Game { GameId = 2, Name = "Game2", Price = 55 };

            Cart cart = new Cart();

            //Act
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game1, 3);

            decimal total = cart.ComputeTotalValue();

            //Assert
            Assert.AreEqual(total, 455);

        }

        [TestMethod]
        public void Can_Clear_Content()
        {
            //Arrange
            Game game1 = new Game { GameId = 1, Name = "Game1", Price = 100 };
            Game game2 = new Game { GameId = 2, Name = "Game2", Price = 55 };

            Cart cart = new Cart();

            //Act
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game1, 1);
            cart.Clear();    

            //Assert
            Assert.AreEqual(cart.Lines.Count(), 0);

        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart() //ამ ტესტ მეთოდით ვამოწმებთ, რომ შეუძლებელია გადასვლა ცარიელი კალათით შეკვეთის შესრულებაზე გადახდაზე.
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();//შევქმენით იმიტირებული ორდერის ჰენდლერი

            Cart cart = new Cart(); //შევქმენით ცარიელი კალათის ობიექტი

            ShippingDetails shippingDetails = new ShippingDetails();//შევქმენით დელივერის ობიექტი

            CartController cartController = new CartController(null, mock.Object);

            ViewResult result = cartController.Checkout(cart, shippingDetails);

            mock.Verify(x=> x.OrderProcess(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), //ვამოწმებთ შეკვეთა რო არიყო გადაცემული ჰენდლერზე
                 Times.Never());

            Assert.AreEqual(string.Empty, result.ViewName);//ვამოწმებთ, მეთოდი აბრუნებს თუ არა სტანდარტულ View-ს.

            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);// ვამოწმებთ რომ View გადაეცა არასწორ მოდელს

        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            Cart cart = new Cart();
            cart.AddItem(new Game(), 1);

            // Организация — создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Организация — добавление ошибки в модель
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Утверждение - проверка, что заказ не передается обработчику
            mock.Verify(m => m.OrderProcess(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Утверждение - проверка, что метод вернул стандартное представление
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            Cart cart = new Cart();
            cart.AddItem(new Game(), 1);

            // Организация — создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Утверждение - проверка, что заказ передан обработчику
            mock.Verify(m => m.OrderProcess(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Once());

            // Утверждение - проверка, что метод возвращает представление 
            Assert.AreEqual("Completed", result.ViewName);

            // Утверждение - проверка, что представлению передается допустимая модель
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
