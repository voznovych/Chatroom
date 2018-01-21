using BLL;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Linq;
using UI.Domain;
//using System.Windows.Controls;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Collections.Generic;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BLLClass _bll;
        UserInfo _currentUser;
        List<CountryInfo> countries;
        List<SexInfo> sexes;
       

        private void InitUserEditInfBlock()
        {
            _currentUser = _bll.GetCurrentUser();
            FirstNameTextBox.Text = _currentUser.Name;
            LastNameTextBox.Text = _currentUser.Surname;
            DateOfBirthPicker.SelectedDate = _currentUser.DateOfBirth;
            CountryComboBox.SelectedItem = countries.First(c => c.Id == _currentUser.CountryId);
            SexComboBox.SelectedIndex = sexes.TakeWhile(s => s.Id == _currentUser.SexId).Count();
            UserAvatarChangingBox.ImageSource = Util.ImageToImageSource(_currentUser.Photo);
        }

        public MainWindow(BLLClass bll)
        {
            InitializeComponent();
            _bll = bll;

            countries = bll.GetCountriesInf().ToList();
            CountryComboBox.ItemsSource = countries;
            sexes = _bll.GetSexesInf().ToList();
            SexComboBox.ItemsSource = sexes;


            const int count = 18;

            RoomDTO[] roomArr = new RoomDTO[count];

            for (int i = 0; i < count; i++)
            {
                roomArr[i] = new RoomDTO()
                {
                    Name = "Test Room",
                    Photo = Util.ByteArrayToImage(File.ReadAllBytes("Resources/avatar_m.png"))
                };
            }

            MessageDTO[] msArr = new MessageDTO[count];

            for (int i = 0; i < count; i++)
            {
                msArr[i] = new MessageDTO()
                {
                    Text = "Hello pidor andty, ya tia diuznu hhhnfhf ghfhhfhh fgghff hffdsf",
                    DateOfSend = DateTime.Now,
                    User = new UserDTO() { Name = "Vlad" }
                };
            }

            RoomInfo[] riArr = new RoomInfo[count];
            Random rnd = new Random();

            for (int i = 0; i < count; i++)
            {
                riArr[i] = new RoomInfo()
                {
                    Room = roomArr[i],
                    Message = msArr[i],
                    UnreadMessages = rnd.Next(3)
                };
            }

            RoomInfoBox[] ribArr = new RoomInfoBox[count];

            for (int i = 0; i < count; i++)
            {
                ribArr[i] = new RoomInfoBox()
                {
                    DataContext = riArr[i]
                };
                //roomsListBox.Items.Add(ribArr[i]);
            }

            roomsListBox.ItemsSource = ribArr;
        }

        private bool IsSetedRequiredFieldsForEdit()
        {
            return !String.IsNullOrEmpty(FirstNameTextBox.Text)
                && !String.IsNullOrEmpty(LastNameTextBox.Text)
                && SexComboBox.SelectedIndex != -1
                && DateOfBirthPicker.SelectedDate != null
                && CountryComboBox.SelectedIndex != -1;
        }

        private void ShowSampleMessageDialog(string content)
        {
            var sampleMessageDialog = new SampleMessageDialog
            {
                Message = { Text = content }
            };

            DialogHost.Show(sampleMessageDialog, "RootDialog");
        }

        private void UpdateUserInfo_Click(object sender, RoutedEventArgs e)
        {
            if(IsSetedRequiredFieldsForEdit())
            {
                bool needEdit = true;
                if(!string.IsNullOrEmpty(NewPasswordBox.Password))
                {
                    if(NewPasswordBox.Password != ConfirmedPasswordBox.Password)
                    {
                        needEdit = false;
                        ShowSampleMessageDialog("Passwords don't match");
                    }
                    else
                    {
                        _currentUser.Password = NewPasswordBox.Password;
                    }
                    NewPasswordBox.Password = string.Empty;
                    ConfirmedPasswordBox.Password = string.Empty;
                }
                if (needEdit)
                {
                    _currentUser.Photo = Util.ImageSourceToImage(UserAvatarChangingBox.ImageSource);
                    _currentUser.CountryId = ((CountryInfo)CountryComboBox.SelectedItem).Id;
                    _currentUser.SexId = ((SexInfo)SexComboBox.SelectedItem).Id;
                    _currentUser.Name = FirstNameTextBox.Text;
                    _currentUser.Surname = LastNameTextBox.Text;
                    _currentUser.DateOfBirth = DateOfBirthPicker.SelectedDate;
                    switch (_bll.UpdateUser(_currentUser))
                    {
                        case UpdateResult.NameIsInvalidOrEmpty:
                            ShowSampleMessageDialog("Name is invalid or empty!");
                            break;
                        case UpdateResult.SurnameIsInvalidOrEmpty:
                            ShowSampleMessageDialog("Surname is invalid or empty!");
                            break;
                        case UpdateResult.PasswordIsInvalid:
                            ShowSampleMessageDialog("Password is invalid or empty!");
                            break;
                        case UpdateResult.SexIsInvalidOrNotSelected:
                            ShowSampleMessageDialog("Sex is not selected!");
                            break;
                        case UpdateResult.BirthDateIsInvalidOrNotSelected:
                            ShowSampleMessageDialog("Date of birth is invalid or not selected!");
                            break;
                    }
                }
            }
        }


        private bool _isRightButtonDownPictureChange = false;
        private void ChangePicture_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _isRightButtonDownPictureChange = true;
        }

        private void ChangePicture_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(_isRightButtonDownPictureChange)
            {
                Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
                ofd.Filter = $@"Image Files(*.BMP;*.JPG;*.JPEG;)|*.BMP;*.JPG;*.JPEG;";
                if (ofd.ShowDialog() == true)
                {
                    Uri uri = new Uri(ofd.FileName);
                    UserAvatarChangingBox.ImageSource = Util.ImageToImageSource(new Bitmap(ofd.FileName));
                }
                _isRightButtonDownPictureChange = false;
            }
        }

        private void MouseLeavedPictureBox(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _isRightButtonDownPictureChange = false;
            Cursor = System.Windows.Input.Cursors.Arrow;
        }

        private void MouseEntered(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = System.Windows.Input.Cursors.Hand;
        }

        private void OpenUserInfoBlock_Click(object sender, RoutedEventArgs e)
        {
            NewPasswordBox.Password = string.Empty;
            ConfirmedPasswordBox.Password = string.Empty;
            InitUserEditInfBlock();
        }
    }

    class RoomInfo : INotifyPropertyChanged
    {
        public RoomDTO Room { get; set; }
        public MessageDTO Message { get; set; }
        private int _unreadMessages;
        private ImageSource _photo;

        public int? UnreadMessages
        {
            get { return _unreadMessages == 0 ? null : (int?)_unreadMessages; }

            set
            {
                _unreadMessages = value.Value;
                OnPropertyChanged("UnreadMessages");
            }
        }

        public string Text
        {
            get { return Message.Text; }
            set
            {
                Message.Text = value;
                OnPropertyChanged("Text");
            }
        }

        public string DateOfSend
        {
            get { return Message.DateOfSend.Day == DateTime.Now.Day ? Message.DateOfSend.ToShortTimeString() : Message.DateOfSend.ToShortDateString(); }
            set
            {
                //Message.DateOfSend = value;
                OnPropertyChanged("DateOfSend");
            }
        }

        public ImageSource Photo
        {
            get { return Util.ImageToImageSource(Room.Photo); }
            set
            {
                _photo = value;
                OnPropertyChanged("Photo");
            }
        }

        public string Name
        {
            get { return Room.Name; }
            set
            {
                Room.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string SenderName
        {
            get { return Message.User.Name; }
            set
            {
                Message.User.Name = value;
                OnPropertyChanged("SenderName");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
