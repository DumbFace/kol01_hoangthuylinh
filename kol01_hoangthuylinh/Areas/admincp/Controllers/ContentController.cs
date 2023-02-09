using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;
using kol01_hoangthuylinh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace kol01_hoangthuylinh.Areas.admincp.Controllers
{
    [Area("Admincp")]
    public class ContentController : BaseController
    {
        private int limitPageSize = Helpers.Constants.limitContent;
        // GET: admincp/Content
         public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View(new tblNews());
        }

        public ActionResult Update(int Id)
        {
            tblNews tbl = new myfunc().GetNewsbyId(Id);
            ViewBag.lstTag = tbl.Tags;
            return View(tbl);
        }

        public ActionResult SaveNews(tblNews tbl, int intstatus = 0, int intFlag = 0, string dtPublic = "", string tags = "")
        {
            tblUser obj = GetLoggedUser;

            tbl.Title = myCommon.RemoveSpecial(tbl.Title);
            tbl.Teaser = myCommon.RemoveSpecial(tbl.Teaser);
            tbl.UrlLink = myCommon.FriendlyUrl(tbl.Title);
            tbl.Status = intstatus;
            tbl.IdUser = obj.Id;
            tbl.PublishDate = DateTime.ParseExact(dtPublic, "yyyy/MM/dd HH:mm", null);

            int Id = 0;
            myfunc myfunc = new myfunc();

            if (intFlag == 0)
            {
                Id = myfunc.InsertNews(tbl);
            }
            else
            {
                Id = tbl.Id;
                myfunc.UpdateNews(tbl);
                myfunc.DeleteNewsTags(Id);
            }


            if (!string.IsNullOrEmpty(tags))
            {
                myfunc.InsertNewsTags(Id, tags);
            }

            return Content("ok");
        }

        public ActionResult ListActive()
        {
            return View();
        }
        public ActionResult GetListActive(string key, string fromdate, string todate, string idtype,
                                                int page = 1, int pagesize = 10)
        {
            ViewBag.page = page;
            ViewBag.pagesize = pagesize;
            ViewBag.key = key;
            ViewBag.fromdate = fromdate;
            ViewBag.todate = todate;
            ViewBag.idtype = idtype;

            return PartialView(new myfunc().FilterContent(fromdate, todate, key, 1, idtype, 0, page, pagesize));
        }
        public ActionResult GetListDelay(string key, string fromdate, string todate, string idtype, int page = 1, int pagesize = 10)
        {
            ViewBag.page = page;
            ViewBag.pagesize = pagesize;
            ViewBag.key = key;
            ViewBag.fromdate = fromdate;
            ViewBag.todate = todate;
            ViewBag.idtype = idtype;
            return PartialView(new myfunc().FilterContent(fromdate, todate, key, 0, idtype, 0, page, pagesize));
        }

        public ActionResult ListDelay()
        {
            return View();
        }

        public ActionResult UpdateStatus(int Id)
        {
            new myfunc().UpdateStatus(Id);
            return Content("ok");
        }

        public ActionResult DeleteNews(int Id)
        {
            new myfunc().DeleteNews(Id);
            return Content("ok");
        }
        public ActionResult ShowUrlImage(int Id)
        {
            ViewBag.IdTypeImage = Id;
            return PartialView();
        }

        public async Task<JsonResult> AutoFilter(string url, string whichWeb)
        {
            tblNews tbl = new tblNews();
            HtmlDocument htmlDoc = new HtmlDocument();

            string htmlString = await GetHtmlAsync(url);

            if (htmlString != null && htmlString != "")
            {
                htmlDoc.LoadHtml(htmlString);
            }
            //Lấy tiêu đề, tóm tắt, ThumbFB
            tbl = GetMetaTag(htmlDoc);
            //Lấy AvatarThumb
            tbl.Avatar2x1 = GetAvatar(htmlDoc, whichWeb);
            //Lấy nội dung bài viết
            tbl.Body = GetBody(htmlDoc, whichWeb);


            return Json(new { Status = true, Tbl = tbl });
        }



        public string GetBody(HtmlDocument htmlDoc, string whichWeb)
        {
            string bodyHtml = "";
            switch (whichWeb)
            {
                case "yan":
                    bodyHtml = GetContentYan(htmlDoc);
                    break;
                case "bestie":
                    bodyHtml = GetContentBestie(htmlDoc);
                    break;
                case "dienanh":
                    bodyHtml = GetContentDAN(htmlDoc);
                    break;
                case "tiin":
                    bodyHtml = GetContentTiinVN(htmlDoc);
                    break;
                case "kenh14":
                    bodyHtml = GetContentKenh14(htmlDoc);
                    break;
                case "youtube":
                    bodyHtml = GetContentVideoYoutube(htmlDoc);
                    break;
            }

            return bodyHtml;
        }

        public tblNews GetMetaTag(HtmlDocument htmlDoc)
        {
            tblNews tbl = new tblNews();
            HtmlNodeCollection metaNodes = SelectMetaNodes("meta", "property", "og:", htmlDoc);

            if (metaNodes != null)
            {
                foreach (HtmlNode node in metaNodes)
                {
                    if (node.Attributes["property"].Value.ToLower() == "og:title")
                    {
                        //Giải mã các kí tự " & < >... có trong title
                        tbl.Title = WebUtility.HtmlDecode(node.Attributes["content"].Value);
                    }
                    if (node.Attributes["property"].Value.ToLower() == "og:description")
                    {
                        tbl.Teaser = WebUtility.HtmlDecode(node.Attributes["content"].Value);
                    }
                    if (node.Attributes["property"].Value.ToLower() == "og:image")
                    {
                        tbl.Avatar2x1 = node.Attributes["content"].Value;
                        tbl.AvatarFB = node.Attributes["content"].Value;
                    }
                }
            }
            return tbl;
        }

        public string GetContentTiinVN(HtmlDocument htmlDoc)
        {
            string bodyContent = "";
            HtmlNode bodyNode = SelectSingleNode("div", "class", "detail-content", htmlDoc);
            HtmlNode bodyVideoNode = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class,'video-page')]//div[@class='col-md-7 top']");

            if (bodyNode != null)
            {
                //Bỏ quảng cáo, tin tức liên quan...
                //...
                RemoveScript(bodyNode);
                RemoveChild("div", "class", "more-detail bg_cms_vang", bodyNode);

                //Chú thích giữa hình ảnh
                HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//p[@class='p-chuthich']");
                if (nodes != null)
                {
                    foreach (var item in nodes)
                    {
                        item.SetAttributeValue("style", "text-align:center;");
                    }
                }

                //Lấy hình ảnh trang tiin
                HtmlNodeCollection nodeImages = htmlDoc.DocumentNode.SelectNodes("//p//img");
                if (nodeImages != null)
                {
                    foreach (HtmlNode node in nodeImages)
                    {
                        node.Attributes.Add("src", node.Attributes["data-src"].Value);
                    }
                }


                bodyContent = bodyNode.InnerHtml;
            }

            return bodyContent;
        }

        public string GetContentKenh14(HtmlDocument htmlDoc)
        {
            string bodyContent = "";
            HtmlNode bodyNode = SelectSingleNode("div", "class", "knc-content", htmlDoc);
            if (bodyNode != null)
            {
                //Bỏ quảng cáo, tin tức liên quan...
                //...
                RemoveScript(bodyNode);
                RemoveChild("span", "id", "hdExclusive", bodyNode);

                //Chú thích giữa hình ảnh
                HtmlNodeCollection nodeCenterText = htmlDoc.DocumentNode.SelectNodes("//div[@class='PhotoCMS_Caption']//p");
                if (nodeCenterText != null)
                {
                    foreach (var item in nodeCenterText)
                    {
                        item.SetAttributeValue("style", "text-align:center;");
                    }
                }

                bodyContent = bodyNode.InnerHtml;
            }
            return bodyContent;
        }

        public string GetContentVideoYoutube(HtmlDocument htmlDoc)
        {
            string bodyContent = "";
            HtmlNode nodeUrlLink = htmlDoc.DocumentNode.SelectSingleNode("//meta[@property='og:url']");
            if (nodeUrlLink != null)
            {
                string urlLink = nodeUrlLink.Attributes["content"].Value; ;
                bodyContent = string.Format(myCommon.patternVideoYoutube, urlLink);
            }
            return bodyContent;
        }

        public string GetContentYan(HtmlDocument htmlDoc)
        {
            string bodyContent = "";
            HtmlNode bodyNode = SelectSingleNode("div", "id", "contentBody", htmlDoc);
            if (bodyNode != null)
            {
                //Bỏ quảng cáo, tin tức liên quan...
                //...
                RemoveScript(bodyNode);
                RemoveChild("img", "class", "imgShareLink", bodyNode);
                RemoveChild("img", "class", "imgShareFB", bodyNode);

                bodyContent = bodyNode.InnerHtml;
            }

            return bodyContent;
        }

        public string GetContentBestie(HtmlDocument htmlDoc)
        {
            string bodyContent = "";
            HtmlNode bodyNode = SelectSingleNode("div", "id", "postContent", htmlDoc);
            if (bodyNode != null)
            {
                //Bỏ quảng cáo, tin tức liên quan...
                //...
                RemoveScript(bodyNode);
                RemoveStyle(bodyNode);
                RemoveChild("div", "class", "content-video-caption", bodyNode);
                RemoveChild("p", "id", "myBtn", bodyNode);

                var nodeImage = bodyNode.SelectNodes("//div[@class='content-img']");
                if (nodeImage != null)
                {
                    //Đổi element div sang figure vì dùng div cuối ckeditor sẽ thừa breakline
                    foreach (HtmlNode node in nodeImage)
                    {
                        node.Name = "figure";
                    }
                }

                bodyContent = bodyNode.InnerHtml;
            }
            return bodyContent;
        }
        public string GetContentDAN(HtmlDocument htmlDoc)
        {
            string bodyContent = "";
            HtmlNode bodyNode = SelectSingleNode("div", "id", "body-content", htmlDoc);
            if (bodyNode != null)
            {
                //Bỏ quảng cáo, tin tức liên quan...
                //...
                RemoveScript(bodyNode);
                RemoveChild("div", "class", "info-box", bodyNode);

                bodyContent = bodyNode.InnerHtml;
            }
            return bodyContent;
        }

        public string GetAvatar(HtmlDocument htmlDoc, string whichWeb)
        {
            string avatar = "";
            switch (whichWeb)
            {
                case "yan":
                    avatar = GetAvatarYan(htmlDoc);
                    break;
                case "bestie":
                    avatar = GetAvatarBestie(htmlDoc);
                    break;
                case "dienanh":
                    avatar = GetAvatarDienAnh(htmlDoc);
                    break;
                case "tiin":
                    avatar = GetAvatarTiin(htmlDoc);
                    break;
                case "kenh14":
                    avatar = GetAvatarKenh14(htmlDoc);
                    break;
                    
            }
            return avatar;
        }

        public string GetAvatarYan(HtmlDocument htmlDoc)
        {
            string avatar = "";
            HtmlNode node = htmlDoc.DocumentNode.SelectSingleNode("//script[contains(., 'ImageObject')]");
            if (node != null)
            {
                var splitQuote = node.InnerText.Split('"');
                foreach (string str in splitQuote)
                {
                    if (str.Contains("YanThumbNews"))
                    {
                        avatar = str;
                    }
                }
            }

            return avatar;
        }

        public string GetAvatarBestie(HtmlDocument htmlDoc)
        {
            string avatar = "";
            HtmlNode node = htmlDoc.DocumentNode.SelectSingleNode("//meta[@property='dable:image']");
            if (node != null)
            {
                avatar = node.Attributes["content"].Value;
            }
            return avatar;
        }

        public string GetAvatarDienAnh(HtmlDocument htmlDoc)
        {
            string avatar = "";
            HtmlNode node = htmlDoc.DocumentNode.SelectSingleNode("//script[contains(., 'ImageObject')]");
            if (node != null)
            {
                var splitQuote = node.InnerText.Split('"');
                foreach (string str in splitQuote)
                {
                    if (str.Contains("dienanh.net/upload"))
                    {
                        avatar = str;
                    }
                }
            }
            return avatar;
        }

        public string GetAvatarTiin(HtmlDocument htmlDoc)
        {
            string avatar = "";
            HtmlNode node = htmlDoc.DocumentNode.SelectSingleNode("//meta[@property='og:image']");
            if (node != null)
            {
                avatar = node.Attributes["content"].Value;
            }
            return avatar;
        }

        public string GetAvatarKenh14(HtmlDocument htmlDoc)
        {
            string avatar = "";
            HtmlNode node = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='adm_meta_thumb_500']");
            if (node != null)
            {
                avatar = node.Attributes["value"].Value;
            }
            return avatar;
        }



        public async Task<string> GetHtmlAsync(string url)
        {
            using (var request = new HttpClient())
            {
                request.DefaultRequestHeaders.Add("User-Agent", "C# program");
                return await request.GetStringAsync(url);
            }
        }

        public void RemoveChild(string element, string attribute, string name, HtmlNode HtmlDocument)
        {
            HtmlNodeCollection nodes = HtmlDocument.SelectNodes($"//{element}[@{attribute}='{name}']");
            if (nodes != null)
            {
                foreach (HtmlNode node in HtmlDocument.SelectNodes($"//{element}[@{attribute}='{name}']"))
                {
                    node.Remove();
                }
            }
        }

        public HtmlNode SelectSingleNode(string element, string attribute, string name, HtmlDocument HtmlDocument)
        {
            return HtmlDocument.DocumentNode.SelectSingleNode($"//{element}[@{attribute}='{name}']");
        }
        public HtmlNodeCollection SelectMetaNodes(string element, string attribute, string name, HtmlDocument HtmlDocument)
        {
            return HtmlDocument.DocumentNode.SelectNodes($"//{element}[contains(@{attribute},'{name}')]");
        }

        public void RemoveScript(HtmlNode htmlNode)
        {
            HtmlNodeCollection nodeScripts = htmlNode.SelectNodes("//script");
            if (nodeScripts != null)
            {
                foreach (HtmlNode node in nodeScripts)
                {
                    node.Remove();
                }
            }
        }
        public void RemoveStyle(HtmlNode htmlNode)
        {
            HtmlNodeCollection nodeScripts = htmlNode.SelectNodes("//style");
            if (nodeScripts != null)
            {
                foreach (HtmlNode node in nodeScripts)
                {
                    node.Remove();
                }
            }
        }

        public void ReplaceElement(HtmlNode htmlNode)
        {

        }


        public ActionResult ListTemp()
        {
            return View();
        }

        public ActionResult GetListTemp(string key, string fromdate, string todate, string idtype, int page = 1, int pagesize = 10)
        {
            int idUser = MyAuthorize.GetLoggedUser(HttpContext).Id;
            return PartialView(new myfunc().FilterContent(fromdate, todate, key, -1, idtype, idUser, page, pagesize));
        }
    }
}