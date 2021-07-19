using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace iCho.UI.Icons
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MaterialIcons : ResourceDictionary
    {
        public MaterialIcons()
        {
            InitializeComponent();
        }
    }
}
