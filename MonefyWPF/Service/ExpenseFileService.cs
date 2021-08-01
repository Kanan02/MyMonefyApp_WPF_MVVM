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
    public class ExpenseFileService : IExpenseFileService
    {
        public ObservableCollection<Expense> Open(string fileName)
        {
            var trans = new ObservableCollection<Expense>();
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(ObservableCollection<Expense>));
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                trans = jsonFormatter.ReadObject(fs) as ObservableCollection<Expense>;
            }

            return trans;
        }

        public void Save(string fileName, ObservableCollection<Expense> trans)
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(ObservableCollection<Expense>));
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, trans);
            }
        }
    }
}
