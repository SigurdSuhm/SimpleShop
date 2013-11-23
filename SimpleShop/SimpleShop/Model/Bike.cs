#region Using Statements

#endregion

namespace SimpleShop.Model
{
    /// <summary>
    /// Bike class derived from the Item class.
    /// </summary>
    public class Bike : Item
    {
        #region Fields

        // Bike wheel type
        private string wheelType;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the wheel type of the bike.
        /// </summary>
        public string WheelType
        {
            get { return wheelType; }
            set
            {
                if (wheelType != value)
                {
                    wheelType = value;
                    OnPropertyChanged("WheelType");
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the Bike class.
        /// </summary>
        /// <param name="id">Bike ID.</param>
        /// <param name="description">Bike description.</param>
        /// <param name="price">Bike price.</param>
        /// <param name="wheelType">Bike wheel type.</param>
        public Bike(int id, string description, double price, string wheelType)
            : base(id, description, price)
        {
            Type = ItemType.Bike;

            this.wheelType = wheelType;
        }

        #endregion
    }
}