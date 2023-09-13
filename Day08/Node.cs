using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day08
{
    internal class Node
    {
        public Node(int nChildren, int nMetadata)
        {
            Children = new List<Node>();
            Metadata = new List<int>();
            NChildren = nChildren;
            NMetadata = nMetadata;
            Value = 0;
        }
        public int NChildren { get; set; }
        public int NMetadata { get; set; }
        public List<Node> Children { get; set; }
        public List<int> Metadata { get; set; }

        public int Value { get; set; }

        public override string ToString()
        {
            return string.Format("Children: {0}, Metadata: {1}, Value: {2}", Children.Count, Metadata.Count, Value);
        }
    }
}
