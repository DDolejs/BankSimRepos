using System;
using System.Collections.Generic;
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
        static void Main(string[] args)
        {
            Account acc = new Account();
            Timer ti = new Timer(1000);
            Time t = new Time(DateTime.Now);
            t.TimeStart(ti);
            List<Account> AccountList = new List<Account>();
            do
            {
                Console.Clear();
                Console.Write(@"Add ~ Vytvoření účtu
Manage ~ Možnost úpravy účtu
Remove ~ Zrušení účtu
Date ~ Vypsání dnešního data
Příkaz: ");
                string decision = Console.ReadLine();

                switch (decision)
                {
                    case "Add":
                        acc.AccAdd(AccountList);
                        break;
                    case "Manage":
                        acc.AccManage(AccountList);
                        break;
                    case "Remove":
                        acc.DeleteAcc(AccountList);
                        break;
                    case "Date":
                        Console.WriteLine(t.ToString());
                        break;
                }
                Console.ReadLine();

            } while (true);
        }
    }

    public class Time
    { 
        public Time(DateTime dt)
        {
            Dt = dt;
        }
        DateTime Dt { get; set; }
        public void TimeStart(System.Timers.Timer t) 
        {
            t.Enabled = true;
            t.Elapsed += TickEvent;
        }

        private void TickEvent(Object source, ElapsedEventArgs e)
        {
           Dt =  Dt.AddDays(1);
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

        public void AccAdd(List<Account> AccountList)
        {
            Console.Write("Zadejte příjmení vlastníka: ");
            string surname = Console.ReadLine();
            Console.Write("Zadejte typ účtu (Spořící/Úvěrový/Studentský): ");
            string type = Console.ReadLine();
            if (type == "Spořící" || type == "Sporici") { Sporici spAcc = new Sporici(0, surname); AccountList.Add(spAcc); }
            if (type == "Úvěrový" || type == "Uverovy") { Uverovy uvAcc = new Uverovy(0, surname); AccountList.Add(uvAcc); }
            if (type == "Studentský" || type == "Studentsky") { Studentsky stAcc = new Studentsky(0, surname); AccountList.Add(stAcc); }
        }

        public void DeleteAcc(List<Account> AccountList)
        {
            Console.Write("Zadej jméno vlastníka, co účet maže: ");
            string surname = Console.ReadLine();
            foreach(Account acc in AccountList)
            {
                if(acc.Owner == surname) { AccountList.Remove(acc); return; }
            }
        }

        public void AccManage(List<Account> AccountList)
        {
            Console.Clear();
            Console.Write(@"Zadejte, jak chcete účet upravit?
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
                    break;
                case "History":
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
