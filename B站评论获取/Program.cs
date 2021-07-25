using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace B站评论获取
{
    class Program
    {
        static void Main(string[] args)
        {
            bool success = false;
            Console.WriteLine("输入AV号(BV无效)");
            string avid = Console.ReadLine().Replace("av","");
            Console.WriteLine("按什么排序(0:时间, 1:点赞数, 2:回复数)");
            string sort = Console.ReadLine();
            S:
            Console.WriteLine("输入你要看的页数");
            string pn = Console.ReadLine();

            Console.WriteLine("评论:\n");
            try
            {
                string data = (GetData(avid, pn, sort));
                Root data1 = JsonConvert.DeserializeObject<Root>(data);
                List<RepliesItem> reples = data1.data.replies;
                foreach (var commit in reples)
                {
                    RepliesItem text = commit;
                    List<RepliesItem> list = commit.replies;
                    string result = text.content.message;
                    string name = text.member.uname;
                    string level = text.member.level_info.current_level.ToString();
                    Console.WriteLine(name +" [LV."+level+ "] 说:\n" + result );
                    if (list.Count == 0)
                    {

                    }
                    else
                    {
                        foreach (var item in list)
                        {
                            string t = item.content.message;
                            Console.WriteLine(t);
                        }
                    }
                    Console.WriteLine("\n");
                    
                }
                success = true;
            }
            catch (Exception)
            {

                Console.WriteLine("读取失败:\n");
                success = false;
            }
            
            Console.ReadKey();
            if (success)
            {
                goto S;
            }
            
        }

        public static  string GetTotal(string oid)
        {
            Dictionary<string, string> da = new Dictionary<string, string>();
            da.Add("type", "1");
            da.Add("oid", oid);



            string result = HttpGet("http://api.bilibili.com/x/v2/reply/count", da);
            return result;
        }

        public static  string GetData(string oid,string pn,string sort)
        {
            Dictionary<string, string> da = new Dictionary<string, string>();
            da.Add("type", "1");
            da.Add("oid", oid);
            da.Add("pn", pn);
            da.Add("sort", sort);


            string result = HttpGet("http://api.bilibili.com/x/v2/reply", da);
            return result;
        }


        /// <summary>
        /// 发送Get请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="dic">请求参数定义</param>
        /// <returns></returns>
        public static string HttpGet(string url, Dictionary<string, string> dic)
        {
            string result = "";
            StringBuilder builder = new StringBuilder();
            builder.Append(url);
            if (dic.Count > 0)
            {
                builder.Append("?");
                int i = 0;
                foreach (var item in dic)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }
            }
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(builder.ToString());
            //添加参数
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            try
            {
                //获取内容
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            finally
            {
                stream.Close();
            }
            return result;
        }
    }

    public class Page
    {
        /// <summary>
        /// 
        /// </summary>
        public long num { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long acount { get; set; }
    }
    public class Config
    {
        /// <summary>
        /// 
        /// </summary>
        public long showadmin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long showentry { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long showfloor { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long showtopic { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string show_up_flag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string read_only { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string show_del_log { get; set; }
    }
    public class Level_info
    {
        /// <summary>
        /// 
        /// </summary>
        public long current_level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long current_min { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long current_exp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long next_exp { get; set; }
    }
    public class Pendant
    {
        /// <summary>
        /// 
        /// </summary>
        public long pid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string image { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long expire { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string image_enhance { get; set; }
    }
    public class Nameplate
    {
        /// <summary>
        /// 
        /// </summary>
        public long nid { get; set; }
        /// <summary>
        /// 字幕君
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string image { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string image_small { get; set; }
        /// <summary>
        /// 稀有勋章
        /// </summary>
        public string level { get; set; }
        /// <summary>
        /// 弹幕大赛获得
        /// </summary>
        public string condition { get; set; }
    }
    public class Official_verify
    {
        /// <summary>
        /// 
        /// </summary>
        public long type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string desc { get; set; }
    }
    public class Label
    {
        /// <summary>
        /// 
        /// </summary>
        public string path { get; set; }
    }
    public class Vip
    {
        /// <summary>
        /// 
        /// </summary>
        public long vipType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long vipDueDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dueRemark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long accessStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long vipStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string vipStatusWarn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long themeType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Label label { get; set; }
    }
    public class User_sailing
    {
        /// <summary>
        /// 
        /// </summary>
        public Pendant pendant { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public CardbgItem cardbg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cardbg_with_focus { get; set; }
    }

    public class CardbgItem
    {
        public long id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string jump_url { get; set; }
        public FanItem fan { get; set; }
        public string type { get; set; }
    }

    public class FanItem
    {
        public int is_fan { get; set; }
        public long number { get; set; }
        public string color { get; set; }
        public string name { get; set; }
        public string num_desc { get; set; }
    }

    public class Member
    {
        /// <summary>
        /// 
        /// </summary>
        public string mid { get; set; }
        /// <summary>
        /// 残星什么的就是残星
        /// </summary>
        public string uname { get; set; }
        /// <summary>
        /// 男
        /// </summary>
        public string sex { get; set; }
        /// <summary>
        /// 少说话多做事 _微博@残星
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string avatar { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rank { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DisplayRank { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Level_info level_info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Pendant pendant { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Nameplate nameplate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Official_verify official_verify { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Vip vip { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fans_detail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long following { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long is_followed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public User_sailing user_sailing { get; set; }
    }
    public class Jump_url
    {
    }
    public class Content
    {
        /// <summary>
        /// 貌似没人来
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long plat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string device { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Member> members { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Jump_url jump_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long max_line { get; set; }
    }
    
    public class Folder
    {
        /// <summary>
        /// 
        /// </summary>
        public string has_folded { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string is_folded { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rule { get; set; }
    }
    public class Up_action
    {
        /// <summary>
        /// 
        /// </summary>
        public string like { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reply { get; set; }
    }
    public class RepliesItem
    {
        /// <summary>
        /// 
        /// </summary>
        public long rpid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long oid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long mid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long root { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long parent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long dialog { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long rcount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long state { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long fansgrade { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long attr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long ctime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rpid_str { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string root_str { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string parent_str { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long like { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long action { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Member member { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Content content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public string replies { get; set; }

        public List<RepliesItem> replies { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long assist { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Folder folder { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Up_action up_action { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string show_follow { get; set; }
    }



    public class Upper
    {
        /// <summary>
        /// 
        /// </summary>
        public long mid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string top { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string vote { get; set; }
    }
    public class Notice
    {
        /// <summary>
        /// 
        /// </summary>
        public long id { get; set; }
        /// <summary>
        /// 你的钱包被什么掏空了？回血红包拿好！>>
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 你的钱包被什么掏空了？回血红包拿好！>>
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string link { get; set; }
    }
    
    public class Control
    {
        /// <summary>
        /// 
        /// </summary>
        public string input_disable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string root_input_text { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string child_input_text { get; set; }
        /// <summary>
        /// 看看下面~来发评论吧
        /// </summary>
        public string bg_text { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string web_selection { get; set; }
        /// <summary>
        /// 需要升级成为lv2会员后才可以评论，先去答题转正吧！
        /// </summary>
        public string answer_guide_text { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string answer_guide_icon_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string answer_guide_ios_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string answer_guide_android_url { get; set; }
    }
    public class Data
    {
        /// <summary>
        /// 
        /// </summary>
        public Page page { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Config config { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<RepliesItem> replies { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<RepliesItem> hots { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Upper upper { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string top { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Notice notice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long vote { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long blacklist { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long assist { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long mode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<long> support_mode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Folder folder { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lottery_card { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string show_bvid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Control control { get; set; }
    }
    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public long code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long ttl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Data data { get; set; }
    }


}
