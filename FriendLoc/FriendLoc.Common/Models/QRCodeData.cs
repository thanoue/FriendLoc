using System;
namespace FriendLoc.Common.Models
{
    public class QRCodeData
    {
        public object Data { get; set; }
        public object Image { get; set; }

        public QRCodeData(object data = null, object image = null)
        {
            Data = data;
            Image = image;
        }
    }
}
