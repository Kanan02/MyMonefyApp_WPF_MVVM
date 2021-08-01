using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MonefyWPF.Model;
using MonefyWPF.Service;
using MonefyWPF.Service.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace MonefyWPF.ViewModel
{
    public class MainViewModel:INotifyPropertyChanged
    {
        public IExpenseFileService ExpenseFileService { get; set; }
        public ICategoryFileService CategoryFileService { get; set; }
        public IIncomeFileService IncomeFileService { get; set; }
        public ObservableCollection<Expense> Expenses { get; set; }
       
        public ObservableCollection<Income> Incomes { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public SeriesCollection SeriesColl { get; set; }
        RelayCommand addCommand;
        public RelayCommand AddCommand { get => addCommand; set => addCommand = value; }

        RelayCommand dayCommand;
        public RelayCommand DayCommand { get => dayCommand; set => dayCommand = value; }

        RelayCommand monthCommand;
        public RelayCommand MonthCommand { get => monthCommand; set => monthCommand = value; }
        RelayCommand yearCommand;
        public RelayCommand YearCommand { get => yearCommand; set => yearCommand = value; }

        RelayCommand allCommand;
        public RelayCommand AllCommand { get => allCommand; set => allCommand = value; }
        RelayCommand dateSortCommand;
        public RelayCommand DateSortCommand { get => dateSortCommand; set => dateSortCommand = value; }
        RelayCommand amountSortCommand;
        public RelayCommand AmountSortCommand { get => amountSortCommand; set => amountSortCommand = value; }

        RelayCommand changeDarkModeCommand;
        public RelayCommand ChangeDarkModeCommand { get => changeDarkModeCommand; set => changeDarkModeCommand = value; }

        RelayCommand usdCommand;
        public RelayCommand USDCommand { get => usdCommand; set => usdCommand = value; }
        RelayCommand azeCommand;
        public RelayCommand AZECommand { get => azeCommand; set => azeCommand = value; }
        RelayCommand rubCommand;
        public RelayCommand RUBCommand { get => rubCommand; set => rubCommand = value; }

        public MainViewModel(IExpenseFileService expenseFileService, ICategoryFileService categoryFileService,IIncomeFileService incomeFileService)
        {
            ExpenseFileService = expenseFileService;
            CategoryFileService = categoryFileService;
            IncomeFileService = incomeFileService;
            Expenses = new ObservableCollection<Expense>();
            Incomes = new ObservableCollection<Income>();
            ExpensesList = new ObservableCollection<Expense>();
            IncomesList = new ObservableCollection<Income>();
            if (FileExists("expenses.json"))
            {
                Expenses = expenseFileService.Open("expenses.json");
            }
            if (FileExists("incomes.json"))
            {
                Incomes = incomeFileService.Open("incomes.json");
            }
            usdCommand = new RelayCommand((obj) =>Currency = "$",()=>Currency!="$");
            azeCommand = new RelayCommand((obj) =>Currency = "₼", ()=>Currency!= "₼");
            rubCommand = new RelayCommand((obj) =>Currency = "₽", ()=>Currency!= "₽");
            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals(nameof(Currency)))
                {
                   usdCommand.RiseCanExecuteChange();
                   azeCommand.RiseCanExecuteChange();
                   rubCommand.RiseCanExecuteChange();
                }
            };
            changeDarkModeCommand = new RelayCommand(obj =>
            {
                if (Background== "#D0FFDE")
                {
                    Background = "#B5B3B3";
                    PanelColor = "#50CB75";
                }
                else
                {
                    Background = "#D0FFDE";
                    PanelColor = "#8AFFA2";
                }
            });
            dateSortCommand = new RelayCommand(obj => {
                SelectedSort = "Date";
                List<Expense> expenses= ExpensesList.OrderBy(x => x.Date).ToList();
                ExpensesList = new ObservableCollection<Expense>();
                foreach (var item in expenses)
                {
                    ExpensesList.Add(item);
                }

                List<Income> incomes=IncomesList.OrderBy(x => x.Date).ToList();
                IncomesList = new ObservableCollection<Income>();
                foreach (var item in incomes)
                {
                    IncomesList.Add(item);
                }
                
            }, () => SelectedSort != "Date");

            amountSortCommand = new RelayCommand(obj => {
                SelectedSort = "Amount";
                List<Expense> expenses= ExpensesList.OrderBy(x => x.Amount).ToList();
                ExpensesList = new ObservableCollection<Expense>();
                foreach (var item in expenses)
                {
                    ExpensesList.Add(item);
                }
                List<Income> incomes = IncomesList.OrderBy(x => x.Value).ToList();
                IncomesList = new ObservableCollection<Income>();
                foreach (var item in incomes)
                {
                    IncomesList.Add(item);
                }
            }, () => SelectedSort != "Amount");
            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals(nameof(SelectedSort)))
                {
                    dateSortCommand.RiseCanExecuteChange();
                    amountSortCommand.RiseCanExecuteChange();
                }
            };


            dayCommand = new RelayCommand(obj => {
                SelectedTime = "Day";
                UpdateUIChartNewDate();
                
            },()=> SelectedTime != "Day");

            monthCommand = new RelayCommand(obj => {
                SelectedTime = "Month";
                UpdateUIChartNewDate();

            }, () => SelectedTime != "Month");

            yearCommand = new RelayCommand(obj => {
                SelectedTime = "Year";
                UpdateUIChartNewDate();

            }, () => SelectedTime != "Year");

            allCommand = new RelayCommand(obj =>
            {
                SelectedTime = "All";
                  UpdateUIChartNewDate();
            }, () => SelectedTime != "All");
            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName.Equals(nameof(SelectedTime)))
                {
                    dayCommand.RiseCanExecuteChange();
                    monthCommand.RiseCanExecuteChange();
                    yearCommand.RiseCanExecuteChange();
                    allCommand.RiseCanExecuteChange();
                }
            };
            addCommand = new RelayCommand(obj => {
                if (NewValue=="")
                {
                    newValue = "0";
                }
                if (NewNote=="")
                {
                    NewNote = "Empty";
                }
                if (obj as string == "income")
                {
                    Incomes.Add(new Income(double.Parse(NewValue), DateTime.Now,NewNote));
                    IncomeFileService.Save("incomes.json", Incomes);
                    TotalIncomes += double.Parse(NewValue);
                    TotalValue += double.Parse(NewValue);
                }
                else
                {
                    Categories.Where(x => x.Name == obj as string).ElementAt(0).Sum += double.Parse(NewValue);
                    Expenses.Add(new Expense(double.Parse(NewValue), obj as string, DateTime.Now,NewNote));
                    ExpenseFileService.Save("expenses.json", Expenses);
                    CategoryFileService.Save("categories.json", Categories);
                    UpdateUIChartNewTransaction();
                    TotalExpenses -= double.Parse(NewValue);
                    TotalValue -= double.Parse(NewValue);
                }
                UpdateUIChartNewDate();
                SelectedCategory = "";
                NewValue = "";
                NewNote = "Empty";
            });
            if (FileExists("categories.json"))
            {
                Categories = categoryFileService.Open("categories.json");

            }
            else
            {
                Categories = new ObservableCollection<Category>()
                {
                    new Category("Restaurants","#1B5032"),
                    new Category("Toiletry","#1E2452"),
                    new Category("Food","#9CAF4E"),
                    new Category("Sport","#952EB4"),
                    new Category("Health","#648DAD"),
                    new Category("Clothes","#964850"),
                    new Category("Taxi","#FDD400"),
                    new Category("Entertainment","#E05A47"),
                    new Category("Phone","#5BB883"),
                    new Category("Transport","#DDA35F"),
                    new Category("House","#38B6FF"),
                    new Category("Car","#801724"),


                };


                categoryFileService.Save("categories.json", Categories);

            }
            SeriesColl = new SeriesCollection();
            var bc = new BrushConverter();

            for (int i = 0; i < Categories.Count; i++)
            {
                SeriesColl.Add(new PieSeries
                {
                    Title = Categories[i].Name,
                    Values = new ChartValues<ObservableValue>{
                         new ObservableValue(Categories[i].Sum)
                        },

                    Fill = (Brush)bc.ConvertFrom(Categories[i].Color),
                    DataLabels = true
                }
                );
            }
            UpdateUIChartNewDate();

        }
        void UpdateUIChartNewTransaction()
        {

            for (int i = 0; i < Categories.Count; i++)
            {
                if (SelectedCategory==SeriesColl[i].Title)
                {
                    
                    SeriesColl[i].Values = new ChartValues<ObservableValue>{
                         new ObservableValue(Categories[i].Sum)
                        };
                    break;
                }
            }   
               
         }

        void UpdateUIChartNewDate()
        {
            TotalExpenses = 0;
            TotalIncomes = 0;
            TotalValue = 0;
            ExpensesList = new ObservableCollection<Expense>();
            IncomesList = new ObservableCollection<Income>();
            for (int i = 0; i < Categories.Count; i++)
            {
                double val = 0;
                foreach (var item in Expenses.Where(x=>x.CategoryName==Categories[i].Name))
                {
                    if (selectedTime == "Day" && item.Date.Date == DateTime.Now.Date)
                    {
                        val += item.Amount;
                        ExpensesList.Add(item);

                    }
                    else if (selectedTime == "Month" && item.Date.Year == DateTime.Now.Year && item.Date.Month == DateTime.Now.Month)
                    {
                        val += item.Amount;
                        ExpensesList.Add(item);
                    }
                    else if (selectedTime == "Year" && item.Date.Year == DateTime.Now.Year)
                    {
                        val += item.Amount;
                        ExpensesList.Add(item);
                    }
                    else if (selectedTime == "All")
                    {
                        val += item.Amount;
                        ExpensesList.Add(item);
                    }
                }
                TotalExpenses -= val;
                SeriesColl[i].Values = new ChartValues<ObservableValue>{
                         new ObservableValue(val)
                        };
                
            }
            foreach (var item in Incomes)
            {
                if (selectedTime == "Day" && item.Date.Date == DateTime.Now.Date)
                {
                    TotalIncomes += item.Value;
                    IncomesList.Add(item);
                }
                else if (selectedTime == "Month" && item.Date.Year == DateTime.Now.Year && item.Date.Month == DateTime.Now.Month)
                {
                    TotalIncomes += item.Value;
                    IncomesList.Add(item);
                }
                else if (selectedTime == "Year" && item.Date.Year == DateTime.Now.Year)
                {
                    TotalIncomes += item.Value;
                    IncomesList.Add(item);
                }
                else if (selectedTime == "All")
                {
                    TotalIncomes += item.Value;
                    IncomesList.Add(item);
                }
            }
            TotalValue = TotalIncomes + TotalExpenses;

        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string newValue= "";
        public string NewValue
        {
            get => newValue;
            set
            {
                if (!newValue.Equals(value))
                {
                    newValue = value;
                    OnPropertyChanged(nameof(NewValue));
                }
            }

        }
        private string newNote = "";
        public string NewNote
        {
            get => newNote;
            set
            {
                if (!newNote.Equals(value))
                {
                    newNote = value;
                    OnPropertyChanged(nameof(NewNote));
                }
            }

        }
        private double totalValue = 0;
        public double TotalValue
        {
            get => totalValue;
            set
            {
                if (!totalValue.Equals(value))
                {
                    totalValue = value;
                    OnPropertyChanged(nameof(TotalValue));
                }
            }

        }
        private double totalExpenses = 0;
        public double TotalExpenses
        {
            get => totalExpenses;
            set
            {
                if (!totalExpenses.Equals(value))
                {
                    totalExpenses = value;
                    OnPropertyChanged(nameof(TotalExpenses));
                }
            }

        }
        private ObservableCollection<Expense> expensesList = new ObservableCollection<Expense>();
        public ObservableCollection<Expense> ExpensesList
        {
            get => expensesList;
            set
            {
                if (!expensesList.Equals(value))
                {
                    expensesList = value;
                    OnPropertyChanged(nameof(ExpensesList));
                }
            }

        }

        private ObservableCollection<Income> incomesList = new ObservableCollection<Income>();
        public ObservableCollection<Income> IncomesList
        {
            get => incomesList;
            set
            {
                if (!incomesList.Equals(value))
                {
                    incomesList = value;
                    OnPropertyChanged(nameof(IncomesList));
                }
            }

        }
        private double totalIncomes = 0;
        public double TotalIncomes
        {
            get => totalIncomes;
            set
            {
                if (!totalIncomes.Equals(value))
                {
                    totalIncomes = value;
                    OnPropertyChanged(nameof(TotalIncomes));
                }
            }

        }
        private string selectedCategory = "";
        public string SelectedCategory
        {
            get => selectedCategory;
            set
            {
                if (!selectedCategory.Equals(value))
                {
                    selectedCategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));
                }
            }

        }

        private string selectedSort = "";
        public string SelectedSort
        {
            get => selectedSort;
            set
            {
                if (!selectedSort.Equals(value))
                {
                    selectedSort = value;
                    OnPropertyChanged(nameof(SelectedSort));
                }
            }

        }
        private string background = "#D0FFDE";
        public string Background
        {
            get => background;
            set
            {
                if (!background.Equals(value))
                {
                    background = value;
                    OnPropertyChanged(nameof(Background));
                }
            }

        }
        private string panelColor = "#8AFFA2";
        public string PanelColor
        {
            get => panelColor;
            set
            {
                if (!panelColor.Equals(value))
                {
                    panelColor = value;
                    OnPropertyChanged(nameof(PanelColor));
                }
            }

        }
        private string selectedTime = "All";
        public string SelectedTime
        {
            get => selectedTime;
            set
            {
                if (!selectedTime.Equals(value))
                {
                    selectedTime = value;
                    OnPropertyChanged(nameof(SelectedTime));
                }
            }

        }
        private string currency = "$";
        public string Currency
        {
            get => currency;
            set
            {
                if (!currency.Equals(value))
                {
                    currency = value;
                    OnPropertyChanged(nameof(Currency));
                }
            }

        }
        public bool FileExists(string filename)
        {

            DirectoryInfo directoryInfo = new DirectoryInfo(AppContext.BaseDirectory);
            foreach (var item in directoryInfo.GetFiles())
            {

                if (item.Name == filename)
                {
                    return true;
                }

            }
            return false;
        }
    }
}
