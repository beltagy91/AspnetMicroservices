﻿using System;
using System.Threading.Tasks;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class CheckOutModel : PageModel
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public CheckOutModel(IBasketService basketService, IOrderService orderService)
        {
            _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }

        [BindProperty]
        public BasketCheckoutModel Order { get; set; }

        public BasketModel Cart { get; set; } = new BasketModel();

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await _basketService.GetBasket("boda");
            return Page();
        }

        public async Task<IActionResult> OnPostCheckOutAsync()
        {
            Cart = await _basketService.GetBasket("boda");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Order.UserName = "boda";
            Order.TotalPrice = Cart.TotalPrice;

            await _basketService.CheckoutBasket(Order);
            
            return RedirectToPage("Confirmation", "OrderSubmitted");
        }       
    }
}