using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProxyPatternPOC
{
    public class HybridWebView : View
    {
        //private int counter = 0;
        private IDictionary<string, Action<string>> actions;
        private PrevailingEvent _prevailingEvent;

        public static readonly BindableProperty UriProperty = BindableProperty.Create(
            propertyName: "Uri",
            returnType: typeof(string),
            declaringType: typeof(HybridWebView),
            defaultValue: default(string));

        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }

        public void RegisterAction(string identifier, Action<string> callback)
        {
            if (actions == null) actions = new Dictionary<string, Action<string>>();
            actions.Add(identifier, callback);
        }

        public void Cleanup()
        {
            actions.Clear();
            actions = null;
        }

        public void InvokeAction(string identifier, string argument)
        {
            if (actions == null || identifier == null || !actions.ContainsKey(identifier))
            {
                return;
            }
            actions[identifier].Invoke(argument);
        }

        public void setPrevailingEvent(PrevailingEvent prevailingEvent)
        {
            this._prevailingEvent = prevailingEvent;
        }

        public void CallJavascript(string data)
        {
            _prevailingEvent?.prevail(data);
        }

        public interface PrevailingEvent
        {
            void prevail(string data);
        }
    }
}

