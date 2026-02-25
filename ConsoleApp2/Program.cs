using System;
using System.Collections.Generic;
using System.Linq;

namespace ZooManagerApp
{
    // Base abstract class Animal
    public abstract class Animal
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Habitat { get; set; }
        public string DietType { get; set; }
        public double Weight { get; set; }
        public string Color { get; set; }

        public Animal(string name, int age, string habitat, string dietType, double weight, string color)
        {
            Name = name;
            Age = age;
            Habitat = habitat;
            DietType = dietType;
            Weight = weight;
            Color = color;
        }

        public virtual string GetInfo()
        {
            return $"Name: {Name}, Age: {Age}, Habitat: {Habitat}, " +
                   $"Diet Type: {DietType}, Weight: {Weight} kg, Color: {Color}";
        }
    }

    // Mammal class
    public class Mammal : Animal
    {
        public bool HasFur { get; set; }

        public Mammal(string name, int age, string habitat, string dietType, double weight, string color, bool hasFur)
          : base(name, age, habitat, dietType, weight, color)
        {
            HasFur = hasFur;
        }

        public override string GetInfo()
        {
            string furInfo = HasFur ? "yes" : "no";
            return base.GetInfo() + $", Type: Mammal, Fur: {furInfo}";
        }
    }

    // Bird class
    public class Bird : Animal
    {
        public double WingSpan { get; set; }

        public Bird(string name, int age, string habitat, string dietType, double weight, string color, double wingSpan)
          : base(name, age, habitat, dietType, weight, color)
        {
            WingSpan = wingSpan;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $", Type: Bird, Wingspan: {WingSpan} m";
        }
    }

    // Fish class
    public class Fish : Animal
    {
        public string WaterType { get; set; }

        public Fish(string name, int age, string habitat, string dietType, double weight, string color, string waterType)
          : base(name, age, habitat, dietType, weight, color)
        {
            WaterType = waterType;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $", Type: Fish, Water Type: {WaterType}";
        }
    }

    // Reptile class
    public class Reptile : Animal
    {
        public bool IsVenomous { get; set; }

        public Reptile(string name, int age, string habitat, string dietType, double weight, string color, bool isVenomous)
          : base(name, age, habitat, dietType, weight, color)
        {
            IsVenomous = isVenomous;
        }

        public override string GetInfo()
        {
            string venomInfo = IsVenomous ? "venomous" : "non-venomous";
            return base.GetInfo() + $", Type: Reptile, Venomous: {venomInfo}";
        }
    }

    // Amphibian class
    public class Amphibian : Animal
    {
        public string SkinMoisture { get; set; }

        public Amphibian(string name, int age, string habitat, string dietType, double weight, string color, string skinMoisture)
          : base(name, age, habitat, dietType, weight, color)
        {
            SkinMoisture = skinMoisture;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $", Type: Amphibian, Skin Moisture: {SkinMoisture}";
        }
    }

    // AnimalManager class (Singleton)
    public sealed class AnimalManager
    {
        private static AnimalManager _instance = null;
        private static readonly object _lock = new object();
        private List<Animal> _animals;

        private AnimalManager()
        {
            _animals = new List<Animal>();
        }

        public static AnimalManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new AnimalManager();
                    }
                    return _instance;
                }
            }
        }

        public void AddAnimal(Animal animal)
        {
            if (animal != null)
            {
                _animals.Add(animal);
                Console.WriteLine($"Animal {animal.Name} successfully added to the zoo!");
            }
            else
            {
                Console.WriteLine("Error: animal cannot be null!");
            }
        }

        public void ShowAllAnimals()
        {
            if (_animals.Count == 0)
            {
                Console.WriteLine("There are no animals in the zoo yet.");
                return;
            }

            string result = "\n=== LIST OF ALL ANIMALS ===\n";

            for (int i = 0; i < _animals.Count; i++)
            {
                result += $"\n[{i + 1}] {_animals[i].GetInfo()}\n";
            }

            Console.Write(result);
        }

        public void ShowAnimalByName(string name)
        {
            List<Animal> foundAnimals = _animals.Where(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();

            if (foundAnimals.Count == 0)
            {
                Console.WriteLine($"Animal with name '{name}' not found.");
                return;
            }

            string result = $"\n=== ANIMALS WITH NAME '{name}' ===\n";

            foreach (Animal animal in foundAnimals)
            {
                result += animal.GetInfo() + "\n";
            }

            Console.Write(result);
        }

        public void ShowAnimalByIndex(int index)
        {
            if (index < 0 || index >= _animals.Count)
            {
                Console.WriteLine("Animal with this number does not exist.");
                return;
            }

            Console.Write($"\n=== ANIMAL #{index + 1} ===\n{_animals[index].GetInfo()}\n");
        }
    }

    // Main program class
    class Program
    {
        static void Main(string[] args)
        {
            string welcomeMessage = "Welcome to the Zoo Management Program!\n" +
                                   "==========================================";
            Console.WriteLine(welcomeMessage);

            AnimalManager manager = AnimalManager.Instance;

            CreateDemoAnimals(manager);

            bool exit = false;

            while (!exit)
            {
                string menu = "\n=== MAIN MENU ===\n" +
                             "1. Show all animals\n" +
                             "2. Find animal by name\n" +
                             "3. Find animal by number\n" +
                             "4. Add new animal\n" +
                             "5. Exit\n" +
                             "Choose action (1-5): ";
                Console.Write(menu);

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        manager.ShowAllAnimals();
                        break;

                    case "2":
                        FindAnimalByName(manager);
                        break;

                    case "3":
                        FindAnimalByIndex(manager);
                        break;

                    case "4":
                        AddNewAnimal(manager);
                        break;

                    case "5":
                        exit = true;
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid input. Please choose 1-5.");
                        break;
                }
            }
        }

        static void CreateDemoAnimals(AnimalManager manager)
        {
            string createMessage = "\nCreating demo animals...";
            Console.WriteLine(createMessage);

            Mammal lion = new Mammal("Kiko", 5, "Savanna", "Predator", 180.5, "Golden", true);
            Bird eagle = new Bird("Kiper", 3, "Mountains", "Predator", 6.2, "Brown", 2.3);
            Fish salmon = new Fish("Lososya", 1, "Ocean", "Omnivore", 2.5, "Orange", "Salt");
            Reptile snake = new Reptile("Viper", 4, "Jungle", "Predator", 15.0, "Green", true);
            Amphibian frog = new Amphibian("Kvak", 2, "Swamp", "Insectivore", 0.3, "Green", "Moist");

            manager.AddAnimal(lion);
            manager.AddAnimal(eagle);
            manager.AddAnimal(salmon);
            manager.AddAnimal(snake);
            manager.AddAnimal(frog);

            string successMessage = "Demo animals added!";
            Console.WriteLine(successMessage);
        }

        static void FindAnimalByName(AnimalManager manager)
        {
            Console.Write("\nEnter animal name to search: ");
            string name = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(name))
            {
                manager.ShowAnimalByName(name);
            }
            else
            {
                Console.WriteLine("Name cannot be empty!");
            }
        }

        static void FindAnimalByIndex(AnimalManager manager)
        {
            Console.Write("\nEnter animal number (starting from 1): ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int index))
            {
                manager.ShowAnimalByIndex(index - 1);
            }
            else
            {
                Console.WriteLine("Invalid number format!");
            }
        }

        static void AddNewAnimal(AnimalManager manager)
        {
            string menu = "\n=== ADD NEW ANIMAL ===\n" +
                         "Select animal type:\n" +
                         "1. Mammal\n" +
                         "2. Bird\n" +
                         "3. Fish\n" +
                         "4. Reptile\n" +
                         "5. Amphibian\n" +
                         "Your choice (1-5): ";
            Console.Write(menu);

            string typeChoice = Console.ReadLine();

            Console.Write("Enter name: ");
            string name = Console.ReadLine();

            Console.Write("Enter age: ");
            string ageInput = Console.ReadLine();

            if (!int.TryParse(ageInput, out int age))
            {
                Console.WriteLine("Invalid age format!");
                return;
            }

            Console.Write("Enter habitat: ");
            string habitat = Console.ReadLine();

            Console.Write("Enter diet type (Predator/Herbivore/Omnivore): ");
            string dietType = Console.ReadLine();

            Console.Write("Enter weight (kg): ");
            string weightInput = Console.ReadLine();

            if (!double.TryParse(weightInput, out double weight))
            {
                Console.WriteLine("Invalid weight format!");
                return;
            }

            Console.Write("Enter color: ");
            string color = Console.ReadLine();

            Animal newAnimal = null;

            switch (typeChoice)
            {
                case "1":
                    Console.Write("Has fur? (yes/no): ");
                    string furInput = Console.ReadLine().ToLower();
                    bool hasFur = furInput == "yes" || furInput == "y";
                    newAnimal = new Mammal(name, age, habitat, dietType, weight, color, hasFur);
                    break;

                case "2":
                    Console.Write("Enter wingspan (m): ");
                    string wingSpanInput = Console.ReadLine();

                    if (double.TryParse(wingSpanInput, out double wingSpan))
                    {
                        newAnimal = new Bird(name, age, habitat, dietType, weight, color, wingSpan);
                    }
                    else
                    {
                        Console.WriteLine("Invalid wingspan format!");
                        return;
                    }
                    break;

                case "3":
                    string waterType = "";
                    bool validWaterType = false;

                    while (!validWaterType)
                    {
                        Console.Write("Water type (fresh/salt): ");
                        waterType = Console.ReadLine().ToLower();

                        if (waterType == "fresh" || waterType == "salt")
                        {
                            validWaterType = true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid water type! Please enter 'fresh' or 'salt'.");
                        }
                    }

                    newAnimal = new Fish(name, age, habitat, dietType, weight, color, waterType);
                    break;

                case "4":
                    Console.Write("Is venomous? (yes/no): ");
                    string venomInput = Console.ReadLine().ToLower();
                    bool isVenomous = venomInput == "yes" || venomInput == "y";
                    newAnimal = new Reptile(name, age, habitat, dietType, weight, color, isVenomous);
                    break;

                case "5":
                    Console.Write("Skin moisture (moist/dry/etc.): ");
                    string skinMoisture = Console.ReadLine();
                    newAnimal = new Amphibian(name, age, habitat, dietType, weight, color, skinMoisture);
                    break;

                default:
                    Console.WriteLine("Invalid animal type choice!");
                    return;
            }

            if (newAnimal != null)
            {
                manager.AddAnimal(newAnimal);
            }
        }
    }
}