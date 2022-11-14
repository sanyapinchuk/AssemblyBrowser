using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
 
namespace WpfApplication
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private LibraryInfo library;

        public ObservableCollection<LibraryInfo> LibraryInfos { get; set; }
        public LibraryInfo SelectLibraryInfo
        {
            get { return library; }
            set
            {
                library = value;
                OnPropertyChanged("SelectedPhone");
            }
        }

        public ApplicationViewModel()
        {
            library = new LibraryInfo();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}