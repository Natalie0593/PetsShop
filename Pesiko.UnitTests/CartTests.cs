using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pesiko.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}
