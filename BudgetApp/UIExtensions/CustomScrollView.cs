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
using Android.Util;

namespace BudgetApp
{

    public class CustomScrollView : ScrollView
    {

        public CustomScrollView(Context context) : base(context) { }

        public CustomScrollView(Context context, IAttributeSet attrs) : base(context, attrs) { }

        public CustomScrollView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) { }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            var action = ev.Action;
            switch (action)
            {
                case MotionEventActions.Down:
                    //Log.i("VerticalScrollview", "onInterceptTouchEvent: DOWN super false" );
                    base.OnTouchEvent(ev);
                    break;

                case MotionEventActions.Move:
                    return false; // redirect MotionEvents to ourself

                case MotionEventActions.Cancel:
                    //Log.i("VerticalScrollview", "onInterceptTouchEvent: CANCEL super false" );
                    base.OnTouchEvent(ev);
                    break;

                case MotionEventActions.Up:
                    //Log.i("VerticalScrollview", "onInterceptTouchEvent: UP super false" );
                    return false;

                default: break;
            }

            return false;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            base.OnTouchEvent(e);
            return true;
        }
    }
}