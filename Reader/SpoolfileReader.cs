using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Reader.Utility;

namespace EMFSpoolfileReader
{
	public class SpoolfileReader : ISpoolfileReaderBase
	{
		public static System.Diagnostics.TraceSwitch ApplicationTracing = new System.Diagnostics.TraceSwitch("EMFSpoolfileReader", "EMF Spool File reader application tracing");

		private const string PERFORMANCE_COUNTER_NAME = "EMF Spoolfile Pages";
		private const string PERFORMANCE_TIMER_NAME = "EMF Spoolfile Pages/sec";
		private const string PERFORMANCE_COUNTER_CATEGORY = "Spoolfile Readers";

		private static PerformanceCounter SpoolfileReaderPerformaceCounter;
		private void InitialiseCounterCategory()
		{
			//\\ Create the performace category if it is not already done
			if (PerformanceCounterCategory.Exists(PERFORMANCE_COUNTER_CATEGORY)) {
				PerformanceCounterCategory.Delete(PERFORMANCE_COUNTER_CATEGORY);
			}

			CounterCreationDataCollection SpoolerCounterCreationDataCollection = new CounterCreationDataCollection();
			CounterCreationDataCollection SpoolfileReaderCounterCreationDataCollection = new CounterCreationDataCollection();

			CounterCreationData SpoolfileReaderCounterCreationData = new CounterCreationData(PERFORMANCE_COUNTER_NAME, "Spoolfile pages read", PerformanceCounterType.NumberOfItems32);
			CounterCreationData SpoolfileReaderTimerCreationData = new CounterCreationData(PERFORMANCE_TIMER_NAME, "Spoolfile pages read second", PerformanceCounterType.NumberOfItems32);

			SpoolfileReaderCounterCreationDataCollection.Add(SpoolfileReaderCounterCreationData);
			SpoolfileReaderCounterCreationDataCollection.Add(SpoolfileReaderTimerCreationData);

			try
      {
				PerformanceCounterCategory.Create(PERFORMANCE_COUNTER_CATEGORY, "", System.Diagnostics.PerformanceCounterCategoryType.MultiInstance, SpoolerCounterCreationDataCollection);
			}
      catch (System.Security.SecurityException ex)
      {
				System.Diagnostics.Trace.TraceError(ex.ToString());
			}

		}

		private void InitialiseCounter()
		{
			//SpoolfileReaderPerformaceCounter = New PerformanceCounter(PERFORMANCE_COUNTER_CATEGORY, PERFORMANCE_COUNTER_NAME, True)
		}

			//\\ The number of copies per page
		private int _Copies = 1;
		private int _Pages = 0;

		private enum SpoolerRecordTypes
		{
			SRT_EOF = 0x0,
			// // int32 zero
			SRT_RESERVED_1 = 0x1,
			//*  1                                */
			SRT_FONTDATA = 0x2,
			//  2 Font Data                      */
			SRT_DEVMODE = 0x3,
			//  3 DevMode                        */
			SRT_FONT2 = 0x4,
			//4 Font Data                      */
			SRT_RESERVED_5 = 0x5,
			// 5                                */
			SRT_FONT_MM = 0x6,
			// 6 Font Data (Multiple Master)    */
			SRT_FONT_SUB1 = 0x7,
			//   7 Font Data (SubsetFont 1)       */
			SRT_FONT_SUB2 = 0x8,
			//   8 Font Data (SubsetFont 2)      
			SRT_RESERVED_9 = 0x9,
			SRT_UNKNOWN = 0x10,
			// // int unknown...
			SRT_RESERVED_A = 0xa,
			SRT_RESERVED_B = 0xb,
			SRT_PAGE = 0xc,
			// 12  Enhanced Meta File (EMF)       */
			SRT_EOPAGE1 = 0xd,
			// 13  EndOfPage                      */
			SRT_EOPAGE2 = 0xe,
			// 14  EndOfPage                      */
			SRT_EXT_FONT = 0xf,
			// 15  Ext Font Data                  */
			SRT_EXT_FONT2 = 0x10,
			// 16  Ext Font Data                  */
			SRT_EXT_FONT_MM = 0x11,
			// 17  Ext Font Data (Multiple Master)
			SRT_EXT_FONT_SUB1 = 0x12,
			// 18  Ext Font Data (SubsetFont 1)   */
			SRT_EXT_FONT_SUB2 = 0x13,
			//* 19  Ext Font Data (SubsetFont 2)   */
			SRT_EXT_PAGE = 0x14,
			// 20  Enhanced Meta File? 
			SRT_JOB_INFO = 0x10000
			// // int length, wchar jobDescription
		}

		private struct EMFMetaRecordHeader
		{
			public long Seek;
			public SpoolerRecordTypes iType;
			public Int32 nSize;
		}

    public string ExtractText( string spoolFileName )
    {
      string retVal = "";
      spoolFileName = Path.ChangeExtension( spoolFileName, ".SPL" );

      //\\ Open a binary reader for the spool file
      var SpoolFileStream = new FileStream( spoolFileName, FileMode.Open, FileAccess.Read );
      var SpoolBinaryReader = new BinaryReader( SpoolFileStream, System.Text.Encoding.UTF8 );

      try
      {
        //Read the spooler records and count the total pages
        var recNext = NextHeader( ref SpoolBinaryReader );
        DebugLogger.Instance.Log( string.Format( "header. type={0}, size={1}", recNext.iType, recNext.nSize ) );
        var txtList = new List<string>(); 
        while( recNext.iType != SpoolerRecordTypes.SRT_EOF )
        {
          txtList.Add(ProcessHeader( recNext, ref SpoolBinaryReader ));
          recNext = NextHeader( ref SpoolBinaryReader );
          DebugLogger.Instance.Log( string.Format( "header. type={0}, size={1}", recNext.iType, recNext.nSize ) );
        }

        var builder = new StringBuilder();
        foreach( var str in txtList )
          builder.Append( str );
        retVal = builder.ToString();
      }
      catch(Exception ex)
      {
        Console.WriteLine( "exception occurred when extracting text. Exception={0}", ex.Message );
      }
      finally
      {
        SpoolBinaryReader.Close();
        SpoolFileStream.Close();
      }
      return retVal;
    }

		public int GetTruePageCount(string spoolFilename)
		{

			if (ApplicationTracing.TraceVerbose)
      {
				Trace.WriteLine("GetTruePageCount for " + spoolFilename, this.GetType().ToString());
			}


			//\\ The number of copies is held in the shadow file
			string ShadowFilename = null;
			if (!(Path.GetExtension(spoolFilename).ToUpper() == ".SHD"))
      {
				ShadowFilename = Path.ChangeExtension(spoolFilename, ".SHD");
			}
      else
      {
				ShadowFilename = spoolFilename;
			}
			System.IO.FileInfo fiShadow = new System.IO.FileInfo(ShadowFilename);
			if (fiShadow.Exists)
      {
				System.IO.FileStream ShadowFileStream = new System.IO.FileStream(ShadowFilename, FileMode.Open, FileAccess.Read);
				BinaryReader ShadowBinaryReader = new BinaryReader(ShadowFileStream);

				//\\ Close the shadow file 
				ShadowBinaryReader.Close();
				ShadowFileStream.Close();
			}

			spoolFilename = Path.ChangeExtension(spoolFilename, ".SPL");

			//\\ Open a binary reader for the spool file
			System.IO.FileStream SpoolFileStream = new System.IO.FileStream(spoolFilename, FileMode.Open, FileAccess.Read);
			BinaryReader SpoolBinaryReader = new BinaryReader(SpoolFileStream, System.Text.Encoding.UTF8);

			//Read the spooler records and count the total pages
			EMFMetaRecordHeader recNext = NextHeader(ref SpoolBinaryReader);
			while (recNext.iType != SpoolerRecordTypes.SRT_EOF)
      {
				if (recNext.iType == SpoolerRecordTypes.SRT_PAGE)
        {
					_Pages += 1;
				}
				//SpoolfileReaderPerformaceCounter.Increment()
				ProcessHeader(recNext, ref SpoolBinaryReader);
				recNext = NextHeader(ref SpoolBinaryReader);
			}

			//\\ Close the spool file
			SpoolBinaryReader.Close();
			SpoolFileStream.Close();

			return _Pages * _Copies;
		}

		private EMFMetaRecordHeader NextHeader(ref BinaryReader SpoolBinaryReader)
		{
			EMFMetaRecordHeader functionReturnValue = default(EMFMetaRecordHeader);

			EMFMetaRecordHeader recRet = new EMFMetaRecordHeader();

      //\\ get the record type
      recRet.Seek = SpoolBinaryReader.BaseStream.Position;
			try
      {
        recRet.iType = (SpoolfileReader.SpoolerRecordTypes)SpoolBinaryReader.ReadInt32();
			}
      catch (EndOfStreamException e)
      {
        recRet.iType = SpoolerRecordTypes.SRT_EOF;
				return functionReturnValue;
			}
      //\\ Get the record size
      recRet.nSize = SpoolBinaryReader.ReadInt32();
			return recRet;
		}

		private string ProcessHeader(EMFMetaRecordHeader Header, ref BinaryReader SpoolBinaryReader)
		{
      string retVal = "";
			var header = Header;
			if (header.nSize <= 0)
      {
				header.nSize = 8;
			}
			if (header.iType == SpoolerRecordTypes.SRT_JOB_INFO)
      {
				char[] JobInfo = null;
				JobInfo = SpoolBinaryReader.ReadChars(header.nSize);
        //Console.WriteLine("[JOB INFO] {0}", new string(JobInfo));
				SpoolBinaryReader.BaseStream.Seek(header.Seek + header.nSize, SeekOrigin.Begin);
			}
      else if (header.iType == SpoolerRecordTypes.SRT_EOF)
      {
				//\\ End of file reached..do nothing
			}
      else if (header.iType == SpoolerRecordTypes.SRT_DEVMODE)
      {
				//\\ Spool job DEVMODE
				DevMode _dmThis = new DevMode(SpoolBinaryReader);
				_Copies = _dmThis.Copies;
				SpoolBinaryReader.BaseStream.Seek(header.Seek + 8 + header.nSize, SeekOrigin.Begin);
			}
      else if (header.iType == SpoolerRecordTypes.SRT_PAGE | header.iType == SpoolerRecordTypes.SRT_EXT_PAGE)
      {
				//\\ 
				retVal = ProcessEMFRecords(Header, SpoolBinaryReader);
			}
      else if (header.iType == SpoolerRecordTypes.SRT_EOPAGE1 | header.iType == SpoolerRecordTypes.SRT_EOPAGE2)
      {
				//\\ int plus long
				byte[] EOPAGE = null;
				EOPAGE = SpoolBinaryReader.ReadBytes(header.nSize);
				if (header.nSize == 0x8) {
					SpoolBinaryReader.BaseStream.Seek(header.Seek + header.nSize + 8, SeekOrigin.Begin);
				}
			}
      else if (header.iType == SpoolerRecordTypes.SRT_UNKNOWN)
      {
				SpoolBinaryReader.BaseStream.Seek(header.Seek + 4, SeekOrigin.Begin);
			}
      else
      {
				SpoolBinaryReader.BaseStream.Seek(header.Seek + header.nSize, SeekOrigin.Begin);
			}
      return retVal;
    }

		private string ProcessEMFRecords(EMFMetaRecordHeader EMFRecord, BinaryReader SpoolBinaryReader)
		{
      string retVal = "";
			long nNextRecordStart = 0;

			nNextRecordStart = EMFRecord.Seek + 8;
			SpoolBinaryReader.BaseStream.Seek(nNextRecordStart, SeekOrigin.Begin);

			//\\ EMRMETAHEADER followed by other EMR records 
			var thisPage = new Page(SpoolBinaryReader);
      retVal = thisPage.GetText();
      nNextRecordStart = nNextRecordStart + thisPage.Header.FileSize;
			SpoolBinaryReader.BaseStream.Seek(nNextRecordStart, SeekOrigin.Begin);
      return retVal;

    }

		public SpoolfileReader()
		{
			//InitialiseCounterCategory();
			//InitialiseCounter();
		}
	}
}

