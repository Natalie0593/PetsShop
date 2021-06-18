using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pesiko.Domain.Abstract;
using Pesiko.Domain.Entities;
using Pesiko.WebUI.Controllers;
using Pesiko.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Pesiko.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // Организация - создание нескольких тестовых игр
            Pesik pes1 = new Pesik { PesikId = 1, Name = "Игра1" };
            Pesik pes2 = new Pesik { PesikId = 2, Name = "Игра2" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(pes1, 1);
            cart.AddItem(pes2, 1);
            List<CartLine> results = cart.Lines.ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Pesik, pes1);
            Assert.AreEqual(results[1].Pesik, pes2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            
            Pesik pes1 = new Pesik { PesikId = 1, Name = "Игра1" };
            Pesik pes2 = new Pesik { PesikId = 2, Name = "Игра2" };

            
            Cart cart = new Cart();

            
            cart.AddItem(pes1, 1);
            cart.AddItem(pes2, 1);
            cart.AddItem(pes1, 5);
            List<CartLine> results = cart.Lines.OrderBy(c => c.Pesik.PesikId).ToList();

            
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Quantity, 6);   
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            
            Pesik pes1 = new Pesik { PesikId = 1, Name = "Игра1" };
            Pesik pes2 = new Pesik { PesikId = 2, Name = "Игра2" };
            Pesik pes3 = new Pesik { PesikId = 3, Name = "Игра3" };

           
            Cart cart = new Cart();

            
            cart.AddItem(pes1, 1);
            cart.AddItem(pes2, 4);
            cart.AddItem(pes3, 2);
            cart.AddItem(pes2, 1);

            
            cart.RemoveLine(pes2);

            
            Assert.AreEqual(cart.Lines.Where(c => c.Pesik == pes2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
           
            Pesik pes1 = new Pesik { PesikId = 1, Name = "Игра1", Price = 100 };
            Pesik pes2 = new Pesik { PesikId = 2, Name = "Игра2", Price = 55 };

            Cart cart = new Cart();

            
            cart.AddItem(pes1, 1);
            cart.AddItem(pes2, 1);
            cart.AddItem(pes1, 5);
            decimal result = cart.ComputeTotalValue();

            
            Assert.AreEqual(result, 655);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
     
            Pesik game1 = new Pesik { PesikId = 1, Name = "Игра1", Price = 100 };
            Pesik game2 = new Pesik { PesikId = 2, Name = "Игра2", Price = 55 };

            
            Cart cart = new Cart();

            
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game1, 5);
            cart.Clear();

            
            Assert.AreEqual(cart.Lines.Count(), 0);
        }


        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация - создание пустой корзины
            Cart cart = new Cart();

            // Организация - создание деталей о доставке
            ShippingDetails shippingDetails = new ShippingDetails();

            // Организация - создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Действие
            ViewResult result = controller.Checkout(cart, shippingDetails);

            // Утверждение — проверка, что заказ не был передан обработчику 
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Утверждение — проверка, что метод вернул стандартное представление 
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            Cart cart = new Cart();
            cart.AddItem(new Pesik(), 1);

            // Организация — создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Организация — добавление ошибки в модель
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Утверждение - проверка, что заказ не передается обработчику
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
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
            cart.AddItem(new Pesik(), 1);

            // Организация — создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Утверждение - проверка, что заказ передан обработчику
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Once());

            // Утверждение - проверка, что метод возвращает представление 
            Assert.AreEqual("Completed", result.ViewName);

            // Утверждение - проверка, что представлению передается допустимая модель
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }

}
