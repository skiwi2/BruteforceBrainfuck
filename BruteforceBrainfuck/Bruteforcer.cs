using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using BruteforceBrainfuck.Interpreter;

namespace BruteforceBrainfuck
{
    public class Bruteforcer
    {
        private IList<char> Symbols { get; set; }

        private IList<Testcase> Testcases { get; set; }

        private TimeSpan MaxTime { get; set; }

        private uint MemorySize { get; set; }

        public Bruteforcer(IList<char> symbols, IList<Testcase> testcases, TimeSpan maxTime, uint memorySize)
        {
            Symbols = symbols;
            Testcases = testcases;
            MaxTime = maxTime;
            MemorySize = memorySize;
        }

        public string ComputeProgram()
        {
            var blockingQueue = new BlockingCollection<string>();
            foreach (var symbol in Symbols)
            {
                blockingQueue.Add(symbol.ToString());
            }
            foreach (var program in blockingQueue.GetConsumingEnumerable())
            {
                var cancellationToken = new CancellationTokenSource(MaxTime).Token;
                Task.Run(() =>
                {
                    Console.WriteLine("Verifying: " + program);
                    if (VerifyProgram(program, cancellationToken, out var shouldContinue))
                    {
                        Console.WriteLine("Found program: " + program);
                    }
                    if (shouldContinue)
                    {
                        foreach (var symbol in Symbols)
                        {
                            blockingQueue.Add(program + symbol);
                        }
                    }
                    else
                    {
                        //Console.WriteLine("Killed off: " + program);
                    }
                }, cancellationToken);
            }
            return null;
        }

        private bool VerifyProgram(string programText, CancellationToken cancellationToken, out bool shouldContinue)
        {
            try
            {
                var program = new BFProgram(programText, new BFMemory(MemorySize));
                foreach (var testcase in Testcases)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        shouldContinue = false;
                        return false;
                    }
                    var output = program.Execute(testcase.Input, cancellationToken);
                    if (!Enumerable.SequenceEqual(testcase.Output, output))
                    {
                        shouldContinue = true;
                        return false;
                    }
                }
                shouldContinue = true;
                return true;
            }
            catch (InvalidBFProgramException ex)
            {
                shouldContinue = ex.Recoverable;
                return false;
            }
        }
    }
}
