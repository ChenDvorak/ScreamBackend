using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.IdentityModel.Tokens; 

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
        public static AccountResult Successful() => new AccountResult
        {
            Succeeded = true
        };

        public static AccountResult<T> Successful<T>(T data)
        {
            var result = Successful();
            return AccountResult<T>.Parse(result, data, new List<string>());
        }

        public static AccountResult Unsuccessful(params string[] errors)
        {
            return new AccountResult
            {
                Succeeded = false,
                Errors = errors
            };
        }

        public static AccountResult<T> Unsuccessful<T>(T data, params string[] errors)
        {
            var baseResult = Unsuccessful(errors);
            return AccountResult<T>.Parse(baseResult, data, baseResult.Errors);
        }

        public bool Succeeded { get; protected set; }

        public ICollection<string> Errors { get; set; }
    }

    public class AccountResult<T> : AccountResult
    {
        internal static AccountResult<T> Parse(AccountResult baseResult, T data, ICollection<string> errors)
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
