using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace WebServices.Controllers
{
    public class CourseHistoryController : Controller
    {
        //
        // GET: /CourseHistory/
        [HttpPost]
        public string Index(string Name)
        {
            List<AcademicCalender> acacalList = AcademicCalenderManager.GetAll();
            return new JavaScriptSerializer().Serialize(acacalList);
        }

    }
}
