using System;

using Android.App;
using Android.Content.PM;
using Android.Hardware;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace SKordubaylo.Mobile.Flashlight.Droid
{
    [Activity(Label = "SKordubaylo.Mobile.Flashlight", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {

		Camera camera;
		Button button;
		bool cameraFlashLightOn;

	    protected override void OnCreate(Bundle bundle)
	    {
		    base.OnCreate(bundle);

		    button = FindViewById<Button>(Resource.Id.myButton);

		    //SetContentView(Resource.Layout.Main);

		    global::Xamarin.Forms.Forms.Init(this, bundle);
		    LoadApplication(new App());
	    }

	    protected override void OnResume()
		{
			if (!PackageManager.HasSystemFeature(Android.Content.PM.PackageManager.FeatureCamera))
			{
				Toast.MakeText(this, "No back-facing camera available", ToastLength.Long);
				return;
			}

			if (!PackageManager.HasSystemFeature(Android.Content.PM.PackageManager.FeatureCameraFlash))
			{
				Toast.MakeText(this, "No camera flash available", ToastLength.Long);
				return;
			}

			camera = Camera.Open();
			button.Click += button_Click;

			base.OnResume();
		}

		protected override void OnStop()
		{
			button.Click -= button_Click;
			camera.Release();

			base.OnStop();
		}

		void button_Click(object sender, EventArgs e)
		{
			var parameters = camera.GetParameters();

			if (!cameraFlashLightOn)
			{
				parameters.FlashMode = Camera.Parameters.FlashModeTorch;

				camera.SetParameters(parameters);
				camera.StartPreview();
				cameraFlashLightOn = true;
				return;
			}

			parameters.FlashMode = Camera.Parameters.FlashModeOff;

			camera.SetParameters(parameters);
			camera.StopPreview();
			cameraFlashLightOn = false;
		}
	}
}

