import React, { Component } from "react";
import { StartGame } from "./StartGame";
import styled from "styled-components";

const GameContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  background-color: #c2c2c2;
  border-top: 0.25em #fefefe solid;
  border-left: 0.25em #fefefe solid;
  border-bottom: 0.25em #787976 solid;
  border-right: 0.25em #787976 solid;
  padding: 1em;
  gap: 1em;
  width: 100%;
  height: 100%;
`;

const CreateButton = styled.button`
  background-color: #c2c2c2;
  padding: 0.5em 1em;
  margin: 0;
  border-top: 0.25em #fefefe solid;
  border-left: 0.25em #fefefe solid;
  border-bottom: 0.25em #787976 solid;
  border-right: 0.25em #787976 solid;
  height: 3em;
`;

const DifficultyDropdown = styled.select`
  background-color: #c2c2c2;
  padding: 0.5em 1em;
  margin: 0;
  border-top: 0.25em #fefefe solid;
  border-left: 0.25em #fefefe solid;
  border-bottom: 0.25em #787976 solid;
  border-right: 0.25em #787976 solid;
  height: 3em;
`;

export class Home extends Component {
  static displayName = Home.name;

  constructor(props) {
    super(props);
    this.state = {
      bombPercentage: 5,
      difficulty: "easy",
      gridSize: 10,
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

  renderDifficultyDropdown() {
    return (
      <DifficultyDropdown
        aria-label="difficulty"
        value={this.state.difficulty}
        onChange={(e) => {
          const difficulty = e.target.value;
          this.setState({
            difficulty,
            showCustomSettings: difficulty === "custom", // Show inputs only if difficulty is "custom"
          });
        }}
      >
        <option value="easy">Easy</option>
        <option value="medium">Medium</option>
        <option value="hard">Hard</option>
        <option value="extreme">Extreme</option>
        <option value="custom">Custom</option>
      </DifficultyDropdown>
    );
  }

  renderCustomSettings() {
    return (
      <>
        <div>
          gridSize: <input
            aria-label="gridSize"
            value={this.state.gridSize}
            onChange={(e) => this.setState({ gridSize: e.target.value })}
          />
        </div>
        <div>
          bombPercentage: <input
            aria-label="bombPercentage"
            value={this.state.bombPercentage}
            onChange={(e) => this.setState({ bombPercentage: e.target.value })}
          />
        </div>
      </>
    );
  }

  render() {
    return (
      <GameContainer>
        {this.state.showCustomSettings && this.renderCustomSettings()}
        <div>
          {this.renderDifficultyDropdown()}
          <CreateButton onClick={this.handleStartGameClick}>Start Game</CreateButton>
        </div>
        <StartGame
          ref={(component) => (this.startGameComponent = component)}
          boardSize={this.state.gridSize}
          difficulty={this.state.difficulty}
          bombPercentage={this.state.bombPercentage}
        />
      </GameContainer>
    );
  }
}
