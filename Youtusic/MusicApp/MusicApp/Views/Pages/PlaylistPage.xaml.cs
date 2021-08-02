using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Ioc;
using MusicApp.ViewModel;
using Xamarin.Forms;

namespace MusicApp.Pages
{
    public partial class PlaylistPage
    {
        public PlaylistPage()
        {
            var vm = SimpleIoc.Default.GetInstance<PlaylistViewModel>();
            vm.PropertyChanged += Vm_PropertyChanged;

            this.BindingContext = vm;


            InitializeComponent();
        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var vm = (PlaylistViewModel)BindingContext;

            switch (e.PropertyName)
            {
                case nameof(vm.PlaylistDescription):

                    if (playlistDescription != null)
                    {
                        playlistDescription.Text = vm.PlaylistDescription;
                    }

                    break;

                case nameof(vm.PlaylistChannelTitle):

                    if (playlistAuthor != null)
                    {
                        playlistAuthor.Text = vm.PlaylistChannelTitle;
                    }

                    break;

                case nameof(vm.PlaylistTitle):

                    if (playlistName != null)
                    {
                        playlistName.Text = vm.PlaylistTitle;
                    }

                    break;

                case nameof(vm.PlaylistThumbnail):

                    if(playlistThumbnail != null)
                    {
                        playlistThumbnail.Src = vm.PlaylistThumbnail;
                    }

                    break;
            }
           
        }
    }
}
