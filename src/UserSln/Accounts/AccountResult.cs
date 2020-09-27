using System;
using System.Collections.Generic;
using System.Text;

namespace Accounts
{
    /// <summary>
    /// Result
    /// </summary>
    public class AccountResult
    {
        /// <summary>
        /// Return a result of succeed
        /// </summary>
        public static AccountResult Successful => new AccountResult
        {
            Succeeded = true
        };

        public static AccountResult Unsuccessful(params string[] errors)
        {
            return new AccountResult
            {
                Succeeded = false,
                Errors = errors
            };
        }

        public bool Succeeded { get; protected set; }

        public ICollection<string> Errors { get; set; }
    }

    public class AccountResult<T> : AccountResult
    {
        /// <summary>
        /// Return a result of succeed
        /// </summary>
        public static new AccountResult<T> Successful(T data)
        {
            var result = AccountResult.Successful as AccountResult<T>;
            result.Data = data;
            return result;
        }

        public static AccountResult Unsuccessful(T data, params string[] errors)
        {
            var baseResult = Unsuccessful(errors);
            return Parse(baseResult, data, baseResult.Errors);
        }
        private static AccountResult<T> Parse(AccountResult baseResult, T data, ICollection<string> errors)
        {
            var result = new AccountResult<T>
            {
                Succeeded = baseResult.Succeeded,
                Data = data,
                Errors = errors
            };
            result.Data = data;
            return result;
        }

        public T Data { get; protected set; }
    }
}
