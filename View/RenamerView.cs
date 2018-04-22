
namespace Renamer.View
{
    public delegate void ViewEventHandler();
    public delegate void ViewEventHandler<T>(T param);


    public interface IView
    {
        void Build();
        event ViewEventHandler OnBuilt;
    }
}
