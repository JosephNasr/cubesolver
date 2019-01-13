using CubeSolver.Models.Components;
using CubeSolver.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CubeSolver.Models
{
    public class Cube
    {
        private readonly IEnumerable<Edge> _edges;
        private readonly IEnumerable<Corner> _corners;

        /// <summary>
        /// List of adjacent colors for each center (in clockwise order)
        /// </summary>
        private readonly IDictionary<Color, IEnumerable<Color>> _adjacentColorMap;

        public Cube()
        {
            _edges = GetEdges();
            _corners = GetCorners();
            _adjacentColorMap = GetColorMap();
        }

        public void Clockwise(Color centerColor)
        {
            Rotate(centerColor, true);
        }

        public void AntiClockwise(Color centerColor)
        {
            Rotate(centerColor, false);
        }

        public void Scramble(int moves = 20)
        {
            var random = new Random();
            var colors = Enum.GetValues(typeof(Color));

            Console.Write("Scramble: ");

            for (var counter = moves; counter > 0; counter--)
            {
                var parity = random.Next(2);
                var selectedColor = (Color)colors.GetValue(random.Next(colors.Length));

                (parity == 0 ? (Action<Color>)Clockwise : AntiClockwise)(selectedColor);

                Console.Write(selectedColor.ToString()[0] + (parity == 0 ? "" : "'") + " ");
            }

            Console.WriteLine("\n");
        }

        public void Print(bool withDetails = false)
        {
            var allColors = Enum.GetValues(typeof(Color));

            foreach (Color color in allColors)
            {
                PrintCubeFace(color, withDetails);
            }
        }

        private void Rotate(Color centerColor, bool isClockwise)
        {
            RotateEdges(centerColor, isClockwise);
            RotateCorners(centerColor, isClockwise);
        }

        private void RotateEdges(Color centerColor, bool isClockwise)
        {
            var adjacentColors = _adjacentColorMap[centerColor];

            foreach (var edge in _edges.Where(edge => edge.ContainsCenter(centerColor)))
            {
                var (centeredFace, invertedCenteredFace) = edge.GetFacesByCenter(centerColor);
                RotateFace(invertedCenteredFace, adjacentColors, isClockwise);
            }
        }

        private void RotateCorners(Color centerColor, bool isClockwise)
        {
            var adjacentColors = _adjacentColorMap[centerColor];

            foreach (var corner in _corners.Where(corner => corner.ContainsCenter(centerColor)))
            {
                var (centeredFace, otherCenteredFace1, otherCenteredFace2) = corner.GetFacesByCenter(centerColor);
                RotateFace(otherCenteredFace1, adjacentColors, isClockwise);
                RotateFace(otherCenteredFace2, adjacentColors, isClockwise);
            }
        }

        private void RotateFace(Face faceToRotate, IEnumerable<Color> adjacentColors, bool isClockwise)
        {
            var offset = isClockwise ? 1 : 3;
            var index = adjacentColors.ToList().FindIndex(color => color == faceToRotate.Center) + offset;

            faceToRotate.ChangeCenter(adjacentColors.ElementAt(index % 4));
        }

        private void PrintCubeFace(Color centerColor, bool withDetails = false)
        {
            Console.WriteLine($"{centerColor}:");

            PrintEdges(centerColor, withDetails);
            Console.WriteLine();

            PrintCorners(centerColor, withDetails);
            Console.WriteLine();
        }

        private void PrintEdges(Color centerColor, bool withDetails = false)
        {
            Console.Write("\tEdges:\t\t");

            foreach (var edge in _edges.Where(edge => edge.ContainsCenter(centerColor)))
            {
                var (centeredFace, invertedCenteredFace) = edge.GetFacesByCenter(centerColor);
                Console.Write($"{centeredFace.Color}{(withDetails ? $"({invertedCenteredFace.Center})\t" : "")}\t");
            }
        }

        private void PrintCorners(Color centerColor, bool withDetails = false)
        {
            Console.Write("\tCorners:\t");

            foreach (var corner in _corners.Where(corner => corner.ContainsCenter(centerColor)))
            {
                var (centeredFace, otherCenteredFace1, otherCenteredFace2) = corner.GetFacesByCenter(centerColor);
                Console.Write($"{centeredFace.Color}{(withDetails ? $"({otherCenteredFace1.Center}-{otherCenteredFace2.Center})" : "")}\t");
            }
        }

        private IEnumerable<Edge> GetEdges() =>
            new HashSet<Edge>
            {
                new Edge(Color.Blue, Color.White),
                new Edge(Color.Blue, Color.Orange),
                new Edge(Color.Blue, Color.Yellow),
                new Edge(Color.Blue, Color.Red),
                new Edge(Color.Green, Color.White),
                new Edge(Color.Green, Color.Orange),
                new Edge(Color.Green, Color.Yellow),
                new Edge(Color.Green, Color.Red),
                new Edge(Color.White, Color.Orange),
                new Edge(Color.White, Color.Red),
                new Edge(Color.Yellow, Color.Orange),
                new Edge(Color.Yellow, Color.Red),
            };

        private IEnumerable<Corner> GetCorners() =>
            new HashSet<Corner>
            {
                new Corner(Color.Blue, Color.White, Color.Orange),
                new Corner(Color.Blue, Color.White, Color.Red),
                new Corner(Color.Blue, Color.Yellow, Color.Orange),
                new Corner(Color.Blue, Color.Yellow, Color.Red),
                new Corner(Color.Green, Color.White, Color.Orange),
                new Corner(Color.Green, Color.White, Color.Red),
                new Corner(Color.Green, Color.Yellow, Color.Orange),
                new Corner(Color.Green, Color.Yellow, Color.Red),
            };

        private static Dictionary<Color, IEnumerable<Color>> GetColorMap() =>
            new Dictionary<Color, IEnumerable<Color>>
            {
                { Color.Red, new List<Color> { Color.White, Color.Blue, Color.Yellow, Color.Green } },
                { Color.Blue, new List<Color> { Color.White, Color.Orange, Color.Yellow, Color.Red } },
                { Color.Green, new List<Color> { Color.White, Color.Red, Color.Yellow, Color.Orange } },
                { Color.White, new List<Color> { Color.Blue, Color.Red, Color.Green, Color.Orange } },
                { Color.Yellow, new List<Color> { Color.Red, Color.Blue, Color.Orange, Color.Green } },
                { Color.Orange, new List<Color> { Color.White, Color.Green, Color.Yellow, Color.Blue } },
            };
    }
}
