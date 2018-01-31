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
    public class MessageInfViewModel : INotifyPropertyChanged
    {
        private MessageInfoForMessageInfoCard _message;

        public MessageInfoForMessageInfoCard Message { get { return _message; } }

        public MessageInfViewModel(MessageInfoForMessageInfoCard message)
        {
            _message = message;
        }

        public void Update(MessageInfoForMessageInfoCard message)
        {
            var oldMsg = _message;
            _message = message;

            if (_message.Text != oldMsg.Text)
            {
                OnPropertyChanged("Text");
            }
            if (_message.DateOfSend != oldMsg.DateOfSend)
            {
                OnPropertyChanged("DateOfSend");
            }
            if (_message.Sender.Photo != _message.Sender.Photo)
            {
                OnPropertyChanged("Photo");
            }
            if (_message.Sender.FullName != _message.Sender.FullName)
            {
                OnPropertyChanged("FullName");
            }
        }

        public string Text
        {
            get { return _message.Text; }
        }
        public string DateOfSend
        {
            get
            {
                return _message.DateOfSend.Day == DateTime.Now.Day
                  ? _message.DateOfSend.ToShortTimeString()
                  : _message.DateOfSend.ToShortDateString();
            }
        }
        public ImageSource Photo
        {
            get { return Util.ImageToImageSource(_message.Sender.Photo); }
        }
        public string FullName
        {
            get { return _message.Sender.FullName; }
        }
       
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
