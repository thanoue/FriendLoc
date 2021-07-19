using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using iCho.Core.Utils;
using iCho.UI.Statics;
using iCho.UI.Views.Forms;
using Newtonsoft.Json;
using Plugin.FacebookClient;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace iCho.UI.ViewModels.Forms
{
    /// <summary>
    /// ViewModel for login page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class LoginPageViewModel : LoginViewModel
    {
        #region Fields

        private string password;
     
        string _platFormType;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="LoginPageViewModel" /> class.
        /// </summary>
        public LoginPageViewModel()
        {
            this.LoginCommand = new Command(this.LoginClicked);
            this.SignUpCommand = new Command(this.SignUpClicked);
            this.ForgotPasswordCommand = new Command(this.ForgotPasswordClicked);
            this.FacebookLoginCommand = new Command(this.FacebookLoggedIn);
            this.GoogleLoginCommand = new Command(this.GoogleLoggedIn);

            if (Device.RuntimePlatform == Device.iOS)
            {
                _platFormType = "IOS";
            }
            else
                _platFormType = "ANDROID";
           
        }

        #endregion

        #region property

        /// <summary>
        /// Gets or sets the property that is bound with an entry that gets the password from user in the login page.
        /// </summary>
        public string Password
        {
            get
            {
                return this.password;
            }

            set
            {
                if (this.password == value)
                {
                    return;
                }

                this.password = value;
                this.NotifyPropertyChanged();
            }
        }

        #endregion

        #region Command

        /// <summary>
        /// Gets or sets the command that is executed when the Log In button is clicked.
        /// </summary>
        public Command LoginCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Sign Up button is clicked.
        /// </summary>
        public Command SignUpCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Forgot Password button is clicked.
        /// </summary>
        public Command ForgotPasswordCommand { get; set; }

        public Command FacebookLoginCommand { get; set; }
        public Command GoogleLoginCommand { get; set; }

        #endregion

        #region methods

        /// <summary>
        /// Invoked when the Log In button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void LoginClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the Sign Up button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void SignUpClicked(object obj)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SignUpPage());
        }

        /// <summary>
        /// Invoked when the Forgot Password button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void ForgotPasswordClicked(object obj)
        {
            var label = obj as Label;
            label.BackgroundColor = Color.FromHex("#70FFFFFF");
            await Task.Delay(100);
            label.BackgroundColor = Color.Transparent;
        }

        /// <summary>
        /// Invoked when social media login button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void FacebookLoggedIn(object obj)
        {
            var token = await ServiceInstances.SocialMediaAuthService.FacebookLogin();

            if (!string.IsNullOrEmpty(token))
            {
                Login(false, token);
            }
        }

        private async void GoogleLoggedIn(object obj)
        {
            var token = await ServiceInstances.SocialMediaAuthService.GoogleLogin();

            if (!string.IsNullOrEmpty(token))
            {
                Login(true, token);
            }
        }


        async void Login(bool isGogle,string token = "")
        {
            var user = isGogle ? await ServiceInstances.ApiClient.LoginByGoogle(token) : await ServiceInstances.ApiClient.LoginByFacebook(token);

            if (user != null)
            {
                using (var stream = await UIServiceInstances.PhotoPickerService.GetImageStreamAsync())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);
                        var data = memoryStream.ToArray();

                        var house = await ServiceInstances.ApiClient.CreateHouse(new Core.ApiModel.HouseApiModel()
                        {
                            Address = "200 duong dinh hoi",
                            FullName = "Nha Minh",
                            OwnerId = ServiceInstances.ApiClient.User.Id,
                            PhoneNumber = "0982738482"
                        }, data);

                        if(house != null)
                        {

                        }

                    }
                }
            }
        }
    }

    #endregion
}
