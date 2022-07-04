using GemBox.Document;
using GemBox.Pdf;
using homework.Domain.Dto;
using homework.Domain.Models;
using homework.Repository.Interface;
using homework.Service.Interface;
using homework.web.ExportModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace homework.web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IWebHostEnvironment environment;
        private readonly IUserRepository _userRepository;
        private readonly ITicketService _ticketService;

        public UserController(ITicketService ticketService, IShoppingCartService shoppingCartService, IOrderItemRepository orderItemRepository, IWebHostEnvironment environment, IUserRepository userRepository)
        {
            _shoppingCartService = shoppingCartService;
            _orderItemRepository = orderItemRepository;
            this.environment = environment;
            _userRepository = userRepository;
            _ticketService = ticketService;

            GemBox.Document.ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            GemBox.Pdf.ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        [HttpGet]
        public IActionResult Purchases()
        {
            var user = _userRepository.Get(User.Identity.Name);
            var purchases = _shoppingCartService.FindAllFromUser(User.Identity.Name);
            ViewData["Purchases"] = purchases;
            List<int> prices = new List<int>();
            foreach (var purchase in purchases)
            {
                int price = 0;
                foreach(var orderItem in _shoppingCartService.FindAllFromPurchase(purchase.Id))
                {
                    price += orderItem.Quantity * (int)orderItem.Screaning.Price;
                }

                prices.Add(price);
            }
            ViewData["Prices"] = prices;

            ViewData["Tickets"] = _ticketService.GetAllValidFromUser(user.Id);

            return View();
        }

        [HttpGet]
        public IActionResult Purchase(Guid cartId, int price)
        {
            var cart = _shoppingCartService.FindById(cartId);
            List<PurchaseItemViewModel> items = new List<PurchaseItemViewModel>();
            cart.OrderItems.ForEach(i => items.Add(_shoppingCartService.GetPurchaseItemViewModel(i)));
            ViewData["items"] = items;
            ViewData["Price"] = price.ToString();

            return View(cart);
        }

        // Testing export to pdf
        [HttpPost]
        public IActionResult ExportPurchase(Guid purchaseId, List<Guid> items)
        {
            // Find data
            var purchase = _shoppingCartService.GetPurchaseViewModel(purchaseId, items);
            var model = this.ViewModelToExport(purchase);
            var user = _userRepository.Get(User.Identity.Name);

            DocumentModel document = new DocumentModel();

            Section section = new Section(document);
            document.Sections.Add(section);

            Paragraph paragraph = new Paragraph(document);
            section.Blocks.Add(paragraph);

            StringBuilder str = new StringBuilder();
            var line = string.Format("Invoice for user: {0}", user.UserName);
            Run run = new Run(document, line);
            paragraph.Inlines.Add(run);

            paragraph = new Paragraph(document);
            section.Blocks.Add(paragraph);
            line = string.Format("Purchased on: {0}", model.TimeOfPurchase);
            run = new Run(document, line);
            paragraph.Inlines.Add(run);

            paragraph = new Paragraph(document);
            section.Blocks.Add(paragraph);
            line = string.Format("Total price: {0}", model.Price);
            run = new Run(document, line);
            paragraph.Inlines.Add(run);

            paragraph = new Paragraph(document);
            section.Blocks.Add(paragraph);
            line = "Purchased items:";
            run = new Run(document, line);
            paragraph.Inlines.Add(run);

            foreach (var item in model.Items)
            {
                paragraph = new Paragraph(document);
                section.Blocks.Add(paragraph);
                line = string.Format("Ticket for movie: {0}, Screens at: {1}, Individual price: {2}, Quantity: {3}, Total price: {4}",
                    item.Movie,
                    item.ScreaningTime,
                    item.Price,
                    item.Quantity,
                    item.TotalPrice());

                run = new Run(document, line);
                paragraph.Inlines.Add(run);
            }

            var stream = new MemoryStream();
            document.Save(stream, new GemBox.Document.PdfSaveOptions());

            return File(stream.ToArray(), new GemBox.Document.PdfSaveOptions().ContentType , "export.pdf");
        }

        private ExportPucrhase ViewModelToExport(PurchaseViewModel vm)
        {
            ExportPucrhase model = new ExportPucrhase();
            model.Price = vm.Price;
            model.TimeOfPurchase = vm.TimeOfPurchase;
            model.Items = vm.Items;

            return model;
        }
    }
}
