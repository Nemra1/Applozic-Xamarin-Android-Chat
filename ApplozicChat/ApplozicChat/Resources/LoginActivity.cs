
using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Applozic;
using Com.Applozic.Mobicomkit.Api.Account.Register;
using Com.Applozic.Mobicommons.People.Channel;
using Android.Util;

namespace ApplozicChat
{
	[Activity(Label = "Login" , MainLauncher = true, Icon = "@mipmap/icon")]
	public class LoginActivity : Activity
	{

		private UserLoginListener loginListener;
        private AddMemberListner addMemberListner;
        String contactGroupId = "GROUPNAME";
        public string CONTACTS_GROUP_ID = "CONTACTS_GROUP_ID";
        ApplozicContactService contactService;
        EditText userName;

        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

            addMemberListner = new AddMemberListner();
            addMemberListner.OnAddMemberSucessHandler += OnAddMemberSucessHandler;
            addMemberListner.OnAddMemberFailedHandler += OnAddMemberFailedHandler;

            loginListener= new UserLoginListener();
			loginListener.OnRegistrationSucessHandler += OnRegistrationSucessHandler;
			loginListener.OnRegistrationFailedHandler += OnRegistrationFailedHandler;

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.login_activity_layout);

            // Get our button from the layout resource,
            // and attach an event to it
            userName = FindViewById<EditText>(Resource.Id.username_input);
			EditText password = FindViewById<EditText>(Resource.Id.password_input);
			Button signIn = FindViewById<Button>(Resource.Id.sign_in_btn);
			ApplozicChatManager chatManager = new ApplozicChatManager(this);


            signIn.Click += delegate
			{   
                chatManager.RegisterUser(userName.Text, userName.Text, password.Text, loginListener);
            };

			if (chatManager.ISUserLoggedIn())
			{
				System.Console.WriteLine("Already Registred ::");
                Log.Debug("LoginActivity", contactGroupId);
                Intent myIntent = new Intent(this, typeof(MainActivity));
                myIntent.PutExtra(CONTACTS_GROUP_ID, contactGroupId);
                this.StartActivity(myIntent);
                this.Finish();
			}


		}


		void OnRegistrationSucessHandler( RegistrationResponse res, Context context)
		{
			System.Console.WriteLine("Successfully got callback in LoginActivity :" + res.Message);
            contactService = new ApplozicContactService(this);
            contactService.AddMember(this, contactGroupId, (String)(Channel.GroupType.ContactGroup.Value), userName.Text, addMemberListner);
            Intent myIntent = new Intent(this, typeof(MainActivity));
            myIntent.PutExtra(CONTACTS_GROUP_ID, contactGroupId);
            this.StartActivity(myIntent);
            this.Finish();
        }

		void OnRegistrationFailedHandler(RegistrationResponse res, Java.Lang.Exception exception)
		{
			System.Console.WriteLine("Error while doing registrations:" + exception.Message);

			Toast.MakeText(ApplicationContext, "Login Failed : " + exception.Message , ToastLength.Long).Show();
		}


        void OnAddMemberSucessHandler(bool res, Context context)
        {
          
            System.Console.WriteLine("Successfully got callback for AddMember in LoginActivity :" + res);
        }

        void OnAddMemberFailedHandler(bool res, Java.Lang.Exception exception , Context context)
        {
           
            System.Console.WriteLine("Error while Adding Member:" + exception.Message);

            Toast.MakeText(ApplicationContext, "Adding Failed : " + exception.Message, ToastLength.Long).Show();
        }

    }

}
