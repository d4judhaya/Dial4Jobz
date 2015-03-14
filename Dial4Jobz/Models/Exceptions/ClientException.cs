using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dial4Jobz.Models.Exceptions
{
    /// <summary>
    /// Exception caused by an incorrect action by client.  This exception's message can be displayed to the user.
    /// </summary>
    public class ClientException : Exception
    {
        public ClientException()
        {
        }

        public ClientException(string message)
            : base(message)
        {
        }
    }
}