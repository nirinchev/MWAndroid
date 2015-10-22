using Android.App;
using Android.OS;
using Android.Webkit;
using Android.Views;

namespace MWWebApp
{
	[Activity(Label = "WebWrapperActivity")]			
	public class WebWrapperActivity : Activity
	{
		public const string WebAddressKey = "WebAddress";
		private readonly MWWebViewClient webViewClient = new MWWebViewClient();

		private WebView webView;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			this.RequestWindowFeature(WindowFeatures.NoTitle);

			this.SetContentView(Resource.Layout.WebWrapper);

			this.webView = this.FindViewById<WebView>(Resource.Id.webView);
			this.webView.Settings.LoadWithOverviewMode = true;
			this.webView.Settings.BuiltInZoomControls = true;
			this.webView.Settings.DisplayZoomControls = false;
			this.webView.Settings.UseWideViewPort = true;
			this.webView.Settings.JavaScriptEnabled = true;
			this.webView.Settings.JavaScriptCanOpenWindowsAutomatically = true;
			this.webView.SetInitialScale(0);
			this.webView.LoadUrl(this.Intent.GetStringExtra(WebAddressKey));
			this.webView.SetWebViewClient(this.webViewClient);
		}

		protected override void OnSaveInstanceState(Bundle outState)
		{
			base.OnSaveInstanceState(outState);

			outState.PutString(WebAddressKey, this.webView.Url);
		}

		protected override void OnRestoreInstanceState(Bundle savedInstanceState)
		{
			base.OnRestoreInstanceState(savedInstanceState);

			var url = savedInstanceState.GetString(WebAddressKey);
			if (!string.IsNullOrEmpty(url))
			{
				this.webView.LoadUrl(url);
			}
		}

		public override void OnBackPressed()
		{
			if (this.webView.CanGoBack())
			{
				this.webView.GoBack();
			}
			else
			{
				base.OnBackPressed();
			}
		}

		private class MWWebViewClient : WebViewClient
		{
			public override bool ShouldOverrideUrlLoading(WebView view, string url)
			{
				view.LoadUrl(url);
				return true;
			}
		}
	}
}