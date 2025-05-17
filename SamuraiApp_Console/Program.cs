using SamuraiApp_Model;
using SamuraiApp.Shared.Data.DB;
using SamuraiApp.Shared.Model;
using Microsoft.IdentityModel.Tokens;

internal class Program
{
    private static void Main(string[] args)
    {
        var SamDal = new DAL<Samurai>();
        var DojDal = new DAL<Dojo>();
        var KenDal = new DAL<Kenjutsu>();

        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("Welcome to SamuraiApp!\n" +
                "Choose an option below.\n\n");
            Console.WriteLine("Option 1: Register a Samurai.");
            Console.WriteLine("Option 2: Register a Dojo.");
            Console.WriteLine("Option 3: Register a Kenjutsu.");
            Console.WriteLine("Option 4: List all Samurai.");
            Console.WriteLine("Option 5: List all Dojo.");
            Console.WriteLine("Option 6: List all Kenjutsu.");
            Console.WriteLine("Option 7: Enroll Samurai to Dojo.");
            Console.WriteLine("Option 8: Assign Kenjutsu to Samurai.");
            Console.WriteLine("Option 9: List enrolled Samurai from Dojo.");
            Console.WriteLine("Option 10: List known Kenjutsu from Samurai.");
            Console.WriteLine("Option 11: List Samurai who know given Kenjutsu.");
            Console.WriteLine("Option 0: Exit\n");

            int option = int.Parse(Console.ReadLine());

            switch (option)
            {
                default:
                    Console.WriteLine("Invalid option.");
                    Console.ReadKey();
                    break;
                case 0:
                    Console.WriteLine("Farewell.");
                    exit = true;
                    break;
                case 1:
                    SamuraiRegistration();
                    break;
                case 2:
                    DojoRegistration();
                    break;
                case 3:
                    KenjutsuRegistration();
                    break;
                case 4:
                    ListSamurai();
                    break;
                case 5:
                    ListDojo();
                    break;
                case 6:
                    ListKenjutsu();
                    break;
                case 7:
                    SamuraiEnrollment();
                    break;
                case 8:
                    KenjutsuLearning();
                    break;
            }
        }

        void SamuraiRegistration()
        {
            Console.Clear();
            Console.WriteLine("Enlisting new Samurai.");
            Console.WriteLine("Name?");
            string name = Console.ReadLine();
            Console.WriteLine("Clan?");
            string clan = Console.ReadLine();
            Samurai s = new Samurai(name, clan);
            SamDal.Create(s);
            Console.WriteLine($"Samurai {name} enlisted successfully.");
            Console.ReadKey();
        }

        void DojoRegistration()
        {
            Console.Clear();
            Console.WriteLine("Establishing new Dojo.");
            Console.WriteLine("Name?");
            string name = Console.ReadLine();
            Console.WriteLine("Region?");
            string region = Console.ReadLine();
            Dojo d = new Dojo(name, region);
            DojDal.Create(d);
            Console.WriteLine($"Dojo {name} established successfully.");
            Console.ReadKey();
        }

        void KenjutsuRegistration()
        {
            Console.Clear();
            Console.WriteLine("Inventing new Kenjutsu.");
            Console.WriteLine("Style?");
            string style = Console.ReadLine();
            Kenjutsu k = new Kenjutsu(style);
            KenDal.Create(k);
            Console.WriteLine($"Kenjutsu style {style} invented successfully.");
            Console.ReadKey();
        }

        void ListSamurai()
        {
            Console.Clear();
            var samList = SamDal.Read();
            if (!samList.IsNullOrEmpty())
            {
                foreach (var sam in samList)
                {
                    Console.WriteLine(sam);
                }
            }
            else Console.WriteLine("No Samurai found.");
                Console.WriteLine("\nPress anything to continue.");
            Console.ReadKey();
        }

        void ListDojo()
        {
            Console.Clear();
            var dojList = DojDal.Read();
            if (!dojList.IsNullOrEmpty())
            {
                foreach (var doj in dojList)
                {
                    Console.WriteLine(doj);
                }
            }
            else Console.WriteLine("No Dojo found.");

            Console.WriteLine("\nPress anything to continue.");
            Console.ReadKey();
        }

        void ListKenjutsu()
        {
            Console.Clear();
            var kenList = KenDal.Read();
            if (!kenList.IsNullOrEmpty())
            {
                foreach (var ken in kenList)
                {
                    Console.WriteLine(ken);
                }
            }
            else Console.WriteLine("No Kenjutsu found.");
            Console.WriteLine("\nPress anything to continue.");
            Console.ReadKey();
        }

        void SamuraiEnrollment()
        {
            Console.Clear();
            Console.WriteLine("Enrolling Samurai to Dojo.");
            Console.WriteLine("Which Samurai are you enrolling?");
            string samName = Console.ReadLine();
            var targetSam = SamDal.ReadBy(s=>s.Name.Equals(samName));
            if (targetSam != null)
            {
                Console.WriteLine("To which Dojo?");
                string dojName = Console.ReadLine();
                var targetDoj = DojDal.ReadBy(d=>d.Name.Equals(dojName));
                if (targetDoj != null)
                {
                    targetDoj.AddSamurai(targetSam);
                    Console.WriteLine($"Samurai {samName} enrolled to " +
                        $"Dojo {dojName} successfully.");
                }
                else
                {
                    Console.WriteLine("No such Dojo.");
                }
            }
            else
            {
                Console.WriteLine("No such Samurai");
            }
            Console.ReadKey();
        }

        void KenjutsuLearning()
        {
            Console.Clear();
            Console.WriteLine("Assigning Kenjutsu to Samurai.");
            Console.WriteLine("What Kenjutsu style?");
            string kenName = Console.ReadLine();
            // rest of logic
        }
    }
}