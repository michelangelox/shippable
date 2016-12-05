﻿using System;
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
	//TODO: move this class somewhere to a separate GitHub helper class
	public class IssuesHelper
	{
		public int IssueId;

		public int IssuesAllOpen { get; set; } = -1;
		public int Issues_OpeninLast24Hours { get; set; } = -1;
		public int Issues_OpenMoreThan7Days { get; set; } = -1;

	
		//TODO: move static strings to web.config file
		private readonly string GitHubUserOwner = "miguelmoreno";
		private readonly string GitHubRepository = "shippable";
		private readonly string GitHubAccessToken = "2ffa71b8d7be587f779268b0b888763fd13e9f4a";

		//Predefined filters for issues
		internal RepositoryIssueRequest _issues_AllOpen = new RepositoryIssueRequest
		{ 
			Filter = IssueFilter.All,
			State = ItemStateFilter.Open
		};

		internal RepositoryIssueRequest _issues_OpeninLast24Hours = new RepositoryIssueRequest
		{
			Filter = IssueFilter.All,
			State = ItemStateFilter.Open,
			Since = DateTimeOffset.Now.Subtract(TimeSpan.FromHours(24))
		};

		//TODO: incorporate this filter in the list
		//internal IssueRequest _issues_OpeninLast24HoursButLessThan7Days = new IssueRequest
		//{
		//	Filter = IssueFilter.All,
		//	State = ItemState.All,
		//	Since = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(14))
		//};

		internal RepositoryIssueRequest _issues_OpenMoreThan7Days = new RepositoryIssueRequest
		{
			Filter = IssueFilter.All,
			State = ItemStateFilter.Open,
			Since = DateTimeOffset.Now.AddDays(7)
		};

		public string getIssuesForDefinedRepository(string gitlHubRepository, string gitHubOwner) {

			//Client authentication
			GitHubClient client = new GitHubClient(new ProductHeaderValue(GitHubRepository));
			Credentials tokenAuth = new Credentials(GitHubAccessToken);
			client.Credentials = tokenAuth;

			//User retrieval
			User user = client.User.Get(GitHubUserOwner).Result; ;

			//All issues for specified repository 
			var issuesAllForShippableRepository = client.Issue.GetAllForRepository(GitHubUserOwner, GitHubRepository).Result;

			//TODO: Check if repository exists
			//TODO: any other error from GitHub render in error form

			var issueCollectionForRepo = client.Issue;

			//Issues filtered by defined parameteres
			var issuesAllOpenEver = issueCollectionForRepo.GetAllForRepository(GitHubUserOwner, GitHubRepository, _issues_AllOpen).Result;
			this.IssuesAllOpen = issuesAllOpenEver.Count;

			var issuesOpeninLast24Hours = issueCollectionForRepo.GetAllForRepository(GitHubUserOwner, GitHubRepository, _issues_OpeninLast24Hours).Result;
			this.Issues_OpeninLast24Hours = issuesOpeninLast24Hours.Count;

			var issuesOpenMoreThan7Days = issueCollectionForRepo.GetAllForRepository(GitHubUserOwner, GitHubRepository, _issues_OpenMoreThan7Days).Result;
			this.Issues_OpenMoreThan7Days = issuesOpenMoreThan7Days.Count;

			//TODO: return serialized JSON object or error from GitHub 
			return "";
		}

	}

	public class ResultController : Controller
	{
		[HttpPost]
		public string Result(string gitlHubRepository, string gitHubOwner)
		{
			if (gitlHubRepository != null && gitHubOwner != null)
			{
				return String.Format(gitlHubRepository+'/'+gitHubOwner);
			}

			if (gitlHubRepository == null)
			{
				return "gitlHubRepository does not exist...";
			}

			if (gitHubOwner == null)
			{
				return "GitHub gitHubOwner does not exist...";
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