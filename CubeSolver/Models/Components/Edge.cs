using CubeSolver.Models.Enums;

namespace CubeSolver.Models.Components
{
    public class Edge
    {
        public Face Face1 { get; set; }
        public Face Face2 { get; set; }

        public Edge(Color color1, Color color2)
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
        }
    }
}
