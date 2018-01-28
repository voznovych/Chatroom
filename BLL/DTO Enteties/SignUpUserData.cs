﻿using BLL.DTO_Enteties;
using System;

namespace BLL
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
        public byte[] Photo { get; set; }
    }
}
