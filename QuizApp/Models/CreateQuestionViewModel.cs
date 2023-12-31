﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace QuizApp.Models
{
    public class CreateQuestionViewModel
    {
        public int Id { get; set; }
        [ValidateNever]
        public int ViewNumber { get; set; }
        public string QuestionText { get; set; }
        public int CorrectOptionIndex { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
