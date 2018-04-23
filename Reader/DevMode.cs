using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;

namespace EMFSpoolfileReader
{
	public class DevMode
	{
		private char[] dmDeviceName = new char[65];
		private short dmSpecVersion;
		private short dmDriverVersion;
		private short dmSize;
		private short dmDriverExtra;
		private int dmFields;
		private short dmOrientation;
		private short dmPaperSize;
		private short dmPaperLength;
		private short dmPaperWidth;
		private short dmScale;
		private short dmCopies;
		private short dmDefaultSource;
		private short dmPrintQuality;
		private short dmColor;
		private short dmDuplex;
		private short dmYResolution;
		private short dmTTOption;
		private short dmCollate;
		private char[] dmFormName = new char[33];
		private short dmUnusedPadding;
		private int dmBitsPerPel;
		private int dmPelsWidth;
		private int dmPelsHeight;
		private int dmDisplayFlags;
		private int dmDisplayFrequency;
		private int dmICMMethod;
		private int dmICMIntent;
		private int dmMediaType;
		private int dmDitherType;
		private int dmReserved1;
		private int dmReserved2;
		private int dmPanningWidth;
		private int dmPanningHeight;

		public short Copies
    {
			get {
				if (dmCopies < 1) {
					dmCopies = 1;
				}
				return dmCopies;
			}
		}

		public bool Collate
    {
			get { return (dmCollate > 0); }
		}

		public DevMode(System.IO.BinaryReader FileReader)
		{
			dmDeviceName = FileReader.ReadChars(64);
			dmSpecVersion = FileReader.ReadInt16();
			dmDriverVersion = FileReader.ReadInt16();
			dmSize = FileReader.ReadInt16();
			dmDriverExtra = FileReader.ReadInt16();
			dmFields = FileReader.ReadInt32();
			dmOrientation = FileReader.ReadInt16();
			dmPaperSize = FileReader.ReadInt16();
			dmPaperLength = FileReader.ReadInt16();
			dmPaperWidth = FileReader.ReadInt16();
			dmScale = FileReader.ReadInt16();
			dmCopies = FileReader.ReadInt16();
			dmDefaultSource = FileReader.ReadInt16();
			dmPrintQuality = FileReader.ReadInt16();
			dmColor = FileReader.ReadInt16();
			dmDuplex = FileReader.ReadInt16();
			dmYResolution = FileReader.ReadInt16();
			dmTTOption = FileReader.ReadInt16();
			dmCollate = FileReader.ReadInt16();
			dmFormName = FileReader.ReadChars(32);
			//32 chars
			dmUnusedPadding = FileReader.ReadInt16();
			dmBitsPerPel = FileReader.ReadInt32();
			dmPelsWidth = FileReader.ReadInt32();
			dmPelsHeight = FileReader.ReadInt32();
			dmDisplayFlags = FileReader.ReadInt32();
			dmDisplayFrequency = FileReader.ReadInt32();
			dmICMMethod = FileReader.ReadInt32();
			dmICMIntent = FileReader.ReadInt32();
			dmMediaType = FileReader.ReadInt32();
			dmDitherType = FileReader.ReadInt32();
			dmReserved1 = FileReader.ReadInt32();
			dmReserved2 = FileReader.ReadInt32();
			dmPanningWidth = FileReader.ReadInt32();
			dmPanningHeight = FileReader.ReadInt32();
		}
	}
}
