namespace CaptureViewer
{
  partial class TextExtractorForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing )
    {
      if( disposing && ( components != null ) )
      {
        components.Dispose();
      }
      base.Dispose( disposing );
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextExtractorForm));
      this.TextBoxLogger = new System.Windows.Forms.TextBox();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.openResultFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // TextBoxLogger
      // 
      this.TextBoxLogger.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
      this.TextBoxLogger.Dock = System.Windows.Forms.DockStyle.Fill;
      this.TextBoxLogger.ForeColor = System.Drawing.Color.White;
      this.TextBoxLogger.Location = new System.Drawing.Point(0, 33);
      this.TextBoxLogger.Multiline = true;
      this.TextBoxLogger.Name = "TextBoxLogger";
      this.TextBoxLogger.ReadOnly = true;
      this.TextBoxLogger.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.TextBoxLogger.Size = new System.Drawing.Size(578, 611);
      this.TextBoxLogger.TabIndex = 0;
      // 
      // menuStrip1
      // 
      this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openResultFolderToolStripMenuItem,
            this.clearToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(578, 33);
      this.menuStrip1.TabIndex = 1;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // openResultFolderToolStripMenuItem
      // 
      this.openResultFolderToolStripMenuItem.Name = "openResultFolderToolStripMenuItem";
      this.openResultFolderToolStripMenuItem.Size = new System.Drawing.Size(175, 29);
      this.openResultFolderToolStripMenuItem.Text = "Open Result Folder";
      this.openResultFolderToolStripMenuItem.Click += new System.EventHandler(this.OpenResultFolderToolStripMenuItemClick);
      // 
      // clearToolStripMenuItem
      // 
      this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
      this.clearToolStripMenuItem.Size = new System.Drawing.Size(63, 29);
      this.clearToolStripMenuItem.Text = "Clear";
      this.clearToolStripMenuItem.Click += new System.EventHandler(this.ClearToolStripMenuItemClick);
      // 
      // TextExtractorForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(578, 644);
      this.Controls.Add(this.TextBoxLogger);
      this.Controls.Add(this.menuStrip1);
      this.DoubleBuffered = true;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this.menuStrip1;
      this.MaximizeBox = false;
      this.MinimumSize = new System.Drawing.Size(500, 500);
      this.Name = "TextExtractorForm";
      this.Text = "Text Extractor";
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox TextBoxLogger;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem openResultFolderToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
  }
}

