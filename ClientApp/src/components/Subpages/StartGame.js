import React, { Component } from "react";
import authService from "../api-authorization/AuthorizeService";
import axios from "axios";
import styled from "styled-components";

const BoardContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  /* background-color: #c2c2c2;
  border-top: 0.25em #fefefe solid;
  border-left: 0.25em #fefefe solid;
  border-bottom: 0.25em #787976 solid;
  border-right: 0.25em #787976 solid; */
  padding: 1em;
  gap: 1em;
`;

const InfoContainer = styled.div`
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: center;
  background-color: #c2c2c2;
  padding: 1em;
  gap: 1em;
`;

const Board = styled.div`
  display: grid;
  grid-template-columns: repeat(var(--size), auto);
  grid-template-rows: repeat(var(--size), auto);
  gap: 0;
  max-width: 100vw;
  border-top: .25em #787976 solid;
  border-left: .25em #787976 solid;
  border-bottom: .25em #fefefe solid;
  border-right: .25em #fefefe solid;
`;

const Tile = styled.div`
  width: clamp(.01rem, 6vw, 4rem);
  height: clamp(.01rem, 6vw, 4rem);
  font-size: clamp(.01rem, 2.5vw, 2.5rem);
  aspect-ratio: 1/1;
  display: flex;
  align-items: center;
  justify-content: center;
  max-width: 100%;
  /* text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.4); */

  /* Style for hidden tiles */
  &.hidden {
    border-top: .25rem #fefefe solid;
    border-left: .25rem #fefefe solid;
    border-bottom: .25rem #787976 solid;
    border-right: .25rem #787976 solid;
  }

  /* Style for revealed tiles */
  &.revealed {
    border: .1rem #787976 solid;
  }

  /* Style for mines */
  &.revealed.mine {
    background: red;
    border: .2rem #787976 solid;
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

const TimeDisplay = styled.p`
  background-color: #c2c2c2;
  color: red;
  padding: 0.5em 1em;
  margin: 0;
  border-top: 0.25em #fefefe solid;
  border-left: 0.25em #fefefe solid;
  border-bottom: 0.25em #787976 solid;
  border-right: 0.25em #787976 solid;
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
        const gameDuration = `${String(Math.floor(durationInSeconds / 60)).padStart(2, '0')}:${String(durationInSeconds % 60).padStart(2, '0')}`;

        return {
          gameDuration,
        };
      }

      return null;
    });
  }

  async startNewGame(onGameStarted) {
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
        boardSize: this.props.boardSize,
        difficulty: this.props.difficulty,
        bombPercentage: this.props.bombPercentage
      }
    };

    try {
      const response = await axios(config);
      if (response.status >= 200 && response.status < 300) {
        this.setState({ game: response.data, loading: false });
        if (onGameStarted) {
          onGameStarted(response.data);
        }
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
      <BoardContainer>
        <InfoContainer>
          {isGameEnded && (
            <h2>{game.gameWon ? "Congratulations! You won!" : "Sorry, you lost."}</h2>
          )}

          {game.gameStarted && <TimeDisplay>{this.state.gameDuration}</TimeDisplay>}
        </InfoContainer>
        <Board style={{ "--size": boardSize }}>
          {game.tiles.map((tile, index) => (
            <Tile
              key={index}
              className={`tile ${tile.isRevealed ? "revealed" : "hidden"} ${tile.isMine && tile.isRevealed ? "mine" : ""}`}
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
      </BoardContainer>
    );
  }

  render() {
    return (
      this.renderGame()
    );
  }
}
