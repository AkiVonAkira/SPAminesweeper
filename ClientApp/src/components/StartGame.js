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
  border-top: .25em #787976 solid;
  border-left: .25em #787976 solid;
  border-bottom: .25em #fefefe solid;
  border-right: .25em #fefefe solid;
`;

const Tile = styled.div`
  width: 2em;
  height: 2em;
  aspect-ratio: 1/1;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 2em;
  /* text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.4); */

  /* Style for hidden tiles */
  &.hidden {
    border-top: .1em #fefefe solid;
    border-left: .1em #fefefe solid;
    border-bottom: .1em #787976 solid;
    border-right: .1em #787976 solid;
  }

  /* Style for revealed tiles */
  &.revealed {
    border: 1px #787976 solid;
  }

  /* Style for mines */
  &.mine {
    background-color: red;
  }

  /* Style for adjecent mine numbers */
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
  background-color: #c2c2c2;
  margin: 0;
  padding: 0;
  border: 0;
`;

export class StartGame extends Component {
  static displayName = StartGame.name;

  constructor(props) {
    super(props);
    this.state = { game: null, loading: false };
  }

  async componentDidMount() {
    await this.startNewGame();

    this.intervalId = setInterval(() => {
      this.updateGameDuration();
    }, 1000);
  }

  componentWillUnmount() {
    clearInterval(this.intervalId);
  }

  updateGameDuration() {
    this.setState((prevState) => {
      if (prevState.game && prevState.game.gameStarted) {
        const gameStarted = new Date(prevState.game.gameStarted);
        const gameEnded = prevState.game.gameEnded
          ? new Date(prevState.game.gameEnded)
          : new Date();

        const durationInSeconds = Math.floor((gameEnded - gameStarted) / 1000);
        const gameDuration = `${durationInSeconds % 60}`;

        return {
          gameDuration,
        };
      }

      return null;
    });
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
      if (response.status >= 200 && response.status < 300) {
        this.setState({ game: response.data, loading: false });
      } else {
        console.error("Failed to start a new game", response);
      }
    } catch (error) {
      console.error("Error starting a new game", error);
    } finally {
      this.setState({ loading: false });
    }
  }

  async handleTileClick(tile, gameId, event) {
    event.preventDefault();
    if (event.button === 0) {
      this.setState({ loading: true });

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
        if (response.status >= 200 && response.status < 300) {
          this.setState({ game: response.data, loading: false });
        } else {
          console.error("Failed to reveal tile", response);
        }
      } catch (error) {
        console.error("Error revealing tile", error);
      } finally {
        this.setState({ loading: false });
      }
    }
  }

  async handleFlagTile(tile, gameId, event) {
    event.preventDefault();
    if (event.button === 2) {
      this.setState({ loading: true });

      const token = await authService.getAccessToken();
      const config = {
        method: "post",
        url: "/api/tile/flagtile",
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
        if (response.status >= 200 && response.status < 300) {
          this.setState({ game: response.data, loading: false });
        } else {
          console.error("Failed to reveal tile", response);
        }
      } catch (error) {
        console.error("Error revealing tile", error);
      } finally {
        this.setState({ loading: false });
      }
    }
  }

  renderGame() {
    const { game } = this.state;

    if (!game) {
      return <p>Loading...</p>;
    }

    const boardSize = game.boardSize;
    const isGameEnded = game.gameEnded !== null;

    return (
      <div>
        {isGameEnded && (
          <div>
            <h2>{game.gameWon ? "Congratulations! You won!" : "Sorry, you lost."}</h2>
          </div>
        )}

        {game.gameStarted && <p>Duration: {this.state.gameDuration}s</p>}
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
                    <span adjacentMines={tile.adjacentMines}>
                      {tile.adjacentMines}
                    </span>
                  )
                )
              ) : (
                <TileButton
                  onClick={(event) => this.handleTileClick(tile, game.id, event)}
                  onContextMenu={(event) => this.handleFlagTile(tile, game.id, event)}
                  disabled={this.state.loading}
                >
                  {tile.isFlagged ? "🚩" : ""}
                </TileButton>
              )}
            </Tile>
          ))}
        </Board>
      </div>
    );
  }

  render() {
    return (
      <div>
        {this.renderGame()}
      </div>
    );
  }
}
