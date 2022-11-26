using Caliburn.Micro;
using System;

namespace BiliDownloader.ViewModels.Dialogs
{
    public abstract class DialogScreen<T> : PropertyChangedBase
    {
        public T? DialogResult { get; private set; }

        public event EventHandler? Closed;

        public void Close(T? dialogResult = default)
        {
            DialogResult = dialogResult;
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }

    public class DialogScreen : DialogScreen<bool>
    {

    }
}
