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

  /* Style for hidden tiles */
  &.hidden {
    border-top: 4px #787976 solid;
    border-left: 4px #787976 solid;
    border-bottom: 4px #fefefe solid;
    border-right: 4px #fefefe solid;
  }

  /* Style for revealed tiles */
  &.revealed {
    border: 2px #787976 solid;
  }
`;

const NumberedTile = styled.span`
  & [adjacentMines="1"] {
    color: blue;
  }

  & [adjacentMines="2"] {
    color: green;
  }

  & [adjacentMines="3"] {
    color: red;
  }

  & [adjacentMines="4"] {
    color: purple;
  }

  & [adjacentMines="5"] {
    color: maroon;
  }

  & [adjacentMines="6"] {
    color: turquoise;
  }

  & [adjacentMines="7"] {
    color: black;
  }

  & [adjacentMines="8"] {
    color: gray;
  }
`;

const TileButton = styled.button`
  width: 100%;
  height: 100%;
`;

export class StartGame extends Component {
  static displayName = StartGame.name;

  constructor(props) {
    super(props);
    this.state = { game: null, loading: false };
  }

  async componentDidMount() {
    await this.startNewGame();
  }

  async startNewGame() {
    this.setState({ loading: true });

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
      if (response.status === 201) {
        this.setState({ game: response.data, loading: false });
      } else {
        console.error("Failed to start a new game", response);
      }
    } catch (error) {
      console.error("Error starting a new game", error);
    }
  }

  async handleTileClick(tile, gameId) {
    const token = await authService.getAccessToken();
    const config = {
      method: "post",
      url: "/api/tile/revealtile",
      headers: {
        "Content-Type": "application/json",
        Authorization: token ? `Bearer ${token}` : ""
      },
      data: {
        GameId: gameId,
        X: tile.x,
        Y: tile.y
      }
    };

    try {
      const response = await axios(config);
      if (response.status === 200) {
        this.setState({ game: response.data });
      } else {
        console.error("Failed to reveal tile", response);
      }
    } catch (error) {
      console.error("Error revealing tile", error);
    }
  }

  renderGame() {
    const { game } = this.state;

    if (!game) {
      return <p>Loading...</p>;
    }

    const boardSize = game.boardSize;

    return (
      <Board style={{ "--size": boardSize }}>
        {game.tiles.map((tile, index) => (
          <Tile
            key={index}
            className={`tile ${tile.isRevealed ? "revealed" : "hidden"}`}
          >
            {tile.isRevealed ? (
              tile.isMine ? (
                <span className="mine">💣</span>
              ) : (
                tile.adjacentMines > 0 && (
                  <NumberedTile adjacentMines={tile.adjacentMines}>
                    {tile.adjacentMines}
                  </NumberedTile>
                )
              )
            ) : (
              <TileButton
                onClick={() => this.handleTileClick(tile, game.id)}
                disabled={this.state.loading}
              >
                {tile.isFlagged ? "🚩" : ""}
              </TileButton>
            )}
          </Tile>
        ))}
      </Board>
    );
  }

  render() {
    return (
      <div>
        <h1 id="tableLabel">Minesweeper</h1>
        {this.renderGame()}
      </div>
    );
  }
}
