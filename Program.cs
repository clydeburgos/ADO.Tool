using System.Threading.Tasks;
using ADO.Tool.Service;

namespace ADO.Tool
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var data = await WikiService.Search(string.Empty, "Hi everyone!");
        }
    }
}
