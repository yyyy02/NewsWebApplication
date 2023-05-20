namespace NewsWebApplication.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string? CommentDetail { get; set; }

        public int NewId { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public virtual New New { get; set; } = null!;
    }
}
