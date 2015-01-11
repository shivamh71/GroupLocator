using GroupLocator.Common;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
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
    public sealed partial class tracker : Page
    {
        private NavigationHelper navigationHelper;

        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        public ObservableCollection<Member> myMembers = new ObservableCollection<Member>();

        public tracker()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            memberList.DataContext = myMembers;
            fetchMyMembers();
            //renderMapView();
        }

        public async void fetchMyMembers()
        {
            MobileServiceCollection<Membership, Membership> items;
            items = await GlobalVars.membershipTable
                .Where(membership => membership.groupId == GlobalVars.groupId)
                .ToCollectionAsync();
            

            foreach (Membership m in items)
            {
                MobileServiceCollection<User, User> userItems;
                userItems = await GlobalVars.userTable
                    .Where(user => user.emailId == m.emailId)
                    .ToCollectionAsync();

                //string location = await FindLocationFromLatLong(userItems[0].latitude, userItems[0].longitude);
                string location = await FindLocationFromLatLong(userItems[0].latitude, userItems[0].longitude);
                string timestamp = String.Format("{0:m}", userItems[0].lastSeen);
                timestamp += ", "+String.Format("{0:t}", userItems[0].lastSeen);

                myMembers.Add(new Member(userItems[0].userName,  location,
                    userItems[0].latitude, userItems[0].longitude, timestamp));
            }

            MapControl1.Center =
                new Geopoint(new BasicGeoposition()
                {
                    Latitude = GlobalVars.currentUser.latitude,
                    Longitude = GlobalVars.currentUser.longitude
                });
            MapControl1.ZoomLevel = 12;
            MapControl1.LandmarksVisible = false;
            foreach (Member m in myMembers)
            {
                addPin(m);
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

        private async void addPin(Member m)
        {
            MapIcon MapIcon1 = new MapIcon();
            MapIcon1.Location = new Geopoint(new BasicGeoposition()
            {
                Latitude = m.latitude,
                Longitude = m.longitude
            });
            
            MapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
            MapIcon1.Title = m.userName;
            MapControl1.MapElements.Add(MapIcon1);
            Windows.UI.Xaml.Shapes.Ellipse fence = new Windows.UI.Xaml.Shapes.Ellipse();
            MapControl1.Children.Add(fence);
            MapControl.SetLocation(fence, MapIcon1.Location);
            await MapControl1.TrySetViewAsync(MapIcon1.Location, 18D, 0, 0, MapAnimationKind.Bow);
        }

        private void renderMapView()
        {
            
            MapControl1.Center =
                new Geopoint(new BasicGeoposition()
                {
                    Latitude = GlobalVars.currentUser.latitude,
                    Longitude = GlobalVars.currentUser.longitude
                });
            MapControl1.ZoomLevel = 12;
            MapControl1.LandmarksVisible = false;
            foreach (Member m in myMembers)
            {
                addPin(m);
            }         
        }

        private void listView_Click(object sender, RoutedEventArgs e)
        {
        }

        public static async Task<string> FindLocationFromLatLong(double latitude, double longitude)
        {
            Debug.WriteLine("Fetching...");

            string loc = String.Empty;
            string bingMapsKey = "AuNMbb3S69zU1wAhAH7rbXpzlYXNypGSGadNH0k6RvPdxctH8MgWXUr-Jdb8GXwj";
            if (latitude != 0 || longitude != 0)
            {
                try
                {
                    string url = "http://dev.virtualearth.net/REST/v1/Locations/" + latitude + "," + longitude + "?key=" + bingMapsKey;

                    HttpClient httpClient = new HttpClient();
                    var response = await httpClient.GetAsync(url);
                    var json = await response.Content.ReadAsStringAsync();

                    JObject root = JObject.Parse(json);
                    JArray resourceSets = (JArray)root["resourceSets"];
                    JArray resources = (JArray)resourceSets[0]["resources"];
                    
                    JToken addressLine = resources[0]["address"]["addressLine"];
                    JToken adminDistrict = resources[0]["address"]["adminDistrict"];
                    JToken countryRegion = resources[0]["address"]["countryRegion"];
                    JToken city = resources[0]["address"]["locality"];


                    loc = addressLine.ToString() + ", " + adminDistrict.ToString() + ", " + city.ToString() + ", " + countryRegion.ToString();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return loc;
        }
    }
}
