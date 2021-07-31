﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace MusicApp.Icons
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MaterialDesignIcons : ResourceDictionary
    {
        public MaterialDesignIcons()
        {
            InitializeComponent();
        }
    }
}