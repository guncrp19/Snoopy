using System.Windows.Forms;
using Utility;

namespace svchost
{
  public partial class svchostForm : Form
  {
    private readonly AppWatcherBase _appWatcher;

    public svchostForm()
    {
      InitializeComponent();
      //this.WindowState = FormWindowState.Minimized;
      //this.ShowInTaskbar = false;
      _appWatcher = new AppWatcher( "CaptureViewer.exe" );
    }

    private void svchostFormClosing( object sender, FormClosingEventArgs e )
    {
      _appWatcher.StopWatcher();
    }
  }
}
