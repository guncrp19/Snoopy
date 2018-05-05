﻿using System;
using System.Reflection;
using System.Windows.Forms;

namespace CaptureViewer
{
  public partial class TextExtractorForm : Form
  {
    private TextExtractorFormController _controller;

    public TextExtractorForm()
    {
      InitializeComponent();
      InitTitle();
      _controller = new TextExtractorFormController( this );
    }

    public void PrintToLog( string message, string fullPath )
    {
      if( InvokeRequired )
      {
        BeginInvoke( (Action)( () => PrintToLog( message, fullPath ) ) );
        return;
      }
      TextBoxLogger.AppendText( message );
      TextBoxLogger.AppendText( "=======================" );
      TextBoxLogger.AppendText( Environment.NewLine );
      TextBoxLogger.AppendText( "Saved to : " );
      TextBoxLogger.AppendText( Environment.NewLine );
      TextBoxLogger.AppendText( fullPath );
      TextBoxLogger.AppendText( Environment.NewLine );
      TextBoxLogger.AppendText( "=======================" );
      TextBoxLogger.AppendText( Environment.NewLine );
    }

    public void PrintToLog( string message )
    {
      if( InvokeRequired )
      {
        BeginInvoke( (Action)( () => PrintToLog( message ) ) );
        return;
      }
      TextBoxLogger.AppendText( message );
      TextBoxLogger.AppendText( Environment.NewLine );
    }

    public void ClearLog()
    {
      if( InvokeRequired )
      {
        BeginInvoke( (Action)( () => ClearLog() ) );
        return;
      }
      TextBoxLogger.Clear(); ;
    }

    private void InitTitle()
    {
      var version = Assembly.GetExecutingAssembly().GetName().Version;
      Text = string.Format( "Text Extractor({0})", version.ToString() );
    }

    private void ClearToolStripMenuItemClick( object sender, EventArgs e )
    {
      ClearLog();
    }

    private void OpenResultFolderToolStripMenuItemClick( object sender, EventArgs e )
    {
      _controller.ShowResultDirectory();
    }

    private void debugToolStripMenuItem_Click( object sender, EventArgs e )
    {
      _controller.ShowDebugDialog();
    }

    private void ExtractorFormClosed( object sender, FormClosedEventArgs e )
    {
      _controller.CleanUp();
    }
  }
}