using Eventos.IO.Domain.Models;
using System;

namespace ConsoleTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            var evento = new Evento(
                            "Nome do Evento",
                            DateTime.Now,
                            DateTime.Now,
                            false,
                            50,
                            false,
                            "Eduardo Pires");
            
            // Estou usando o evento 2 para testar as validações do objeto
            var evento2 = new Evento(
                            "",
                            DateTime.Now,
                            DateTime.Now,
                            true,
                            50,
                            false,
                            "");

            Console.WriteLine(evento2.ToString());
            Console.WriteLine(evento2.EhValido());
            if (!evento2.ValidationResult.IsValid)
            {
                foreach (var erro in evento2.ValidationResult.Errors)
                {
                    Console.WriteLine(erro.ErrorMessage);
                }
            }

            Console.ReadKey();
        }
    }
}
