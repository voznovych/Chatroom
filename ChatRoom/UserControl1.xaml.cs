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
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatRoom
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public ImageSource ChatAvatar { get; set; }


        private int _msgsCount = 0;
        bool IsPressed = false;
       




        public string Msgs
        {
            get { return (string)GetValue(MsgsProperty); }
            set { SetValue(MsgsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Msgs.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MsgsProperty =
            DependencyProperty.Register("Msgs", typeof(string), typeof(UserControl1), new PropertyMetadata(0));



        public UserControl1()
        {
            InitializeComponent();
            //this.DataContext = this;
        }

        public void SetNewAmountOfMessages(int amount)
        {
            if(amount > 9999)
            {
                UnreadedMsgs.Text = "+9999";
                BorderForUnreaded.Visibility = Visibility.Visible;

            }
            else if(amount == 0)
            {
                BorderForUnreaded.Visibility = Visibility.Hidden;
            }
            else
            {
                UnreadedMsgs.Text = amount.ToString();
                BorderForUnreaded.Visibility = Visibility.Visible;
            }
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsPressed = true;
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            IsPressed = false;
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(IsPressed)
            {
                MessageBox.Show("Ok");
                IsPressed = false;
            }
        }
    }
}
