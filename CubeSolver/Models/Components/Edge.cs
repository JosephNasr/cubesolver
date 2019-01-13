using CubeSolver.Models.Enums;

namespace CubeSolver.Models.Components
{
    public class Edge
    {
        public Face Face1 { get; }
        public Face Face2 { get; }

        public bool IsFace1Placed => Face1.Color == Face1.Center;
        public bool IsFace2Placed => Face2.Color == Face2.Center;
        public bool IsPlaced => IsFace1Placed && IsFace2Placed;

        public Edge(Color firstColor, Color secondColor)
        {
            Face1 = new Face(firstColor);
            Face2 = new Face(secondColor);
        }

        public bool ContainsColor(Color color)
        {
            return Face1.Color == color || Face2.Color == color;
        }

        public bool ContainsCenter(Color center)
        {
            return Face1.Center == center || Face2.Center == center;
        }

        public (Face coloredFace, Face invertedColoredFace) GetFacesByColor(Color color)
        {
            return Face1.Color == color ? (Face1, Face2) : (Face2, Face1);
        }

        public (Face centeredFace, Face invertedCenteredFace) GetFacesByCenter(Color center)
        {
            return Face1.Center == center ? (Face1, Face2) : (Face2, Face1);
        }
    }
}
