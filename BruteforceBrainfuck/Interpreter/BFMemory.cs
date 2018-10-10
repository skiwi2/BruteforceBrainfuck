using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BruteforceBrainfuck.Interpreter
{
    public class BFMemory
    {
        private uint MemorySize { get; set; }

        private byte[] Cells { get; set; }

        private uint Pointer { get; set; }

        public BFMemory(uint memorySize)
        {
            MemorySize = memorySize;
            Cells = new byte[memorySize];
            Pointer = 0;
        }

        public void MoveRight()
        {
            Pointer = (Pointer + 1) % MemorySize;
        }

        public void MoveLeft()
        {
            Pointer = (Pointer + (MemorySize - 1)) % MemorySize;
        }

        public void Increment()
        {
            Cells[Pointer]++;
        }

        public void Decrement()
        {
            Cells[Pointer]--;
        }

        public void SetValue(byte value)
        {
            Cells[Pointer] = value;
        }

        public byte GetValue()
        {
            return Cells[Pointer];
        }
    }
}
