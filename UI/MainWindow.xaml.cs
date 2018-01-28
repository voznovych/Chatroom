using BLL;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Media;
using UI.Controls;
using UI.ViewModels;

namespace UI
{
    public delegate void RoomIBoxClick(RoomInfoBox roomInfoBox);

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private properties
        private readonly BLLClass _bll;
        private bool _isGlobalSearch;

        private RoomInfoBox _selectedRoom;

        private RoomsViewModel _roomsViewModel;
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

            _roomsViewModel = new RoomsViewModel(_bll.GetInfosAboutAllUserRooms(), Room_Click);
            roomsList.DataContext = _roomsViewModel;
            //messagesList.DataContext = new MessageViewModel();

            IsGlobalSearch = false;
        }

        private void Room_Click(RoomInfoBox roomInfoBox)
        {
            SelectedRoom = roomInfoBox;
        }

        private void CreateRoomButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new RoomInfoDialog(_bll);
            DialogHost.Show(dialog, "MainWindow");
        }
        private void UserDetails_Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new UserInfoDialog(_bll);
            DialogHost.Show(dialog, "MainWindow");
        }

        private void GlobalSearchButton_Click(object sender, RoutedEventArgs e)
        {
            IsGlobalSearch = !IsGlobalSearch;
        }
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            _bll.Logout();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //_roomsViewModel.Update();
            _roomsViewModel.Sort();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //_bll.EditRoomTest(1);
        }

        // Search our rooms
        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            _roomsViewModel.Filter(SearchTectBlock.Text);
        }
    }
}
