

namespace gwig.Web.Services
{
    public class FollowersService : BaseService, IFollowersService
    {
        public int InsertTest(FollowersAddRequest model)
        {

            int uid = 0;
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Followers_Insert"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@FollowingUserId", model.FollowingUserId);
                   paramCollection.AddWithValue("@FollowerUserId", model.FollowerUserId);


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



        public void Update(FollowersUpdateRequest model, int id)
        {


            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Followers_updateById"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@FollowingUserId", model.FollowingUserId);
                   paramCollection.AddWithValue("@FollowerUserId", model.FollowerUserId);
                   paramCollection.AddWithValue("@Id", id);

               });
        }



        public List<FollowersPost> GetAllFollowingByFollowersId(followersUserIdRequest model, ref int TotalItemCount)
        {
            List<FollowersPost> followingList = null;
            int totalItemCount = 0;
            DataProvider.ExecuteCmd(GetConnection, "dbo.Followers_SelectByFollowerUserIdUserProfileIdmyMediaUserId"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {

                   paramCollection.AddWithValue("@FollowerUserId", model.FollowerUserId);
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


                   if (followingList == null)
                   {
                       followingList = new List<FollowersPost>();

                   }

                   followingList.Add(p);

               }

               );

            TotalItemCount = totalItemCount;

            return followingList;

        }



        public FollowersPost SelectByFollowingId(followersUserIdRequest model)
        {
            FollowersPost p = null;
            DataProvider.ExecuteCmd(GetConnection, "dbo.Followers_SelectByFollowersId"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Id", model.FollowerUserId);



               }, map: delegate (IDataReader reader, short set)
               {
                   p = new FollowersPost();
                   int startingindex = 0;
                   p.id = reader.GetSafeInt32(startingindex++);
                   p.Created = reader.GetSafeDateTime(startingindex++);
                   p.FollowingUserId = reader.GetSafeString(startingindex++);
                   p.FollowerUserId = reader.GetSafeString(startingindex++);

               }

               );


            return p;







        }
        public List<FollowersPost> GetAllPost()
        {
            List<FollowersPost> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Followers_SelectAll"
               , inputParamMapper: null
               , map: delegate (IDataReader reader, short set)
               {
                   FollowersPost p = new FollowersPost();
                   int startingindex = 0; //startingOrdinal
                   p.id = reader.GetSafeInt32(startingindex++);
                   p.Created = reader.GetSafeDateTime(startingindex++);
                   p.FollowingUserId = reader.GetSafeString(startingindex++);
                   p.FollowerUserId = reader.GetSafeString(startingindex++);


                   if (list == null)
                   {
                       list = new List<FollowersPost>();
                   }

                   list.Add(p);
               }
               );


            return list;
        }

        public void DeleteById(string followedUserId)
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

        public List<string> CheckIsFollowingUsers(string userId, IEnumerable<string> userIds)
        {
            var users = new List<string>();

            DataProvider.ExecuteCmd(GetConnection, "dbo.Followers_CheckIsFollowingUsers", param =>
            {
                param.AddWithValue("@UserId", userId);

                SqlParameter p = new SqlParameter("@UserIds", SqlDbType.Structured);
                p.Value = new NVarcharTable(userIds);

                param.Add(p);
            }, (reader, set) =>
            {
                int startingIndex = 0;

                string uId = reader.GetSafeString(startingIndex++);
                
                users.Add(uId);
            });

            return users;
        }

        public List<string> CheckIsUsersFollowing(string userId, IEnumerable<string> userIds)
        {
            var users = new List<string>();

            DataProvider.ExecuteCmd(GetConnection, "dbo.Followers_CheckUserFollowingUsers", param =>
            {
                param.AddWithValue("@UserId", userId);

                SqlParameter p = new SqlParameter("@UserIds", SqlDbType.Structured);
                p.Value = new NVarcharTable(userIds);

                param.Add(p);
            }, (reader, set) =>
            {
                int startingIndex = 0;

                string uId = reader.GetSafeString(startingIndex++);

                users.Add(uId);
            });

            return users;
        }
    }
}
