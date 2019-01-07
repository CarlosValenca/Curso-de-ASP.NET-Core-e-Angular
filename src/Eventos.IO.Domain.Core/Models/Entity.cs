using FluentValidation;
using FluentValidation.Results;
using System;

namespace Eventos.IO.Domain.Core.Models
{
    // Esta é a classe básica que servirá de base para os outros domínios
    // O abstract é para não poderem instanciar esta classe só herdar
    // Eu instalei o FluentValidation de modo a poder herdar da classe AbstractValidator
    // usando o comando abaixo tanto no Domain.Core como no Domain
    // Install-Package FluentValidation -pre
    // Entity implementa um T genérico que passa para o AbstractValidator onde este T genérico
    // fosse uma classe que herdasse de Entity
    public abstract class Entity<T> : AbstractValidator<T> where T : Entity<T>
    {
        public Guid Id { get; protected set; }

        // Inicializando o ValidationResult para poder acessar este objeto a qualquer hora
        protected Entity()
        {
            ValidationResult = new ValidationResult();
        }

        public abstract bool EhValido();

        public ValidationResult ValidationResult { get; protected set; }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity<T>;

            // Estou comparando o objeto passado com o this para ver se se trata
            // de uma entidade da mesma instancia (caso em que retornaremos true)
            if (ReferenceEquals(this, compareTo)) return true;
            // Caso não seja um objeto da mesma instancia, então vou comparar com o null
            // para ver se o objeto é inválido de alguma forma
            if (ReferenceEquals(null, compareTo)) return false;
            // Caso nao passe por nenhuma das duas comparações, confirma se os objetos possuem
            // o mesmo id, ainda que sejam de instancias diferentes
            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity<T> a, Entity<T> b)
        {
            // Valida se as duas são comparáveis com nulo
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;
            // Está utilizando o id
            return a.Equals(b);
        }

        public static bool operator !=(Entity<T> a, Entity<T> b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            // O número 907 pode ser qualquer número, serve para criar um
            // valor especial para esta entidade, será gerado um hashcode
            // único de modo a ter certeza que numa comparação estamos falando
            // da mesma entidade
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            // Facilitará na identificação da entidade toda vez que se der o toString
            return GetType().Name + "[id = " + Id + "]";
        }
    }
}
