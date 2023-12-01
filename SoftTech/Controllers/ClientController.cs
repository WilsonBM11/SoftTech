using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SoftTech.Controllers
{
    [Authorize] //everyone can use this controller
    public class ClientController : Controller
    {
        
    }
}
