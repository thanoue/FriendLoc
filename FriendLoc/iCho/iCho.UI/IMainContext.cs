using System;
namespace iCho.UI
{
    public interface IMainContext<T> where T : class
    {
        T Context { get; }

    }
}
