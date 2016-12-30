using Sabio.Web.Domain;
using Sabio.Web.Models.Requests.Followers;
using Sabio.Web.Models.Requests.Pagination;
using Sabio.Web.Models.Responses;
using Sabio.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sabio.Web.Controllers
{
    [RoutePrefix("api/following")]
    public class FollowingAPIController : ApiController
    {
        [Route("GetById"), HttpGet]
        [Authorize]

        public HttpResponseMessage GetAllFollowersByFollowingId([FromUri] followersUserIdRequest model)

        {
            if (model == null)
            {
                model = new followersUserIdRequest();
            }


            string userId = UserService.GetCurrentUserId();

            model.FollowingUserId = userId;
            int TotalItemCount = 0;




            List<FollowersPost> followers = FollowingServices.GetAllFollowersByFollowingId(model, ref TotalItemCount);

            PagedItemsResponse<FollowersPost> response = new PagedItemsResponse<FollowersPost>();
            response.CurrentPage = model.CurrentPage;
            response.ItemsPerPage = model.ItemsPerPage;
            response.TotalItemCount = TotalItemCount;
            response.Items = followers;

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }



        [Route("GetFriends"), HttpGet]
        [Authorize]
        public HttpResponseMessage GetAllFriends([FromUri] followersUserIdRequest model)
        {
            if (model == null)
            {
                model = new followersUserIdRequest();
            }

            string userId = UserService.GetCurrentUserId();

            model.FollowingUserId = userId;


            int TotalItemCount = 0;

            List<FollowersPost> friends = FollowingServices.GetAllFriends(model, ref TotalItemCount);

            PagedItemsResponse<FollowersPost> response = new PagedItemsResponse<FollowersPost>();
            response.CurrentPage = model.CurrentPage;
            response.ItemsPerPage = model.ItemsPerPage;
            response.TotalItemCount = TotalItemCount;
            response.Items = friends;

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }



        [Route("{id}"), HttpDelete]
        public HttpResponseMessage DeletebyID(string Id)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            FollowingServices.DeleteById(Id);

            ItemResponse<bool> response = new ItemResponse<bool>();
            response.Item = true;


            return Request.CreateResponse(HttpStatusCode.OK, response.Item);
        }

    }
}
