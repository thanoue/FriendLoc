using System;

namespace FriendLoc.Common.Services
{
    public interface IGlobalUIService
    {
        void StartLoading();
        void StopLoading();
        void ErrorToast(string content, Action onClick = null);
        void WarningToast(string content, Action onClick = null);
        void InfToast(string content, Action onClick = null);
        void SuccessToast(string content, Action onClick = null);
    }
}