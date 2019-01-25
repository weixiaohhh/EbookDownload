using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

using MahApps.Metro.Controls;
using EbookDownload.views;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Cache;
using System.ComponentModel;
using System.IO;
using System.Threading;
namespace EbookDownload.views
{
    /// <summary>
    /// ePUBeeResultControl.xaml 的交互逻辑
    /// </summary>
    public partial class JiuMoResultControl : UserControl,BookInterface
    {
        public List<JiuMoItem> itemList = new List<JiuMoItem>();
        BackgroundWorker worker = new BackgroundWorker();
        public JiuMoResultControl()
        {
            InitializeComponent();

         //  this.listView.Items.Add(new MyItem { title = "dasd", BID = "David", Addr= "https://bbs.csdn.net/topics/390125323" ,imgSource = new BitmapImage(new Uri("http://files.epubee.com/getCover.ashx?fpath=6c/6ca735ffa7a27376187e38c23eda836c_s.jpg" , UriKind.Absolute)) });

           
        


        }



        private long ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (long)((DateTime.Now - startTime).TotalMilliseconds);
        }


        public void run(string title)
        {
            listView.Items.Clear();
            string cookies = @"_ga=GA1.2.1404969523.1547112556|_gat=1|_gid=GA1.2.956377130.1548293901|loaded=12";
            Spider s = new Spider(cookies);

            Task.Run(async () =>
            {

                var formVariables = new List<KeyValuePair<string, string>>();
                formVariables.Add(new KeyValuePair<string, string>("q", title));
                formVariables.Add(new KeyValuePair<string, string>("remote_ip", ""));
                formVariables.Add(new KeyValuePair<string, string>("time_init", ConvertDateTimeInt(DateTime.Now).ToString()));

                s.ContentType = "application/x-www-form";
                string url = "https://www2.jiumodiary.com/init_hubs.php";
                string content = await s.GetResponse(url, "post", formVariables, "https://www.jiumodiary.com/","form");
                var _json = JsonConvert.DeserializeObject(content);
                JObject o = _json as JObject;
                string id = o["id"].ToString();

                formVariables.Clear();
                formVariables.Add(new KeyValuePair<string, string>("id", id));
                formVariables.Add(new KeyValuePair<string, string>("set", "0"));
                url = "https://www.jiumodiary.com/ajax_fetch_hubs.php";

                content = await s.GetResponse(url, "post", formVariables, "https://www.jiumodiary.com/", "form");
                _json = JsonConvert.DeserializeObject(content);
                 o = _json as JObject;
                foreach (var source in o["sources"])
                {
                    foreach(var data in source["details"]["data"])
                    {
                        try
                        {
                            string Addr = data["link"].ToString();
                            string a = data["title"].ToString();
                            string des = data["des"].ToString();
                            string host = data["host"].ToString();
                        }
                        catch(Exception e)
                        {

                        }
                        JiuMoItem d = new JiuMoItem()
                        {
                            Addr = data["link"].ToString(),
                            title = data["title"].ToString(),
                            desc = data["des"].ToString(),
                            host = data["host"].ToString()
                        };
                        Action ac = () =>
                        {
                            listView.Items.Add(d);
                        };
                       await Application.Current.Dispatcher.BeginInvoke(ac);
                    }
                    
                }  
                
                              
            }
            );
        }



        private void HypeLink1_Click_1(object sender, RoutedEventArgs e)
        {
            Hyperlink link = sender as Hyperlink;
            try
            {

                Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
            }
            catch (Exception ex)    //容错处理 - 用户没有设置默认浏览器时-则使用IE浏览器打开
            {
                //调用IE浏览器  
                Process.Start("firefox.exe", link.NavigateUri.AbsoluteUri);
            }
        }
    }

    public class JiuMoParam1
    {
       public string q { get; set; }
        public string remote_ip { get; set; }
        public string time_init { get; set; }
    }

    public class JiuMoItem
    {
        public string title { get; set; }

        public string desc { get; set; }

        public string Addr { get; set; }

        public string host { get; set; }
    }
}
