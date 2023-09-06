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

        public ActionResult Result()
        {
            
            return View();
        }
        //[Authorize(Roles = "Development")]
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
