using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace UI.Domain
{
    public class TextFieldsViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _surname;
        private string _login;
        private string _sex;

        public TextFieldsViewModel()
        {
            Name = null;
            Surname = null;
            Login = null;
            Sex = null;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                this.MutateVerbose(ref _name, value, RaisePropertyChanged());
            }
        }

        public string Surname
        {
            get { return _surname; }
            set
            {
                this.MutateVerbose(ref _surname, value, RaisePropertyChanged());
            }
        }

        public string Login
        {
            get { return _login; }
            set
            {
                this.MutateVerbose(ref _login, value, RaisePropertyChanged());
            }
        }

        public string Sex
        {
            get { return _sex; }
            set
            {
                this.MutateVerbose(ref _sex, value, RaisePropertyChanged());
            }
        }

        //public DemoItem DemoItem => new DemoItem("Mr. Test", null, Enumerable.Empty<DocumentationLink>());

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}