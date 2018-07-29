﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Utility
{
  public static class Logger
  {
    private readonly static object _syncRoot = new object();
    private static readonly ILog Log =
              LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

    public static void LogError(string message)
    {
      lock( _syncRoot )
      { 
        Log.Error(message);
      }
    }

    public static void LogException( Exception ex )
    {
      lock( _syncRoot )
      {
        Log.Error(ex.Message, ex);
      }
    }

    public static void LogInfo(string message)
    {
      lock( _syncRoot )
      {
        Log.Info(message);
      }
    }

    public static void LogDebug( string message )
    {
      lock( _syncRoot )
      {
        Log.Debug( message );
      }
    }
  }
}
