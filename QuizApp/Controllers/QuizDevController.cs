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
    public class QuizDevController : Controller
    {
        //private readonly IQuizRepository _quizRepository;
        private readonly AppDbContext _dbContext;


        public QuizDevController(AppDbContext db)
        {
            _dbContext = db;

        }


        //private List<Question> _questions = new List<Question>{

        ////    new Question
        ////    {Text= "What is the full meaning of SEO?",
        ////     //Answers = Answer
        ////     CorrectAnswerId = 0
        ////    },             //Answers = new List<Answer>()
        ////     //{

        ////     //}
        ////    new Question
        ////    {Text= "What is the full Meaning of LAN?",

        ////     CorrectAnswerId = 1
        ////    }
        //};
        public IActionResult CreateQuiz()
        {
            //var questions = _dbContext.Questions.Include(q => q.Answers).ToList();

            return View();

        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult CreateQuiz2(Question question)
        {

            if (ModelState.IsValid)
            {
                _dbContext.Questions.Add(question);
                _dbContext.SaveChanges();
                return RedirectToAction("Development","QuizDev");
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
        [HttpPost]
        public IActionResult CalculatePercentage(List<Question> submittedQuestions)
        {
            //    //List<Question> _questions = _quizRepository.GetAll().ToList();

            int totalQuestions = submittedQuestions.Count;
            int correctAnswers = submittedQuestions.Count(q => q.Answers.Any(o => o.IsCorrect));

            double percentage = (double)correctAnswers / totalQuestions * 100;
            ViewBag.Percentage = percentage;


            foreach (var submittedQuestion in submittedQuestions)
            {

                var dbQuestion = _dbContext.Questions.Include(q => q.Answers).FirstOrDefault(q => q.Id == submittedQuestion.Id);
                if (dbQuestion != null)
                {
                    foreach (var option in submittedQuestion.Answers)
                    {
                        var dbOption = dbQuestion.Answers.FirstOrDefault(o => o.Id == option.Id);
                        if (dbOption != null)
                        {
                            dbOption.IsCorrect = option.IsCorrect;
                        }
                    }
                }

            }
            _dbContext.SaveChanges();
            return View();

        }

        public ActionResult Result()
        {

            return View();
        }
        [Authorize(Roles = "Development")]
        public ActionResult Development()
        {
            var question = _dbContext.Questions.Include(q => q.Answers).ToList();

            return View(question);
        }
        [Authorize(Roles = "Data Analyst")]
        public ActionResult DataAnalysis()
        {
            return View();
        }


    }
}
