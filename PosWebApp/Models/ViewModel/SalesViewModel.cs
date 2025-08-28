using PosWebAppCommon.Models;

namespace PosWebApp.Models.ViewModel
{
    public class SalesViewModel
    {
        public List<Item> Products { get; set; } = new List<Item>();
        public List<Item> Cart { get; set; } = new List<Item>();
    }
}
