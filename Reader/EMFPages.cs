using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMFSpoolfileReader
{
  public class EMFPages
  {
    private List<EMFPage> emfPages;
    public EMFPages()
    {
      emfPages = new List<EMFPage>();
    }

    public void Add( EMFPage page )
    {
      emfPages.Add( page );
    }

    public int Count()
    {
      return emfPages.Count;
    }

    public List<EMFPage> Pages
    {
      get
      {
        return emfPages;
      }
    }
  }
}
