using System.Data;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Utilities
{
    public static class JsonHelper
    {
        /// <summary>
        /// Añade un nuevo valor al array "nodeName" dentro del JSON.
        /// Si la propiedad no existe, la crea automáticamente.
        /// </summary>
        /// <param name="json">JSON original como string</param>
        /// <param name="newValue">Nuevo valor a añadir</param>
        /// <returns>JSON actualizado como string</returns>
        public static string AddValue(string json, string nodeName, string newValue)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                // Crear JSON inicial si está vacío
                json = @"{ ""{arrayName}"": [] }";
            }

            // Parsear JSON
            JsonNode node = JsonNode.Parse(json) ?? new JsonObject();

            // Obtener o crear el array "nodeName"
            JsonArray array;
            if (node[nodeName] is JsonArray existingArray) {
                array = existingArray;
            }
            else {
                array = new JsonArray();
                node[nodeName] = array;
            }

            // Añadir el nuevo valor solo si no existe ya en el array
            bool exists = false;
            foreach (var item in array) {
                if (item?.ToString() == newValue) {
                    exists = true;
                    break;
                }
            }

            if (!exists) {
                array.Add(newValue);
            }

            // Serializar de vuelta a string
            return JsonSerializer.Serialize(node);
        }
    }
}