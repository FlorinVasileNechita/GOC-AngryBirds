using System.Collections;
using System;
using AppKit;
using Foundation;
using Mono;

namespace GoCAngryBirds
{
	public partial class ViewController : NSViewController
	{
		

		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			pathField.StringValue = "/Users";

			var dlg = NSOpenPanel.OpenPanel;
			dlg.CanChooseFiles = true;
			dlg.CanChooseDirectories = false;

			if (dlg.RunModal () == 1) {
				// Nab the first file
				var url = dlg.Urls [0];

				if (url != null) {
					var path = url.Path;

					// Create a new window to hold the text
					var newWindowController = new MainWindowController ();
					newWindowController.Window.MakeKeyAndOrderFront (this);

					// Load the text into the window
					var window = newWindowController.Window as MainWindow;
					window.Text = File.ReadAllText (path);
					window.SetTitleWithRepresentedFilename (Path.GetFileName (path));
					window.RepresentedUrl = url;

				}
			}

		}

		public override NSObject RepresentedObject {
			get {
				return base.RepresentedObject;
			}
			set {
				base.RepresentedObject = value;
				// Update the view, if already loaded.
			}
		}
	}
}
