using CubeSolver.Models;
using CubeSolver.Models.Enums;
using System;

namespace CubeSolver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var cube = new Cube();

            cube.Scramble();
            cube.Clockwise(Color.White);
            cube.Print();

            Console.ReadKey(true);
        }
    }
}
