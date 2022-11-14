using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace WpfApplication
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private Library selectedLibrary;
        
        public ObservableCollection<Library> Libraries { get; set; }
        public Library SelectedLibrary
        {
            get { return selectedLibrary; }
            set
            {
                selectedLibrary = value;
                OnPropertyChanged("SelectedLibrary");
            }
        }

        public ApplicationViewModel()
        {
            selectedLibrary = new Library();
            selectedLibrary.Title = "TestLibrary.dll";
            selectedLibrary.Path = "C:/Users/Asus/Desktop/учеба/спп/3/AssemblyBrowser/TestLibrary/bin/Debug/net6.0";
            Libraries= new ObservableCollection<Library>() { selectedLibrary };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


    }
}