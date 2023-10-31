using System.Security.Claims;

namespace GreenMaterialBackEnd.Security
{
    public class Jwt
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }    
        public string Subject { get; set; }

        public static int retornaIdPorToken(ClaimsIdentity identity)
        {
            if (!identity.Claims.Any())
            {
                throw new Exception("Verificar token");
            }

            return int.Parse(identity.Claims.FirstOrDefault(x => x.Type == "id").Value);
        }
    }
}
