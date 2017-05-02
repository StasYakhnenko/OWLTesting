using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BAL.Interface;
using Microsoft.AspNetCore.Authorization;

namespace OwlTesting.Controllers
{
    public class HomeController : Controller
    {
		private readonly ISubjectManager subjectManager;
		public HomeController(ISubjectManager subjectManager)
		{
			this.subjectManager = subjectManager;
		}
		[AllowAnonymous]
        public IActionResult Index()
        {
			var subjects = subjectManager.GetAll();
            return View(subjects);
        }
    }
}
