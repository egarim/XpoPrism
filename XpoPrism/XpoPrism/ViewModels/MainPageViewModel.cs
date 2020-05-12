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
    public class MainPageViewModel : AppMapViewModelBase
    {


        public MainPageViewModel(INavigationService navigationService) : base (navigationService)
        {
        }


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

            base.OnNavigatedTo(parameters);
            this.UoW = XpoHelper.CreateUnitOfWork();
            this.LoadData();
        }
    }
}

