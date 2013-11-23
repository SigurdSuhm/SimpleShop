#region Using Statements

using System;
using System.ComponentModel;

#endregion

namespace SimpleShop.Model
{
    /// <summary>
    /// Enum for item types.
    /// </summary>
    public enum ItemType
    {
        Bike,
        Shirt
    }

    /// <summary>
    /// Base class for items in the shop inventory.
    /// </summary>
    public abstract class Item : INotifyPropertyChanged
    {
        #region Fields

        // Item ID
        private int id;
        // Item description
        private string description;
        // Item price
        private double price;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of the item.
        /// </summary>
        public ItemType Type { get; protected set; }

        /// <summary>
        /// Gets or sets the ID of the item.
        /// </summary>
        public int ID
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        /// <summary>
        /// Gets or sets the description of the item.
        /// </summary>
        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        /// <summary>
        /// Gets or sets the price of the item.
        /// </summary>
        public double Price
        {
            get { return price; }
            set
            {
                if (price != value)
                {
                    price = value;
                    OnPropertyChanged("Price");
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the Item class.
        /// </summary>
        /// <param name="id">Item ID.</param>
        /// <param name="description">Item description.</param>
        /// <param name="price">Item price.</param>
        public Item(int id, string description, double price)
        {
            this.id = id;
            this.description = description;
            this.price = price;
        }

        #endregion

        #region INotifyPropertyChanged Members

        // Event handler for INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}