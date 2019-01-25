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
    public partial class ePUBeeResultControl : UserControl,BookInterface
    {
        public List<ePUBeeItem> itemList = new List<ePUBeeItem>();
        BackgroundWorker worker = new BackgroundWorker();
        public ePUBeeResultControl()
        {
            InitializeComponent();

         //  this.listView.Items.Add(new MyItem { title = "dasd", BID = "David", Addr= "https://bbs.csdn.net/topics/390125323" ,imgSource = new BitmapImage(new Uri("http://files.epubee.com/getCover.ashx?fpath=6c/6ca735ffa7a27376187e38c23eda836c_s.jpg" , UriKind.Absolute)) });

           
            worker.DoWork += (s, e) =>
            {
                List<string> item = e.Argument as List<string>;
                Uri uri = new Uri(item[3], UriKind.Absolute);
                using (WebClient webClient = new WebClient())
                {
                    webClient.Proxy = null;  //avoids dynamic proxy discovery delay
                    webClient.CachePolicy = new RequestCachePolicy(RequestCacheLevel.Default);
                    try
                    {
                        byte[] imageBytes = null;

                        imageBytes = webClient.DownloadData(uri);

                        if (imageBytes == null)
                        {
                            e.Result = null;
                            return;
                        }
                        MemoryStream imageStream = new MemoryStream(imageBytes);
                        BitmapImage image = new BitmapImage();

                        image.BeginInit();
                        image.StreamSource = imageStream;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.EndInit();

                        image.Freeze();
                        imageStream.Close();

                        e.Result = image;

                        ePUBeeItem m = new ePUBeeItem
                        {
                            title = item[0],
                            BID = item[1],
                            Addr = item[2],
                            imgSource = image
                        };
                        Action ac = () =>
                        {
                            listView.Items.Add(m);
                        };
                         Application.Current.Dispatcher.BeginInvoke(ac);
                    }
                    catch (WebException ex)
                    {
                        //do something to report the exception
                        e.Result = ex;
                    }
                }
            };


            worker.RunWorkerCompleted += (s, e) =>
            {
               
                BitmapImage bitmapImage = e.Result as BitmapImage;
 
                if (bitmapImage != null)
                {
                   
                }
                worker.Dispose();
            };
        }

        private void HypeLink1_Click(object sender, RoutedEventArgs e)
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

        public void run(string title)
        {
            listView.Items.Clear();

            Spider s = new Spider();
            ePUBeeParam p = new ePUBeeParam();
            p.skey = title;


            string url = "http://cn.epubee.com/keys/get_ebook_list_search.asmx/getSearchList";
            Task.Run(async () =>
            {

                string content = await s.GetResponse(url, "post", p);
                var _json = JsonConvert.DeserializeObject(content);
                JObject o = _json as JObject;
                foreach (var r in o["d"])
                {
                    List<string> item = new List<string>()
                    { 
                        r["Title"].ToString(),
                         r["BID"].ToString(),
                        "http://cn.epubee.com/",
                        "http://files.epubee.com/getCover.ashx?fpath=" + r["Cover"].ToString()
                    };
                    while(worker.IsBusy)
                    {
                        Thread.Sleep(10);
                    }
                    worker.RunWorkerAsync(item);
                }             
            }
            );
        }
    }


    class ePUBeeParam
    {
        public string skey { get; set; }
    }

    public class ePUBeeItem
    {
        public string title { get; set; }

        public BitmapImage imgSource { get; set; }

        public string Addr { get; set; }

        public string BID { get; set; }
    }
}
