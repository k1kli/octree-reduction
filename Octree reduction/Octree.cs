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
            public int color;
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
            public OctNode InitChild(int child)
            {
                if (children[child] != null) throw new Exception("Child already present");
                children[child] = new OctNode(Tree, this.depth + 1);
                ChildrenCount++;
                if (ChildrenCount > 1)
                    Tree.LeavesCount++;
                return children[child];
            }
            public void RemoveChild(int child)
            {
                if (children[child] == null || !children[child].Leaf)
                    throw new Exception("Cant remove this child");
                children[child] = null;
                ChildrenCount--;
                if (ChildrenCount > 0)
                    Tree.LeavesCount--;
            }

            public void GetColorAndPixelCountFromChildren()
            {
                long totalPixelCount=0;
                long red = 0;
                long green = 0;
                long blue = 0;
                for(int i = 0; i < 8; i++)
                {
                    if(children[i] != null)
                    {
                        red += (long)((children[i].color >> 16) & 255)
                            * children[i].PixelCount;
                        green += (long)((children[i].color >> 8) & 255)
                            * children[i].PixelCount;
                        blue += (long)(children[i].color & 255)
                            * children[i].PixelCount;
                        totalPixelCount += children[i].PixelCount;
                    }
                }
                red /= totalPixelCount;
                green /= totalPixelCount;
                blue /= totalPixelCount;
                color = 255;
                color <<= 8;
                color |= ((int)red & 255);
                color <<= 8;
                color |= ((int)green & 255);
                color <<= 8;
                color |= ((int)blue & 255);
                PixelCount = (int)totalPixelCount;
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
                foreach (var color in bitmap.Bits)
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
            while (true)
            {
                int branch = node.Branch(color);
                if (node.children[branch] == null) break;
                node = node.children[branch];
            }
            return node.color;
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
                node.PixelCount++;
                node.color = color;
            }
            else
            {
                int next = node.Branch(color);
                if (node.children[next] == null)
                {
                    levels[node.depth+1].AddLast(node.InitChild(next));
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
                ReduceLevel(resultingLeavesCount, i);
            }
        }
        private void ReduceLevel(int resultingLeavesCount, int atLevel)
        {
            //foreach(var octnode in levels[atLevel-1])
            //{
            //    octnode.SetColorAsAverageOfChildren();
            //}
            Parallel.ForEach(levels[atLevel - 1], (octNode) =>
               {
                   octNode.GetColorAndPixelCountFromChildren();
               });
            levels[atLevel-1].Sort((octNode1, octNode2) => octNode1.PixelCount.CompareTo(octNode2.PixelCount));
            var node = levels[atLevel-1].First;
            while(node != null && resultingLeavesCount < LeavesCount)
            {
                
                for(int i = 0; i < 8;i++)
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
