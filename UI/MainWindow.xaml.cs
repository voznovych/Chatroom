using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
//using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BLLClass _bll;

        public MainWindow(BLLClass bll)
        {
            InitializeComponent();
            _bll = bll;

            RoomDTO room = new RoomDTO()
            {
                Name = "Test Room",
                Photo = Util.ByteArrayToImage(File.ReadAllBytes("Resources/avatar_m.png"))
            };

            MessageDTO ms = new MessageDTO()
            {
                Text = "Hello pidor andty, ya tia diuznu",
                DateOfSend = DateTime.Now,
            };

            RoomInfo ri = new RoomInfo()
            {
                Room = room,
                Message = ms,
                UnreadMessages = 1
            };

            RoomInfoBox rib = new RoomInfoBox()
            {
                DataContext = ri
            };

            roomsListBox.Items.Add(rib);
        }
    }

    class RoomInfo : INotifyPropertyChanged
    {
        public RoomDTO Room { get; set; }
        public MessageDTO Message { get; set; }
        private int _unreadMessages;
        private ImageSource _photo;

        public int UnreadMessages
        {
            get { return _unreadMessages; }
            set
            {
                _unreadMessages = value;
                OnPropertyChanged("UnreadMessages");
            }
        }

        public string Text
        {
            get { return Message.Text; }
            set
            {
                Message.Text = value;
                OnPropertyChanged("Text");
            }
        }

        public string DateOfSend
        {
            get { return Message.DateOfSend.Day == DateTime.Now.Day ? Message.DateOfSend.ToShortTimeString() : Message.DateOfSend.ToShortDateString(); }
            set
            {
                //Message.DateOfSend = value;
                OnPropertyChanged("DateOfSend");
            }
        }

        public ImageSource Photo
        {
            get { return Util.ImageToImageSource(Room.Photo); }
            set
            {
                _photo = value;
                OnPropertyChanged("Photo");
            }
        }

        public string Name
        {
            get { return Room.Name; }
            set
            {
                Room.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Sender
        {
            get { return $"{Message.User.Name} {Message.User.Surname}"; }
            set
            {
                Room.Name = value;
                OnPropertyChanged("Sender");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
