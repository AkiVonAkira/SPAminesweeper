import React, { Component } from "react";
import { StartGame } from "../Subpages/StartGame";
import Chathub from "../Subpages/Chat";
import styled from "styled-components";

const GameContainer = styled.div`
display: flex;
flex-direction: column;
align-items: center;
justify-content: center;
background-color: #c2c2c2;
border: 0.25em #787976 solid;
border-radius: 0.5em;
padding: 1em;
gap: 1em;
max-width: 100vw;
`;

const TextField = styled.input`
display: flex;
align-items: center;
justify-content: center;
background-color: #c2c2c2;
border: 0.25em #787976 solid;
border-radius: 0.5em;
padding: 1em;
`;

const CreateButton = styled.button`
background-color: #c2c2c2;
padding: 0.5em 1em;
margin: 0;
border: 0.25em red solid;
border-radius: 0.5em;
height: 3em;
`;
const Switch = styled.input`
background-color: #c2c2c2;
padding: 0.5em 1em;
margin: 0;
border: 0.25em red solid;
border-radius: 0.5em;
height: 1.25em;
width: 1.25em;
`;

const DifficultyList = styled.ul`
  background-color: #c2c2c2;
  padding: 0.5em 1em;
  margin: 0;
  flex-wrap: nowrap;
  list-style: none;
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

  renderDifficultyList() {
    return (
      <DifficultyList
        aria-label="difficulty"
        value={this.state.difficulty}
        onClick={(e) => {
          const difficulty = e.target.value;
          this.setState({
            difficulty
          });
        }}
      >
        <CreateButton value="easy" onClick={this.handleStartGameClick}>Easy</CreateButton>
        <CreateButton value="medium" onClick={this.handleStartGameClick}>Medium</CreateButton>
        <CreateButton value="hard" onClick={this.handleStartGameClick}>Hard</CreateButton>
        <CreateButton value="extreme" onClick={this.handleStartGameClick}>Extreme</CreateButton>
      </DifficultyList>
    );
  }

  renderCustomSettings() {
    return (
      <>
        <TextField
          label="Grid Size"
          type="number"
          value={this.state.gridSize}
          onChange={(e) => this.setState({ gridSize: e.target.value })}
        />
        <TextField
          label="Bomb Percentage"
          type="number"
          value={this.state.bombPercentage}
          onChange={(e) => this.setState({ bombPercentage: e.target.value })}
        />
        <CreateButton
          onClick={(e) => {
            this.handleStartGameClick();
            this.setState({
              difficulty: "custom",
            });
          }}
        >
          Start Custom Game
        </CreateButton>
      </>
    );
  }

  render() {
    return (
      <GameContainer>
        <div>
          {<h1>Select Difficulty:</h1>}
          {this.renderDifficultyList()}
          <span>Show Custom Settings</span>
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
          <div>
            {this.state.showCustomSettings && this.renderCustomSettings()}
          </div>
        </div>
        <Chathub />
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

