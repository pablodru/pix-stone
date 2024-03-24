using Pix.DTOs;
using Pix.Models;
using Pix.RabbitMQ;

namespace Pix.Services;

public class ConcilliationService(ConcilliationProducer concilliationProducer)
{
    private readonly ConcilliationProducer _concilliationProducer = concilliationProducer;
    public void CreateConcilliation(ConcilliationDTO dto, Bank validatedBank)
    {
        _concilliationProducer.PublishConcilliation(dto, validatedBank);
        return;
    }
}