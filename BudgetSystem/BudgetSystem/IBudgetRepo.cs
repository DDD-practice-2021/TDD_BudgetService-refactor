using System.Collections.Generic;

namespace BudgetSystem
{
    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }
}