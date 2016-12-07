using System;
using System.Collections.Generic;
using Octokit;

namespace mvc_app.Classes
{
	public class IssuesHelper
	{
		public int IssueId;

		public int IssuesAllOpen { get; set; } = -1;
		public int IssuesOpeninLast24Hours { get; set; } = -1;
		public int IssuesOpenMoreThan7Days { get; set; } = -1;

		//Predefined filters for issues
		private readonly RepositoryIssueRequest _issuesAllOpen = new RepositoryIssueRequest
		{
			Filter = IssueFilter.All,
			State = ItemStateFilter.Open
		};

		private readonly RepositoryIssueRequest _issuesOpeninLast24Hours = new RepositoryIssueRequest
		{
			Filter = IssueFilter.All,
			State = ItemStateFilter.Open,
			Since = DateTimeOffset.Now.Subtract(TimeSpan.FromHours(24))
		};

		private readonly RepositoryIssueRequest _issuesOpenMoreThan7Days = new RepositoryIssueRequest
		{
			Filter = IssueFilter.All,
			State = ItemStateFilter.Open,
			Since = DateTimeOffset.Now.AddDays(7)
		};

		public List<IReadOnlyList<Issue>> GetIssuesForDefinedRepository(string gitlHubRepository, string gitHubOwner, string gitHubAccessToken)
		{
			try
			{
				List<IReadOnlyList<Issue>> collectionOfIssues = new List<IReadOnlyList<Issue>>();

				//Client authentication
				GitHubClient client = new GitHubClient(new ProductHeaderValue(gitlHubRepository));
				Credentials tokenAuth = new Credentials(gitHubAccessToken);
				client.Credentials = tokenAuth;

				//User retrieval - not required
				User user = client.User.Get(gitHubOwner).Result;

				//All issues for specified repository - not required
				var issuesAllForShippableRepository = client.Issue.GetAllForRepository(gitHubOwner, gitlHubRepository).Result;

				//TODO: Check if repository exists
				//TODO: any other error from GitHub render in error form

				var issueCollectionForRepo = client.Issue;

				//Issues filtered by defined parameteres
				var issuesAllOpenEver = 
					issueCollectionForRepo.GetAllForRepository(gitHubOwner, gitlHubRepository, _issuesAllOpen).Result;
				this.IssuesAllOpen = issuesAllOpenEver.Count;
			
				var issuesOpeninLast24Hours =
					issueCollectionForRepo.GetAllForRepository(gitHubOwner, gitlHubRepository, _issuesOpeninLast24Hours).Result;
				this.IssuesOpeninLast24Hours = issuesOpeninLast24Hours.Count;

				var issuesOpenMoreThan7Days =
					issueCollectionForRepo.GetAllForRepository(gitHubOwner, gitlHubRepository, _issuesOpenMoreThan7Days).Result;
				this.IssuesOpenMoreThan7Days = issuesOpenMoreThan7Days.Count;

				//Adds filtered issues to the collection to be returned
				collectionOfIssues.Add(issuesAllOpenEver);
				collectionOfIssues.Add(issuesOpeninLast24Hours);				
				collectionOfIssues.Add(issuesOpenMoreThan7Days);

				return collectionOfIssues;
			}
			catch (Exception ex)
			{
				throw;
			}
			return null;
		}
	}

}