namespace Web.Controllers
{
    using Domain.Models;
    using iText.Kernel.Pdf;
    using iText.Layout.Element;
    using iText.Layout.Properties;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Service.Implementation;
    using Service.Interface;
    using Stripe;
    using Stripe.Issuing;
    using System.Security.Claims;
    using iText.Layout;
    using System.IO;
    using System.Linq;
    using iText.Kernel.Exceptions;
    using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

    public class CartController : Controller
    {
        private readonly StripeService _stripeService;
        private readonly IAlbumService _albumService;
        private readonly IUserService _userService;
        private readonly ITrackService _trackService;

        public CartController(StripeService stripeService, IAlbumService albumService, IUserService userService, ITrackService trackService)
        {
            _stripeService = stripeService;
            _albumService = albumService;
            _userService = userService;
            _trackService = trackService;
        }

        private ShoppingCart GetShoppingCart()
        {
            var cartJson = HttpContext.Session.GetString("ShoppingCart");
            if (string.IsNullOrEmpty(cartJson))
            {
                return new ShoppingCart();
            }

            return JsonConvert.DeserializeObject<ShoppingCart>(cartJson);
        }

        private void SaveShoppingCart(ShoppingCart cart)
        {
            var cartJson = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("ShoppingCart", cartJson);
        }

        [HttpGet]
        public async Task<IActionResult> AddToCart(Guid? id, String type)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
            if (userId == null)
            {
                return Redirect("Identity/Account/Login");
            }

            if (id == null)
            {
                return BadRequest();
            }

            if (type.Equals("album"))
            {
                var album = await _albumService.GetById(id.Value);

                if (album == null)
                {
                    return NotFound();
                }

                var cart = GetShoppingCart();
                cart.Albums.Add(album);
                cart.Total += album.Price;
                SaveShoppingCart(cart);


            }
            else if (type.Equals("track"))
            {
                var track = await _trackService.GetById(id.Value);

                if (track == null)
                {
                    return NotFound();
                }

                var cart = GetShoppingCart();
                cart.Tracks.Add(track);
                cart.Total += track.Price;
                SaveShoppingCart(cart);


            }

            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            var cart = GetShoppingCart();
            return View(cart);
        }

        [HttpPost]
        public IActionResult Checkout()
        {
            double totalPrice = GetShoppingCart().Total;
            var session = _stripeService.CreateCheckoutSession(
                Url.Action("Success", "Cart", null, Request.Scheme),
                Url.Action("Cancel", "Cart", null, Request.Scheme),
                totalPrice
            );

            return Redirect(session.Url);
        }

        [HttpGet("checkout/success")]
        public async Task<IActionResult> SuccessAsync()
        {
            var cart = GetShoppingCart();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;

            foreach (var item in cart.Albums)
            {
                await _userService.BuyAlbum(userId, item.Id);
            }

            foreach (var item in cart.Tracks)
            {
                await _userService.BuyTrack(userId, item.Id);
            }

            return View("Success",cart);

        }

        [HttpGet("checkout/cancel")]
        public IActionResult Cancel()
        {
            var cart = GetShoppingCart();
            
            cart.Tracks.Clear();
            cart.Albums.Clear();
            cart.Total = 0;

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAlbum(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = GetShoppingCart();
            var album = await _albumService.GetById(id.Value);
            cart.Albums.RemoveAll(t => t.Id == album!.Id);
            cart.Total -= album!.Price;

            SaveShoppingCart(cart);

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> DeleteTrack(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = GetShoppingCart();
            var track = await _trackService.GetById(id.Value);
            cart.Tracks.RemoveAll(t => t.Id == track!.Id);
            cart.Total -= track!.Price;

            SaveShoppingCart(cart);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("checkout/download-invoice")]
        public IActionResult DownloadInvoice()
        {
            var cart = GetShoppingCart();

            byte[] pdfBytes = GeneratePdf(cart);

            HttpContext.Session.Remove("ShoppingCart");

            return File(pdfBytes, "application/pdf", "Invoice.pdf");
        }

        private byte[] GeneratePdf(ShoppingCart cart)
        {
            var htmlContent = GenerateInvoiceHtml(cart);
            var converter = new SelectPdf.HtmlToPdf();

            var pdfDocument = converter.ConvertHtmlString(htmlContent);

            using (var memoryStream = new MemoryStream())
            {
                pdfDocument.Save(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private string GenerateInvoiceHtml(ShoppingCart cart)
        {   
            var albumsHtml = string.Join("", cart.Albums.Select(album =>
                $"<tr><td>Album</td><td>{album.Title}</td><td>{album.Artist?.Name}</td><td>{album.Price:C}</td></tr>"));
            var tracksHtml = string.Join("", cart.Tracks.Select(track =>
                $"<tr><td>Track</td><td>{track.Title}</td><td>{track.Artist?.Name}</td><td>{track.Price:C}</td></tr>"));

            return $@"
                <html>
                <head>
                    <style>
                        table {{ width: 100%; border-collapse: collapse; }}
                        th, td {{ border: 1px solid black; padding: 10px; text-align: left; }}
                           h1 {{ text-align: center}}
                    </style>
                </head>
                <body>
                    <h1>Invoice</h1>
                    <p>Date: {DateTime.Now.ToShortDateString()}</p>
                    <table>
                        <thead>
                            <tr>
                                <th>Album/Track</th>
                                <th>Title</th>
                                <th>Artist</th>
                                <th>Price</th>
                            </tr>
                        </thead>
                        <tbody>
                            {albumsHtml}
                            {tracksHtml}
                        </tbody>
                    </table>
                    <h2>Total: {cart.Total:C}</h2>
                    <p>Thank you for your purchase!</p>
                </body>
    </html>";
        }

        //[HttpGet("checkout/cancel")]
        //public IActionResult Cancel()
        //{
        //    return View();
        //}
    }

}
