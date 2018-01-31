using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using UI.Controls;
using System.Windows.Data;
using System.ComponentModel;

namespace UI.ViewModels
{
    class NewRoomsViewModel
    {
        private string _filter;
        private AddRoomClick _addClick;

        private List<NewRoomInfoViewModel> _roomInfoViewModels;
        private List<NewRoomInfo> _roomInfoCards;

        private CollectionViewSource _viewSource;

        public ICollectionView View => _viewSource.View;

        public NewRoomsViewModel(AddRoomClick addClick)
        {
            _roomInfoViewModels = new List<NewRoomInfoViewModel>();
            _roomInfoCards = new List<NewRoomInfo>();

            _viewSource = new CollectionViewSource()
            {
                Source = _roomInfoCards
            };
            _filter = String.Empty;
            _viewSource.Filter += Filter;
            _viewSource.SortDescriptions.Add(new SortDescription("Room.Room.AmountOfMembers", ListSortDirection.Descending));

            _addClick = addClick;
        }

        private void Add(RoomInfoForNewRoomCard room, IEnumerable<int> userRoomsId)
        {
            var rivm = new NewRoomInfoViewModel(room);
            _roomInfoViewModels.Add(rivm);
            _roomInfoCards.Add(new NewRoomInfo(rivm, _addClick, userRoomsId.Contains(room.Id)));
        }
        private void Filter(object sender, FilterEventArgs e)
        {
            if (String.IsNullOrEmpty(_filter))
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = (e.Item as NewRoomInfo).Room.Room.Name.ToLower().Contains(_filter.ToLower());
            }
        }

        #region Public methods
        public void Filter(string name)
        {
            _filter = name;
            _viewSource.View.Refresh();
        }

        public void Load(IEnumerable<RoomInfoForNewRoomCard> rooms, IEnumerable<int> userRoomsId)
        {
            foreach (var r in rooms)
            {
                Add(r, userRoomsId);
            }
            _viewSource.View.Refresh();
        }
        public void Reload(IEnumerable<RoomInfoForNewRoomCard> rooms, IEnumerable<int> userRoomsId)
        {
            _roomInfoViewModels.Clear();
            _roomInfoCards.Clear();
            Load(rooms, userRoomsId);
        }
        public void Update(IEnumerable<RoomInfoForNewRoomCard> rooms, IEnumerable<int> userRoomsId)
        {
            // Update
            foreach (var room in rooms)
            {
                var founded = _roomInfoViewModels.FirstOrDefault(r => r.Room.Id == room.Id);
                if (founded == null)
                {
                    Add(room, userRoomsId);
                }
                else
                {
                    founded.Update(room);
                }
            }
            foreach (var room in _roomInfoViewModels)
            {
                if (rooms.FirstOrDefault(r => r.Id == room.Room.Id) == null)
                {
                    _roomInfoViewModels.Remove(room);
                }
            }

            //var addItemsId = (from ri in _roomInfoViewModels select ri.Room.Id)
            //            .Intersect(from r in rooms select r.Id);
            //
            //foreach (var r in _roomInfoViewModels.Where(r => addItemsId.Contains(r.Room.Id)))
            //{
            //    
            //}

            _viewSource.View.Refresh();
        }
        #endregion
    }
}
