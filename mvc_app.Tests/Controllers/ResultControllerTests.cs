using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mvc_app;
using mvc_app.Controllers;
using mvc_app.Models;

namespace mvc_app.Controllers.Tests
{
	[TestClass]
	public class ResultControllerTest
	{
		//Global Arrange
		public RepositoryModel RepositoryModel = new RepositoryModel()
		{
			RepositoryUrl = @"https://github.com/miguelmoreno/shippable",
			RepositoryName = ConfigurationManager.AppSettings["GitHubRepoName"].ToString(),
			RepositoryOwner = ConfigurationManager.AppSettings["GitHubRepoOwner"].ToString(),
			RepositoryToken = ConfigurationManager.AppSettings["GitHubAccessToken"].ToString()
		};

		[TestMethod]
		public void Index()
		{			
			ResultController controller = new ResultController();
		
			// Act
			ViewResult result = controller.Index(RepositoryModel) as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}
	}
}