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


    public class ActionResult
    {
        public int CountTotal { get; set; }
        public int CountSuccess { get; set; }
        public int CountFail { get; set; }

        public ActionResult()
        {
            this.CountTotal = 0;
            this.CountSuccess = 0;
            this.CountFail = 0;
        }
    }
}
