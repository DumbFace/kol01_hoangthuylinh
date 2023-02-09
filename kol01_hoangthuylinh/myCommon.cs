using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using kol01_hoangthuylinh.Models;
using Newtonsoft.Json;
using System.Diagnostics;

namespace kol01_hoangthuylinh
{
    public static class SessionExtensions
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }

    public class myCommon
    {
        public const string hsKey = "Pa$$w0rd#Web#2021";
        public const string hsDetail = "Detail-";
        public const string hsContentId = "ContentId-{0}";
        public const string hsLastHome = "HomeLast";
        public const string hsNewsHome = "ListNewsHome";
        public const string hsConfigWeb = "ConfigWeb";
        public const string hsAboutMe = "AboutMe";
        public const string hsTags = "ListTags";
        public const string hsNewsByTag = "ListNewsByTag-{0}";
        public const string hsTagId = "TagId-{0}";
        public const string hsTopTagNews = "TopTagNews-{0}";
        public const string hsCookieName = "webCMS1";
        public const string hsSessionName = "TuiKhon_User";


        // public static string RedisConnection
        // {
        //     get
        //     {
        //         return WebConfigurationManager.AppSettings["RedisConnectString"];
        //     }
        // }

        // public static string RedisSuffix
        // {
        //     get
        //     {
        //         return WebConfigurationManager.AppSettings["RedisKeySuffix"];
        //     }
        // }

        public static bool isEnableRedisCached => Helpers.Constants.isRedisCachedEnable;

        // public static void CachedNews(int Id)
        // {
        //     if (Id != 0)
        //     {
        //         MyCacheFunc.SetNews(Id);
        //     }
        //     MyCacheFunc.SetLastNewsHome();
        //     MyCacheFunc.SetNewsHome();
        //     MyCacheFunc.SetTags();
        //     foreach (tblTag tbl in new myfunc().GetListTagsByIdNews(Id))
        //     {
        //         MyCacheFunc.SetTopTagNews(tbl.Id);
        //         MyCacheFunc.SetNewsByIdTag(tbl.Id);
        //     }
        // }

        public static List<tblNewsType> GetNewsIdType => new List<tblNewsType>()
        {
            new tblNewsType(){Id =  1, Name = "Bài viết", NameEng="NEWS", Url = "bai-viet"},
            new tblNewsType(){Id = 3, Name = "Bài viết video", NameEng="VIDEO", Url= "bai-viet-video" },
        };

        public static List<tblNewsType> MenuHeader => new List<tblNewsType>()
        {
            new tblNewsType(){Id =  1, Name = "VIDEO", Url = "bai-viet-video"},
            new tblNewsType(){Id =  2, Name = "NEWS", Url = "bai-viet"},
            new tblNewsType(){Id = 3, Name = "GALLERY", Url= "bo-suu-tap" },
        };

        public static string Encrypt(string strText)
        {

            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(strText);

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hsKey));

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string strText)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(strText);

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hsKey));

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static string RandomPass(int intLen = 15)
        {
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            Random random = new Random();

            char[] chars = new char[intLen];
            for (int i = 0; i < intLen; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }

        public static string RemoveSpecial(string str)
        {
            Regex reg = new Regex(@"[\x00-\x08 \x0B-\x1F \x7F]");
            return reg.Replace(str, " ");
        }

        public static string FriendlyUrl(string strTitle)
        {
            return ReplaceSpecial(strTitle);
        }

        public static string ReplaceSpecial(string title)
        {
            if (title == null) return string.Empty;

            const int maxlen = 500;
            int len = title.Length;
            bool prevdash = false;
            var sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = title[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    sb.Append((char)(c | 32));
                    prevdash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' ||
                    c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevdash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevdash = true;
                    }
                }
                else if ((int)c >= 128)
                {
                    int prevlen = sb.Length;
                    sb.Append(RemapInternationalCharToAscii(c));
                    if (prevlen != sb.Length) prevdash = false;
                }
                if (i == maxlen) break;
            }

            if (prevdash)
                return sb.ToString().Substring(0, sb.Length - 1);
            else
                return sb.ToString();
        }

        public static string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("áàảãạăắằẳẵặâấầẩẫậ".Contains(s))
            {
                return "a";
            }
            else if ("éèẻẽẹêếềểễệ".Contains(s))
            {
                return "e";
            }
            else if ("íìỉĩị".Contains(s))
            {
                return "i";
            }
            else if ("óòỏõọôốồổỗộơớờởỡợ".Contains(s))
            {
                return "o";
            }
            else if ("úùủũụưứừửữự".Contains(s))
            {
                return "u";
            }
            else if ("ýỳỷỹỵ".Contains(s))
            {
                return "y";
            }
            else if ("đ".Contains(s))
            {
                return "d";
            }
            else
            {
                return "";
            }
        }

        public static string Sha256Encrypt(string plainText)
        {
            SHA256Managed crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(plainText), 0, Encoding.UTF8.GetByteCount(plainText));
            foreach (byte bit in crypto)
            {
                hash += bit.ToString("x2");
            }
            return hash;
        }

        public static string GetContentRss(string strText, int Id)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(strText);
            var a = doc.DocumentNode.SelectNodes("//a");
            if (a != null)
            {
                foreach (var item in a)
                {
                    var href = item.Attributes["href"].Value;
                    if (!href.Contains("utm_campaign"))
                    {
                        string t = "IA";
                        string utm = string.Format("?utm_campaign=Internallink&utm_source={0}&utm_medium={1}", Id, t);
                        item.SetAttributeValue("href", href + utm);
                    }
                }
            }
            strText = doc.DocumentNode.OuterHtml;

            //process BODY
            Regex reParagraph = new Regex(@"<p.+?>(.|\n)*?</p>", RegexOptions.IgnoreCase);
            Regex reDiv = new Regex(@"<div.+?>(.|\n)*?</div>", RegexOptions.IgnoreCase);
            MatchCollection mc = reParagraph.Matches(strText);
            MatchCollection mcDiv = reDiv.Matches(strText);

            Regex reImg = new Regex(@"<img\s[^>]*>(.|\n)*?", RegexOptions.IgnoreCase);
            Regex reIframe = new Regex(@"<iframe\s[^>]*>", RegexOptions.IgnoreCase);
            Regex reSrc = new Regex(@"src=(?:(['""])(?<src>(?:(?!\1).)*)\1|(?<src>[^\s>]+))", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            //For Image
            foreach (Match m in mc)
            {
                MatchCollection mcImg = reImg.Matches(m.Groups[0].Value);
                if (mcImg.Count > 0)
                {
                    string strCaption = "";
                    string str = "";

                    Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);
                    strCaption = _htmlRegex.Replace(m.Groups[0].Value, string.Empty);

                    foreach (Match mImg in mcImg)
                    {
                        Match mSrc = reSrc.Match(mImg.Groups[0].Value);
                        var src = mSrc.Groups["src"].Value;

                        str += "<figure><img src=\"" + System.Web.HttpUtility.UrlPathEncode(src) + "\" />"
                                                          + (strCaption.Trim() != "" ? ("<figcaption>" + strCaption + "</figcaption>") : "")
                                                          + "</figure>";
                    }
                    strText = strText.ToString().Replace(m.Groups[0].Value, str);
                }

                //For video in P
                MatchCollection mcIframes = reIframe.Matches(m.Groups[0].Value);
                if (mcIframes.Count > 0)
                {
                    foreach (Match mIframe in mcIframes)
                    {
                        Match mSrc = reSrc.Match(mIframe.Groups[0].Value);
                        var src = mSrc.Groups["src"].Value;
                        if (!String.IsNullOrEmpty(src))
                        {
                            if (src.ToLower().Contains("/embed/") && !src.ToLower().Contains("youtube.com"))
                            {

                            }
                            else if (src.Contains("youtube"))
                            {
                                strText = strText.Replace(m.Groups[0].Value, "<figure class=\"op-social\"><iframe width=\"300\" height=\"169\" src=\"" + src + "\" frameborder=\"0\" allowfullscreen></iframe></figure>");
                            }
                        }
                    }
                }
            }

            //For Video
            foreach (Match mDiv in mcDiv)
            {
                //search <iframe>
                MatchCollection mcIframes = reIframe.Matches(mDiv.Groups[0].Value);
                if (mcIframes.Count > 0)
                {
                    foreach (Match mIframe in mcIframes)
                    {
                        Match mSrc = reSrc.Match(mIframe.Groups[0].Value);
                        var src = mSrc.Groups["src"].Value;
                        if (!String.IsNullOrEmpty(src))
                        {
                            if (src.ToLower().Contains("/embed/") && !src.ToLower().Contains("youtube.com"))
                            {

                            }
                            else if (src.Contains("youtube"))
                            {
                                strText = strText.Replace(mDiv.Groups[0].Value, "<figure class=\"op-social\"><iframe width=\"300\" height=\"169\" src=\"" + src + "\" frameborder=\"0\" allowfullscreen></iframe></figure>");
                            }
                        }
                    }
                }
            }

            return strText;
        }
        public static string FormatDateTimeYYYY(DateTime date)
        {
            return date.ToString("yyyy-MM-ddTHH:mm:00+07:00");
        }


        public static string patternVideoYoutube = @"<figure class='media ck-widget' contenteditable='false'>
                                                    <div class='ck-media__wrapper' data-oembed-url='{0}'>
                                                    <div style='position: relative; padding-bottom: 100%; height: 0; padding-bottom: 56.2493%;'>
                                                   <iframe src='{0}' style='position: absolute; width: 100%; height: 100%; top: 0; left: 0;' frameborder='0' allow='autoplay; encrypted-media' allowfullscreen=''></iframe>
                                                    </div>
                                                    </div>
                                                </figure>";
    }
}