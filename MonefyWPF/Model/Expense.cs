using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MonefyWPF.Model
{
    public class Expense
    {
        public double Amount { get; set; } = 0;
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public string CategoryName { get; set; }
        public Expense()
        {

        }
        
        public Expense(double amount,string categoryName , DateTime date,string note)
        {
            Amount = amount;
            CategoryName = categoryName;
            Date = date;
            Note = note;
        }
    }
}
