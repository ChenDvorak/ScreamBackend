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
        public static AccountResult Unsuccessful => new AccountResult
        {
            Succeeded = false
        };
        public bool Succeeded { get; protected set; }
 
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

        public static new AccountResult Unsuccessful(T data)
        {
            var baseResult = AccountResult.Unsuccessful;
            return Parse(baseResult, data);
        }
        private static AccountResult<T> Parse(AccountResult baseResult, T data)
        {
            var result = new AccountResult<T>
            {
                Succeeded = baseResult.Succeeded,
                Data = data
            };
            result.Data = data;
            return result;
        }

        public T Data { get; protected set; }
    }
}
