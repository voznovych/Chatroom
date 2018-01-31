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
    public class RoomInfoViewModel : INotifyPropertyChanged
    {
        private RoomInfoForRoomInfoBox _room;
        private int _userId;

        public RoomInfoForRoomInfoBox Room { get { return _room; } }

        public RoomInfoViewModel(RoomInfoForRoomInfoBox room, int userId)
        {
            _room = room;
            _userId = userId;
        }

        public void Update(RoomInfoForRoomInfoBox room)
        {
            var oldRoom = _room;
            _room = room;

            if (_room.LastMessage?.Text != oldRoom.LastMessage?.Text)
            {
                OnPropertyChanged("Text");
            }
            if (_room.LastMessage?.DateOfSend != oldRoom.LastMessage?.DateOfSend)
            {
                OnPropertyChanged("DateOfSend");
            }
            if (_room.LastMessage?.SenderName != oldRoom.LastMessage?.SenderName)
            {
                OnPropertyChanged("SenderName");
            }
            if (_room.AmountOfUnreadedMsgs != oldRoom.AmountOfUnreadedMsgs)
            {
                OnPropertyChanged("UnreadMessages");
            }
            if (_room.Photo != oldRoom.Photo)
            {
                OnPropertyChanged("Photo");
            }
            if (_room.Name != oldRoom.Name)
            {
                OnPropertyChanged("Name");
            }
        }

        public int? UnreadMessages
        {
            get { return _room.AmountOfUnreadedMsgs == 0 ? null : (int?)_room.AmountOfUnreadedMsgs; }
        }
        public string Text
        {
            get
            {
                if (_room.LastMessage == null)
                {
                    return "No messages here yet...";
                }
                return _room.LastMessage.Text;
            }
        }
        public string DateOfSend
        {
            get
            {
                if (_room.LastMessage == null)
                {
                    return String.Empty;
                }
                return _room.LastMessage.DateOfSend.Day == DateTime.Now.Day
                    ? _room.LastMessage.DateOfSend.ToShortTimeString()
                    : _room.LastMessage.DateOfSend.ToShortDateString();
            }
        }
        public ImageSource Photo
        {
            get { return Util.ImageToImageSource(_room.Photo); }
        }
        public string Name
        {
            get { return _room.Name; }
        }
        public string SenderName
        {
            get
            {
                if (_room.LastMessage == null)
                {
                    return String.Empty;
                }
                return $"{(_room.LastMessage.SenderId == _userId ? "You" : _room.LastMessage.SenderName)}: ";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
