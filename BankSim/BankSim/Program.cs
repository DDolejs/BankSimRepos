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
                    Account acc = new Account("Sporici", 50000);
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
        public Account(string typ, int vklad)
        {
            Typ = typ;
            Vklad = vklad;
        }

        public string Typ { get; set; }
        public int Vklad { get; set; }
    }
}
