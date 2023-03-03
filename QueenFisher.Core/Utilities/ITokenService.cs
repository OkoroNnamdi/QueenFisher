using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueenFisher.Core.Utilities
{
    public interface ITokenService
    {
        string CreateToken(UserModel user);
        RefreshToken SetRefreshToken();
    }
}
