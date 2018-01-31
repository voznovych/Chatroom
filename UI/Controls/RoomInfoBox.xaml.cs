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
using UI.ViewModels;

namespace UI
{
    /// <summary>
    /// Interaction logic for RoomInfoBox.xaml
    /// </summary>
    public partial class RoomInfoBox : UserControl
    {
        private RoomInfoViewModel _room;
        private RoomIBoxClick _click;
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value && !_isSelected)
                {
                    _click?.Invoke(this);
                }

                IsSelectedRect.Visibility = value ? Visibility.Visible : Visibility.Hidden;
                _isSelected = value;
            }
        }
        public RoomInfoViewModel Room => _room;

        public RoomInfoBox(RoomInfoViewModel room, RoomIBoxClick click)
        {
            InitializeComponent();
            _room = room;
            _click = click;
            IsSelected = false;

            DataContext = _room;
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsSelected = true;
        }
    }
}
