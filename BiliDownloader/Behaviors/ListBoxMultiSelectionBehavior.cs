using BiliDownloader.Core.Videos;
using BiliDownloader.Core.Videos.Pages;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BiliDownloader.Behaviors
{
    public class ListBoxMultiSelectionBehavior : Behavior<ListBox>
    {
        // Using a DependencyProperty as the backing store for SelectedItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(IList), typeof(ListBoxMultiSelectionBehavior),
                new FrameworkPropertyMetadata(null, OnSelectedItemsChanged));

        
        public IList SelectedItems
        {
            get => (IList)GetValue(SelectedItemsProperty);
            set => SetValue(SelectedItemsProperty, value);
        }


        //当ui被初始化完成之后会进入此函数
        //是因为上面的属性注册 当SelectedItems的值发生改变的时候 
        //这里改变是因为 初始化的时候 需要全部选中 就会导致这里触发回调 
        //如果不需要刚开始就全部选中 可以不用这个回调 
        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (ListBoxMultiSelectionBehavior)d;
            if (behavior._modelHandled) return;

            if (behavior.AssociatedObject is null)
                return;

            if (behavior.AssociatedObject.SelectedItems.Count > 0)
                return;

            behavior._modelHandled = true;
            behavior.SelectItems();
            behavior._modelHandled = false;
        }

        private bool _viewHandled;
        private bool _modelHandled;

        private void SelectItems()
        {
            _viewHandled = true;

            if (SelectedItems is not null)
            {
                foreach (var item in SelectedItems)
                    AssociatedObject.SelectedItems.Add(item);
            }

            _viewHandled = false;
        }

        //这个事件是 当某一行或者很多行被选择的时候 会执行到此处
        private void OnListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //这里判断_viewHandled 是因为 当上面的AssociatedObject.SelectedItems.Add被执行的时候
            //这个事件也会被触发 所以为了防止多次操作同一个数据集 这里直接退出
            if (_viewHandled) return;
            if (AssociatedObject.Items.SourceCollection is null) return;

            SelectedItems = AssociatedObject.SelectedItems.Cast<IPlaylist>().ToArray();
        }
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += OnListBoxSelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectionChanged -= OnListBoxSelectionChanged;
        }
    }
}
