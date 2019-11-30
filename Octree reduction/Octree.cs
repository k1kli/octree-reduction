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
            public int ChildrenPixelCount;
            public int depth;
            public int color;
            public LinkedListNode<OctNode>[] children;
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
                children = new LinkedListNode<OctNode>[8];
            }
            public void InitChild(int child)
            {
                if (children[child] != null) throw new Exception("Child already present");
                children[child] = Tree.levels[depth+1].AddLast(new OctNode(Tree, this.depth + 1));
                ChildrenCount++;
                if (ChildrenCount > 1)
                    Tree.LeavesCount++;
            }
            public void RemoveChild(int child)
            {
                if (children[child] == null || !children[child].Value.Leaf)
                    throw new Exception("Cant remove this child");
                children[child].List.Remove(children[child]);
                children[child] = null;
                ChildrenCount--;
                if (ChildrenCount > 0)
                    Tree.LeavesCount--;
            }

            public void UpdateColorAndPixelCountFromChildren()
            {
                long red = ((color >> 16) & 255)*PixelCount;
                long green = ((color >> 8) & 255) * PixelCount;
                long blue = (color & 255) * PixelCount;
                for(int i = 0; i < 8; i++)
                {
                    if(children[i] != null)
                    {
                        red += (long)((children[i].Value.color >> 16) & 255)
                            * children[i].Value.PixelCount;
                        green += (long)((children[i].Value.color >> 8) & 255)
                            * children[i].Value.PixelCount;
                        blue += (long)(children[i].Value.color & 255)
                            * children[i].Value.PixelCount;
                    }
                }
                PixelCount += ChildrenPixelCount;
                ChildrenPixelCount = 0;
                red /= PixelCount;
                green /= PixelCount;
                blue /= PixelCount;
                color = 255;
                color <<= 8;
                color |= ((int)red & 255);
                color <<= 8;
                color |= ((int)green & 255);
                color <<= 8;
                color |= ((int)blue & 255);
            }
        }
        LinkedList<OctNode>[] levels;
        public OctNode root;
        public int LeavesCount { get; private set; } = 1;
        public Octree()
        {
            levels = new LinkedList<OctNode>[9];
            for(int i = 0; i < 9; i++)
            {
                levels[i] = new LinkedList<OctNode>();
            }
            root = new OctNode(this, 0);
            levels[0].AddLast(root);
        }
        public void LoadBitmap(DirectBitmap bitmap, IProgress<int> progressReport)
        {
                foreach (int color in bitmap.Bits)
                {
                    InsertColor(color);
                    progressReport.Report(1);
                }
        }
        public void LoadBitmapReduceAlong(DirectBitmap bitmap,
                                          IProgress<int> progressReport,
                                          int resultingLeavesCount)
        {
            foreach (int color in bitmap.Bits)
            {
                InsertColor(color);
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
            while (true)
            {
                int branch = node.Branch(color);
                if (node.children[branch] == null) break;
                node = node.children[branch].Value;
            }
            return node.color;
        }

        public void InsertColor(int color)
        {
            InsertColor(root, color);
        }
        public void InsertColor(OctNode node, int color)
        {
            if (node.LowestLevel)
            {
                node.color = color;
                node.PixelCount++;
            }
            else
            {
                int next = node.Branch(color);
                if (node.children[next] == null)
                {
                    node.InitChild(next);
                }
                node.ChildrenPixelCount++;
                InsertColor(node.children[next].Value, color);
            }
        }
        public void Reduce(int resultingLeavesCount)
        {
            for (int i = 8; i > 0 && resultingLeavesCount < LeavesCount; i--)
            {
                ReduceLevel(resultingLeavesCount, i);
            }
        }
        private void ReduceLevel(int resultingLeavesCount, int atLevel)
        {
            //Parallel.ForEach(levels[atLevel - 1], (octNode) =>
            //   {
            //       octNode.GetColorAndPixelCountFromChildren();
            //   });
            if(levels[atLevel-1].Count > 1)
                levels[atLevel-1].Sort((octNode1, octNode2) => (octNode1.PixelCount+octNode1.ChildrenPixelCount)
                .CompareTo(octNode2.PixelCount + octNode2.ChildrenPixelCount));
            var node = levels[atLevel-1].First;
            while(node != null && resultingLeavesCount < LeavesCount)
            {

                node.Value.UpdateColorAndPixelCountFromChildren();
                for (int i = 0; i < 8;i++)
                {
                    if(node.Value.children[i] != null)
                    {
                        node.Value.RemoveChild(i);
                    }
                }
                node = node.Next;
            }
        }
    }
}
