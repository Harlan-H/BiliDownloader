using BiliDownloader.ViewModels.Dialogs;
using System.Threading.Tasks;

namespace BiliDownloader.ViewModels
{
    public partial class MessageBoxViewModel : DialogScreen
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

    public partial class MessageBoxViewModel
    {
        public static MessageBoxViewModel CreateMessageBoxViewModel(string title,string message, string confirmText = "确定", string CancelText= "取消")
        {
            var view = new MessageBoxViewModel
            {
                Title = title,
                Message = message,
                CancelBtnText = CancelText,
                ConfirmBtnText = confirmText
            };
            return view;
        }

        public static async Task<bool> ShowMessageBox(string title, string message)
        {
            var dialog = CreateMessageBoxViewModel(title, message);
            return await DialogManager.ShowDialogAsync(dialog);
        }
    }
}
