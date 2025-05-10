using SamuraiApp_Console;
using SamuraiApp.Shared.Data.DB;

internal class Program
{
    public static Dictionary<string, Samurai> SamuraiList = new();
    public static Dictionary<string, Dojo> DojoList = new();

    private static void Main(string[] args)
    {
        var SamDal = new SamuraiDAL(new SamuraiAppContext());

        //SamDal.Create(new Samurai("Tomoe", "Wagasaki"));
        //SamDal.Update(new Samurai("Mami", "Wagasaki") { Id = 1003 });
        //SamDal.Delete(new Samurai() { Id = 1003 });

        var SamList = SamDal.Read();

        foreach (var s in SamList)
        {
            Console.WriteLine(s);
        }

        return;
        // commenting below

        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

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
                    break;
                case 4:
                    break;
            }
        }

        Samurai samurai1 = new Samurai("Jin", "Sakana");
        Dojo dojo1 = new Dojo("Shinboshi", "Kansai");

        dojo1.AddSamurai(samurai1);

        dojo1.ListSamurai();
        // Console.WriteLine(samurai1);
    }

    private static void SamuraiRegistration()
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

    private static void DojoRegistration()
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

    private static void SamuraiEnlisting()
    {
        Console.Clear();
        Console.WriteLine("Enrolling Samurai to Dojo.");
        Console.WriteLine("Which Samurai are you enrolling?");
        string samName = Console.ReadLine();
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