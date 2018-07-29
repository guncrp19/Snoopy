using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Utility;

namespace ServerCommunication
{
  public class PostWorker
  {
    private Thread _postWork;
    private PostWorkerSettings _settings;
    private const int WorkingInterval = 5000;
    private bool _abort = false;

    public PostWorker( PostWorkerSettings settings)
    {
      _settings = settings;
      _postWork = new Thread(DoWork);
      _postWork.Start();
    }

    public void AbortWorker()
    {
      _abort = true;
    }

    private void DeleteFile( string fullPath )
    {
      int retry = 0;
      while( File.Exists( fullPath ) && retry < 100 )
      {
        try
        {
          File.Delete( fullPath );
        }
        catch(Exception ex)
        {
          Logger.LogInfo(string.Format("Exception in PostWorker deletefile. ex={0}", ex.Message));
          Thread.Sleep( 100 );
        }
        retry++;
      }
    }


    private bool PostOldestFile()
    {
      bool success = false;
      try
      {
        var fileInfo = new DirectoryInfo( _settings.WorkingDirectory ).GetFileSystemInfos().OrderByDescending( fi => fi.CreationTime ).First();
        SendDataToServer(File.ReadAllText(fileInfo.FullName));
        success = true;

        DeleteFile(fileInfo.FullName);
      }
      catch(Exception ex)
      {
        Logger.LogException(ex);
        success = false;
      }

      return success;
    }

    private void SendDataToServer( string data )
    {
      var payload = new PostReqPayload()
      {
        PostingTime = DateTime.Now.ToString(),
        UserName = _settings.UserName,
        Content = data,
      };

      _settings.PostReq.SendPostCommand( payload );
    }

    private void DoWork()
    {
      if( !Directory.Exists( _settings.WorkingDirectory ) )
        Directory.CreateDirectory( _settings.WorkingDirectory );

      while( !_abort )
      {
        try
        {
          var dirInfo = new DirectoryInfo(_settings.WorkingDirectory);
          while(dirInfo.GetFiles().Length > 0)
          {
            if(!PostOldestFile())
              break;
          }
        }
        catch(Exception)
        {

        }
        Thread.Sleep(WorkingInterval);
      }
    }
  }
}
