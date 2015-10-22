using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Refractored.Xam.Settings;
using Android.Content;

namespace MWWebApp
{
	[Activity(Label = "MWWebApp", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		private const string WebUrlKey = "WebUrl";

		private Button startButton;
		private EditText urlEditText;
		private CheckBox rememberUrlCheckBox;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			this.SetContentView(Resource.Layout.Main);

			this.urlEditText = FindViewById<EditText>(Resource.Id.txtUrl);
			this.rememberUrlCheckBox = FindViewById<CheckBox>(Resource.Id.cbxRememberUrl);
			this.startButton = FindViewById<Button>(Resource.Id.btnStart);

			var savedUrl = CrossSettings.Current.GetValueOrDefault<string>(WebUrlKey);
			if (!string.IsNullOrEmpty(savedUrl))
			{
				this.urlEditText.Text = savedUrl;
				this.rememberUrlCheckBox.Checked = true;

				this.ShowWebVersion(savedUrl);
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			this.startButton.Click += this.OnStartButtonClicked;
		}

		protected override void OnStop()
		{
			base.OnStop();
			this.startButton.Click -= this.OnStartButtonClicked;
		}

		private void OnStartButtonClicked(object sender, EventArgs e)
		{
			var url = this.urlEditText.Text;
			if (!string.IsNullOrEmpty(url))
			{
				if (this.rememberUrlCheckBox.Checked)
				{
					CrossSettings.Current.AddOrUpdateValue(WebUrlKey, url);
				}
				else
				{
					CrossSettings.Current.AddOrUpdateValue(WebUrlKey, string.Empty);
				}

				this.ShowWebVersion(url);
			}
		}

		private void ShowWebVersion(string url)
		{
			var intent = new Intent(this, typeof(WebWrapperActivity));
			intent.PutExtra(WebWrapperActivity.WebAddressKey, url);
			this.StartActivity(intent);
		}
	}
}