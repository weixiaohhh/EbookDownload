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
using HtmlAgilityPack;

namespace EbookDownload.views
{
    /// <summary>
    /// ePUBeeResultControl.xaml 的交互逻辑
    /// </summary>
    public partial class ZhiXuanResultControl : UserControl, BookInterface
    {
        public List<ePUBeeItem> itemList = new List<ePUBeeItem>();
        BackgroundWorker worker = new BackgroundWorker();
        public ZhiXuanResultControl()
        {
            InitializeComponent();

            //  this.listView.Items.Add(new MyItem { title = "dasd", BID = "David", Addr= "https://bbs.csdn.net/topics/390125323" ,imgSource = new BitmapImage(new Uri("http://files.epubee.com/getCover.ashx?fpath=6c/6ca735ffa7a27376187e38c23eda836c_s.jpg" , UriKind.Absolute)) });


            worker.DoWork += (s, e) =>
            {
                List<string> item = e.Argument as List<string>;
                Uri uri = new Uri(item[4], UriKind.Absolute);
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

                        ZhiXuanItem m = new ZhiXuanItem
                        {
                            title = item[0],
                            desc = item[1],

                            sort = item[2],

                            sortHref = item[3],

                            imgSource = image,

                            Addr = item[5]
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

               // BitmapImage bitmapImage = e.Result as BitmapImage;

                //if (bitmapImage != null)
                //{

                //}
                worker.Dispose();
            };
        }

        //下载地址
        private void HypeLink1_Click(object sender, RoutedEventArgs e)
        {



        }
        public void spider(string url)
        {
            Spider s = new Spider();

            HtmlDocument doc = new HtmlDocument();
            Task.Run(async () =>
            {
                string content = await s.GetResponse(url, "get");

                doc.LoadHtml(content);

                var data = doc.DocumentNode.SelectNodes(@"//*[@id='plist']");
                for (int i = 0; i < data.Count; i++)
                {

                    while (worker.IsBusy)
                    {
                        Thread.Sleep(10);
                    }

                    string t = data[i].SelectNodes(@".//dt/a")[0].InnerText;
                    string href = data[i].SelectNodes(@".//dt/a")[0].Attributes["href"].Value;
                    string desc = data[i].SelectNodes(@".//*[@class='des']")[0].InnerText.Trim();
                    string sort = data[i].SelectNodes(@".//dd/a")[0].InnerText;
                    string sortHref = data[i].SelectNodes(@".//dd/a")[0].Attributes["href"].Value;
                    string id = href.Split('/')[href.Split('/').Length - 1];

                    content = await s.GetResponse(href, "get");
                    HtmlDocument imgDoc = new HtmlDocument();
                    imgDoc.LoadHtml(content);
                    string imgUrl = imgDoc.DocumentNode.SelectNodes(@"//html/body/div[4]/div[2]/div[2]/a/img")[0].Attributes["src"].Value;

                    List<string> item = new List<string>()
                    {
                         t,
                        desc,
                        sort,
                        sortHref,
                        imgUrl,
                        "http://www.zxcs.me/download.php?id=" + id
                    };

                    worker.RunWorkerAsync(item);
                }
            }
            );
        }


        public void run(string title)
        {
            listView.Items.Clear();
            string url = "http://www.zxcs.me/index.php?keyword=" + title;
            spider(url);
        }



        private void downloadLink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = sender as Hyperlink;
            string url = link.NavigateUri.AbsoluteUri;
            Task.Run(async () =>
            {

                Spider s = new Spider();
                var content = await s.GetResponse(url, "get");
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(content);
                string downloadUrl = doc.DocumentNode.SelectNodes(@"//html/body/div[2]/div[2]/div[3]/div[2]/span[1]/a")[0].Attributes["href"].Value;

                try
                {

                    Process.Start(new ProcessStartInfo(downloadUrl));
                }
                catch (Exception ex)    //容错处理 - 用户没有设置默认浏览器时-则使用IE浏览器打开
                {
                    //调用IE浏览器  
                    Process.Start("firefox.exe", downloadUrl);
                }
            });
         
        }

        private void TypeLink_Click(object sender, RoutedEventArgs e)
        {
            listView.Items.Clear();
            Hyperlink link = sender as Hyperlink;

            spider(link.NavigateUri.ToString());
        }
    }

    public class ZhiXuanItem
    {
        public string title { get; set; }
        public string desc { get; set; }

        public string sort { get; set; }

        public string sortHref { get; set; }

        public BitmapImage imgSource { get; set; }

        public string Addr { get; set; }


    }

}
