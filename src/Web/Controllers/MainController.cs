using BLL.DTOs;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;

[ApiController]
[Route("api/[controller]")]
public class MainController : ControllerBase
{
    private readonly IEnumerable<IExchangeApiClient> _clients;

    public MainController(IEnumerable<IExchangeApiClient> clients)
    {
        _clients = clients;
    }
    
}