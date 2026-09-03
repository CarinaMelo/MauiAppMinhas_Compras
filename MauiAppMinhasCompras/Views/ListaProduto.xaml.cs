using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    // ObservableCollection utilizada para atualizar
    // automaticamente a lista apresentada na tela
    private ObservableCollection<Produto> lista =
        new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();

        // Liga a ObservableCollection à ListView
        lst_produtos.ItemsSource = lista;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            // Limpa a lista atual
            lista.Clear();

            // Busca todos os produtos no banco
            List<Produto> tmp = await App.Db.GetAll();

            // Adiciona os produtos na ObservableCollection
            foreach (Produto produto in tmp)
            {
                lista.Add(produto);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ops",
                ex.Message,
                "OK");
        }
    }

    // Botão Adicionar
    private async void ToolbarItem_Clicked(
        object sender,
        EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(
                new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ops",
                ex.Message,
                "OK");
        }
    }

    // ==========================================
    // BUSCA DINÂMICA
    // ==========================================

    private async void txt_search_TextChanged(
        object sender,
        TextChangedEventArgs e)
    {
        try
        {
            // Pega o texto digitado
            string q = e.NewTextValue?.Trim() ?? "";

            // Limpa os resultados anteriores
            lista.Clear();

            List<Produto> tmp;

            // Se não tiver texto, mostra todos
            // os produtos cadastrados
            if (string.IsNullOrEmpty(q))
            {
                tmp = await App.Db.GetAll();
            }
            else
            {
                // Se tiver texto, realiza a pesquisa
                tmp = await App.Db.Search(q);
            }

            // Adiciona os resultados encontrados
            // na ObservableCollection
            foreach (Produto produto in tmp)
            {
                lista.Add(produto);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ops",
                ex.Message,
                "OK");
        }
    }

    // Botão Somar
    private async void ToolbarItem_Clicked_1(
        object sender,
        EventArgs e)
    {
        try
        {
            double soma = lista.Sum(
                i => i.Total);

            string msg =
                $"O total é {soma:C}";

            await DisplayAlert(
                "Total dos Produtos",
                msg,
                "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ops",
                ex.Message,
                "OK");
        }
    }

    // Excluir produto
    private async void MenuItem_Clicked(
        object sender,
        EventArgs e)
    {
        try
        {
            MenuItem selecionado =
                sender as MenuItem;

            Produto p =
                selecionado.BindingContext as Produto;

            bool confirm = await DisplayAlert(
                "Tem Certeza?",
                $"Remover {p.Descricao}?",
                "Sim",
                "Não");

            if (confirm)
            {
                await App.Db.Delete(p.Id);

                // Remove também da ObservableCollection
                lista.Remove(p);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ops",
                ex.Message,
                "OK");
        }
    }

    // Selecionar produto para editar
    private async void lst_produtos_ItemSelected(
        object sender,
        SelectedItemChangedEventArgs e)
    {
        try
        {
            if (e.SelectedItem == null)
                return;

            Produto p =
                e.SelectedItem as Produto;

            await Navigation.PushAsync(
                new Views.EditarProduto
                {
                    BindingContext = p
                });

            // Remove a seleção visual
            lst_produtos.SelectedItem = null;
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ops",
                ex.Message,
                "OK");
        }
    }

    // Atualizar a lista com Pull to Refresh
    private async void lst_produtos_Refreshing(
        object sender,
        EventArgs e)
    {
        try
        {
            lista.Clear();

            List<Produto> tmp =
                await App.Db.GetAll();

            foreach (Produto produto in tmp)
            {
                lista.Add(produto);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ops",
                ex.Message,
                "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }
}