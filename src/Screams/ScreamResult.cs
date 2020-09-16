using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Screams
{
    /// <summary>
    /// quice result
    /// </summary>
    public static class Result
    {
        /// <summary>
        /// quice get a success scream result without data
        /// </summary>
        /// <returns></returns>
        public static ScreamResult<object> Successful() => new ScreamResult<object>(success: true);
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
        public static ScreamResult<object> Unsuccessful(params string[] errors)
        {
            var result = new ScreamResult<object>(success: false)
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
    /// <summary>
    /// the result from scream process
    /// </summary>
    public class ScreamResult<T>
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
        /// result data if necessary
        /// </summary>
        public T Data { get; internal set; }
        /// <summary>
        /// error informations if result unseccessful
        /// </summary>
        public List<string> Errors { get; internal set; }
    }
}
