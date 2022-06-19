using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Translator.MVC.Controllers;
using Translator.MVC.Models;

namespace Presentation.UnitTests
{
    public class HomeControllerTest
    {
        [Fact]
        public void Index_ReturnsAViewResult()
        {
            var controller = new HomeController();
            var result = controller.Index();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error_ReturnsViewResultWithAModelOfErrorViewModel()
        {
            var controller = new HomeController();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var result = controller.Error();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ErrorViewModel>(viewResult.Model);
        }
    }
}