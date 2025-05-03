using Library.eCommerce.Models;
using Library.eCommerce.Services;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maui.eCommerce.ViewModels
{
    public class ShoppingCartViewModel : INotifyPropertyChanged
    {
        private readonly ShoppingCartServiceProxy _svc = ShoppingCartServiceProxy.Current;

        public ShoppingCartViewModel()
        {
            RefreshCommand = new Command(async () => await LoadCart());
            SearchCommand = new Command(async () => { await _svc.Search(Query); OnPropertyChanged(nameof(CartItems)); });
            PurchaseCommand = new Command<int>(async id => { await _svc.Purchase(id); await LoadCart(); });
            ReturnCommand = new Command<int>(async id => { await _svc.Return(id); await LoadCart(); });
            CheckoutCommand = new Command(async () =>
            {
                Receipt = await _svc.Checkout();
                await LoadCart();
            });
        }

        /// <summary>
        /// The list bound to your ListView.
        /// </summary>
        public ObservableCollection<Item> CartItems
            => new ObservableCollection<Item>(_svc.CartItems);

        private string _query = string.Empty;
        public string Query
        {
            get => _query;
            set { _query = value; OnPropertyChanged(); }
        }

        private Receipt? _receipt;
        public Receipt? Receipt
        {
            get => _receipt;
            set { _receipt = value; OnPropertyChanged(); }
        }

        public ICommand RefreshCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand PurchaseCommand { get; }
        public ICommand ReturnCommand { get; }
        public ICommand CheckoutCommand { get; }

        private async Task LoadCart()
        {
            await _svc.Refresh();
            OnPropertyChanged(nameof(CartItems));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
