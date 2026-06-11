using appProvaA1Enfermeiro.Model;
using System.Collections.ObjectModel;

namespace appProvaA1Enfermeiro.Views;

public partial class TelaListaEnfermeiro : ContentPage
{
    ObservableCollection<Enfermeiro> listagemEnfermeiros = new ObservableCollection<Enfermeiro>();

    public TelaListaEnfermeiro()
    {
        InitializeComponent();
        lstEnfermeiros.ItemsSource = listagemEnfermeiros;
    }

    private async void irTelaIncluirEnfermeiro(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TelaIncluirEnfermeiro());
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await CarregarLista();
    }

    private async Task CarregarLista()
    {
        listagemEnfermeiros.Clear();

        List<Enfermeiro> temp = await App.Database.GetAll();

        foreach (Enfermeiro enfermeiro in temp)
        {
            listagemEnfermeiros.Add(enfermeiro);
        }
    }

    private async void alterarEnfermeiro(object sender, EventArgs e)
    {
        Button botao = sender as Button;

        if (botao == null)
            return;

        Enfermeiro enfermeiroSelecionado = botao.CommandParameter as Enfermeiro;

        if (enfermeiroSelecionado == null)
            return;

        await Navigation.PushAsync(new TelaAlterarEnfermeiro
        {
            BindingContext = enfermeiroSelecionado
        });
    }

    private async void excluirEnfermeiro(object sender, EventArgs e)
    {
        Button botao = sender as Button;

        if (botao == null)
            return;

        Enfermeiro enfermeiroSelecionado = botao.CommandParameter as Enfermeiro;

        if (enfermeiroSelecionado == null)
            return;

        bool confirmar = await DisplayAlert(
            "Excluir Enfermeiro",
            $"Deseja realmente excluir {enfermeiroSelecionado.enfNome}?",
            "Sim",
            "Não"
        );

        if (confirmar)
        {
            await App.Database.Delete(enfermeiroSelecionado.enfID);
            listagemEnfermeiros.Remove(enfermeiroSelecionado);

            await DisplayAlert("Sucesso", "Enfermeiro excluído com sucesso!", "OK");
        }
    }

    private async void txtBuscar(object sender, TextChangedEventArgs e)
    {
        string busca = e.NewTextValue;

        lstEnfermeiros.IsRefreshing = true;

        listagemEnfermeiros.Clear();

        List<Enfermeiro> temp = await App.Database.Search(busca);

        foreach (Enfermeiro enfermeiro in temp)
        {
            listagemEnfermeiros.Add(enfermeiro);
        }

        lstEnfermeiros.IsRefreshing = false;
    }

    private async void lstEnfermeirosItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        Enfermeiro enfermeiroSelecionado = e.SelectedItem as Enfermeiro;

        if (enfermeiroSelecionado == null)
            return;

        await Navigation.PushAsync(new TelaAlterarEnfermeiro
        {
            BindingContext = enfermeiroSelecionado
        });

        lstEnfermeiros.SelectedItem = null;
    }

    private async void refCarregando(object sender, EventArgs e)
    {
        await CarregarLista();
        lstEnfermeiros.IsRefreshing = false;
    }
}