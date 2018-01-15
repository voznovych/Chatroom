using BLL;
using UI.Domain;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UI
{
    public enum Operation { SIGN_IN, SIGN_UP };

    /// <summary>
    /// Interaction logic for Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        private readonly BLLclass _bll;
        private Operation selectedOp;

        public Operation SelectedOp
        {
            get => selectedOp;
            set
            {
                switch (value)
                {
                    case Operation.SIGN_IN:
                        SignInCard.Background = this.FindResource("PrimaryHueDarkBrush") as Brush;
                        SignInCard.Foreground = this.FindResource("PrimaryHueDarkForegroundBrush") as Brush;

                        SignUpCard.Background = this.FindResource("SecondaryAccentBrush") as Brush;
                        SignUpCard.Foreground = this.FindResource("SecondaryAccentForegroundBrush") as Brush;

                        SignUpAdditionalGrid.Visibility = Visibility.Collapsed;
                        selectedOp = value;
                        break;

                    case Operation.SIGN_UP:
                        SignUpCard.Background = this.FindResource("PrimaryHueDarkBrush") as Brush;
                        SignUpCard.Foreground = this.FindResource("PrimaryHueDarkForegroundBrush") as Brush;

                        SignInCard.Background = this.FindResource("SecondaryAccentBrush") as Brush;
                        SignInCard.Foreground = this.FindResource("SecondaryAccentForegroundBrush") as Brush;

                        SignUpAdditionalGrid.Visibility = Visibility.Visible;
                        selectedOp = value;
                        break;
                }
            }
        }

        public Authorization()
        {
            InitializeComponent();

            _bll = new BLLclass();
            DataContext = new TextFieldsViewModel();

            SelectedOp = Operation.SIGN_IN;

            SexComboBox.ItemsSource = _bll.GetAllSexes();
            SexComboBox.DisplayMemberPath = "Name";

            CountryComboBox.ItemsSource = _bll.GetAllCountries();
            CountryComboBox.DisplayMemberPath = "Name";
        }

        private bool IsSetedRequiredFieldsForSignIn()
        {
            return String.IsNullOrEmpty(PasswordBox.Password); // is seted password field
        }
        private bool IsSetedRequiredFieldsForSignUp()
        {
            return IsSetedRequiredFieldsForSignIn() // is seted sign in fields
                && !String.IsNullOrEmpty(ConfirmedPasswordBox.Password) // is seted confirmed password
                && !String.IsNullOrEmpty(FirstNameTextBox.Text) // is seted first name
                && !String.IsNullOrEmpty(LastNameTextBox.Text) // is seted last name
                && SexComboBox.SelectedIndex != -1; // is seted sex
        }

        private void OpenChat(UserDTO user)
        {
            MainWindow chat = new MainWindow(_bll);

            chat.Loaded += (send, k) =>
            {
                _bll.SetOnlineStatus(user.Id);
            };

            chat.Closed += (send, k) =>
            {
                _bll.SetOfflineStatus(user.Id);

                var authorization = new Authorization();
                authorization.Show();
            };

            chat.Show();
            this.Close();
        }

        private UserDTO SignUp()
        {
            // is seted required fields for sign up
            if (IsSetedRequiredFieldsForSignUp())
            {
                // reauired fields
                var login = LoginTextBox.Text;
                var password = Util.GetHashString(PasswordBox.Password);
                var confirmedPassword = Util.GetHashString(ConfirmedPasswordBox.Password);
                var name = FirstNameTextBox.Text;
                var surname = LastNameTextBox.Text;
                var dateOfBirth = DateOfBirthPicker.SelectedDate;
                var sex = SexComboBox.SelectedItem as SexDTO;

                // not required fields
                CountryDTO country = null;
                if (CountryComboBox.SelectedIndex != -1)
                {
                    country = CountryComboBox.SelectedItem as CountryDTO;
                }

                if (_bll.IsLoginExist(login))
                {
                    throw new Exception("Login is already exist.");
                }

                if (password != confirmedPassword)
                {
                    throw new Exception("Passwords do not match.");
                }

                byte[] photo = null;
                switch (sex.Name)
                {
                    case "Male":
                        photo = File.ReadAllBytes("Resources/avatar_m.png");
                        break;
                    case "Female":
                        photo = File.ReadAllBytes("Resources/avatar_f.png");
                        break;
                    default:
                        photo = File.ReadAllBytes("Resources/no_photo.png");
                        break;
                }


                var user = new UserDTO()
                {
                    Login = login,
                    Password = password,
                    Name = name,
                    Surname = surname,
                    DateOfBirth = dateOfBirth,
                    SexId = sex.Id,
                    CountryId = country?.Id,
                    StatusId = _bll.GetOfflineUserStatus().Id,
                    Photo = Util.ByteArrayToImage(photo)
                };

                return _bll.AddUser(user);
            }

            else
            {
                throw new Exception("Enter your login,\n" +
                    "password, confirmed password,\n" +
                    "first name, last name and sex.");
            }
        }
        private UserDTO SignIn()
        {
            // is seted required fields for sign in
            if (IsSetedRequiredFieldsForSignIn())
            {
                var login = LoginTextBox.Text;
                var password = Util.GetHashString(PasswordBox.Password);

                var user = _bll.GetUserByLoginAndPassword(login, password);

                if (user == null)
                {
                    throw new Exception("Invalid login or password.");
                }

                return user;
            }

            else
            {
                throw new Exception("Enter your login and password");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (SelectedOp)
                {
                    case Operation.SIGN_IN:
                        OpenChat(SignIn());
                        break;
                    case Operation.SIGN_UP:
                        OpenChat(SignUp());
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowSampleMessageDialog(ex.Message);
            }
        }

        private async void ShowSampleMessageDialog(string content)
        {
            var sampleMessageDialog = new SampleMessageDialog
            {
                Message = { Text = content }
            };
            
            await DialogHost.Show(sampleMessageDialog, "RootDialog");
        }

        private void CardSignIn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SelectedOp = Operation.SIGN_IN;
        }
        private void CardSignUp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SelectedOp = Operation.SIGN_UP;
        }
    }
}