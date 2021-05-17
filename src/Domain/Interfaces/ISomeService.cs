using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISomeService
    {
        Task<List<string>> GetSomething();
    }
}
