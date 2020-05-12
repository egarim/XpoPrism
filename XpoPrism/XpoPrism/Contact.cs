using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Text;

namespace XpoPrism
{
    public class Contact : XPObject
    {
        public Contact(Session session) : base(session)
        { }


        string phone;
        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }
        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Phone
        {
            get => phone;
            set => SetPropertyValue(nameof(Phone), ref phone, value);
        }

    }
}
