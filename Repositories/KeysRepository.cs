using Pix.Models;

namespace Pix.Repositories;

public class KeysRepository
{
    private readonly Keys[] Keys = [];

    public Keys CreateKey(Keys key)
    {
        _ = Keys.Append(key);
        return key;
    }
}