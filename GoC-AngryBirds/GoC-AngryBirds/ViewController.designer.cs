// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace GoCAngryBirds
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSTextField pathField { get; set; }

		[Action ("pathButton:")]
		partial void pathButton (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (pathField != null) {
				pathField.Dispose ();
				pathField = null;
			}
		}
	}
}
