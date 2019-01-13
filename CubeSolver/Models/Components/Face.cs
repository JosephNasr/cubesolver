using CubeSolver.Models.Enums;

namespace CubeSolver.Models.Components
{
    public class Face
    {
        public Color Color { get; }
        public Color Center { get; private set; }

        public bool IsPlaced => Color == Center;

        public Face(Color color) : this(color, color) { }

        public Face(Color color, Color center)
        {
            Color = color;
            Center = center;
        }

        public void ChangeCenter(Color center)
        {
            Center = center;
        }
    }
}
