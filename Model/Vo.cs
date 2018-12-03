using System.IO;

namespace Renamer.Model
{
    public static class AppState
    {
        public static readonly string INIT = "Initial";
        public static readonly string SELECT_DIR_DONE = "directory just selected";
        public static readonly string QUERY_LIST_DONE = "list just queried";
        public static readonly string CONVERT_DONE = "just converted";
        public static readonly string UNDONE = "just undone";
    }


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
    

    public enum VoType
    {
        FILE,
        DIRECTORY
    }


    public class QNode
    {
        public bool Root { get; set; }
        public DirectoryInfo DirInfo { get; set; }
    }


    public abstract class BaseVo
    {
        public int Index { get; set; }
        public string PathOriginal { get; set; }
        public string Status { get; set; }
        public string PathMoved { get; set; }
        public VoType TypeOfVo { get; set; }

        //public FileVo(int index, FileInfo fi, string status)
        public BaseVo(int index, string fullPath, string status, VoType type)
        {
            //this.fi = fi;
            this.Index = index;
            //this.PathOriginal = fi.FullName;
            this.PathOriginal = fullPath;
            this.Status = status;
            this.TypeOfVo = type;
        }


        public abstract bool ExistsInOriginalPath();

        public abstract void DefinePathMoved(string where, string to);

        public abstract bool ExistsInMovedPath();

        public abstract void Convert();

        /// <summary>
        /// convert 된 파일을 revert 한다.
        /// (PathMoved 에서 convTo 를 convWhere 로 Replace 한다.)
        /// </summary>
        /// <param name="convWhere">convert 되었을 때 원래 파일명의 검색 문자열</param>
        /// <param name="convTo">conver 되었을 때 원래 파일명의 바뀐 문자열</param>
        public abstract void Revert(string convWhere, string convTo);

        


        //public void SetPathMoved(string where, string to)
        //{
        //    PathMoved = PathOriginal.Replace(where, to);
        //}

    }

    public class FileVo : BaseVo
    {
        private FileInfo orgInfo;
        private FileInfo movedInfo;

        public FileVo(int index, FileInfo fi, string status) 
            : base(index, fi.FullName, status, VoType.FILE)
        {
            this.orgInfo = fi;
        }

        public override bool ExistsInOriginalPath()
        {
            return File.Exists(PathOriginal);
        }

        public override void DefinePathMoved(string where, string to)
        {
            PathMoved = orgInfo.DirectoryName + @"\" + orgInfo.Name.Replace(where, to);
            movedInfo = new FileInfo(PathMoved);
        }
        
        public override bool ExistsInMovedPath()
        {
            //return File.Exists(PathMoved);    // PathMoved 에 대한 FileInfo 객체를 또 만들것 같음
            return movedInfo.Exists;
        }

        public override void Convert()
        {
            File.Move(PathOriginal, PathMoved);
        }

        public override void Revert(string convWhere, string convTo)
        {
            //File.Move(PathMoved, PathOriginal);   // 이렇게 하면 Parent 디렉토리가 변경 된 경우, 원복하지 못한다. 따라서 파일명만 먼저 바꾸고 디렉토리는 다시 바꿔주는 로직이 있어야 한다.
            File.Move(PathMoved, movedInfo.DirectoryName + @"\" + movedInfo.Name.Replace(convTo, convWhere));                    
        }

    }


    public class DirectoryVo : BaseVo
    {
        private DirectoryInfo orgInfo;
        private DirectoryInfo movedInfo;

        public DirectoryVo(int index, DirectoryInfo di, string status)
            : base(index, di.FullName, status, VoType.DIRECTORY)
        {
            this.orgInfo = di;
        }

        public override bool ExistsInOriginalPath()
        {
            return Directory.Exists(PathOriginal);
        }

        public override void DefinePathMoved(string where, string to)
        {
            PathMoved = orgInfo.Parent.FullName + @"\" + orgInfo.Name.Replace(where, to);
            movedInfo = new DirectoryInfo(PathMoved);
        }

        public override bool ExistsInMovedPath()
        {
            //return Directory.Exists(PathMoved);   // 마찬가지로 Exists 안에서 PathMoved 에 대한 DirectoryInfo 를 또 만드는 것이 아닐까 함
            return movedInfo.Exists;
        }

        public override void Convert()
        {
            Directory.Move(PathOriginal, PathMoved);
        }

        public override void Revert(string convWhere, string convTo)
        {
            //Directory.Move(PathMoved, PathOriginal);
            Directory.Move(PathMoved, movedInfo.Parent.FullName + @"\" + movedInfo.Name.Replace(convTo, convWhere));
        }
    }
    
}
