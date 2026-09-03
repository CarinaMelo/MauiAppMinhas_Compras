using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        private string _descricao;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Descricao
        {
            get
            {
                return _descricao;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("A descrição do produto deve ser informada.");
                }

                _descricao = value;
            }
        }

        public double Quantidade { get; set; }

        public double Preco { get; set; }

        [Ignore]
        public double Total
        {
            get
            {
                return Quantidade * Preco;
            }
        }
    }
}