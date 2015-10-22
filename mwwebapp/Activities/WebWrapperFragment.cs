using Android.App;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Android.Content;
using Android.Net;
using Android.Runtime;
using Android.Widget;

namespace MWWebApp
{
	public class WebWrapperFragment : Fragment
	{
		private Bundle webViewState;
		private WebView webView;

		public string Url 
		{
			get
			{
				return this.webView.Url;
			}
			set
			{
				if (value != this.webView.Url)
				{
					this.webView.LoadUrl(value);
				}
			}
		}

		public bool CanGoBack
		{
			get
			{
				return this.webView.CanGoBack();
			}
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			this.RetainInstance = true;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view =  inflater.Inflate(Resource.Layout.WebWrapper, container, false);
			this.webView = view.FindViewById<WebView>(Resource.Id.webView);
			this.webView.Settings.LoadWithOverviewMode = true;
			this.webView.Settings.BuiltInZoomControls = true;
			this.webView.Settings.DisplayZoomControls = false;
			this.webView.Settings.UseWideViewPort = true;
			this.webView.Settings.JavaScriptEnabled = true;
			this.webView.Settings.JavaScriptCanOpenWindowsAutomatically = true;
			this.webView.SetInitialScale(0);
			this.webView.SetWebViewClient(new WebViewClient());
			this.webView.Download += this.OnFileDownload;
			if (this.webViewState != null)
			{
				this.webView.RestoreState(this.webViewState);
			}

			return view;
		}

		private void OnFileDownload (object sender, DownloadEventArgs e)
		{
			var request = new DownloadManager.Request(Uri.Parse(e.Url));
			// TODO: this should be fixed on the server!
			request.AllowScanningByMediaScanner();
			request.SetNotificationVisibility(DownloadVisibility.VisibleNotifyCompleted);
			// TODO: set proper file name
			//e.u
			request.SetDestinationInExternalPublicDir(Environment.DirectoryDownloads, "report.docx");
			var dm = this.Activity.GetSystemService(Context.DownloadService).JavaCast<DownloadManager>();
			dm.Enqueue(request);
			this.GoBack();
			Toast.MakeText(this.Activity, "Свалям файла...", ToastLength.Long).Show();
		}

		public override void OnPause()
		{
			this.webViewState = new Bundle();
			this.webView.SaveState(this.webViewState);
			this.webView.Download -= this.OnFileDownload;
			base.OnPause();
		}

		public void GoBack()
		{
			this.webView.GoBack();
		}
	}
}