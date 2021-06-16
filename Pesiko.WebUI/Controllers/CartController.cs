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
    public class CartController : Controller
    {
        private IPesikoRepository repository;
        //Для сохранения и извлечения объектов Cart
        //применяется средство состояния сеанса ASP.NET
        public CartController(IPesikoRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            });
        }
        public RedirectToRouteResult AddToCart(int pesikId, string returnUrl)
        {
            Pesik pesik = repository.Pesiks
                .FirstOrDefault(p => p.PesikId == pesikId);

            if (pesik != null)
            {
                GetCart().AddItem(pesik, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public RedirectToRouteResult RemoveFromCart(int gameId, string returnUrl)
        {
            Pesik pesik = repository.Pesiks
                .FirstOrDefault(p => p.PesikId == gameId);

            if (pesik != null)
            {
                GetCart().RemoveLine(pesik);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
    }
}