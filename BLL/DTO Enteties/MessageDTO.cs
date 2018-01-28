namespace BLL.DTO_Enteties
{
    class MessageDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public UserDTO User { get; set; }

        public int RoomId { get; set; }
        public RoomDTO Room { get; set; }

        public string Text { get; set; }
        public System.DateTime DateOfSend { get; set; }
    }
}
