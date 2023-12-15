namespace QuizApp.Models
{
    public class Department
    {

        public int Id { get; set; }
        public string Support { get; set; }
        public string Development { get; set; }
        public string DataAnalysis { get; set; }
        // Navigation property representing the collection of users in this department
        public ICollection<DepartmentUser> Users { get; set; } = new List<DepartmentUser>();
    }
}
