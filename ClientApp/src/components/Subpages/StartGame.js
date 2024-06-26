﻿import React, { Component } from "react";
import authService from "../api-authorization/AuthorizeService";
import axios from "axios";
import styled from "styled-components";
import Leaderboard from "../Subpages/LeaderBoard";
import Profile from "../Subpages/Profile";

const BoardContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: flex-start;
  gap: 1em;
  flex-grow: 1;
  max-width: 100%;
  width: fit-content;
  padding: 2em;
`;

const InfoContainer = styled.div`
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: center;
  padding: 1em;
  gap: 1em;
  color: var(--text);
  border: .2em var(--accent) solid;
  border-radius: 0.5em;
  width: 100%;
  margin-bottom: 1em;
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
  background-color: var(--secondary);
  color: var(--text);
  padding: 0.5em 1em;
  margin: 0;
  border: .2em var(--accent) solid;
  border-radius: 0.5em;
`;

const Tile = styled.div`
  width: min(2em, calc(30vw / var(--size)));
  height: min(2em, calc(30vw / var(--size)));
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
    border: min(1em, calc(1vw / var(--size))) var(--primary) solid;
  }

  /* Style for revealed tiles */
  &.revealed {
    border: min(1em, calc(1vw / var(--size))) var(--primary) solid;
  }

  /* Style for mines */
  &.revealed.mine {
    background: #DD3E3E;
    border: min(1em, calc(1vw / var(--size))) var(--primary) solid;
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

const ShrinkBoxContainer = styled.div`
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  //border: 0.25em var(--accent) solid;
  border-radius: 0.5em;
  padding: 1em;
  gap: 1em;
  flex-grow: 1;
  flex-wrap: wrap;
  height: 100%;
`;

const DifficultyContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1em;
`;

const Switch = styled.input`
  background-color: #c2c2c2;
  padding: 0.5em 1em;
  margin: 0;
  border: 0.25em red solid;
  border-radius: 0.5em;
  height: 1.25em;
  width: 1.25em;
  margin-right: 1em;
`;

export class StartGame extends Component {
  static displayName = StartGame.name;

  constructor(props) {
    super(props);
    this.state = { game: null, loading: false, authorized: false };
  }

  async componentDidMount() {
    await this.startNewGame();

    this.intervalId = setInterval(() => {
      this.updateGameDuration();
      this.updateScore(this.state.game?.id);
    }, 1000);
  }

  componentWillUnmount() {
    clearInterval(this.intervalId);
  }

  updateGameDuration() {
    this.setState((prevState) => {
      if (prevState.game && prevState.game.gameStarted != null) {
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

    try {
      const token = await authService.getAccessToken();
      const response = await axios.post("/api/game/startgame", {
        boardSize: this.props.boardSize,
        difficulty: this.props.difficulty,
        bombPercentage: this.props.bombPercentage
      }, {
        headers: {
          "Content-Type": "application/json",
          Authorization: token ? `Bearer ${token}` : ""
        }
      });

      if (response.status >= 200 && response.status < 300) {
        this.setState({ game: response.data, loading: false, authorized: true });
        if (onGameStarted) {
          onGameStarted(response.data);
        }
      } else if (response.status > 400 && response.status < 500) {
        this.setState({ loading: true });
      } else {
        console.error("Failed to start a new game", response);
      }
    } catch (error) {
      console.error("Error starting a new game", error);
    } finally {
      this.setState({ loading: false, authorized: true });
    }
  }

  async handleTileClick(tile, gameId, event) {
    event.preventDefault();
    if (event.button === 0) {
      this.setState({ loading: true });

      try {
        const token = await authService.getAccessToken();
        const response = await axios.post("/api/tile/revealtile", {
          GameId: gameId,
          X: tile.x,
          Y: tile.y
        }, {
          headers: {
            "Content-Type": "application/json",
            Authorization: token ? `Bearer ${token}` : ""
          }
        });

        if (response.status >= 200 && response.status < 300) {
          this.setState({ game: response.data, loading: false, authorized: true });
        } else if (response.status > 400 && response.status < 500) {
          this.setState({ loading: true });
        } else {
          console.error("Failed to reveal tile", response);
        }
      } catch (error) {
        console.error("Error revealing tile", error);
      } finally {
        this.updateScore(gameId);
        this.setState({ loading: false, authorized: true });
      }
    }
  }

  async updateScore(gameId) {
    if (!this.state.game) { return; }
    if (this.state.game.gameStarted && !this.state.game.gameEnded) {
      this.setState({ loading: true });

      try {
        const token = await authService.getAccessToken();
        const config = {
          method: "post",
          url: "/api/score/addscore",
          headers: {
            "Content-Type": "application/json",
            Authorization: token ? `Bearer ${token}` : ""
          },
          data: {
            GameId: gameId,
          }
        };

        const response = await axios(config);
        if (response.status >= 200 && response.status < 300) {
          // Update only the score data
          const updatedScore = response.data.score.highScore;
          this.setState((prevState) => ({
            game: {
              ...prevState.game,
              score: {
                ...prevState.game.score,
                highScore: updatedScore
              }
            },
            loading: false,
            authorized: true
          }));
        } else {
          console.error("Failed to update score", response);
        }

      } catch (error) {
        console.error("Error updating score", error);
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
      return <ShrinkBoxContainer>
        <h1>Welcome to MineSweeperSPA!</h1>

        <p>
          MineSweeperSPA is a dynamic single-page web application designed to boost
          your gaming experience.
          <br />

          Along with user registration and
          login features, we offer a even: a global chat that lets you
          connect with fellow players in real time.
        </p>

        <h3>Key Features:</h3>

        <p>
          <strong>User Registration and Login: </strong>Create your MineSweeper
          account or log in to keep track of your progress, save your
          scores, and compete with other players on the global leaderboard.
        </p>

        <p>
          <strong>Global Leaderboard: </strong> You can even check out the leaderboard on
          the homepage to see how you stack up against the top 5 players
          worldwide.
        </p>

        <p>
          <strong>Global Chat: </strong>Engage with other Minesweeper players in
          the global chatroom. Enjoy a
          friendly conversation as you play the game.
        </p>

        <Leaderboard />
      </ShrinkBoxContainer>;
    }

    if (game.authorized) {
      return <h1>Please Sign out and Sign back in.</h1>
    }


    const boardSize = game.boardSize;
    const isGameEnded = game.gameEnded !== null;

    return (
      <div>
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
            <TimeDisplay>{game.score.highScore}</TimeDisplay>
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
        <ShrinkBoxContainer>
          <Leaderboard />
          <Profile />
          <DifficultyContainer>
            {<h2>Difficulty</h2>}
            {this.renderDifficultyGrid()}
            <div>
              <Switch
                type="checkbox"
                checked={this.state.showCustomSettings}
                onChange={() =>
                  this.setState((prevState) => ({
                    showCustomSettings: !prevState.showCustomSettings,
                  }))
                }
                color="primary"
              />
              <span>Show Custom Settings</span>
            </div>
            {this.state.showCustomSettings && this.renderCustomSettings()}
          </DifficultyContainer>
        </ShrinkBoxContainer>
      </div>
    );
  }

  render() {
    return this.renderGame();
  }
}
