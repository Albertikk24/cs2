using System;
using System.Collections.Generic;
using System.Linq;

namespace ZooManagerApp
{
    // Базовый абстрактный класс Animal
    public abstract class Animal
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Habitat { get; set; }
        public string Diet { get; set; }
        public double Weight { get; set; }
        public string Color { get; set; }

        public Animal(string name, int age, string habitat, string diet, double weight, string color)
        {
            Name = name;
            Age = age;
            Habitat = habitat;
            Diet = diet;
            Weight = weight;
            Color = color;
        }

        public virtual string GetInfo()
        {
            return $"Кличка: {Name}, Возраст: {Age}, Среда: {Habitat}, " +
                   $"Питание: {Diet}, Вес: {Weight} кг, Окрас: {Color}";
        }
    }

    // Класс Mammal (Млекопитающее)
    public class Mammal : Animal
    {
        public bool HasFur { get; set; }

        public Mammal(string name, int age, string habitat, string diet, double weight, string color, bool hasFur)
          : base(name, age, habitat, diet, weight, color)
        {
            HasFur = hasFur;
        }

        public override string GetInfo()
        {
            string furInfo = HasFur ? "есть" : "нет";
            return base.GetInfo() + $", Тип: Млекопитающее, Шерсть: {furInfo}";
        }
    }

    // Класс Bird (Птица)
    public class Bird : Animal
    {
        public double WingSpan { get; set; }

        public Bird(string name, int age, string habitat, string diet, double weight, string color, double wingSpan)
          : base(name, age, habitat, diet, weight, color)
        {
            WingSpan = wingSpan;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $", Тип: Птица, Размах крыльев: {WingSpan} м";
        }
    }

    // Класс Fish (Рыба)
    public class Fish : Animal
    {
        public string WaterType { get; set; }

        public Fish(string name, int age, string habitat, string diet, double weight, string color, string waterType)
          : base(name, age, habitat, diet, weight, color)
        {
            WaterType = waterType;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $", Тип: Рыба, Вода: {WaterType}";
        }
    }

    // Класс Reptile (Пресмыкающееся)
    public class Reptile : Animal
    {
        public bool IsVenomous { get; set; }

        public Reptile(string name, int age, string habitat, string diet, double weight, string color, bool isVenomous)
          : base(name, age, habitat, diet, weight, color)
        {
            IsVenomous = isVenomous;
        }

        public override string GetInfo()
        {
            string venomInfo = IsVenomous ? "ядовитое" : "неядовитое";
            return base.GetInfo() + $", Тип: Пресмыкающееся, Ядовитость: {venomInfo}";
        }
    }

    // Класс Amphibian (Земноводное)
    public class Amphibian : Animal
    {
        public string SkinMoisture { get; set; }

        public Amphibian(string name, int age, string habitat, string diet, double weight, string color, string skinMoisture)
          : base(name, age, habitat, diet, weight, color)
        {
            SkinMoisture = skinMoisture;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $", Тип: Земноводное, Влажность кожи: {SkinMoisture}";
        }
    }

    // Класс AnimalManager (Singleton)
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
                    return _instance ?? (_instance = new AnimalManager());
                }
            }
        }

        public void AddAnimal(Animal animal)
        {
            if (animal != null)
            {
                _animals.Add(animal);
                Console.WriteLine($"Животное {animal.Name} успешно добавлено в зоопарк!");
            }
            else
            {
                Console.WriteLine("Ошибка: животное не может быть пустым!");
            }
        }

        public void ShowAllAnimals()
        {
            if (_animals.Count == 0)
            {
                Console.WriteLine("В зоопарке пока нет животных.");
                return;
            }

            string result = "\n=== СПИСОК ВСЕХ ЖИВОТНЫХ ===\n";
            for (int i = 0; i < _animals.Count; i++)
            {
                result += $"\n[{i + 1}] {_animals[i].GetInfo()}\n";
            }
            Console.Write(result);
        }

        public void ShowAnimalByName(string name)
        {
            var foundAnimals = _animals.Where(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();

            if (foundAnimals.Count == 0)
            {
                Console.WriteLine($"Животное с именем '{name}' не найдено.");
                return;
            }

            string result = $"\n=== ЖИВОТНЫЕ С ИМЕНЕМ '{name}' ===\n";
            foreach (var animal in foundAnimals)
            {
                result += animal.GetInfo() + "\n";
            }
            Console.Write(result);
        }

        public void ShowAnimalByIndex(int index)
        {
            if (index < 0 || index >= _animals.Count)
            {
                Console.WriteLine("Животного с таким номером не существует.");
                return;
            }

            Console.Write($"\n=== ЖИВОТНОЕ №{index + 1} ===\n{_animals[index].GetInfo()}\n");
        }
    }

    // Главный класс программы
    class Program
    {
        static void Main(string[] args)
        {
            string welcomeMessage = "Добро пожаловать в программу управления зоопарком!\n" +
                                   "=================================================";
            Console.WriteLine(welcomeMessage);

            AnimalManager manager = AnimalManager.Instance;
            CreateDemoAnimals(manager);

            bool exit = false;
            while (!exit)
            {
                string menu = "\n=== ГЛАВНОЕ МЕНЮ ===\n" +
                             "1. Показать всех животных\n" +
                             "2. Найти животное по имени\n" +
                             "3. Найти животное по номеру\n" +
                             "4. Добавить новое животное\n" +
                             "5. Выйти\n" +
                             "Выберите действие (1-5): ";
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
                        Console.WriteLine("До свидания!");
                        break;
                    default:
                        Console.WriteLine("Неверный ввод. Пожалуйста, выберите 1-5.");
                        break;
                }
            }
        }

        static void CreateDemoAnimals(AnimalManager manager)
        {
            string createMessage = "\nСоздаем демонстрационных животных...";
            Console.WriteLine(createMessage);

            Mammal lion = new Mammal("Жопастик", 5, "Саванна", "Хищник", 180.5, "Золотистый", true);
            Bird eagle = new Bird("Жорик", 3, "Горы", "Хищник", 6.2, "Коричневый", 2.3);
            Fish salmon = new Fish("Габриэль", 1, "Океан", "Всеядное", 2.5, "Оранжевый", "Морская");
            Reptile snake = new Reptile("Роман", 4, "Джунгли", "Хищник", 15.0, "Зеленый", true);
            Amphibian frog = new Amphibian("Остолоп", 2, "Болото", "Насекомоядное", 0.3, "Зеленый", "Влажная");

            manager.AddAnimal(lion);
            manager.AddAnimal(eagle);
            manager.AddAnimal(salmon);
            manager.AddAnimal(snake);
            manager.AddAnimal(frog);

            string successMessage = "Демонстрационные животные добавлены!";
            Console.WriteLine(successMessage);
        }

        static void FindAnimalByName(AnimalManager manager)
        {
            Console.Write("\nВведите имя животного для поиска: ");
            string name = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(name))
            {
                manager.ShowAnimalByName(name);
            }
            else
            {
                Console.WriteLine("Имя не может быть пустым!");
            }
        }

        static void FindAnimalByIndex(AnimalManager manager)
        {
            Console.Write("\nВведите номер животного (начиная с 1): ");
            if (int.TryParse(Console.ReadLine(), out int index))
            {
                manager.ShowAnimalByIndex(index - 1);
            }
            else
            {
                Console.WriteLine("Неверный формат числа!");
            }
        }

        static void AddNewAnimal(AnimalManager manager)
        {
            string menu = "\n=== ДОБАВЛЕНИЕ НОВОГО ЖИВОТНОГО ===\n" +
                         "Выберите тип животного:\n" +
                         "1. Млекопитающее\n" +
                         "2. Птица\n" +
                         "3. Рыба\n" +
                         "4. Пресмыкающееся\n" +
                         "5. Земноводное\n" +
                         "Ваш выбор (1-5): ";
            Console.Write(menu);

            string typeChoice = Console.ReadLine();

            Console.Write("Введите кличку: ");
            string name = Console.ReadLine();

            Console.Write("Введите возраст: ");
            if (!int.TryParse(Console.ReadLine(), out int age))
            {
                Console.WriteLine("Неверный формат возраста!");
                return;
            }

            Console.Write("Введите среду обитания: ");
            string habitat = Console.ReadLine();

            Console.Write("Введите тип питания (Хищник/Травоядное/Всеядное): ");
            string diet = Console.ReadLine();

            Console.Write("Введите вес (кг): ");
            if (!double.TryParse(Console.ReadLine(), out double weight))
            {
                Console.WriteLine("Неверный формат веса!");
                return;
            }

            Console.Write("Введите окрас: ");
            string color = Console.ReadLine();

            Animal newAnimal = null;

            switch (typeChoice)
            {
                case "1":
                    Console.Write("Есть шерсть? (да/нет): ");
                    string furInput = Console.ReadLine().ToLower();
                    bool hasFur = furInput == "да" || furInput == "yes" || furInput == "y";
                    newAnimal = new Mammal(name, age, habitat, diet, weight, color, hasFur);
                    break;

                case "2":
                    Console.Write("Введите размах крыльев (м): ");
                    if (double.TryParse(Console.ReadLine(), out double wingSpan))
                    {
                        newAnimal = new Bird(name, age, habitat, diet, weight, color, wingSpan);
                    }
                    else
                    {
                        Console.WriteLine("Неверный формат размаха крыльев!");
                        return;
                    }
                    break;

                case "3":
                    Console.Write("Тип воды (пресная/морская): ");
                    string waterType = Console.ReadLine();
                    newAnimal = new Fish(name, age, habitat, diet, weight, color, waterType);
                    break;

                case "4":
                    Console.Write("Ядовитое? (да/нет): ");
                    string venomInput = Console.ReadLine().ToLower();
                    bool isVenomous = venomInput == "да" || venomInput == "yes" || venomInput == "y";
                    newAnimal = new Reptile(name, age, habitat, diet, weight, color, isVenomous);
                    break;

                case "5":
                    Console.Write("Влажность кожи (влажная/сухая/и т.д.): ");
                    string skinMoisture = Console.ReadLine();
                    newAnimal = new Amphibian(name, age, habitat, diet, weight, color, skinMoisture);
                    break;

                default:
                    Console.WriteLine("Неверный выбор типа животного!");
                    return;
            }

            if (newAnimal != null)
            {
                manager.AddAnimal(newAnimal);
            }
        }
    }
}