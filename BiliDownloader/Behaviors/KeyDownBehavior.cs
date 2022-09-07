using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BiliDownloader.Behaviors
{
    public class KeyDownBehavior : Behavior<TextBox>
    {
        public string KeyCode
        {
            get { return (string)GetValue(KeyCodeProperty); }
            set { SetValue(KeyCodeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Key.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeyCodeProperty =
            DependencyProperty.Register("Key", typeof(string), typeof(KeyDownBehavior),new PropertyMetadata("\x000D"));


        protected override void OnAttached()
        {
            base.OnAttached();
            if(AssociatedObject is not null)
            {
                AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown; ;
            }
        }

        private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
            {
                e.Handled = true;

                AccessKeyManager.ProcessKey(null, KeyCode, false);
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if(AssociatedObject is not null)
            {
                AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
            }
        }
    }
}
