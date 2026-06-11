/*
 * A biblioteca SQLite é chamada aqui para que as anotações de chave primária
 * e de autoIncremento possam ser usadas na propriedade enfID
 */
using SQLite;

namespace appProvaA1Enfermeiro.Model
{
    [Table("Enfermeiro")]
    public class Enfermeiro
    {
        [PrimaryKey, AutoIncrement, Unique, NotNull]
        public int enfID { get; set; }
        [MaxLength(1000)]
        public string? enfNome { get; set; }
        [MaxLength(100)]
        public string? enfEspecialidade { get; set; }

    }
}
