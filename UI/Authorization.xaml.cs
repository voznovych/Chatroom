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
using BLL.DBO_Enteties;

namespace UI
{
    public enum Operation { SIGN_IN, SIGN_UP };

    /// <summary>
    /// Interaction logic for Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        private readonly BLLClass _bll;
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

            _bll = new BLLClass();
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
            return !String.IsNullOrEmpty(PasswordBox.Password)
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
                ShowSampleMessageDialog("Some of required fields are not setted!");
            }

            if (PasswordBox.Password != ConfirmedPasswordBox.Password)
            {
                ShowSampleMessageDialog("Passwords don't match");
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

            RegistrationResult result = _bll.SignUp(registrationData);

            switch (result)
            {
                case RegistrationResult.Success:
                    SignIn();
                    break;
                case RegistrationResult.LoginIsAlreadyExist:
                    ShowSampleMessageDialog("Login is already exist!");
                    break;
                case RegistrationResult.LoginIsInvalidOrEmpty:
                    ShowSampleMessageDialog("Login is invalid or empty!");
                    break;
                case RegistrationResult.NameIsInvalidOrEmpty:
                    ShowSampleMessageDialog("Name is invalid or empty!");
                    break;
                case RegistrationResult.SurnameIsInvalidOrEmpty:
                    ShowSampleMessageDialog("Surname is invalid or empty!");
                    break;
                case RegistrationResult.PasswordIsInvalidOrEmpty:
                    ShowSampleMessageDialog("Password is invalid or empty!");
                    break;
                case RegistrationResult.SexIsInvalidOrNotSelected:
                    ShowSampleMessageDialog("Sex is not selected!");
                    break;
                case RegistrationResult.BirthDateIsInvalidOrNotSelected:
                    ShowSampleMessageDialog("Date of birth is invalid or not selected!");
                    break;
            }
        }
        private void SignIn()
        {

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