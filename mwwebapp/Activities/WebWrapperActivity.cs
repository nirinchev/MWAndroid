using Android.App;
using Android.OS;
using Android.Views;

namespace MWWebApp
{
	[Activity(Label = "WebWrapperActivity")]			
	public class WebWrapperActivity : Activity
	{
		public const string WebAddressKey = "WebAddress";

		private WebWrapperFragment webFragment;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			this.RequestWindowFeature(WindowFeatures.NoTitle);
			this.SetContentView(Resource.Layout.WebWrapperActivity);

			this.webFragment = this.FragmentManager.FindFragmentById<WebWrapperFragment>(Resource.Id.webwrapperfragment);
			if (string.IsNullOrEmpty(this.webFragment.Url))
			{
				this.webFragment.Url = this.Intent.GetStringExtra(WebAddressKey);
			}
		}

		public override void OnBackPressed()
		{
			if (this.webFragment.CanGoBack)
			{
					this.webFragment.GoBack();
			}
			else
			{
				base.OnBackPressed();
			}
		}
	}
}