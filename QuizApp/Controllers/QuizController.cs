using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuizApp.Data;
using QuizApp.Migrations;
using QuizApp.Models;
using QuizApp.Repositories.IRepository;
using QuizApp.Services;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace QuizApp.Controllers
{
    public class QuizController : Controller
    {
        //private readonly IQuizRepository _quizRepository;
        private readonly AppDbContext _dbContext;
        private readonly IQuestionService _questionService;


        public QuizController(AppDbContext db, IQuestionService questionService)
        {
            _dbContext = db;
            _questionService = questionService;
        }



        public IActionResult CreateQuiz()
        {
            //var questions = _dbContext.Questions.Include(q => q.Answers).ToList();

            
               return View();
            
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult CreateQuiz(Question question) {

            if (ModelState.IsValid)
            {
                _dbContext.Questions.Add(question);
                _dbContext.SaveChanges();
                return RedirectToAction("Support2", "Quiz");
            }

            return View(question);

        }

        //[Authorize(Roles = "Support")]
        public ActionResult Support2()
        {
            var question = _dbContext.Questions.Include(q => q.Answers).ToList();

            return View(question);
        }
        public IActionResult CalculatePercentage()
        {
            return View();
        }

        public IActionResult SubmitAnswer()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SubmitAnswer(List<int> answers)
        {
            if (answers != null && answers.Count > 0)
            {
                var questions = _dbContext.Questions.Include(q => q.Answers).ToList();
                double correctCount = 0;
                for (int i = 0; i < answers.Count; i++)
                {
                    int selecteOptionIndex = answers[i];
                    if (selecteOptionIndex >= 0 && selecteOptionIndex < questions[i].Answers.Count)
                    {
                        if (questions[i].Answers[selecteOptionIndex].IsCorrect)
                        {
                            correctCount++;
                        }
                    }
                }
                double percentage = (correctCount / questions.Count) * 100.0;
                ViewBag.Percentage = percentage;
                return View();
            }
            return View("Support2");
        }
        [HttpGet]
        public ActionResult Result(double percentage)
        {
            ViewBag.Percentage = percentage;

            return View();
        }


        public IActionResult CalculateResult()
        {

            return View();
        }

        [HttpPost]
        public IActionResult CalculateResult([Bind("Questions,SubmittedQuestions,ViewNumber")] CustomViewViewModel viewModel)
        {
            if (viewModel == null || viewModel.Questions == null || viewModel.Questions.Count == 0)
            {
                return NotFound(); // Handle invalid input gracefully
            }

            // Retrieve the correct answers for the selected view (e.g., View1, View2, or View3) from the database
            List<Question> correctAnswers = null;

            if (viewModel.ViewNumber == 1)
            {
                correctAnswers = _dbContext.Questions.Include(q => q.Answers).Where(q => q.Id >= 1 && q.Id <= 10).ToList();


            }
            else if (viewModel.ViewNumber == 2)
            {
                correctAnswers = _dbContext.Questions.Include(q => q.Answers).Where(q => q.Id >= 11 && q.Id <= 20).ToList();

            }
            else if (viewModel.ViewNumber == 3)
            {
                correctAnswers = _dbContext.Questions.Include(q => q.Answers).Where(q => q.Id >= 21 && q.Id <= 30).ToList();

            }

            if (correctAnswers == null || correctAnswers.Count == 0)
            {
                return NotFound(); // Handle the case where there are no correct answers for the selected view
            }

            //calculate the score based on the submitted answers and correct options
            //if (viewModel.SubmittedQuestions == null)
            //{
            //    viewModel.SubmittedQuestions = new List<SubmittedQuestionViewModel>(); // Initialize it as an empty list
            //}


            int correctCount = 0;

            foreach (var submittedQuestion in viewModel.SubmittedQuestions)
            {
                var correctAnswer = correctAnswers.FirstOrDefault(q => q.Id == submittedQuestion.QuestionId);
                if (correctAnswer != null)
                {
                    // Check if the submitted answer index is valid
                    if (submittedQuestion.CorrectOptionIndex >= 0 && submittedQuestion.CorrectOptionIndex < correctAnswer.Answers.Count)
                    {
                        var selectedOption = correctAnswer.Answers[submittedQuestion.CorrectOptionIndex];

                        // Find the corresponding correct answer for the question
                        var correct = correctAnswers.FirstOrDefault(q => q.Id == correctAnswer.Id);

                        if (correct != null)
                        {
                            var correctOption = correctAnswer.Answers.FirstOrDefault(o => o.IsCorrect);

                            if (correctOption != null && correctOption.Id == selectedOption.Id)
                            {
                                correctCount++;
                            }
                        }
                    }
                }
            }

            double totalQuestions = viewModel.Questions.Count;
            double percentage = (correctCount / totalQuestions) * 100;
            ViewBag.Percentage = percentage;
            return RedirectToAction("Result", new { percentage });
            
           
        }



        [HttpGet]
        public ActionResult CreateAllQuestions(int ViewNumber)
        {
            //var questions = _questionService.GetQuestionsForView(ViewNumber)
            var isAuthenticated = HttpContext.Session.GetString("IsAuthenticated");

            if (isAuthenticated == "true")
            {
                
           
                var model = new CreateQuestionViewModel
                {
                    //Text = QuestionText,
                    ViewNumber = ViewNumber,
                    Answers = new List<Answer>
                {
                    new Answer()

                }
                };
                return View(model);
            }
            return RedirectToAction("Login"); // Redirect to a login page or show an access denied view
        }
        [HttpPost]
        public ActionResult CreateAllQuestions(CreateQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var question = new Question
                {
                    Text = model.QuestionText,
                    Answers = model.Answers
                };
                _dbContext.Questions.Add(question);
                _dbContext.SaveChanges();
                //return RedirectToAction("Index");
                if (model.ViewNumber == 1)
                {
                    return RedirectToAction("Support");
                }
                else if (model.ViewNumber == 2)
                {
                    return RedirectToAction("Development");
                }
                else if (model.ViewNumber == 3)
                {
                    return RedirectToAction("DataAnalysis");
                }
                else
                {
                    return NotFound();
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string password)
        {
            // Check if the provided password is correct 
            if (password == Startup.DemoPassword)
            {
                

                HttpContext.Session.SetString("IsAuthenticated", "true");

                // Redirect to the CreateQuiz page after successful login
                return RedirectToAction("CreateAllQuestions");
            }
            else
            {
                // Password is incorrect, show an error message or redirect to a login page with an error
                ModelState.AddModelError(string.Empty, "Invalid password you are not Authorised to access this page.");
                return View();
            }
        }

     [HttpGet]
        public ActionResult Index(int ViewNumber)
        {
            //var questions = _dbContext.Questions.Include(q => q.Answers).OrderBy(q => q.Id).ToList();
            var viewModel = new CustomViewViewModel
            {
                ViewNumber = ViewNumber,
                Questions = new List<Question>() //_questionService.GetQuestionsForView(ViewNumber)Initialize an empty 
            }; 
            string viewName = null;
            //switch (viewNumber)
            //{
            //    case 1:
            //        questions = _dbContext.Questions.Include(q => q.Answers).Where(q => q.Id >= 1 && q.Id <= 3).ToList();
            //        viewName = "Support2";
            //       break;
            //    case 2:
            //       questions = _dbContext.Questions.Include(q => q.Answers).Where(q => q.Id >= 4 && q.Id <= 6).ToList();
            //        viewName = "Development";
            //        break;
            //   case 3:
            //        questions = _dbContext.Questions.Include(q => q.Answers).Where(q => q.Id >= 7 && q.Id <= 9).ToList();
            //        viewName = "DataAnalysis";
            //        break;
            //   default:
            //        return NotFound();
            //}
            if (ViewNumber == 1)
            {
                // Questions for View1 (e.g., questions 1 to 3)
                viewModel.Questions = _dbContext.Questions.Include(q => q.Answers).Where(q => q.Id >= 1 && q.Id <= 10).ToList();
                viewName = "Support"; // Change this to your desired view name for View1
            }
            else if (ViewNumber == 2)
            {
                // Questions for View2 (e.g., questions 4 to 6)
                viewModel.Questions = _dbContext.Questions.Include(q => q.Answers).Where(q => q.Id >= 11 && q.Id <= 20).ToList();
                viewName = "Development"; // Change this to your desired view name for View2
            }
            else if (ViewNumber == 3)
            {
                // Questions for View3 (e.g., questions 7 to 9)
                viewModel.Questions = _dbContext.Questions.Include(q => q.Answers).Where(q => q.Id >= 21 && q.Id <= 30).ToList();
                viewName = "DataAnalysis"; // Change this to your desired view name for View3
            }
            else
            {
                return NotFound();
            }

            return View(viewName, viewModel);
        }
        public ActionResult Support()
        {
           
               
                List<Question> questions = _dbContext.Questions.Include(q => q.Answers).Where(q => q.Id >= 1 && q.Id <= 10).ToList();

                var viewModel = new CustomViewViewModel
                {
                    Questions = questions,
                    ViewNumber = 1,
                };
                ViewBag.ViewNumber = 1; // Pass the viewNumber to the view
                return View("Support", viewModel);
            
        }



            //[Authorize(Roles = "Development")]
            public ActionResult Development()
            {
                List<Question> questions = _dbContext.Questions.Include(q => q.Answers).Where(q => q.Id >= 11 && q.Id <= 20).ToList();
                var viewModel = new CustomViewViewModel
                {
                    Questions = questions,
                    ViewNumber = 2
                };
                ViewBag.ViewNumber = 2; // Pass the viewNumber to the view
                return View("Development", viewModel);
            }
            //[Authorize(Roles = "Data Analyst")]
            public ActionResult DataAnalysis()
            {
                List<Question> questions = _dbContext.Questions.Include(q => q.Answers).Where(q => q.Id >= 21 && q.Id <= 30).ToList();
                var viewModel = new CustomViewViewModel
                {
                    Questions = questions,
                    ViewNumber = 3
                };
                ViewBag.ViewNumber = 3; // Pass the viewNumber to the view
                return View("DataAnalysis", viewModel);
            }

        }
    }
