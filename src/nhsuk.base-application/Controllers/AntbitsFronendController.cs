using System;
using System.Linq;
using System.Net;
using System.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Html;
using nhsuk.base_application.Extensions;
using nhsuk.base_application.Models;
using nhsuk.base_application.ServiceFilter;
using nhsuk.base_application.ViewModels;
using Newtonsoft.Json;

namespace nhsuk.base_application.Controllers
{
    public class AntbitsFrontendController : Controller
    {
        private const string CookieName = "AntbitsFrontendId";

        [Route("antbits-frontend")]
        [HttpGet]
        public IActionResult Index()
        {
            AntbitsSessionData antbitsSessionData = createSession();
            var viewModel = MapToIntro(antbitsSessionData.configData.introTitle, antbitsSessionData.configData.introCopy);

            return View(viewModel);
        }

        private AntbitsSessionData createSession()
        {
            TempData.Clear();
            var antbitsSessionData = new AntbitsSessionData();

            if (!Request.Cookies.ContainsKey(CookieName))
            {
                var id = Guid.NewGuid();

                Response.Cookies.Append(
                    CookieName,
                    id.ToString(),
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(30)
                    });

                antbitsSessionData.id = id;
            }
            else
            {
                if (Request.Cookies.TryGetValue(CookieName, out string idString))
                {
                    antbitsSessionData.id = Guid.Parse(idString);
                }
                else
                {
                    var id = Guid.NewGuid();

                    Response.Cookies.Append(
                        CookieName,
                        id.ToString(),
                        new CookieOptions
                        {
                            Expires = DateTimeOffset.UtcNow.AddDays(30)
                        });

                    antbitsSessionData.id = id;
                }
            }

            string json = "";
            using (WebClient wc = new WebClient())
            {
                json = wc.DownloadString("https://assets.nhs.uk/tools/self-assessments/packages/as_44/data.json");
            }
            dynamic stuffData = JsonConvert.DeserializeObject(json);

            antbitsSessionData.configData.introTitle = stuffData.config.intro_title;
            antbitsSessionData.configData.introCopy = stuffData.config.intro_copy;

            foreach (dynamic question in stuffData.questions)
            {
                QuestionData questionData = new QuestionData();
                questionData.type = question.type;
                questionData.body = question.body;
                foreach (dynamic answer in question.answers)
                {
                    Answer questionAnswer = new Answer();
                    questionAnswer.body = answer.body;
                    foreach (dynamic action in questionAnswer.actions)
                    {
                        AnswerAction answerAction = new AnswerAction();
                        answerAction.type = action.type;
                        answerAction.value = action.value;
                        answerAction.actionOperator = "+";
                        questionAnswer.actions.Add(answerAction);
                    }
                    questionData.answers.Add(questionAnswer);
                }
                antbitsSessionData.questions.Add(questionData);
            }
            

            TempData.Set(antbitsSessionData);

            return antbitsSessionData;
        }

        private static AntbitsIntroViewModel MapToIntro(string intro_title, string intro_copy) =>
            new AntbitsIntroViewModel
            {
                Intro_title = new HtmlString(intro_title),
                Intro_copy = new HtmlString(intro_copy),
            };

        [Route("antbits-frontend/question")]
        [HttpGet]
        public IActionResult Question()
        {
            // As this is a GET it must be the first question 
            AntbitsSessionData antbitsSessionData = TempData.Get<AntbitsSessionData>();
            QuestionData currentQuestion = antbitsSessionData.questions[antbitsSessionData.currentQuestionNumber];
            var viewModel = MapToQuestion(currentQuestion);
            return View(viewModel);
        }

        private static AntbitsQuestionViewModel MapToQuestion(QuestionData questionData) {
            AntbitsQuestionViewModel question = new AntbitsQuestionViewModel();
            question.body = questionData.body;

            question.answers = new List<Dictionary<string, string>>();

            int id = 0;
            foreach (Answer answerData in questionData.answers)
            {
                question.answers.Add(new Dictionary<string, string>(){
                    {"id", id.ToString()},
                    {"value", answerData.body}
                });
                id += 1;
            }
            return question;
        }

        [Route("antbits-frontend/question")]
        [HttpPost]
        public IActionResult Question(AntbitsQuestionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            AntbitsSessionData antbitsSessionData = TempData.Get<AntbitsSessionData>();
            // Probably need to do some validation at this point
            ResponseDetails responseDetails = new ResponseDetails();
            responseDetails.responseBody = model.body;
            // get the responseActions by pulling the actions from antbitsSessionData.questions[model.body]

            Dictionary<int, ResponseDetails> response = new Dictionary<int, ResponseDetails>()
            {
                {antbitsSessionData.currentQuestionNumber, responseDetails}
            };

            ResponseData responseData = new ResponseData();
            responseData.responseDetails.Add(response);

            antbitsSessionData.responses.Add(responseData);
            antbitsSessionData.currentQuestionNumber += 1;
            TempData.Set(antbitsSessionData);

            QuestionData currentQuestion = antbitsSessionData.questions[antbitsSessionData.currentQuestionNumber];

            // This really needs to be a test for Have we asked the final question
            if (antbitsSessionData.currentQuestionNumber == 2)
            {
                 return RedirectToAction(nameof(Result));
            }
            var viewModel = MapToQuestion(currentQuestion);
            return View(viewModel);
        }

        [Route("antbits-frontend/result")]
        [HttpGet]
        public IActionResult Result()
        {
            AntbitsSessionData antbitsSessionData = TempData.Get<AntbitsSessionData>();

            //Will need to process the session data at this point so that the correct values can be displayed in the result
            var viewModel = MapToResult(antbitsSessionData);

            return View(viewModel);
        }

        private static AntbitsResultViewModel MapToResult(AntbitsSessionData antbitsSessionData)
        {
            AntbitsResultViewModel antbitsResultViewModel = new AntbitsResultViewModel();

            // Will need to process the session data at this point so that the correct values can be displayed in the result
            // Pulling out the links, link_items and calculating variable scores

            Dictionary<string, int> depressionScore = new Dictionary<string, int>(){
                {"depressionScore", 12},
                {"depressionMax", 24},
                {"depressionMin", 0}
            };

            antbitsResultViewModel.accumulatedValues.Add("depressionScore", 12);
            antbitsResultViewModel.accumulatedValues.Add("depressionMax", 24);
            antbitsResultViewModel.accumulatedValues.Add("depressionMin", 0);

            return antbitsResultViewModel;
        }
    }
}