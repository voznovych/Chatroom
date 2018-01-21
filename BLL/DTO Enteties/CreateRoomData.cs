using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CreateRoomData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsPersonal { get; set; }
        public Image Photo { get; set; }
        public GenreDTO Genre { get; set; }
    }
}