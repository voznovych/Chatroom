using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace UI.ViewModels
{
    public class NewRoomInfoViewModel : INotifyPropertyChanged
    {
        private RoomInfoForNewRoomCard _room;

        public RoomInfoForNewRoomCard Room { get { return _room; } }

        public NewRoomInfoViewModel(RoomInfoForNewRoomCard room)
        {
            _room = room;
        }

        public void Update(RoomInfoForNewRoomCard room)
        {
            var oldRoom = _room;
            _room = room;

            if (_room.AmountOfMembers != oldRoom.AmountOfMembers)
            {
                OnPropertyChanged("Members");
            }
            if (_room.Description != oldRoom.Description)
            {
                OnPropertyChanged("Description");
            }
            if (_room.Photo != oldRoom.Photo)
            {
                OnPropertyChanged("Photo");
            }
            if (_room.Name != oldRoom.Name)
            {
                OnPropertyChanged("Name");
            }
            if (_room.Genre.Name != oldRoom.Genre.Name)
            {
                OnPropertyChanged("GenreName");
            }
        }

        public int Members
        {
            get { return _room.AmountOfMembers; }
        }
        public string Description
        {
            get { return _room.Description; }
        }
        public ImageSource Photo
        {
            get { return Util.ImageToImageSource(_room.Photo); }
        }
        public string Name
        {
            get { return _room.Name; }
        }
        public string GenreName
        {
            get { return _room.Genre.Name; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
