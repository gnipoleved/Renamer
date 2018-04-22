using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
