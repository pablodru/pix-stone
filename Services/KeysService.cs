using Microsoft.AspNetCore.Mvc;
using Pix.DTOs;
using Pix.Models;
using Pix.Repositories;


namespace Pix.Services;

public class KeyService
{
    private readonly KeysRepository _keyRepository;

    public KeyService(KeysRepository keyRepository)
    {
        _keyRepository = keyRepository;
    }

    public Keys CreateKey(CreateKeyDTO dto)
    {
        Keys key = _keyRepository.CreateKey(dto.ToEntity());
        return key;
    }
}