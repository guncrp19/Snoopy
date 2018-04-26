using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CaptureViewer
{
  public partial class DebugForm : Form
  {
    public DebugForm()
    {
      InitializeComponent();
    }

    public void PrintToDebug( string message )
    {
      if( InvokeRequired )
      {
        BeginInvoke( (Action)( () => PrintToDebug(message) ) );
        return;
      }
      textBoxDebug.AppendText( message );
    }

    public void ClearLog()
    {
      if( InvokeRequired )
      {
        BeginInvoke( (Action)( () => ClearLog() ) );
        return;
      }
      textBoxDebug.Clear(); ;
    }
  }
}
