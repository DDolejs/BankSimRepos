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
        public static bool isusing = false;
        static void Main(string[] args)
        {
            Domain_Load(AccountList);
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
                        Environment.Exit(0);
                        break;
                }
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

            FileStream fsAcc = new FileStream("Accounts.txt", FileMode.Create);
            StreamWriter swAcc = new StreamWriter(fs);
            foreach(Account acc in AccountList)
            {
                sw.WriteLine($"{acc.Typ}/{acc.Owner}/{acc.Zustatek}");
            }
            swAcc.Close();
            fs.Close();
        }
        static void Domain_Load(List<Account> AccountList)
        {
            FileStream fs = new FileStream("Accounts.txt", FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            int count = File.ReadAllLines("Accounts.txt").Length;
            for(int x = 0; x < count; x++)
            {
                string s = sr.ReadLine();
                string[] Array = s.Split('/');
                AccountList.Add(new Account(Array[0], Convert.ToInt32(Array[1]), Array[2]));
            }
            sr.Close();
            fs.Close();
        }
    }

    public class Time
    { 
        public Time(DateTime dt,List<Account> ac)
        {
            Dt = dt;
            Ac = ac;
        }
        DateTime Dt { get; set; }
        List<Account> Ac { get; set; }
        public void TimeStart(System.Timers.Timer t) 
        {
            t.Enabled = true;
            t.Elapsed += TickEvent;
        }

        public void TickEvent(Object source, ElapsedEventArgs e)
        {
           Dt =  Dt.AddDays(1);
            if (Dt.Month != Dt.AddDays(-1).Month)//pokaždé, když se změní měsíc >>> do this
            {
                foreach(Account acc in Ac)
                {
                    if(acc.Typ != "Uverovy")
                    {
                        acc.Zustatek *= 1.02;
                    }
                    else
                    {
                        Uverovy uv = acc as Uverovy;
                        uv.Jistina *= 1.02;
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
        public Account(string typ, double vklad, string owner)
        {
            Typ = typ;
            Zustatek = vklad;
            Owner = owner;
        }

        public Account()
        {

        }

        public string Typ { get; set; }
        public double Zustatek { get; set; }

        public string Owner { get; set; }

        public void AccAdd(List<Account> AccountList, List<string> HistoryList)
        {
            Console.Write("Zadejte příjmení vlastníka: ");
            string surname = Console.ReadLine();
            Console.Write("Zadejte typ účtu (Spořící/Úvěrový/Studentský): ");
            string type = Console.ReadLine();
            if (type == "Spořící" || type == "Sporici") { Sporici spAcc = new Sporici(500.0, surname); AccountList.Add(spAcc); HistoryList.Add($"Added {type} account owned by {surname}"); }
            if (type == "Úvěrový" || type == "Uverovy") { Uverovy uvAcc = new Uverovy(500.0, surname, 2000, 50000); AccountList.Add(uvAcc); HistoryList.Add($"Added {type} account owned by {surname}"); }
            if (type == "Studentský" || type == "Studentsky") { Studentsky stAcc = new Studentsky(500.0, surname); AccountList.Add(stAcc); HistoryList.Add($"Added {type} account owned by {surname}"); }
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
                    double Plus = Convert.ToDouble(s);
                    foreach (Account acc in AccountList)
                    {
                        if (acc.Owner == surname) 
                        {
                            acc.Zustatek += Plus;
                            if(acc.Typ == "Uverove")
                            {
                                Uverovy uv = acc as Uverovy;
                                uv.Jistina -= Plus;
                            }
                        }
                    }
                    HistoryList.Add($"Added {Plus}czk to account owned by {surname}.");
                    break;
                case "Deposit":
                    Console.Write("Zadejte vlastníka účtu: ");
                    string sur = Console.ReadLine();
                    Console.Write("Kolik se odečte: ");
                    string t = Console.ReadLine();
                    double minus = Convert.ToDouble(t);
                    foreach (Account acc in AccountList)
                    {
                        if (acc.Owner == sur || acc.Typ == "Sporici")
                        {
                            if(acc.Zustatek - minus > 0) { acc.Zustatek -= minus; }
                        }
                        if (acc.Owner == sur || acc.Typ == "Studentsky")
                        {
                            if (acc.Zustatek - minus > 0 && minus <= 500) { acc.Zustatek -= minus; }
                        }
                        if (acc.Owner == sur || acc.Typ == "Uverovy")
                        {
                            Uverovy sp = acc as Uverovy;
                            if (minus < sp.UverovyRamec) { sp.Zustatek -= minus; }
                        }
                    }
                    HistoryList.Add($"Deposited {minus}czk from account owned by {sur}.");
                    break;
                case "History":
                    FileStream fs = new FileStream("history.txt", FileMode.Open);
                    StreamReader sr = new StreamReader(fs);
                    Console.WriteLine(sr.ReadToEnd());
                    sr.Close();
                    fs.Close();
                    break;
            }
        }
    }

    public class Sporici:Account
    {
        public Sporici(double vklad, string owner):base("Sporici", vklad, owner)
        {
            Zustatek = vklad;
            Owner = owner;
        }
    }

    public class Uverovy:Account
    {
        public Uverovy(double vklad, string owner, double UR, double dluh) : base("Uverovy", vklad, owner)
        {
            Zustatek = vklad;
            Owner = owner;
            UverovyRamec = UR;
            Jistina = dluh;
        }

        public double UverovyRamec { get; set; }
        public double Jistina { get; set; }
    }

    public class Studentsky:Account
    {
        public Studentsky(double vklad, string owner) :base("Studentsky", vklad, owner)
        {
            Zustatek = vklad;
            Owner = owner;
        }
    }
}
