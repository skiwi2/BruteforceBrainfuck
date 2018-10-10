using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BruteforceBrainfuck.Interpreter
{
    public class InvalidProgramException : Exception
    {
        public InvalidProgramException() : base()
        {

        }

        public InvalidProgramException(string message) : base(message)
        {

        }
    }
}
