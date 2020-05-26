using System;
using System.Collections.ObjectModel;
using DevExpress.Xpo;
using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using XpoPrism.Infrastructure;
using System.Linq;
namespace XpoPrism.ViewModels
{
    public class ContactListViewModel : AppMapViewModelBase
    {

        
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy == value)
                    return;
                isBusy = value;
                RaisePropertyChanged();
            }
        }
        
        public ContactListViewModel(INavigationService navigationService) : base(navigationService)
        {

            SearchCommand = new DelegateCommand<string>(SearchExecute, CanSearch);
            AddNewContactCommand = new DelegateCommand(AddNewContactExecute, CanAddNewContact);
            this.PropertyChanged += ViewModelPropertyChanged;


        }
        protected async virtual void AddNewContactExecute()
        {
            await NavigationService.NavigateAsync("ContactPage");
        }
        protected virtual bool CanAddNewContact()
        {
            return !this.IsBusy;
        }
        protected virtual bool CanSearch(string arg)
        {
            return !this.IsBusy;
        }
        string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                if (searchText == value)
                    return;
                searchText = value;
                RaisePropertyChanged();
            }
        }


        public DelegateCommand<string> SearchCommand { get; private set; }
        public DelegateCommand AddNewContactCommand { get; private set; }

        bool isBusy;
        ObservableCollection<dynamic> data;
        public ObservableCollection<dynamic> Data
        {
            get { return data; }
            set
            {
                if (data == value)
                    return;
                data = value;
                RaisePropertyChanged();
            }
        }
        protected  void SearchExecute(string obj)
        {

            if (string.IsNullOrEmpty(obj) || string.IsNullOrWhiteSpace(obj))
            {
                this.LoadData();
                return;
            }

            var list = from c in this.UoW.Query<Contact>()
                       where (c.Name.Contains(obj) || c.Phone.Contains(obj))
                       orderby c.Name
                       select new { Name = c.Name, Phone = c.Phone, Oid = c.Oid };
            Data.Clear();

            foreach (var item in list)
            {
                Data.Add(item);
            }
            this.RaisePropertyChanged("Data");
        }
        protected virtual void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SearchText))
            {
                if (string.IsNullOrEmpty(SearchText) || string.IsNullOrWhiteSpace(SearchText))
                {
                    this.LoadData();
                }
            }
        }
        protected  void LoadData()
        {
       

            var list = from c in this.UoW.Query<Contact>()
                      
                       orderby c.Name
                       select new { Name = c.Name, Phone = c.Phone, Oid = c.Oid };
            Data.Clear();

            foreach (var item in list)
            {
                Data.Add(item);
            }
            this.RaisePropertyChanged("Data");
        }
        private UnitOfWork UoW;
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            this.IsBusy = true;
            base.OnNavigatedTo(parameters);
            this.UoW = XpoHelper.CreateUnitOfWork();
            this.Data = new ObservableCollection<dynamic>();
            this.LoadData();
            this.IsBusy = false;
        }
    }
}

