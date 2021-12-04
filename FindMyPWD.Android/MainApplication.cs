using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using FindMyPWD.Droid;
using Plugin.CurrentActivity;
using System;
//using Xamarin.Forms;

[Application]
public class MainApplication : Application, Application.IActivityLifecycleCallbacks
{
    public MainApplication(IntPtr handle, JniHandleOwnership transer)
      : base(handle, transer)
    {
    }

    public override void OnCreate()
    {
        base.OnCreate();
        RegisterActivityLifecycleCallbacks(this);
        //A great place to initialize Xamarin.Insights and Dependency Services!



        //Periodic service....might not be needed read documentation
        //var intent = new Intent(this, typeof(PeriodicService)); //disabled for now
        //StartService(intent);
    }

    public override void OnTerminate()
    {
        base.OnTerminate();
        UnregisterActivityLifecycleCallbacks(this);
    }

    public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
    {
        CrossCurrentActivity.Current.Activity = activity;
    }

    public void OnActivityDestroyed(Activity activity)
    {
    }

    public void OnActivityPaused(Activity activity)
    {
    }

    public void OnActivityResumed(Activity activity)
    {
        CrossCurrentActivity.Current.Activity = activity;
    }

    public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
    {
    }

    public void OnActivityStarted(Activity activity)
    {
        CrossCurrentActivity.Current.Activity = activity;
    }

    public void OnActivityStopped(Activity activity)
    {
    }
}