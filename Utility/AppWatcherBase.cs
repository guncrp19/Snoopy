using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Utility
{
  public abstract class AppWatcherBase
  {
    private Thread watcher;
    private const int PollingInterval = 1000;
    private const int ReAskingInterval = 1000;
    private const int RetryNum = 5;
    protected const string spFp = @"D:\wdFile";
    protected string WatcherPath;
    private bool _abortThread = false;
    private string _appName;

    public AppWatcherBase(string appName)
    {
      _appName = appName;
      watcher = new Thread( Watch );
      watcher.Start();
      WatcherPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
    }

    public void StopWatcher()
    {
      _abortThread = true;
    }

    protected abstract bool RuleCheck();
    protected abstract void ActionIfRuleCheck();

    private bool MonitoredAppExists( out Process outProc )
    {
      outProc = null;
      try
      {
        Process[] processList = Process.GetProcessesByName( _appName );
        foreach( var proc in processList )
        {
          if( Equals( Path.GetDirectoryName(proc.StartInfo.FileName), WatcherPath))
          {
            outProc = proc;
            return true;
          }
        }

        return false;
      }
      catch( Exception )
      {
        return true;
      }
    }

    private void Watch()
    {
      while( true )
      {
        try
        {
          if( _abortThread )
            break;
          if( RuleCheck() )
          {
            ActionIfRuleCheck();
          }
          else
          {
            //make sure RetryNum times
            bool abort = false;
            for( int i = 0; i < RetryNum; i++ )
            {
              Thread.Sleep( ReAskingInterval );
              if( RuleCheck() )
              {
                ActionIfRuleCheck();
                abort = true;
                break;
              }
            }
            if( abort )
              continue;

            //restartApps
            Process proc;
            if( MonitoredAppExists(out proc ) )
            {
              proc.Kill();
            }
            Process.Start( _appName );
          }
          Thread.Sleep( PollingInterval );
        }
        catch(Exception)
        {
          //ignore any exceptions
        }
      }
    }
  }
}
