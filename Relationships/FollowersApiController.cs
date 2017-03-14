

namespace gwig.Web.Controllers
{
    [RoutePrefix("api/followers")]
    public class FollowersApiController : ApiController
    {
        [Dependency]
        public ISystemEventsService SystemEventsService { get; set; }
        [Dependency]
        public IFollowersService _followersService { get; set; }

        [Route, HttpPost]
        [Authorize]
        public HttpResponseMessage AddFollowers(FollowersAddRequest model)
        {
            HttpRequest httpRequest = HttpContext.Current.Request;
            if (!ModelState.IsValid)
            {


                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }


            // set FullerUserId to Current User Id

            model.FollowerUserId = UserService.GetCurrentUserId();
   
            int Id = _followersService.InsertTest(model);

            ItemResponse<int> response = new ItemResponse<int>();
            response.Item = Id;

            SystemEventsService.AddSystemEvent(new AddSystemEventModel
            {
                ActorUserId = model.FollowerUserId,
                ActorType = ActorType.User,
                EventType = SystemEventType.UserFollowUser,
                TargetUserId = model.FollowingUserId,
                TargetType = TargetType.User
            });

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [Route("{id:int}"), HttpPut]
        public HttpResponseMessage Update(FollowersUpdateRequest model, int Id)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }



            _followersService.Update(model, Id);

            SuccessResponse response = new SuccessResponse();


            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [Route("GetById"), HttpGet]
        [Authorize]
        public HttpResponseMessage GetAllFollowingByFollowersId([FromUri] followersUserIdRequest model)

        {
            if (model == null)
            {
                model = new followersUserIdRequest();
            }

            string userId = UserService.GetCurrentUserId();

            

            model.FollowerUserId = userId;

            int TotalItemCount = 0;


            List<FollowersPost> following  = _followersService.GetAllFollowingByFollowersId(model, ref TotalItemCount);


            PagedItemsResponse<FollowersPost> response = new PagedItemsResponse<FollowersPost>();
            response.CurrentPage = model.CurrentPage;
            response.ItemsPerPage = model.ItemsPerPage;
            response.TotalItemCount = TotalItemCount;
            response.Items = following;

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [Route(), HttpGet]
        public HttpResponseMessage GetAll()

        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }


            List<FollowersPost> userId = _followersService.GetAllPost();



            return Request.CreateResponse(HttpStatusCode.OK, userId);
        }
        [Route("{id}"), HttpDelete]
        public HttpResponseMessage DeletebyID(string Id)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            _followersService.DeleteById(Id);

            ItemResponse<bool> response = new ItemResponse<bool>();
            response.Item = true;


            return Request.CreateResponse(HttpStatusCode.OK, response.Item);
        }

    }
}
