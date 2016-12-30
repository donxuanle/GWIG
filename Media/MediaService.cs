using Sabio.Data;
using Sabio.Web.Domain.MyMedia;
using Sabio.Web.Enums;
using Sabio.Web.Models.Requests.MyMedia;
using Sabio.Web.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sabio.Web.Services.MyMedia_Services
{
    public class MediaService : BaseService,IMediaService

    {

        public  int InsertTest(MediaAddRequest model)
        {

            int uid = 0;
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.MyMedia_Insert"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@UserId", model.UserId);
                   paramCollection.AddWithValue("@MediaType", (int)model.MediaType);
                   paramCollection.AddWithValue("@DataType", model.DataType);
                   paramCollection.AddWithValue("@Title", model.Title);
                   paramCollection.AddWithValue("@Description", model.Description);
                   paramCollection.AddWithValue("@Url", model.Url);
                   paramCollection.AddWithValue("@ExternalMediaId", model.ExternalMediaId);
                   paramCollection.AddWithValue("@ThumbnailUrl", model.ThumbnailUrl);

                   SqlParameter p = new SqlParameter("@Id", System.Data.SqlDbType.Int);
                   p.Direction = System.Data.ParameterDirection.Output;

                   paramCollection.Add(p);

               }, returnParameters: delegate (SqlParameterCollection param)
               {
                   int.TryParse(param["@Id"].Value.ToString(), out uid);
               }
               );
            return uid;


        }


        public  void Update(MediaUpdateRequest model, int id)
        {


            DataProvider.ExecuteNonQuery(GetConnection, "dbo.MyMedia_UpdateById"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@UserId", model.UserId);
                   paramCollection.AddWithValue("@MediaType", model.MediaType);
                   paramCollection.AddWithValue("@DataType", model.DataType);
                   paramCollection.AddWithValue("@Title", model.Title);
                   paramCollection.AddWithValue("@Description", model.Description);
                   paramCollection.AddWithValue("@Url", model.Url);
                   paramCollection.AddWithValue("@Id", id);

               });
        }


        public  Media SelectById(int MediaId)
        {
            Media p = null;
            DataProvider.ExecuteCmd(GetConnection, "dbo.MyMedia_SelectById"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Id", MediaId);



               }, map: delegate (IDataReader reader, short set)
               {
                   p = new Media();
                   int startingindex = 0;
                   p.Id = reader.GetSafeInt32(startingindex++);
                   p.Created = reader.GetSafeDateTime(startingindex++);
                   p.UserId = reader.GetSafeString(startingindex++);
                   p.MediaType = reader.GetSafeEnum<MediaType>(startingindex++);
                   p.DataType = reader.GetSafeString(startingindex++);
                   p.Title = reader.GetSafeString(startingindex++);
                   p.Description = reader.GetSafeString(startingindex++);
                   p.Url = reader.GetSafeString(startingindex++);



               }
               );
            return p;

        }


        public  List<Media> GetbyUserId(MediaUserIdRequest model)
        {
            List<Media> mediaList = null;
            DataProvider.ExecuteCmd(GetConnection, "dbo.myMedia_SelectByUserId"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@UserId", model.UserId);
                },
                map: delegate (IDataReader reader , short set)
                {
                    Media p = new Media();
                    int startingindex = 0; //startingOrdinal
                    p.Id = reader.GetSafeInt32(startingindex++);
                    p.Created = reader.GetSafeDateTime(startingindex++);
                   
                    p.MediaType = reader.GetSafeEnum<MediaType>(startingindex++);
                    p.DataType = reader.GetSafeString(startingindex++);
                    p.Title = reader.GetSafeString(startingindex++);
                    p.Description = reader.GetSafeString(startingindex++);
                    p.Url = reader.GetSafeString(startingindex++);

                    if(mediaList == null)
                    {
                        mediaList = new List<Media>();
                    }
                    mediaList.Add(p);
                }
                );
            return mediaList;
        }



        public  List<Media> GetAllPost()
        {
            List<Media> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.MyMedia_SelectAll"
               , inputParamMapper: null
               , map: delegate (IDataReader reader, short set)
               {
                   Media p = new Media();
                   int startingindex = 0; //startingOrdinal
                   p.Id = reader.GetSafeInt32(startingindex++);
                   p.Created = reader.GetSafeDateTime(startingindex++);
                   p.UserId = reader.GetSafeString(startingindex++);
                   p.MediaType = reader.GetSafeEnum<MediaType>(startingindex++);
                   p.DataType = reader.GetSafeString(startingindex++);
                   p.Title = reader.GetSafeString(startingindex++);
                   p.Description = reader.GetSafeString(startingindex++);
                   p.Url = reader.GetSafeString(startingindex++);


                   if (list == null)
                   {
                       list = new List<Media>();
                   }

                   list.Add(p);
               }
               );


            return list;
        }


        public  void DeleteById(int id)
        {


            DataProvider.ExecuteNonQuery(GetConnection, "dbo.MyMedia_DeleteById"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {

                   paramCollection.AddWithValue("@Id", id);


               });

        }



    }
}