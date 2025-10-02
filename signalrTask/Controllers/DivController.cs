using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using signalrTask.Hubs;

namespace signalrTask.Controllers
{
    public class DivController : Controller
    {
        IHubContext<MoveDivHub> _hubContext;
        public DivController(IHubContext<MoveDivHub> _hubContext)
        {
            this._hubContext = _hubContext;
        }
        public IActionResult index()
        {
            return View();
        }
    }
}
