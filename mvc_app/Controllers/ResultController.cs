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
		

		public async Task<string> getIssues(string url) {

			GitHubClient client = new GitHubClient(new ProductHeaderValue("Shippable"));
			Credentials tokenAuth = new Credentials("2ffa71b8d7be587f779268b0b888763fd13e9f4a");
			client.Credentials = tokenAuth;

			User user = client.User.Get("miguelmoreno").Result; ;

			Console.WriteLine("{0} has {1} public repositories - go check out their profile at {2}",
				user.Name,
				user.PublicRepos,
				user.Url);

			var issues = client.Issue.GetAllForRepository("miguelmoreno", "shippable").Result;
			var issuesShippable = client.Issue.GetAllForRepository("miguelmoreno", "shippable").Result;
			//-------
			HttpWebRequest issuesRequest = WebRequest.Create(url) as HttpWebRequest;
			issuesRequest.UserAgent = "Shippable";
			issuesRequest.Method = "GET";
			WebResponse issuesResponse = issuesRequest.GetResponse();

			Stream receiveStream = issuesResponse.GetResponseStream();
			StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
			string content = reader.ReadToEnd();

			return content;
		}

	}

	public class ResultController : Controller
    {
		

		private static string url = @"https://api.github.com/repos/miguelmoreno/shippable/issues -u 'miguelmoreno:2ffa71b8d7be587f779268b0b888763fd13e9f4a'";
		// GET: Result
		public ActionResult Result()
        {
			ViewBag.TotalIssues = 10;

			Issues IssueRequest = new Issues();
	        var poep = IssueRequest.getIssues(url);
			ViewBag.IssueRequest = poep;

			return View();
        }
    }
}