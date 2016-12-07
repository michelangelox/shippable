using System;
using System.Collections.Generic;
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
					repositoryUrl = (repositoryUrl.IndexOf(@"https://github.com/", StringComparison.Ordinal) > 0) ? repositoryUrl : repositoryUrl.Replace(@"https://github.com/", "");
					//strip if http:// had no s
					repositoryUrl = (repositoryUrl.IndexOf(@"http://github.com/", StringComparison.Ordinal) > 0 )? repositoryUrl : repositoryUrl.Replace(@"http://github.com/", "");
					//strip last lingering, if any, slash and everythiung after that
					var firstslash = repositoryUrl.IndexOf(@"/", StringComparison.Ordinal);
					repositoryUrl = (firstslash < repositoryUrl.Length-1) ? repositoryUrl : repositoryUrl.Substring(0, repositoryUrl.LastIndexOf(@"/", StringComparison.Ordinal));

					Char delimiter = '/';
					var domainSections = repositoryUrl.Split(delimiter);

					//HACK: make this logic better and moren fault proof
					repository.RepositoryOwner = domainSections[0];
					repository.RepositoryName = domainSections[1];
					repository.RepositoryToken = ConfigurationManager.AppSettings["GitHubAccessToken"].ToString();

					//TODO: Check if repository exists
					//TODO: any other error from GitHub render in error form

					IssuesHelper issuesHelper = new IssuesHelper();

					List<IReadOnlyList<Issue>> collectionOfissues = issuesHelper.GetIssuesForDefinedRepository(repository.RepositoryName, repository.RepositoryOwner, repository.RepositoryToken);

					ViewBag.issues_AllOpen = issuesHelper.IssuesAllOpen;
					ViewBag.issues_OpeninLast24Hours = issuesHelper.IssuesOpeninLast24Hours;
					ViewBag.issues_OpenMoreThan7Days = issuesHelper.IssuesOpenMoreThan7Days;

					return View();
				}

				catch (Exception ex)
				{
					throw;
				}
			}

			ViewBag.issues_AllOpen = 100;
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