using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace UI.ViewModels
{
    public class MessagesViewModel
    {
        private string _filter;
        private int _userId;

        private List<MessageInfViewModel> _messagesInfViewModels;
        private List<MessageInfoCard> _messageCards;

        private CollectionViewSource _viewSource;

        public ICollectionView View => _viewSource.View;

        public MessagesViewModel(int userId)
        {
            _messagesInfViewModels = new List<MessageInfViewModel>();
            _messageCards = new List<MessageInfoCard>();

            _viewSource = new CollectionViewSource()
            {
                Source = _messageCards
            };
            _viewSource.Filter += Filter;
            _filter = String.Empty;
            _viewSource.SortDescriptions.Add(new SortDescription("Message.Message.Date", ListSortDirection.Descending));

            _userId = userId;
        }

        private void Add(MessageInfoForMessageInfoCard message)
        {
            var rivm = new MessageInfViewModel(message);
            _messagesInfViewModels.Add(rivm);
            _messageCards.Add(new MessageInfoCard(rivm, message.Sender.Id == _userId));
        }
        
        private void Filter(object sender, FilterEventArgs e)
        {
            if (String.IsNullOrEmpty(_filter))
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = (e.Item as MessageInfoCard).Message.Message.Text.ToLower().Contains(_filter.ToLower());
            }
        }

        #region Public methods
        public void Filter(string text)
        {
            _filter = text;
            _viewSource.View.Refresh();
        }
        
        public void Load(IEnumerable<MessageInfoForMessageInfoCard> messages)
        {
            foreach (var m in messages)
            {
                Add(m);
            }
            _viewSource.View.Refresh();
        }
        public void Reload(IEnumerable<MessageInfoForMessageInfoCard> messages)
        {
            _messageCards.Clear();
            _messagesInfViewModels.Clear();
            Load(messages);
        }
        public void Update(IEnumerable<MessageInfoForMessageInfoCard> messages)
        {
            // Update
            foreach (var message in messages)
            {
                var founded = _messagesInfViewModels.FirstOrDefault(m => m.Message.Id == message.Id);
                if (founded == null)
                {
                    // Add new messages
                    Add(message);
                }
                else
                {
                    // Update existing messages
                    founded.Update(message);
                }
            }
            // Delete deleted messages
            //foreach (var message in _messagesInfViewModels)
            //{
            //    if (messages.FirstOrDefault(r => r.Id == message.Message.Id) == null)
            //    {
            //        _messagesInfViewModels.Remove(message);
            //    }
            //}

            _viewSource.View.Refresh();
        }
        #endregion
    }
}
