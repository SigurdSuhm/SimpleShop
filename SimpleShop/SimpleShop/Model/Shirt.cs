#region Using Statements

#endregion

namespace SimpleShop.Model
{
    public enum Color
    {
        Red,
        Green,
        Blue,
        Yellow,
        Black,
        White
    }

    /// <summary>
    /// Shirt class derived from the Item class.
    /// </summary>
    public class Shirt : Item
    {
        #region Fields

        // Bike wheel type
        private Color color;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the wheel type of the bike.
        /// </summary>
        public Color Color
        {
            get { return color; }
            set
            {
                if (color != value)
                {
                    color = value;
                    OnPropertyChanged("WheelType");
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the Shirt class.
        /// </summary>
        /// <param name="id">Shirt ID.</param>
        /// <param name="description">Shirt description.</param>
        /// <param name="price">Shirt price.</param>
        /// <param name="wheelType">Shirt wheel type.</param>
        public Shirt(int id, string description, double price, Color color)
            : base(id, description, price)
        {
            Type = ItemType.Shirt;

            this.color = color;
        }

        #endregion
    }
}