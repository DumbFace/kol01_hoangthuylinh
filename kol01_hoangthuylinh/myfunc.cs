using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using kol01_hoangthuylinh.Models;
using cloudscribe.Pagination.Models;
using kol01_hoangthuylinh.Helpers;
using kol01_hoangthuylinh;

namespace kol01_hoangthuylinh
{
    public class myfunc
    {

        public static string ConnectionString
        {
            get
            {
                return Constants.webConnection;
            }
        }
        public List<tblZone> GetListZone(int limit = 10)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query<tblZone>("SELECT * FROM tblZone limit " + limit.ToString()).ToList();
            }
        }

        public tblZone GetZonebyId(int Id)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query<tblZone>("SELECT * FROM tblZone WHERE Id = @Id", new { Id = Id }).FirstOrDefault();
            }
        }

        public void InsertZone(string strName)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO tblZone(ZoneName)
											VALUES(@ZoneName)";
                conn.Execute(sql, new { ZoneName = strName });
            }
        }

        public void UpdateZone(tblZone tbl)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"UPDATE tblZone
		                                SET 
		                                    ZoneName = @ZoneName
                                        WHERE Id = @Id";
                conn.Execute(sql, tbl);
            }
        }

        public void DeleteZone(int Id)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"DELETE FROM tblZone WHERE Id = @Id";
                conn.Execute(sql, new { Id = Id });
            }
        }

        public List<tblCategory> GetListCategory()
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query<tblCategory>("SELECT * FROM tblCategory").ToList();
            }
        }

        public List<tblCategory> GetListCategoryByIdZone(int Id)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query<tblCategory>(@"SELECT * 
                                                                            FROM tblCategory   
                                                                        WHERE IdZone = @IdZone", new { IdZone = Id }).ToList();
            }
        }

        public tblCategory GetCategorybyId(int Id)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query<tblCategory>("SELECT * FROM tblCategory WHERE Id = @Id", new { Id = Id }).FirstOrDefault();
            }
        }

        public void InsertCategory(tblCategory tbl)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO tblCategory(IdZone, CategoryName)
											VALUES(@IdZone, @CategoryName)";
                conn.Execute(sql, tbl);
            }
        }

        public void UpdateCategory(tblCategory tbl)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"UPDATE tblCategory
		                                SET 
		                                    CategoryName = @CategoryName,PagedResult
                                            IdZone = @IdZone
                                        WHERE Id = @Id";
                conn.Execute(sql, tbl);
            }
        }

        public void DeleteCategory(int Id)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"DELETE FROM tblCategory WHERE Id = @Id";
                conn.Execute(sql, new { Id = Id });
            }
        }

        public PagedResult<tblNews> GetListNews(int intstatus, string IdZone, string strText, int page = 1, int pageSize = 20)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string strSelect = "*";
                string strFrom = string.Format(@"(SELECT IdZone, ZoneName, IdCategory, Category, Title, Avatar2x1, IdUser, UserName, 
                                                                            PublishDate, ViewCount, UrlLink 
	                                                                    FROM tblnews a
	                                                                        LEFT JOIN tblzone b on a.IdZone = b.Id
                                                                            LEFT JOIN tblcategory c on a.IdCategory = c.Id
                                                                        WHERE Status = @Status AND Title LIKE N'%{0}%'", strText);
                if (!string.IsNullOrEmpty(IdZone))
                {
                    strFrom += " AND IdZone LIKE '" + IdZone + "'";
                }
                string sqlWhere = "1=1";
                return conn.QueryPaging<tblNews>(strSelect, strFrom, sqlWhere, "PublishDate", null, page - 1, pageSize);
            }
        }

        public int InsertNews(tblNews tbl)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO tblNews(
                                           IdZone, Title, Teaser, Avatar2x1, Avatar1x2, Avatar1x1, IdUser, CreatedDate, UpdatedDate, PublishDate, Body, Status, UrlLink,
                                            ViewCount, AvatarThumb, AvatarFB, IdType, UrlSource, SEONoFollow)
								        VALUES(@IdZone,@Title, @Teaser, @Avatar2x1, @Avatar1x2, @Avatar1x1, @IdUser, Now(), Now(), @PublishDate, @Body, @Status, @UrlLink, 
                                                        0, @AvatarThumb, @AvatarFB,@IdType, @UrlSource, @SEONoFollow);
                                        SELECT  LAST_INSERT_ID();";
                return conn.Query<int>(sql, tbl).FirstOrDefault();
            }
        }

        public void UpdateNews(tblNews tbl)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                //Update không cập nhập URL link cho SEO, chỉ cập nhập URL lần đầu khi thêm bài viết
                string sql = @"UPDATE tblNews
                                            SET
                                                IdZone = @IdZone,
                                                UrlLink = @UrlLink,
                                                Title = @Title, 
                                                Teaser = @Teaser, 
                                                Avatar2x1 = @Avatar2x1, 
                                                Avatar1x2 = @Avatar1x2, 
                                                Avatar1x1 = @Avatar1x1, 
                                                IdUser = @IdUser, 
                                                UpdatedDate = Now(), 
                                                PublishDate = @PublishDate, 
                                                Body = @Body, 
                                                Status = @Status,
                                                AvatarThumb = @AvatarThumb, 
                                                AvatarFB = @AvatarFB,
                                                IdType = @IdType,
                                                UrlSource = @UrlSource, 
                                                SEONoFollow = @SEONoFollow
                                            WHERE Id = @Id";
                conn.Execute(sql, tbl);
            }
        }

        public PagedResult<tblNews> FilterContent(string fromdate, string todate, string key, int status,
                                                               string idtype, int idUser = 0, int page = 1, int pageSize = 20)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string strSelect = @" a.Id, Title, HomeFeature, PublishDate, ViewCount, Avatar2x1, UrlLink,
                                    ZoneName,IdType, SEONoFollow, UrlSource";
                string strFrom = @"tblNews a
	                                                LEFT JOIN tblZone b on a.IdZone= b.Id";
                string sqlWhere = "Status=" + status.ToString();

                if (idUser > 0)
                {
                    sqlWhere += string.Format(" AND IdUser='{0}'", idUser.ToString());
                }

                if (!string.IsNullOrEmpty(fromdate))
                {
                    sqlWhere += string.Format(" AND PublishDate>='{0}'", fromdate);
                }

                if (!string.IsNullOrEmpty(todate))
                {
                    todate += " 23:59:00";
                    sqlWhere += string.Format(" AND PublishDate<='{0}'", todate);
                }

                if (!string.IsNullOrEmpty(key))
                {
                    sqlWhere += string.Format(" AND (Title LIKE N'%{0}%' OR a.Id LIKE '{0}')", key);
                }

                if (!string.IsNullOrEmpty(idtype))
                {
                    sqlWhere += string.Format(" AND a.IdType LIKE '{0}'", idtype);
                }

                var lst = conn.QueryPaging<tblNews>(strSelect, strFrom, sqlWhere, "PublishDate DESC", null, page - 1, pageSize);
                var sql = @"SELECT b.*
                                        FROM tblNewsTag a
                                            LEFT JOIN tblTag b on a.IdTag = b.Id
                                    Where IdNews = {0};";

                StringBuilder sb = new StringBuilder();

                foreach (var c in lst.Data)
                {
                    sb.AppendFormat(sql, c.Id);
                }

                if (lst.Data.Count() > 0)
                {
                    using (var grid = conn.QueryMultiple(sb.ToString()))
                    {
                        foreach (var c in lst.Data)
                        {
                            c.Tags = grid.Read<tblTag>().ToList();
                        }
                    }
                }
                return lst;
            }
        }

        public tblNews GetNewsbyId(int Id)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                tblNews tbl = new tblNews();
                string sql = @"SELECT * FROM tblNews WHERE Id = @Id;
                                        SELECT b.*
                                            FROM tblNewsTag a
                                                LEFT JOIN tblTag b on a.IdTag = b.Id
                                        WHERE IdNews = @Id;";

                var query = conn.QueryMultiple(sql, new { Id = Id });
                tbl = query.Read<tblNews>().FirstOrDefault();
                tbl.Tags = query.Read<tblTag>().ToList();

                return tbl;
            }
        }


        public void UpdateStatus(int Id)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"UPDATE tblNews SET Status = 1 WHERE Id = @Id";
                conn.Execute(sql, new { Id = Id });
            }
        }

        public void DeleteNews(int Id)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"DELETE FROM tblNews WHERE Id = @Id";
                conn.Execute(sql, new { Id = Id });
            }
        }

        public PagedResult<tblUser> GetListUser(int page = 1, string role = "" , string name = "", int pageSize = 10)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sqlWhere = "IsDelete=0";

                if (!string.IsNullOrEmpty(role))
                {
                    sqlWhere += " AND IdRole LIKE '" + role + "'";
                }

                if (!string.IsNullOrEmpty(name))
                {
                    sqlWhere += " AND UserName LIKE '%" + name + "%'";
                }

                return conn.QueryPaging<tblUser>("Id, UserName, IdRole, IsDelete", "tblUser", sqlWhere, "UserName", null, page - 1, pageSize);
            }
        }

        public bool IsExistUserName(string str)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = string.Format("SELECT 1 FROM tblUser WHERE UserName = '{0}'", str);
                return conn.Query<bool>(sql).FirstOrDefault();
            }
        }

        public void InsertUser(tblUser tbl)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO tblUser(
                                            UserName, Password, IdRole, IsDelete)
											VALUES(@UserName, @Password, @IdRole, 0)";
                conn.Execute(sql, tbl);
            }
        }
        public void DeleteUser(int Id)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"UPDATE tblUser SET IsDelete = 1 WHERE Id = @Id";
                conn.Execute(sql, new { Id = Id });
            }
        }

        public void ResetPass(int Id, string str)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"UPDATE tblUser SET Password = @Password WHERE Id = @Id";
                conn.Execute(sql, new { Id = Id, Password = str });
            }
        }

        public tblUser GetUserbyId(int Id)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query<tblUser>("SELECT * FROM tblUser WHERE Id = @Id", new { Id = Id }).FirstOrDefault();
            }
        }

        public void UpdateRole(int Id, int IdRole)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"UPDATE tblUser SET IdRole = @IdRole WHERE Id = @Id";
                conn.Execute(sql, new { Id = Id, IdRole = IdRole });
            }
        }

        public List<tblHome> GetHomeFeature()
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query<tblHome>(@"SELECT a.Id, Title, Avatar2x1, Teaser, PublishDate, UrlLink, CategoryName, UserName
                                                                            FROM tblNews a
                                                                                LEFT JOIN tblCategory b on a.IdCategory = b.Id
                                                                                LEFT JOIN tblUser c on a.IdUser = c.Id
                                                                        WHERE HomeFeature=1 AND Status = 1
                                                                        ORDER BY PublishDate DESC
                                                                        LIMIT 3").ToList();
            }
        }

        public List<tblHome> GetLastNewsHome(int intNum)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query<tblHome>(@"SELECT a.Id, Title, Avatar2x1, Avatar1x1, Avatar1x2, AvatarThumb, Teaser, 
                                                    IdType, PublishDate, UrlLink, UserName
                                                                            FROM tblNews a
                                                                                LEFT JOIN tblUser c on a.IdUser = c.Id
                                                                        WHERE Status = 1
                                                                        ORDER BY PublishDate DESC
                                                                        LIMIT @intNum", new { intNum = intNum }).ToList();
            }
        }

        public tblUser CheckLoginUser(tblUser cls)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"SELECT Id, UserName, IdRole
                                            FROM tblUser
                                        WHERE UserName = @UserName AND Password = @Password AND IsDelete = 0";
                tblUser tbl = conn.Query<tblUser>(sql, new { UserName = cls.UserName, Password = cls.Password }).FirstOrDefault();
                return tbl;
            }
        }
        public List<tblHome> GetNewsHomeByIdZone(int IdZone, int intNum)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query<tblHome>(@"SELECT a.Id, Title, Avatar2x1, AvatarThumb,Teaser, PublishDate, UrlLink, CategoryName, UserName
                                                                            FROM tblNews a
                                                                                LEFT JOIN tblCategory b on a.IdCategory = b.Id
                                                                                LEFT JOIN tblUser c on a.IdUser = c.Id
                                                                        WHERE a.IdZone = @IdZone AND Status = 1
                                                                        ORDER BY PublishDate DESC
                                                                        LIMIT @intNum", new { IdZone = IdZone, intNum = intNum }).ToList();
            }
        }

        public PagedResult<tblHome> GetNewsByType(int IdType, int page = 1, int pageSize = 20)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string strSelect = @" Id, Title, Avatar2x1, Avatar1x2, Avatar1x1, Teaser, IdType, PublishDate, UrlLink";
                string strFrom = @"tblNews";
                string sqlWhere = "Status=1 AND IdType=" + IdType.ToString();

                return conn.QueryPaging<tblHome>(strSelect, strFrom, sqlWhere, "PublishDate DESC", null, page - 1, pageSize);
            }
        }

        public void UpdateViewCount(int Id)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"UPDATE tblNews 
                                            SET ViewCount = ViewCount + 1
                                        WHERE Id = @Id";
                conn.Execute(sql, new { Id = Id });
            }
        }
        public tblNews GetContentNewsbyId(int Id)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                var data = conn.QueryMultiple(@"SELECT a.Id, Title, Avatar2x1, a.IdZone, AvatarThumb, Teaser, Body, UrlSource,
                                                PublishDate, UrlLink, ZoneName, UserName, AvatarFB, SEONoFollow, IdType
                                            FROM tblNews a
                                                LEFT JOIN tblZone b on a.IdZone = b.Id
                                                LEFT JOIN tblUser c on a.IdUser = c.Id    
                                            WHERE a.Id = @Id;
                                            SELECT b.*
                                                FROM tblNewsTag a
                                                    LEFT JOIN tblTag b on a.IdTag = b.Id
                                            WHERE IdNews = @Id; ",
                                             new { Id = Id });
                tblNews result = data.Read<tblNews>().FirstOrDefault();
                result.Tags = data.Read<tblTag>().ToList();
                return result;
            }
        }

        public List<tblHome> GetRelatedNews(int IdZone, int Id)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query<tblHome>(@"SELECT a.Id, Title, Avatar2x1, PublishDate, UrlLink, CategoryName, UserName
                                                                            FROM tblNews a
                                                                                LEFT JOIN tblCategory b on a.IdCategory = b.Id
                                                                                LEFT JOIN tblUser c on a.IdUser = c.Id
                                                                        WHERE a.IdZone = @IdZone AND Status = 1 AND a.Id <> @Id
                                                                        ORDER BY PublishDate DESC
                                                                        LIMIT 5", new { IdZone = IdZone, Id = Id }).ToList();
            }
        }

        public List<tblHome> GetNewsRss()
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query<tblHome>(@"SELECT a.Id, a.IdZone, Title, Teaser, Avatar2x1, AvatarThumb, Body, Teaser, PublishDate, UrlLink, CategoryName, UserName
                                                                            FROM tblNews a
                                                                                LEFT JOIN tblCategory b on a.IdCategory = b.Id
                                                                                LEFT JOIN tblUser c on a.IdUser = c.Id
                                                                        WHERE Status = 1
                                                                        ORDER BY PublishDate DESC
                                                                        LIMIT 30").ToList();
            }
        }

        public bool GetOldPass(string strName, string strPass)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = string.Format("SELECT 1 FROM tblUser WHERE UserName = @UserName AND Password = @Password");
                return conn.Query<bool>(sql, new { UserName = strName, Password = strPass }).FirstOrDefault();
            }
        }

        public void UpdatePassword(string strName, string strPass)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"UPDATE tblUser
                                            SET Password = @Password
                                        WHERE UserName = @UserName";
                conn.Execute(sql, new { UserName = strName, Password = strPass });
            }
        }

        public PagedResult<tblTag> GetListTag(int page = 1, int pageSize = 20, string name = "")
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sqlWhere = "1=1";

                if (!string.IsNullOrEmpty(name))
                {
                    sqlWhere += " AND TagName LIKE N'%" + name + "%'";
                }

                return conn.QueryPaging<tblTag>("Id, TagName, Description", "tblTag", sqlWhere, "TagName", null, page - 1, pageSize);
            }
        }
        public List<tblTag> GetListTag()
        {
            string sql = @"SELECT a.*
                                        FROM tblTag a 
	                                        INNER JOIN tblNewsTag b on a.Id = b.IdTag
                                            INNER JOIN tblNews c on b.IdNews = c.Id
                                        WHERE Status = 1
                                        ORDER BY RAND()
                                        LIMIT 10";
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query<tblTag>(sql).ToList();
            }
        }
        public PagedResult<tblHome> GetNewsByTag(int IdTag, int page = 1, int pageSize = 20)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string strSelect = @" a.Id, Title, Avatar2x1, Teaser, PublishDate, UrlLink";
                string strFrom = @"tblNews a
	                                                LEFT JOIN tblNewsTag b on a.Id = b.IdNews";
                string sqlWhere = @"Status=1 AND IdTag = @IdTag";

                return conn.QueryPaging<tblHome>(strSelect, strFrom, sqlWhere, "PublishDate DESC", new { IdTag = IdTag }, page - 1, pageSize);
            }
        }

        public List<tblTag> GetListTags(string str)
        {
            string sql = string.Format("SELECT Id, TagName FROM tblTag WHERE TagName LIKE N'{0}%'", str);

            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query<tblTag>(sql).ToList();
            }
        }

        public tblTag GetTagbyId(int Id)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query<tblTag>("SELECT * FROM tblTag WHERE Id = @Id", new { Id = Id }).FirstOrDefault();
            }
        }

        public int InsertTag(tblTag model)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO tblTag(TagName, Description)
											VALUES(@TagName, @Description);
                                             SELECT LAST_INSERT_ID();";
                return conn.Query<int>(sql, model).FirstOrDefault();
            }
        }

        public void UpdateTag(tblTag tbl)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"UPDATE tblTag
		                                SET 
		                                    TagName = @TagName, Description = @Description
                                        WHERE Id = @Id";
                conn.Execute(sql, tbl);
            }
        }

        public void DeleteTag(int Id)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"DELETE FROM tblTag WHERE Id = @Id";
                conn.Execute(sql, new { Id = Id });
            }
        }

        public List<tblTag> GetListTagsByIdNews(int Id)
        {
            string sql = @"SELECT b.*
                                        FROM tblNewsTag a
                                            LEFT JOIN tblTag b on a.IdTag = b.Id
                                    WHERE IdNews = @Id";
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query<tblTag>(sql, new { Id = Id }).ToList();
            }
        }

        public void InsertNewsTags(int Id, string tags)
        {
            string sql = @"INSERT INTO tblNewsTag(IdNews, IdTag) VALUES ";
            string strSql = string.Empty;

            foreach (string str in tags.Split(','))
            {
                if (!string.IsNullOrEmpty(str))
                {
                    strSql += string.Format("({0}, {1}),", Id.ToString(), str);
                }
            }

            if (strSql.Length > 0)
            {
                strSql = strSql.Substring(0, strSql.Length - 1);
            }

            sql += strSql;

            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                var result = conn.Execute(sql);
            }
        }
        public void DeleteNewsTags(int Id)
        {
            string sql = @"DELETE FROM tblNewsTag where IdNews=@Id";
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                conn.Execute(sql, new { Id = Id });
            }
        }

        public List<tblHome> GetLastNewsHome(int intNum, int Id = 0)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"SELECT a.Id, Title, Avatar2x1, AvatarThumb, Teaser, PublishDate, UrlLink, UserName
                                            FROM tblNews a
                                                LEFT JOIN tblUser c on a.IdUser = c.Id
                                            WHERE Status = 1 AND a.Id <> @Id
                                            ORDER BY PublishDate DESC
                                            LIMIT @intNum";
                return conn.Query<tblHome>(sql, new { intNum = intNum, Id = Id }).ToList();
            }
        }

        public PagedResult<tblHome> GetNewsHome(int page, int pagesize = 10)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string strSelect = @"a.Id, Title,IdZone,ZoneName, Avatar2x1, AvatarThumb, Teaser, PublishDate, UrlLink, UserName";
                string strFrom = @"tblNews a
                                                    LEFT JOIN tblUser c on a.IdUser = c.Id
                                                    LEFT JOIN tblZone b on a.IdZone = b.Id";
                string sqlWhere = "Status = 1";
                return conn.QueryPaging<tblHome>(strSelect, strFrom, sqlWhere, "PublishDate DESC", null, page - 1, pagesize);
            }
        }

        public List<tblTag> GetListTagView(int intNum = 10)
        {
            string sql = @"SELECT Id, TagName
                                        FROM tblTag a
	                                        INNER JOIN (SELECT IdTag
                                                                        FROM tblNewsTag a
                                                                            INNER JOIN tblNews b on a.IdNews = b.Id
																	WHERE Status = 1
                                                                    GROUP BY IdTag
                                                                    ORDER BY COUNT(IdTag) DESC) b on a.Id = b.IdTag
                                        LIMIT " + intNum.ToString();
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query<tblTag>(sql).ToList();
            }
        }

        public tblNews GetTopTag(int IdTag)
        {
            string sql = @"SELECT Title, Teaser, PublishDate
                                        FROM tblNews a
	                                        LEFT JOIN tblNewsTag b on a.Id = b.IdNews
                                        WHERE Status = 1 and IdTag = @IdTag
                                        LIMIT 1";
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query<tblNews>(sql, new { IdTag = IdTag }).FirstOrDefault();
            }
        }

        public tblNews GetTopZone(int IdZone)
        {
            string sql = @"SELECT Title, Teaser, PublishDate
                                        FROM tblNews a
	                                        LEFT JOIN tblNewsTag b on a.Id = b.IdNews
                                        WHERE Status = 1 and IdZone = @IdZone
                                        LIMIT 1";
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query<tblNews>(sql, new { IdZone = IdZone }).FirstOrDefault();
            }
        }

        public List<tblNews> GetListSitemap()
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query<tblNews>(@"SELECT Id, UrlLink, PublishDate
                                                                            FROM tblNews
                                                                        WHERE Status = 1
                                                                        ORDER BY PublishDate DESC").ToList();
            }
        }

        public PagedResult<tblHome> SearchNewsbyKey(string strText, int page = 1, int pageSize = 20)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string strSelect = @"Id, Title, Avatar2x1, Avatar1x1, Avatar1x2, Teaser, PublishDate, UrlLink, IdType";
                string strFrom = @"tblNews";
                string sqlWhere = string.Format(@"Status= 1 
                                                            AND IdType in (1,3) 
                                                            AND (Title LIKE N'%{0}%' OR Teaser LIKE N'%{0}%')", strText);
                return conn.QueryPaging<tblHome>(strSelect, strFrom, sqlWhere, "PublishDate DESC", null, page - 1, pageSize);
            }
        }

        public void UpdateAboutMe(tblKOL tbl)
        {
            string sql = @"UPDATE tblKOL
		                                SET 
		                                    Body = @Body,
                                            Avatar = @Avatar,
                                            Title = @Title,
                                            Teaser = @Teaser
                                        WHERE Id = @Id";
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                conn.Execute(sql, tbl);
            }
        }

        public tblKOL GetProfile()
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query<tblKOL>("SELECT * FROM tblKOL").FirstOrDefault();
            }
        }

        public tblConfig GetConfigWeb()
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"SELECT * FROM tblConfig";
                tblConfig tbl = conn.Query<tblConfig>(sql).FirstOrDefault();
                if (tbl == null)
                {
                    conn.Execute("INSERT INTO tblConfig (TitleWeb,DescriptionWeb) VALUES ('','')");
                    tbl = conn.Query<tblConfig>(sql).FirstOrDefault();
                }
                return tbl;
            }
        }

        public void UpdateConfigWeb(tblConfig tbl)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"UPDATE tblConfig SET
                                                    TitleWeb = @TitleWeb,
                                                    DescriptionWeb = @DescriptionWeb
                                                WHERE Id = @Id;";
                conn.Execute(sql, tbl);
            }
        }

        public List<tblHome> GetAllNewsHome()
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @" SELECT a.Id, Title,IdZone,ZoneName, Avatar2x1, AvatarThumb, Teaser, PublishDate, UrlLink, UserName 
						        FROM tblNews a
							        LEFT JOIN tblUser c on a.IdUser = c.Id
							        LEFT JOIN tblZone b on a.IdZone = b.Id
                                WHERE Status = 1
                                ORDER BY PublishDate DESC;";
                return conn.Query<tblHome>(sql).ToList();
            }
        }

        public List<tblHome> GetAllLastNewsHome(int intNum, int Id = 0)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"SELECT a.Id, Title, Avatar2x1, AvatarThumb, Teaser, PublishDate, UrlLink, UserName
                                            FROM tblNews a
                                                LEFT JOIN tblUser c on a.IdUser = c.Id
                                            WHERE Status = 1 AND a.Id <> @Id
                                            ORDER BY PublishDate DESC
                                            LIMIT @intNum";
                return conn.Query<tblHome>(sql, new { intNum = intNum, Id = Id }).ToList();
            }
        }

        public List<tblHome> GetListHome(int limitNews, int limitVideo)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"SELECT a.Id, Title, Avatar2x1,
                                        Avatar1x2, Avatar1x1, Teaser, 
                                               IdType, PublishDate, UrlLink
                                FROM tblNews a 
	                                INNER JOIN 
                               ((SELECT Id FROM tblNews WHERE IdType = 1 AND Status = 1 ORDER BY PublishDate DESC LIMIT @limitNews)
                                UNION
                                (SELECT Id FROM tblNews WHERE IdType = 3 AND Status = 1 ORDER BY PublishDate DESC LIMIT @limitVideo)) 
                                AS b on a.Id = b.Id 
                                ORDER BY PublishDate DESC
                                ";
                return conn.Query<tblHome>(sql, new { limitNews = limitNews, limitVideo = limitVideo }).ToList();
            }
        }

        public tblNews GetTopTypeNews(int IdType)
        {
            string sql = @"SELECT Title, Teaser, PublishDate
                                        FROM tblNews a
	                                        LEFT JOIN tblNewsTag b on a.Id = b.IdNews
                                        WHERE Status = 1 and IdZone = @IdType
                                        LIMIT 1";
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query<tblNews>(sql, new { IdType = IdType }).FirstOrDefault();
            }
        }

        public PagedResult<tblHome> GetVideoCategory(int page, int pageSize)
        {
            string sqlSelect = @"Id, Title, Avatar2x1, Avatar1x2, 
                          Avatar1x1, Teaser, IdType, PublishDate, UrlLink";
            string sqlFrom = @"tblNews";
            string sqlWhere = @"IdType = 3 ORDER BY PublishDate DESC";
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.QueryPaging<tblHome>(sqlSelect, sqlFrom, sqlWhere, "PublishDate DESC", null, page - 1, pageSize);
            }
        }

        public PagedResult<tblHome> GetNewsCategory(int page, int pageSize)
        {
            string sqlSelect = @"Id, Title, Avatar2x1, Teaser, IdType, PublishDate, UrlLink";
            string sqlFrom = @"tblNews";
            string sqlWhere = @"IdType = 1 ORDER BY PublishDate DESC";
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.QueryPaging<tblHome>(sqlSelect, sqlFrom, sqlWhere, "PublishDate DESC", null, page - 1, pageSize);
            }
        }

        public PagedResult<tblGallery> GetGalleryCategory(int page, int pageSize)
        {
            string sqlSelect = @"Id, Title, AvatarCover";
            string sqlFrom = @"tblGallery";
            string sqlWhere = @"1=1";
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.QueryPaging<tblGallery>(sqlSelect, sqlFrom, sqlWhere, "CreateDate DESC", null, page - 1, pageSize);
            }
        }

        public List<tblHome> GetGalleryAlbumCategory()
        {
            string sql = @"SELECT Id, Title, Avatar2x1, Avatar1x2, 
                          Avatar1x1, Teaser, IdType, PublishDate, UrlLink  
	                        FROM tblNews 
	                        Where IdType = 2
	                        ORDER BY PublishDate DESC 
	                        LIMIT 12 ";
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query<tblHome>(sql).ToList();
            }
        }

        public void InsertGallery(tblGallery model)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO tblGallery
                                                    (Title, CreateDate, AvatarCover)
											VALUES
                                                    (@Title, @CreateDate, @AvatarCover)";
                conn.Execute(sql, model);
            }
        }

        public void UpdateGallery(tblGallery tbl)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"UPDATE tblGallery
		                                SET 
		                                    Title = @Title,
                                            CreateDate = @CreateDate,
		                                    AvatarCover = @AvatarCover
                                        WHERE Id = @Id";
                conn.Execute(sql, tbl);
            }
        }

        public void DeleteGallery(int Id)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"DELETE FROM tblGallery WHERE Id = @Id";
                conn.Execute(sql, new { Id });
            }
        }

        public tblGallery GetGalleryById(int Id)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query<tblGallery>(@"SELECT Id, Title, CreateDate, AvatarCover 
                                                FROM tblGallery 
                                                WHERE Id = @Id", new { Id }).FirstOrDefault();
            }
        }

        public List<tblGallery> GetListGalleryHome(int Limit)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"SELECT Id, Title, CreateDate, AvatarCover 
                                FROM tblGallery 
                                  Where 1=1 
                                 Order by CreateDate DESC 
                                Limit @Limit";
                return conn.Query<tblGallery>(sql, new { Limit }).ToList();
            }
        }

        public PagedResult<tblGallery> GetListGallery(int page = 1, int pageSize = 20, string name = "")
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.QueryPaging<tblGallery>("Id, Title, CreateDate, AvatarCover", "tblGallery", "1=1", "CreateDate DESC", null, page - 1, pageSize);
            }
        }

        public PagedResult<tblGalleryDT> GetListGalleryDT(int id, int page = 1, int pageSize = 10, string name = "")
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sqlSelect = @"a.Id, Avatar, a.CreateDate, b.Title";
                string sqlFROM = @"tblGalleryDT a 
                                    LEFT JOIN tblGallery b on a.IdGallery = b.Id";
                string sqlWhere = "a.IdGallery = " + id;

                return conn.QueryPaging<tblGalleryDT>(sqlSelect, sqlFROM, sqlWhere, "a.CreateDate DESC", null, page - 1, pageSize);
            }
        }

        public List<tblGalleryDT> GetAllAlbumById(int id)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = @"SELECT a.Id, Avatar, a.CreateDate, b.Title
                                    FROM tblGalleryDT a 
                                     LEFT JOIN tblGallery b on a.IdGallery = b.Id
                                    WHERE a.IdGallery = @id
                                       ORDER BY CreateDate DESC";
                return conn.Query<tblGalleryDT>(sql, new { id }).ToList();
            }
        }


        public int InsertGalleryDetail(tblGalleryDT tbl)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO tblGalleryDT (IdGallery, Avatar, CreateDate)
								                     VALUES 
                                                        (@IdGallery, @Avatar, @CreateDate);";
                return conn.Query<int>(sql, tbl).FirstOrDefault();
            }
        }

        public void DeleteGalleryDT(int Id)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"DELETE FROM tblGalleryDT WHERE Id = @Id";
                conn.Execute(sql, new { Id });
            }
        }

        public List<tblHome> GetAllNewsByIdTag(int IdTag)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string sql = @"SELECT a.Id, Title, Avatar2x1, Teaser, PublishDate, UrlLink
                                            FROM tblNews a
	                                                LEFT JOIN tblNewsTag b on a.Id = b.IdNews
                                            WHERE Status = 1 AND IdTag = @IdTag
                                        ORDER BY PublishDate DESC";
                return conn.Query<tblHome>(sql, new { IdTag }).ToList();
            }
        }
    }
}