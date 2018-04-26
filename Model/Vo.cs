using System.IO;

namespace Renamer.Model
{
    public static class Status
    {
        public static readonly string STANDBY = "대기";
        public static readonly string ON_CONV = "변환중...";
        public static readonly string CONV_COMPLETE = "[완료]";
        public static readonly string NOT_ABLE_TO_CONV = "@변환실패@";
        public static readonly string ON_REVERT = "원복중...";
        public static readonly string REVERT_COMPLETE = "<원복완료>";
        public static readonly string NOT_ABLE_TO_REVERT = "#원복불가#";
    }
    


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


        public void SetPathMoved(string where, string to)
        {
            PathMoved = PathOriginal.Replace(where, to);
        }
        
    }
}
