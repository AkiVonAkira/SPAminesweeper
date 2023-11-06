import React, { Component } from "react";
import authService from "./api-authorization/AuthorizeService";
import axios from "axios";
import styled from "styled-components";

const Board = styled.div`
  display: grid;
  grid-template-columns: repeat(var(--size), auto);
  grid-template-rows: repeat(var(--size), auto);
  gap: 0;
  min-width: 10em;
  min-height: 10em;
  border-top: 4px #787976 solid;
  border-left: 4px #787976 solid;
  border-bottom: 4px #fefefe solid;
  border-right: 4px #fefefe solid;
`;

const Tile = styled.div`
  width: 2em;
  height: 2em;
  aspect-ratio: 1/1;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 2em;
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.4);
  border: 2px #787976 solid;
`;

const TileButton = styled.button`
  width: 100%;
  height: 100%;
`;

export class StartGame extends Component {
  static displayName = StartGame.name;

  constructor(props) {
    super(props);
    this.state = { board: [], loading: true };
  }

  componentDidMount() {
    this.populateBoardData();
  }

  static renderGame(game) {
    return (
      <Board
        className="minesweeper-board"
        style={{ "--size": `${game.boardSize}` }}
      >
        {game.tiles.map((tile, index) => (
          <Tile
            key={index}
            className={`tile /*${tile.isRevealed ? "revealed" : "hidden"}*/`}
          >
            {tile.isRevealed ? (
              tile.isMine ? (
                <span className="mine">💣</span>
              ) : (
                tile.adjacentMines > 0 && <span>{tile.adjacentMines}</span>
              )
            ) : (
              <TileButton onClick={() => handleTileClick(tile, game.id)}>
                {tile.isFlagged ? "🚩" : ""}
              </TileButton>
            )}
          </Tile>
        ))}
      </Board>
    );
  }

  render() {
    let contents = this.state.loading ? (
      <p>
        <em>Loading...</em>
      </p>
    ) : (
      StartGame.renderGame(this.state.board)
    );

    return (
      <div>
        <h1 id="tableLabel">Minesweeper</h1>
        {contents}
      </div>
    );
  }

  async populateBoardData() {
    const token = await authService.getAccessToken();

    const config = {
      method: "post",
      url: "/api/game/startgame",
      headers: {
        "Content-Type": "application/json",
        Authorization: token ? `Bearer ${token}` : ""
      },
      data: {
        boardSize: 10,
        difficulty: "easy",
        bombPercentage: 5
      }
    };

    try {
      const response = await axios(config);

      if (response.status === 200) {
        this.setState({ board: response.data, loading: false });
        const boardId = response.data.id;
        await populateGameData(boardId);
      } else {
        console.error("Failed to fetch board data", response);
      }
    } catch (error) {
      console.error("Error fetching board data", error);
    }
  }
}

const handleTileClick = async (tile, gameId) => {
  const token = await authService.getAccessToken();

  const config = {
    method: "post",
    url: "/api/game/revealtile",
    headers: {
      "Content-Type": "application/json",
      Authorization: token ? `Bearer ${token}` : "",
    },
    data: {
      GameId: gameId,
      X: tile.X,
      Y: tile.Y
    },
  };

  try {
    const response = await axios(config);

    if (response.status === 200) {
      this.setState({ board: response.data });
    } else {
      console.error("Failed to reveal tile", response);
    }
  } catch (error) {
    console.error("Error revealing tile", error);
  }
}

const populateGameData = async (boardId) => {
  const token = await authService.getAccessToken();

  const config = {
    method: "post",
    url: "/api/game/startgame",
    headers: {
      "Content-Type": "application/json",
      Authorization: token ? `Bearer ${token}` : ""
    },
    data: {
      Score: 0,
      BoardId: boardId
    }
  };

  try {
    const response = await axios(config);

    if (response.status === 200) {
      this.setState({ game: response.data, loading: false });
    } else {
      console.error("Failed to fetch game data", response);
    }
  } catch (error) {
    console.error("Error fetching game data", error);
  }
};
