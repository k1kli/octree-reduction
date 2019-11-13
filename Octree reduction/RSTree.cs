using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            public bool LowestLevel => depth == 8;
            public int Branch(int color)
            {
                color <<= depth;
                color >>= 7;
                int res = color & 1;
                color >>= 7;
                res |= color & 2;
                color >>= 7;
                res |= color & 4;
                return res;
                //return ((color >> (23 - depth)) & 1) * 4
                //    + ((color >> (15 - depth)) & 1) * 2
                //    + ((color >> (7 - depth)) & 1) * 1;
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
                if (ChildrenCount > 1)
                    Interlocked.Increment(ref Tree.leavesCount);
            }
            public void RemoveChild(int child)
            {
                if (children[child] == null || !children[child].Leaf) throw new Exception("Cant remove this child");
                children[child] = null;
                ChildrenCount--;
                if (ChildrenCount > 0)
                    Tree.LeavesCount--;
            }

        }
        public OctNode root;
        private int leavesCount = 1;
        int lowestLevel = 0;
        public int LeavesCount { get => leavesCount; private set => leavesCount = value; }
        public Octree()
        {
            root = new OctNode(this, 0);
        }
        public void LoadBitmap(DirectBitmap bitmap, IProgress<int> progressReport)
        {
            unchecked
            {
                Parallel.ForEach(bitmap.Bits, color =>
                       {
                           InsertColor(color);
                           progressReport.Report(1);
                       });
            }
        }
        public void LoadBitmapReduceAlong(DirectBitmap bitmap,
                                          IProgress<int> progressReport,
                                          int resultingLeavesCount)
        {
            foreach (int color in bitmap.Bits)
            {
                InsertColorWithReducing(color);
                Reduce(resultingLeavesCount);
                progressReport.Report(1);
            }
        }
        public void UpdateBitmap(DirectBitmap bitmap)
        {
            for (int i = 0; i < bitmap.Bits.Length; i++)
            {
                bitmap.Bits[i] = GetReducedColor(bitmap.Bits[i]);
            }
        }

        private int GetReducedColor(int color)
        {
            OctNode node = root;
            int r = 0;
            int g = 0;
            int b = 0;
            int k = 7;
            while (true)
            {
                int branch = node.Branch(color);
                if (node.children[branch] == null) break;
                node = node.children[branch];
                r |= ((branch >> 2) & 1) << k;
                g |= ((branch >> 1) & 1) << k;
                b |= ((branch >> 0) & 1) << k;
                k--;
            }
            int res = 255 << 24 | r << 16 | g << 8 | b;
            return res;
        }

        public void InsertColor(int color)
        {
            InsertColor(root, color);
        }
        public void InsertColorWithReducing(int color)
        {
            InsertColorWithReducing(root, color);
        }
        public void InsertColor(OctNode node, int color)
        {
            if (node.LowestLevel)
            {
                Interlocked.Increment(ref node.PixelCount);
            }
            else
            {
                int next = node.Branch(color);
                if (node.children[next] == null)
                {
                    lock (node.children)
                    {
                        if (node.children[next] == null)
                            node.InitChild(next);
                    }
                }
                InsertColor(node.children[next], color);
            }
        }
        public void InsertColorWithReducing(OctNode node, int color)
        {
            if (node.LowestLevel)
            {
                node.PixelCount++;
            }
            else
            {
                int next = node.Branch(color);
                if (node.children[next] == null)
                {
                    node.InitChild(next);
                }
                InsertColorWithReducing(node.children[next], color);
            }
        }
        public void Reduce(int resultingLeavesCount)
        {
            for (int i = 8; i > 0 && resultingLeavesCount < LeavesCount; i--)
            {
                Reduce(root, resultingLeavesCount, i);
            }
        }
        private static readonly List<int> indexes = new List<int>();
        private void Reduce(OctNode node, int resultingLeavesCount, int atLevel)
        {
            if (atLevel - 1 == node.depth)
            {
                indexes.Clear();
                for (int i = 0; i < 8; i++)
                {
                    if (node.children[i] != null) indexes.Add(i);
                }
                indexes.Sort((int i1, int i2) =>
                node.children[i1].PixelCount.CompareTo(node.children[i2].PixelCount));
                for (int i = 0; i < indexes.Count && resultingLeavesCount < LeavesCount; i++)
                {
                    node.PixelCount += node.children[indexes[i]].PixelCount;
                    node.RemoveChild(indexes[i]);
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    if (node.children[i] != null)
                    {
                        Reduce(node.children[i], resultingLeavesCount, atLevel);
                    }
                }
            }
        }
    }
}
