using Pesiko.Domain.Abstract;
using Pesiko.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pesiko.WebUI.Controllers
{
    public class AdminController : Controller
    {
        IPesikoRepository repository;


        public AdminController(IPesikoRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.Pesiks);
        }
        public ViewResult Edit(int pesikId)
        {
            Pesik pesik = repository.Pesiks
                .FirstOrDefault(g => g.PesikId == pesikId);
            return View(pesik);
        }

        // Перегруженная версия Edit() для сохранения изменений
        [HttpPost]
        public ActionResult Edit(Pesik pesik)
        {
            if (ModelState.IsValid)
            {
                repository.SavePesik(pesik);
                TempData["message"] = string.Format("Изменения в корме \"{0}\" были сохранены", pesik.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(pesik);
            }
        }
    }
}
