using Android.Content;
using Com.Applozic.Mobicomkit.Uiwidgets.Async;
using Com.Applozic.Mobicomkit.Api.Account.Register;
using Com.Applozic.Mobicomkit.Api.Account.User;
using System;

using System.Collections;
using System.Collections.Generic;
using Android.Util;

namespace ApplozicChat
{

    class ApplozicContactService
    {
        Context context;
        public ApplozicContactService(Context context)
        {
            this.context = context;

        }
        public void AddMember(Context context , String contactGroupId ,String contactGroupType , String contactGroupMember , AddMemberListner addMemberListner)
        {
            this.context = context;
            System.Console.WriteLine(contactGroupId +" ::",contactGroupType + "::" , contactGroupMember);
            Java.Lang.Void[] args = null;
            List<String> groupMemberList = new List<String>();
            groupMemberList.Add(contactGroupMember);
            new ApplozicAddMemberToContactGroupTask(context, contactGroupId, contactGroupType, groupMemberList, addMemberListner).Execute(args);
        
        }
    }


    public class AddMemberListner : Java.Lang.Object, ApplozicAddMemberToContactGroupTask.IGroupMemberListener
    {


        public delegate void OnAddMemberSucess(bool response, Context context);
        public delegate void OnAddMemberFailed(bool response, Java.Lang.Exception e , Context context);
        public event OnAddMemberSucess OnAddMemberSucessHandler;
        public event OnAddMemberFailed OnAddMemberFailedHandler;

        public void OnSuccess(bool res, Context context)
        {
            //Send call back to caller
            

            if (OnAddMemberSucessHandler != null)
            {
                OnAddMemberSucessHandler(res, context);
            }

            System.Console.WriteLine("Successfully Added"+ res);

        }

        public void OnFailure(bool  res, Java.Lang.Exception e ,Context context)
        {

            //Send call back to caller
            if (OnAddMemberFailedHandler != null)
            {
               
                OnAddMemberFailedHandler(res, e , context);
            }
        }
    }
}