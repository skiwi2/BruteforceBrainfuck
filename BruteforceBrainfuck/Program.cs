using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BruteforceBrainfuck.Interpreter;

namespace BruteforceBrainfuck
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new BFProgram("+[-->-[>>+>-----<<]<--<---]>-.>>>+.>>..+++[.>]<<<<.+++.------.<<-.>>>>+.");
            Console.WriteLine(program);
        }
    }
}
