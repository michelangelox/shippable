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
			ViewBag.repositoryUrl = repositoryUrl;

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
					//repository.RepositoryToken = ConfigurationManager.AppSettings["GitHubAccessToken"].ToString();
					string environmentVariable = Environment.GetEnvironmentVariable("GitHubAccessToken");
					if (environmentVariable != null)
						repository.RepositoryToken = environmentVariable.ToString();

					//TODO: Check if repository exists
					//TODO: any other error from GitHub render in error form

					IssuesHelper issuesHelper = new IssuesHelper();

					//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
					//!! This returns the entire collection of issues with dates, authors, replies, and message content for all issues for the corresponding repo. This was not aslked in the exercise, but will be incredibly handy in rendereing on the Resultspage. Not implemented, but data is available to use. 
					List<IReadOnlyList<Issue>> collectionOfissues = issuesHelper.GetIssuesForDefinedRepository(repository.RepositoryName, repository.RepositoryOwner, repository.RepositoryToken);
					//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

					//Below only what was asked, which was the amount of issues per criteria. 
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
			return View();
		}
	}
}