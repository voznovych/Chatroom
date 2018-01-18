using BLL;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
//using System.Windows.Controls;
using System.Windows.Media;

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

            const int count = 9;

            RoomDTO[] roomArr = new RoomDTO[count];

            for (int i = 0; i < count; i++)
            {
                roomArr[i] = new RoomDTO()
                {
                    Name = "Test Room",
                    Photo = Util.ByteArrayToImage(File.ReadAllBytes("Resources/avatar_m.png"))
                };
            }

            MessageDTO[] msArr = new MessageDTO[count];

            for (int i = 0; i < count; i++)
            {
                msArr[i] = new MessageDTO()
                {
                    Text = "Hello pidor andty, ya tia diuznu hhhnfhf ghfhhfhh fgghff hffdsf",
                    DateOfSend = DateTime.Now,
                    User = new UserDTO() { Name = "Vlad" }
                };
            }

            RoomInfo[] riArr = new RoomInfo[count];
            Random rnd = new Random();

            for (int i = 0; i < count; i++)
            {
                riArr[i] = new RoomInfo()
                {
                    Room = roomArr[i],
                    Message = msArr[i],
                    UnreadMessages = rnd.Next(3)
                };
            }

            RoomInfoBox[] ribArr = new RoomInfoBox[count];

            for (int i = 0; i < count; i++)
            {
                ribArr[i] = new RoomInfoBox()
                {
                    DataContext = riArr[i]
                };
                //roomsListBox.Items.Add(ribArr[i]);
            }

            roomsListBox.ItemsSource = ribArr;
        }
    }

    class RoomInfo : INotifyPropertyChanged
    {
        public RoomDTO Room { get; set; }
        public MessageDTO Message { get; set; }
        private int _unreadMessages;
        private ImageSource _photo;

        public int? UnreadMessages
        {
            get { return _unreadMessages == 0 ? null : (int?)_unreadMessages; }

            set
            {
                _unreadMessages = value.Value;
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

        public string SenderName
        {
            get { return Message.User.Name; }
            set
            {
                Message.User.Name = value;
                OnPropertyChanged("SenderName");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
