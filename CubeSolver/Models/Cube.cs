using CubeSolver.Models.Components;
using CubeSolver.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CubeSolver.Models
{
    public class Cube
    {
        private readonly Color Red;
        private readonly Color Blue;
        private readonly Color Green;
        private readonly Color White;
        private readonly Color Yellow;
        private readonly Color Orange;

        private readonly List<Edge> Edges;
        private readonly List<Corner> Corners;

        /// <summary>
        /// List of adjacent colors for each color (in clockwise order)
        /// </summary>
        private readonly Dictionary<Color, List<Color>> AdjacentColorMap;

        public Cube()
        {
            Red = Color.Red;
            Blue = Color.Blue;
            Green = Color.Green;
            White = Color.White;
            Yellow = Color.Yellow;
            Orange = Color.Orange;
            Edges = GetEdges();
            Corners = GetCorners();
            AdjacentColorMap = GetColorMap();
        }

        public void Clockwise(Color color)
        {
            Rotate(color, true);
        }

        public void AntiClockwise(Color color)
        {
            Rotate(color, false);
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

                if (parity == 0)
                {
                    Clockwise(selectedColor);
                }
                else
                {
                    AntiClockwise(selectedColor);
                }

                Console.Write(selectedColor.ToString()[0] + (parity == 0 ? "" : "'") + " ");
            }

            Console.WriteLine("\n");
        }

        public void Print(bool withDetails = false)
        {
            var colors = Enum.GetValues(typeof(Color));

            foreach (Color color in colors)
            {
                PrintFace(color, withDetails);
            }
        }

        private void Rotate(Color color, bool isClockwise)
        {
            RotateEdges(color, isClockwise);
            RotateCorners(color, isClockwise);
        }

        private void RotateEdges(Color color, bool isClockwise)
        {
            var adjacent = AdjacentColorMap[color];
            var adjacentEdges = Edges.Where(edge => edge.Face1.Center == color || edge.Face2.Center == color);

            foreach (var edge in adjacentEdges)
            {
                if (edge.Face1.Center == color)
                {
                    RotateFace(adjacent, edge.Face2, isClockwise);
                }
                else
                {
                    RotateFace(adjacent, edge.Face1, isClockwise);
                }
            }
        }

        private void RotateCorners(Color color, bool isClockwise)
        {
            var adjacent = AdjacentColorMap[color];
            var adjacentCorners = Corners.Where(corner => corner.Face1.Center == color || corner.Face2.Center == color || corner.Face3.Center == color);

            foreach (var corner in adjacentCorners)
            {
                if (corner.Face1.Center == color)
                {
                    RotateFace(adjacent, corner.Face2, isClockwise);
                    RotateFace(adjacent, corner.Face3, isClockwise);
                }
                else if (corner.Face2.Center == color)
                {
                    RotateFace(adjacent, corner.Face1, isClockwise);
                    RotateFace(adjacent, corner.Face3, isClockwise);
                }
                else
                {
                    RotateFace(adjacent, corner.Face1, isClockwise);
                    RotateFace(adjacent, corner.Face2, isClockwise);
                }
            }
        }

        private void RotateFace(List<Color> adjacent, Face face, bool isClockwise)
        {
            var offset = isClockwise ? 1 : 3;
            var index = adjacent.FindIndex(color => color == face.Center) + offset;
            index %= 4;
            face.Center = adjacent[index];
        }

        private void PrintFace(Color color, bool withDetails = false)
        {
            Console.WriteLine($"{color}:");

            PrintEdges(color, withDetails);
            Console.WriteLine();

            PrintCorners(color, withDetails);
            Console.WriteLine();
        }

        private void PrintEdges(Color color, bool withDetails = false)
        {
            Console.Write("\tEdges:\t\t");

            Edges.Where(edge => edge.Face1.Center == color || edge.Face2.Center == color).ToList().ForEach(edge =>
            {
                var message = "";

                if (edge.Face1.Center == color)
                {
                    message += $"{edge.Face1.Color}{(withDetails ? $"({edge.Face2.Center})\t" : "")}";
                }
                else
                {
                    message += $"{edge.Face2.Color}{(withDetails ? $"({edge.Face1.Center})\t" : "")}";
                }

                Console.Write($"{message}\t");
            });
        }

        private void PrintCorners(Color color, bool withDetails = false)
        {
            Console.Write("\tCorners:\t");

            Corners.Where(corner => corner.Face1.Center == color || corner.Face2.Center == color || corner.Face3.Center == color).ToList().ForEach(corner =>
            {
                var message = "";

                if (corner.Face1.Center == color)
                {
                    message += $"{corner.Face1.Color}{(withDetails ? $"({corner.Face2.Center}-{corner.Face3.Center})" : "")}";
                }
                else if (corner.Face2.Center == color)
                {
                    message += $"{corner.Face2.Color}{(withDetails ? $"({corner.Face1.Center}-{corner.Face3.Center})" : "")}";
                }
                else
                {
                    message += $"{corner.Face3.Color}{(withDetails ? $"({corner.Face1.Center}-{corner.Face2.Center})" : "")}";
                }

                Console.Write($"{message}\t");
            });
        }

        private List<Edge> GetEdges()
        {
            return new List<Edge>
            {
                new Edge(Blue, White),
                new Edge(Blue, Orange),
                new Edge(Blue, Yellow),
                new Edge(Blue, Red),
                new Edge(Green, White),
                new Edge(Green, Orange),
                new Edge(Green, Yellow),
                new Edge(Green, Red),
                new Edge(Yellow, Orange),
                new Edge(Yellow, Red),
                new Edge(Red, White),
                new Edge(White, Orange),
            };
        }

        private List<Corner> GetCorners()
        {
            return new List<Corner>
            {
                new Corner(Blue, White, Orange),
                new Corner(Blue, White, Red),
                new Corner(Blue, Yellow, Orange),
                new Corner(Blue, Yellow, Red),
                new Corner(Green, White, Orange),
                new Corner(Green, White, Red),
                new Corner(Green, Yellow, Orange),
                new Corner(Green, Yellow, Red),
            };
        }

        private static Dictionary<Color, List<Color>> GetColorMap()
        {
            return new Dictionary<Color, List<Color>>
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
}
