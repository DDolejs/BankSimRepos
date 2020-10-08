using System;
using System.Collections.Generic;
using System.Linq;
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
            do
            {
                Console.Clear();
                Timer ti = new Timer(1000);
                Time t = new Time(DateTime.Now);
                t.TimeStart(ti);
                Console.Write(@"Add ~ Vytvoření účtu
Manage ~ Možnost úpravy účtu
Remove ~ Zrušení účtu
Date ~ Vypsání dnešního data
Příkaz: ");
                string decision = Console.ReadLine();

                switch (decision)
                {
                    case "Add":
                        Console.Write("Zadej typ (Spořící/Úvěrový/Studentský): ");
                        string type = Console.ReadLine();
                        if (type == "Sporici" || type == "Spořící") { Sporici sp = new Sporici(50000); }
                        if (type == "Studentsky" || type == "Studentský") { Studentsky st = new Studentsky(50000); }
                        if (type == "Uverovy" || type == "Úvěrový") { Uverovy uv = new Uverovy(50000); }
                        else { throw new FormatException(); }
                        break;
                    case "Manage":
                        break;
                    case "Remove":
                        break;
                    case "Date":
                        Console.WriteLine(t.ToString());
                        break;
                }
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
            Vklad = vklad;
        }

        public string Typ { get; set; }
        public int Vklad { get; set; }
    }

    public class Sporici:Account
    {
        public Sporici(int vklad):base("Sporici", vklad)
        {
            Vklad = vklad;
        }

        public bool Student { get; set; }
    }

    public class Uverovy:Account
    {
        public Uverovy(int vklad) : base("Uverovy", vklad)
        {
            Vklad = vklad;
        }
    }

    public class Studentsky:Account
    {
        public Studentsky(int vklad):base("Studentsky", vklad)
        {
            Vklad = vklad;

        }
    }
}
