namespace Screams
{
    /// <summary>
    /// this is a  paging of scream
    /// </summary>
    public class Screams: Infrastructures.Paging<Screams.SingleItem>
    {
        private Screams(int index, int size, int capacity = 0) : base(index, size, capacity)
        { }

        public static Screams Create(int index, int size = 20, int capacity = 0)
        {
            return new Screams(index <= 0 ? 1 : index, size < 0 ? DEFAULT_SIZE : size, capacity);
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
            public string DateTime { get; set; }
        }
    }
}
