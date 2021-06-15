using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pesiko.Domain.Abstract;
using Pesiko.Domain.Entities;
using Pesiko.WebUI.Controllers;
using Pesiko.WebUI.HtmlHelpers;
using Pesiko.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Pesiko.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (arrange)
            Mock<IPesikoRepository> mock = new Mock<IPesikoRepository>();
            mock.Setup(m => m.Pesiks).Returns(new List<Pesik>
            {
                new Pesik { PesikId = 1, Name = "Игра1"},
                new Pesik { PesikId = 2, Name = "Игра2"},
                new Pesik { PesikId = 3, Name = "Игра3"},
                new Pesik { PesikId = 4, Name = "Игра4"},
                new Pesik { PesikId = 5, Name = "Игра5"}
            });
            PesikoController controller = new PesikoController(mock.Object);
            controller.pageSize = 3;

            // Действие (act)
            PesiksListViewModel result = (PesiksListViewModel)controller.List(2).Model;

            // Утверждение (assert)
            List<Pesik> games = result.Pesiks.ToList();
            Assert.IsTrue(games.Count == 2);
            Assert.AreEqual(games[0].Name, "Игра4");
            Assert.AreEqual(games[1].Name, "Игра5");
        }



        /// <summary>
        /// Чтобы протестировать вспомогательный метод PageLinks(),
        /// вызываем метод с тестовыми данными и сравниваем результаты с ожидаемой HTML-разметкой.
        /// </summary>
        [TestMethod]
        public void Can_Generate_Page_Links()
        {

            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        /////контроллер отправляет представлению 
        ///правильную информацию о разбиении на страницы
        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<IPesikoRepository> mock = new Mock<IPesikoRepository>();
            mock.Setup(m => m.Pesiks).Returns(new List<Pesik>
            {
                new Pesik { PesikId = 1, Name = "Игра1"},
                new Pesik { PesikId = 2, Name = "Игра2"},
                new Pesik { PesikId = 3, Name = "Игра3"},
                new Pesik { PesikId = 4, Name = "Игра4"},
                new Pesik { PesikId = 5, Name = "Игра5"}
            });
            PesikoController controller = new PesikoController(mock.Object);
            controller.pageSize = 3;

            // Act
            PesiksListViewModel result
                = (PesiksListViewModel)controller.List(2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }
    }
}
