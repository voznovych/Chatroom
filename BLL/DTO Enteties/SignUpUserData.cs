using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DBO_Enteties
{
    public class SignUpUserData
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? BirthDate { get; set; }
        public SexDTO Sex { get; set; }
        public CountryDTO Country { get; set; }
    }
}
