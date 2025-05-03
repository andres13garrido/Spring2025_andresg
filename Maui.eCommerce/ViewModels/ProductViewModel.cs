using System.ComponentModel;
using System.Runtime.CompilerServices;
using Library.eCommerce.Models;
using Library.eCommerce.Services;

namespace Maui.eCommerce.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private Item? cachedModel;
        public Item? Model { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        void NotifyPropertyChanged([CallerMemberName] string prop = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public string? Name
        {
            get => Model?.Product?.Name;
            set
            {
                if (Model != null && Model.Product!.Name != value)
                {
                    Model.Product.Name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int? Quantity
        {
            get => Model?.Quantity;
            set
            {
                if (Model != null && Model.Quantity != value)
                {
                    Model.Quantity = value;
                    NotifyPropertyChanged();
                }
            }
        }

        // ← NEW:
        public decimal Price
        {
            get => Model?.Price ?? 0m;
            set
            {
                if (Model != null && Model.Price != value)
                {
                    Model.Price = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public void AddOrUpdate()
        {
            ProductServiceProxy.Current.AddOrUpdate(Model);
        }

        public void Undo()
        {
            if (cachedModel != null)
                ProductServiceProxy.Current.AddOrUpdate(cachedModel);
        }

        public ProductViewModel()
        {
            Model = new Item();
            cachedModel = null;
        }
        public ProductViewModel(Item? model)
        {
            Model = model;
            if (model != null)
                cachedModel = new Item(model);
        }
    }
}
