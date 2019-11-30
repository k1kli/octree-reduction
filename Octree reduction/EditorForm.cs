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
            reduceAfterProgressBar.Maximum = originalSizeBitmap.Bits.Length;
            reduceAlongProgressBar.Value = 0;
            reduceAlongProgressBar.Maximum = originalSizeBitmap.Bits.Length;
            ReduceAfter(colorNumberTrackBar.Value);
            //backgroundWorker1.RunWorkerAsync(colorNumberTrackBar.Value);
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
            //ReduceAlong((int)e.Argument);

        }
        private void ReduceAfter(int resultingLeavesCount)
        {
            var tree = new Octree();
            var progressReport = new ProgressReporter(reduceAfterProgressBar);
            progressReport.StartReporting();
            tree.LoadBitmap(originalSizeBitmap, progressReport);
            tree.Reduce(resultingLeavesCount);
            originalSizeBitmapReducedAfter.DrawOther(originalSizeBitmap);
            tree.UpdateBitmap(originalSizeBitmapReducedAfter);
            progressReport.StopReporting();
            this.Invoke((Action)delegate { DrawToPictureBoxes(); });
        }
        private void ReduceAlong(int resultingLeavesCount)
        {
            var tree = new Octree();
            var progressReport = new ProgressReporter(reduceAlongProgressBar);
            progressReport.StartReporting();
            tree.LoadBitmapReduceAlong(originalSizeBitmap, progressReport, resultingLeavesCount);
            originalSizeBitmapReducedAlong.DrawOther(originalSizeBitmap);
            tree.UpdateBitmap(originalSizeBitmapReducedAlong);
            progressReport.StopReporting();
            this.Invoke((Action)delegate { DrawToPictureBoxes(); });
        }

    }
}
class ProgressReporter : Progress<int>
{
    CancellationTokenSource tokenSource = new CancellationTokenSource();
    int k = 0;
    ProgressBar progressBar;
    Task reportingTask;
    public ProgressReporter(ProgressBar progressBar)
    {
        this.progressBar = progressBar;
    }
    protected override void OnReport(int value)
    {
        base.OnReport(value);
        Interlocked.Increment(ref k);
    }
    public void StartReporting()
    {
        CancellationToken ct = tokenSource.Token;
        reportingTask = new Task(UpdateBar, tokenSource.Token);
        reportingTask.Start();
    }
    public void StopReporting()
    {
        tokenSource.Cancel();
    }
    private void UpdateBar()
    {
        while (!tokenSource.Token.IsCancellationRequested)
        {
            progressBar.Invoke((Action)delegate
            {
                progressBar.Value = k;
            });
            if (tokenSource.Token.IsCancellationRequested) break;
            Thread.Sleep(50);
        }
    }
};
