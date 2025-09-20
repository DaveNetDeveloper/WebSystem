using System.Drawing;
using System.Security.Cryptography;
using System.Text.Json;

namespace Domain.Entities
{
    public class ReglaSegmento
    {
        //
        // Propiedades de la regla de segmentación
        //
        private string _campo;
        public string Campo
        {
            get => _campo;
            set {
                if (null == value)
                    throw new ArgumentException("El campo 'Campo' tiene que estar informado.");
                _campo = value;
            }
        }
        
        private string _operador;
        public string Operador {
            get => _operador;
            set {
                if (null == value)
                    throw new ArgumentException("El campo 'Operador' tiene que estar informado.");
                _operador = value;
            }
        }
       
        private string _valor;
        public string Valor {
            get => _valor;
            set {
                if (null == value)
                    throw new ArgumentException("El campo 'Valor' tiene que estar informado.");
                _valor = value;
            }
        }

        // Enumerado de operadores soportados
        public enum OperadorReglaSegmentacion
        {
            Between,
            HigtherOrEqual,
            LowerOrEqual,
            Higther,
            Lower,
            In,
            NotIn,
            Equal,
            Distinct,
            Contains,
            NotContains
        }

        // Método para evaluar si una regla se aplica a un usuario concreto
        public bool AplicaReglaAUsuario(Usuario usuario, ReglaSegmento regla)
        {
            // Obtener el valor del usuario según el campo de la regla
            var prop = typeof(Usuario).GetProperty(regla.Campo);
            if (prop == null) return false;

            var valorUsuario = prop.GetValue(usuario); 

            return EvaluarRegla(valorUsuario);
        }

        public bool EvaluarRegla(object valorUsuario)
        {
            using var doc = JsonDocument.Parse(Valor);
            if (doc.RootElement.ValueKind == JsonValueKind.Array) {
                var valores = doc.RootElement.EnumerateArray()
                    .Select(v => v.ValueKind == JsonValueKind.Number ? v.GetInt32().ToString() : v.GetString())
                    .ToList();
                Console.WriteLine($" - Valores: {string.Join(", ", valores)}");
            }
            else if (doc.RootElement.ValueKind == JsonValueKind.Number) {
                int valorNum = doc.RootElement.GetInt32();
                Console.WriteLine($" - Valor: {valorNum}");
            }
            else if (doc.RootElement.ValueKind == JsonValueKind.String) {
                string valorStr = doc.RootElement.GetString();
                Console.WriteLine($" - Valor: {valorStr}");
            }

            //  
            if (valorUsuario == null) return false;

            // Convertir ambos a string para comparaciones de texto
            string usuarioStr = valorUsuario.ToString() ?? string.Empty;

            switch (ParseOperador(Operador)) 
            {
                case OperadorReglaSegmentacion.Between:

                    string valorReglaClean = Valor.Trim('[', ']', ' ');
                    string[] array = valorReglaClean.Split(',');
                    int min = int.Parse(array[0]);
                    int max = int.Parse(array[1]);

                    if (decimal.TryParse(usuarioStr, out var numUsuario))
                        return (numUsuario >= min) && (numUsuario <= max);
                    break;

                case OperadorReglaSegmentacion.Equal:
                    return usuarioStr.Equals(Valor, StringComparison.OrdinalIgnoreCase);

                case OperadorReglaSegmentacion.Distinct:
                    return !usuarioStr.Equals(Valor, StringComparison.OrdinalIgnoreCase);

                case OperadorReglaSegmentacion.Higther:
                    if (decimal.TryParse(usuarioStr, out numUsuario) && decimal.TryParse(Valor, out var numRegla))
                        return numUsuario > numRegla;
                    break;

                case OperadorReglaSegmentacion.HigtherOrEqual:
                    if (decimal.TryParse(usuarioStr, out numUsuario) && decimal.TryParse((Valor), out numRegla))
                        return numUsuario >= numRegla;
                    break;

                case OperadorReglaSegmentacion.Lower:
                    if (decimal.TryParse(usuarioStr, out numUsuario) && decimal.TryParse(Valor, out numRegla))
                        return numUsuario < numRegla;
                    break;

                case OperadorReglaSegmentacion.LowerOrEqual:
                    if (decimal.TryParse(usuarioStr, out numUsuario) && decimal.TryParse(Valor, out numRegla))
                        return numUsuario <= numRegla;
                    break;

                case OperadorReglaSegmentacion.Contains:
                    return usuarioStr.Contains(Valor, StringComparison.OrdinalIgnoreCase);

                case OperadorReglaSegmentacion.NotContains:
                    return !usuarioStr.Contains(Valor, StringComparison.OrdinalIgnoreCase);

                case OperadorReglaSegmentacion.In:
                    return false;

                case OperadorReglaSegmentacion.NotIn:
                    return false;
            }
            return false;
        }

        private OperadorReglaSegmentacion ParseOperador(string? operador)
        {
            switch (operador.Trim())
            {
                case "Equal":
                    return OperadorReglaSegmentacion.Equal;

                case "Distinct":
                    return OperadorReglaSegmentacion.Distinct;

                case "Higther":
                    return OperadorReglaSegmentacion.Higther;

                case "HigtherOrEqual":
                    return OperadorReglaSegmentacion.HigtherOrEqual;

                case "Lower":
                    return OperadorReglaSegmentacion.Lower;

                case "LowerOrEqual":
                    return OperadorReglaSegmentacion.LowerOrEqual;

                case "In":
                    return OperadorReglaSegmentacion.In;

                case "NotIn":
                    return OperadorReglaSegmentacion.NotIn;

                case "Between":
                    return OperadorReglaSegmentacion.Between;

                case "Contains":
                    return OperadorReglaSegmentacion.Contains;

                case "NotContains":
                    return OperadorReglaSegmentacion.NotContains;

                default:
                    throw new ArgumentException($"Operador no soportado: {operador}");
            }
        }

    }
}