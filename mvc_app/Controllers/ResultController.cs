using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using mvc_app.Classes;
using mvc_app.Models;
using Octokit;

namespace mvc_app.Controllers
{
	public class ResultController : Controller
	{
		//TODO: move static strings to web.config file
		

		public ActionResult Result()
		{
			throw new NotImplementedException();
		}

		public ActionResult Index(RepositoryModel repository)
		{
			var repositoryUrl = repository.RepositoryUrl;

			if (repositoryUrl != null)
			{
				try
				{ 
					//ValidateRequest hithub url must contain at least 4 //
					//TODO: harden this fault logic - use REGEX

					//strip http and Github.com domain
					repositoryUrl = (repositoryUrl.IndexOf(@"https://github.com/") > 0) ? repositoryUrl : repositoryUrl.Replace(@"https://github.com/", "");
					//strip if http:// had no s
					repositoryUrl = (repositoryUrl.IndexOf(@"http://github.com/") > 0 )? repositoryUrl : repositoryUrl.Replace(@"http://github.com/", "");
					//strip last lingering, if any, slash and everythiung after that
					var fistslash = repositoryUrl.IndexOf(@"/");
					repositoryUrl = (fistslash < repositoryUrl.Length-1) ? repositoryUrl : repositoryUrl.Substring(0, repositoryUrl.LastIndexOf(@"/"));

					Char delimiter = '/';
					var DomainSections = repositoryUrl.Split(delimiter);

					//HACK: make this logic better and moren fault proof
					repository.RepositoryOwner = DomainSections[0];
					repository.RepositoryName = DomainSections[1];

					//Client authentication
					//GitHubClient client = new GitHubClient(new ProductHeaderValue(repository.RepositoryName));
					//Credentials tokenAuth = new Credentials(GitHubAccessToken);
					//client.Credentials = tokenAuth;

					//User retrieval
					//User user = client.User.Get(repository.RepositoryOwner).Result;

					//All issues for specified repository 
					//var issuesAllForShippableRepository = client.Issue.GetAllForRepository(GitHubUserOwner, GitHubRepository).Result;

					//TODO: Check if repository exists
					//TODO: any other error from GitHub render in error form

					//var issueCollectionForRepo = client.Issue;
					repository.RepositoryToken = ConfigurationManager.AppSettings["GitHubAccessToken"].ToString();

					IssuesHelper IssuesHelper = new IssuesHelper();

					var issues = IssuesHelper.getIssuesForDefinedRepository(repository.RepositoryName, repository.RepositoryOwner, repository.RepositoryToken);
					var poep = issues;

					ViewBag.issues_AllOpen = IssuesHelper.IssuesAllOpen;
					ViewBag.issues_OpeninLast24Hours = IssuesHelper.Issues_OpeninLast24Hours;
					ViewBag.issues_OpenMoreThan7Days = IssuesHelper.Issues_OpenMoreThan7Days;

					return View();
				}

				catch (Exception ex)
				{
					throw;
				}
			}

			return View();
		}

		/*
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
		*/

	}
}