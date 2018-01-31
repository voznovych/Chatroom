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
        private string _filter;
        private RoomIBoxClick _click;
        private int _userId;

        private List<RoomInfoViewModel> _roomInfoViewModels;
        private List<RoomInfoBox> _roomInfoBoxes;

        private CollectionViewSource _viewSource;

        public ICollectionView View => _viewSource.View;


        public RoomsViewModel(RoomIBoxClick click, int userId)
        {
            _roomInfoViewModels = new List<RoomInfoViewModel>();
            _roomInfoBoxes = new List<RoomInfoBox>();

            _viewSource = new CollectionViewSource()
            {
                Source = _roomInfoBoxes
            };
            _filter = String.Empty;
            _viewSource.Filter += Filter;
            _viewSource.SortDescriptions.Add(new SortDescription("Room.Room.LastMessage.DateOfSend", ListSortDirection.Descending));

            _click = click;
            _userId = userId;
        }

        private void Add(RoomInfoForRoomInfoBox room)
        {
            var rivm = new RoomInfoViewModel(room, _userId);
            _roomInfoViewModels.Add(rivm);
            _roomInfoBoxes.Add(new RoomInfoBox(rivm, _click));
        }
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

        #region Public methods
        public void Filter(string name)
        {
            _filter = name;
            _viewSource.View.Refresh();
        }
       
        public void SelectFirstRoom()
        {
            if (_roomInfoBoxes.Count >= 1)
            {
                _roomInfoBoxes.OrderByDescending(d => d.Room.DateOfSend).First().IsSelected = true;
            }
        }
        public void Load(IEnumerable<RoomInfoForRoomInfoBox> rooms)
        {
            foreach (var r in rooms)
            {
                Add(r);
            }
            _viewSource.View.Refresh();
        }
        public void Update(IEnumerable<RoomInfoForRoomInfoBox> rooms)
        {
            // Update
            foreach (var room in rooms)
            {
                var founded = _roomInfoViewModels.FirstOrDefault(r => r.Room.Id == room.Id);
                if (founded == null)
                {
                    // Add new room
                    Add(room);
                }
                else
                {
                    // Update existing room
                    founded.Update(room);
                }
            }

            // Delet deleted room
            if (_roomInfoViewModels.Count != rooms.Count())
            {
                List<int> ides = new List<int>();
                foreach (var room in _roomInfoViewModels) // optimize
                {
                    if (rooms.FirstOrDefault(r => r.Id == room.Room.Id) == null)
                    {
                        ides.Add(room.Room.Id);
                    }
                }
                foreach (var item in ides)
                {
                    var box = _roomInfoBoxes.FirstOrDefault(r => r.Room.Room.Id == item);
                    var info = box?.Room;
                    if (box != null)
                    {
                        _roomInfoBoxes.Remove(box);
                        _roomInfoViewModels.Remove(info);
                    }
                }
            }
            
            _viewSource.View.Refresh();
        }
        #endregion
    }

    //class RoomEqualityComparer : IEqualityComparer<RoomInfoForRoomInfoBox>
    //{
    //    public bool Equals(RoomInfoForRoomInfoBox ri1, RoomInfoForRoomInfoBox ri2)
    //    {
    //        return ri1.Id == ri2.Id;
    //    }
    //
    //    public int GetHashCode(RoomInfoForRoomInfoBox ri)
    //    {
    //        return ri.GetHashCode();
    //    }
    //}
}
