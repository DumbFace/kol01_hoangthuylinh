using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KOL_HoangThuyLinh;
using KOL_HoangThuyLinh.Models;

namespace KOL_HoangThuyLinh
{
    public class MyCacheFunc
    {
        private static RedisCache redis = new RedisCache();

        public static tblNews GetNews(int Id)
        {
            string strKey = myCommon.RedisSuffix + string.Format(myCommon.hsContentId, Id);
            tblNews tbl = redis.Get<tblNews>(strKey);
            if (tbl == null)
            {
                tbl = new myfunc().GetContentNewsbyId(Id);
                redis.Set(strKey, tbl);
            }
            return tbl;
        }

        public static void SetNews(int Id)
        {
            string strKey = myCommon.RedisSuffix + string.Format(myCommon.hsContentId, Id);
            redis.Remove(strKey);
            redis.Set(strKey, new myfunc().GetContentNewsbyId(Id));
        }

        public static void RemoveNews(int Id)
        {
            string strKey = myCommon.RedisSuffix + string.Format(myCommon.hsContentId, Id);
            redis.Remove(strKey);
        }

        public static void SetLastNewsHome()
        {
            string strKey = myCommon.RedisSuffix + myCommon.hsLastHome;
            redis.Remove(strKey);
            redis.Set(strKey, new myfunc().GetLastNewsHome(8));
        }

        public static List<tblHome> GetLastNews(int id = 0)
        {
            string strKey = myCommon.RedisSuffix + myCommon.hsLastHome;
            List<tblHome> lst = redis.Get<List<tblHome>>(strKey);
            if (lst == null || lst.Count == 0)
            {
                lst = new myfunc().GetLastNewsHome(8);
                redis.Set(strKey, lst);
            }
            return lst;
        }

        public static void SetNewsHome()
        {
            string strKey = myCommon.RedisSuffix + myCommon.hsNewsHome;
            redis.Remove(strKey);
            redis.Set(strKey, new myfunc().GetAllNewsHome());
        }

        public static List<tblHome> GetNewsHome()
        {
            string strKey = myCommon.RedisSuffix + myCommon.hsNewsHome;
            List<tblHome> lst = redis.Get<List<tblHome>>(strKey);
            if (lst == null || lst.Count == 0)
            {
                lst = new myfunc().GetAllNewsHome();
                redis.Set(strKey, lst);
            }
            return lst;
        }

        public static void SetConfigWeb()
        {
            string strKey = myCommon.RedisSuffix + myCommon.hsConfigWeb;
            redis.Remove(strKey);
            redis.Set(strKey, new myfunc().GetConfigWeb());
        }

        public static tblConfig GetConfigWeb()
        {
            string strKey = myCommon.RedisSuffix + myCommon.hsConfigWeb;
            tblConfig lst = redis.Get<tblConfig>(strKey);
            if (lst == null)
            {
                lst = new myfunc().GetConfigWeb();
                redis.Set(strKey, lst);
            }
            return lst;
        }

        public static void SetAboutMe()
        {
            string strKey = myCommon.RedisSuffix + myCommon.hsAboutMe;
            redis.Remove(strKey);
            redis.Set(strKey, new myfunc().GetProfile());
        }

        public static tblKOL GetAboutMe()
        {
            string strKey = myCommon.RedisSuffix + myCommon.hsAboutMe;
            tblKOL lst = redis.Get<tblKOL>(strKey);
            if (lst == null)
            {
                lst = new myfunc().GetProfile();
                redis.Set(strKey, lst);
            }
            return lst;
        }

        public static void SetTags()
        {
            string strKey = myCommon.RedisSuffix + myCommon.hsTags;
            redis.Remove(strKey);
            redis.Set(strKey, new myfunc().GetListTagView(10));

        }
        public static List<tblTag> GetTags()
        {
            string strKey = myCommon.RedisSuffix + myCommon.hsTags;
            List<tblTag> lst = redis.Get<List<tblTag>>(strKey);
            if (lst == null || lst.Count == 0)
            {
                lst = new myfunc().GetListTagView(10);
                redis.Set(strKey, lst);
            }
            return lst;
        }

        public static List<tblHome> GetNewsByIdTag(int Id)
        {
            string strKey = myCommon.RedisSuffix + string.Format(myCommon.hsNewsByTag, Id);
            List<tblHome> lst = redis.Get<List<tblHome>>(strKey);
            if (lst == null || lst.Count == 0)
            {
                lst = new myfunc().GetAllNewsByIdTag(Id);
                redis.Set(strKey, lst);
            }
            return lst;
        }

        public static void SetNewsByIdTag(int Id)
        {
            string strKey = myCommon.RedisSuffix + string.Format(myCommon.hsNewsByTag, Id);
            redis.Remove(strKey);
            redis.Set(strKey, new myfunc().GetAllNewsByIdTag(Id));

        }

        public static void SetTag(int Id)
        {
            string strKey = myCommon.RedisSuffix + string.Format(myCommon.hsTagId, Id);
            redis.Remove(strKey);
            redis.Set(strKey, new myfunc().GetTagbyId(Id));
        }

        public static tblTag GetTag(int Id)
        {
            string strKey = myCommon.RedisSuffix + string.Format(myCommon.hsTagId, Id);
            tblTag tbl = redis.Get<tblTag>(strKey);
            if (tbl == null)
            {
                tbl = new myfunc().GetTagbyId(Id);
                redis.Set(strKey, tbl);
            }
            return tbl;
        }

        public static void SetTopTagNews(int Id)
        {
            string strKey = myCommon.RedisSuffix + string.Format(myCommon.hsTopTagNews, Id);
            redis.Remove(strKey);
            redis.Set(strKey, new myfunc().GetTopTag(Id));
        }
    }
}