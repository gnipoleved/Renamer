
namespace Renamer.View
{
    public delegate void ViewEventHandler();
    public delegate void ViewEventHandler<T>(T param);


    public interface IView
    {
        string ErrorMsg { set; }
        string Directory { set; }

        void Build();
        event ViewEventHandler OnBuilt;
        event ViewEventHandler<string> OnDirectorySelected;
    }
}
