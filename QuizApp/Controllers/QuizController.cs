using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuizApp.Data;
using QuizApp.Models;
using QuizApp.Repositories.IRepository;
using static System.Net.Mime.MediaTypeNames;

namespace QuizApp.Controllers
{
    public class QuizController : Controller
    {
        //private readonly IQuizRepository _quizRepository;
        private readonly AppDbContext _dbContext;


        public QuizController(AppDbContext db)
        {
            _dbContext = db;
           
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
                return RedirectToAction("Support","Quiz");
            }
           
            return View(question);

        }

        //[Authorize(Roles = "Support")]
        public ActionResult Support()
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
            if(answers != null && answers.Count > 0)
            {
                var questions = _dbContext.Questions.Include(q => q.Answers).ToList();
                double correctCount = 0;
                for(int i = 0; i < answers.Count; i++)
                {
                    int selecteOptionIndex = answers[i];
                    if(selecteOptionIndex >= 0 && selecteOptionIndex < questions[i].Answers.Count)
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
            return View("Support");
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
                correctAnswers = _dbContext.Questions.Include(q => q.Answers).Where(q => q.Id >= 1 && q.Id <= 3).ToList();


            }
            else if (viewModel.ViewNumber == 2)
            {
                correctAnswers = _dbContext.Questions.Include(q => q.Answers).Where(q => q.Id >= 4 && q.Id <= 6).ToList();

            }
            else if (viewModel.ViewNumber == 3)
            {
                correctAnswers = _dbContext.Questions.Include(q => q.Answers).Where(q => q.Id >= 7 && q.Id <= 9).ToList();

            }

            if (correctAnswers == null || correctAnswers.Count == 0)
            {
                return NotFound(); // Handle the case where there are no correct answers for the selected view
            }

            // Calculate the score based on the submitted answers and correct options
            int correctCount = 0;

            foreach (var question in viewModel.Questions)
            {
                var submittedQuestion = viewModel.SubmittedQuestions.FirstOrDefault(sq => sq.QuestionId == question.Id);

                if (submittedQuestion != null)
                {
                    var correctAnswer = correctAnswers.FirstOrDefault(q => q.Id == submittedQuestion.QuestionId);

                    if (correctAnswer != null && correctAnswer.Answers.Any(o => o.IsCorrect) &&
                        submittedQuestion.CorrectOptionIndex >= 0 && submittedQuestion.CorrectOptionIndex < correctAnswer.Answers.Count &&
                        correctAnswer.Answers[submittedQuestion.CorrectOptionIndex].IsCorrect)
                    {
                        correctCount++;
                    }
                }
            }

            double totalQuestions = correctAnswers.Count;
            double percentage = (correctCount / totalQuestions) * 100;

            // Redirect to a result view with the calculated percentage
            return RedirectToAction("Result", new { percentage });
        }
       

         [HttpGet]
        public ActionResult CreateAllQuestions( int viewNumber)
        {
            var model = new CreateQuestionViewModel
            {
                ViewNumber = viewNumber,
                Answers = new List<Answer>
                {
                    new Answer()
      
                }
            };
            return View(model);
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
                    return RedirectToAction("Support2");
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
        public ActionResult Index(int viewNumber)
        {
            //var questions = _dbContext.Questions.Include(q => q.Answers).OrderBy(q => q.Id).ToList();
          
            List<Question> questions = null;
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
            if (viewNumber == 1)
            {
                // Questions for View1 (e.g., questions 1 to 3)
                questions = _dbContext.Questions.Include(q => q.Answers).Where(q => q.Id >= 1 && q.Id <= 3).ToList();
                viewName = "Support2"; // Change this to your desired view name for View1
            }
            else if (viewNumber == 2)
            {
                // Questions for View2 (e.g., questions 4 to 6)
                questions = _dbContext.Questions.Include(q => q.Answers).Where(q => q.Id >= 4 && q.Id <= 6).ToList();
                viewName = "Development"; // Change this to your desired view name for View2
            }
            else if (viewNumber == 3)
            {
                // Questions for View3 (e.g., questions 7 to 9)
                questions = _dbContext.Questions.Include(q => q.Answers).Where(q => q.Id >= 7 && q.Id <= 9).ToList();
                viewName = "DataAnalysis"; // Change this to your desired view name for View3
            }
            else
            {
                return NotFound();
            }

            return View(viewName, questions);
        }
        public ActionResult Support2(int viewNumber)
        {
            List<Question> questions = _dbContext.Questions.Include(q => q.Answers).Where(q => q.Id >= 1 && q.Id <= 3).ToList();
            var viewModel = new CustomViewViewModel
            {
                Questions = questions,
                ViewNumber = viewNumber
            };
            ViewBag.ViewNumber = viewNumber; // Pass the viewNumber to the view
            return View("Support2",viewModel);
        }
        //[Authorize(Roles = "Development")]
        public ActionResult Development(int viewNumber)
        {
            List<Question> questions = _dbContext.Questions.Include(q => q.Answers).Where(q => q.Id >= 4 && q.Id <= 6).ToList();
            var viewModel = new CustomViewViewModel
            {
                Questions = questions,
                ViewNumber = viewNumber
            };
            ViewBag.ViewNumber = viewNumber; // Pass the viewNumber to the view
            return View("Development",viewModel);
        }
        //[Authorize(Roles = "Data Analyst")]
        public ActionResult DataAnalysis(int viewNumber)
        {
            List<Question> questions = _dbContext.Questions.Include(q => q.Answers).Where(q => q.Id >= 7 && q.Id <= 9).ToList();
            var viewModel = new CustomViewViewModel
            {
                Questions = questions,
                ViewNumber = viewNumber
            };
            ViewBag.ViewNumber = viewNumber; // Pass the viewNumber to the view
            return View("DataAnalysis",viewModel);
        }

    }
}
