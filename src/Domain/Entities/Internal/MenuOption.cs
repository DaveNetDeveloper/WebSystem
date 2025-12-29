
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class MenuOption
    {
        public string Nombre { get; set; }
        public int Nivel { get; set; }
        public string Path { get; set; }
        public string Parent { get; set; }
    }
}