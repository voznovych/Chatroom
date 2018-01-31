using BLL;
using BLL.DTO_Enteties;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using UI.Domain;

namespace UI.Controls
{
    enum Operation
    {
        Create,
        Update,
    }
    /// <summary>
    /// Interaction logic for CreateRoomDialog.xaml
    /// </summary>
    public partial class RoomInfoDialog : UserControl
    {
        private readonly BLLClass _bll;
        private Operation _op;
        private RoomDTO _room;

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

        public RoomInfoDialog(BLLClass bll)
        {
            InitializeComponent();
            TextBlockTitle.Text = "CREATE ROOM";
            _bll = bll;

            Photo = Util.ByteArrayToImage(File.ReadAllBytes("Resources/if_photo_icon.jpg"));
            GenreComboBox.ItemsSource = _bll.GetAllGenres();
            GenreComboBox.DisplayMemberPath = "Name";

            _op = Operation.Create;
        }
        public RoomInfoDialog(BLLClass bll, RoomDTO room)
        {
            InitializeComponent();
            TextBlockTitle.Text = "ROOM";
            _bll = bll;
            _room = room;

            GenreComboBox.ItemsSource = _bll.GetAllGenres();
            GenreComboBox.DisplayMemberPath = "Name";

            Photo = room.Photo ?? Util.ByteArrayToImage(File.ReadAllBytes("Resources/if_photo_icon.jpg"));
            NameTextBox.Text = room.Name;
            DescriptionTextBox.Text = room.Description;
            GenreComboBox.SelectedIndex = GetIndexOfGenre(room.Genre);
            IsPrivateChechBox.IsChecked = room.IsPrivate;

            _op = Operation.Update;
        }

        private int GetIndexOfGenre(GenreDTO genre)
        {
            for (int i = 0; i < GenreComboBox.Items.Count; i++)
            {
                if ((GenreComboBox.Items[i] as GenreDTO).Name == genre.Name)
                {
                    return i;
                }
            }
            return -1;
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
            var image = LoadImage();
            if (image != null)
            {
                Photo = image;
            }
        }

        private bool IsSetedRequiredFields()
        {
            return !String.IsNullOrEmpty(NameTextBox.Text)
                && GenreComboBox.SelectedIndex != -1;
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
        private void SetRoomDTO()
        {
            _room.Name = NameTextBox.Text;
            _room.Description = DescriptionTextBox.Text;
            _room.Photo = this.Photo;
            _room.IsPrivate = IsPrivateChechBox.IsChecked.Value;
            _room.GenreId = (GenreComboBox.SelectedItem as GenreDTO).Id;
        }
        private void CreateRoom()
        {
            if (!IsSetedRequiredFields())
            {
                throw new Exception("Some of required fields are not setted!");
            }

            _bll.CreateRoom(GetCreateRoomData());            
        }
        private void UpdateRoom()
        {
            if (!IsSetedRequiredFields())
            {
                throw new Exception("Some of required fields are not setted!");
            }

            SetRoomDTO();
            _bll.UpdateRoom(_room);
        }

        private void AcceptButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (_op == Operation.Create)
                {
                    CreateRoom();
                }
                else if (_op == Operation.Update)
                {
                    UpdateRoom();
                }
                DialogHost.CloseDialogCommand.Execute(null, null);
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
    }
}
