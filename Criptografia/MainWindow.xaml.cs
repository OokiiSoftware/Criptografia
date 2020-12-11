using Criptografia.Auxiliar;
using System;
using System.ComponentModel;
using System.Windows;

namespace Criptografia
{
    public partial class MainWindow : Window
    {
        private BackgroundWorker backgroundWorker;

        public MainWindow()
        {
            InitializeComponent();
            CriarCriptografia.OnProgressChanged += NovaCriptOnProgress;
        }

        #region Eventos

        private void Btn_encript_Click(object sender, RoutedEventArgs e)
        {
            if (!tb_entrada.Text.Equals(string.Empty))
                tb_saida_encript.Text += Cript.Encript(tb_entrada.Text) + "\n";
        }
        private void Btn_decript_Click(object sender, RoutedEventArgs e)
        {
            if (!tb_entrada.Text.Equals(string.Empty))
                tb_saida_encript.Text += Cript.Decript(tb_entrada.Text) + "\n";
        }

        private void Btn_NovaCriptografia_Click(object sender, RoutedEventArgs e)
        {
            btn_Cancelar.Visibility = Visibility.Visible;
            btn_NovaCriptografia.IsEnabled = false;
            btn_NovaCriptografia.Content = "Gerando Criptografia";

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += async (s, d) => await CriarCriptografia.Criar();
            backgroundWorker.RunWorkerCompleted += (s, d) =>
            {
                progressBar.Value = 0;
                btn_NovaCriptografia.IsEnabled = true;
                btn_NovaCriptografia.Content = "Gerar Nova Criptografia";
                btn_Cancelar.Visibility = Visibility.Hidden;
            };
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.RunWorkerAsync();
            backgroundWorker.Dispose();
        }

        private void Btn_Cancelar_Click(object sender, RoutedEventArgs e)
        {
            backgroundWorker.CancelAsync();
            backgroundWorker.Dispose();
            btn_Cancelar.Visibility = Visibility.Hidden;
            CriarCriptografia.Cancel = true;
        }

        #endregion

        #region Metodos

        private void NovaCriptOnProgress(double value)
        {
            try
            {
                progressBar.Dispatcher.Invoke(new Action(() => {
                    progressBar.Value = value;
                }));
            }
            catch (Exception ex)
            {
                Log.Erro(Name, ex);
            }
        }

        #endregion
    }
}
