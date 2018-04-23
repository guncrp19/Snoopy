using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
namespace EMFSpoolfileReader
{
	public interface ISpoolfileReaderBase
	{
		int GetTruePageCount(string SpoolFilename);
	}
}
