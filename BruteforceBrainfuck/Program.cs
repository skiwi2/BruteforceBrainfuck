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
            var program = new BFProgram("+[-->-[>>+>-----<<]<--<---]>-.>>>+.>>..+++[.>]<<<<.+++.------.<<-.>>>>+.", new BFMemory(30000));
            var result = program.Execute(new List<byte>());

            //var program = new BFProgram(",[->++<].", new BFMemory(30000));
            //var result = program.Execute(new List<byte> { 21 });

            var outputText = string.Join("", result.Select(b => (char)b));
            Console.WriteLine(outputText);
        }
    }
}
