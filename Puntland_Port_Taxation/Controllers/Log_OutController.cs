using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Puntland_Port_Taxation.Controllers
{
    public class Log_OutController : Controller
    {
        //
        // GET: /Log_Out/

        public ActionResult Index()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return Redirect("Home");
        }

    }
}
