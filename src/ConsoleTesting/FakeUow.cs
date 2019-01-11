using Eventos.IO.Domain.Core.Commands;
using Eventos.IO.Domain.Interfaces;
using System;

public class FakeUow : IUnitOfWork
{
    public CommandResponse Commit()
    {
        // Aqui estamos simulando um evento gravou corretamente no banco de dados
        return new CommandResponse(true);

        // Habilitar aqui para simjlar um evento que não gravou corretamente no banco de dados
        // return new CommandResponse(false);
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
