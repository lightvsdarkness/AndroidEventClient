using System.IO;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using SQLite;
//using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using Android.Content;
using Android.Runtime;
using AEC.Fragments;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.App;
using Android.Views;
using Android.OS;
using Android.Widget;
namespace AEC
{
    public class SearchViewExpandListener : Java.Lang.Object, MenuItemCompat.IOnActionExpandListener
    {
        private readonly IFilterable standardEventsAdapter;

        public SearchViewExpandListener(IFilterable searchAdapter)
        {
            standardEventsAdapter = searchAdapter;
        }

        public bool OnMenuItemActionCollapse(IMenuItem item)
        {
            standardEventsAdapter.Filter.InvokeFilter("");
            return true;
        }

        public bool OnMenuItemActionExpand(IMenuItem item)
        {
            return true;
        }
    }

    //OBJECT EXTENSIONS
        public class JavaHolder : Java.Lang.Object
        {
            public readonly object Instance;

            public JavaHolder(object instance)
            {
                Instance = instance;
            }
        }

        public static class ObjectExtensions
        {
            public static TObject ToNetObject<TObject>(this Java.Lang.Object value)
            {
                if (value == null)
                    return default(TObject);

                if (!(value is JavaHolder))
                    throw new InvalidOperationException("Unable to convert to .NET object. Only Java.Lang.Object created with .ToJavaObject() can be converted.");

                TObject returnVal;
                try { returnVal = (TObject)((JavaHolder)value).Instance; }
                finally { value.Dispose(); }
                return returnVal;
            }

            public static Java.Lang.Object ToJavaObject<TObject>(this TObject value)
            {
                if (Equals(value, default(TObject)) && !typeof(TObject).IsValueType)
                    return null;

                var holder = new JavaHolder(value);

                return holder;
            }
        }
}