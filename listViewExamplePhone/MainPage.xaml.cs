using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace GroupLocator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page 
    {
        public ObservableCollection<Group> Groups { get; set; }


        public MainPage()
        {
            this.InitializeComponent();
            Groups = new ObservableCollection<Group>();
            //GroupItems.DataContext = Groups;
            this.NavigationCacheMode = NavigationCacheMode.Required;
            
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
            
        }

        private void AddGroup_Click(object sender, RoutedEventArgs e)
        {
            Group nGroup = new Group(12, "My family");
            Groups.Add(nGroup);
        }

        private void GroupClicked(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("Selected: {0}", e.AddedItems[0]);
            //Frame.Navigate(typeof(MainPage));
        }

        private void clickFunction(object sender, RoutedEventArgs e)
        {
            //t1.Text = "Text is changed";

        }

        private void signInButton_Click(object sender, RoutedEventArgs e)
        {
            bool isEnrolled = false;
            /*
             send a request to authenticate (set is enrooled)
             * 
             */
            // authenticate user
            if (isEnrolled)
            {
               // send query to get group IDs and names and invites name and ID
               // update 
                StaticUser.emailId = emailId.Text;
                // Fill user info
                Frame.Navigate(typeof(profile));
            }
            else
            {
                Frame.Navigate(typeof(MainPage));
            }
        }

        private void signUpButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(signUpPage));
        }
    }
}
