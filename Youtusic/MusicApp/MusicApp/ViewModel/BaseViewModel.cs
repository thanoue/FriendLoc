using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using MusicApp.Converter;
using MusicApp.Static;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;

namespace MusicApp.ViewModel
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private DropShadow _btnShadow;
        public DropShadow BtnShadow
        {
            get { return _btnShadow; }
            set
            {
                _btnShadow = value;
                OnPropertyChanged();
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public BaseViewModel()
        {
            BtnShadow = new DropShadow()
            {
                Opacity = 0.6f,
                Color = Color.Black,
                BlurRadius = 10
            };

        }
    }
}
