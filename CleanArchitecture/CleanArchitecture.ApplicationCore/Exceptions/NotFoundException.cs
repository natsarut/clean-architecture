using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.ApplicationCore.Exceptions
{
    public class NotFoundException(string message) : Exception(message)
    {
    }
}
