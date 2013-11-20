#region Using Statements

using System.ComponentModel;

#endregion

namespace SimpleShop.ViewModel
{
    internal class ItemInformationProperty : INotifyPropertyChanged
    {
        #region Fields

        // Property description
        private string description;
        // Property value
        private object value;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the description of the property.
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
        /// Gets or sets the value of the property.
        /// </summary>
        public object Value
        {
            get { return this.value; }
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    OnPropertyChanged("Value");
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the ItemInformationProperty class.
        /// </summary>
        public ItemInformationProperty(string description, object value)
        {
            this.description = description;
            this.value = value;
        }

        #endregion

        #region INotifyPropertyChanged Members

        // Event handler for INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property changed.</param>
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}