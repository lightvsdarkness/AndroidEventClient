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

namespace AEC
{
    [Activity(Label = "Забыли пароль?", MainLauncher = false, Theme = "@android:style/Theme.Holo.Light.Dialog")]
    [MetaData("android.support.PARENT_ACTIVITY", Value = "aec.AccLogInAct")]
	public class AccCantAccessAct : Activity
    {
        // int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.AccCantAccess);

            // Get our button from the layout resource,
            // and attach an event to it
			Button AnswerSecretQuestionButton = FindViewById<Button>(Resource.Id.AnswerSecretQuestion);
			AnswerSecretQuestionButton.Click += delegate
            {
                AnswerSecretQuestionButton.Text = string.Format("Ваш пароль: ");
            };
        }
    }
}

