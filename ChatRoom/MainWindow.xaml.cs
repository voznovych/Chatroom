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

namespace ChatRoom
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //new window
        public MainWindow()
        {
            InitializeComponent();
            MyControl.SetNewAmountOfMessages(0);
            MyControl.Avatar.ImageSource = new BitmapImage(new Uri(@"C:\Users\Win7\Desktop\arts\364955.jpg"));
            MyControl.UnreadedMsgs.Text = "";
        }

        int amOfMsg = 0;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            amOfMsg += 100;
            MyControl.SetNewAmountOfMessages(amOfMsg);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            amOfMsg -= 200;
            MyControl.Msgs = amOfMsg.ToString();
        }
    }
}
