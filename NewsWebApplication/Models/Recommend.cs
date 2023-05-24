namespace NewsWebApplication.Models
{
    public class Recommend
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? NewId { get; set; }
        public int? Recommendation { get; set; }

        public DateTime? ReadTime { get; set; }

    }
}
