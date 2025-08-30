using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class InvalidCredentialsException : Exception
    {
        // Constructor افتراضي برسالة عامة
        public InvalidCredentialsException(): base("Invalid email or password.")
        {
        }

        // Constructor يسمح بإرسال رسالة مخصصة
        public InvalidCredentialsException(string message) : base(message)
        {
        }
    }
}
