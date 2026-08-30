using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    private ObservableCollection<Produto> lista =
        new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();

        // Liga a ObservableCollection à ListView
        lst_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        try
        {
            lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();

            foreach (Produto produto in tmp)
            {
                lista.Add(produto);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Evento executado sempre que o texto do SearchBar é alterado
    private async void txt_search_TextChanged(
        object sender,
        TextChangedEventArgs e)
    {
        try
        {
            // Pega o texto digitado e remove espaços desnecessários
            string q = e.NewTextValue?.Trim() ?? "";

            lista.Clear();

            List<Produto> tmp;

            // Se o SearchBar estiver vazio,
            // mostra todos os produtos
            if (string.IsNullOrEmpty(q))
            {
                tmp = await App.Db.GetAll();
            }
            else
            {
                // Se tiver texto, realiza a pesquisa
                tmp = await App.Db.Search(q);
            }

            // Adiciona os resultados na ObservableCollection
            foreach (Produto produto in tmp)
            {
                lista.Add(produto);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);

        string msg = $"O total é {soma:C}";

        DisplayAlert("Total dos Produtos", msg, "OK");
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            MenuItem selecionado = sender as MenuItem;

            Produto p = selecionado.BindingContext as Produto;

            bool confirm = await DisplayAlert(
                "Tem Certeza?",
                $"Remover {p.Descricao}?",
                "Sim",
                "Não");

            if (confirm)
            {
                await App.Db.Delete(p.Id);

                lista.Remove(p);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void lst_produtos_ItemSelected(
        object sender,
        SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto p = e.SelectedItem as Produto;

            Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = p,
            });
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void lst_produtos_Refreshing(
        object sender,
        EventArgs e)
    {
        try
        {
            lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();

            foreach (Produto produto in tmp)
            {
                lista.Add(produto);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }
}
