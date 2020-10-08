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
                        acc.AccountAdd(AccountList);
                        break;
                    case "Manage":
                        break;
                    case "Remove":
                        acc.AccountRemove(AccountList);
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

        public List<Account> AccountAdd(List<Account> AccountList)
        {
            Console.Write("Zadej typ (Spořící/Úvěrový/Studentský): ");
            string type = Console.ReadLine();
            if (type == "Sporici" || type == "Spořící") { Sporici sp = new Sporici(50000); AccountList.Add(sp); }
            if (type == "Studentsky" || type == "Studentský") { Studentsky st = new Studentsky(50000); AccountList.Add(st); }
            if (type == "Uverovy" || type == "Úvěrový") { Uverovy uv = new Uverovy(50000); AccountList.Add(uv); }
            else { throw new FormatException(); }
            return AccountList;
        }

        public List<Account> AccountRemove(List<Account> AccountList)
        {
            Console.Write("Zadej typ (Spořící/Úvěrový/Studentský): ");
            string whatToRemove = Console.ReadLine();
            if (whatToRemove == "Sporici" || whatToRemove == "Spořící") { Sporici sp = new Sporici(50000); AccountList.Remove(sp); }
            if (whatToRemove == "Studentsky" || whatToRemove == "Studentský") { Studentsky st = new Studentsky(50000); AccountList.Remove(st); }
            if (whatToRemove == "Uverovy" || whatToRemove == "Úvěrový") { Uverovy uv = new Uverovy(50000); AccountList.Remove(uv); }
            else { throw new FormatException(); }
            return AccountList;

        }
    }

    public class Sporici:Account
    {
        public Sporici(int vklad):base("Sporici", vklad)
        {
            Zustatek = vklad;
        }
    }

    public class Uverovy:Account
    {
        public Uverovy(int vklad) : base("Uverovy", vklad)
        {
            Zustatek = vklad;
        }
    }

    public class Studentsky:Account
    {
        public Studentsky(int vklad):base("Studentsky", vklad)
        {
            Zustatek = vklad;

        }
    }
}
