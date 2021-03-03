using System;
using DT.Xamarin.Agora.Rtm;

namespace DT.Samples.Agora.Rtm.Droid
{
    public class ResultCallback : Java.Lang.Object, IResultCallback
    {
        public Action<ErrorInfo> OnFailureAction;
        public Action<Java.Lang.Object> OnSuccessAction;

        public void OnFailure(ErrorInfo errorInfo)
        {
            OnFailureAction?.Invoke(errorInfo);
        }

        public void OnSuccess(Java.Lang.Object arg)
        {
            OnSuccessAction?.Invoke(arg);
        }
    }
}