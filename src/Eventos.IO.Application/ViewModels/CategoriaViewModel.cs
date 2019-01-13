using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Eventos.IO.Application.ViewModels
{
    public class CategoriaViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }

        // Atendera o combo de Categorias
        public SelectList Categorias()
        {
            return new SelectList(ListarCategorias(), "Id", "Nome");
        }

        // Nesta versao do codigo as categorias ficarao aqui, nao sendo necessario criar um arquivo a parte
        // pois diferentemente do Estado que praticamente e muito dificil de mudar, uma categoria ainda que
        // seja dificil de mudar pode eventualmente ser necessario fazer isto.
        // Em versoes futuras colocaremos isto no banco de dados
        public List<CategoriaViewModel> ListarCategorias()
        {
            var categoriasList = new List<CategoriaViewModel>()
            {
                new CategoriaViewModel(){ Id = new Guid("ac381ba8-c187-482c-a5cb-899ad7176137"), Nome = "Congresso"},
                new CategoriaViewModel(){ Id = new Guid("1bbfa7e9-5a1f-4cef-b209-58954303dfc3"), Nome = "Meetup"},
                new CategoriaViewModel(){ Id = new Guid("d443f7c6-04e5-4f48-8fe0-9e6726b4fdb0"), Nome = "Workshop"}
            };

            return categoriasList;
        }
    }
}