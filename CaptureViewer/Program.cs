using System;
using System.Windows.Forms;
using Utility;

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
      Logger.LogError("[Unhandled Exception]");
      Exception ex = e.ExceptionObject as Exception;
      if( ex != null ) Logger.LogException( ex );
    }

    private static void Application_ThreadException( object sender, System.Threading.ThreadExceptionEventArgs e )
    {
      Logger.LogError( "[Thread Exception]" );
      Logger.LogException( e.Exception );
    }
  }
}
