using System;
namespace FriendLoc.Droid.ViewModels
{
    public enum SpinnerTypes
    {
        None,
        SingleTitle,
        MultiTitle
    }

    public abstract class SpinnerItem
    {
        public abstract SpinnerTypes Type {get;}

        public string MainTitle { get; set; }
        public object Value { get; set; }
        public string LeftImgUrl { get; set; }
        public int LeftImgResId { get; set; }

        public SpinnerItem()
        {

        }
    }

    public class SingleTitleSpinnerItem : SpinnerItem
    {
        public override SpinnerTypes Type => SpinnerTypes.SingleTitle;
    }

    public class MultiTitleSpinnerItem : SpinnerItem
    {
        public override SpinnerTypes Type => SpinnerTypes.MultiTitle;

        public string SubTitle { get; set; }
    }
}
