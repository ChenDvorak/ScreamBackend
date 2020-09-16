using System.Collections.Generic;
using System.Linq;

namespace Screams
{
    /// <summary>
    /// quice result
    /// use this to quick instance a scream result
    /// </summary>
    public static class QuickResult
    {
        /// <summary>
        /// quice get a success scream result without data
        /// </summary>
        /// <returns></returns>
        public static ScreamResult Successful() => new ScreamResult(success: true);
        /// <summary>
        /// quice get a success scream result with data
        /// </summary>
        public static ScreamResult<T> Successful<T>(T data)
        {
            var result = new ScreamResult<T>(success: true)
            {
                Data = data
            };
            return result;
        }
        /// <summary>
        /// quice get a unsuccess scream result without data
        /// </summary>
        /// <returns></returns>
        public static ScreamResult Unsuccessful(params string[] errors)
        {
            var result = new ScreamResult(success: false)
            {
                Errors = errors?.ToList() ?? new List<string>(0)
            };
            return result;
        }

        /// <summary>
        /// quice get a unsuccess scream result with data
        /// </summary>
        public static ScreamResult<T> Unsuccessful<T>(T data, params string[] errors)
        {
            var result = new ScreamResult<T>(success: false)
            {
                Data = data,
                Errors = errors?.ToList() ?? new List<string>(0)
            };
            return result;
        }
    }

    /*
     *  there are tow scream result follow the code
     *  obviously difference only the property that named Data
     *  ScreamResult without result data
     *  ScreamResult<T> within result data
     */

    /// <summary>
    /// the result from scream process without result data
    /// </summary>
    public class ScreamResult
    {
        internal ScreamResult(bool success)
        {
            Successed = success;
        }
        /// <summary>
        /// is this result successfull
        /// </summary>
        public bool Successed { get; } = false;
        /// <summary>
        /// error informations if result unseccessful
        /// </summary>
        public List<string> Errors { get; internal set; }
    }

    /// <summary>
    /// the result from scream process within result data
    /// </summary>
    public class ScreamResult<T> : ScreamResult
    {
        internal ScreamResult(bool success) : base(success)
        { }

        /// <summary>
        /// result data if necessary
        /// </summary>
        public T Data { get; internal set; }
    }
}
