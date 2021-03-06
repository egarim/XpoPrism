﻿using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using Orm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orm
{
    public static class XpoHelper
    {
        public static void GetInstance()
        {
            
        }
        static readonly Type[] entityTypes = new Type[] {
            typeof(Contact)
        };
        public static void InitXpo(string connectionString)
        {
            var dictionary = PrepareDictionary();

            if (XpoDefault.DataLayer == null)
            {
                using (var updateDataLayer = XpoDefault.GetDataLayer(connectionString, dictionary, AutoCreateOption.DatabaseAndSchema))
                {
                    updateDataLayer.UpdateSchema(false, dictionary.CollectClassInfos(entityTypes));
                }
            }

            var dataStore = XpoDefault.GetConnectionProvider(connectionString, AutoCreateOption.SchemaAlreadyExists);
            XpoDefault.DataLayer = new ThreadSafeDataLayer(dictionary, dataStore);
            XpoDefault.Session = null;


        }
        public static UnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork();
        }
        static XPDictionary PrepareDictionary()
        {
            var dict = new ReflectionDictionary();
            dict.GetDataStoreSchema(entityTypes);
            return dict;
        }
    }
}
