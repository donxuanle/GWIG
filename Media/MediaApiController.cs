using Sabio.Web.Domain.MyMedia;
using Sabio.Web.Models.Requests.MyMedia;
using Sabio.Web.Models.Responses;
using Sabio.Web.Services.MyMedia_Service;
using Sabio.Web.Services.MyMedia_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Sabio.Web.Enums;
using Sabio.Web.Models.Requests.Youtube;
using Sabio.Web.Services;
using Microsoft.Practices.Unity;
using Sabio.Web.Services.Interface;

namespace Sabio.Web.Controllers.Api.MyMedia_Api
{
    [RoutePrefix("api/media")]
    public class MediaApiController : BaseApiController
    {
        [Dependency]
        public IYoutubeService _youtubeService { get; set; }

        [Dependency]
        public IMediaService _mediaService { get; set; }


        [Route(), HttpPost]
        [Authorize]
        public HttpResponseMessage Add()
        {
            if (!ModelState.IsValid)
            {
                //return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            string docfile = "";
            int newmediaId = 0;
           
            HttpRequest httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                string fileName = "";
                FileService newFileUpload = new FileService();
                foreach (string file in httpRequest.Files)
                {
                    HttpPostedFile postedFile = httpRequest.Files[file];
                    string filePath = HttpContext.Current.Server.MapPath("~/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    docfile = filePath;



                    //(string filePath, string newFileName,string s3Bucket,  bool deleteLocalFileOnSuccess)
                    fileName = Guid.NewGuid() + postedFile.FileName;

                    newFileUpload.UploadFile(docfile, fileName, "sabio-training/C23", true);
                }


                MediaAddRequest mediaModel = new MediaAddRequest();


                string mimeType = System.Web.MimeMapping.GetMimeMapping(fileName);

                string UserId = UserService.GetCurrentUserId();

                
                mediaModel.FileName = fileName;
                mediaModel.Title = httpRequest.Form["Title"];  // STACKOVER FLOW SAVES THE DAY! 
                mediaModel.Description = httpRequest.Form["Description"];
                mediaModel.UserId = httpRequest.Form["UserId"];
                //  mediaModel.MediaType = (MediaType)Enum.Parse(typeof(MediaType), httpRequest.Form["MediaType"]);
                mediaModel.MediaType = MediaType.Upload;
                mediaModel.UserId = UserId;   
                mediaModel.MediaType = (MediaType)Enum.Parse(typeof(MediaType), httpRequest.Form["MediaType"]);
                mediaModel.DataType = mimeType;
                mediaModel.Url = newFileUpload.getS3Url() +"/" + fileName;
       
                newmediaId = _mediaService.InsertTest(mediaModel);
         


            };

            ItemResponse<int> response = new ItemResponse<int>();
            response.Item =  newmediaId;

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [Authorize]
        [Route("youtube")]
        [HttpPost]
        public ItemResponse<int> YoutubeVideo(YoutubeMediaAddRequest model)
        {
            var video = _youtubeService.GetVideo(model.VideoId);

            var media = new MediaAddRequest
            {
                Title = video.Snippet.Title,
                Description = video.Snippet.Description,
                MediaType = MediaType.YouTube,
                Url = video.Snippet.Thumbnails.High.Url,
                UserId = UserService.GetCurrentUserId(),
                ExternalMediaId = video.Id
            };

            int id = _mediaService.InsertTest(media);

            return Item(id);
        }

        [Authorize]
        [Route("video/placesUpload")]
        public HttpResponseMessage videoInsert(MediaAddRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            string UserId = UserService.GetCurrentUserId();
            model.UserId = UserId;
           int mediaId = _mediaService.InsertTest(model);

            ItemResponse<int> response = new ItemResponse<int>();

            response.Item = mediaId;

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [Route("{id:int}"), HttpPut]
        public HttpResponseMessage Update(MediaUpdateRequest model, int Id)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }



            _mediaService.Update(model, Id);

            SuccessResponse response = new SuccessResponse();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [Route("{id:int}"), HttpGet]
        public HttpResponseMessage GetById(int id)

        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            Media x = _mediaService.SelectById(id);



            return Request.CreateResponse(HttpStatusCode.OK, x);
        }

        [Route("UserId"), HttpGet]
        [Authorize]
        public HttpResponseMessage GetbyUserId()
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            MediaUserIdRequest model = new MediaUserIdRequest();
            model.UserId = UserService.GetCurrentUserId();


            List<Media> MediaStuff = _mediaService.GetbyUserId(model);

            ItemsResponse<Media> response = new ItemsResponse<Media>();

            response.Items = MediaStuff;

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [Route(), HttpGet]
        public HttpResponseMessage GetAll()

        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            List<Media> Ids = _mediaService.GetAllPost();
            
            ItemsResponse<Media> response= new ItemsResponse<Media>();

            response.Items = Ids;


            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [Route("{id:int}"), HttpDelete]
        public HttpResponseMessage DeletebyID(int Id)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            _mediaService.DeleteById(Id);

            ItemResponse<bool> response = new ItemResponse<bool>();
            response.Item = true;


            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
