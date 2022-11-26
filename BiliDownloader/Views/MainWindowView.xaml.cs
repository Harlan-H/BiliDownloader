using System.Windows;
using System.Windows.Input;

namespace BiliDownloader.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowView()
        {
            InitializeComponent();
        }

//         private void TextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
//         {
//             if (e.Key == Key.Enter && e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
//             {
//                 e.Handled = true;
// 
//                 // We handle the event here so we have to directly "press" the default button
//                 AccessKeyManager.ProcessKey(null, "\x000D", false);
//             }
//         }
    }
}
