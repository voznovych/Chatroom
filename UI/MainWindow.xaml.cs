using BLL;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using UI.Controls;
using UI.ViewModels;

namespace UI
{
    public delegate void RoomIBoxClick(RoomInfoBox roomInfoBox);
    public delegate void AddRoomClick(int roomId, bool isAdd);

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private properties
        public DispatcherTimer _refreshRoomsTimer;
        public DispatcherTimer _refreshMessagesTimer;
        public DispatcherTimer _refreshLastVisiDate;

        private readonly BLLClass _bll;
        private bool _isGlobalSearch;

        private RoomInfoBox _selectedRoom;

        private RoomsViewModel _roomsViewModel;
        private MessagesViewModel _messagesViewModel;
        private NewRoomsViewModel _newRoomsViewModel;
        #endregion

        #region Public properties
        public RoomInfoBox SelectedRoom
        {
            get { return _selectedRoom; }
            set
            {
                if (_selectedRoom != null)
                {
                    _selectedRoom.IsSelected = false;
                }

                SelectedRoomPhoto_Ellipse.DataContext = value.Room;
                SelectedRoomName_TB.DataContext = value.Room;
                EditRoom_Button.IsEnabled = _bll.IsIRoomCreator(value.Room.Room.Id);

                _refreshLastVisiDate.Stop();
                _refreshLastVisiDate.Start();

                messagesScroll.ScrollToBottom();

                _selectedRoom = value;
            }
        }
        public bool IsGlobalSearch
        {
            get { return _isGlobalSearch; }
            set
            {
                if (value)
                {
                    globalSearchButton.Foreground = this.FindResource("SecondaryAccentBrush") as Brush;
                    _isGlobalSearch = value;
                }
                else
                {
                    globalSearchButton.Foreground = this.FindResource("PrimaryHueDarkBrush") as Brush;
                    _isGlobalSearch = value;
                }
            }
        }
        #endregion

        public MainWindow(BLLClass bll)
        {
            InitializeComponent();
            _bll = bll;

            _roomsViewModel = new RoomsViewModel(Room_Click, _bll.GetUserInfo().Id);
            _roomsViewModel.Load(_bll.GetInfosAboutAllUserRooms());
            roomsList.DataContext = _roomsViewModel;

            _newRoomsViewModel = new NewRoomsViewModel(AddRoom_Click);

            _messagesViewModel = new MessagesViewModel(_bll.GetUserInfo().Id);
            messagesList.DataContext = _messagesViewModel;

            _refreshRoomsTimer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 6)
            };
            _refreshRoomsTimer.Tick += _refreshRoomsTimer_Tick;
            _refreshRoomsTimer.Start();

            _refreshMessagesTimer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 5)
            };
            _refreshMessagesTimer.Tick += _refreshMessagesTimer_Tick;
            _refreshMessagesTimer.Start();

            _refreshLastVisiDate = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 3)
            };
            _refreshLastVisiDate.Tick += _refreshLastVisiDate_Tick;

            IsGlobalSearch = false;
            _roomsViewModel.SelectFirstRoom();
        }

        private void _refreshLastVisiDate_Tick(object sender, EventArgs e)
        {
            if (SelectedRoom != null)
            {
                _bll.UpdateVisitInfo(SelectedRoom.Room.Room.Id);
            }
        }

        private void _refreshMessagesTimer_Tick(object sender, EventArgs e)
        {
            if (SelectedRoom != null)
            {
                _messagesViewModel.Update(_bll.GetMessagesOfRoom(SelectedRoom.Room.Room.Id));
            }
        }
        private void _refreshRoomsTimer_Tick(object sender, EventArgs e)
        {
            _roomsViewModel.Update(_bll.GetInfosAboutAllUserRooms());
        }

        private void Room_Click(RoomInfoBox roomInfoBox)
        {
            SelectedRoom = roomInfoBox;
            if (SelectedRoom != null)
            {
                _messagesViewModel.Reload(_bll.GetMessagesOfRoom(SelectedRoom.Room.Room.Id));
            }
        }
        private void AddRoom_Click(int roomId, bool isAdd)
        {
            if (isAdd)
            {
                _bll.AddRoom(roomId);
            }
            else
            {
                _bll.RemoveRoom(roomId);
            }
        }
        private void GlobalSearchButton_Click(object sender, RoutedEventArgs e)
        {
            IsGlobalSearch = !IsGlobalSearch;

            if (IsGlobalSearch)
            {
                _newRoomsViewModel.Reload(_bll.GetAllPublicRooms(), _bll.GetUserRoomsId());
                roomsList.DataContext = _newRoomsViewModel;
                _refreshRoomsTimer.Stop();
            }
            else
            {
                _roomsViewModel.Update(_bll.GetInfosAboutAllUserRooms());
                roomsList.DataContext = _roomsViewModel;
                _refreshRoomsTimer.Start();
            }
        }

        private void CreateRoomButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new RoomInfoDialog(_bll);
            var task = DialogHost.Show(dialog, "MainWindow", new DialogClosingEventHandler(Button_Click_3));
        }
        private void UserDetails_Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new UserInfoDialog(_bll);
            DialogHost.Show(dialog, "MainWindow");
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            _bll.Logout();
        }

        // Edit room
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (SelectedRoom != null)
            {
                var dialog = new RoomInfoDialog(_bll, _bll.GetRoom(SelectedRoom.Room.Room.Id));
                var task = DialogHost.Show(dialog, "MainWindow", new DialogClosingEventHandler(Button_Click_3));
            }
        }

        // Search our rooms
        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (IsGlobalSearch)
            {
                _newRoomsViewModel.Filter(SearchTectBlock.Text);
            }
            else
            {
                _roomsViewModel.Filter(SearchTectBlock.Text);
            }
        }

        private void RefreshMsgsButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRoom != null)
            {
                _messagesViewModel.Update(_bll.GetMessagesOfRoom(SelectedRoom.Room.Room.Id));
            }
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRoom != null)
            {
                var text = MessageTextBox.Text.Trim();
                if (!String.IsNullOrEmpty(text))
                {
                    _bll.SendMessage(SelectedRoom.Room.Room.Id, text);
                    _messagesViewModel.Update(_bll.GetMessagesOfRoom(SelectedRoom.Room.Room.Id));
                    MessageTextBox.Text = String.Empty;
                }
            }
        }

        private void MessageSearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            _messagesViewModel.Filter(MessageSearchTextBox.Text);
        }

        // Refresh rooms
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            _roomsViewModel.Update(_bll.GetInfosAboutAllUserRooms());
        }
    }
}
