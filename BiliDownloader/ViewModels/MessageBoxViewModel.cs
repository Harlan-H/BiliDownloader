using BiliDownloader.ViewModels.Dialogs;

namespace BiliDownloader.ViewModels
{
    public class MessageBoxViewModel : DialogScreen
    {
        public string? Title { get; set; }
        public string? Message { get; set; }

        public string ConfirmBtnText { get; set; } = default!;
        public string CancelBtnText { get; set; } = default!;

        public void Confirm()
        {
            Close(true);
        }

        public void Cancel()
        {
            Close(false);
        }
    }

    public static class MessageBoxViewModelExtensions
    {
        public static MessageBoxViewModel CreateMessageBoxViewModel(this IViewModelFactory factory,string title,string message, string confirmText = "确定", string CancelText= "取消")
        {
            var view = factory.CreateMessageBoxViewModel();

            view.Title = title;
            view.Message = message;
            view.CancelBtnText = CancelText;
            view.ConfirmBtnText = confirmText;
            return view;
        }
    }
}
