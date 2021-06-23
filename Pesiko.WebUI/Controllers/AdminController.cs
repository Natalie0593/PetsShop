using Pesiko.Domain.Abstract;
using Pesiko.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pesiko.WebUI.Controllers
{
    [Authorize]
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
        public ActionResult Edit(Pesik pesik, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    pesik.ImageMimeType = image.ContentType;
                    pesik.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(pesik.ImageData, 0, image.ContentLength);
                }
                repository.SavePesik(pesik);
                TempData["message"] = string.Format("Изменения в игре \"{0}\" были сохранены", pesik.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(pesik);
            }
        }
        public ViewResult Create()
        {
            return View("Edit", new Pesik());
        }

        [HttpPost]
        public ActionResult Delete(int pesikId)
        {
            Pesik deletedPesik = repository.DeletePesik(pesikId);
            if (deletedPesik != null)
            {
                TempData["message"] = string.Format("Корм \"{0}\" был удален",
                    deletedPesik.Name);
            }
            return RedirectToAction("Index");
        }
    }
}
