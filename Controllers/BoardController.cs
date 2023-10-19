using Microsoft.AspNetCore.Mvc;
using SPAmineseweeper.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPAmineseweeper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoardController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateBoard(int boardSize, int bombPercentage)
        {
            var board = new Board
            {
                Tiles = new List<Tile>()
            };
            var minePositions = GetMinePositions(boardSize, bombPercentage);

            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize; y++)
                {
                    var tile = new Tile
                    {
                        BoardId = board.Id,
                        X = x,
                        Y = y,
                        IsMine = minePositions.Any(position => PositionMatch(position, x, y)),
                        IsRevealed = false,
                        IsFlagged = false,
                        AdjacentMines = 0 
                    };

                    board.Tiles.Add(tile); 
                }
            }

            return Ok(board); 
        }

        // Hjälpmetod för att generera slumpmässiga minpositioner
        private List<(int, int)> GetMinePositions(int boardSize, int bombPercentage)
        {
             var random = new Random();
             var minePositions = new List<(int, int)>();
             for (int i = 0; i < bombPercentage; i++)
             {
                var x = random.Next(boardSize);
                var y = random.Next(boardSize);
                minePositions.Add((x, y));
             }
               return minePositions;
        }

        // Hjälpmetod för att matcha positioner
        private bool PositionMatch((int, int) position, int x, int y)
        {
            return position.Item1 == x && position.Item2 == y;
        }
    }
}

