using BLL;
using BLL.DTO_Enteties;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UI.Domain;

namespace UI.Controls
{
    /// <summary>
    /// Interaction logic for ChangeUserInfoDialog.xaml
    /// </summary>
    public partial class UserInfoDialog : UserControl
    {
        private readonly BLLClass _bll;

        private System.Drawing.Image _photo;
        public System.Drawing.Image Photo
        {
            get { return _photo; }
            set
            {
                PhotoEllipse.Fill = new ImageBrush()
                {
                    ImageSource = Util.ImageToImageSource(value),
                    Stretch = Stretch.Fill
                };
                _photo = value;
            }
        }

        public UserInfoDialog(BLLClass bll)
        {
            InitializeComponent();
            _bll = bll;

            var userInfo = _bll.GetUserInfo();

            SexComboBox.ItemsSource = _bll.GetAllSexes();
            SexComboBox.DisplayMemberPath = "Name";

            CountryComboBox.ItemsSource = _bll.GetAllCountries();
            CountryComboBox.DisplayMemberPath = "Name";

            Photo = userInfo.Photo ?? Util.ByteArrayToImage(File.ReadAllBytes("Resources/if_photo_icon.jpg"));
            LoginTextBlock.Text = userInfo.Login;
            NameTextBox.Text = userInfo.Name;
            SurnameTextBox.Text = userInfo.Surname;
            DateOfBirthPicker.SelectedDate = userInfo.BirthDate;
            SexComboBox.SelectedIndex = GetIndexOfSex(userInfo.Sex);

            if (userInfo.Country != null)
            {
                CountryComboBox.SelectedIndex = GetIndexOfCountry(userInfo.Country);
            }
        }

        private int GetIndexOfSex(SexDTO sex)
        {
            for (int i = 0; i < SexComboBox.Items.Count; i++)
            {
                if ((SexComboBox.Items[i] as SexDTO).Name == sex.Name)
                {
                    return i;
                }
            }
            return -1;
        }
        private int GetIndexOfCountry(CountryDTO country)
        {
            for (int i = 0; i < CountryComboBox.Items.Count; i++)
            {
                if ((CountryComboBox.Items[i] as CountryDTO).Name == country.Name)
                {
                    return i;
                }
            }
            return -1;
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChangeUserInfo();
                DialogHost.CloseDialogCommand.Execute(null, null);
            }
            catch (Exception ex)
            {
                ShowSampleMessageDialog(ex.Message);
            }
        }

        private UserInfo GetUserInfo()
        {
            UserInfo userDetails = new UserInfo()
            {
                Photo = this.Photo,
                Name = NameTextBox.Text,
                Surname = SurnameTextBox.Text,
                BirthDate = DateOfBirthPicker.SelectedDate.Value,
                Sex = SexComboBox.SelectedItem as SexDTO,
                Country = CountryComboBox.SelectedItem as CountryDTO,
            };

            if (IsUpdatePassword())
            {
                userDetails.Password = PasswordBox.Password;
            }

            return userDetails;
        }
        private void ChangeUserInfo()
        {
            if (!IsSetedRequiredFields())
            {
                throw new Exception("Some of required fields are not setted!");
            }

            else if (IsUpdatePassword() && !IsComparePassword())
            {
                throw new Exception("Passwords don't match");
            }

            UpdateResult result = _bll.UpdateUserInfo(GetUserInfo());

            switch (result)
            {
                case UpdateResult.Success:
                    break;
                case UpdateResult.NameIsInvalid:
                    throw new Exception("Name is invalid or empty!");
                case UpdateResult.SurnameIsInvalid:
                    throw new Exception("Surname is invalid or empty!");
                case UpdateResult.PasswordIsInvalid:
                    throw new Exception("Password is invalid!");
                case UpdateResult.BirthDateIsInvalid:
                    throw new Exception("Date of birth is invalid or not selected!");
                case UpdateResult.SexIsInvalid:
                    throw new Exception("Sex is not selected!");
            }
        }

        private bool IsSetedRequiredFields()
        {
            return !String.IsNullOrEmpty(NameTextBox.Text)
                && !String.IsNullOrEmpty(SurnameTextBox.Text)
                && DateOfBirthPicker.SelectedDate != null
                && SexComboBox.SelectedIndex != -1;
        }
        private bool IsUpdatePassword()
        {
            return !String.IsNullOrEmpty(PasswordBox.Password)
                || !String.IsNullOrEmpty(ConfirmPasswordBox.Password);
        }
        private bool IsComparePassword()
        {
            return PasswordBox.Password == ConfirmPasswordBox.Password;
        }

        private void ShowSampleMessageDialog(string content)
        {
            var sampleMessageDialog = new SampleMessageDialog
            {
                Message = { Text = content }
            };

            DialogHost.Show(sampleMessageDialog, "RootDialog");
        }

        private System.Drawing.Image LoadImage()
        {
            System.Drawing.Image image = null;

            OpenFileDialog open = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*"
            };

            if (open.ShowDialog().Value == true)
            {
                byte[] imageByteArray;

                using (FileStream fs = new FileStream(open.FileName, FileMode.Open))
                {
                    imageByteArray = new byte[fs.Length];
                    fs.Read(imageByteArray, 0, imageByteArray.Length);
                }

                image = Util.ByteArrayToImage(imageByteArray);
            }

            return image;
        }
        private void PhotoEllipse_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var image = LoadImage();

            if (image != null)
            {
                Photo = image;
            }
        }
    }
}
