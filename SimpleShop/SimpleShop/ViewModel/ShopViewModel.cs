#region Using Statements

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using Microsoft.Win32;

using SimpleShop.Helpers;
using SimpleShop.Model;

#endregion

namespace SimpleShop.ViewModel
{
    /// <summary>
    /// ViewModel for the shop.
    /// </summary>
    internal class ShopViewModel : ViewModelBase
    {
        #region Fields

        // Collection of items in the shop inventory
        private ObservableCollection<Item> itemCollection;
        // Selected item
        private Item selectedItem;
        // Collection of properties for the selected item
        private ObservableCollection<ItemInformationProperty> selectedItemProperties;

        // Name of the currently loaded file
        private string loadedFileName;
        // Flag indicating if changes have been made to the loaded file
        private bool loadedFileDirty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the collection of items in the shop inventory.
        /// </summary>
        public ObservableCollection<Item> ItemCollection
        {
            get { return itemCollection; }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        public Item SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    updateSelectedItemProperties(selectedItem, new PropertyChangedEventArgs("SelectedItem"));

                    if (selectedItem != null)
                        selectedItem.PropertyChanged += updateSelectedItemProperties;

                    OnPropertyChanged("SelectedItem");
                }
            }
        }

        /// <summary>
        /// Gets the collection of properties for the selected item.
        /// </summary>
        public ObservableCollection<ItemInformationProperty> SelectedItemProperties
        {
            get { return selectedItemProperties; }
        }

        /// <summary>
        /// Gets the name of the currently loaded file.
        /// </summary>
        public string LoadedFileName
        {
            get { return loadedFileName; }
            private set
            {
                if (loadedFileName != value)
                {
                    loadedFileName = value;
                    OnPropertyChanged("LoadedFileName");
                }
            }
        }

        /// <summary>
        /// Gets the flag indicating if the loaded file has been changed.
        /// </summary>
        public bool LoadedFileDirty
        {
            get { return loadedFileDirty; }
            private set
            {
                if (loadedFileDirty != value)
                {
                    loadedFileDirty = value;
                    OnPropertyChanged("LoadedFileDirty");
                }
            }
        }

        #endregion

        #region Command Handlers

        /// <summary>
        /// Gets the command for creating a new file.
        /// </summary>
        public ICommand NewFileCommand { get; private set; }

        /// <summary>
        /// Gets the command for opening a file.
        /// </summary>
        public ICommand OpenFileCommand { get; private set; }

        /// <summary>
        /// Gets the command for saving inventory to the loaded file.
        /// </summary>
        public ICommand SaveFileCommand { get; private set; }

        /// <summary>
        /// Gets the command for saving inventory to a specified file.
        /// </summary>
        public ICommand SaveAsFileCommand { get; private set; }

        /// <summary>
        /// Gets the command for exiting the application.
        /// </summary>
        public ICommand ExitCommand { get; private set; }

        /// <summary>
        /// Gets the command for showing the about box.
        /// </summary>
        public ICommand AboutCommand { get; private set; }

        /// <summary>
        /// Gets the command for creating a new item.
        /// </summary>
        public ICommand AddItemCommand { get; private set; }

        /// <summary>
        /// Gets the command for deleting the selected item.
        /// </summary>
        public ICommand DeleteItemCommand { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the ShopViewModel class.
        /// </summary>
        public ShopViewModel()
        {
            itemCollection = new ObservableCollection<Item>();
            loadedFileName = null;
            LoadedFileDirty = false;

            selectedItemProperties = new ObservableCollection<ItemInformationProperty>();

            // Initialize UI commands
            NewFileCommand = new RelayCommand(newFileCommand_Execute);
            OpenFileCommand = new RelayCommand(openFileCommand_Execute);
            SaveFileCommand = new RelayCommand(saveFileCommand_Execute);
            SaveAsFileCommand = new RelayCommand(saveAsFileCommand_Execute);
            ExitCommand = new RelayCommand(exitCommand_Execute);
            AboutCommand = new RelayCommand(aboutCommand_Execute);
            AddItemCommand = new RelayCommand(addItemCommand_Execute);
            DeleteItemCommand = new RelayCommand(deleteItemCommand_Execute);
        }

        #endregion

        #region UI Commands

        /// <summary>
        /// Creates a new inventory file.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        private void newFileCommand_Execute(object parameter)
        {
            itemCollection.Clear();
            LoadedFileName = null;
            LoadedFileDirty = false;
        }

        /// <summary>
        /// Opens an existing inventory file.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        private void openFileCommand_Execute(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Xml Files (*.xml)|*.xml";

            bool? dialogResult = openFileDialog.ShowDialog();

            if (dialogResult == true)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(openFileDialog.FileName);

                itemCollection.Clear();

                XmlNodeList itemNodeList = xmlDoc.DocumentElement.SelectNodes("Item");

                foreach (XmlNode itemNode in itemNodeList)
                {
                    itemCollection.Add(parseItemXmlNode(itemNode));
                }

                LoadedFileName = openFileDialog.FileName;
                LoadedFileDirty = false;
            }
        }

        /// <summary>
        /// Saves the current inventory file.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        private void saveFileCommand_Execute(object parameter)
        {
            if (loadedFileName == null)
                saveAsFileCommand_Execute(parameter);
            else
                writeInventoryToFile(loadedFileName);
        }

        /// <summary>
        /// Saves the current inventory to the specified file.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        private void saveAsFileCommand_Execute(object parameter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Xml Files (*.xml)|*.xml";
            saveFileDialog.DefaultExt = ".xml";
            saveFileDialog.FileName = "ItemList";

            bool? dialogResult = saveFileDialog.ShowDialog();

            if (dialogResult == true)
                writeInventoryToFile(saveFileDialog.FileName);
        }

        /// <summary>
        /// Exits the application.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        private void exitCommand_Execute(object parameter)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Shows the 'About' box.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        private void aboutCommand_Execute(object parameter)
        {
            MessageBox.Show("SimpleShop v2.0 by Sigurd Suhm\n\nIcons by FamFamFam", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Opens the view for creating a new item.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        private void addItemCommand_Execute(object parameter)
        {
            View.AddItemWindow addItemWindow = new View.AddItemWindow();
            AddItemViewModel addItemViewModel = addItemWindow.DataContext as AddItemViewModel;
            addItemViewModel.View = addItemWindow;

            addItemWindow.ShowDialog();
            
            if (addItemViewModel.CreateItem)
                itemCollection.Add(addItemViewModel.Item);
        }

        /// <summary>
        /// Deletes the selected item.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        private void deleteItemCommand_Execute(object parameter)
        {
            MessageBoxResult deleteItemResult = MessageBox.Show(String.Format("Are you sure you want to delete the item [ID: {0}] {1}?", selectedItem.ID, selectedItem.Description),
                "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (deleteItemResult == MessageBoxResult.Yes)
            {
                itemCollection.Remove(selectedItem);
                LoadedFileDirty = true;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the collection of properties for the selected item.
        /// </summary>
        private void updateSelectedItemProperties(object sender, PropertyChangedEventArgs e)
        {
            selectedItemProperties.Clear();

            if (selectedItem != null)
            {
                // Get common properties for items
                selectedItemProperties.Add(new ItemInformationProperty("ID", selectedItem.ID));
                selectedItemProperties.Add(new ItemInformationProperty("Description", selectedItem.Description));
                selectedItemProperties.Add(new ItemInformationProperty("Price", selectedItem.Price));

                // Get properties unique to derived classes
                if (selectedItem is Bike)
                    selectedItemProperties.Add(new ItemInformationProperty("Wheel type", (selectedItem as Bike).WheelType));
                else if (selectedItem is Shirt)
                    selectedItemProperties.Add(new ItemInformationProperty("Color", (selectedItem as Shirt).Color));
            }
        }

        /// <summary>
        /// Parses an Xml node and returns an item from the node.
        /// </summary>
        /// <param name="itemNode">Xml node to parse.</param>
        /// <returns>Item object.</returns>
        private Item parseItemXmlNode(XmlNode itemNode)
        {
            Item returnItem = null;

            if (itemNode["Type"].InnerText == "Bike")
            {
                returnItem = new Bike(int.Parse(itemNode["ID"].InnerText),
                        itemNode["Description"].InnerText,
                        double.Parse(itemNode["Price"].InnerText),
                        itemNode["WheelType"].InnerText); 
            }
            else if (itemNode["Type"].InnerText == "Shirt")
            {
                returnItem = new Shirt(int.Parse(itemNode["ID"].InnerText),
                        itemNode["Description"].InnerText,
                        double.Parse(itemNode["Price"].InnerText),
                        (Color)Enum.Parse(typeof(Color), itemNode["Color"].InnerText));
            }

            return returnItem;
        }

        /// <summary>
        /// Writes the inventory to and Xml file at the specified path.
        /// </summary>
        /// <param name="fileName">Path to write to.</param>
        private void writeInventoryToFile(string fileName)
        {
            using (XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartDocument();
                writer.WriteStartElement("ItemList");

                foreach (Item item in itemCollection)
                {
                    string type = null;

                    if (item is Bike)
                        type = "Bike";
                    else if (item is Shirt)
                        type = "Shirt";

                    writer.WriteStartElement("Item");

                    writer.WriteElementString("Type", type);
                    writer.WriteElementString("ID", item.ID.ToString());
                    writer.WriteElementString("Description", item.Description);
                    writer.WriteElementString("Price", item.Price.ToString());

                    if (type == "Bike")
                        writer.WriteElementString("WheelType", (item as Bike).WheelType);
                    else if (type == "Shirt")
                        writer.WriteElementString("Color", (item as Shirt).Color.ToString());

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            LoadedFileName = fileName;
            LoadedFileDirty = false;
        }

        #endregion
    }
}