namespace CaptureViewer
{
  partial class DebugForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugForm));
      this.textBoxDebug = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // textBoxDebug
      // 
      this.textBoxDebug.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
      this.textBoxDebug.Dock = System.Windows.Forms.DockStyle.Fill;
      this.textBoxDebug.ForeColor = System.Drawing.Color.SpringGreen;
      this.textBoxDebug.Location = new System.Drawing.Point(0, 0);
      this.textBoxDebug.Multiline = true;
      this.textBoxDebug.Name = "textBoxDebug";
      this.textBoxDebug.ReadOnly = true;
      this.textBoxDebug.Size = new System.Drawing.Size(378, 344);
      this.textBoxDebug.TabIndex = 0;
      // 
      // DebugForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoScroll = true;
      this.ClientSize = new System.Drawing.Size(378, 344);
      this.Controls.Add(this.textBoxDebug);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(400, 400);
      this.Name = "DebugForm";
      this.Text = "Debug";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox textBoxDebug;
  }
}