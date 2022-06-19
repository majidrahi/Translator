using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq.Expressions;
using Translator.Application.Interfaces;
using Translator.Application.ViewModels;
using Translator.MVC.Controllers;
using Translator.MVC.Models;

namespace Presentation.UnitTests
{
    public class TranslationControllerTest
    {
        [Fact]
        public void LeetSpeak_ReturnsAViewResult()
        {
            var mockService = new Mock<ILeetTranslationService>();
            var controller = new TranslationController(mockService.Object);
            var result = controller.LeetSpeak();
            Assert.IsType<ViewResult>(result);
        }

        private IReadOnlyCollection<TranslationViewModel> GetTestTranslations()
        {
            List<TranslationViewModel> model = new List<TranslationViewModel>()
            {
                new TranslationViewModel(){
                    Id=1,
                    IsSucceded=true,
                    CreatedDateTime=new DateTimeOffset(),
                    SourceText="SourceText",
                    TranslatedText="SourceText",
                    TranslationType="LeetSpeak"
                },

                 new TranslationViewModel(){
                    Id=2,
                    IsSucceded=false,
                    CreatedDateTime=new DateTimeOffset(),
                    SourceText="Majid",
                    TranslatedText="Error",
                    TranslationType="LeetSpeak"
                }
                 ,

                 new TranslationViewModel(){
                    Id=3,
                    IsSucceded=false,
                    CreatedDateTime=new DateTimeOffset(),
                    SourceText="MajidRahimpour",
                    TranslatedText="Error",
                    TranslationType="LeetSpeak"
                }
            };

            return model;
        }

        [Fact]
        public async Task LeetSpeakList_ReturnsAViewResult_WithAListOfTranslationViewModel()
        {
            var mockService = new Mock<ILeetTranslationService>();
            Expression<Func<TranslationViewModel, bool>> expression = default!;

            mockService.Setup(srv => srv.GetByFilter(expression, x => x.OrderByDescending(x => x.Id), "", 0, 0)).ReturnsAsync(GetTestTranslations());

            var controller = new TranslationController(mockService.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var result = await controller.LeetSpeakList();


            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<TranslationViewModel>>(
                viewResult.ViewData.Model);
            Assert.IsType<List<TranslationViewModel>>(model);
            Assert.Equal(3, model.Count());
        }

        [Fact]
        public async Task LeetSpeakList_ReturnsAViewResult_WithAListOfFilteredTranslationViewModel()
        {
            var mockService = new Mock<ILeetTranslationService>();
            Expression<Func<TranslationViewModel, bool>> expression = default!;
            var filteredModel = GetTestTranslations().Where(x => x!.SourceText!.Contains("Majid")).ToList();
            mockService.Setup(srv => srv.GetByFilter(expression, x => x.OrderByDescending(x => x.Id), "", 0, 0)).ReturnsAsync(filteredModel);

            var controller = new TranslationController(mockService.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.QueryString = new QueryString("?filter=qIndex=SourceText&q=Majid");
            var result = await controller.LeetSpeakList();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<TranslationViewModel>>(
                viewResult.ViewData.Model);
            Assert.IsType<List<TranslationViewModel>>(model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void LeetSpeakListPost_RedirectsToLeetSpeakList_WithFilterRouteValue()
        {
            var mockService = new Mock<ILeetTranslationService>();

            var controller = new TranslationController(mockService.Object);
            var result = controller.LeetSpeakList("SourceText", "Majid");

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.True(redirectResult?.RouteValues?.ContainsKey("filter"));
        }

        [Fact]
        public void LeetSpeakListPost_RedirectsToLeetSpeakList_WithNoRouteValueIfsearchQueryIsEmpty()
        {
            var mockService = new Mock<ILeetTranslationService>();

            var controller = new TranslationController(mockService.Object);
            var result = controller.LeetSpeakList("","");

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.Null(redirectResult?.RouteValues);
        }

        [Fact]
        public async Task LeetTranslate_ReturnsTranslationJsonResultWithAModelOfTranslationResult()
        {
            var mockService = new Mock<ILeetTranslationService>();
            TranslationInputViewModel sourceTextInput = new() { SourceText = "Majid" };
            var controller = new TranslationController(mockService.Object);
            var result = await controller.LeetTranslate(sourceTextInput);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.IsType<TranslationResult>(jsonResult.Value);
        }

        [Fact]
        public void Error_ReturnsViewResultWithAModelOfErrorViewModel()
        {
            var mockService = new Mock<ILeetTranslationService>();
            var controller = new TranslationController(mockService.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var result = controller.Error();

            // Assert
            var viewResult =  Assert.IsType<ViewResult>(result);
            Assert.IsType<ErrorViewModel>(viewResult.Model) ;
        }
    }
}