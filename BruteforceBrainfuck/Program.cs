using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using BruteforceBrainfuck.Interpreter;

namespace BruteforceBrainfuck
{
    class Program
    {
        static void Main(string[] args)
        {
            //var program = new BFProgram("+[-->-[>>+>-----<<]<--<---]>-.>>>+.>>..+++[.>]<<<<.+++.------.<<-.>>>>+.", new BFMemory(30000));
            //var result = program.Execute(new List<byte>(), CancellationToken.None);

            //var program = new BFProgram(",[->++<]>.", new BFMemory(30000));
            //var result = program.Execute(new List<byte> { 21 }, CancellationToken.None);

            //var outputText = string.Join("", result.Select(b => (char)b));
            //Console.WriteLine(outputText);

            var bruteforcer = new Bruteforcer(
                new List<char> { '>', '<', '+', '-', '.', ',', '[', ']' },
                new List<Testcase>
                {
                    new Testcase(
                        new List<byte> { 1 },
                        new List<byte> { 2, 0 }
                    ),
                    new Testcase(
                        new List<byte> { 2 },
                        new List<byte> { 3, 0 }
                    ),
                    new Testcase(
                        new List<byte> { 3 },
                        new List<byte> { 4, 0 }
                    )
                },
                TimeSpan.FromMilliseconds(100d),
                100
            );
            var program = bruteforcer.ComputeProgram();
            Console.WriteLine();
            Console.WriteLine(program);
        }
    }
}
