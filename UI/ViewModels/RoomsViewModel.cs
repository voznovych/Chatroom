using BLL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace UI.ViewModels
{
    class RoomsViewModel
    {
        private List<RoomInfoViewModel> _roomInfoViewModels;
        private List<RoomInfoBox> _roomInfoBoxes;

        private CollectionViewSource _viewSource;

        public ICollectionView ViewData => _viewSource.View;
        //public ObservableCollection<RoomInfoBox> Rooms
        //{
        //    get { return _roomInfoBoxes; }
        //}


        public RoomsViewModel(IEnumerable<RoomInfo> rooms, RoomIBoxClick click)
        {
            _roomInfoViewModels = new List<RoomInfoViewModel>();
            foreach (var room in rooms)
            {
                _roomInfoViewModels.Add(new RoomInfoViewModel(room));
            }

            _roomInfoBoxes = new List<RoomInfoBox>();
            foreach (var room in _roomInfoViewModels)
            {
                _roomInfoBoxes.Add(new RoomInfoBox(room, click));
            }

            _viewSource = new CollectionViewSource()
            {
                Source = _roomInfoBoxes
            };
            _viewSource.Filter += Filter;
            _filter = String.Empty;
        }

        private string _filter;
        private void Filter(object sender, FilterEventArgs e)
        {
            if (String.IsNullOrEmpty(_filter))
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = (e.Item as RoomInfoBox).Room.Room.Name.ToLower().Contains(_filter.ToLower());
            }
        }

        public void Filter(string name)
        {
            _filter = name;
            _viewSource.View.Refresh();
        }
        public void Sort()
        {
            _viewSource.SortDescriptions.Add(new SortDescription("Room.Room.Name", ListSortDirection.Ascending));
        }

        public void Update()
        {
            //foreach (var room in rooms)
            //{
            //    _roomInfoViewModels.FirstOrDefault(r => r.Room.Id == room.Id).Update(room);
            //}

        }

        public void SortByDateOfLastMessage()
        {
            //_roomInfoBoxes = _roomInfoBoxes.OrderBy(r => r.Room.Room.LastMessage.DateOfSend);
            //_roomInfoBoxes.TO
        }
    }
}
