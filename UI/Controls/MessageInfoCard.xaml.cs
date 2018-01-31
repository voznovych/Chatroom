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
using BLL;
using UI.ViewModels;

namespace UI
{
    /// <summary>
    /// Interaction logic for MessageInfoCard.xaml
    /// </summary>
    public partial class MessageInfoCard : UserControl
    {
        private MessageInfViewModel _message;

        public MessageInfViewModel Message => _message;

        public MessageInfoCard(MessageInfViewModel message, bool isFromMe)
        {
            InitializeComponent();
            _message = message;

            DataContext = _message;

            if (isFromMe)
            {
                gridFromSomebody.Visibility = Visibility.Collapsed;
                gridFromMe.Visibility = Visibility.Visible;
            }
            else
            {
                gridFromMe.Visibility = Visibility.Collapsed;
                gridFromSomebody.Visibility = Visibility.Visible;
            }
        }
    }
}
