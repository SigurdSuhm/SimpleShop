#region Using Statements

using System;
using System.Windows.Input;

using SimpleShop.Helpers;
using SimpleShop.Model;
using System.Windows;

#endregion

namespace SimpleShop.ViewModel
{
    /// <summary>
    /// ViewModel for creating a new item.
    /// </summary>
    internal class AddItemViewModel : ViewModelBase
    {
        #region Fields

        // Indicates if the item should be added to the inventory or discarded
        private bool createItem;
        // Item being created
        private Item item;
        // Type of the item being created as a string
        private string itemType;

        // Wheel type for bikes
        private string wheelType;
        // Color for shirts
        private Color color;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the view associated with the view model.
        /// </summary>
        public Window View { get; set; }

        /// <summary>
        /// Gets the boolean value indicating if the item should be created or discarded.
        /// </summary>
        public bool CreateItem
        {
            get { return createItem; }
        }

        /// <summary>
        /// Gets the item currently being created.
        /// </summary>
        public Item Item
        {
            get { return item; }
        }

        /// <summary>
        /// Gets or sets the wheel type for bikes.
        /// </summary>
        public string WheelType
        {
            get { return wheelType; }
            set
            {
                if (wheelType != value)
                {
                    wheelType = value;

                    if (item is Bike)
                        (item as Bike).WheelType = wheelType;

                    OnPropertyChanged("WheelType");
                }
            }
        }

        /// <summary>
        /// Gets or sets the color for shirts.
        /// </summary>
        public Color Color
        {
            get { return color; }
            set
            {
                if (color != value)
                {
                    color = value;

                    if (item is Shirt)
                        (item as Shirt).Color = color;

                    OnPropertyChanged("Color");
                }
            }
        }

        /// <summary>
        /// Gets or sets the type of the item.
        /// </summary>
        public string ItemType
        {
            get { return itemType; }
            set
            {
                if (itemType != value)
                {
                    itemType = value;
                    updateItemByType();
                    OnPropertyChanged("ItemType");
                }
            }
        }

        /// <summary>
        /// Gets the boolean value indicating if the unique properties for bikes should be visible.
        /// </summary>
        public Visibility BikePropertiesVisibility
        {
            get { return itemType == "Bike" ? Visibility.Visible : Visibility.Collapsed; }
        }

        /// <summary>
        /// Gets the boolean value indicating if the unique properties for shirts should be visible.
        /// </summary>
        public Visibility ShirtPropertiesVisibility
        {
            get { return itemType == "Shirt" ? Visibility.Visible : Visibility.Collapsed; }
        }

        #endregion

        #region Command Handlers

        /// <summary>
        /// Gets the command for adding the item.
        /// </summary>
        public ICommand AddItemCommand { get; private set; }

        /// <summary>
        /// Gets the command for canceling the item creation.
        /// </summary>
        public ICommand CancelCommand { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the AddItemViewModel class.
        /// </summary>
        public AddItemViewModel()
        {
            createItem = false;
            item = new Bike(-1, String.Empty, 0.0, String.Empty);

            // Initialize UI commands
            AddItemCommand = new RelayCommand(addItemCommand_Execute, addItemCommand_CanExecute);
            CancelCommand = new RelayCommand(cancelCommand_Execute);
        }

        #endregion

        #region UI Commands

        /// <summary>
        /// Closes the view and adds the item though the ShopViewModel.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        private void addItemCommand_Execute(object parameter)
        {
            createItem = true;
            View.Close();
        }

        /// <summary>
        /// Determines if the AddItemCommand can execute.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>True if the AddItemCommand can execute.</returns>
        private bool addItemCommand_CanExecute(object parameter)
        {
            bool returnValue = true;

            return returnValue;
        }

        /// <summary>
        /// Cancels the item creation process.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        private void cancelCommand_Execute(object parameter)
        {
            View.Close();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the item currently being created based on selected type.
        /// </summary>
        private void updateItemByType()
        {
            Item newItem = null;

            if (itemType == "Bike")
                newItem = new Bike(item.ID, item.Description, item.Price, wheelType);
            else if (itemType == "Shirt")
                newItem = new Shirt(item.ID, item.Description, item.Price, color);
            else
                return;

            item = newItem;

            OnPropertyChanged("BikePropertiesVisibility");
            OnPropertyChanged("ShirtPropertiesVisibility");
            OnPropertyChanged("Item");
        }

        #endregion
    }
}
