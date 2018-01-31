using AutoMapper;
using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using BLL;
using DAL.Interfaces;
using BLL.DTO_Enteties;

namespace BLL
{
    public enum LoginResult
    {
        Success,
        LoginIsNotExist,
        PasswordIsWrong
    }
    public enum RegistrationResult
    {
        Success,
        LoginIsAlreadyExist,
        LoginIsInvalidOrEmpty,
        NameIsInvalidOrEmpty,
        SurnameIsInvalidOrEmpty,
        PasswordIsInvalidOrEmpty,
        BirthDateIsInvalidOrNotSelected,
        SexIsInvalidOrNotSelected,
    }
    public enum UpdateResult
    {
        Success,
        NameIsInvalid,
        SurnameIsInvalid,
        PasswordIsInvalid,
        BirthDateIsInvalid,
        SexIsInvalid,
    }

    public class BLLClass
    {
        #region Private properties
        private static bool _isMapperInitialized;

        private const string STATUS_ONLINE = "Online";
        private const string STATUS_OFFLINE = "Offline";
        private const string STATUS_DND = "Do not disturb";

        const int HUNDRED_YEARS_BEFORE = -100;
        private string LoginPattern { get; set; } = @"^[a-zA-Z]\w{5,19}$";
        private string PasswordPattern { get; set; } = @"\w{6,25}";

        private readonly IDAL _dal;
        private User AuthenticatedUser { get; set; }
        #endregion

        static BLLClass()
        {
            _isMapperInitialized = false;
        }
        public BLLClass()
        {
            if (!_isMapperInitialized)
            {
                // Init AutoMapper
                Mapper.Initialize(cfg =>
                {

                #region Entity to DTO
                cfg.CreateMap<User, UserDTO>()
                        .ForMember(dest => dest.Photo,
                            opts => opts.MapFrom(src => Util.ByteArrayToImage(src.Photo)));

                    cfg.CreateMap<Room, RoomDTO>()
                        .ForMember(dest => dest.Photo,
                                opts => opts.MapFrom(src => Util.ByteArrayToImage(src.Photo)));

                    cfg.CreateMap<Sex, SexDTO>();

                    cfg.CreateMap<Country, CountryDTO>();

                    cfg.CreateMap<UserStatus, UserStatusDTO>();

                    cfg.CreateMap<VisitInfo, VisitInfoDTO>();

                    cfg.CreateMap<Message, MessageDTO>();

                    cfg.CreateMap<Genre, GenreDTO>();
                #endregion

                #region DTO to Entity
                cfg.CreateMap<UserDTO, User>()
                        .ForMember(dest => dest.Photo,
                            opts => opts.MapFrom(src => Util.ImageToByteArray(src.Photo)));

                    cfg.CreateMap<RoomDTO, Room>()
                        .ForMember(dest => dest.Photo,
                                opts => opts.MapFrom(src => Util.ImageToByteArray(src.Photo)));

                    cfg.CreateMap<SexDTO, Sex>();

                    cfg.CreateMap<CountryDTO, Country>();

                    cfg.CreateMap<UserStatusDTO, UserStatus>();

                    cfg.CreateMap<VisitInfoDTO, VisitInfo>();

                    cfg.CreateMap<MessageDTO, Message>();

                    cfg.CreateMap<GenreDTO, Genre>();
                #endregion

                cfg.CreateMap<User, UserInfo>()
                        .ForMember(dest => dest.Password,
                            opts => opts.Ignore());

                    cfg.CreateMap<Message, MessageInfoForMessageInfoCard>()
                        .ForMember(dest => dest.Sender,
                            opts => opts.MapFrom(src => src.User));

                    cfg.CreateMap<Room, RoomInfoForNewRoomCard>()
                        .ForMember(dest => dest.Photo,
                            opts => opts.MapFrom(src => Util.ByteArrayToImage(src.Photo)))
                        .ForMember(dest => dest.AmountOfMembers,
                            opts => opts.MapFrom(src => GetAmountOfMembersInRoom(src.Id)));

                    cfg.CreateMap<User, UserInfoForMessageInfoCard>()
                        .ForMember(dest => dest.Photo,
                            opts => opts.MapFrom(src => Util.ByteArrayToImage(src.Photo)))
                        .ForMember(dest => dest.FullName,
                            opts => opts.MapFrom(src => $"{src.Name} {src.Surname}"));
                });

                _isMapperInitialized = true;
            }

            try
            {
                _dal = new DALClass();
            }
            catch (Exception)
            {
            }
        }

        #region Public methods
        public RegistrationResult SignUp(SignUpUserData data)
        {
            if (!IsValidLogin(data.Login))
            {
                return RegistrationResult.LoginIsInvalidOrEmpty;
            }
            else if (IsLoginExist(data.Login))
            {
                return RegistrationResult.LoginIsAlreadyExist;
            }
            else if (!IsValidPassword(data.Password))
            {
                return RegistrationResult.PasswordIsInvalidOrEmpty;
            }
            else if (!IsValidName(data.Name))
            {
                return RegistrationResult.NameIsInvalidOrEmpty;
            }
            else if (!IsValidName(data.Surname))
            {
                return RegistrationResult.SurnameIsInvalidOrEmpty;
            }
            else if (!data.BirthDate.HasValue || !IsValidBirthDate(data.BirthDate.Value))
            {
                return RegistrationResult.BirthDateIsInvalidOrNotSelected;
            }
            else if (!IsValidSex(data.Sex))
            {
                return RegistrationResult.SexIsInvalidOrNotSelected;
            }

            User registeredUser = new User()
            {
                Login = data.Login,
                Password = Util.GetHashString(data.Password),
                Name = data.Name,
                Surname = data.Surname,
                BirthDate = data.BirthDate,
                SexId = data.Sex.Id,
                CountryId = data.Country?.Id,
                Photo = data.Photo
            };

            registeredUser.StatusId = GetUserStatusId(STATUS_OFFLINE);
            AddUser(registeredUser);

            return RegistrationResult.Success;
        }
        public LoginResult Login(string login, string password)
        {
            if (!IsLoginExist(login))
            {
                return LoginResult.LoginIsNotExist;
            }

            if (!IsPasswordRight(login, password))
            {
                return LoginResult.PasswordIsWrong;
            }

            AuthenticatedUser = GetUserByLogin(login);
            AuthenticatedUser.StatusId = GetUserStatusId(STATUS_ONLINE);
            UpdateUser();

            return LoginResult.Success;
        }
        public void Logout()
        {
            AuthenticatedUser.StatusId = GetUserStatusId(STATUS_OFFLINE);
            UpdateUser();
            AuthenticatedUser = null;
        }

        public UpdateResult UpdateUserInfo(UserInfo user)
        {
            if (user.Password != null && !IsValidPassword(user.Password))
            {
                return UpdateResult.PasswordIsInvalid;
            }
            else if (user.Name != null && !IsValidName(user.Name))
            {
                return UpdateResult.NameIsInvalid;
            }
            else if (user.Surname != null && !IsValidName(user.Surname))
            {
                return UpdateResult.SurnameIsInvalid;
            }
            else if (user.BirthDate.HasValue && !IsValidBirthDate(user.BirthDate.Value))
            {
                return UpdateResult.BirthDateIsInvalid;
            }
            else if (!IsValidSex(user.Sex))
            {
                return UpdateResult.SexIsInvalid;
            }

            AuthenticatedUser.Photo = Util.ImageToByteArray(user.Photo);
            AuthenticatedUser.Password = user.Password == null ? this.AuthenticatedUser.Password : Util.GetHashString(user.Password);
            AuthenticatedUser.Name = user.Name;
            AuthenticatedUser.Surname = user.Surname;
            AuthenticatedUser.BirthDate = user.BirthDate;
            AuthenticatedUser.SexId = user.Sex.Id;
            AuthenticatedUser.CountryId = user.Country?.Id;

            UpdateUser();

            return UpdateResult.Success;
        }
        public UserInfo GetUserInfo()
        {
            return Mapper.Map<User, UserInfo>(AuthenticatedUser);
        }

        public IEnumerable<SexDTO> GetAllSexes()
        {
            return Mapper.Map<IQueryable<Sex>, IEnumerable<SexDTO>>(_dal.Sexes.GetAll());
        }
        public IEnumerable<CountryDTO> GetAllCountries()
        {
            return Mapper.Map<IQueryable<Country>, IEnumerable<CountryDTO>>(_dal.Countries.GetAll()).OrderBy(n => n.Name);
        }
        public IEnumerable<GenreDTO> GetAllGenres()
        {
            return Mapper.Map<IQueryable<Genre>, IEnumerable<GenreDTO>>(_dal.Genres.GetAll()).OrderBy(n => n.Name);
        }

        public IEnumerable<MessageInfoForMessageInfoCard> FindMessagesInRoom(int roomId, string text)
        {
            //var messages = _dal.Rooms.GetAll()
            //                   .FirstOrDefault(r => r.Id == roomId)
            //                   ?.Messages.AsQueryable()
            //                   .Where(m=>m.Text.Contains(text));

            var messages = _dal.Rooms.GetAll()
                               .Where(r => r.Id == roomId)
                               .SelectMany(r => r.Messages)
                               .Where(m => m.Text.Contains(text))
                               .OrderBy(m => m.DateOfSend);

            return Mapper.Map<IQueryable<Message>, IEnumerable<MessageInfoForMessageInfoCard>>(messages);
        }
        public IEnumerable<MessageInfoForMessageInfoCard> FindMessages(string text)
        {
            //var messages = AuthenticatedUser.Rooms.AsQueryable()
            //                                       .SelectMany(r => r.Messages)
            //                                       .Where(m => m.Text.Contains(text));

            var messages = _dal.Users.GetAll().Where(u => u.Id == AuthenticatedUser.Id)
                                              .SelectMany(u => u.Rooms)
                                              .SelectMany(r => r.Messages)
                                              .Where(m => m.Text.Contains(text))
                                              .OrderBy(m => m.DateOfSend);

            return Mapper.Map<IQueryable<Message>, IEnumerable<MessageInfoForMessageInfoCard>>(messages);
        }

        public void RemoveRoom(int id)
        {
            var room = _dal.Rooms.GetAll().FirstOrDefault(v => v.Id == id);
            AuthenticatedUser.Rooms.Remove(room);
            UpdateUser();
        }
        public void AddRoom(int id)
        {
            AuthenticatedUser.Rooms.Add(_dal.Rooms.GetAll().FirstOrDefault(r => r.Id == id));
            UpdateUser();

            if (_dal.VisitInfos.GetAll().FirstOrDefault(v => v.RoomId == id && v.UserId == AuthenticatedUser.Id) == null)
            {
                _dal.VisitInfos.Add(new VisitInfo()
                {
                    RoomId = id,
                    UserId = AuthenticatedUser.Id,
                    LastDateOfVisit = DateTime.Now
                });
            }
        }

        public bool IsIRoomCreator(int roomId)
        {
            return _dal.Rooms.GetAll().FirstOrDefault(r => r.Id == roomId).CreatorId == AuthenticatedUser.Id;
        }
        public RoomDTO GetRoom(int id)
        {
            return Mapper.Map<Room, RoomDTO>(_dal.Rooms.GetAll().FirstOrDefault(r => r.Id == id));
        }

        public void UpdateRoom(RoomDTO room)
        {
            _dal.Rooms.Update(Mapper.Map<RoomDTO, Room>(room));
        }
        public void CreateRoom(CreateRoomData data)
        {
            var room = new Room()
            {
                Name = data.Name,
                Description = data.Description,
                GenreId = data.Genre.Id,
                CreatorId = AuthenticatedUser.Id,
                IsPersonal = data.IsPersonal,
                IsPrivate = data.IsPrivate,
                Photo = Util.ImageToByteArray(data.Photo),
            };

            _dal.Rooms.Add(room);
            AddRoom(room.Id);
        }

        public void UpdateVisitInfo(int roomId)
        {
            _dal.VisitInfos.Update(AuthenticatedUser.VisitInfos.FirstOrDefault(v => v.RoomId == roomId).Id, DateTime.Now);
        }

        public IEnumerable<RoomInfoForRoomInfoBox> GetInfosAboutAllUserRooms()
        {
            return _dal.Users.GetAll().First(u => u.Id == AuthenticatedUser.Id).
                Rooms.Select(r => new RoomInfoForRoomInfoBox
                {
                    Id = r.Id,
                    AmountOfUnreadedMsgs = GetAmountOfUnreadedMessages(r.Id),
                    Photo = Util.ByteArrayToImage(r.Photo),
                    Name = r.Name,
                    LastMessage = GetInfoAboutMessage(GetLastMessage(r.Id))
                });
        }

        public IEnumerable<int> GetUserRoomsId()
        {
            return _dal.Users.GetAll().First(u => u.Id == AuthenticatedUser.Id).Rooms.Select(r => r.Id);
        }
        public int GetAmountOfMembersInRoom(int roomId)
        {
            return _dal.Rooms.GetAll().FirstOrDefault(r => r.Id == roomId).Users.Count();
        }
        public IEnumerable<RoomInfoForNewRoomCard> GetAllPublicRooms()
        {
            return Mapper.Map<IQueryable<Room>, IEnumerable<RoomInfoForNewRoomCard>>(_dal.Rooms.GetAll().Where(r => !r.IsPrivate));
        }

        public bool SendMessage(int roomId, string text)
        {
            if (AuthenticatedUser == null)
                return false;

            _dal.Messages.Add(new Message()
            {
                UserId = AuthenticatedUser.Id,
                RoomId = roomId,
                Text = text,
                DateOfSend = DateTime.Now
            });

            return true;
        }

        private IEnumerable<RoomInfoForRoomInfoBox> ConvertRoomToRoomInfo(IQueryable<Room> rooms)
        {
            return rooms.Select(r => new RoomInfoForRoomInfoBox()
            {
                Id = r.Id,
                AmountOfUnreadedMsgs = GetAmountOfUnreadedMessages(r.Id),
                Photo = Util.ByteArrayToImage(r.Photo),
                Name = r.Name,
                LastMessage = GetInfoAboutMessage(GetLastMessage(r.Id))
            });
        }

        public IEnumerable<RoomInfoForRoomInfoBox> FindUserRooms(string name, int genreId)
        {
            if (genreId == -1)
            {
                var rooms = _dal.Users.GetAll()
                .Where(u => u.Id == AuthenticatedUser.Id)
                .SelectMany(u => u.Rooms)
                .Where(r => r.Name.Contains(name));

                return ConvertRoomToRoomInfo(rooms);
            }

            if (IsGenreExist(genreId))
            {
                var genreRooms = _dal.Users.GetAll()
                    .Where(u => u.Id == AuthenticatedUser.Id)
                    .SelectMany(u => u.Rooms)
                    .Where(r => r.GenreId == genreId)
                    .Where(r => r.Name.Contains(name));

                return ConvertRoomToRoomInfo(genreRooms);
            }

            return null;
        }
        public IEnumerable<RoomInfoForRoomInfoBox> FindRooms(string name, int genreId)
        {
            if (genreId == -1)
            {
                var rooms = _dal.Rooms.GetAll()
                .Where(r => r.Name.Contains(name));

                return ConvertRoomToRoomInfo(rooms);
            }

            if (IsGenreExist(genreId))
            {
                var genreRooms = _dal.Rooms.GetAll()
                    .Where(r => r.GenreId == genreId)
                    .Where(r => r.Name.Contains(name));

                return ConvertRoomToRoomInfo(genreRooms);
            }

            return null;
        }

        public IEnumerable<MessageInfoForMessageInfoCard> GetMessagesOfRoom(int RoomId)
        {
            if (_dal.Rooms.GetAll().FirstOrDefault(r => r.Id == RoomId) == null)
            {
                throw new Exception("Room couldn't be founded!");
            }
            return Mapper.Map<IQueryable<Message>, IEnumerable<MessageInfoForMessageInfoCard>>(_dal.Messages.GetAll().Where(m => m.RoomId == RoomId));
        }
        public IEnumerable<MessageInfoForMessageInfoCard> GetNPackOfMessages(int RoomId, int NumberofPack, int AmountOfMsgsInPack)
        {
            if (_dal.Rooms.GetAll().FirstOrDefault(r => r.Id == RoomId) == null)
            {
                throw new Exception("Room couldn't be founded!");
            }
            IQueryable<Message> msgs = _dal.Messages.GetAll().Where(m => m.RoomId == RoomId).OrderBy(m => m.DateOfSend);
            int count = msgs.Count();
            if (count < NumberofPack * AmountOfMsgsInPack - AmountOfMsgsInPack)
            {
                msgs = null;
            }
            else
            {
                msgs = msgs.Skip(NumberofPack * AmountOfMsgsInPack - AmountOfMsgsInPack).Take(AmountOfMsgsInPack);
            }

            return Mapper.Map<IQueryable<Message>, IEnumerable<MessageInfoForMessageInfoCard>>(msgs);
        }
        public IEnumerable<MessageInfoForMessageInfoCard> GetNewMessages(int RoomId)
        {
            if (_dal.Rooms.GetAll().FirstOrDefault(r => r.Id == RoomId) == null)
            {
                throw new Exception("Room couldn't be founded!");
            }
            var msgs = _dal.Messages.GetAll().Where(m => m.RoomId == RoomId).OrderBy(m => m.DateOfSend);
            var lastVisit = _dal.VisitInfos.GetAll().FirstOrDefault(vi => vi.RoomId == RoomId && vi.UserId == AuthenticatedUser.Id).LastDateOfVisit;

            return Mapper.Map<IQueryable<Message>, IEnumerable<MessageInfoForMessageInfoCard>>(msgs.SkipWhile(m => m.DateOfSend < lastVisit));
        }
        #endregion

        #region Private methods
        private void AddUser(User user)
        {
            _dal.Users.Add(user);
        }
        private User GetUserByLogin(string login)
        {
            return _dal.Users.GetAll().First(u => u.Login == login);
        }
        private void UpdateUser()
        {
            _dal.Users.Update(AuthenticatedUser);
        }

        private bool IsValidLogin(string login)
        {
            return !String.IsNullOrEmpty(login) && Regex.IsMatch(login, LoginPattern);
        }
        private bool IsValidName(string name)
        {
            return !String.IsNullOrEmpty(name);
        }
        private bool IsValidSurname(string surname)
        {
            return !String.IsNullOrEmpty(surname);
        }
        private bool IsValidPassword(string password)
        {
            return !String.IsNullOrEmpty(password) && Regex.IsMatch(password, PasswordPattern);
        }
        private bool IsValidBirthDate(DateTime birthDate)
        {
            return birthDate < DateTime.UtcNow && birthDate > DateTime.UtcNow.AddYears(HUNDRED_YEARS_BEFORE);
        }
        private bool IsValidSex(SexDTO sex)
        {
            if (sex == null)
            {
                return false;
            }

            return IsSexExist(sex.Id);
        }
        private bool IsGenreExist(int id)
        {
            return _dal.Genres.GetAll().FirstOrDefault(g => g.Id == id) != null;
        }
        private bool IsSexExist(int id)
        {
            return _dal.Sexes.GetAll().FirstOrDefault(s => s.Id == id) != null;

        }
        private bool IsLoginExist(string login)
        {
            return _dal.Users.GetAll().FirstOrDefault(u => u.Login == login) != null;
        }
        private bool IsPasswordRight(string login, string password)
        {
            return _dal.Users.GetAll().First(u => u.Login == login).Password == Util.GetHashString(password);
        }

        private int GetUserStatusId(string status)
        {
            return _dal.UserStatuses.GetAll().FirstOrDefault(s => s.Name == status).Id;
        }

        private bool RoomExists(int roomId)
        {
            return _dal.Rooms.GetAll().FirstOrDefault(r => r.Id == roomId) != null;
        }

        private bool UserExists(int userId)
        {
            return _dal.Users.GetAll().FirstOrDefault(u => u.Id == userId) != null;
        }

        private int GetAmountOfUnreadedMessages(int roomId)
        {
            if (!RoomExists(roomId))
            {
                throw new Exception("Room cannot be found!");
            }

            var lastDateOfVisisit = _dal.VisitInfos.GetAll().First(inf => inf.RoomId == roomId && inf.UserId == AuthenticatedUser.Id).LastDateOfVisit;
            return _dal.Messages.GetAll().Where(m => m.RoomId == roomId && m.UserId != AuthenticatedUser.Id && m.DateOfSend > lastDateOfVisisit).Count();
        }

        private MessageInfoForRoomInfoBox GetInfoAboutMessage(Message msg)
        {
            if (msg == null)
            {
                return null;
                //return new MessageInfoForRoomInfoBox
                //{
                //    Text = "Test Message: Bds faeigh kd kdhkd hdahg dgha ghraeighearig raighr h!iweufh.",
                //    SenderName = "Test Sender",
                //    DateOfSend = DateTime.Now,
                //    //SenderId = 0
                //};
            }
            return new MessageInfoForRoomInfoBox
            {
                Text = msg.Text,
                SenderName = msg.User.Name,
                DateOfSend = msg.DateOfSend,
                SenderId = msg.UserId
            };
        }

        private Message GetLastMessage(int roomId)
        {
            if (!RoomExists(roomId))
            {
                throw new Exception("Room cannot be found!");
            }
            return _dal.Messages.GetAll().Where(m => m.RoomId == roomId).OrderByDescending(m => m.DateOfSend).FirstOrDefault();
        }
        #endregion
    }

    public class MessageInfoForMessageInfoCard
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime DateOfSend { get; set; }
        public UserInfoForMessageInfoCard Sender { get; set; }
    }

    public class UserInfoForMessageInfoCard
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public Image Photo { get; set; }
    }

    public class MessageInfoForRoomInfoBox
    {
        public string Text { get; set; }
        public DateTime DateOfSend { get; set; }
        public string SenderName { get; set; }
        public int SenderId { get; set; }
    }
}