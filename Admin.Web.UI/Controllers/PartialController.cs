using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Web.UI.Controllers
{
    public class PartialController : Controller
    {
        public PartialViewResult DrawerPartial()
        {
            return PartialView("Partial/_DrawerPartial");
        }

        public PartialViewResult HeaderPartial()
        {
            return PartialView("Partial/_HeaderPartial");
        }

        public PartialViewResult ModalPartial()
        {
            return PartialView("Partial/_ModalPartial");
        }
    }
}