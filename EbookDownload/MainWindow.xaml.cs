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
using MahApps.Metro.Controls;
using EbookDownload.views;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace EbookDownload
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public static ObservableCollection<TabItem> ou = new ObservableCollection<TabItem>();

        public  MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            addBookResource("鸠摩搜书",new JiuMoResultControl());
            addBookResource("ePUBee",new ePUBeeResultControl());
            addBookResource("知轩藏书", new ZhiXuanResultControl());
            tabControl.ItemsSource = ou;

        }

        public void addBookResource(string Header,object o)
        {

            TabItem t = new TabItem();
            t.Header = Header;
            t.Content = o;
            ou.Add(t);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {        

        }

        private void toggleButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach(var item in ou)
            {
                ((BookInterface)item.Content).run(textBox.Text);
            }
        }
    }


}
