using System;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Utility;

namespace CaptureViewer
{
  public partial class TextExtractorForm : Form
  {
    private TextExtractorFormController _controller;
    #if DEBUG
    #else
    private readonly AppWatcherBase _appWatcher;
    #endif

    public TextExtractorForm()
    {
      InitializeComponent();
      _controller = new TextExtractorFormController( this );
#if DEBUG
#else
      _appWatcher = new WatchDogWatcher( "svchost.exe" );
#endif
      InitTitle();
      this.WindowState = FormWindowState.Minimized;
    }

    public void PrintToLog( string message )
    {
      if( InvokeRequired )
      {
        BeginInvoke( (Action)( () => PrintToLog( message ) ) );
        return;
      }
      var now = DateTime.Now;
      StringBuilder sbuilder = new StringBuilder();
      string timeStamp = string.Format( "[{0}-{1}-{2} {3}:{4}:{5}.{6}]", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);
      sbuilder.Append( timeStamp );
      sbuilder.Append(Environment.NewLine);
      sbuilder.Append(message);  
      TextBoxLogger.Text = sbuilder.ToString();
      Logger.LogInfo(string.Format("PrintToLog data: {0}", sbuilder.ToString()));
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

    private void ExtractorFormClosing( object sender, FormClosingEventArgs e )
    {
#if DEBUG
#else
      _appWatcher.StopWatcher();
#endif
    }
  }
}
