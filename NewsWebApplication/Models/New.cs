namespace NewsWebApplication.Models
{
    public class New
    {
        public int Id { get; set; }
        public string? NewTitle { get; set; }
        public string? NewContent { get; set; }

        public string? NewColumn { get; set; }

        public string? NewPicture { get; set;}

        public DateTime? Date { get; set; }

        public int? NewHeat { get; set; }
    }
}
