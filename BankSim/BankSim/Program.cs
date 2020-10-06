using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSim
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write(@"Add ~ Vytvoření účtu
Manage ~ Možnost úpravy účtu
Remove ~ Zrušení účtu
");
            string decision = Console.ReadLine();

            switch(decision)
            {
                case "Add":
                    Console.Write("Zadej typ (Spořící/Úvěrový): ");
                    string type = Console.ReadLine();
                    if(type == "Sporici" || type == "Spořící") { Sporici sp = new Sporici(50000); }
                    if (type == "Uverovy" || type == "Úvěrový") { Uverovy uv = new Uverovy(50000); }
                    break;
                case "Manage":
                    break;
                case "Remove":
                    break;
            }
        }
    }

    public class Account
    {
        public Account(string typ)
        {
            Typ = typ;
        }

        public string Typ { get; set; }
    }

    public class Sporici:Account
    {
        public Sporici(int vklad):base("Sporici")
        {
            Vklad = vklad;
        }

        public int Vklad { get; set; }
    }

    public class Uverovy:Account
    {
        public Uverovy(int vklad) : base("Uverovy")
        {
            Vklad = vklad;
        }

        public int Vklad { get; set; }
    }
}
