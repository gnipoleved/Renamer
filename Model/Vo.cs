using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renamer.Model
{
    public class FileVo
    {
        //private FileInfo fi;

        public int Index { get; set; }

        public string PathOriginal { get; set; }

        public string Status { get; set; }

        public string PathMoved { get; set; }


        public FileVo(int index, FileInfo fi, string status)
        {
            //this.fi = fi;
            this.Index = index;
            this.PathOriginal = fi.FullName;
            this.Status = status;
        }
        
    }
}
