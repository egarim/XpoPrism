
using BIT.Xpo.Providers.WebApi.Client;
using DevExpress.Xpo;
using Orm;
using System;
using System.Linq;

namespace XpoWebApiConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start");
            Console.ReadKey();

            XPOWebApi.Register();

           var  connectionString = XPOWebApi.GetConnectionString("http://192.168.122.101/BitServer", string.Empty, "db1");


            XpoHelper.InitXpo(connectionString);



            using (var UoW = XpoHelper.CreateUnitOfWork())
            {
                if (UoW.Query<Contact>().Count() == 0)
                {
                    Contact Joche = new Contact(UoW) { Name = "Jose Manuel Ojeda Melgar", Phone = "+7897654321" };
                    Contact Javier = new Contact(UoW) { Name = "Jose Javier Columbie", Phone = "+1897654321" };
                    Contact Jaime = new Contact(UoW) { Name = "Jaime Macias", Phone = "+123423431" };
                    Contact Rafael = new Contact(UoW) { Name = "Rafael Gonzales", Phone = "+14305345345" };
                }

                if (UoW.InTransaction)
                    UoW.CommitChanges();
            }


            Console.WriteLine("Hello World XPO!");
        }
    }
}
