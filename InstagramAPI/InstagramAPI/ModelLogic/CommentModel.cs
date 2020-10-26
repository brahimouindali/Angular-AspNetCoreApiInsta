namespace InstagramAPI.ModelLogic
{
    public class CommentModel
    {
        public int Id { get; set; }
        public int MediaId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
    }
}
