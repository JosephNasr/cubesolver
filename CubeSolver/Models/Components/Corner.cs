using CubeSolver.Models.Enums;

namespace CubeSolver.Models.Components
{
    public class Corner
    {
        public Face Face1 { get; set; }
        public Face Face2 { get; set; }
        public Face Face3 { get; set; }

        public Corner(Color color1, Color color2, Color color3)
        {
            Face1 = new Face
            {
                Center = color1,
                 Color = color1,
            };

            Face2 = new Face
            {
                Center = color2,
                 Color = color2,
            };

            Face3 = new Face
            {
               Center = color3,
                Color = color3,
            };
        }
    }
}
