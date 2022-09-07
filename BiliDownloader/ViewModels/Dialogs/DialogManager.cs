using MaterialDesignThemes.Wpf;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliDownloader.ViewModels.Dialogs
{
    public class DialogManager
    {
        private readonly IViewManager viewManager;

        public DialogManager(IViewManager viewManager)
        {
            this.viewManager = viewManager;
        }

        public async Task<T?> ShowDialogAsync<T>(DialogScreen<T> dialogScreen)
        {
            var view = viewManager.CreateAndBindViewForModelIfNecessary(dialogScreen);

            void OnDialogOpened(object? openSender, DialogOpenedEventArgs openArgs)
            {
                // Delegate to close the dialog and unregister event handler
                void OnScreenClosed(object? closeSender, EventArgs closeArgs)
                {
                    openArgs.Session.Close();
                    dialogScreen.Closed -= OnScreenClosed;
                }

                dialogScreen.Closed += OnScreenClosed;
            }
            await DialogHost.Show(view, OnDialogOpened);

            return dialogScreen.DialogResult;
        }


    }
}
