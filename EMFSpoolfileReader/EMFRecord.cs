using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Drawing.Imaging;
namespace EMFSpoolfileReader
{

	public class EMFRecord
	{

		private long _Seek;
		private Int32 _Type;
		private Int32 _Size;

		/// <summary>
		/// The type of this enhanced metafile record 
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public EmfPlusRecordType Type {
			get { return (EmfPlusRecordType)_Type; }
		}

		public Int32 Size {
			get { return _Size; }
		}

		public long Seek {
			get { return _Seek; }
		}

    public EMFRecord( BinaryReader SpoolBinaryReader )
    {
      var _with1 = SpoolBinaryReader;
      _Seek = _with1.BaseStream.Position;
      _Type = _with1.ReadInt32();
      _Size = _with1.ReadInt32();
    }

	}
}
