﻿using Movedor.ViewModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Brushes = System.Windows.Media.Brushes;
namespace Movedor.View;

public partial class MainWindow : Window
{
    private MainWindowsViewModel _viewModel;
    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new MainWindowsViewModel();
        DataContext = _viewModel;
    }
    private async void Click_SelecionarPasta(object sender, RoutedEventArgs e)
    {
        LimparTela();

        await _viewModel.SelecionarPasta();

        ExibirMensagemStatus();
        ExibirTempo(_viewModel.Cronometro);
    }
    private void ExibirTempo(Stopwatch tempo)
    {
        TimeSpan tempoDecorrido = tempo.Elapsed;
        string tempoFormatado = string.Format($"{tempoDecorrido.Hours:D2}:{tempoDecorrido.Minutes:D2}:{tempoDecorrido.Seconds:D2}");

        TempoDecorrido.Text = tempoFormatado.Equals("00:00:00")
            ? $"Executou em {tempo.Elapsed.Milliseconds}ms\n"
            : $"Executou em: {tempoFormatado}\n";
    }
    private void ExibirMensagemStatus()
    {
        MensagemStatus.Foreground = _viewModel.Contador > 0 ? Brushes.Green : Brushes.Red;

        if (_viewModel.Contador == 1) _viewModel.MensagemStatus = "Arquivo movido com sucesso!";
        else if (_viewModel.Contador == 0) _viewModel.MensagemStatus = "Nenhum Arquivo foi movido!";
        else if (_viewModel.Contador > 1) _viewModel.MensagemStatus = $"{_viewModel.Contador} arquivos movidos com sucesso!";
    }
    private void LimparTela()
    {
        _viewModel.Contador = 0;
        _viewModel.Progresso = 0;
        _viewModel.ArquivosMovidos.Clear();
        _viewModel.Cronometro.Reset();
        _viewModel.MensagemStatus = "Selecione uma pasta para começar...";
        _viewModel.MensagemErro = "";
        MensagemStatus.Foreground = Brushes.Black;
        TempoDecorrido.Text = "";

    }
    private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        => _viewModel.LinkIcone();
    private void Click_AbrirArquivo(object sender, MouseButtonEventArgs e)
        => _viewModel.AbrirArquivo(ArquivosListBox);
    private void Click_Cancelar(object sender, RoutedEventArgs e)
        => _viewModel.Cancelar();
    private async void Click_SelecionarDestino(object sender, RoutedEventArgs e)
    {
        await _viewModel.SelecionarDestino();
        if (!string.IsNullOrEmpty(_viewModel.PastaDestino))
        {
            MensagemStatus.Foreground = Brushes.OrangeRed;
            SelectDestinationFolder.Content = "Destino ✅";
            SelectDestinationFolder.Foreground = Brushes.ForestGreen;
        }
        else
        {
            SelectDestinationFolder.Content = "Selecionar Destino";
            SelectDestinationFolder.Foreground = Brushes.Black;
        }
    }
}

