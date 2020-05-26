using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpoPrism.Views;
using XpoPrism.ViewModels;
using System;
using System.IO;
using DevExpress.Xpo.DB;
using DevExpress.Xpo;
using DevExpress.Data.Linq.Helpers;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XpoPrism
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            //MSSqlServer

            //string connectionString = MSSqlConnectionProvider.GetConnectionString("YOUR_SERVER_NAME", "sa", "", "XamarinDemo");

            //SQLite

            var filePath = Path.Combine(documentsPath, "xpoXamarinDemo.db");
            string connectionString = SQLiteConnectionProvider.GetConnectionString(filePath) + ";Cache=Shared;";

            //In-memory data store with saving/loading from the xml file

            //var filePath = Path.Combine(documentsPath, "xpoXamarinDemo.xml");
            //string connectionString = InMemoryDataStore.GetConnectionString(filePath);



            XpoHelper.InitXpo(connectionString);


            using(var UoW=XpoHelper.CreateUnitOfWork())
            {
                if(UoW.Query<Contact>().Count()==0)
                {
                    Contact Joche = new Contact(UoW) { Name = "Jose Manuel Ojeda Melgar", Phone = "+7897654321" };
                    Contact Javier = new Contact(UoW) { Name = "Jose Javier Columbie", Phone = "+1897654321" };
                    Contact Jaime = new Contact(UoW) { Name = "Jaime Macias", Phone = "+123423431" };
                    Contact Rafael = new Contact(UoW) { Name = "Rafael Gonzales", Phone = "+14305345345" };
                }
           
                if (UoW.InTransaction)
                    UoW.CommitChanges();
            }

            await NavigationService.NavigateAsync("NavigationPage/ContactList");
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<ContactList, ContactListViewModel>();
            containerRegistry.RegisterForNavigation<ContactPage, ContactPageViewModel>();
        }
    }
}
