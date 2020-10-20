using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Timers;


namespace BankSim
{
    class Program
    {
        static List<string> HistoryList = new List<string>();
        public static List<Account> AccountList = new List<Account>();
        static void Main(string[] args)
        {
            Account acc = new Account();
            Timer ti = new Timer(1000);
            Time t = new Time(DateTime.Now, AccountList);
            t.TimeStart(ti);
            do
            {
                Console.Clear();
                Console.Write(@"Add ~ Vytvoření účtu
Manage ~ Možnost úpravy účtu
Remove ~ Zrušení účtu
Date ~ Vypsání dnešního data
Exit ~ Vypnutí aplikace
Příkaz: ");
                string decision = Console.ReadLine();

                switch (decision)
                {
                    case "Add":
                        acc.AccAdd(AccountList, HistoryList);
                        break;
                    case "Manage":
                        acc.AccManage(AccountList, HistoryList);
                        break;
                    case "Remove":
                        acc.DeleteAcc(AccountList, HistoryList);
                        break;
                    case "Date":
                        Console.WriteLine(t.ToString());
                        break;
                    case "Exit":
                        AppDomain.CurrentDomain.ProcessExit += new EventHandler(Domain_Exit);
                        break;
                }
                //Console.Write(AccountList[0].Zustatek); // jen test jestli fungují listy
                Console.ReadLine();
            } while (true);
        }

        static void Domain_Exit(object sender, EventArgs e)
        {
            FileStream fs = new FileStream("history.txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            foreach(string s in HistoryList.ToArray())
            {
                sw.WriteLine(s);
            }
            sw.Close();
            fs.Close();
        }
    }

    public class Time
    { 
        public Time(DateTime dt,List<Account> ac)
        {
            Dt = dt;
        }
        DateTime Dt { get; set; }
        List<Account> Ac { get; set; }
        public void TimeStart(System.Timers.Timer t) 
        {
            t.Enabled = true;
            t.Elapsed += TickEvent;
        }

        private void TickEvent(Object source, ElapsedEventArgs e)
        {
           Dt =  Dt.AddDays(1);
            if (Dt.Month != Dt.AddDays(-1).Month)//pokaždé, kdž se změní měsíc >>> do this
            {
                
                if (Ac != null)
                {
                    foreach (Account item in Ac)
                    {
                        Console.WriteLine(item);
                    }
                } 
                
            }
        }

        public override string ToString()
        {
            return Dt.ToString();
        }
    }

    public class Account
    {
        public Account(string typ, int vklad)
        {
            Typ = typ;
            Zustatek = vklad;
        }

        public Account()
        {

        }

        public string Typ { get; set; }
        public int Zustatek { get; set; }

        public string Owner { get; set; }

        public void AccAdd(List<Account> AccountList, List<string> HistoryList)
        {
            Console.Write("Zadejte příjmení vlastníka: ");
            string surname = Console.ReadLine();
            Console.Write("Zadejte typ účtu (Spořící/Úvěrový/Studentský): ");
            string type = Console.ReadLine();
            if (type == "Spořící" || type == "Sporici") { Sporici spAcc = new Sporici(0, surname); AccountList.Add(spAcc); HistoryList.Add($"Added {type} account owned by {surname}"); }
            if (type == "Úvěrový" || type == "Uverovy") { Uverovy uvAcc = new Uverovy(0, surname); AccountList.Add(uvAcc); HistoryList.Add($"Added {type} account owned by {surname}"); }
            if (type == "Studentský" || type == "Studentsky") { Studentsky stAcc = new Studentsky(0, surname); AccountList.Add(stAcc); HistoryList.Add($"Added {type} account owned by {surname}"); }
        }

        public void DeleteAcc(List<Account> AccountList, List<string> HistoryList)
        {
            Console.Write("Zadej jméno vlastníka, co účet maže: ");
            string surname = Console.ReadLine();
            foreach(Account acc in AccountList)
            {
                if(acc.Owner == surname) { HistoryList.Add($"Removed {acc.Typ} account owned by {surname}"); AccountList.Remove(acc);  return; }
            }
        }

        public void AccManage(List<Account> AccountList, List<string> HistoryList)
        {
            Console.Clear();
            Console.Write(@"Zadejte, co chcete dělat?
Deposit ~ vybere peníze z účtu
Add ~ Přidá penize na účet
History ~ Vypíše historii pohybů účtu
Příkaz: ");
            string decision = Console.ReadLine();
            switch (decision)
            {
                case "Add":
                    Console.Write("Zadejte vlastníka účtu: ");
                    string surname = Console.ReadLine();
                    Console.Write("Kolik se připočte: ");
                    string s = Console.ReadLine();
                    int Plus = Convert.ToInt32(s);
                    foreach (Account acc in AccountList)
                    {
                        if (acc.Owner == surname) 
                        {
                            acc.Zustatek += Plus;
                        }
                    }
                    HistoryList.Add($"Added {Plus}czk.");
                    break;
                case "Deposit":
                    Console.Write("Zadejte vlastníka účtu: ");
                    string sur = Console.ReadLine();
                    Console.Write("Kolik se odečte: ");
                    string t = Console.ReadLine();
                    int minus = Convert.ToInt32(t);
                    foreach (Account acc in AccountList)
                    {
                        if (acc.Owner == sur)
                        {
                            if(acc.Zustatek - minus > 0) { acc.Zustatek -= minus; }
                        }
                    }
                    HistoryList.Add($"Deposited {minus}czk.");
                    break;
                case "History":
                    FileStream fs = new FileStream("history.txt", FileMode.Open);
                    StreamReader sr = new StreamReader(fs);
                    
                    sr.Close();
                    fs.Close();
                    break;
            }
            Console.ReadLine();
        }
    }

    public class Sporici:Account
    {
        public Sporici(int vklad, string owner):base("Sporici", vklad)
        {
            Zustatek = vklad;
            Owner = owner;
        }
    }

    public class Uverovy:Account
    {
        public Uverovy(int vklad, string owner) : base("Uverovy", vklad)
        {
            Zustatek = vklad;
            Owner = owner;
        }
    }

    public class Studentsky:Account
    {
        public Studentsky(int vklad, string owner) :base("Studentsky", vklad)
        {
            Zustatek = vklad;
            Owner = owner;
        }
    }
}
