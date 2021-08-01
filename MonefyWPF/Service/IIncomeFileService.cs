using MonefyWPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonefyWPF.Service
{
    public interface IIncomeFileService
    {
        ObservableCollection<Income> Open(string fileName);
        void Save(string fileName, ObservableCollection<Income> trans);
    }
}
