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

namespace UI.Controls
{
    /// <summary>
    /// Interaction logic for NewRoomInfo.xaml
    /// </summary>
    public partial class NewRoomInfo : UserControl
    {
        private NewRoomInfoViewModel _room;
        private AddRoomClick _add;
        private bool _isAdded;

        public bool IsAdded
        {
            get { return _isAdded; }
            set
            {
                if (value)
                {
                    AddButton.Background = this.FindResource("SecondaryAccentBrush") as Brush;
                    AddButton.Foreground = this.FindResource("SecondaryAccentForegroundBrush") as Brush;
                }
                else
                {
                    AddButton.Background = this.FindResource("PrimaryHueLightBrush") as Brush;
                    AddButton.Foreground = this.FindResource("PrimaryHueLightForegroundBrush") as Brush;
                }
                AddButton.IsChecked = value;
                _isAdded = value;
            }
        }
        public NewRoomInfoViewModel Room => _room;

        public NewRoomInfo(NewRoomInfoViewModel room, AddRoomClick add, bool isAdded)
        {
            InitializeComponent();
            _room = room;
            _add = add;
            IsAdded = isAdded;

            DataContext = _room;
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            IsAdded = !IsAdded;
            _add?.Invoke(_room.Room.Id, IsAdded);
        }
    }
}
