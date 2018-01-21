using BLL;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
//using UI.Controls;
using System.Windows.Media;
using UI.Controls;
using UI.Domain;

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

            roomsListBox.DataContext = new RoomViewModel();
            messageListBox.DataContext = new MessageViewModel();
        }

        public static RoomInfoBox[] GetRoomInfoBoxArr(int count)
        {
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
                    UnreadMessages = rnd.Next(5)
                };
            }

            RoomInfoBox[] ribArr = new RoomInfoBox[count];
            for (int i = 0; i < count; i++)
            {
                ribArr[i] = new RoomInfoBox()
                {
                    DataContext = riArr[i]
                };
            }

            return ribArr;
        }
        public static MessageInfoCard[] GetMessageInfoCardArr(int count)
        {
            MessageDTO[] msArr = new MessageDTO[count];
            msArr[0] = new MessageDTO()
            {
                Text = "Hello !!!",
                DateOfSend = DateTime.Now,
                User = new UserDTO()
                {
                    Name = "Vitya",
                    Photo = Util.ByteArrayToImage(File.ReadAllBytes("Resources/avatar_m.png"))
                }
            };
            for (int i = 1; i < count - 1; i++)
            {
                msArr[i] = new MessageDTO()
                {
                    Text = "Hello pidor andty, ya tia diuznu hhhnfhf ghfhhfhh fgghff hffdsf",
                    DateOfSend = DateTime.Now,
                    User = new UserDTO()
                    {
                        Name = "Vlad",
                        Photo = Util.ByteArrayToImage(File.ReadAllBytes("Resources/avatar_f.png"))
                    }
                };
            }
            msArr[count - 1] = new MessageDTO()
            {
                Text = ".",
                DateOfSend = DateTime.Now,
                User = new UserDTO()
                {
                    Name = "Andriu",
                    Photo = Util.ByteArrayToImage(File.ReadAllBytes("Resources/key_security.png"))
                }
            };

            MessageInfo[] miArr = new MessageInfo[count];
            for (int i = 0; i < count; i++)
            {
                miArr[i] = new MessageInfo()
                {
                    Message = msArr[i],
                };
            }

            MessageInfoCard[] micArr = new MessageInfoCard[count];
            for (int i = 0; i < count; i++)
            {
                micArr[i] = new MessageInfoCard()
                {
                    DataContext = miArr[i]
                };
            }

            return micArr;
        }
        public static MessageInfoCardFromMe GetMICFM()
        {
            var ms = new MessageDTO()
            {
                Text = "Hello pidor andty, ya tia diuznu hhhnfhf ghfhhfhh fgghff hffdsf",
                DateOfSend = DateTime.Now,
                User = new UserDTO()
                {
                    Name = "Vlad",
                    Photo = Util.ByteArrayToImage(File.ReadAllBytes("Resources/avatar_f.png"))
                }
            };


            MessageInfo mi = new MessageInfo()
            {
                Message = ms
            };


            MessageInfoCardFromMe micfm = new MessageInfoCardFromMe()
            {
                DataContext = mi
            };

            return micfm;
        }

        private void CreateRoomButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CreateRoomDialog(_bll);
            DialogHost.Show(dialog, "CreateRoomDialog");
        }
    }

    public class RoomViewModel
    {
        private ICollectionView _rooms;

        public ICollectionView Rooms
        {
            get { return _rooms; }
        }

        public RoomViewModel()
        {
            IList<RoomInfoBox> rooms = MainWindow.GetRoomInfoBoxArr(15);
            _rooms = CollectionViewSource.GetDefaultView(rooms);

            //_rooms.SortDescriptions.Add(
            //    new SortDescription("Prop", ListSortDirection.Ascending));
        }
    }

    public class MessageViewModel
    {
        private ICollectionView _messages;

        public ICollectionView Messages
        {
            get { return _messages; }
        }

        public MessageViewModel()
        {
            System.Collections.ArrayList messages = new System.Collections.ArrayList();
            messages.AddRange(MainWindow.GetMessageInfoCardArr(4));
            messages.Add(MainWindow.GetMICFM());
            messages.Add(MainWindow.GetMICFM());
            messages.AddRange(MainWindow.GetMessageInfoCardArr(3));
            messages.Add(MainWindow.GetMICFM());
            messages.AddRange(MainWindow.GetMessageInfoCardArr(6));
            messages.Add(MainWindow.GetMICFM());
            messages.Add(MainWindow.GetMICFM());

            _messages = CollectionViewSource.GetDefaultView(messages);

            //_rooms.SortDescriptions.Add(
            //    new SortDescription("Prop", ListSortDirection.Ascending));
        }
    }

    class RoomInfo : INotifyPropertyChanged
    {
        private RoomDTO _room;
        private MessageDTO _message;
        private int _unreadMessages;
        private ImageSource _photo;

        public RoomDTO Room
        {
            get { return _room; }
            set { _room = value; }
        }

        public MessageDTO Message
        {
            get { return _message; }
            set { _message = value; }
        }

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
            get { return _message.Text; }
            set
            {
                _message.Text = value;
                OnPropertyChanged("Text");
            }
        }

        public string DateOfSend
        {
            get { return _message.DateOfSend.Day == DateTime.Now.Day ? _message.DateOfSend.ToShortTimeString() : _message.DateOfSend.ToShortDateString(); }
            set
            {
                _message.DateOfSend = DateTime.Parse(value);
                OnPropertyChanged("DateOfSend");
            }
        }

        public ImageSource Photo
        {
            get { return Util.ImageToImageSource(_room.Photo); }
            set
            {
                _photo = value;
                OnPropertyChanged("Photo");
            }
        }

        public string Name
        {
            get { return _room.Name; }
            set
            {
                _room.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string SenderName
        {
            get { return _message.User.Name; }
            set
            {
                _message.User.Name = value;
                OnPropertyChanged("SenderName");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    class MessageInfo : INotifyPropertyChanged
    {
        private MessageDTO _message;

        public MessageDTO Message
        {
            get { return _message; }
            set { _message = value; }
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

        public ImageSource SenderPhoto
        {
            get { return Util.ImageToImageSource(_message.User.Photo); }
            set
            {
                //_photo = value;
                OnPropertyChanged("SenderPhoto");
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
                Message.DateOfSend = DateTime.Parse(value);
                OnPropertyChanged("DateOfSend");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
