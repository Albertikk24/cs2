using System;
using System.Collections.Generic;
using System.Linq;

namespace ZooManagerApp {
  public abstract class Animal {
    public string Name { get; set; }
    public int Age { get; set; }
    public string Habitat { get; set; }
    public string DietType { get; set; }
    public double Weight { get; set; }
    public string Color { get; set; }

    public Animal(string name, int age, string habitat, string dietType, double weight, string color) {
      Name = name;
      Age = age;
      Habitat = habitat;
      DietType = dietType;
      Weight = weight;
      Color = color;
    }

    public virtual string GetInfo() {
      return $"Name: {Name}, Age: {Age}, Habitat: {Habitat}, " +
             $"Diet Type: {DietType}, Weight: {Weight} kg, Color: {Color}";
    }
  }

  public class Mammal : Animal {
    public bool HasFur { get; set; }

    public Mammal(string name, int age, string habitat, string dietType, double weight, string color, bool hasFur)
      : base(name, age, habitat, dietType, weight, color) {
      HasFur = hasFur;
    }

    public override string GetInfo() {
      string furStatus = HasFur ? "yes" : "no";
      return base.GetInfo() + $", Type: Mammal, Fur: {furStatus}";
    }
  }

  public class Bird : Animal {
    public double WingSpan { get; set; }

    public Bird(string name, int age, string habitat, string dietType, double weight, string color, double wingSpan)
      : base(name, age, habitat, dietType, weight, color) {
      WingSpan = wingSpan;
    }

    public override string GetInfo() {
      return base.GetInfo() + $", Type: Bird, Wingspan: {WingSpan} m";
    }
  }

  public class Fish : Animal {
    public string WaterType { get; set; }

    public Fish(string name, int age, string habitat, string dietType, double weight, string color, string waterType)
      : base(name, age, habitat, dietType, weight, color) {
      WaterType = waterType;
    }

    public override string GetInfo() {
      return base.GetInfo() + $", Type: Fish, Water Type: {WaterType}";
    }
  }

  public class Reptile : Animal {
    public bool IsVenomous { get; set; }

    public Reptile(string name, int age, string habitat, string dietType, double weight, string color, bool isVenomous)
      : base(name, age, habitat, dietType, weight, color) {
      IsVenomous = isVenomous;
    }

    public override string GetInfo() {
      string venomStatus = IsVenomous ? "venomous" : "non-venomous";
      return base.GetInfo() + $", Type: Reptile, Venomous: {venomStatus}";
    }
  }

  public class Amphibian : Animal {
    public string SkinMoisture { get; set; }

    public Amphibian(string name, int age, string habitat, string dietType, double weight, string color, string skinMoisture)
      : base(name, age, habitat, dietType, weight, color) {
      SkinMoisture = skinMoisture;
    }

    public override string GetInfo() {
      return base.GetInfo() + $", Type: Amphibian, Skin Moisture: {SkinMoisture}";
    }
  }

  public sealed class AnimalManager {
    private static AnimalManager _instance = null;
    private static readonly object _lock = new object();
    private List<Animal> _animals;

    private AnimalManager() {
      _animals = new List<Animal>();
    }

    public static AnimalManager Instance {
      get {
        lock (_lock) {
          if (_instance == null) {
            _instance = new AnimalManager();
          }
          return _instance;
        }
      }
    }

    public void AddAnimal(Animal animal) {
      if (animal != null) {
        _animals.Add(animal);
        Console.WriteLine($"Animal {animal.Name} successfully added to the zoo!");
      } else {
        Console.WriteLine("Error: animal cannot be null!");
      }
    }

    public void ShowAllAnimals() {
      if (_animals.Count == 0) {
        Console.WriteLine("There are no animals in the zoo yet.");
        return;
      }

      string result = "\n=== LIST OF ALL ANIMALS ===\n";
      
      for (int animalIndex = 0; animalIndex < _animals.Count; ++animalIndex) {
        result += $"\n[{animalIndex + 1}] {_animals[animalIndex].GetInfo()}\n";
      }
      
      Console.Write(result);
    }

    public void ShowAnimalByName(string name) {
      List<Animal> foundAnimals = _animals.Where(animal => animal.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
      
      if (foundAnimals.Count == 0) {
        Console.WriteLine($"Animal with name '{name}' not found.");
        return;
      }

      string result = $"\n=== ANIMALS WITH NAME '{name}' ===\n";
      
      foreach (Animal currentAnimal in foundAnimals) {
        result += currentAnimal.GetInfo() + "\n";
      }
      
      Console.Write(result);
    }

    public void ShowAnimalByIndex(int index) {
      if (index < 0 || index >= _animals.Count) {
        Console.WriteLine("Animal with this number does not exist.");
        return;
      }

      Console.Write($"\n=== ANIMAL #{index + 1} ===\n{_animals[index].GetInfo()}\n");
    }

    public void RunMainMenu() {
      bool programIsExited = false;
      
      while (!programIsExited) {
        string menu = "\n=== MAIN MENU ===\n" +
                     "1. Show all animals\n" +
                     "2. Find animal by name\n" +
                     "3. Find animal by number\n" +
                     "4. Add new animal\n" +
                     "5. Exit\n" +
                     "Choose action (1-5): ";
        Console.Write(menu);

        string userChoice = Console.ReadLine();

        switch (userChoice) {
          case "1":
            this.ShowAllAnimals();
            break;
            
          case "2":
            this.FindAnimalByName();
            break;
            
          case "3":
            this.FindAnimalByIndex();
            break;
            
          case "4":
            this.AddNewAnimal();
            break;
            
          case "5":
            programIsExited = true;
            Console.WriteLine("Goodbye!");
            break;
            
          default:
            Console.WriteLine("Invalid input. Please choose 1-5.");
            break;
        }
      }
    }

    private void FindAnimalByName() {
      Console.Write("\nEnter animal name to search: ");
      string searchName = Console.ReadLine();
      
      if (!string.IsNullOrWhiteSpace(searchName)) {
        this.ShowAnimalByName(searchName);
      } else {
        Console.WriteLine("Name cannot be empty!");
      }
    }

    private void FindAnimalByIndex() {
      Console.Write("\nEnter animal number (starting from 1): ");
      string userInput = Console.ReadLine();
      
      if (int.TryParse(userInput, out int animalIndex)) {
        this.ShowAnimalByIndex(animalIndex - 1);
      } else {
        Console.WriteLine("Invalid number format!");
      }
    }

    private void AddNewAnimal() {
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
      string animalName = Console.ReadLine();
      
      Console.Write("Enter age: ");
      string ageInput = Console.ReadLine();
      int animalAge;
      
      if (!int.TryParse(ageInput, out animalAge)) {
        Console.WriteLine("Invalid age format!");
        return;
      }
      
      Console.Write("Enter habitat: ");
      string animalHabitat = Console.ReadLine();
      
      Console.Write("Enter diet type (Predator/Herbivore/Omnivore): ");
      string animalDietType = Console.ReadLine();
      
      Console.Write("Enter weight (kg): ");
      string weightInput = Console.ReadLine();
      double animalWeight;
      
      if (!double.TryParse(weightInput, out animalWeight)) {
        Console.WriteLine("Invalid weight format!");
        return;
      }
      
      Console.Write("Enter color: ");
      string animalColor = Console.ReadLine();

      Animal newAnimal = null;

      switch (typeChoice) {
        case "1": {
          Console.Write("Has fur? (yes/no): ");
          string furInput = Console.ReadLine().ToLower();
          bool hasFurStatus = furInput == "yes" || furInput == "y";
          newAnimal = new Mammal(animalName, animalAge, animalHabitat, animalDietType, animalWeight, animalColor, hasFurStatus);
          break;
        }

        case "2": {
          Console.Write("Enter wingspan (m): ");
          string wingSpanInput = Console.ReadLine();
          double wingspanValue;
          
          if (double.TryParse(wingSpanInput, out wingspanValue)) {
            newAnimal = new Bird(animalName, animalAge, animalHabitat, animalDietType, animalWeight, animalColor, wingspanValue);
          } else {
            Console.WriteLine("Invalid wingspan format!");
            return;
          }
          break;
        }

        case "3": {
          string waterTypeValue = "";
          bool validWaterType = false;
          
          while (!validWaterType) {
            Console.Write("Water type (fresh/salt): ");
            waterTypeValue = Console.ReadLine().ToLower();
            
            if (waterTypeValue == "fresh" || waterTypeValue == "salt") {
              validWaterType = true;
            } else {
              Console.WriteLine("Invalid water type! Please enter 'fresh' or 'salt'.");
            }
          }
          
          newAnimal = new Fish(animalName, animalAge, animalHabitat, animalDietType, animalWeight, animalColor, waterTypeValue);
          break;
        }

        case "4": {
          Console.Write("Is venomous? (yes/no): ");
          string venomInput = Console.ReadLine().ToLower();
          bool isVenomousStatus = venomInput == "yes" || venomInput == "y";
          newAnimal = new Reptile(animalName, animalAge, animalHabitat, animalDietType, animalWeight, animalColor, isVenomousStatus);
          break;
        }

        case "5": {
          Console.Write("Skin moisture (moist/dry/etc.): ");
          string skinMoistureValue = Console.ReadLine();
          newAnimal = new Amphibian(animalName, animalAge, animalHabitat, animalDietType, animalWeight, animalColor, skinMoistureValue);
          break;
        }

        default: {
          Console.WriteLine("Invalid animal type choice!");
          return;
        }
      }

      if (newAnimal != null) {
        this.AddAnimal(newAnimal);
      }
    }

    public void CreateDemoAnimals() {
      string createMessage = "\nCreating demo animals...";
      Console.WriteLine(createMessage);

      Mammal lion = new Mammal("Kisa", 5, "Savanna", "Predator", 180.5, "Golden", true);
      Bird eagle = new Bird("Kiko", 3, "Mountains", "Predator", 6.2, "Brown", 2.3);
      Fish salmon = new Fish("Dori", 1, "Ocean", "Omnivore", 2.5, "Orange", "salt");
      Reptile snake = new Reptile("Vipera", 4, "Jungle", "Predator", 15.0, "Green", true);
      Amphibian frog = new Amphibian("Kva", 2, "Swamp", "Insectivore", 0.3, "Green", "Moist");

      this.AddAnimal(lion);
      this.AddAnimal(eagle);
      this.AddAnimal(salmon);
      this.AddAnimal(snake);
      this.AddAnimal(frog);

      string successMessage = "Demo animals added!";
      Console.WriteLine(successMessage);
    }
  }

  class Program {
    static void Main(string[] args) {
      string welcomeMessage = "Welcome to the Zoo Management Program!\n" +
                             "==========================================";
      Console.WriteLine(welcomeMessage);

      AnimalManager manager = AnimalManager.Instance;
      
      manager.CreateDemoAnimals();
      manager.RunMainMenu();
    }
  }
}