using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pesiko.Domain.Abstract;
using Pesiko.Domain.Entities;
using Pesiko.WebUI.Controllers;
using System;
using System.Web.Mvc;

namespace Pesiko.UnitTests
{
    [TestClass]
    public class EditPesik
    {
        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IPesikoRepository> mock = new Mock<IPesikoRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Game
            Pesik pesik = new Pesik { Name = "Test" };

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(pesik);

            // Утверждение - проверка того, что к хранилищу производится обращение
            mock.Verify(m => m.SavePesik(pesik));

            // Утверждение - проверка типа результата метода
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IPesikoRepository> mock = new Mock<IPesikoRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Game
            Pesik pesik = new Pesik { Name = "Test" };

            // Организация - добавление ошибки в состояние модели
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(pesik);

            // Утверждение - проверка того, что обращение к хранилищу НЕ производится 
            mock.Verify(m => m.SavePesik(It.IsAny<Pesik>()), Times.Never());

            // Утверждение - проверка типа результата метода
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
