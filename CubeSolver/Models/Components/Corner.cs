using CubeSolver.Models.Enums;

namespace CubeSolver.Models.Components
{
    public class Corner
    {
        public Face Face1 { get; }
        public Face Face2 { get; }
        public Face Face3 { get; }

        public bool IsFace1Placed => Face1.IsPlaced;
        public bool IsFace2Placed => Face2.IsPlaced;
        public bool IsFace3Placed => Face3.IsPlaced;
        public bool IsPlaced => (IsFace1Placed && IsFace2Placed) || (IsFace1Placed && IsFace3Placed) || (IsFace2Placed && IsFace3Placed);

        public Corner(Color firstColor, Color secondColor, Color thirdColor)
        {
            Face1 = new Face(firstColor);
            Face2 = new Face(secondColor);
            Face3 = new Face(thirdColor);
        }

        public bool ContainsColor(Color color)
        {
            return Face1.Color == color || Face2.Color == color || Face3.Color == color;
        }

        public bool ContainsCenter(Color center)
        {
            return Face1.Center == center || Face2.Center == center || Face3.Center == center;
        }

        public (Face coloredFace, Face otherColoredFace1, Face otherColoredFace2) GetFacesByColor(Color color)
        {
            return Face1.Color == color ? (Face1, Face2, Face3) : Face2.Color == color ? (Face2, Face1, Face3) : (Face3, Face1, Face2);
        }

        public (Face centeredFace, Face otherCenteredFace1, Face otherCenteredFace2) GetFacesByCenter(Color center)
        {
            return Face1.Center == center ? (Face1, Face2, Face3) : Face2.Center == center ? (Face2, Face1, Face3) : (Face3, Face1, Face2);
        }
    }
}
