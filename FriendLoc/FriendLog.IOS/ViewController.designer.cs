// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace FriendLog.IOS
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton loginBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton startBtn { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (loginBtn != null) {
                loginBtn.Dispose ();
                loginBtn = null;
            }

            if (startBtn != null) {
                startBtn.Dispose ();
                startBtn = null;
            }
        }
    }
}