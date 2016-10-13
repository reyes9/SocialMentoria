using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SocialMentorApp;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Json;
using System.Net;
using System.IO;
using Android.Content.PM;

namespace SocialMentorApp.Droid
{
	
	[Activity (Label = "MainActivity",ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : Activity
	{
		List<Mentorat> mentorats;
        //Button submitButton;
        //Button submitButton = FindViewById<Button>(Resource.Id.Submit);

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            

           /* submitButton.Click += (o, e) => {
                /*StartActivity(typeof(LoginActivity));
                this.Finish();
                Toast.MakeText(this, "Perfecto!", ToastLength.Short).Show();
            
            };*/

           // submitButton = FindViewById<Button>(Resource.Id.Submit);
          //  submitButton.SetOnClickListener(new onClickListener());
           
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
			// Get our button from the layout resource,
			// and attach an event to it
			RecyclerView recyclerView = FindViewById<RecyclerView> (Resource.Id.RecyclerViewMentorats);
			ActionBar.SetTitle (Resource.String.llista_mentorats);
			mentorats = new List<Mentorat> ();
			mentorats = MainApplication.AppDatabase.GetMentorats ().ToList();
			LinearLayoutManager llm = new LinearLayoutManager(this);
			recyclerView.SetLayoutManager (llm);
			MentoratItemAdapter adapter = new MentoratItemAdapter (mentorats,this);
			recyclerView.SetAdapter (adapter);
           
            Button button = FindViewById<Button>(Resource.Id.button);

            button.Click += (o, e) =>
            {
                StartActivity(typeof(LoginActivity));
                this.Finish();
               // Toast.MakeText(this, "Perfecto!", ToastLength.Short).Show();
            };


            /*button.Click += delegate
                {
                    /*StartActivity(typeof(LoginActivity));
                    this.Finish();
                    Toast.MakeText(this, "Perfecto!", ToastLength.Short).Show();

                };*/
            
            
            if (mentorats == null || mentorats.Count() == 0)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("No hay mentorados");
                alert.SetMessage("Ponte en contacto con la cordinadora");
                alert.SetNeutralButton("OK", (senderAlert, args) =>
                 {
                     Toast.MakeText(this, "Perfecto!", ToastLength.Short).Show();
                 });

                Dialog dialog = alert.Create();
                dialog.Show();
            }

              



            }





    }
}


