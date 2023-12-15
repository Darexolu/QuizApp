namespace QuizApp.Models
{
    public class DepartmentUser
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public Department Department { get; set; }

    }
}
