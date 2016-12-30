using Sabio.Data;
using Sabio.Web.Domain;
using Sabio.Web.Enums;
using Sabio.Web.Models.Requests.Followers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sabio.Web.Services
{
    public class FollowingServices : BaseService
    {
        public static List<FollowersPost> GetAllFollowersByFollowingId(followersUserIdRequest model, ref int TotalItemCount)
        {
            List<FollowersPost> followersList = null;
            int totalItemCount = 0;
            DataProvider.ExecuteCmd(GetConnection, "dbo.Followers_SelectByFollowingUserIdUserProfileIdmyMediaUserId"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                  
                   paramCollection.AddWithValue("@FollowingUserId", model.FollowingUserId);
                   paramCollection.AddWithValue("@CurrentPage", model.CurrentPage);
                   paramCollection.AddWithValue("@ItemsPerPage", model.ItemsPerPage);


               }, map: delegate (IDataReader reader, short set)
               {
                   FollowersPost p = new FollowersPost();
                   int startingindex = 0;

                   totalItemCount = reader.GetSafeInt32(startingindex++);
                   p.userName = reader.GetSafeString(startingindex++);
                   p.FollowingUserId = reader.GetSafeString(startingindex++);
                   p.userId = reader.GetSafeString(startingindex++);
                   p.id = reader.GetSafeInt32(startingindex++);
                   p.mediaId = reader.GetSafeInt32(startingindex++);
                   p.Url = reader.GetSafeString(startingindex++);
                   p.FollowerUserId = reader.GetSafeString(startingindex++);


                   if (followersList == null)
                   {
                       followersList = new List<FollowersPost>();

                   }

                   followersList.Add(p);

               }

               );
            TotalItemCount = totalItemCount;
            return followersList;

        }
        public static List<FollowersPost> GetAllFriends(followersUserIdRequest model, ref int TotalItemCount)
        {
            List<FollowersPost> followersList = null;
            int totalItemCount = 0;
            DataProvider.ExecuteCmd(GetConnection, "dbo.Followers_SelectFriends"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {

                   paramCollection.AddWithValue("@FollowingUserId", model.FollowingUserId);
                   paramCollection.AddWithValue("@CurrentPage", model.CurrentPage);
                   paramCollection.AddWithValue("@ItemsPerPage", model.ItemsPerPage);

               }, map: delegate (IDataReader reader, short set)
               {
                   FollowersPost p = new FollowersPost();
                   int startingindex = 0;

                   totalItemCount = reader.GetSafeInt32(startingindex++);
                   p.userName = reader.GetSafeString(startingindex++);
                   p.FollowingUserId = reader.GetSafeString(startingindex++);
                   p.userId = reader.GetSafeString(startingindex++);
                   p.id = reader.GetSafeInt32(startingindex++);
                   p.mediaId = reader.GetSafeInt32(startingindex++);
                   p.Url = reader.GetSafeString(startingindex++);
                   p.FollowerUserId = reader.GetSafeString(startingindex++);


                   if (followersList == null)
                   {
                       followersList = new List<FollowersPost>();

                   }

                   followersList.Add(p);

               }

               );
            TotalItemCount = totalItemCount;

            return followersList;

        }
        public static void DeleteById(string followedUserId)
        {

            string userId = UserService.GetCurrentUserId();
            if (userId.Length > 0)
            {
                DataProvider.ExecuteNonQuery(GetConnection, "dbo.Followers_DeleteById"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {

                   paramCollection.AddWithValue("@FollowerUserId", userId);
                   paramCollection.AddWithValue("@FollowingUserId", followedUserId);


               });
            }

        }
    }
}