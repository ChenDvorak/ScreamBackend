namespace Screams
{
    /// <summary>
    /// this is a  paging of scream
    /// </summary>
    public class ScreamPaging: Infrastructures.Paging<ScreamPaging.ScreamItem>
    {
        private ScreamPaging(int index, int size, int capacity = 0) : base(index, size, capacity)
        { }

        public static ScreamPaging Create(int index, int size = 20, int capacity = 0)
        {
            return new ScreamPaging(index <= 0 ? 1 : index, size < 0 ? 0 : size, capacity);
        }

        /// <summary>
        /// item of scream paging list
        /// </summary>
        public class ScreamItem
        {
            public int Id { get; set; }
            public int ScreamerId { get; set; }
            public string Screamer { get; set; }
            public string Content { get; set; }
            public string DateTime { get; set; }
        }
    }
}
