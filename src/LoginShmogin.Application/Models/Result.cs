using System.Collections.Generic;
using System.Linq;

namespace LoginShmogin.Application.Models
{
    public class Result
    {
        internal Result(bool succeeded, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
            Is2FARequired = false;
            IsLockedOut = false;
        }

        internal Result(bool succeeded, IEnumerable<string> errors, bool is2FARequires) : this(succeeded, errors)
        {
            Is2FARequired = is2FARequires;
        }

        public bool Succeeded { get; set; }
        public bool Is2FARequired { get; set; }
        public bool IsLockedOut { get; set; }

        public string[] Errors { get; set; }

        public static Result Success()
        {
            return new Result(true, new string[] { });
        }
        public static Result TwoFactorRequired()
        {
            return new Result(false, new string[] { }, true);
        }

        public static Result Failure(IEnumerable<string> errors)
        {
            return new Result(false, errors);
        }
        public static Result LockedOut()
        {
            return new Result(false, new string[] { "User account locked out." });
        }
    }
}