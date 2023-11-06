﻿using SPAmineseweeper.Models;
using SPAmineseweeper.Models.ViewModels;

namespace SPAmineseweeper.Helper
{
    public class GameConverter
    {
        public static GameView ConvertGame(Game game)
        {
            var gameView = new GameView();

            gameView.Id = game.Id;
            gameView.GameStarted = game.GameStarted;
            gameView.GameEnded = game.GameEnded;
            gameView.Score = game.Score;
            gameView.BoardSize = game.BoardSize;
            gameView.BombPercentage = game.BombPercentage;
            gameView.Difficulty = game.Difficulty;

            var _tiles = new List<TileView>();
            foreach (var t in game.Tiles)
            {
                _tiles.Add(TileConverter.ConvertTiles(t));
            }
            gameView.Tiles = _tiles;

            return gameView;
        }
    }
}