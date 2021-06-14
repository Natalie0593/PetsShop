using Pesiko.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pesiko.WebUI.Controllers
{
    public class PesikoController : Controller
    {
        // GET: Pesiko
        private IPesikoRepository repository;
        public PesikoController(IPesikoRepository repo)
        {
            repository = repo;
        }

        public ViewResult List()
        {
            return View(repository.Pesiks);
        }
    }
}