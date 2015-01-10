using GroupLocator.Common;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace GroupLocator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class profile : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ObservableCollection<Group> myGroups = new ObservableCollection<Group>();
        public ObservableCollection<InviteItem> myInvites = new ObservableCollection<InviteItem>();

        public profile()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            GroupItems.DataContext = myGroups;
            InviteItems.DataContext = myInvites;

            fetchMyGroups();
            fetchMyInvites();

        }

        public async void fetchMyGroups()
        {
            MobileServiceCollection<Membership, Membership> items;
            items = await GlobalVars.membershipTable
                .Where(membership => membership.emailId == GlobalVars.currentUser.emailId)
                .ToCollectionAsync();
            foreach (Membership m in items)
            {
                MobileServiceCollection<Group, Group> groupItems;
                groupItems = await GlobalVars.groupTable
                    .Where(group => group.Id == m.groupId)
                    .ToCollectionAsync();
                myGroups.Add(groupItems[0]);
            }

        }

        public async void fetchMyInvites()
        {
            MobileServiceCollection<Invite, Invite> items;
            items = await GlobalVars.inviteTable
                .Where(Invite => Invite.receiverEmailId == GlobalVars.currentUser.emailId)
                .ToCollectionAsync();
            foreach (Invite inv in items)
            {
                Debug.WriteLine(inv.senderEmailId + " " + inv.receiverEmailId+ "\n");

                MobileServiceCollection<Group, Group> groupItems;
                groupItems = await GlobalVars.groupTable
                    .Where(group => group.Id == inv.groupId)
                    .ToCollectionAsync();
                myInvites.Add(new InviteItem(groupItems[0].Id, groupItems[0].groupName, inv.senderEmailId));
            }

        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void InviteClicked(object sender, SelectionChangedEventArgs e)
        {
            var dialog = new MessageDialog("Do you accept this invite?", "Confirmation");
            dialog.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler(DeleteCommandHandler), e.AddedItems[0]));
            dialog.Commands.Add(new UICommand("No", new UICommandInvokedHandler(DeleteCommandHandler), e.AddedItems[0]));
            await dialog.ShowAsync();
        }

        private async void DeleteCommandHandler(IUICommand command)
        {
            var commandLabel = command.Label;
            InviteItem inv = command.Id as InviteItem;
            switch (commandLabel)
            {
                case "Yes":
                    Debug.WriteLine("Yes is clicked");

                    Membership m = new Membership(inv.groupId, GlobalVars.currentUser.emailId);
                    await GlobalVars.membershipTable.InsertAsync(m);

                    break;
                case "No":
                    Debug.WriteLine("Yes is clicked");
                    break;
            }
            MobileServiceCollection<Invite, Invite> items;
            items = await GlobalVars.inviteTable
                .Where(Invite => Invite.receiverEmailId == GlobalVars.currentUser.emailId && Invite.senderEmailId == inv.senderEmailId && Invite.groupId == inv.groupId)
                .ToCollectionAsync();

            await GlobalVars.inviteTable.DeleteAsync(items[0]);
            Frame.Navigate(typeof(profile));
        }

        // which group is clicked. 
        private void GroupClicked(object sender, SelectionChangedEventArgs e)
        {
            Group groupClicked = e.AddedItems[0] as Group;
            GlobalVars.groupId = groupClicked.Id;
            Debug.WriteLine(groupClicked.Id);
            Frame.Navigate(typeof(tracker));
            
        }

        private void addGroup_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(addGroup));
        }

        private void acceptInvite_click(object sender, RoutedEventArgs e)
        {
            //InviteItem inv = sender as InviteItem;
            

        }

        private void declineInvite_click(object sender, RoutedEventArgs e)
        {

        }
    }

    
}
