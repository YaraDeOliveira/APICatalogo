using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogo.Models
{
    [Table("Produtos")]
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }
        [Required(ErrorMessage = "O nome é obrigatorio")]
        [StringLength(80, ErrorMessage ="O nome de conter até 80 caracteres")]
        public string Nome { get; set; }
        [Required]
        [MaxLength(300)]
        public string Descricao { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName="decimal(8,2)")]
        [Range(1,1000, ErrorMessage = "O preço deve estar entre {1} e {2}")]
        public decimal Preco { get; set; }
        [Required]
        [MaxLength(300)]
        public string ImagemUrl { get; set; }
        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }
        public Categoria Categoria { get; set; }
        public int CategoriaId { get; set; }

    }
}
