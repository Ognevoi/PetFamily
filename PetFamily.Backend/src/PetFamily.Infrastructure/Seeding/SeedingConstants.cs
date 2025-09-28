namespace PetFamily.Infrastructure.Seeding;

public static class SeedingConstants
{
    // Species and Breeds
    public const int SPECIES_COUNT = 5;
    public const int BREEDS_PER_SPECIES_MIN = 2;
    public const int BREEDS_PER_SPECIES_MAX = 8;
    
    // Volunteers
    public const int VOLUNTEERS_COUNT = 50;
    public const int PETS_PER_VOLUNTEER_MIN = 0;
    public const int PETS_PER_VOLUNTEER_MAX = 5;
    
    // Batch processing
    public const int BATCH_SIZE = 100;
    
    // Sample data arrays
    public static readonly string[] SpeciesNames = 
    {
        "Dog", "Cat", "Bird", "Rabbit", "Hamster", "Fish", "Turtle", "Guinea Pig"
    };
    
    public static readonly string[][] BreedNames = 
    {
        new[] { "Golden Retriever", "Labrador", "German Shepherd", "Bulldog", "Poodle", "Beagle", "Rottweiler", "Siberian Husky" },
        new[] { "Persian", "Maine Coon", "Siamese", "British Shorthair", "Ragdoll", "Sphynx", "Scottish Fold", "Bengal" },
        new[] { "Canary", "Parakeet", "Cockatiel", "Finch", "Lovebird", "Cockatoo", "Macaw", "Conure" },
        new[] { "Holland Lop", "Mini Rex", "Netherland Dwarf", "Flemish Giant", "Lionhead", "Angora", "Himalayan", "Dutch" },
        new[] { "Syrian", "Dwarf Campbell", "Roborovski", "Chinese", "European", "Winter White", "Teddy Bear", "Long-haired" }
    };
    
    public static readonly string[] FirstNames = 
    {
        "Mart", "Kadri", "Andres", "Liis", "Jaan", "Katrin", "Priit", "Ene", "Peeter", "Marika",
        "Mihkel", "Olga", "Sergei", "Anastasia", "Kristjan", "Tatjana", "Toomas", "Irina", "Vladimir", "Annika",
        "Rain", "Natalia", "Karl", "Mari", "Anton", "Svetlana", "Marko", "Kristi", "Igor", "Julia",
        "Siim", "Oksana", "Viktor", "Helena", "Arvo", "Ekaterina", "Tarmo", "Alina", "Roman", "Maarja",
        "Heiki", "Galina", "Dmitri", "Veronika", "Silver", "Larissa", "Uku", "Olga", "Aleksandr", "Evelin"
    };

    public static readonly string[] LastNames = 
    {
        "Tamm", "Saar", "Sepp", "Kask", "Mägi", "Smirnov", "Ivanov", "Petrov", "Volkov", "Novikov",
        "Kuznetsov", "Koppel", "Pärt", "Lepp", "Ots", "Sokolov", "Morozov", "Jakobson", "Orlov", "Mikhailov",
        "Kalda", "Kivisild", "Melnikov", "Popov", "Vassiljev", "Peterson", "Voronov", "Laur", "Belov", "Stepanov",
        "Lepik", "Mets", "Ilves", "Hunt", "Kuld", "Egorov", "Nikitin", "Pavlov", "Karjane", "Fedorov",
        "Kuusk", "Õun", "Vaher", "Koppelmaa", "Bogdanov", "Zaitsev", "Makarov", "Tarasov", "Arsov", "Grigorjev"
    };

    public static readonly string[] PetNames = 
    {
        "Muri", "Nublu", "Pontu", "Lotte", "Nässu", "Tipa", "Muki", "Masha", "Reks", "Luna",
        "Barsik", "Kassu", "Bella", "Sharik", "Miku", "Sonya", "Zeus", "Nika", "Vaska", "Mimi",
        "Tibu", "Chizhik", "Murka", "Puhh", "Rex", "Karu", "Sasha", "Pätu", "Simba", "Lada",
        "Miisu", "Zhuchka", "Oskar", "Strela", "Villi", "Lilka", "Tosha", "Joosep", "Pushok", "Kity",
        "Pätakas", "Ryzhik", "Kiki", "Timosha", "Nora", "Lusha", "Bublik", "Volna", "Maximka", "Triinu"
    };
    
    public static readonly string[] PetColors = 
    {
        "Black", "White", "Brown", "Golden", "Gray", "Orange", "Cream", "Silver", "Tan", "Brindle",
        "Calico", "Tortoiseshell", "Tabby", "Sable", "Chocolate", "Fawn", "Blue", "Red", "Lilac", "Cinnamon"
    };
    
    public static readonly string[] HealthConditions = 
    {
        "Healthy", "Needs medication", "Recovering from surgery", "Allergic to certain foods", 
        "Requires special diet", "Has arthritis", "Vision impaired", "Hearing impaired", 
        "Diabetes", "Heart condition", "Kidney issues", "Dental problems"
    };

    public static readonly string[] Streets =
    {
        "Pikk tn", "Tartu mnt", "Narva mnt", "Tallinna tn", "Viru tn", "Posti tn", "Jaama tn", "Kooli tn",
        "Parki tee", "Vabaduse pst", "Lossi tn", "Raekoja plats", "Kesk tn", "Viljandi mnt", "Jõgeva tn"
    };

    public static readonly string[] Cities =
    {
        "Tallinn", "Tartu", "Narva", "Pärnu", "Kohtla-Järve", "Viljandi", "Rakvere", "Maardu",
        "Kuressaare", "Sillamäe", "Võru", "Valga", "Haapsalu", "Jõhvi", "Paide", "Keila",
        "Tapa", "Põlva", "Rapla", "Saue", "Kiviõli", "Elva", "Jõgeva", "Otepää"
    };

    public static readonly string[] States =
    {
        "Harju County", "Tartu County", "Ida-Viru County", "Pärnu County", "Ida-Viru County", "Viljandi County",
        "Lääne-Viru County", "Harju County",
        "Saare County", "Ida-Viru County", "Võru County", "Valga County", "Lääne County", "Ida-Viru County",
        "Järva County", "Harju County",
        "Lääne-Viru County", "Põlva County", "Rapla County", "Harju County", "Ida-Viru County", "Tartu County",
        "Jõgeva County", "Valga County"
    };

    public static readonly string[] SocialNetworkTypes =
    {
        "Facebook", "Instagram", "Twitter", "LinkedIn", "TikTok", "YouTube", "Snapchat", "Pinterest"
    };
    
    public static readonly string[] AssistanceTypes = 
    {
        "Medical care", "Food and supplies", "Transportation", "Grooming", "Training", "Foster care",
        "Adoption assistance", "Emergency care", "Behavioral support", "Veterinary visits"
    };
}
