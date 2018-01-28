using BLL;
using UI.Domain;
using MaterialDesignThemes.Wpf;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using BLL.DTO_Enteties;

namespace UI
{
    public enum Operation { SIGN_IN, SIGN_UP };

    /// <summary>
    /// Interaction logic for Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        private readonly BLLClass _bll;
        private Operation _selectedOp;

        public Operation SelectedOp
        {
            get { return _selectedOp; }
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
                        _selectedOp = value;
                        break;

                    case Operation.SIGN_UP:
                        SignUpCard.Background = this.FindResource("PrimaryHueDarkBrush") as Brush;
                        SignUpCard.Foreground = this.FindResource("PrimaryHueDarkForegroundBrush") as Brush;

                        SignInCard.Background = this.FindResource("SecondaryAccentBrush") as Brush;
                        SignInCard.Foreground = this.FindResource("SecondaryAccentForegroundBrush") as Brush;

                        SignUpAdditionalGrid.Visibility = Visibility.Visible;
                        _selectedOp = value;
                        break;
                }
            }
        }

        public Authorization()
        {
            InitializeComponent();

            _bll = new BLLClass();
            DataContext = new TextFieldsViewModel();

            SelectedOp = Operation.SIGN_IN;

            SexComboBox.ItemsSource = _bll.GetAllSexes();
            SexComboBox.DisplayMemberPath = "Name";

            CountryComboBox.ItemsSource = _bll.GetAllCountries();
            CountryComboBox.DisplayMemberPath = "Name";

            //LoginTextBox.Text = "tests44";
            //PasswordBox.Password = "123456";
            //SignIn();
        }

        private bool IsSetedRequiredFieldsForSignIn()
        {
            return !String.IsNullOrEmpty(LoginTextBox.Text)
                && !String.IsNullOrEmpty(PasswordBox.Password); // is seted password field
        }
        private bool IsSetedRequiredFieldsForSignUp()
        {
            return !String.IsNullOrEmpty(LoginTextBox.Text)
                && !String.IsNullOrEmpty(PasswordBox.Password)
                && !String.IsNullOrEmpty(ConfirmedPasswordBox.Password)
                && !String.IsNullOrEmpty(FirstNameTextBox.Text)
                && !String.IsNullOrEmpty(LastNameTextBox.Text)
                && SexComboBox.SelectedIndex != -1
                && DateOfBirthPicker.SelectedDate != null;
        }

        private void SignUp()
        {
            if (!IsSetedRequiredFieldsForSignUp())
            {
                throw new Exception("Some of required fields are not setted!");
            }

            if (PasswordBox.Password != ConfirmedPasswordBox.Password)
            {
                throw new Exception("Passwords don't match");
            }

            SignUpUserData registrationData = new SignUpUserData()
            {
                Login = LoginTextBox.Text,
                Password = PasswordBox.Password,
                Name = FirstNameTextBox.Text,
                Surname = LastNameTextBox.Text,
                BirthDate = DateOfBirthPicker.SelectedDate.Value,
                Sex = SexComboBox.SelectedItem as SexDTO,
                Country = CountryComboBox.SelectedItem as CountryDTO
            };

            switch (registrationData.Sex.Name)
            {
                case "Male":
                    registrationData.Photo = File.ReadAllBytes("Resources/avatar_m.png");
                    break;
                case "Female":
                    registrationData.Photo = File.ReadAllBytes("Resources/avatar_f.png");
                    break;
                default:
                    registrationData.Photo = File.ReadAllBytes("Resources/no_photo.png");
                    break;
            }

            RegistrationResult result = _bll.SignUp(registrationData);

            switch (result)
            {
                case RegistrationResult.Success:
                    SignIn();
                    break;
                case RegistrationResult.LoginIsAlreadyExist:
                    throw new Exception("Login is already exist!");
                case RegistrationResult.LoginIsInvalidOrEmpty:
                    throw new Exception("Login is invalid or empty!");
                case RegistrationResult.NameIsInvalidOrEmpty:
                    throw new Exception("Name is invalid or empty!");
                case RegistrationResult.SurnameIsInvalidOrEmpty:
                    throw new Exception("Surname is invalid or empty!");
                case RegistrationResult.PasswordIsInvalidOrEmpty:
                    throw new Exception("Password is invalid or empty!");
                case RegistrationResult.SexIsInvalidOrNotSelected:
                    throw new Exception("Sex is not selected!");
                case RegistrationResult.BirthDateIsInvalidOrNotSelected:
                    throw new Exception("Date of birth is invalid or not selected!");
            }
        }
        private void SignIn()
        {
            if (!IsSetedRequiredFieldsForSignIn())
            {
                throw new Exception("Some of required fields are not setted!");
            }

            LoginResult result = _bll.Login(LoginTextBox.Text, PasswordBox.Password);

            if (result == LoginResult.Success)
            {
                new MainWindow(_bll).Show();
                Close();
            }
            else if (result == LoginResult.LoginIsNotExist)
            {
                throw new Exception("Such login isn't exist!");
            }
            else if (result == LoginResult.PasswordIsWrong)
            {
                throw new Exception("Wrong password!");
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
                        SignIn();
                        break;
                    case Operation.SIGN_UP:
                        SignUp();
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowSampleMessageDialog(ex.Message);
            }
        }

        private void ShowSampleMessageDialog(string content)
        {
            var sampleMessageDialog = new SampleMessageDialog
            {
                Message = { Text = content }
            };

            DialogHost.Show(sampleMessageDialog, "RootDialog");
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