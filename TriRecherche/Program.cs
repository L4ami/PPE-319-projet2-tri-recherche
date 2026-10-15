using System.Diagnostics;

int[] tailles = { 100, 1000, 10000 };
Console.WriteLine("Taille | Tri à bulles | Array.Sort | Recherche linéaire x1000 | List.Find x1000");

foreach (int taille in tailles)
{
    List<Client> clients = GenererClients(taille);
    Random hasard = new Random();

    // 1. Tri à bulles (le nôtre), sur une copie
    List<Client> copie1 = new List<Client>(clients);
    Stopwatch chrono = Stopwatch.StartNew();
    TriABulles(copie1);
    chrono.Stop();
    double tempsBulles = chrono.Elapsed.TotalMilliseconds;

    // 2. Array.Sort (celui de C#), sur une autre copie
    Client[] copie2 = clients.ToArray();
    chrono = Stopwatch.StartNew();
    Array.Sort(copie2, (a, b) => a.Id.CompareTo(b.Id));
    chrono.Stop();
    double tempsArraySort = chrono.Elapsed.TotalMilliseconds;

    // 3. Recherche linéaire (la nôtre), 1000 fois
    chrono = Stopwatch.StartNew();
    for (int k = 0; k < 1000; k++)
    {
        RechercheLineaire(clients, hasard.Next(1, taille + 1));
    }
    chrono.Stop();
    double tempsLineaire = chrono.Elapsed.TotalMilliseconds;

    // 4. List.Find (celle de C#), 1000 fois
    chrono = Stopwatch.StartNew();
    for (int k = 0; k < 1000; k++)
    {
        int id = hasard.Next(1, taille + 1);
        clients.Find(c => c.Id == id);
    }
    chrono.Stop();
    double tempsFind = chrono.Elapsed.TotalMilliseconds;

    Console.WriteLine($"{taille} | {tempsBulles:F2} ms | {tempsArraySort:F2} ms | {tempsLineaire:F2} ms | {tempsFind:F2} ms");
    Console.WriteLine($"   Test tri à bulles : liste triée ? {EstTrie(copie1)}");
    Console.WriteLine($"   Test recherche Id 1 : case {RechercheLineaire(copie1, 1)} (attendu : 0)");
    Console.WriteLine($"   Test recherche Id 999999 : case {RechercheLineaire(copie1, 999999)} (attendu : -1)");
}
static List<Client> GenererClients(int nombre)
{
    List<Client> liste = new List<Client>();
    for (int i = 1; i <= nombre; i++)
    {
        liste.Add(new Client(i, $"Nom{i}", $"Prenom{i}", $"Ville{i}"));
    }
    Random hasard = new Random();
    for (int i = liste.Count - 1; i > 0; i--)
    {
        int j = hasard.Next(i + 1);
        Client temp = liste[i];
        liste[i] = liste[j];
        liste[j] = temp;
    }
    return liste;
}

static void Afficher10(List<Client> liste)
{
    for (int i = 0; i < 10; i++)
    {
        Console.WriteLine($"Id {liste[i].Id}, Nom : {liste[i].Nom}");
    }
}

static void TriABulles(List<Client> liste)
{
    for (int passe = 0; passe < liste.Count - 1; passe++)
    {
        for (int i = 0; i < liste.Count - 1 - passe; i++)
        {
            if (liste[i].Id > liste[i + 1].Id)
            {
                Client temp = liste[i];
                liste[i] = liste[i + 1];
                liste[i + 1] = temp;
            }
        }
    }
}

static int RechercheLineaire(List<Client> liste, int idCherche)
{
    for (int i = 0; i < liste.Count; i++)
    {
        if (liste[i].Id == idCherche)
        {
            return i;
        }
    }
    return -1;
}
static bool EstTrie(List<Client> liste)
{
    for (int i = 0; i < liste.Count - 1; i++)
    {
        if (liste[i].Id > liste[i + 1].Id)
        {
            return false;
        }
    }
    return true;
}
public class Client
{
    public Client(int id, string nom, string prenom, string ville)
    {
        Id = id;
        Nom = nom;
        Prenom = prenom;
        Ville = ville;

    }

    public int Id { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string Ville { get; set; }
}
