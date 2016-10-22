using GlobalUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using LinqDb;

namespace reExp.Utils
{
    public class Search
    {
        public static void PutUserItem(UsersItem item)
        {
            //try
            //{ 
            //    JavaScriptSerializer ser = new JavaScriptSerializer();
            //    ser.MaxJsonLength = Int32.MaxValue;
            //    string data = ser.Serialize(item);
            //    using (var client = new WebClient())
            //    {
            //        client.Encoding = Encoding.UTF8;
            //        client.UploadString(TopSecret.ElasticUrl + item.UserId + "/" + item.ID, "PUT", data);
            //    }
            //}
            //catch (Exception e)
            //{
            //    Log.LogInfo(e.Message, "error while putting to search index");
            //}
            try
            {
                Utils.search_db.Table<UsersItem>().Save(item);
            }
            catch (Exception e)
            {
                Log.LogInfo(e.Message, "error while putting to search index");
            }
        }

        public static void DeleteUserItem(int UserId, string Id)
        {
            //try
            //{
            //    using (var client = new WebClient())
            //    {
            //        client.Encoding = Encoding.UTF8;
            //        client.UploadString(TopSecret.ElasticUrl + UserId + "/" + Id, "DELETE", "");
            //    }
            //}
            //catch (Exception e)
            //{
            //    Log.LogInfo(e.Message, "error while deleting search index");
            //}
            try
            {
                var item = Utils.search_db.Table<UsersItem>().Where(f => f.UserId == UserId && f.ID == Id).SelectEntity().FirstOrDefault();
                if (item == null)
                {
                    throw new Exception("Item not found");
                }
                Utils.search_db.Table<UsersItem>().Delete(item.Id);
            }
            catch (Exception e)
            {
                Log.LogInfo(e.Message, "error while deleting search index");
            }
        }

        public static List<SearchResult> MakeSearch(string query, int UserId)
        {
            var result = new List<SearchResult>();

            var tmp = Utils.search_db.Table<UsersItem>().Where(f => f.UserId == UserId);
            foreach (var part in query.Split().Where(f => !string.IsNullOrEmpty(f)))
            {
                tmp.Search(f => f.Title, part, true).Or().Search(f => f.Code, part, true)
                   .Or().Search(f => f.Regex, part, true).Or().Search(f => f.Replace, part, true);
            }

            return tmp.SelectEntity().ToList()
                      .Select(f => new SearchResult
                      {
                          ID = f.ID,
                          Title = f.Title,
                          Code = f.Code,
                          Lang = f.Lang,
                          Guid = f.Guid,
                          Score = 0,
                          Regex = f.Regex,
                          Replace = f.Replace,
                          Date = f.Date,
                          IsLive = f.IsLive == 1 ? true : false
                      })
                      .ToList();

            //            using (var client = new MyWebClient())
            //            {
            //                client.Encoding = Encoding.UTF8;
            //                var data =
            //@"{
            //    ""query"": {
            //        ""function_score"": {
            //            ""query"": {
            //                ""multi_match"" : {
            //                     ""query"" : """ + query + @""",
            //                     ""type"" : ""phrase"",
            //                     ""slop"" : 15,
            //                     ""fields"": [""Code^1"", ""Regex^1"", ""Replace^1"", ""Text^1"", ""Title^2"", ""Lang^2""]
            //                }
            //            }
            //        }
            //    },
            //    ""highlight"" : {
            //        ""order"" : ""score"",
            //        ""fields"" : {
            //                ""Code"" : {""fragment_size"" : 200, ""number_of_fragments"" : 1, ""no_match_size"": 100}
            //        }
            //    }
            //};";

            //                string result = null;
            //                try
            //                {
            //                    result = client.UploadString(TopSecret.ElasticUrl + UserId + "/_search", data);
            //                }
            //                catch (Exception e)
            //                {
            //                    return new List<SearchResult>();
            //                }
            //                dynamic stuff = JsonConvert.DeserializeObject(result);
            //                var l = new List<SearchResult>();
            //                for (int i=0; i< stuff.hits.hits.Count; i++)
            //                {
            //                    var res = new SearchResult()
            //                    {
            //                        ID = stuff.hits.hits[i]._source.ID,
            //                        Title = stuff.hits.hits[i]._source.Title,
            //                        Code = stuff.hits.hits[i]._source.Code,
            //                        Lang = stuff.hits.hits[i]._source.Lang,
            //                        Guid = stuff.hits.hits[i]._source.Guid,
            //                        Score = stuff.hits.hits[i]._score,
            //                        Regex = stuff.hits.hits[i]._source.Regex,
            //                        Replace = stuff.hits.hits[i]._source.Replace,
            //                        Date = stuff.hits.hits[i]._source.Date,
            //                        IsLive = stuff.hits.hits[i]._source.IsLive
            //                    };
            //                    if (stuff.hits.hits[i].highlight != null)
            //                    {
            //                        for (int j = 0; j < stuff.hits.hits[i].highlight.Code.Count; j++)
            //                        {
            //                            res.Highlight += stuff.hits.hits[i].highlight.Code[j] + "... ";
            //                        }
            //                    }
            //                    l.Add(res);
            //                }
            //                return l;
            //            }
        }
    }

    public class UsersItem
    {
        public int Id { get; set; }
        public string ID { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string Lang { get; set; }
        public string Guid { get; set; }
        public string Regex { get; set; }
        public string Replace { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int? IsLive { get; set; }
    }

    public class SearchResult
    { 
        public string ID { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string Lang { get; set; }
        public string Guid { get; set; }
        public string Regex { get; set; }
        public string Replace { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string Highlight { get; set; }
        public double Score { get; set; }
        public bool? IsLive { get; set; }
    }

    [System.ComponentModel.DesignerCategory("")]
    public class MyWebClient : WebClient
    {
        private int _timeout;
        /// <summary>
        /// Time in seconds
        /// </summary>
        public int Timeout
        {
            get
            {
                return _timeout;
            }
            set
            {
                _timeout = value;
            }
        }

        public MyWebClient()
        {
            this._timeout = 60;
        }
        /// <summary>
        /// Time in seconds
        /// </summary>
        //public MyWebClient(int timeout)
        //{
        // this._timeout = timeout;
        //}

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            //request.Timeout = this._timeout * 1000;
            request.ServicePoint.ConnectionLimit = Int32.MaxValue;
            return request;
        }

    }
}