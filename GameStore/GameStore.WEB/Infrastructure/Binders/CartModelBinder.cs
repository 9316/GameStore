using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace GameStore.WEB.Infrastructure.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string sessionKey = "Cart";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Cart cart = null; //cart ობიექტის მიღება სესიიდან.

            if (controllerContext.HttpContext.Session != null)
            {
                cart = (Cart)controllerContext.HttpContext.Session[sessionKey];
            }

            if (cart == null)
            {
                cart = new Cart(); //თუ cart ობიექტი არ არის სესიაში
            }

            if (controllerContext.HttpContext.Session != null)
            {
                controllerContext.HttpContext.Session[sessionKey] = cart;
            }

            return cart;
        }
    }
}