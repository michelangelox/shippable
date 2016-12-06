using System;
using System.Web.Mvc;
using mvc_app.Classes;

namespace mvc_app.Controllers
{
	public class ResultController : Controller
	{
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

		// GET: Result
		public ActionResult Result()
        {
			IssuesHelper _issues = new IssuesHelper();
	        var GitHubRepository = "shippable";
	        var GitHubUserOwner = "miguelmoreno";

			var issues = _issues.getIssuesForDefinedRepository(GitHubRepository, GitHubUserOwner);

	        ViewBag.issues_AllOpen = _issues.IssuesAllOpen;
			ViewBag.issues_OpeninLast24Hours = _issues.Issues_OpeninLast24Hours;
			ViewBag.issues_OpenMoreThan7Days = _issues.Issues_OpenMoreThan7Days;

			return View();
        }

	}
}