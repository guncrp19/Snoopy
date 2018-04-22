using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.IO;
namespace EMFSpoolfileReader
{

	public class EMFSpoolfileReader : ISpoolfileReaderBase
	{

		#region "Application trace switch"
			#endregion
		public static System.Diagnostics.TraceSwitch ApplicationTracing = new System.Diagnostics.TraceSwitch("EMFSpoolfileReader", "EMF Spool File reader application tracing");

		#region "Perfomance monitoring information"

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

			try {
				PerformanceCounterCategory.Create(PERFORMANCE_COUNTER_CATEGORY, "", System.Diagnostics.PerformanceCounterCategoryType.MultiInstance, SpoolerCounterCreationDataCollection);
			} catch (System.Security.SecurityException ex) {
				System.Diagnostics.Trace.TraceError(ex.ToString());
			}

		}

		private void InitialiseCounter()
		{
			//SpoolfileReaderPerformaceCounter = New PerformanceCounter(PERFORMANCE_COUNTER_CATEGORY, PERFORMANCE_COUNTER_NAME, True)
		}
		#endregion

		#region "Private member variables"
			//\\ The number of copies per page
		private int _Copies = 1;
		private int _Pages = 0;
			#endregion
		private EMFPages _EMFPages = new EMFPages();

		#region "Private enumerated types"
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
		#endregion

		#region "Private type defs"
		private struct EMFMetaRecordHeader
		{
			public long Seek;
			public SpoolerRecordTypes iType;
			public Int32 nSize;
		}
		#endregion

		#region "ISpoolerfileReaderBase implementation"
		public int GetTruePageCount(string SpoolFilename)
		{

			if (ApplicationTracing.TraceVerbose)
      {
				Trace.WriteLine("GetTruePageCount for " + SpoolFilename, this.GetType().ToString());
			}


			//\\ The number of copies is held in the shadow file
			string ShadowFilename = null;
			if (!(Path.GetExtension(SpoolFilename).ToUpper() == ".SHD"))
      {
				ShadowFilename = Path.ChangeExtension(SpoolFilename, ".SHD");
			}
      else
      {
				ShadowFilename = SpoolFilename;
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

			SpoolFilename = Path.ChangeExtension(SpoolFilename, ".SPL");

			//\\ Open a binary reader for the spool file
			System.IO.FileStream SpoolFileStream = new System.IO.FileStream(SpoolFilename, FileMode.Open, FileAccess.Read);
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
				SkipAHeader(recNext, ref SpoolBinaryReader);
				recNext = NextHeader(ref SpoolBinaryReader);
			}

			//\\ Close the spool file
			SpoolBinaryReader.Close();
			SpoolFileStream.Close();

			return _Pages * _Copies;
		}
		#endregion

		#region "Private functions"
		private EMFMetaRecordHeader NextHeader(ref BinaryReader SpoolBinaryReader)
		{
			EMFMetaRecordHeader functionReturnValue = default(EMFMetaRecordHeader);

			EMFMetaRecordHeader recRet = new EMFMetaRecordHeader();

      //\\ get the record type
      recRet.Seek = SpoolBinaryReader.BaseStream.Position;
			try
      {
        recRet.iType = (EMFSpoolfileReader.SpoolerRecordTypes)SpoolBinaryReader.ReadInt32();
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


		private void SkipAHeader(EMFMetaRecordHeader Header, ref BinaryReader SpoolBinaryReader)
		{
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
				ProcessEMFRecords(Header, SpoolBinaryReader);
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

		}

		private void ProcessEMFRecords(EMFMetaRecordHeader EMFRecord, BinaryReader SpoolBinaryReader)
		{
			long nNextRecordStart = 0;

			nNextRecordStart = EMFRecord.Seek + 8;

			SpoolBinaryReader.BaseStream.Seek(nNextRecordStart, SeekOrigin.Begin);

			//\\ EMRMETAHEADER followed by other EMR records 
			EMFPage ThisPage = new EMFPage(SpoolBinaryReader);
			_EMFPages.Add(ThisPage);
			nNextRecordStart = nNextRecordStart + ThisPage.Header.FileSize;
			SpoolBinaryReader.BaseStream.Seek(nNextRecordStart, SeekOrigin.Begin);

		}
		#endregion

		#region "Public interface"
		public EMFPages Pages {
			get { return _EMFPages; }
		}
		#endregion

		#region "Public constructor"
		public EMFSpoolfileReader()
		{
			InitialiseCounterCategory();
			InitialiseCounter();
		}
		#endregion
	}
}

