using System.Collections.Generic;
using LAB2_OOP_MAUI.Models;
using LAB2_OOP_MAUI.Strategies;

namespace LAB2_OOP_MAUI.Controllers
{
    public class SystemControl
    {
        private static SystemControl _instance;

        // 1. Посилання на алгоритм пошуку (Стратегію)
        public ISearchStrategy CurrentStrategy { get; set; }

        // 2. Шлях до файлу XML (Вкажіть реальний шлях на вашому ПК!)
        // Для лабораторної найпростіше покласти файл на диск C: або D:
        public string XmlFilePath { get; set; } = @"C:\Users\Admin\source\repos\LAB2_OOP_MAUI\sports.xml";

        public static SystemControl Instance
        {
            get
            {
                if (_instance == null) _instance = new SystemControl();
                return _instance;
            }
        }

        private SystemControl()
        {
            // Стратегія за замовчуванням
            CurrentStrategy = new LinqStrategy();
        }

        // 3. Головний метод пошуку
        public List<Section> FindSection(Section criteria)
        {
            // Делегуємо роботу конкретній стратегії (SAX, DOM або LINQ)
            return CurrentStrategy.Search(criteria, XmlFilePath);
        }
    }
}