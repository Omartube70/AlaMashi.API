using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class PhoneAlreadyExistsException : ConflictException
    {
        public PhoneAlreadyExistsException(string phone)
            : base($"The phone '{phone}' is already registered.")
        {
        }
    }
}
