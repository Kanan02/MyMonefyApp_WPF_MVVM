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
    public class IncomeFileService : IIncomeFileService
    {
        public ObservableCollection<Income> Open(string fileName)
        {
            var inc = new ObservableCollection<Income>();
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(ObservableCollection<Income>));
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                inc = jsonFormatter.ReadObject(fs) as ObservableCollection<Income>;
            }

            return inc;
        }

        public void Save(string fileName, ObservableCollection<Income> inc)
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(ObservableCollection<Income>));
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, inc);
            }
        }
    }
}
