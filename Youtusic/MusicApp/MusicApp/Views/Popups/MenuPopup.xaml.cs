using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicApp.Converter;
using MusicApp.ViewModel;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicApp.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPopup : BasePopup
    {
        private IList<BottomMenuItem> _items;
        public  string SearchIcon { get; set; }
        private Action<BottomMenuItem> _onSelected;
        public MenuPopup(Action<BottomMenuItem> onSelected, IList<BottomMenuItem> items)
        {
            _onSelected = onSelected;
            _items = items;
            InitializeComponent();
            AddMenuItem();
        }

        void AddMenuItem()
        {
            int index = 0;
            foreach (var item in _items)
            {
                var viewItem = new StackLayout()
                {
                    Padding = new Thickness(10,0,10,0),
                    Orientation = StackOrientation.Horizontal,
                    HeightRequest = 50,
                    Spacing = 10,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.White
                };
                
                viewItem.GestureRecognizers.Add(new TapGestureRecognizer((view) =>
                {
                    _onSelected?.Invoke(item);
                    this.Dismis();
                }));
                
                var icon = new Label()
                {
                    TextColor = Color.Black,
                    FontSize = 24,
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontFamily = "MaterialIcons",
                    WidthRequest = 40,
                    HeightRequest = 40
                };

                icon.SetBinding(Label.TextProperty, new Binding()
                {
                    Path = "SearchIcon",
                    Source = this,
                    Converter = new MaterialIcon()
                    {
                        Icon = item.Icon
                    }
                });
                
                viewItem.Children.Add(icon);
                
                var titleTv = new Label()
                {
                    TextColor = Color.Black,
                    FontSize = 20,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalTextAlignment = TextAlignment.Center,
                    Text = item.Title
                };
                
                viewItem.Children.Add(titleTv);
                
                menuBg.Children.Add(viewItem);

                index += 1;
            }
        }

        private void OutsizeTouch(object sender, EventArgs e)
        {
            this.Dismis();
        }
    }
}