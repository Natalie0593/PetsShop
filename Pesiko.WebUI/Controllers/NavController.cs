using Pesiko.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pesiko.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IPesikoRepository repository;
        public NavController(IPesikoRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult Menu()
        {
            IEnumerable<string> categories = repository.Pesiks
                .Select(pesik => pesik.Category)
                .Distinct() //удаляет дублированные элементы из входной последовательности
                .OrderBy(x => x);
            return PartialView(categories);// ркзультат объект PartialViewResult
        }
    }
}