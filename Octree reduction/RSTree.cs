using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Octree_reduction
{
    public class Octree
    {
        public class OctNode
        {
            public Octree Tree;
            public int PixelCount;
            public int depth;
            public OctNode[] children;
            public int ChildrenCount { get; private set; }
            public bool Leaf => ChildrenCount == 0;
            public int Branch(Color color)
            {
                return ((color.R >> (7 - depth)) & 1) * 4
                    + ((color.G >> (7 - depth)) & 1) * 2
                    + ((color.B >> (7 - depth)) & 1) * 1;
            }
            public OctNode(Octree tree, int depth)
            {
                this.Tree = tree;
                this.depth = depth;
                children = new OctNode[8];
            }
            public void InitChild(int child)
            {
                if (children[child] != null) throw new Exception("Child already present");
                children[child] = new OctNode(Tree, this.depth + 1);
                ChildrenCount++;
                Tree.LeavesCount++;
            }
            public void RemoveChild(int child)
            {
                if (children[child] == null || !children[child].Leaf) throw new Exception("Cant remove this child");
                children[child] = null;
                ChildrenCount--;
                Tree.LeavesCount--;
            }

            public OctNode GetCopy(Octree copyTree)
            {
                OctNode res = new OctNode(copyTree, this.depth);
                res.PixelCount = PixelCount;
                res.ChildrenCount = ChildrenCount;
                for(int i = 0; i < 8; i++)
                {
                    res.children[i] = children[i]?.GetCopy(copyTree);
                }
                return res;
            }
        }
        public OctNode root;
        public int LeavesCount { get; private set; }
        public Octree()
        {
            root = new OctNode(this, 0);
        }
        public void InsertColor(Color color)
        {
            InsertColor(root, color);
        }
        public Octree GetCopy()
        {
            Octree res = new Octree();
            res.root = root.GetCopy(res);
            res.LeavesCount = LeavesCount;
            return res;
        }
        public void InsertColor(OctNode node, Color color)
        {
            if(node.Leaf)
            {
                node.PixelCount++;
            }
            else
            {
                int next = node.Branch(color);
                if (node.children[next] == null)
                    node.InitChild(next);
                InsertColor(node.children[next], color);
            }
        }
        public void Reduce(int resultingLeavesCount)
        {
            for(int i = 8; i > 0; i--)
            {
                Reduce(root, resultingLeavesCount, i);
            }
        }
        private static readonly int[] indexes = { 0, 1, 2, 3, 4, 5, 6, 7 };
        private void Reduce(OctNode node, int resultingLeavesCount, int atLevel)
        {
            Array.Sort(indexes, (int i1, int i2) =>
            node.children[i1] == null ? 1
            : node.children[i2] == null ? -1
            : node.children[i1].PixelCount.CompareTo(node.children[i2].PixelCount));
            for(int i = 0; i < 8 && node.children[indexes[i]] != null && resultingLeavesCount > LeavesCount; i++)
            {
                if(atLevel - 1 == node.depth)
                {
                    node.PixelCount += node.children[indexes[i]].PixelCount;
                    node.RemoveChild(indexes[i]);
                }
                else
                {
                    Reduce(node.children[indexes[i]], resultingLeavesCount, atLevel);
                }
            }
        }
    }
}
