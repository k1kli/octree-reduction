using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Octree_reduction
{
    public partial class EditorForm : Form
    {
        private DirectBitmap originalSizeBitmap;
        private DirectBitmap forOriginalPictureBox;
        private DirectBitmap originalSizeBitmapReducedAfter;
        private DirectBitmap forReduceAfterPictureBox;
        public EditorForm()
        {
            InitializeComponent();
            LoadBitmap("..\\..\\Data\\Bitmap.jpg");
        }

        private void LoadBitmap(string bitmapSrc)
        {
            Bitmap bitmap = new Bitmap(bitmapSrc);
            if (originalSizeBitmap != null)
            {
                originalSizeBitmap.Dispose();
            originalSizeBitmapReducedAfter.Dispose();
            }
            originalSizeBitmap = new DirectBitmap(bitmap);
            originalSizeBitmapReducedAfter = new DirectBitmap(bitmap);
            if (forOriginalPictureBox == null)
                forOriginalPictureBox
                = new DirectBitmap(originalPictureBox.Width, originalPictureBox.Height);
            if (forReduceAfterPictureBox == null)
                forReduceAfterPictureBox
                = new DirectBitmap(reduceAfterPictureBox.Width, reduceAfterPictureBox.Height);
            DrawToPictureBoxes();
        }
        public void DrawToPictureBoxes()
        {
            forOriginalPictureBox.DrawOther(originalSizeBitmap);
            forReduceAfterPictureBox.DrawOther(originalSizeBitmapReducedAfter);
            originalPictureBox.Image = forOriginalPictureBox.Bitmap;
            originalPictureBox.Refresh();
            reduceAfterPictureBox.Image = forReduceAfterPictureBox.Bitmap;
            reduceAfterPictureBox.Refresh();

        }

        private void colorNumberTrackBar_Scroll(object sender, EventArgs e)
        {
            reduceButton.Text = $"Reduce to {1 << colorNumberTrackBar.Value} colors";
        }

        private void reduceButton_Click(object sender, EventArgs e)
        {
            var beforeReduction = new Octree();
            beforeReduction.LoadBitmap(originalSizeBitmap);
            var reduced = beforeReduction.GetCopy();
            reduced.Reduce(1 << colorNumberTrackBar.Value);
            originalSizeBitmapReducedAfter.DrawOther(originalSizeBitmap);
            reduced.UpdateBitmap(originalSizeBitmapReducedAfter);
            DrawToPictureBoxes();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "jpg files (*.jpg)|*.jpg|png files (*.png)|*.png|All files (*.*)|*.*";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                LoadBitmap(ofd.FileName);
            }
        }
    }
}
