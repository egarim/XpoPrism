using System;
using System.Collections.ObjectModel;
using DevExpress.Xpo;
using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using XpoPrism.Infrastructure;
using System.Linq;
using System.ComponentModel;

namespace XpoPrism.ViewModels
{
    public class ContactPageViewModel : AppMapViewModelBase
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
        
        public ContactPageViewModel(INavigationService navigationService) : base(navigationService)
        {



            this.SaveCommand  = new DelegateCommand(Save, CanSave);

        }
        protected virtual bool CanSave()
        {
            return !this.IsBusy;
        }
     

        public DelegateCommand SaveCommand { get; private set; }

        bool isBusy;
        INotifyPropertyChanged data;
        public INotifyPropertyChanged Data
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
       
        public void OnChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine(e.PropertyName); 
        }
    
        private UnitOfWork UoW;
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            this.IsBusy = true;
            base.OnNavigatedTo(parameters);
            this.UoW = XpoHelper.CreateUnitOfWork();
            this.Data =new Contact(UoW);
            this.data.PropertyChanged += OnChanged;
            this.IsBusy = false;
        }
        protected async void Save()
        {
            if (this.UoW.InTransaction)
            {
                await this.UoW.CommitChangesAsync();
                await this.NavigationService.GoBackAsync();
            }
            
        }
    }
}

