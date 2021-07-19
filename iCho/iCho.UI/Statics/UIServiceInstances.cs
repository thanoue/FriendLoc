using System;
using iCho.Service;
using Xamarin.Forms;

namespace iCho.UI.Statics
{
    public static class UIServiceInstances
    {
        public static IPhotoPickerService PhotoPickerService => DependencyService.Get<IPhotoPickerService>();
    }
}
