using Pesiko.Domain.Abstract;
using Pesiko.Domain.Entities;
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
        private IPesikoRepository repository;
        public int pageSize = 4;
        public PesikoController(IPesikoRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(string category, int page = 1)
        {
            PesiksListViewModel model = new PesiksListViewModel
            {
                Pesiks = repository.Pesiks
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(pesik => pesik.PesikId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
                    repository.Pesiks.Count() :
                    repository.Pesiks.Where(pesik => pesik.Category == category).Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }

        public FileContentResult GetImage(int pesikId)
        {
            Pesik pesik = repository.Pesiks
                .FirstOrDefault(g => g.PesikId == pesikId);

            if (pesik != null)
            {
                return File(pesik.ImageData, pesik.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}