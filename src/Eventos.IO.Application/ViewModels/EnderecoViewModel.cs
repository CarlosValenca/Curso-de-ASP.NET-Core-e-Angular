using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace Eventos.IO.Application.ViewModels
{
    // Nao posso ter Data Annotations aqui por que o endereco nao e obrigatorio
    // Estas validacoes serao feitas pelo backend
    public class EnderecoViewModel
    {
        public EnderecoViewModel()
        {
            Id = Guid.NewGuid();
        }

        // Para esta lista de estados funcionar corretamente e necessario instalar o pacote abaixo:
        // install-package Microsoft.AspNetCore.Mvc.ViewFeatures
        public SelectList Estados()
        {
            return new SelectList(EstadoViewModel.ListarEstados(), "UF", "Nome");
        }

        [Key]
        public Guid Id { get; set; }

        public string Logradouro { get; set; }

        public string Numero { get; set; }

        public string Complemento { get; set; }

        public string Bairro { get; set; }

        public string CEP { get; set; }

        public string Cidade { get; set; }

        // O Estado sera oferecido atraves de um combo
        public string Estado { get; set; }
        public Guid EventoId { get; set; }

        // Esta sobreposicao permite exibir o endereco de uma vez inteiro de uma forma mais amigavel ao usuario
        public override string ToString()
        {
            return Logradouro + ", " + Numero + " - " + Bairro + ", " + Cidade + " - " + Estado;
        }
    }
}