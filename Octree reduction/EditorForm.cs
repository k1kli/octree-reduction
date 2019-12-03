using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
        private DirectBitmap originalSizeBitmapReducedAlong;
        private DirectBitmap forReduceAlongPictureBox;
        public EditorForm()
        {
            InitializeComponent();
            LoadBitmap("..\\..\\Data\\Bitmap.jpg");
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker2.WorkerReportsProgress = true;
            backgroundWorker1.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            backgroundWorker1.ProgressChanged += BackgroundWorker1_ProgressChanged;
            backgroundWorker2.RunWorkerCompleted += BackgroundWorker2_RunWorkerCompleted;
            backgroundWorker2.ProgressChanged += BackgroundWorker2_ProgressChanged;
        }

        private void LoadBitmap(string bitmapSrc)
        {
            Bitmap bitmap = new Bitmap(bitmapSrc);
            if (originalSizeBitmap != null)
            {
                originalSizeBitmap.Dispose();
                originalSizeBitmapReducedAfter.Dispose();
                originalSizeBitmapReducedAlong.Dispose();
            }
            originalSizeBitmap = new DirectBitmap(bitmap);
            originalSizeBitmapReducedAfter = new DirectBitmap(bitmap);
            originalSizeBitmapReducedAlong = new DirectBitmap(bitmap);
            if (forOriginalPictureBox == null)
                forOriginalPictureBox
                = new DirectBitmap(originalPictureBox.Width, originalPictureBox.Height);
            if (forReduceAfterPictureBox == null)
                forReduceAfterPictureBox
                = new DirectBitmap(reduceAfterPictureBox.Width, reduceAfterPictureBox.Height);
            if (forReduceAlongPictureBox == null)
                forReduceAlongPictureBox
                = new DirectBitmap(reduceAlongPictureBox.Width, reduceAlongPictureBox.Height);
            DrawToPictureBoxes();
        }
        public void DrawToPictureBoxes()
        {
            forOriginalPictureBox.DrawOther(originalSizeBitmap);
            forReduceAfterPictureBox.DrawOther(originalSizeBitmapReducedAfter);
            forReduceAlongPictureBox.DrawOther(originalSizeBitmapReducedAlong);
            originalPictureBox.Image = forOriginalPictureBox.Bitmap;
            originalPictureBox.Refresh();
            reduceAfterPictureBox.Image = forReduceAfterPictureBox.Bitmap;
            reduceAfterPictureBox.Refresh();
            reduceAlongPictureBox.Image = forReduceAlongPictureBox.Bitmap;
            reduceAlongPictureBox.Refresh();

        }

        private void colorNumberTrackBar_Scroll(object sender, EventArgs e)
        {
            reduceButton.Text = $"Reduce to {colorNumberTrackBar.Value} colors";
        }

        private void reduceButton_Click(object sender, EventArgs e)
        {
            reduceAfterProgressBar.Value = 0;
            reduceAlongProgressBar.Value = 0;

            backgroundWorker1.RunWorkerAsync(colorNumberTrackBar.Value);
            backgroundWorker2.RunWorkerAsync(colorNumberTrackBar.Value);
        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
                reduceAfterProgressBar.Value = e.ProgressPercentage;
                reduceAfterProgressBar.Refresh();
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DrawToPictureBoxes();
        }

        private void BackgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
                reduceAlongProgressBar.Value = e.ProgressPercentage;
                reduceAlongProgressBar.Refresh();
        }

        private void BackgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DrawToPictureBoxes();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "jpg files (*.jpg)|*.jpg|png files (*.png)|*.png|All files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                LoadBitmap(ofd.FileName);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "jpg files (*.jpg)|*.jpg|png files(*.png) | *.png";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                var format =
                    sfd.FilterIndex == 0 ?
                    System.Drawing.Imaging.ImageFormat.Jpeg :
                    System.Drawing.Imaging.ImageFormat.Png;
                using (var stream = sfd.OpenFile())
                    originalSizeBitmapReducedAfter.Bitmap.Save(stream, format);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ReduceAfter((int)e.Argument);

        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            ReduceAlong((int)e.Argument);
        }
        private void ReduceAfter(int resultingLeavesCount)
        {
            var tree = new Octree();
            var progressReport = new ProgressReporter(backgroundWorker1,
                originalSizeBitmap.Width * originalSizeBitmap.Height);
            tree.LoadBitmap(originalSizeBitmap, progressReport);
            tree.Reduce(resultingLeavesCount);
            originalSizeBitmapReducedAfter.DrawOther(originalSizeBitmap);
            tree.UpdateBitmap(originalSizeBitmapReducedAfter);
            progressReport.StopReporting();
        }
        private void ReduceAlong(int resultingLeavesCount)
        {
            var tree = new Octree();
            var progressReport = new ProgressReporter(backgroundWorker2,
                originalSizeBitmap.Width*originalSizeBitmap.Height);
            tree.LoadBitmapReduceAlong(originalSizeBitmap, progressReport, resultingLeavesCount);
            originalSizeBitmapReducedAlong.DrawOther(originalSizeBitmap);
            tree.UpdateBitmap(originalSizeBitmapReducedAlong);
            progressReport.StopReporting();
        }
    }
}
class ProgressReporter : Progress<int>
{
    int k = 0;
    BackgroundWorker backgroundWorker;
    int size;
    int prevPercentReport = 0;
    public ProgressReporter(BackgroundWorker backgroundWorker, int size)
    {
        this.backgroundWorker = backgroundWorker;
        this.size = size;
    }
    protected override void OnReport(int value)
    {
        base.OnReport(value);
        k++;
        int percent = k * 100 / size;
        if(percent > prevPercentReport + 5)
        {
            backgroundWorker.ReportProgress(percent);
            prevPercentReport = percent;
        }
    }
    public void StopReporting()
    {
        backgroundWorker.ReportProgress(100);
    }
};
