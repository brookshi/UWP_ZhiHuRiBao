using ListViewSample.Model;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using XamlListView.Samples.SimpleListViewSample;

namespace ListViewSample
{
    public sealed partial class SimpleListViewSample : Page
    {
        public SimpleListViewSample()
        {
            this.InitializeComponent();
            //ContactsCVS.Source = Contact.GetContactsGrouped(250); 
            var Contacts = Contact.GetContacts(140);
            if (Contacts.Count > 0)
            {
                MasterListView.ItemsSource = Contacts;
            }
            //RefreshPanel.RefreshAction = _action;
        }
        private void ShowSliptView(object sender, RoutedEventArgs e)
        {
           // MySamplesPane.SamplesSplitView.IsPaneOpen = !MySamplesPane.SamplesSplitView.IsPaneOpen;
        }

        private void MasterListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Debug.WriteLine("click item");
        }

        IRefreshAction _action = new MaterialDesignRefreshAction();
        public IRefreshAction RefreshAction { get { return _action; } }
    }
}
