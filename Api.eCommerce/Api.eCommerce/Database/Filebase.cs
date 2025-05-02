using Library.eCommerce.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Api.eCommerce.Database
{
    public class Filebase
    {
        private string _root;
        private string _productRoot;
        private static Filebase _instance;


        public static Filebase Current
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Filebase();
                }

                return _instance;
            }
        }

        private Filebase()
        {
            _root = @"C:\\temp";
            _productRoot = $"{_root}\\Products";

            //create the directory if it doesn't exist
            if (!Directory.Exists(_productRoot))
            {
                Directory.CreateDirectory(_productRoot);
            }
        }

        public int LastKey
        {
            get
            {
                if (Inventory.Any())
                {
                    return Inventory.Select(x => x.Id).Max();
                }
                return 0;
            }
        }

        public Item AddOrUpdate(Item item)
        {
            //set up a new Id if one doesn't already exist
            if (item.Id <= 0)
            {
                item.Id = LastKey + 1;
            }

            //go to the right place
            string path = $"{_productRoot}\\{item.Id}.json";


            //if the item has been previously persisted
            if (File.Exists(path))
            {
                //blow it up
                File.Delete(path);
            }

            //write the file
            File.WriteAllText(path, JsonConvert.SerializeObject(item));

            //return the item, which now has an id
            return item;
        }

        public List<Item?> Inventory
        {
            get
            {
                var root = new DirectoryInfo(_productRoot);
                var _patients = new List<Item>();
                foreach (var patientFile in root.GetFiles())
                {
                    var patient = JsonConvert
                        .DeserializeObject<Item>
                        (File.ReadAllText(patientFile.FullName));
                    if (patient != null)
                    {
                        _patients.Add(patient);
                    }

                }
                return _patients;
            }
        }


      
            public bool Delete(string type, string id)
        {
            // determine which folder to delete from
            string rootPath;
            switch (type.ToLowerInvariant())
            {
                case "product":
                    rootPath = _productRoot;
                    break;
                // add more cases here if you have other types, e.g. "category", "order", etc.
                default:
                    throw new ArgumentException($"Unknown delete type: {type}", nameof(type));
            }

            // build the file path
            var fileName = $"{id}.json";
            var path = Path.Combine(rootPath, fileName);

            // if the file exists, delete it and return true; otherwise return false
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }

            return false;
        }

    }
}

