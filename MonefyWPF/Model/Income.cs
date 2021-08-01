using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonefyWPF.Model
{
    public class Income
    {
        public Income(double value, DateTime date,string note)
        {
            Value = value;
            Date = date;
            Note = note;
        }
        public Income()
        {

        }
        public double Value { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }

    }
}
