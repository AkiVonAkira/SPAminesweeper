import React, { Component } from "react";
import authService from "../api-authorization/AuthorizeService";
import axios from "axios";
import styled from "styled-components";

const BoardContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 1em;
`;

const InfoContainer = styled.div`
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: center;
  padding: 1em;
  gap: 1em;
  color: var(--text);
  border: .25em var(--accent) solid;
  border-radius: 0.5em;
`;

const Board = styled.div`
  display: grid;
  grid-template-columns: repeat(var(--size), auto);
  grid-template-rows: repeat(var(--size), auto);
  gap: 0;
  border: min(2em, calc(2vw / var(--size))) var(--primary) solid;
`;

const TileButton = styled.button`
  width: 100%;
  height: 100%;
  margin: 0;
  padding: 0;
  border: none;
`;

const TimeDisplay = styled.p`
  background-color: var(--primary);
  color: var(--text);
  padding: 0.5em 1em;
  margin: 0;
  border: .25em var(--accent) solid;
  border-radius: 0.5em;
`;

const Tile = styled.div`
  width: min(4em, calc(40vw / var(--size)));
  height: min(4em, calc(40vw / var(--size)));
  /* padding-top: calc(100% / var(--size)); */
  font-size: min(4em, calc(20vw / var(--size)));
  aspect-ratio: 1/1;
  display: flex;
  align-items: center;
  justify-content: center;
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.4);

  /* Style for hidden tiles */
  &.hidden {
    /* border-top: min(2em, calc(2vw / var(--size))) #fefefe solid;
    border-left: min(2em, calc(2vw / var(--size))) #fefefe solid;
    border-bottom: min(2em, calc(2vw / var(--size))) #787976 solid;
    border-right: min(2em, calc(2vw / var(--size))) #787976 solid; */
    border: min(3em, calc(1.5vw / var(--size))) var(--primary) solid;
  }

  /* Style for revealed tiles */
  &.revealed {
    border: min(4em, calc(1vw / var(--size))) var(--primary) solid;
  }

  /* Style for mines */
  &.revealed.mine {
    background: #DD3E3E;
    border: min(3em, calc(1.5vw / var(--size))) #DD3E3E solid;
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
        const gameDuration = `${String(
          Math.floor(durationInSeconds / 60)
        ).padStart(2, "0")}:${String(durationInSeconds % 60).padStart(2, "0")}`;

        return {
          gameDuration
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
            <h2>
              {game.gameWon ? "Congratulations! You won!" : "Sorry, you lost."}
            </h2>
          )}

          {game.gameStarted && (
            <TimeDisplay>{this.state.gameDuration}</TimeDisplay>
          )}
          <TimeDisplay>500p</TimeDisplay>
        </InfoContainer>
        <Board style={{ "--size": boardSize }}>
          {game.tiles.map((tile, index) => (
            <Tile
              key={index}
              className={`tile ${tile.isRevealed ? "revealed" : "hidden"} ${tile.isMine && tile.isRevealed ? "mine" : ""
                }`}
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
                  onClick={(event) =>
                    this.handleTileClick(tile, game.id, event)
                  }
                  onContextMenu={(event) =>
                    this.handleFlagTile(tile, game.id, event)
                  }
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
    return this.renderGame();
  }
}
