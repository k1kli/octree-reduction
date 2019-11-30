namespace Octree_reduction
{
    partial class EditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.originalPictureBox = new System.Windows.Forms.PictureBox();
            this.reduceAfterPictureBox = new System.Windows.Forms.PictureBox();
            this.reduceAlongPictureBox = new System.Windows.Forms.PictureBox();
            this.reduceAlongLabel = new System.Windows.Forms.Label();
            this.reduceAfterLabel = new System.Windows.Forms.Label();
            this.reduceAfterProgressBar = new System.Windows.Forms.ProgressBar();
            this.reduceAlongProgressBar = new System.Windows.Forms.ProgressBar();
            this.colorNumberTrackBar = new System.Windows.Forms.TrackBar();
            this.reduceButton = new System.Windows.Forms.Button();
            this.grayscaleButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.originalPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reduceAfterPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reduceAlongPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorNumberTrackBar)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // originalPictureBox
            // 
            this.originalPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.originalPictureBox.Location = new System.Drawing.Point(13, 42);
            this.originalPictureBox.Name = "originalPictureBox";
            this.originalPictureBox.Size = new System.Drawing.Size(680, 417);
            this.originalPictureBox.TabIndex = 0;
            this.originalPictureBox.TabStop = false;
            // 
            // reduceAfterPictureBox
            // 
            this.reduceAfterPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.reduceAfterPictureBox.Location = new System.Drawing.Point(725, 62);
            this.reduceAfterPictureBox.Name = "reduceAfterPictureBox";
            this.reduceAfterPictureBox.Size = new System.Drawing.Size(435, 239);
            this.reduceAfterPictureBox.TabIndex = 1;
            this.reduceAfterPictureBox.TabStop = false;
            // 
            // reduceAlongPictureBox
            // 
            this.reduceAlongPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.reduceAlongPictureBox.Location = new System.Drawing.Point(725, 359);
            this.reduceAlongPictureBox.Name = "reduceAlongPictureBox";
            this.reduceAlongPictureBox.Size = new System.Drawing.Size(435, 239);
            this.reduceAlongPictureBox.TabIndex = 2;
            this.reduceAlongPictureBox.TabStop = false;
            // 
            // reduceAlongLabel
            // 
            this.reduceAlongLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.reduceAlongLabel.AutoSize = true;
            this.reduceAlongLabel.Location = new System.Drawing.Point(838, 339);
            this.reduceAlongLabel.Name = "reduceAlongLabel";
            this.reduceAlongLabel.Size = new System.Drawing.Size(221, 17);
            this.reduceAlongLabel.TabIndex = 3;
            this.reduceAlongLabel.Text = "Reduce along octree construction";
            // 
            // reduceAfterLabel
            // 
            this.reduceAfterLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.reduceAfterLabel.AutoSize = true;
            this.reduceAfterLabel.Location = new System.Drawing.Point(838, 42);
            this.reduceAfterLabel.Name = "reduceAfterLabel";
            this.reduceAfterLabel.Size = new System.Drawing.Size(215, 17);
            this.reduceAfterLabel.TabIndex = 4;
            this.reduceAfterLabel.Text = "Reduce after octree construction";
            // 
            // reduceAfterProgressBar
            // 
            this.reduceAfterProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.reduceAfterProgressBar.Location = new System.Drawing.Point(725, 308);
            this.reduceAfterProgressBar.Name = "reduceAfterProgressBar";
            this.reduceAfterProgressBar.Size = new System.Drawing.Size(435, 23);
            this.reduceAfterProgressBar.Step = 1;
            this.reduceAfterProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.reduceAfterProgressBar.TabIndex = 5;
            // 
            // reduceAlongProgressBar
            // 
            this.reduceAlongProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.reduceAlongProgressBar.Location = new System.Drawing.Point(725, 604);
            this.reduceAlongProgressBar.Name = "reduceAlongProgressBar";
            this.reduceAlongProgressBar.Size = new System.Drawing.Size(435, 23);
            this.reduceAlongProgressBar.TabIndex = 6;
            // 
            // colorNumberTrackBar
            // 
            this.colorNumberTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.colorNumberTrackBar.Location = new System.Drawing.Point(13, 466);
            this.colorNumberTrackBar.Maximum = 100;
            this.colorNumberTrackBar.Minimum = 1;
            this.colorNumberTrackBar.Name = "colorNumberTrackBar";
            this.colorNumberTrackBar.Size = new System.Drawing.Size(680, 56);
            this.colorNumberTrackBar.TabIndex = 7;
            this.colorNumberTrackBar.Value = 16;
            this.colorNumberTrackBar.Scroll += new System.EventHandler(this.colorNumberTrackBar_Scroll);
            // 
            // reduceButton
            // 
            this.reduceButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.reduceButton.Location = new System.Drawing.Point(239, 528);
            this.reduceButton.Name = "reduceButton";
            this.reduceButton.Size = new System.Drawing.Size(227, 41);
            this.reduceButton.TabIndex = 8;
            this.reduceButton.Text = "Reduce to 16 Colors";
            this.reduceButton.UseVisualStyleBackColor = true;
            this.reduceButton.Click += new System.EventHandler(this.reduceButton_Click);
            // 
            // grayscaleButton
            // 
            this.grayscaleButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.grayscaleButton.Location = new System.Drawing.Point(286, 575);
            this.grayscaleButton.Name = "grayscaleButton";
            this.grayscaleButton.Size = new System.Drawing.Size(132, 41);
            this.grayscaleButton.TabIndex = 9;
            this.grayscaleButton.Text = "To grayscale";
            this.grayscaleButton.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1172, 28);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(150, 26);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(150, 26);
            this.saveAsToolStripMenuItem.Text = "Save as...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1172, 636);
            this.Controls.Add(this.grayscaleButton);
            this.Controls.Add(this.reduceButton);
            this.Controls.Add(this.colorNumberTrackBar);
            this.Controls.Add(this.reduceAlongProgressBar);
            this.Controls.Add(this.reduceAfterProgressBar);
            this.Controls.Add(this.reduceAfterLabel);
            this.Controls.Add(this.reduceAlongLabel);
            this.Controls.Add(this.reduceAlongPictureBox);
            this.Controls.Add(this.reduceAfterPictureBox);
            this.Controls.Add(this.originalPictureBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1190, 683);
            this.MinimumSize = new System.Drawing.Size(1190, 683);
            this.Name = "EditorForm";
            this.Text = "EditorForm";
            ((System.ComponentModel.ISupportInitialize)(this.originalPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reduceAfterPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reduceAlongPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorNumberTrackBar)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox originalPictureBox;
        private System.Windows.Forms.PictureBox reduceAfterPictureBox;
        private System.Windows.Forms.PictureBox reduceAlongPictureBox;
        private System.Windows.Forms.Label reduceAlongLabel;
        private System.Windows.Forms.Label reduceAfterLabel;
        private System.Windows.Forms.ProgressBar reduceAfterProgressBar;
        private System.Windows.Forms.ProgressBar reduceAlongProgressBar;
        private System.Windows.Forms.TrackBar colorNumberTrackBar;
        private System.Windows.Forms.Button reduceButton;
        private System.Windows.Forms.Button grayscaleButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}