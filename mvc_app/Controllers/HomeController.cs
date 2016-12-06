using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Octokit;

namespace mvc_app.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		/*
		public ActionResult GetRespository()
		{
			var respository = new Repository();
			return View(respository);
		}
		*/

		public ActionResult Result(){
			return Content("Received Respository");
		}

		/*
		[HttpPost]
		public string Results(string gitlHubRepository)
		{
			if (gitlHubRepository != null)
			{
				return String.Format(gitlHubRepository);
			}

			if (gitlHubRepository == null)
			{
				return "gitlHubRepository does not exist...";
			}

			return "error";
		}
		*/
	}
}