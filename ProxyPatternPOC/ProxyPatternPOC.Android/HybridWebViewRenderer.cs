using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using ProxyPatternPOC.Droid;
using ProxyPatternPOC;
using Xamarin.Forms.Platform.Android;
using static ProxyPatternPOC.HybridWebView;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace ProxyPatternPOC.Droid
{
   public  class HybridWebViewRenderer : ViewRenderer<HybridWebView, Android.Webkit.WebView>, PrevailingEvent
    {
        const string JavaScriptFunction = "function invokeCSharpAction(identifier, data){jsBridge.invokeAction(identifier, data);}";

        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var webView = new Android.Webkit.WebView(Forms.Context);
                webView.Settings.JavaScriptEnabled = true;
                SetNativeControl(webView);
            }
            if (e.OldElement != null)
            {
                Control.RemoveJavascriptInterface("jsBridge");
                var hybridWebView = e.OldElement as HybridWebView;
                hybridWebView.Cleanup();
            }
            if (e.NewElement != null)
            {
                Control.AddJavascriptInterface(new JSBridge(this), "jsBridge");
                Control.LoadUrl(string.Format("file:///android_asset/Content/{0}", Element.Uri));
                InjectJS(JavaScriptFunction);

                HybridWebView customView = e.NewElement as HybridWebView;
                customView.setPrevailingEvent(this);
            }
        }

        void InjectJS(string script)
        {
            if (Control != null)
            {
                Control.LoadUrl(string.Format("javascript: {0}", script));
            }
        }

        void CallJavascript(string counter)
        {
            if (Control != null)
            {
                Control.LoadUrl(string.Format("javascript: printLog('{0}');", counter));
            }
        }

        /*void waitAsync()
        {
            Task.Run(async () =>
            {
                await Task.Delay(8000);
                CallJavascript();
            });
        }*/

        public void prevail(string counter)
        {
            CallJavascript(counter);
        }
    }
}
