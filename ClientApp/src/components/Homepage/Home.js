import React, { Component } from "react";
import { StartGame } from "../Subpages/StartGame";
import Chathub from "../Subpages/Chat";
import styled from "styled-components";
import { Button, Input } from "../Global/GlobalStyles";

const GameContainer = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: center;
  gap: 1em;
  //flex-wrap: wrap;
`;

const BoxContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  border: 0.25em var(--accent) solid;
  border-radius: 0.5em;
  padding: 1em;
  gap: 1em;
  max-width: 100%;
  flex-grow: 1;

  word-wrap: normal;


  @media screen and (min-width: 1300px)
  {
    flex-direction: row;
  }
  @media screen and (max-width: 720px)
  {
    flex-direction: row;
  }
`;

const ChatBoxContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  border: 0.25em var(--accent) solid;
  border-radius: 0.5em;
  padding: 1em;
  gap: 1em;
  flex-grow: 1;
  max-width: 30vw;
  height: fit-content;

  @media screen and (max-width: 720px)
  {
    max-width: 100%;
  }

`;

const DifficultyGrid = styled.div`
  display: flex;
  padding: 0.5em 1em;
  flex-wrap: wrap;
  gap: 0.5em;
`;

const CustomContainer = styled.div`
  display: flex;
  flex-direction: column;
  padding: 0.5em 1em;
  flex-wrap: wrap;
  gap: 0.5em;

  & [Input] {
    max-width: 50%;
  }
`;

export class Home extends Component {
  static displayName = Home.name;

  constructor(props) {
    super(props);
    this.state = {
      bombPercentage: null,
      difficulty: "easy",
      gridSize: null,
      showCustomSettings: false,
      game: null,
    };
  }

  handleStartGameClick = async () => {
    this.startGameComponent.startNewGame(this.handleGameStarted);
  };

  handleGameStarted = (game) => {
    this.setState({ game });
  };

  renderDifficultyGrid() {
    return (
      <DifficultyGrid
        aria-label="difficulty"
        value={this.state.difficulty}
        onClick={(e) => {
          const difficulty = e.target.value;
          this.setState({
            difficulty,
          });
        }}
      >
        <Button value="easy" onClick={this.handleStartGameClick}>
          Easy
        </Button>
        <Button value="medium" onClick={this.handleStartGameClick}>
          Medium
        </Button>
        <Button value="hard" onClick={this.handleStartGameClick}>
          Hard
        </Button>
        <Button value="extreme" onClick={this.handleStartGameClick}>
          Extreme
        </Button>
      </DifficultyGrid>
    );
  }

  renderCustomSettings() {
    return (
      <CustomContainer>
        <Input
          label="Grid Size"
          placeholder="Grid Size"
          type="number"
          value={this.state.gridSize}
          onChange={(e) => this.setState({ gridSize: e.target.value })}
        />
        <Input
          label="Bomb Percentage"
          placeholder="Bomb Percentage"
          type="number"
          value={this.state.bombPercentage}
          onChange={(e) => this.setState({ bombPercentage: e.target.value })}
        />
        <Button
          onClick={(e) => {
            this.handleStartGameClick();
            this.setState({
              difficulty: "custom",
            });
          }}
        >
          Start Custom Game
        </Button>
      </CustomContainer>
    );
  }

  render() {
    return (
      <GameContainer>
        <ChatBoxContainer>
          <Chathub />
        </ChatBoxContainer>
        <BoxContainer>
          <StartGame
            ref={(component) => (this.startGameComponent = component)}
            boardSize={this.state.gridSize || 10}
            difficulty={this.state.difficulty || "easy"}
            bombPercentage={this.state.bombPercentage || 5}
          />
        </BoxContainer>
      </GameContainer>
    );
  }
}
