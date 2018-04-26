using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reader.Utility
{
  public delegate void LogHandler( string message );

  public class DebugLogger
  {
    public static DebugLogger Instance = new DebugLogger();

    public event LogHandler LogEvent;

    private DebugLogger()
    {
    }

    private void InvokeLogEvent( string message )
    {
      if( LogEvent != null )
        LogEvent( message );
    }

    public void Log(string message)
    {
      InvokeLogEvent( message );
    }
  }
}
