using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAB2_OOP_MAUI.Models;

namespace LAB2_OOP_MAUI.Strategies
{
    // Це контракт, який зобов'язує всі стратегії мати метод Search
    public interface ISearchStrategy
    {
        List<Section> Search(Section searchCriteria, string filePath);
    }
}
