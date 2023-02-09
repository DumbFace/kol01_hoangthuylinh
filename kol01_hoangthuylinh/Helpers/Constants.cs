using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kol01_hoangthuylinh.Helpers
{
    public static class Constants
    {
        //ConnectString DB
        public static string webConnection = "";  

        //Limit produt per 1 page  
        public static int limitZone = 5;
        public static int limitTag = 5;
        public static int limitContent = 5;
        public static int limitUser = 20;

        //URL
        public static string urlHost = "";

        //Facebook
        public static string PlacementFB = "";

        //Redis Cache Enable
        public static bool isRedisCachedEnable = false;
    }
}