using Pesiko.Domain.Abstract;
using Pesiko.WebUI.Models;
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
        public int pageSize = 4;
        public PesikoController(IPesikoRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(int page = 1)
        {
            PesiksListViewModel model = new PesiksListViewModel
            {
                Pesiks = repository.Pesiks
                    .OrderBy(game => game.PesikId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = repository.Pesiks.Count()
                }
            };
            return View(model);
        }
    }
}