using BLL;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Media;
using UI.Controls;
using UI.ViewModels;
using System.Windows.Input;
using System.Collections.Generic;
using MaterialDesignColors;
using MahMaterialDragablzMashUp;
using System.Linq;

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

            //
            Swatches = new SwatchesProvider().Swatches.OrderBy(s => s.Name);
            ThemePrimaryList.ItemsSource = Swatches;
            ThemeAccentList.ItemsSource = Swatches;

            string currentP = Application.Current.Resources.MergedDictionaries.FirstOrDefault(m => m.Source.OriginalString.Contains(@"/Primary/")).Source.OriginalString;
            ThemePrimaryList.SelectedItem = Swatches.FirstOrDefault(s => currentP.Contains(s.Name));
            string currentA = Application.Current.Resources.MergedDictionaries.FirstOrDefault(m => m.Source.OriginalString.Contains(@"/Accent/")).Source.OriginalString;
            ThemeAccentList.SelectedItem = Swatches.FirstOrDefault(s => currentA.Contains(s.Name));

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

        #region Palette block
        private static void ApplyBase(bool isDark)
        {
            new PaletteHelper.PaletteHelper().SetLightDark(isDark);
        }

        public IEnumerable<Swatch> Swatches { get; }

        private static void ApplyPrimary(Swatch swatch)
        {
            new PaletteHelper.PaletteHelper().ReplacePrimaryColor(swatch);
        }
        private static void ApplyAccent(Swatch swatch)
        {
            new PaletteHelper.PaletteHelper().ReplaceAccentColor(swatch);
        }
        #endregion

        private void ToggleButton_StatusChanged(object sender, RoutedEventArgs e)
        {
            ApplyBase((bool)((System.Windows.Controls.Primitives.ToggleButton)sender).IsChecked);
        }

        private void ThemePrimaryList_DropDownClosed(object sender, EventArgs e)
        {
            ApplyPrimary((Swatch)ThemePrimaryList.SelectedItem);
        }

        private void ThemeAccentList_DropDownClosed(object sender, EventArgs e)
        {
            ApplyAccent((Swatch)ThemeAccentList.SelectedItem);
        }
    }
}
