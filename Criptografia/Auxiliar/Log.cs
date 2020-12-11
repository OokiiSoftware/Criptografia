using System;

namespace Criptografia.Auxiliar
{
    public static class Log
    {
        public static void Erro(string tag, Exception exeption, dynamic dadoAux = null)
        {
            string msg = "";
            string titulo;
            string mensagem;
            string dados = "";
            string dadosAux;
            if (exeption != null)
            {
                titulo = "Método: " + exeption.TargetSite.Name;
                mensagem = "Message: " + exeption.Message;

                if (exeption.HelpLink != null) dados = "\nHelpLink: " + exeption.HelpLink;
                if (exeption.Source != null) dados += "\nSource: " + exeption.Source;
                if (exeption.StackTrace != null) dados += "\nStackTrace: " + exeption.StackTrace;

                msg = "\nClasse: " + tag;
                msg += "\n" + titulo;
                msg += "\n" + mensagem;
                msg += dados;
            }
            if (dadoAux != null)
            {
                dadosAux = dadoAux.ToString();
                msg += "\nDadoAuxiliar: " + dadosAux;
            }
            Console.WriteLine(string.Format("Criptografia > ERRO > {0}", msg));
        }

        public static void Msg(string tag, string metodo, dynamic value0 = null, dynamic value1 = null, dynamic value2 = null)
        {
            string msg = "";
            if (value0 != null)
                msg += ": " + value0.ToString();
            if (value1 != null)
                msg += ": " + value1.ToString();
            if (value2 != null)
                msg += ": " + value2.ToString();
            
            Console.WriteLine(string.Format("Criptografia > Msg > {0}: {1}{2}", tag, metodo, msg));
        }
    }
}
