﻿namespace Freelancer.MVC.Controllers
{
    using Freelancer.Data.UnitOfWork;
    using Freelancer.Models;
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Microsoft.AspNet.Identity;

    public abstract class BaseController : Controller
    {
        private IFreelancerData data;

        private User userProfile;

        protected BaseController(IFreelancerData data)
        {
            this.Data = data;
        }

        protected BaseController(IFreelancerData data, User userProfile)
            : this(data)
        {
            this.UserProfile = userProfile;
        }

        protected IFreelancerData Data
        {
            get { return this.data; }
            private set { this.data = value; }
        }

        protected User UserProfile
        {
            get { return this.userProfile; }
            private set { this.userProfile = value; }
        }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var username = requestContext.HttpContext.User.Identity.Name;
                
                var user = this.Data.Users
                    .All()
                    .FirstOrDefault(x => x.UserName == username);

                this.UserProfile = user;
            }

            return base.BeginExecute(requestContext, callback, state);
        }

        private string CurrentUserId()
        {
            var userId = this.User.Identity.GetUserId();
            return userId;
        }

        public bool IsAdmin()
        {
            var isAdmin = (CurrentUserId() != null && this.User.IsInRole("Administrator"));

            return isAdmin;
        }

        public bool IsModerator()
        {
            var isModerator = (CurrentUserId() != null && this.User.IsInRole("Moderator"));

            return isModerator;
        }
    }
}