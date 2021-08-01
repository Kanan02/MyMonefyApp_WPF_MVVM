using MonefyWPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace MonefyWPF.Service
{
    class CategoryFileService : ICategoryFileService
    {
        
        
        public ObservableCollection<Category> Open(string fileName)
        {
            var trans = new ObservableCollection<Category>();
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(ObservableCollection<Category>));
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                trans = jsonFormatter.ReadObject(fs) as ObservableCollection<Category>;
            }

            return trans;
        }

        public void Save(string fileName, ObservableCollection<Category> trans)
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(ObservableCollection<Category>));
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, trans);
            }
        }
    }
}
