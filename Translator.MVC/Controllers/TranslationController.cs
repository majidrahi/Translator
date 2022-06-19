using Translator.Application.Interfaces;
using Translator.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Translator.Application.ViewModels;
using Translator.MVC.Helpers.Search;
using System.Linq.Expressions;

namespace Translator.MVC.Controllers
{
    public class TranslationController : Controller
    {
        private readonly ILeetTranslationService _translationService;

        public TranslationController( ILeetTranslationService translationService)
        {
            this._translationService = translationService;
        }

        [Authorize]
        public IActionResult LeetSpeak()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> LeetSpeakList(int page = 1, int pageSize = 12)
        {
            Expression<Func<TranslationViewModel, bool>> expression =default!;
            var searchIndex = SearchFilter.GetSearchIndex(this);
            var searchQuery = SearchFilter.GetSearchQuery(this);
            if (searchIndex != null && !string.IsNullOrEmpty(searchQuery))
            {
                switch (searchIndex)
                {
                    case "SourceText":
                        expression = x => x.SourceText.Contains(searchQuery); break;
                }
            }

            var translationsList =await _translationService.GetByFilter(expression, x => x.OrderByDescending(x => x.Id));

            return View(translationsList);
        }

        [Authorize]
        [HttpPost]
        public ActionResult LeetSpeakList(string searchOption, string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return RedirectToAction(null);
            }
            return RedirectToAction(null, new { filter = "qIndex=" + searchOption + "&q=" + searchQuery });
        }

        /// <summary>
        /// Leet Speak Translate a text
        /// </summary>
        /// <param name="sourceTextInput"></param>
        /// <returns>JsonResult</returns>
        [Authorize]
        [HttpPost]
        public async Task<JsonResult> LeetTranslate(TranslationInputViewModel sourceTextInput)
        {
            if (!ModelState.IsValid)
            {
                return Json(new TranslationResult("Invalid Input!", false));
            }

            var translationResult = await _translationService.Translate(sourceTextInput.SourceText);

            return Json(translationResult);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}