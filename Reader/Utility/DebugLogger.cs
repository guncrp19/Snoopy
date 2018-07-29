using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

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
      Logger.LogInfo(string.Format("DebugLogger Log: {0}", message));
      InvokeLogEvent( message );
    }
  }
}
