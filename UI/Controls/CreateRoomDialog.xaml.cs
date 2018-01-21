using BLL;
using Microsoft.Win32;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;

namespace UI.Controls
{
    /// <summary>
    /// Interaction logic for CreateRoomDialog.xaml
    /// </summary>
    public partial class CreateRoomDialog : UserControl
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

        public CreateRoomDialog(BLLClass bll)
        {
            InitializeComponent();
            _bll = bll;
            Photo = Util.ByteArrayToImage(File.ReadAllBytes("Resources/if_photo_icon.jpg"));
            GenreComboBox.ItemsSource = _bll.GetAllGenres();
            GenreComboBox.DisplayMemberPath = "Name";
        }

        private System.Drawing.Image LoadImage()
        {
            System.Drawing.Image image = null;

            OpenFileDialog open = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*"
            };

            if (open.ShowDialog() == true)
            {
                string fileName = open.FileName;
                string shortFileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);

                byte[] imageByteArray;

                using (FileStream fs = new FileStream(fileName, FileMode.Open))
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
            Photo = LoadImage();
        }

        private CreateRoomData GetCreateRoomData()
        {
            return new CreateRoomData()
            {
                Name = NameTextBox.Text,
                Description = DescriptionTextBox.Text,
                Photo = this.Photo,
                IsPersonal = false,
                IsPrivate = IsPrivateChechBox.IsChecked.Value,
                Genre = GenreComboBox.SelectedItem as GenreDTO
            };
        }

        private void AcceptButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _bll.CreateRoom(GetCreateRoomData());
        }
    }
}
