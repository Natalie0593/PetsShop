using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pesiko.Domain.Abstract;
using Pesiko.Domain.Entities;
using Pesiko.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Pesiko.UnitTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            // Организация - создание объекта Game с данными изображения
            Pesik pesik = new Pesik
            {
                PesikId = 2,
                Name = "Игра2",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };

            // Организация - создание имитированного хранилища
            Mock<IPesikoRepository> mock = new Mock<IPesikoRepository>();
            mock.Setup(m => m.Pesiks).Returns(new List<Pesik> {
                new Pesik {PesikId = 1, Name = "Игра1"},
                pesik,
                new Pesik {PesikId = 3, Name = "Игра3"}
            }.AsQueryable());

            // Организация - создание контроллера
            PesikoController controller = new PesikoController(mock.Object);

            // Действие - вызов метода действия GetImage()
            ActionResult result = controller.GetImage(2);

            // Утверждение
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(pesik.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            // Организация - создание имитированного хранилища
            Mock<IPesikoRepository> mock = new Mock<IPesikoRepository>();
            mock.Setup(m => m.Pesiks).Returns(new List<Pesik> {
                new Pesik {PesikId = 1, Name = "Игра1"},
                new Pesik {PesikId = 2, Name = "Игра2"}
            }.AsQueryable());

            // Организация - создание контроллера
            PesikoController controller = new PesikoController(mock.Object);

            // Действие - вызов метода действия GetImage()
            ActionResult result = controller.GetImage(10);

            // Утверждение
            Assert.IsNull(result);
        }
    }
}
