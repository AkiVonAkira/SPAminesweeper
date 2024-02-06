import React, { Component } from "react";
import { StartGame } from "../Subpages/StartGame";
import Chathub from "../Subpages/Chat";
import styled from "styled-components";
import { Button, Input } from '../Global/GlobalStyles';

const BoxContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  border: 0.25em var(--accent) solid;
  border-radius: 0.5em;
  padding: 1em;
  gap: 1em;
  max-width: 80%;
`;

const GameContainer = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: center;
  gap: 1em;
  min-height: 100%;
  flex-wrap: wrap;
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

const DifficultyGrid = styled.div`
  display: flex;
  padding: 0.5em 1em;
  flex-wrap: wrap;
  gap: 0.5em;
`

const CustomContainer = styled.div`
  display: flex;
  flex-direction: column;
  padding: 0.5em 1em;
  flex-wrap: wrap;
  gap: 0.5em;

  & [Input] {
    max-width: 50%;
  }
`

export class Home extends Component {
  static displayName = Home.name;

  constructor(props) {
    super(props);
    this.state = {
      bombPercentage: null,
      difficulty: "easy",
      gridSize: null,
      showCustomSettings: false,
      game: null
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
            difficulty
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
              difficulty: "custom"
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
        <BoxContainer>
          <Chathub />
        </BoxContainer>
        <BoxContainer>
          <StartGame
            ref={(component) => (this.startGameComponent = component)}
            boardSize={this.state.gridSize || 10}
            difficulty={this.state.difficulty || 'easy'}
            bombPercentage={this.state.bombPercentage || 5}
          />
        </BoxContainer>
        <BoxContainer>
          <DifficultyContainer>
            {<h2>Difficulty</h2>}
            {this.renderDifficultyGrid()}
            <div>
              <Switch
                type="checkbox"
                checked={this.state.showCustomSettings}
                onChange={() =>
                  this.setState((prevState) => ({
                    showCustomSettings: !prevState.showCustomSettings
                  }))
                }
                color="primary"
              />
              <span>Show Custom Settings</span>
            </div>
            {this.state.showCustomSettings && this.renderCustomSettings()}
          </DifficultyContainer>
        </BoxContainer>
      </GameContainer>
    );
  }
}
