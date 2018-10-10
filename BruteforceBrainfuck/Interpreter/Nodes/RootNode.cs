using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BruteforceBrainfuck.Interpreter.Nodes
{
    public class RootNode
    {
        public ICollection<INode> Children { get; private set; }

        public RootNode(ICollection<INode> children)
        {
            Children = children;
        }
    }
}
