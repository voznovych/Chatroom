using System.Drawing;

namespace BLL
{
    public class RoomInfoForRoomInfoBox
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Image Photo { get; set; }
        public MessageInfoForRoomInfoBox LastMessage { get; set; }
        public int AmountOfUnreadedMsgs { get; set; }
    }
}
