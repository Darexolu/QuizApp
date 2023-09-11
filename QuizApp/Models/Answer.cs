using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace QuizApp.Models
{
    public class Answer
    {
        [ValidateNever]
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; } 
        public int QuestionId { get; set; }
        [ValidateNever]
        public Question Question{ get; set; }
   





}
}
