using System;
using System.Windows.Forms;

namespace CaptureViewer
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      // Add the event handler for handling UI thread exceptions to the event.
      Application.ThreadException += Application_ThreadException;

      // Set the unhandled exception mode to force all Windows Forms errors
      // to go through our handler.
      Application.SetUnhandledExceptionMode( UnhandledExceptionMode.CatchException );

      // Add the event handler for handling non-UI thread exceptions to the event. 
      AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault( false );
      Application.Run( new TextExtractorForm() );
    }

    private static void CurrentDomain_UnhandledException( object sender, UnhandledExceptionEventArgs e )
    {
      //todo : put to log
    }

    private static void Application_ThreadException( object sender, System.Threading.ThreadExceptionEventArgs e )
    {
      //todo : put to log
    }
  }
}
