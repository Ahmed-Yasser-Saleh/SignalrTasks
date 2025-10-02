using Microsoft.AspNetCore.Mvc;
using signalrTask.ViewModels;
using signalrTask.Models;
using System.Diagnostics;
using signalrTask.Context;
using Microsoft.AspNetCore.SignalR;
using signalrTask.Hubs;

namespace signalrTask.Controllers
{
    public class EmployeeController : Controller
    {
        MyContext _context;
        IHubContext<displayEmpHub> _hub;
        public EmployeeController(MyContext _context, IHubContext<displayEmpHub> _hub)
        {
            this._context = _context;
            this._hub = _hub;
        }

        public IActionResult Getall()
        {
            var emps = _context.Employees.Select(e => new EmplyeeViewmodel { name = e.name, address = e.address, age = e.age }).ToList();
            return View(emps);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult Save(Employee emp, string connectionid)
        {
            _context.Employees.Add(emp);
            _context.SaveChanges();
            //call back
            _hub.Clients.AllExcept(connectionid).SendAsync("New Employee Added", emp);
            return RedirectToAction("Getall");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
