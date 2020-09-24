namespace Screams.Comments
{
    /// <summary>
    /// this is a  paging of scream
    /// </summary>
    public class CommentPaging : Infrastructures.Paging<CommentPaging.Comment>
    {
        private CommentPaging(int index, int size, int capacity = 0) : base(index, size, capacity)
        { }

        public static CommentPaging Create(int index, int size = 20, int capacity = 0)
        {
            return new CommentPaging(index <= 0 ? 1 : index, size < 0 ? DEFAULT_SIZE : size, capacity);
        }

        /// <summary>
        /// item of scream paging list
        /// </summary>
        public class Comment
        {
            public int Id { get; set; }
            public int AuthorId { get; set; }
            public string Author { get; set; }
            public string Content { get; set; }
            public int HiddenCount { get; set; }
            public string DateTime { get; set; }
        }
    }
}
