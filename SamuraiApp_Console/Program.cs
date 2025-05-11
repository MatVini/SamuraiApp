using SamuraiApp_Model;
using SamuraiApp.Shared.Data.DB;

internal class Program
{
    public static Dictionary<string, Samurai> SamuraiList = new();
    public static Dictionary<string, Dojo> DojoList = new();

    private static void Main(string[] args)
    {
        var SamDal = new DAL<Samurai>();
        var DojDal = new DAL<Dojo>();

        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("Welcome to SamuraiApp!\n" +
                "Choose an option below.\n\n");
            Console.WriteLine("Option 1: Register a Samurai.");
            Console.WriteLine("Option 2: Register a Dojo.");
            Console.WriteLine("Option 3: Register a Kenjutsu.");
            Console.WriteLine("Option 4: Enroll Samurai to Dojo.");
            Console.WriteLine("Option 5: Assign Kenjutsu to Samurai.");
            Console.WriteLine("Option 6: List enrolled Samurai from Dojo.");
            Console.WriteLine("Option 7: List known Kenjutsu from Samurai.");
            Console.WriteLine("Option 8: List Samurai who know given Kenjutsu.");
            Console.WriteLine("Option 0: Exit");

            int option = int.Parse(Console.ReadLine());

            switch (option)
            {
                default:
                    Console.WriteLine("Invalid option.");
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
                    SamuraiEnrollment();
                    break;
                case 5:
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
            SamuraiList.Add("", s);
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
            DojoList.Add("", d);
        }

        void KenjutsuRegistration()
        {
            Console.Clear();
            // kenjutsu logic
        }

        void SamuraiEnrollment()
        {
            Console.Clear();
            Console.WriteLine("Enrolling Samurai to Dojo.");
            Console.WriteLine("Which Samurai are you enrolling?");
            string samName = Console.ReadLine();
            //var targetSam = 
            if (SamuraiList.ContainsKey(samName))
            {
                Console.WriteLine("To which Dojo?");
                string dojoName = Console.ReadLine();
                DojoList.TryGetValue(dojoName, out var d);
            }
            else
            {
                Console.WriteLine("No such Samurai.");
            }
        }
    }
}