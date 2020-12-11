using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// autor: JONAS SANTOS FERREIRA
/// </summary>
namespace Criptografia.Auxiliar
{
    /// <summary>
    /// Ao gerar uma nova Criptografia um arquivo '<see cref="FILE_NAME"/>' será criado na pasta do app.
    /// Neste projeto substitua o arquivo '<see cref="Properties.Resources.criptografia"/>' pelo arquivo criado.
    /// Compile novamente.
    /// 
    /// Pra add novos Digitos adicione em <see cref="Properties.Resources.digitos"/>
    /// </summary>
    public static class CriarCriptografia
    {
        private const string TAG = "CriarCriptografia";

        #region Variaveis

        public delegate void Progress(double progressValue);
        public static Progress OnProgressChanged;
        public static bool Cancel;

        private static readonly Random random = new Random();

        /// <summary>
        /// Esses valores são usados no inicio e fim de cada caractere criptografado.
        /// Isso impede que o app confunda por Ex:
        /// a = AXF;
        /// b = AXFRIG;
        /// no Valor AXFRIG o app iria confundir com avariavel 'a' os 3 primeiros caracteres.
        /// </summary>
        private static readonly List<char> reservado = new List<char>
        {
            'Q', 'i', 'A', 'E', 'X', 'l', 'z', 'L', 'i', 'I', 'o', 'm', '1', '☡', 'P',
            'Y', 'O', 's', 'k', '4', 'h', '3', 'v', 'd', 'f', 'B', '|', '7', '0', 'x'
        };

        private static List<string> digitos;
        /// <summary>
        /// Lista de digitos compativeis com a criptografia.
        /// </summary>
        private static List<string> GetDigitos
        {
            get
            {
                if (digitos == null)
                {
                    string json = Encoding.UTF8.GetString(Properties.Resources.digitos);
                    digitos = JsonConvert.DeserializeObject<List<string>>(json);
                }
                return digitos;
            }
        }

        /// <summary>
        /// Número mínimo de caracteres na criptogrados
        /// </summary>
        private const int CHAR_MINIMO = 1;

        /// <summary>
        /// Número máximo de caracteres na criptogrados
        /// </summary>
        private const int CHAR_MAXIMO = 4;

        /// <summary>
        /// Ao criptografar o soft usa DateTime.Now.Second. Então essa variável deve ser sempre = 60.
        /// </summary>
        private const int MAX_SECOND = 60;

        /// <summary>
        /// Nome do arquivo salvo.
        /// </summary>
        private const string FILE_NAME = "criptografia.json";

        #endregion

        /// <summary>
        /// Usa os 'segundos' do relógio pra criar 60 criptografias pra cada digito.
        /// Cria uma nova criptografia e salva na pasta do app com o nome => <see cref="FILE_NAME"/>.
        /// </summary>
        public static Task<bool> Criar()
        {
            // Armazena todas as criptografias pra garantir que não vai ter valores replicados.
            var criptografiasGeradas = new List<string>();

            string jsonFile = "{\n";
            List<string> digitos = GetDigitos;
            double progress = 0;
            Cancel = false;

            foreach (string digito in digitos)
            {
                if (Cancel) break;

                progress++;
                OnProgressChanged?.Invoke((progress / digitos.Count) * 100.0);

                string digitoAux = digito;
                if (digito == "\\")// caractere especial -> \
                    digitoAux = "\\\\";
                if (digito == "\"")// caractere especial -> "
                    digitoAux = "\\\"";

                string jsonDigito = string.Format("\t\"{0}\": {{\n", digitoAux);
                string jsonNumeros = "";
                for (int i = 0; i < MAX_SECOND; i++)
                {
                voltar:

                    if (Cancel) break;

                    // Ontem 2 digitos reservados pra adicionar à dript...fia
                    int size = random.Next(CHAR_MINIMO, CHAR_MAXIMO);
                    int init = random.Next(0, reservado.Count);
                    int fim = random.Next(0, reservado.Count);

                    // Fica tipo -> AgyMI <- onde A e I são digitos reservados
                    string value = reservado[init] + RandomString(size) + reservado[fim];

                    if (criptografiasGeradas.Contains(value)) goto voltar;

                    criptografiasGeradas.Add(value);
                    jsonNumeros += string.Format("\t\t\"{0}\": \"{1}\"", i, value);
                    if (i < MAX_SECOND - 1)
                        jsonNumeros += ",\n";
                }
                jsonDigito += jsonNumeros;
                jsonDigito += "\n\t}";
                if (digitos.IndexOf(digitoAux) < digitos.Count - 1)
                    jsonDigito += ",\n";
                jsonFile += jsonDigito;
            }

            if (Cancel)
                Log.Msg(TAG, "Criar", "Cancelado");
            else
            {
                jsonFile += "\n}";
                try
                {
                    Log.Msg(TAG, "Criar", "File Save", "Iniciando");
                    File.WriteAllText(FILE_NAME, jsonFile);
                    Log.Msg(TAG, "Criar", "File Save: OK");
                }
                catch (Exception ex)
                {
                    Log.Erro(TAG, ex);
                }
                
            }

            return Task.FromResult(true);
        }

        /// <summary>
        /// Tentei aumentar a quantidade de criptografias possiveis pra cada digito.
        /// Usando Minuto e Segundo, mas o arquivo final fica gigante e pesa muito na hora de usar.
        /// </summary>
        public static void CriarTexte()
        {
            var criptografiasGeradas = new List<string>();

            var jsonFile = "{\n";
            var digitos = GetDigitos;
            foreach (var digito in digitos)
            {
                Log.Msg(TAG, "Criar1: ", digito);

                string jsonDigito = string.Format("\t\"{0}\": {{\n", digito);
                string jsonMinuto = "";
                for (int minute = 0; minute < MAX_SECOND; minute++)
                {
                    string jsonSegundo = "";
                    jsonMinuto += string.Format("\t\t\"{0}\": {{\n", minute);
                    for (int second = 0; second < MAX_SECOND; second++)
                    {
                    voltar:
                        int size = random.Next(CHAR_MINIMO, CHAR_MAXIMO);
                        int init = random.Next(0, reservado.Count);
                        int fim = random.Next(0, reservado.Count);

                        string value = reservado[init] + RandomString(size) + reservado[fim];

                        if (criptografiasGeradas.Contains(value)) goto voltar;

                        criptografiasGeradas.Add(value);
                        jsonSegundo += string.Format("\t\t\t\"{0}\": \"{1}\"", second, value);
                        if (second < MAX_SECOND - 1)
                            jsonSegundo += ",\n";
                    }
                    jsonMinuto += jsonSegundo + "\n\t\t}";
                    if (minute < MAX_SECOND - 1)
                        jsonMinuto += ",\n";
                }
                jsonDigito += jsonMinuto;
                jsonDigito += "\n\t}";
                if (digitos.IndexOf(digito) < digitos.Count - 1)
                    jsonDigito += ",\n";
                jsonFile += jsonDigito;

            }

            jsonFile += "\n}";
            File.WriteAllText(FILE_NAME, jsonFile);
            Log.Msg(TAG, "Criar1", "File Save: OK");
        }

        private static string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                char ch;
                for (int i = 0; i < size; i++)
                {
                voltar:
                    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65 + i)));
                    if (ch == '\\' || ch == '\"' || reservado.Contains(ch)) goto voltar;
                    builder.Append(ch);
                }
            }
            catch (Exception ex)
            {
                Log.Erro(TAG, ex);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Caso seja perdido o arquivo [json] com os digitos, esse método cria os
        /// digitos através de um arquivo [json] com as criptografias.
        /// </summary>
        public static void GerarDigitos()
        {
            string json = Encoding.UTF8.GetString(Properties.Resources.criptografia);
            var data = JsonConvert.DeserializeObject<Dictionary<char, Dictionary<int, string>>>(json);

            string jsonDigitos = "[";
            foreach (char digito in data.Keys)
            {
                string digitoAux = digito.ToString();
                if (digito == '\\')
                    digitoAux = "\\\\";
                else if (digito == '"')
                    digitoAux = "\\\"";
                jsonDigitos += string.Format("\"{0}\", ", digitoAux);
            }
            jsonDigitos += "]";
            try
            {
                Log.Msg(TAG, "GerarDigitos", "File Save", "Iniciando");
                File.WriteAllText(FILE_NAME, jsonDigitos);
                Log.Msg(TAG, "GerarDigitos", "File Save: OK");
            }
            catch(Exception ex)
            {
                Log.Erro(TAG, ex);
            }
            
        }
    }
}
