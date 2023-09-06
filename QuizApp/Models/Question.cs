using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace QuizApp.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        [ValidateNever]
        public List<Answer> Answers { get; set; }
        //[ForeignKey(nameof(Answer))]    

    }
}
