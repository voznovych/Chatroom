using System.Drawing;

namespace BLL
{
    public class RoomInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Image Photo { get; set; }
        public MessageInfo LastMessage { get; set; }
        public int AmountOfUnreadedMsgs { get; set; }
    }
}
