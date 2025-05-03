using System;
using System.Linq;
using Maui.eCommerce.ViewModels;
using Microsoft.Maui.Controls;
using Library.eCommerce.Models;
using Library.eCommerce.Services;

namespace Maui.eCommerce.Views
{
    public partial class ShoppingManagementView : ContentPage
    {
        // strong‐typed shortcut to your VM:
        ShoppingManagementViewModel VM => (ShoppingManagementViewModel)BindingContext!;

        public ShoppingManagementView()
        {
            InitializeComponent();
            BindingContext = new ShoppingManagementViewModel();
        }

        // ← Return to main
        private async void ReturnToMainClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        // + button tapped
        private async void AddToCartClicked(object sender, EventArgs e)
        {
            // each Button's BindingContext is the ItemViewModel for that row
            if (sender is Button btn && btn.BindingContext is ItemViewModel ivm)
            {
                var id = ivm.Model.Id;

                // 1) Call your API to purchase
                await ShoppingCartServiceProxy.Current.Purchase(id);

                // 2) Reload inventory & cart from the server
                await ProductServiceProxy.Current.RefreshInventory();
                await ShoppingCartServiceProxy.Current.Refresh();

                // 3) Tell the VM/UI to redraw lists & totals
                VM.RefreshUX();
            }
        }

        // – button tapped
        private async void ReturnFromCartClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.BindingContext is ItemViewModel ivm)
            {
                var id = ivm.Model.Id;

                // 1) Call your API to return
                await ShoppingCartServiceProxy.Current.Return(id);

                // 2) Reload inventory & cart
                await ProductServiceProxy.Current.RefreshInventory();
                await ShoppingCartServiceProxy.Current.Refresh();

                // 3) Refresh the UI
                VM.RefreshUX();
            }
        }

        // Search cart filter
        private void SearchCartClicked(object sender, EventArgs e)
        {
            VM.SearchCart();
        }

        // Checkout
        private async void CheckoutClicked(object sender, EventArgs e)
        {
            var receipt = await ShoppingCartServiceProxy.Current.Checkout();

            var lines = receipt.Items
                               .Select(i => $"{i.Product.Name} x{i.Quantity} = {(i.Product.Price * i.Quantity):C}");

            await DisplayAlert(
                "Thank you for your purchase!",
                $"Total: {receipt.Total:C}\n\n" + string.Join("\n", lines),
                "OK"
            );

            // Final refresh
            await ProductServiceProxy.Current.RefreshInventory();
            await ShoppingCartServiceProxy.Current.Refresh();
            VM.RefreshUX();
        }
    }
}
