﻿
using DotNetNuke.Web.Api;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections;

namespace OPSI.UManage.WebApi
{

    public class UsersController : DnnApiController
    {


        [DnnModuleAuthorize(AccessLevel = DotNetNuke.Security.SecurityAccessLevel.View)]
        [HttpGet]
        [ValidateAntiForgeryToken]
        [SupportedModules("UManage")]
        public HttpResponseMessage UsersList(string key, string roles, bool deleted, bool unauth, int ResultsPerPage, int CurrentPage, string orderby, string orderclause)
        {

            //response result
            Responses.Users_List response = new Responses.Users_List();

            try
            {

                //current portal Id
                int portalId = this.ActiveModule.PortalID;

                //Getting portal roles for the current portal
                List<Business.Entities.UserListItem> usersList = Business.Controllers.DNN_Users.ListUsers(portalId, ResultsPerPage, CurrentPage, key, roles, deleted, unauth, orderby, orderclause);

                //Building response including only necessary info
                foreach (Business.Entities.UserListItem userInfo in usersList)
                {

                    response.Users.Add(
                            new Responses.Users_List.Users_List_UserInfo()
                            {
                                UserID = userInfo.UserID,
                                DisplayName = userInfo.DisplayName,
                                Email = userInfo.Email,
                                Telephone = userInfo.Telephone,
                                FirstName = userInfo.FirstName,
                                LastName = userInfo.LastName,
                                Username = userInfo.Username,
                                Password = userInfo.Password,
                                CreatedOnDate = userInfo.CreatedOnDate,
                                LastLoginDate = userInfo.LastLoginDate,
                                IsDeleted = userInfo.IsDeleted,
                                Authorised = userInfo.Authorised,
                                Profile_Picture_FileID = userInfo.Profile_Picture_FileID,
                                TotalRows = userInfo.TotalRows,
                                Pages = userInfo.Pages
                            });


                }

                //positive response
                response.Success = true;

            }
            catch (Exception ex)
            {

                //Logging Error in DNN log
                DotNetNuke.Services.Log.EventLog.EventLogController objEventLog = new DotNetNuke.Services.Log.EventLog.EventLogController();
                objEventLog.AddLog("UManage Error", ex.Message + " - " + ex.StackTrace, PortalSettings, -1, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT);

                //Build response
                response.ErrorMessage = "error";
                response.Success = false;

            }

            //returning response
            return Request.CreateResponse(HttpStatusCode.OK, Newtonsoft.Json.JsonConvert.SerializeObject(response));

        }


        [DnnModuleAuthorize(AccessLevel = DotNetNuke.Security.SecurityAccessLevel.View)]
        [HttpGet]
        [ValidateAntiForgeryToken]
        [SupportedModules("UManage")]
        public HttpResponseMessage UsersGet(int userid)
        {

            //response result
            WebApi.Responses.Users_Get response = new WebApi.Responses.Users_Get();

            try
            {

                //current portal Id
                int portalId = this.ActiveModule.PortalID;

                //Getting the user, and adding it to the response object. 
                //In this case, we need the whole DNN UserInfo class instance.
                response.user = Business.Controllers.DNN_Users.GetUser(userid, portalId);

                //positive response
                response.Success = true;

            }
            catch (Exception ex)
            {

                //Logging Error in DNN log
                DotNetNuke.Services.Log.EventLog.EventLogController objEventLog = new DotNetNuke.Services.Log.EventLog.EventLogController();
                objEventLog.AddLog("UManage Error", ex.Message + " - " + ex.StackTrace, PortalSettings, -1, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT);

                response.ErrorMessage = "error";
                response.Success = false;

            }

            //returning response
            return Request.CreateResponse(HttpStatusCode.OK, Newtonsoft.Json.JsonConvert.SerializeObject(response));

        }


        [DnnModuleAuthorize(AccessLevel = DotNetNuke.Security.SecurityAccessLevel.View)]
        [HttpGet]
        [ValidateAntiForgeryToken]
        [SupportedModules("UManage")]
        public HttpResponseMessage UsersDeleteUnauthorized()
        {

            //response result
            Responses.Base response = new Responses.Base();

            try
            {

                //current portal Id
                int portalId = this.ActiveModule.PortalID;

                //removing unauthorized users
                Business.Controllers.DNN_Users.DeleteUnauthorizedUsers(portalId);

                //positive response
                response.Success = true;

            }
            catch (Exception ex)
            {

                //Logging Error in DNN log
                DotNetNuke.Services.Log.EventLog.EventLogController objEventLog = new DotNetNuke.Services.Log.EventLog.EventLogController();
                objEventLog.AddLog("UManage Error", ex.Message + " - " + ex.StackTrace, PortalSettings, -1, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT);

                response.ErrorMessage = "error";
                response.Success = false;

            }

            //returning response
            return Request.CreateResponse(HttpStatusCode.OK, Newtonsoft.Json.JsonConvert.SerializeObject(response));


        }


        [DnnModuleAuthorize(AccessLevel = DotNetNuke.Security.SecurityAccessLevel.View)]
        [HttpGet]
        [ValidateAntiForgeryToken]
        [SupportedModules("UManage")]
        public HttpResponseMessage UsersRemoveDeleted()
        {

            //response result
            Responses.Base response = new Responses.Base();

            try
            {

                //current portal Id
                int portalId = this.ActiveModule.PortalID;

                //Deleting users
                Business.Controllers.DNN_Users.RemoveDeletedUsers(portalId);

                //positive response
                response.Success = true;

            }
            catch (Exception ex)
            {

                //Logging Error in DNN log
                DotNetNuke.Services.Log.EventLog.EventLogController objEventLog = new DotNetNuke.Services.Log.EventLog.EventLogController();
                objEventLog.AddLog("UManage Error", ex.Message + " - " + ex.StackTrace, PortalSettings, -1, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT);


                response.ErrorMessage = "error";
                response.Success = false;

            }

            //returning response
            return Request.CreateResponse(HttpStatusCode.OK, Newtonsoft.Json.JsonConvert.SerializeObject(response));

        }



        [DnnModuleAuthorize(AccessLevel = DotNetNuke.Security.SecurityAccessLevel.View)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SupportedModules("UManage")]
        public HttpResponseMessage UsersUpdate([FromBody]string _json_content)
        {


            //response result
            Responses.Users_Update response = new Responses.Users_Update();

            try
            {

                //current portal Id
                int portalId = this.ActiveModule.PortalID;

                //reading posted object
                WebApi.Posts.Users_Update receivedUser = Newtonsoft.Json.JsonConvert.DeserializeObject<WebApi.Posts.Users_Update>(_json_content, new Newtonsoft.Json.Converters.IsoDateTimeConverter { DateTimeFormat = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern });

                //transcoding from post to entity
                Business.Entities.User userEntity = new Business.Entities.User();
                userEntity.DisplayName = receivedUser.DisplayName;
                userEntity.Email = receivedUser.Email;
                userEntity.FirstName = receivedUser.FirstName;
                userEntity.LastName = receivedUser.LastName;
                userEntity.Password = receivedUser.Password;
                userEntity.Telephone = receivedUser.Telephone;
                userEntity.UserID = receivedUser.UserID;
                userEntity.Username = receivedUser.Username;

                //Deleting users
                Business.Responses.UpdateUser businessResponse = Business.Controllers.DNN_Users.UpdateUser(userEntity, portalId);

                //positive response
                response.UserID = businessResponse.userId;
                response.responseMessage = businessResponse.responseMessage;
                response.Success = true;

            }
            catch (Exception ex)
            {

                //Logging Error in DNN log
                DotNetNuke.Services.Log.EventLog.EventLogController objEventLog = new DotNetNuke.Services.Log.EventLog.EventLogController();
                objEventLog.AddLog("UManage Error", ex.Message + " - " + ex.StackTrace, PortalSettings, -1, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT);


                response.ErrorMessage = "error";
                response.Success = false;

            }

            //returning response
            return Request.CreateResponse(HttpStatusCode.OK, Newtonsoft.Json.JsonConvert.SerializeObject(response));

        }



        [DnnModuleAuthorize(AccessLevel = DotNetNuke.Security.SecurityAccessLevel.View)]
        [HttpGet]
        [ValidateAntiForgeryToken]
        [SupportedModules("UManage")]
        public HttpResponseMessage UsersAuthorize(int userid, string mode)
        {

            //response result
            Responses.Base response = new Responses.Base();

            try
            {

                //current portal Id
                int portalId = this.ActiveModule.PortalID;

                //Changing the attribute for the user
                Business.Controllers.DNN_Users.ChangeAuthorizedStatus(userid, portalId, mode);

                //positive response
                response.Success = true;

            }
            catch (Exception ex)
            {

                //Logging Error in DNN log
                DotNetNuke.Services.Log.EventLog.EventLogController objEventLog = new DotNetNuke.Services.Log.EventLog.EventLogController();
                objEventLog.AddLog("UManage Error", ex.Message + " - " + ex.StackTrace, PortalSettings, -1, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT);

                response.ErrorMessage = "error";
                response.Success = false;

            }

            //returning response
            return Request.CreateResponse(HttpStatusCode.OK, Newtonsoft.Json.JsonConvert.SerializeObject(response));

        }



        [DnnModuleAuthorize(AccessLevel = DotNetNuke.Security.SecurityAccessLevel.View)]
        [HttpGet]
        [ValidateAntiForgeryToken]
        [SupportedModules("UManage")]
        public HttpResponseMessage UsersDelete(int userid, string mode)
        {

            //response result
            Responses.Base response = new Responses.Base();

            try
            {

                //current portal Id
                int portalId = this.ActiveModule.PortalID;

                //Changing the attribute for the user
                Business.Controllers.DNN_Users.ChangeDeleteStatus(userid, portalId, mode);

                //positive response
                response.Success = true;

            }
            catch (Exception ex)
            {

                //Logging Error in DNN log
                DotNetNuke.Services.Log.EventLog.EventLogController objEventLog = new DotNetNuke.Services.Log.EventLog.EventLogController();
                objEventLog.AddLog("UManage Error", ex.Message + " - " + ex.StackTrace, PortalSettings, -1, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT);

                response.ErrorMessage = "error";
                response.Success = false;

            }

            //returning response
            return Request.CreateResponse(HttpStatusCode.OK, Newtonsoft.Json.JsonConvert.SerializeObject(response));

        }


        [DnnModuleAuthorize(AccessLevel = DotNetNuke.Security.SecurityAccessLevel.View)]
        [HttpGet]
        [ValidateAntiForgeryToken]
        [SupportedModules("UManage")]
        public HttpResponseMessage UsersSendPasswordLink(int userid)
        {

            //response result
            Responses.Base response = new Responses.Base();

            try
            {

                //current portal Id
                int portalId = this.ActiveModule.PortalID;

                //Changing the attribute for the user
                Business.Controllers.DNN_Users.SendPasswordResetLink(portalId, userid, this.PortalSettings);

                //positive response
                response.Success = true;

            }
            catch (Exception ex)
            {

                //Logging Error in DNN log
                DotNetNuke.Services.Log.EventLog.EventLogController objEventLog = new DotNetNuke.Services.Log.EventLog.EventLogController();
                objEventLog.AddLog("UManage Error", ex.Message + " - " + ex.StackTrace, PortalSettings, -1, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT);

                response.ErrorMessage = "error";
                response.Success = false;

            }

            //returning response
            return Request.CreateResponse(HttpStatusCode.OK, Newtonsoft.Json.JsonConvert.SerializeObject(response));

        }

    }

}
