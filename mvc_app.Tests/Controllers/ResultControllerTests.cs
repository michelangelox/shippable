using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mvc_app;
using mvc_app.Controllers;

namespace mvc_app.Controllers.Tests
{
	[TestClass]
	public class ResultControllerTest
	{
		[TestMethod]
		public void Index()
		{
			// Arrange
			ResultController controller = new ResultController();

			// Act
			ViewResult result = controller.Result() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}
	}
}