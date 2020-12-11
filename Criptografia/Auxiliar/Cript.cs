using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// autor: JONAS SANTOS FERREIRA
/// </summary>
namespace Criptografia.Auxiliar
{
    public static class Cript
    {
        private static Dictionary<string, Dictionary<int, string>> criptografia;
        /// <summary>
        /// Lista de digitos criptografados.
        /// </summary>
        private static Dictionary<string, Dictionary<int, string>> GetCriptografia
        {
            get
            {
                if (criptografia == null)
                {
                    string json = Encoding.UTF8.GetString(Properties.Resources.criptografia);
                    criptografia = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<int, string>>>(json);
                }
                return criptografia;
            }
        }

        public static string Encript(string value)
        {
            var digitos = GetCriptografia;

            if (digitos == null) return value;
            
            string result = "";
            int second = DateTime.Now.Second;
            foreach (char digito in value)
            {
                if (digitos.ContainsKey(digito.ToString()))
                    result += digitos[digito.ToString()][second];

                // Se o usuário digitar por ex: 'aaaa' esta linha abaixo impede
                // que o mesmo valor criptografado seja usado em todos os digitos.
                second++;
                if (second == 60) second = 0;
            }
            return result;
        }

        /// <summary>
        /// Esse método de descriptografar não é bom, pois percorre todo o Dictionary com a criptografia
        /// </summary>
        /// <returns></returns>
        public static string Decript(string value)
        {
            string result = value;

            foreach (var letra in GetCriptografia)// letra contém uma coleção de 59 dados criptografados
                foreach (var numero in letra.Value)// numero (0 a 59) cada item representa 1 segundo literal do Relógio
                    if (result.Contains(numero.Value))
                        result = result.Replace(numero.Value, letra.Key.ToString());
            return result;
        }

    }
}
