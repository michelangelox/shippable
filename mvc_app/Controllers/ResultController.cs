using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using mvc_app.Controllers;
using Octokit;

namespace mvc_app.Controllers
{
	public class Issues
	{
		public int IssueId;
		private readonly string GitHubUserName = "miguelmoreno";
		private readonly string GitHubRepository = "shippable";
		private readonly string GitHubAccessToken = "2ffa71b8d7be587f779268b0b888763fd13e9f4a";

		internal RepositoryIssueRequest issues_AllOpen = new RepositoryIssueRequest
		{ 
			Filter = IssueFilter.All,
			State = ItemStateFilter.Open
		};

		internal RepositoryIssueRequest issues_OpeninLast24Hours = new RepositoryIssueRequest
		{
			Filter = IssueFilter.All,
			State = ItemStateFilter.Open,
			Since = DateTimeOffset.Now.Subtract(TimeSpan.FromHours(24))
		};

		//internal IssueRequest issues_OpeninLast24HoursButLessThan7Days = new IssueRequest
		//{
		//	Filter = IssueFilter.All,
		//	State = ItemState.All,
		//	Since = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(14))
		//};

		internal RepositoryIssueRequest issues_OpenMoreThan7Days = new RepositoryIssueRequest
		{
			Filter = IssueFilter.All,
			State = ItemStateFilter.Open,
			Since = DateTimeOffset.Now.AddDays(7)
		};

		public string getIssues(string url) {

			GitHubClient client = new GitHubClient(new ProductHeaderValue(GitHubRepository));
			Credentials tokenAuth = new Credentials(GitHubAccessToken);
			client.Credentials = tokenAuth;

			//TODO: move string to web.config file 
			User user = client.User.Get("miguelmoreno").Result; ;

			//TODO: move string to web.config file
			var allIssuesForShippableRepository = client.Issue.GetAllForRepository(GitHubUserName, GitHubRepository).Result;

			var issueCollectionForRepo = client.Issue;

			var _issues_AllOpenEver = issueCollectionForRepo.GetAllForRepository(GitHubUserName, GitHubRepository, issues_AllOpen).Result;
			var _issues_OpeninLast24Hours = issueCollectionForRepo.GetAllForRepository(GitHubUserName, GitHubRepository, issues_OpeninLast24Hours).Result;
			var _issues_OpenMoreThan7Days = issueCollectionForRepo.GetAllForRepository(GitHubUserName, GitHubRepository, issues_OpenMoreThan7Days).Result;

			//-------
			return "";
		}

	}

	public class ResultController : Controller
    {
		// GET: Result
		public ActionResult Result()
        {
			Issues _issues = new Issues();
	        var poep = _issues.getIssues("");

			//ViewBag.issues_AllOpen = _issues_AllOpenEver;

			ViewBag.IssueRequest = "100";

			return View();
        }
    }
}