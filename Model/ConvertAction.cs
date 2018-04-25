using System.Collections.Generic;

namespace Renamer.Model
{
    public class ConvertAction
    {
        public List<FileVo> FileList { get; set; }
        public string Where { get; set; }
        public string To { get; set; }

        public ConvertAction(List<FileVo> FileList, string Where, string To)
        {
            this.FileList = FileList;
            this.Where = Where;
            this.To = To;
        }

    }
}
