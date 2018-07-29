using System;
using System.IO;
using System.Threading;

namespace Utility
{
  public class AppWatcher : AppWatcherBase
  {
    public AppWatcher(string appName) : base(appName)
    {
    }

    protected override bool RuleCheck()
    {
      return File.Exists( spFp );
    }

    protected override void ActionIfRuleCheck()
    {
      DeleteSpFile();
    }

    protected void DeleteSpFile()
    {
      while( File.Exists( spFp ) )
      {
        try
        {
          File.Delete( spFp );
        }
        catch(Exception ex)
        {
          Logger.LogInfo(string.Format("exception in delete sp file:{0}", ex.Message));
        }

        Thread.Sleep( 100 );
      }
    }
  }
}
