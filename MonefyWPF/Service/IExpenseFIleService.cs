using MonefyWPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonefyWPF.Service
{
    public interface IExpenseFileService
    {
        ObservableCollection<Expense> Open(string fileName);
        void Save(string fileName, ObservableCollection<Expense> trans);
    }
}
