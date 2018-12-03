using Invoisys.Infrastructure.CrossCutting.Util;
using System.Web;

namespace Invoisys.Presentation.Web.DTO
{
    public class ModelDTO
    {
        public string IdCripto => HttpUtility.UrlEncode(EncodedActionLinkExtensions.Encrypt("id=" + Id));
        public string Id { get; set; }
        public string Name { get; set; }
    }
}