using System.Collections.Generic;

namespace Renamer.Model
{
    public class ConvertAction
    {
        public List<BaseVo> VoList { get; set; }
        public bool IncludeFloderOption { get; set; }
        public string Where { get; set; }
        public string To { get; set; }

        public ConvertAction(List<BaseVo> voList, bool includeFolderOption, string where, string to)
        {
            this.VoList = voList;
            this.IncludeFloderOption = IncludeFloderOption;
            this.Where = where;
            this.To = to;
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
