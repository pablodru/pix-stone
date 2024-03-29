using Pix.DTOs;
using Pix.Exceptions;
using Pix.Models;
using Pix.RabbitMQ;

namespace Pix.Services;

public class ConcilliationService(ConcilliationProducer concilliationProducer)
{
    private readonly ConcilliationProducer _concilliationProducer = concilliationProducer;
    public void CreateConcilliation(ConcilliationDTO dto, Bank validatedBank)
    {
        try {
            _concilliationProducer.PublishConcilliation(dto, validatedBank);
        } catch (Exception e)
        {
            throw new RabbitMqException("Internal Server Error. Concilliation was not published in the queue.");
        }
        return;
    }
}