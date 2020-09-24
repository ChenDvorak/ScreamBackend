namespace Screams.Screams
{
    /// <summary>
    /// this is a  paging of scream
    /// </summary>
    public class ScreamPaging: Infrastructures.Paging<ScreamPaging.SingleItem>
    {
        private ScreamPaging(int index, int size, int capacity = 0) : base(index, size, capacity)
        { }

        public static ScreamPaging Create(int index, int size = 20, int capacity = 0)
        {
            return new ScreamPaging(index <= 0 ? 1 : index, size < 0 ? DEFAULT_SIZE : size, capacity);
        }

        /// <summary>
        /// item of scream paging list
        /// </summary>
        public class SingleItem
        {
            public int Id { get; set; }
            public int AuthorId { get; set; }
            public string Author { get; set; }
            public string Content { get; set; }
            public bool IsFullContent { get; set; }
            public int HiddenCount { get; set; }
            public string DateTime { get; set; }
        }
    }
}
