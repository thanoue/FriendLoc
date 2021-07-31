using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Plugin.MaterialDesignControls;
using Xamarin.Forms;

namespace MusicApp.Controls
{
    public class CustomMaterialEntry : MaterialEntry
    {
        public event EventHandler<CompleteEventArgs> OnCompleted;

        public static BindableProperty ReturnCommandProperty = BindableProperty.Create(nameof(ReturnCommand), typeof(ICommand), typeof(CustomMaterialEntry));
        public ICommand ReturnCommand
        {
            get { return (ICommand)GetValue(ReturnCommandProperty); }
            set { SetValue(ReturnCommandProperty, value); }
        }

        public CustomMaterialEntry() : base()
        {
            var control = (Entry)this.FindByName("txtEntry");
            control.ReturnType = ReturnType.Go;
            // control.Completed += (sender,e)=>
            // {
            //     OnCompleted?.Invoke(this, new CompleteEventArgs(this.Text));
            // };
            
            control.ReturnCommand = new Command<object>((obj) =>
            {
                if(ReturnCommand.CanExecute(obj))
                    ReturnCommand.Execute(obj);
                
            }, (ob) => { return true;});
        }
    }

    public class CompleteEventArgs : EventArgs
    {
        public string Text { get; set; }

        public CompleteEventArgs(string text)
        {
            Text = text;
        }
    }
}
